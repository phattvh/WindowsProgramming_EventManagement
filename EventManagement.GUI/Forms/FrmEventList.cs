using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.GUI.Common;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình F04 - Quản lý danh sách sự kiện tổng thể
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-12 / Ngày 7)
    /// Tuân thủ: Mẫu bố cục 2 (Archetype 2), 3 trạng thái màn hình (Xem - Thêm - Sửa),
    /// thẩm định các quy tắc nghiệp vụ: BR-02 (ràng buộc xóa sự kiện), BR-03 (thời gian sự kiện), BR-04 (chống trùng lịch địa điểm).
    /// </summary>
    public partial class FrmEventList : FrmBase
    {
        private readonly IEventService _eventService;
        private bool _isLoading = true;
        private FormMode _currentMode = FormMode.View;
        private List<EventListDto> _currentList = new();
        private List<EventTypeDto> _eventTypes = new();
        private List<VenueDto> _venues = new();

        public FrmEventList() : this(new EventService())
        {
        }

        public FrmEventList(IEventService eventService)
        {
            InitializeComponent();
            _eventService = eventService ?? new EventService();
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            // Thiết lập phong cách nút bấm theo chuẩn Corporate Blue Palette & Semantic Roles
            StyleButton(btnThem, ButtonRole.Success);
            StyleButton(btnSua, ButtonRole.Primary);
            StyleButton(btnXoa, ButtonRole.Danger);
            StyleButton(btnHuySuKien, ButtonRole.Secondary);
            StyleButton(btnChiTiet, ButtonRole.Primary);
            StyleButton(btnXuatExcel, ButtonRole.Secondary);
            StyleButton(btnLuu, ButtonRole.Primary);
            StyleButton(btnHuy, ButtonRole.Secondary);
            StyleButton(btnLamMoi, ButtonRole.Secondary);

            StyleDataGridView(dgvSuKien);
            dgvSuKien.AutoGenerateColumns = false;
            dgvSuKien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvSuKien.ScrollBars = ScrollBars.Both;
            dgvSuKien.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    btnChiTiet_Click(s ?? this, e);
                }
            };

            LoadAuxiliaryData();
        }

        private void LoadAuxiliaryData()
        {
            // 1. Nạp danh mục Loại sự kiện (từ F06)
            _eventTypes = _eventService.GetAvailableEventTypes();

            cboLocLoaiSK.Items.Clear();
            cboLocLoaiSK.Items.Add("Tất cả loại sự kiện");
            foreach (var type in _eventTypes)
            {
                cboLocLoaiSK.Items.Add(type.TenLoaiSK);
            }
            cboLocLoaiSK.SelectedIndex = 0;

            cboLoaiSK.Items.Clear();
            foreach (var type in _eventTypes)
            {
                cboLoaiSK.Items.Add(new ComboBoxItem(type.MaLoaiSK, $"{type.MaLoaiSK} - {type.TenLoaiSK}"));
            }
            if (cboLoaiSK.Items.Count > 0) cboLoaiSK.SelectedIndex = 0;

            // 2. Nạp danh mục Địa điểm (từ F07)
            _venues = _eventService.GetAvailableVenues();

            cboDiaDiem.Items.Clear();
            foreach (var v in _venues)
            {
                string statusNotice = v.TrangThai == "Hoạt động" ? "" : $" [{v.TrangThai}]";
                cboDiaDiem.Items.Add(new ComboBoxItem(v.MaDiaDiem, $"{v.MaDiaDiem} - {v.TenDiaDiem} (Sức chứa: {v.SucChuaToiDa:N0}){statusNotice}"));
            }
            if (cboDiaDiem.Items.Count > 0) cboDiaDiem.SelectedIndex = 0;

            // 3. Nạp danh mục Trạng thái sự kiện
            cboLocTrangThai.Items.Clear();
            cboLocTrangThai.Items.AddRange(new object[]
            {
                "Tất cả trạng thái",
                "Chuẩn bị",
                "Sắp diễn ra",
                "Đang diễn ra",
                "Đã hoàn thành",
                "Đã hủy"
            });
            cboLocTrangThai.SelectedIndex = 0;

            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[]
            {
                "Chuẩn bị",
                "Sắp diễn ra",
                "Đang diễn ra",
                "Đã hoàn thành",
                "Đã hủy"
            });
            cboTrangThai.SelectedIndex = 0;

            // Khoảng thời gian mặc định cho bộ lọc
            dtpLocTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtpLocDenNgay.Value = DateTime.Now.AddMonths(2);

            // 4. Nạp danh mục Người phụ trách từ CSDL
            cboNguoiPhuTrach.Items.Clear();
            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var userList = db.NguoiDungs.OrderBy(u => u.HoTen).Select(u => u.HoTen).ToList();
                foreach (var name in userList)
                {
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        cboNguoiPhuTrach.Items.Add(name);
                    }
                }
            }
            catch { }

            if (cboNguoiPhuTrach.Items.Count == 0)
            {
                cboNguoiPhuTrach.Items.AddRange(new object[]
                {
                    "Quản trị viên Hệ thống",
                    "Trần Minh Tâm (Trưởng BTC)",
                    "Lê Thị Hương (Soát vé)",
                    "Nguyễn Văn An",
                    "Phạm Hoàng Nam"
                });
            }
        }

        private void FrmEventList_Load(object sender, EventArgs e)
        {
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            _isLoading = false;
            DistributeSummaryStats();
            this.Resize += (s, ev) => DistributeSummaryStats();
            LoadData();
            SetFormState(FormMode.View);
            DataSyncNotifier.GeneralDataChanged += OnDataSyncChanged;
            this.FormClosed += (s, ev) => DataSyncNotifier.GeneralDataChanged -= OnDataSyncChanged;
        }

        private void OnDataSyncChanged()
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            try
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(new Action(() => { LoadAuxiliaryData(); LoadData(); }));
                else
                { LoadAuxiliaryData(); LoadData(); }
            }
            catch { }
        }

        public void DistributeSummaryStats()
        {
            var labels = new[] { lblTongSo, lblDangDienRa, lblSapDienRa, lblChuanBi, lblDaHoanThanh, lblDaHuy };
            int totalWidth = pnlSummary.ClientSize.Width - 20;
            if (totalWidth <= 100) return;
            int colWidth = totalWidth / labels.Length;
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Location = new Point(10 + i * colWidth, 10);
            }
        }

        public void LoadData()
        {
            if (_isLoading) return;

            string? keyword = txtTimKiem.Text.Trim();
            string? loaiSK = cboLocLoaiSK.SelectedIndex > 0 ? cboLocLoaiSK.SelectedItem?.ToString() : null;
            string? trangThai = cboLocTrangThai.SelectedIndex > 0 ? cboLocTrangThai.SelectedItem?.ToString() : null;
            DateTime? tuNgay = chkLocThoiGian.Checked ? dtpLocTuNgay.Value.Date : null;
            DateTime? denNgay = chkLocThoiGian.Checked ? dtpLocDenNgay.Value.Date : null;

            var result = _eventService.GetAll(keyword, loaiSK, trangThai, null, tuNgay, denNgay);
            if (result.IsSuccess && result.Data != null)
            {
                _currentList = result.Data;
                dgvSuKien.DataSource = null;
                dgvSuKien.DataSource = _currentList;

                UpdateSummaryStatistics();

                if (_currentList.Count > 0)
                {
                    dgvSuKien.Rows[0].Selected = true;
                    DisplaySelectedRow(_currentList[0]);
                }
                else
                {
                    ClearInputs();
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummaryStatistics()
        {
            int total = _currentList.Count;
            int dangDienRa = _currentList.Count(x => x.TrangThai == "Đang diễn ra");
            int sapDienRa = _currentList.Count(x => x.TrangThai == "Sắp diễn ra");
            int chuanBi = _currentList.Count(x => x.TrangThai == "Chuẩn bị");
            int daHoanThanh = _currentList.Count(x => x.TrangThai == "Đã hoàn thành");
            int daHuy = _currentList.Count(x => x.TrangThai == "Đã hủy");

            lblTongSo.Text = $"Tổng số: {total} sự kiện";
            lblDangDienRa.Text = $"Đang diễn ra: {dangDienRa}";
            lblSapDienRa.Text = $"Sắp diễn ra: {sapDienRa}";
            lblChuanBi.Text = $"Chuẩn bị: {chuanBi}";
            lblDaHoanThanh.Text = $"Đã hoàn thành: {daHoanThanh}";
            lblDaHuy.Text = $"Đã hủy: {daHuy}";
        }

        private void DisplaySelectedRow(EventListDto dto)
        {
            if (_currentMode != FormMode.View) return;

            txtMaSuKien.Text = dto.MaSuKien;
            txtTenSuKien.Text = dto.TenSuKien;

            // Chọn Loại sự kiện
            for (int i = 0; i < cboLoaiSK.Items.Count; i++)
            {
                if (cboLoaiSK.Items[i] is ComboBoxItem item && item.Value.Equals(dto.MaLoaiSK, StringComparison.OrdinalIgnoreCase))
                {
                    cboLoaiSK.SelectedIndex = i;
                    break;
                }
            }

            // Chọn Địa điểm
            for (int i = 0; i < cboDiaDiem.Items.Count; i++)
            {
                if (cboDiaDiem.Items[i] is ComboBoxItem item && item.Value.Equals(dto.MaDiaDiem, StringComparison.OrdinalIgnoreCase))
                {
                    cboDiaDiem.SelectedIndex = i;
                    break;
                }
            }

            dtpNgayBatDau.Value = dto.NgayBatDau;
            dtpNgayKetThuc.Value = dto.NgayKetThuc;
            nudSucChua.Value = dto.SucChua > 0 ? Math.Min(dto.SucChua, nudSucChua.Maximum) : 500;
            nudNganSach.Value = dto.NganSachDuKien >= 0 ? Math.Min(dto.NganSachDuKien, nudNganSach.Maximum) : 0;
            cboNguoiPhuTrach.Text = dto.NguoiPhuTrach ?? string.Empty;
            btnHuySuKien.Enabled = _currentMode == FormMode.View;
            cboTrangThai.SelectedItem = dto.TrangThai;
            txtMoTa.Text = dto.MoTa ?? string.Empty;

            errBaoLoi.Clear();
        }

        private void ClearInputs()
        {
            txtMaSuKien.Clear();
            txtTenSuKien.Clear();
            if (cboLoaiSK.Items.Count > 0) cboLoaiSK.SelectedIndex = 0;
            if (cboDiaDiem.Items.Count > 0) cboDiaDiem.SelectedIndex = 0;
            dtpNgayBatDau.Value = DateTime.Now.Date.AddDays(1).AddHours(8);
            dtpNgayKetThuc.Value = DateTime.Now.Date.AddDays(1).AddHours(17);
            nudSucChua.Value = 500;
            nudNganSach.Value = 100000000m;
            cboNguoiPhuTrach.Text = string.Empty;
            cboTrangThai.SelectedItem = "Chuẩn bị";
            txtMoTa.Clear();
            errBaoLoi.Clear();
        }

        private void SetFormState(FormMode mode)
        {
            _currentMode = mode;

            bool isEditing = mode == FormMode.Add || mode == FormMode.Edit;

            // Quản lý trạng thái các ô nhập liệu theo chuẩn Mẫu 2
            txtMaSuKien.ReadOnly = true; // Khóa chính luôn ReadOnly (khi Add sẽ do SuggestNextCode tự sinh)
            txtTenSuKien.ReadOnly = !isEditing;
            cboLoaiSK.Enabled = isEditing;
            cboDiaDiem.Enabled = isEditing;
            dtpNgayBatDau.Enabled = isEditing;
            dtpNgayKetThuc.Enabled = isEditing;
            nudSucChua.Enabled = isEditing;
            nudNganSach.Enabled = isEditing;
            cboNguoiPhuTrach.Enabled = isEditing;
            cboTrangThai.Enabled = isEditing;
            txtMoTa.ReadOnly = !isEditing;

            // Màu nền các ô nhập liệu hỗ trợ trực quan
            Color bgInput = isEditing ? Color.White : Color.FromArgb(248, 250, 252);
            txtTenSuKien.BackColor = bgInput;
            cboNguoiPhuTrach.BackColor = bgInput;
            txtMoTa.BackColor = bgInput;
            txtMaSuKien.BackColor = Color.FromArgb(241, 245, 249);

            // Nút bấm tác nghiệp
            btnThem.Enabled = !isEditing;
            btnSua.Enabled = !isEditing && _currentList.Count > 0;
            btnXoa.Enabled = !isEditing && _currentList.Count > 0;
            btnHuySuKien.Enabled = !isEditing && _currentList.Count > 0;
            btnChiTiet.Enabled = !isEditing && _currentList.Count > 0;
            btnXuatExcel.Enabled = !isEditing;

            btnLuu.Visible = isEditing;
            btnHuy.Visible = isEditing;
            btnLuu.Enabled = isEditing;
            btnHuy.Enabled = isEditing;


            if (mode == FormMode.Add)
            {
                cboTrangThai.Items.Clear();
                cboTrangThai.Items.AddRange(new object[] { "Chuẩn bị", "Sắp diễn ra", "Đang diễn ra" });
                cboTrangThai.SelectedItem = "Chuẩn bị";
            }
            else
            {
                string? prevSelected = cboTrangThai.SelectedItem?.ToString();
                cboTrangThai.Items.Clear();
                cboTrangThai.Items.AddRange(new object[] { "Chuẩn bị", "Sắp diễn ra", "Đang diễn ra", "Đã hoàn thành", "Đã hủy" });
                if (!string.IsNullOrEmpty(prevSelected) && cboTrangThai.Items.Contains(prevSelected))
                {
                    cboTrangThai.SelectedItem = prevSelected;
                }
            }

            dgvSuKien.Enabled = !isEditing;
            pnlFilter.Enabled = !isEditing;
        }

        #region Event Handlers - Lọc & Tìm kiếm đa điều kiện

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkLocThoiGian_CheckedChanged(object sender, EventArgs e)
        {
            dtpLocTuNgay.Enabled = chkLocThoiGian.Checked;
            dtpLocDenNgay.Enabled = chkLocThoiGian.Checked;
            LoadData();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboLocLoaiSK.SelectedIndex = 0;
            cboLocTrangThai.SelectedIndex = 0;
            chkLocThoiGian.Checked = false;
            LoadData();
        }

        #endregion

        #region Event Handlers - Tác nghiệp CRUD & Nghiệp vụ

        private void btnThem_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.Add);
            ClearInputs();

            // Tự động sinh mã sự kiện theo chuẩn quy ước BR-17
            txtMaSuKien.Text = _eventService.SuggestNextCode();
            txtTenSuKien.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null || dgvSuKien.CurrentRow.DataBoundItem is not EventListDto dto)
            {
                MessageBox.Show("Vui lòng chọn một sự kiện từ danh sách để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SetFormState(FormMode.Edit);
            txtTenSuKien.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null || dgvSuKien.CurrentRow.DataBoundItem is not EventListDto dto)
            {
                MessageBox.Show("Vui lòng chọn một sự kiện để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sự kiện:\n\n[{dto.MaSuKien}] {dto.TenSuKien}\n\nLưu ý: Hành động này không thể hoàn tác!",
                "Xác nhận xóa sự kiện",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            // Gọi dịch vụ xóa để thẩm định quy tắc BR-02
            var result = _eventService.Delete(dto.MaSuKien);
            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                // Xử lý ràng buộc BR-02: Nếu đã phát sinh dữ liệu liên kết thì đề xuất chuyển sang "Đã hủy"
                if (result.ErrorCode == "BR-02")
                {
                    var optCancel = MessageBox.Show(
                        $"{result.Message}\n\nBạn có muốn chuyển trạng thái sự kiện này sang 'Đã hủy' ngay bây giờ không?",
                        "Cảnh báo ràng buộc nghiệp vụ BR-02",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (optCancel == DialogResult.Yes)
                    {
                        var cancelRes = _eventService.CancelEvent(dto.MaSuKien);
                        MessageBox.Show(cancelRes.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi xóa sự kiện", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuySuKien_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null || dgvSuKien.CurrentRow.DataBoundItem is not EventListDto dto)
            {
                MessageBox.Show("Vui lòng chọn một sự kiện để hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dto.TrangThai == "Đang diễn ra" || dto.TrangThai == "Đã kết thúc" || dto.TrangThai == "Đã hoàn thành")
            {
                MessageBox.Show("Sự kiện Đang diễn ra hoặc Đã kết thúc không được phép hủy!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dto.TrangThai == "Đã hủy" || dto.TrangThai == "Hủy")
            {
                MessageBox.Show("Sự kiện này đã ở trạng thái 'Đã hủy' từ trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Xác nhận hủy tổ chức sự kiện:\n\n[{dto.MaSuKien}] {dto.TenSuKien}\n\nTrạng thái sự kiện sẽ chuyển sang 'Đã hủy' (BR-02)?",
                "Xác nhận hủy sự kiện",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            var result = _eventService.CancelEvent(dto.MaSuKien);
            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi hủy sự kiện", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            errBaoLoi.Clear();

            // 1. Kiểm tra tính hợp lệ dữ liệu nhập
            bool hasError = false;

            if (string.IsNullOrWhiteSpace(txtTenSuKien.Text))
            {
                errBaoLoi.SetError(txtTenSuKien, "Tên sự kiện không được để trống!");
                hasError = true;
            }

            var selectedType = cboLoaiSK.SelectedItem as ComboBoxItem;
            if (selectedType == null)
            {
                errBaoLoi.SetError(cboLoaiSK, "Vui lòng chọn loại sự kiện!");
                hasError = true;
            }

            var selectedVenue = cboDiaDiem.SelectedItem as ComboBoxItem;
            if (selectedVenue == null)
            {
                errBaoLoi.SetError(cboDiaDiem, "Vui lòng chọn địa điểm tổ chức!");
                hasError = true;
            }

            // Thuật toán BR-03: NgayKetThuc >= NgayBatDau
            if (dtpNgayKetThuc.Value < dtpNgayBatDau.Value)
            {
                errBaoLoi.SetError(dtpNgayKetThuc, "Ngày kết thúc không được trước ngày bắt đầu (BR-03)!");
                hasError = true;
            }

            if (hasError || selectedType == null || selectedVenue == null)
            {
                MessageBox.Show("Vui lòng kiểm tra lại các trường dữ liệu bị báo lỗi đỏ!", "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = new EventListDto
            {
                MaSuKien = txtMaSuKien.Text.Trim(),
                TenSuKien = txtTenSuKien.Text.Trim(),
                MaLoaiSK = selectedType.Value,
                TenLoaiSK = selectedType.Text.Split('-').Last().Trim(),
                MaDiaDiem = selectedVenue.Value,
                TenDiaDiem = selectedVenue.Text.Split('-').Last().Trim(),
                NgayBatDau = dtpNgayBatDau.Value,
                NgayKetThuc = dtpNgayKetThuc.Value,
                SucChua = (int)nudSucChua.Value,
                NganSachDuKien = nudNganSach.Value,
                NguoiPhuTrach = string.IsNullOrWhiteSpace(cboNguoiPhuTrach.Text) ? "Chưa phân công" : cboNguoiPhuTrach.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Chuẩn bị",
                MoTa = txtMoTa.Text.Trim()
            };

            ServiceResult result;
            if (_currentMode == FormMode.Add)
            {
                result = _eventService.Add(dto);
            }
            else
            {
                result = _eventService.Update(dto);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetFormState(FormMode.View);
                LoadData();

                // Chọn lại dòng vừa lưu
                for (int i = 0; i < dgvSuKien.Rows.Count; i++)
                {
                    if (dgvSuKien.Rows[i].DataBoundItem is EventListDto item && item.MaSuKien.Equals(dto.MaSuKien, StringComparison.OrdinalIgnoreCase))
                    {
                        dgvSuKien.Rows[i].Selected = true;
                        DisplaySelectedRow(item);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi lưu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result.ErrorCode == "BR-03")
                {
                    errBaoLoi.SetError(dtpNgayKetThuc, result.Message);
                }
                else if (result.ErrorCode == "BR-04")
                {
                    errBaoLoi.SetError(cboDiaDiem, result.Message);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            errBaoLoi.Clear();
            SetFormState(FormMode.View);

            if (dgvSuKien.CurrentRow != null && dgvSuKien.CurrentRow.DataBoundItem is EventListDto dto)
            {
                DisplaySelectedRow(dto);
            }
            else if (_currentList.Count > 0)
            {
                DisplaySelectedRow(_currentList[0]);
            }
            else
            {
                ClearInputs();
            }
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null || dgvSuKien.CurrentRow.DataBoundItem is not EventListDto dto)
            {
                MessageBox.Show("Vui lòng chọn một sự kiện để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var frmDetail = new FrmEventDetail(dto.MaSuKien);
            frmDetail.ShowDialog(this);
            LoadData();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (_currentList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu sự kiện để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Tập tin CSV (*.csv)|*.csv|Tất cả tệp (*.*)|*.*",
                FileName = $"DanhSachSuKien_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Xuất danh sách sự kiện tổng thể ra file CSV"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var sb = new StringBuilder();
                // Ghi header CSV
                sb.AppendLine("Mã Sự Kiện,Tên Sự Kiện,Loại Sự Kiện,Địa Điểm,Bắt Đầu,Kết Thúc,Sức Chứa,Ngân Sách (VNĐ),Người Phụ Trách,Trạng Thái,Vé Đã Bán,Tỷ Lệ Check-in (%)");

                foreach (var ev in _currentList)
                {
                    sb.AppendLine($"\"{EscapeCsv(ev.MaSuKien)}\",\"{EscapeCsv(ev.TenSuKien)}\",\"{EscapeCsv(ev.TenLoaiSK)}\",\"{EscapeCsv(ev.TenDiaDiem)}\",\"{ev.NgayBatDau:dd/MM/yyyy HH:mm}\",\"{ev.NgayKetThuc:dd/MM/yyyy HH:mm}\",{ev.SucChua},{ev.NganSachDuKien:0},\"{EscapeCsv(ev.NguoiPhuTrach)}\",\"{EscapeCsv(ev.TrangThai)}\",{ev.SoVeDaBan},{ev.TyLeCheckIn:0.0}");
                }

                // Xuất file với UTF-8 BOM để Excel hiển thị đúng tiếng Việt có dấu
                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);

                MessageBox.Show($"Đã xuất thành công { _currentList.Count} bản ghi sự kiện ra tệp:\n{sfd.FileName}", "Xuất dữ liệu thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong quá trình xuất tệp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string EscapeCsv(string? text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Replace("\"", "\"\"");
        }

        #endregion

        #region DataGridView & Shortcuts

        private void dgvSuKien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow != null && dgvSuKien.CurrentRow.DataBoundItem is EventListDto dto)
            {
                DisplaySelectedRow(dto);
            }
        }

        private void dgvSuKien_DoubleClick(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View && dgvSuKien.CurrentRow != null)
            {
                btnChiTiet.PerformClick();
            }
        }

        private void dgvSuKien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSuKien.Rows.Count) return;

            string colName = dgvSuKien.Columns[e.ColumnIndex].Name;

            if (colName == "colNgayBatDau" || colName == "colNgayKetThuc")
            {
                if (e.Value is DateTime dt)
                {
                    e.Value = dt.ToString("dd/MM/yyyy HH:mm");
                    e.FormattingApplied = true;
                }
            }
            else if (colName == "colNganSachDuKien")
            {
                if (e.Value is decimal val)
                {
                    e.Value = val.ToString("#,##0 đ");
                    e.FormattingApplied = true;
                }
            }
            else if (colName == "colTyLeCheckIn")
            {
                if (e.Value is double rate)
                {
                    e.Value = $"{rate:0.0}%";
                    e.FormattingApplied = true;
                }
            }
            else if (colName == "colTrangThai")
            {
                string? status = e.Value?.ToString();
                if (e.CellStyle != null && !string.IsNullOrEmpty(status))
                {
                    switch (status)
                    {
                        case "Đang diễn ra":
                            e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74); // Xanh lá
                            e.CellStyle.Font = new Font(dgvSuKien.Font, FontStyle.Bold);
                            break;
                        case "Sắp diễn ra":
                            e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6); // Vàng cam
                            e.CellStyle.Font = new Font(dgvSuKien.Font, FontStyle.Bold);
                            break;
                        case "Chuẩn bị":
                            e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235); // Xanh dương
                            break;
                        case "Đã hoàn thành":
                            e.CellStyle.ForeColor = Color.FromArgb(71, 85, 105); // Xám trung tính
                            break;
                        case "Đã hủy":
                            e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38); // Đỏ cảnh báo
                            e.CellStyle.Font = new Font(dgvSuKien.Font, FontStyle.Bold);
                            break;
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_currentMode == FormMode.View)
            {
                if (keyData == Keys.F2 || keyData == (Keys.Control | Keys.N))
                {
                    if (btnThem.Enabled) { btnThem.PerformClick(); return true; }
                }
                else if (keyData == Keys.F3 || keyData == (Keys.Control | Keys.E))
                {
                    if (btnSua.Enabled) { btnSua.PerformClick(); return true; }
                }
                else if (keyData == Keys.Delete && !(ActiveControl is TextBoxBase))
                {
                    if (btnXoa.Enabled) { btnXoa.PerformClick(); return true; }
                }
                else if (keyData == Keys.F5)
                {
                    if (btnLamMoi.Enabled) { btnLamMoi.PerformClick(); return true; }
                }
            }
            else
            {
                if (keyData == (Keys.Control | Keys.S))
                {
                    if (btnLuu.Enabled) { btnLuu.PerformClick(); return true; }
                }
                else if (keyData == Keys.Escape)
                {
                    if (btnHuy.Enabled) { btnHuy.PerformClick(); return true; }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        private class ComboBoxItem
        {
            public string Value { get; set; }
            public string Text { get; set; }

            public ComboBoxItem(string value, string text)
            {
                Value = value;
                Text = text;
            }

            public override string ToString() => Text;
        }
    
        private void FrmEventList_KeyDown(object sender, KeyEventArgs e)
        {
            if (_currentMode == FormMode.View)
            {
                if (e.KeyCode == Keys.F2)
                {
                    if (btnThem.Enabled) { btnThem.PerformClick(); e.Handled = true; }
                }
                else if (e.KeyCode == Keys.F3)
                {
                    if (btnSua.Enabled) { btnSua.PerformClick(); e.Handled = true; }
                }
                else if (e.KeyCode == Keys.Delete && !(ActiveControl is TextBoxBase))
                {
                    if (btnXoa.Enabled) { btnXoa.PerformClick(); e.Handled = true; }
                }
                else if (e.KeyCode == Keys.F5)
                {
                    if (btnLamMoi.Enabled) { btnLamMoi.PerformClick(); e.Handled = true; }
                }
            }
            else
            {
                if (e.Control && e.KeyCode == Keys.S)
                {
                    if (btnLuu.Enabled) { btnLuu.PerformClick(); e.Handled = true; }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (btnHuy.Enabled) { btnHuy.PerformClick(); e.Handled = true; }
                }
            }
        }
}
}
