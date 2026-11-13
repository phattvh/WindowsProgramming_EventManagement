using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.GUI.Common;
using EventManagement.Core.Session;
using ZXing;
using ZXing.Windows.Compatibility;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình F13 - Soát vé & Điểm danh sự kiện qua Webcam QR
    /// Phụ trách: Thành viên 3 (Task T3-19 & T3-20 / Day 7)
    /// Nghiệp vụ:
    /// - BR-11: Thẩm định vé (Tồn tại, đúng sự kiện, đã thanh toán, không bị hủy)
    /// - BR-12: Điểm danh một chiều, phát hiện và cảnh báo quét trùng lặp tức thì
    /// </summary>
    public partial class FrmCheckInCamera : FrmBase
    {
        private readonly ICheckInService _checkInService;
        private FilterInfoCollection? _videoDevices;
        private VideoCaptureDevice? _videoSource;
        private readonly BarcodeReader _barcodeReader;

        private bool _isScanning = false;
        private DateTime _lastScanTime = DateTime.MinValue;
        private string _lastScannedCode = string.Empty;
        private List<(string MaSuKien, string TenSuKien)> _events = new();

        public ComboBox CboSuKien => cboSuKien;
        public ComboBox CboCamera => cboCamera;
        public Button BtnBatCamera => btnBatCamera;
        public Button BtnTatCamera => btnTatCamera;
        public Button BtnCheckInManual => btnCheckInManual;
        public DataGridView DgvRecentCheckIns => dgvRecentCheckIns;
        public Label LblSubTitle => lblSubTitle;

        private Button? _btnUploadQrImage;

        public FrmCheckInCamera() : this(new CheckInService())
        {
        }

        public FrmCheckInCamera(ICheckInService checkInService)
        {
            InitializeComponent();
            _checkInService = checkInService ?? new CheckInService();

            _barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new ZXing.Common.DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat>
                    {
                        BarcodeFormat.QR_CODE,
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.CODE_39
                    }
                }
            };

            AddUploadQrButton();
            ApplyCustomStyles();
            WireEvents();
        }

        private void AddUploadQrButton()
        {
            try
            {
                _btnUploadQrImage = new Button
                {
                    Name = "btnUploadQrImage",
                    Text = "📁 Chọn ảnh QR",
                    Size = new Size(126, 32),
                    Location = new Point(300, 15),
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                    Cursor = Cursors.Hand,
                    UseVisualStyleBackColor = true
                };
                StyleButton(_btnUploadQrImage, ButtonRole.Secondary);
                _btnUploadQrImage.Click += BtnUploadQrImage_Click;
                grpManual.Controls.Add(_btnUploadQrImage);
            }
            catch { }
        }

        private void BtnUploadQrImage_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Chọn file ảnh mã QR để soát vé",
                Filter = "File ảnh QR (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|Tất cả tệp (*.*)|*.*"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    using var bmp = new Bitmap(ofd.FileName);
                    var decodeResult = _barcodeReader.Decode(bmp);
                    if (decodeResult != null && !string.IsNullOrWhiteSpace(decodeResult.Text))
                    {
                        string scannedText = decodeResult.Text.Trim();
                        txtMaVeManual.Text = scannedText;
                        ExecuteCheckIn(scannedText, "File ảnh QR");
                    }
                    else
                    {
                        MessageBox.Show("Không thể giải mã QR từ ảnh đã chọn. Vui lòng thử ảnh rõ nét hơn hoặc nhập mã thủ công.", "Không nhận dạng được QR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi đọc file ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ApplyCustomStyles()
        {
            StyleButton(btnBatCamera, ButtonRole.Success);
            StyleButton(btnTatCamera, ButtonRole.Danger);
            StyleButton(btnCheckInManual, ButtonRole.Primary);
            StyleButton(btnResetDemo, ButtonRole.Secondary);
            StyleButton(btnDong, ButtonRole.Secondary);

            StyleDataGridView(dgvRecentCheckIns);
            dgvRecentCheckIns.EnableHeadersVisualStyles = false;
            dgvRecentCheckIns.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRecentCheckIns.AutoGenerateColumns = false;

            foreach (DataGridViewColumn col in dgvRecentCheckIns.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            colThoiGian.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colMaVe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHangVe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colPhuongThuc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colKetQua.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvRecentCheckIns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            colThoiGian.FillWeight = 16;
            colMaVe.FillWeight = 12;
            colTenNguoi.FillWeight = 26;
            colHangVe.FillWeight = 18;
            colPhuongThuc.FillWeight = 14;
            colKetQua.FillWeight = 14;

            this.Resize += (s, e) =>
            {
                if (scMain != null && scMain.Width > 800)
                {
                    scMain.SplitterDistance = Math.Max(460, (int)(scMain.Width * 0.40));
                }
            };
            if (scMain != null && scMain.Width > 800)
            {
                scMain.SplitterDistance = Math.Max(460, (int)(scMain.Width * 0.40));
            }

            LoadEvents();
            LoadCameraDevices();
            RefreshStatistics();
            RefreshRecentLogs();
            ResetStatusBanner();
        }

        private void WireEvents()
        {
            cboSuKien.SelectedIndexChanged += CboSuKien_SelectedIndexChanged;
            btnBatCamera.Click += BtnBatCamera_Click;
            btnTatCamera.Click += BtnTatCamera_Click;
            btnCheckInManual.Click += BtnCheckInManual_Click;
            btnResetDemo.Click += BtnResetDemo_Click;
            btnDong.Click += (s, e) => Close();

            txtMaVeManual.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    btnCheckInManual.PerformClick();
                }
            };

            FormClosing += FrmCheckInCamera_FormClosing;
        }

        private void LoadEvents()
        {
            try
            {
                var rawEvents = _checkInService.GetActiveEvents();
                var eventItems = rawEvents.Select(e => new EventLookupItem(e.MaSuKien, e.TenSuKien)).ToList();
                cboSuKien.DataSource = null;
                cboSuKien.DisplayMember = nameof(EventLookupItem.TenSuKien);
                cboSuKien.ValueMember = nameof(EventLookupItem.MaSuKien);
                cboSuKien.DataSource = eventItems;

                if (eventItems.Count > 0)
                {
                    cboSuKien.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sự kiện: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSelectedEventCode()
        {
            if (cboSuKien.SelectedItem is EventLookupItem item)
            {
                return item.MaSuKien;
            }
            if (cboSuKien.SelectedValue is string code && !string.IsNullOrEmpty(code))
            {
                return code;
            }
            if (cboSuKien.SelectedValue != null)
            {
                string val = cboSuKien.SelectedValue.ToString() ?? string.Empty;
                if (val.StartsWith("(") && val.Contains(','))
                {
                    var parts = val.Trim('(', ')').Split(',');
                    if (parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0]))
                        return parts[0].Trim();
                }
                return val;
            }
            return string.Empty;
        }

        private void LoadCameraDevices()
        {
            try
            {
                _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                cboCamera.Items.Clear();

                if (_videoDevices.Count > 0)
                {
                    foreach (FilterInfo device in _videoDevices)
                    {
                        cboCamera.Items.Add(device.Name);
                    }
                    cboCamera.SelectedIndex = 0;
                    btnBatCamera.Enabled = true;
                    lblFooterStatus.Text = $"Trạng thái camera: Tìm thấy {_videoDevices.Count} thiết bị camera.";
                }
                else
                {
                    cboCamera.Items.Add("(Không tìm thấy Webcam - Hỗ trợ nhập mã thủ công)");
                    cboCamera.SelectedIndex = 0;
                    btnBatCamera.Enabled = false;
                    btnTatCamera.Enabled = false;
                    lblFooterStatus.Text = "Trạng thái camera: Không tìm thấy camera phần cứng. Vui lòng dùng ô nhập mã thủ công.";
                }
            }
            catch (Exception ex)
            {
                lblFooterStatus.Text = "Lỗi dò tìm thiết bị camera: " + ex.Message;
                btnBatCamera.Enabled = false;
            }
        }

        private void CboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshStatistics();
            RefreshRecentLogs();
            ResetStatusBanner();
        }

        private void BtnBatCamera_Click(object sender, EventArgs e)
        {
            if (_videoDevices == null || _videoDevices.Count == 0 || cboCamera.SelectedIndex < 0)
            {
                MessageBox.Show("Không có camera hợp lệ để khởi động.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                StopCamera();
                string moniker = _videoDevices[cboCamera.SelectedIndex].MonikerString;
                _videoSource = new VideoCaptureDevice(moniker);
                _videoSource.NewFrame += VideoSource_NewFrame;
                _videoSource.Start();

                btnBatCamera.Enabled = false;
                btnTatCamera.Enabled = true;
                cboCamera.Enabled = false;
                lblFooterStatus.Text = $"Trạng thái camera: Đang ghi hình từ '{cboCamera.Text}'...";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi động camera: " + ex.Message, "Lỗi Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopCamera();
            }
        }

        private void BtnTatCamera_Click(object sender, EventArgs e)
        {
            StopCamera();
            btnBatCamera.Enabled = _videoDevices != null && _videoDevices.Count > 0;
            btnTatCamera.Enabled = false;
            cboCamera.Enabled = true;
            lblFooterStatus.Text = "Trạng thái camera: Đã dừng camera.";
        }

        private void StopCamera()
        {
            try
            {
                if (_videoSource != null)
                {
                    if (_videoSource.IsRunning)
                    {
                        _videoSource.SignalToStop();
                        _videoSource.WaitForStop();
                    }
                    _videoSource.NewFrame -= VideoSource_NewFrame;
                    _videoSource = null;
                }
            }
            catch
            {
                // Bỏ qua lỗi ngắt kết nối phần cứng camera
            }

            if (picCameraFeed.Image != null)
            {
                var oldImg = picCameraFeed.Image;
                picCameraFeed.Image = null;
                oldImg.Dispose();
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap frameClone = (Bitmap)eventArgs.Frame.Clone();

                // Hiển thị khung hình lên PictureBox trên UI Thread
                if (!IsDisposed && picCameraFeed.IsHandleCreated)
                {
                    picCameraFeed.BeginInvoke(new Action(() =>
                    {
                        var prev = picCameraFeed.Image;
                        picCameraFeed.Image = frameClone;
                        prev?.Dispose();
                    }));
                }

                // Throttling: Quét mã QR mỗi 350ms để tối ưu hóa CPU
                if (_isScanning) return;
                if ((DateTime.Now - _lastScanTime).TotalMilliseconds < 350) return;

                _isScanning = true;
                _lastScanTime = DateTime.Now;

                using (Bitmap scanBitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    var result = _barcodeReader.Decode(scanBitmap);
                    if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    {
                        // Thử xoay 90 độ nếu góc quét bị nghiêng
                        using var rotated = (Bitmap)scanBitmap.Clone();
                        rotated.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        result = _barcodeReader.Decode(rotated);
                    }

                    if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                    {
                        string scannedText = result.Text.Trim();

                        // Tránh quét liên tục cùng 1 mã trong vòng 2.5 giây
                        if (scannedText == _lastScannedCode && (DateTime.Now - _lastScanTime).TotalSeconds < 2.5)
                        {
                            _isScanning = false;
                            return;
                        }

                        _lastScannedCode = scannedText;

                        // Chuyển việc xử lý check-in sang UI Thread
                        if (!IsDisposed && IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                ExecuteCheckIn(scannedText, "Webcam QR");
                            }));
                        }
                    }
                }
            }
            catch
            {
                // Bỏ qua ngoại lệ khung hình tạm thời
            }
            finally
            {
                _isScanning = false;
            }
        }

        private void BtnCheckInManual_Click(object sender, EventArgs e)
        {
            string code = txtMaVeManual.Text.Trim();
            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Vui lòng nhập mã vé hoặc chuỗi QR token để soát vé.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaVeManual.Focus();
                return;
            }

            ExecuteCheckIn(code, "Nhập thủ công");
            txtMaVeManual.SelectAll();
            txtMaVeManual.Focus();
        }

        private void ExecuteCheckIn(string rawCode, string method)
        {
            string selectedEventId = GetSelectedEventCode();

            var request = new CheckInRequestDto
            {
                MaSuKien = selectedEventId,
                MaQRLuocDoc = rawCode,
                NguoiCheckIn = CurrentSession.TenDangNhap ?? "STAFF",
                PhuongThuc = method
            };

            var serviceResult = _checkInService.ProcessCheckIn(request);

            if (serviceResult.IsSuccess && serviceResult.Data != null)
            {
                var checkIn = serviceResult.Data;
                if (checkIn.IsWarningDuplicate)
                {
                    // BR-12: CẢNH BÁO TRÙNG LẶP
                    ShowBannerDuplicate(checkIn);
                    PlayAlertSound();
                }
                else
                {
                    // BR-11: CHECK-IN THÀNH CÔNG
                    ShowBannerSuccess(checkIn);
                }
            }
            else
            {
                // LỖI HOẶC VÉ KHÔNG HỢP LỆ (BR-11)
                ShowBannerError(serviceResult.Message ?? "Vé không hợp lệ.");
                PlayAlertSound();
            }

            RefreshStatistics();
            RefreshRecentLogs();
        }

        private void ShowBannerSuccess(CheckInResultDto checkIn)
        {
            pnlStatusBanner.BackColor = Color.FromArgb(220, 252, 231); // Green light #DCFCE7
            lblStatusTitle.ForeColor = Color.FromArgb(22, 101, 52);    // Green dark #166534
            lblStatusDetail.ForeColor = Color.FromArgb(21, 128, 61);

            lblStatusTitle.Text = $"✓ CHECK-IN THÀNH CÔNG: {checkIn.TenNguoiThamGia.ToUpper()}";
            lblStatusDetail.Text = $"Mã vé: {checkIn.MaVe} | Hạng vé: {checkIn.TenHangVe} | Sự kiện: {checkIn.TenSuKien} | Giờ vào: {DateTime.Now:HH:mm:ss}";
        }

        private void ShowBannerDuplicate(CheckInResultDto checkIn)
        {
            pnlStatusBanner.BackColor = Color.FromArgb(254, 242, 242); // Red light #FEF2F2
            lblStatusTitle.ForeColor = Color.FromArgb(185, 28, 28);    // Red dark #B91C1C
            lblStatusDetail.ForeColor = Color.FromArgb(153, 27, 27);

            string timeStr = checkIn.ThoiGianCheckInTruocDo.HasValue 
                ? checkIn.ThoiGianCheckInTruocDo.Value.ToString("HH:mm:ss dd/MM/yyyy") 
                : "Không rõ thời gian";
            string checker = !string.IsNullOrWhiteSpace(checkIn.NguoiCheckInTruocDo) ? checkIn.NguoiCheckInTruocDo : "nhân viên";

            lblStatusTitle.Text = $"⚠ CẢNH BÁO: Vé này đã check-in vào lúc {timeStr} bởi {checker}!";
            lblStatusDetail.Text = $"Mã vé: {checkIn.MaVe} | Người tham gia: {checkIn.TenNguoiThamGia} | Trạng thái: Từ chối check-in lần hai";
        }

        private void ShowBannerError(string errorMessage)
        {
            pnlStatusBanner.BackColor = Color.FromArgb(254, 226, 226); // Red pale
            lblStatusTitle.ForeColor = Color.FromArgb(185, 28, 28);
            lblStatusDetail.ForeColor = Color.FromArgb(127, 29, 29);

            lblStatusTitle.Text = errorMessage.Contains("không tồn tại", StringComparison.OrdinalIgnoreCase)
                ? "✗ Mã vé không tồn tại trong hệ thống!"
                : "✗ TỪ CHỐI CHECK-IN: VÉ KHÔNG HỢP LỆ";
            lblStatusDetail.Text = errorMessage;
        }

        private void ResetStatusBanner()
        {
            pnlStatusBanner.BackColor = Color.FromArgb(241, 245, 249);
            lblStatusTitle.ForeColor = Color.FromArgb(30, 58, 138);
            lblStatusDetail.ForeColor = Color.FromArgb(71, 85, 105);

            lblStatusTitle.Text = "SẴN SÀNG QUÉT MÃ QR";
            lblStatusDetail.Text = "Đưa mã QR trên vé hoặc điện thoại vào trước camera để quét tự động";
        }

        private void RefreshStatistics()
        {
            string selectedEventId = GetSelectedEventCode();
            var stats = _checkInService.GetCheckInStatistics(selectedEventId);

            lblKpiTongVe.Text = stats.TongVePhatHanh.ToString();
            lblKpiDaCheckIn.Text = stats.DaCheckIn.ToString();
            lblKpiChuaCheckIn.Text = stats.ChuaCheckIn.ToString();
            lblKpiTyLe.Text = $"{stats.TyLeCheckIn:F1}%";
        }

        private void RefreshRecentLogs()
        {
            string selectedEventId = GetSelectedEventCode();
            var logs = _checkInService.GetRecentCheckIns(selectedEventId, 25);

            dgvRecentCheckIns.Rows.Clear();
            foreach (var log in logs)
            {
                int rowIndex = dgvRecentCheckIns.Rows.Add(
                    log.ThoiGian.ToString("HH:mm:ss"),
                    log.MaVe,
                    log.TenNguoiThamGia,
                    log.TenHangVe,
                    log.PhuongThuc,
                    log.GhiChu
                );

                if (!log.ThanhCong)
                {
                    dgvRecentCheckIns.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(185, 28, 28);
                }
            }
        }

        private void BtnResetDemo_Click(object sender, EventArgs e)
        {
            _checkInService.ResetCheckInForDemo("VE00001");
            _checkInService.ResetCheckInForDemo("VE00002");
            _checkInService.ResetCheckInForDemo("VE00003");
            _lastScannedCode = string.Empty;

            RefreshStatistics();
            RefreshRecentLogs();
            ResetStatusBanner();

            MessageBox.Show(
                "Đã đặt lại trạng thái các vé mẫu về 'Hợp lệ' chưa check-in để thử nghiệm lại quy trình soát vé.",
                "Đặt lại thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void PlayAlertSound()
        {
            try
            {
                SystemSounds.Hand.Play();
            }
            catch
            {
                // Bỏ qua lỗi âm thanh trên môi trường không có sound driver
            }
        }

        private void FrmCheckInCamera_FormClosing(object? sender, FormClosingEventArgs e)
        {
            StopCamera();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            if (keyData == Keys.F5)
            {
                RefreshStatistics();
                RefreshRecentLogs();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
