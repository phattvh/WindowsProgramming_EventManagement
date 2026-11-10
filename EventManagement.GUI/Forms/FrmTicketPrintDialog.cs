using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Hộp thoại hiển thị và in vé điện tử kèm mã QR trực quan (Form F11)
    /// </summary>
    public class FrmTicketPrintDialog : Form
    {
        private readonly string _eventName;
        private readonly string _buyerName;
        private readonly string _buyerPhone;
        private readonly List<TicketDetailDto> _tickets;
        private readonly IReportService _reportService;

        private Panel _pnlHeader = null!;
        private Label _lblHeaderTitle = null!;
        private Label _lblHeaderSubtitle = null!;

        private SplitContainer _scMain = null!;
        private ListBox _lstTickets = null!;

        private Panel _pnlCardContainer = null!;
        private Panel _pnlCard = null!;
        private Label _lblCardEvent = null!;
        private Label _lblCardTierBadge = null!;
        private Label _lblCardPrice = null!;

        private Label _lblTicketCode = null!;
        private Label _lblAttendeeName = null!;
        private Label _lblOrderCode = null!;
        private Label _lblIssueDate = null!;
        private Label _lblStatus = null!;

        private PictureBox _picQrCode = null!;
        private Label _lblQrToken = null!;
        private Label _lblQrHint = null!;

        private Panel _pnlFooter = null!;
        private Button _btnSaveQr = null!;
        private Button _btnExportTxt = null!;
        private Button _btnClose = null!;

        public FrmTicketPrintDialog(
            string eventName,
            string buyerName,
            string buyerPhone,
            List<TicketDetailDto> tickets,
            int initialSelectedIndex = 0)
        {
            _eventName = string.IsNullOrWhiteSpace(eventName) ? "Sự kiện" : eventName;
            _buyerName = string.IsNullOrWhiteSpace(buyerName) ? "Khách hàng" : buyerName;
            _buyerPhone = string.IsNullOrWhiteSpace(buyerPhone) ? "" : buyerPhone;
            _tickets = tickets ?? new List<TicketDetailDto>();
            _reportService = new ReportService();

            InitializeComponent();
            BindTickets(initialSelectedIndex);
        }

        private void InitializeComponent()
        {
            this.Text = "In & Xem vé điện tử";
            this.Size = new Size(840, 560);
            this.MinimumSize = new Size(780, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = Color.FromArgb(248, 250, 252);

            // 1. Header Panel
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(30, 58, 138),
                Padding = new Padding(20, 10, 20, 10)
            };

            _lblHeaderTitle = new Label
            {
                Text = "VÉ THAM DỰ SỰ KIỆN ĐIỆN TỬ (E-TICKET)",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 24
            };

            _lblHeaderSubtitle = new Label
            {
                Text = $"Sự kiện: {_eventName}  |  Người mua: {_buyerName} - {_buyerPhone}",
                ForeColor = Color.FromArgb(224, 231, 255),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Dock = DockStyle.Fill
            };

            _pnlHeader.Controls.Add(_lblHeaderSubtitle);
            _pnlHeader.Controls.Add(_lblHeaderTitle);

            // 2. Footer Panel
            _pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.FromArgb(241, 245, 249),
                Padding = new Padding(16, 9, 16, 9)
            };

            _btnSaveQr = new Button
            {
                Text = "Lưu ảnh mã QR (.png)",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 34),
                Location = new Point(16, 9),
                Cursor = Cursors.Hand
            };
            _btnSaveQr.FlatAppearance.BorderSize = 0;
            _btnSaveQr.Click += BtnSaveQr_Click;

            _btnExportTxt = new Button
            {
                Text = "Xuất danh sách (.txt)",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 34),
                Location = new Point(184, 9),
                Cursor = Cursors.Hand
            };
            _btnExportTxt.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            _btnExportTxt.Click += BtnExportTxt_Click;

            _btnClose = new Button
            {
                Text = "Đóng",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 41, 59),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 34),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 116, 9),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            _btnClose.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            _btnClose.Click += (s, e) => this.Close();

            _pnlFooter.Controls.Add(_btnSaveQr);
            _pnlFooter.Controls.Add(_btnExportTxt);
            _pnlFooter.Controls.Add(_btnClose);

            // 3. Main SplitContainer
            _scMain = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 260,
                FixedPanel = FixedPanel.Panel1,
                IsSplitterFixed = true,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Left: Tickets List
            var pnlLeftHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(12, 8, 12, 8)
            };
            var lblListTitle = new Label
            {
                Text = $"DANH SÁCH VÉ ({_tickets.Count})",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Dock = DockStyle.Fill
            };
            pnlLeftHeader.Controls.Add(lblListTitle);

            _lstTickets = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                IntegralHeight = false,
                ItemHeight = 28
            };
            _lstTickets.SelectedIndexChanged += LstTickets_SelectedIndexChanged;

            _scMain.Panel1.BackColor = Color.White;
            _scMain.Panel1.Controls.Add(_lstTickets);
            _scMain.Panel1.Controls.Add(pnlLeftHeader);

            // Right: Ticket Card Preview
            _pnlCardContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(241, 245, 249),
                Padding = new Padding(16)
            };

            _pnlCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            _pnlCard.Paint += (s, pe) =>
            {
                using var pen = new Pen(Color.FromArgb(203, 213, 225), 1.5f);
                pe.Graphics.DrawRectangle(pen, 0, 0, _pnlCard.Width - 1, _pnlCard.Height - 1);
            };

            // Top Card Info
            _lblCardEvent = new Label
            {
                Text = _eventName,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(20, 16),
                Size = new Size(490, 42),
                AutoEllipsis = true
            };

            _lblCardTierBadge = new Label
            {
                Text = "Vé Tiêu chuẩn",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(29, 78, 216),
                BackColor = Color.FromArgb(239, 246, 255),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 62),
                Size = new Size(180, 26)
            };

            _lblCardPrice = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 38, 38),
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(340, 62),
                Size = new Size(170, 26)
            };

            // Separator
            var pnlSep = new Panel
            {
                Location = new Point(20, 96),
                Size = new Size(490, 1),
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Ticket Details (Left section of card)
            int labelY = 110;
            int stepY = 28;

            Label AddDetailRow(string labelText, string initialVal, Font valFont, Color valColor)
            {
                var lblCaption = new Label
                {
                    Text = labelText,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(100, 116, 139),
                    Location = new Point(20, labelY),
                    Size = new Size(100, 22)
                };
                var lblVal = new Label
                {
                    Text = initialVal,
                    Font = valFont,
                    ForeColor = valColor,
                    Location = new Point(125, labelY),
                    Size = new Size(180, 22)
                };
                _pnlCard.Controls.Add(lblCaption);
                _pnlCard.Controls.Add(lblVal);
                labelY += stepY;
                return lblVal;
            }

            _lblTicketCode = AddDetailRow("Mã vé:", "", new Font("Segoe UI", 10.5F, FontStyle.Bold), Color.FromArgb(37, 99, 235));
            _lblAttendeeName = AddDetailRow("Người tham gia:", "", new Font("Segoe UI", 10.5F, FontStyle.Bold), Color.FromArgb(15, 23, 42));
            _lblOrderCode = AddDetailRow("Mã đơn hàng:", "", new Font("Segoe UI", 9F, FontStyle.Regular), Color.FromArgb(51, 65, 85));
            _lblIssueDate = AddDetailRow("Ngày phát hành:", "", new Font("Segoe UI", 9F, FontStyle.Regular), Color.FromArgb(51, 65, 85));
            _lblStatus = AddDetailRow("Trạng thái:", "Hợp lệ", new Font("Segoe UI", 9.5F, FontStyle.Bold), Color.FromArgb(22, 163, 74));

            // Right QR Code section of card
            int qrX = 315;
            int qrY = 108;

            _picQrCode = new PictureBox
            {
                Location = new Point(qrX, qrY),
                Size = new Size(190, 190),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            _lblQrToken = new Label
            {
                Text = "Mã QR Token",
                Font = new Font("Consolas", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(qrX - 10, qrY + 194),
                Size = new Size(210, 20)
            };

            _lblQrHint = new Label
            {
                Text = "(Quét mã QR tại cổng kiểm soát để check-in)",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.FromArgb(148, 163, 184),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(qrX - 15, qrY + 214),
                Size = new Size(220, 20)
            };

            _pnlCard.Controls.Add(_lblCardEvent);
            _pnlCard.Controls.Add(_lblCardTierBadge);
            _pnlCard.Controls.Add(_lblCardPrice);
            _pnlCard.Controls.Add(pnlSep);
            _pnlCard.Controls.Add(_picQrCode);
            _pnlCard.Controls.Add(_lblQrToken);
            _pnlCard.Controls.Add(_lblQrHint);

            _pnlCardContainer.Controls.Add(_pnlCard);
            _scMain.Panel2.Controls.Add(_pnlCardContainer);

            // Add main components
            this.Controls.Add(_scMain);
            this.Controls.Add(_pnlFooter);
            this.Controls.Add(_pnlHeader);
        }

        private void BindTickets(int selectedIndex)
        {
            _lstTickets.Items.Clear();
            for (int i = 0; i < _tickets.Count; i++)
            {
                var t = _tickets[i];
                _lstTickets.Items.Add($"Vé #{i + 1}: {t.MaVe} - {t.TenNguoiThamGia}");
            }

            if (_tickets.Count > 0)
            {
                int idx = Math.Max(0, Math.Min(selectedIndex, _tickets.Count - 1));
                _lstTickets.SelectedIndex = idx;
            }
        }

        private void LstTickets_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int idx = _lstTickets.SelectedIndex;
            if (idx < 0 || idx >= _tickets.Count) return;

            var ticket = _tickets[idx];
            DisplayTicket(ticket);
        }

        private void DisplayTicket(TicketDetailDto ticket)
        {
            _lblTicketCode.Text = ticket.MaVe;
            _lblAttendeeName.Text = ticket.TenNguoiThamGia;
            _lblOrderCode.Text = ticket.MaDangKy;
            _lblIssueDate.Text = ticket.NgayDangKy.ToString("dd/MM/yyyy HH:mm");
            _lblStatus.Text = string.IsNullOrWhiteSpace(ticket.TrangThaiVe) ? "Hợp lệ" : ticket.TrangThaiVe;
            _lblCardTierBadge.Text = ticket.TenHangVe;
            _lblCardPrice.Text = $"{ticket.GiaVe:N0} đ";
            _lblQrToken.Text = ticket.MaQR;

            // Generate QR Code image dynamically
            try
            {
                byte[] qrBytes = _reportService.GenerateQrCodeImageBytes(ticket.MaQR, 200, 200);
                if (qrBytes != null && qrBytes.Length > 0)
                {
                    using var ms = new MemoryStream(qrBytes);
                    var oldImage = _picQrCode.Image;
                    _picQrCode.Image = new Bitmap(ms);
                    oldImage?.Dispose();
                }
                else
                {
                    _picQrCode.Image = null;
                }
            }
            catch
            {
                _picQrCode.Image = null;
            }
        }

        private TicketDetailDto? GetCurrentTicket()
        {
            int idx = _lstTickets.SelectedIndex;
            if (idx >= 0 && idx < _tickets.Count)
            {
                return _tickets[idx];
            }
            return null;
        }

        private void BtnSaveQr_Click(object? sender, EventArgs e)
        {
            var ticket = GetCurrentTicket();
            if (ticket == null || _picQrCode.Image == null)
            {
                MessageBox.Show("Không có hình ảnh mã QR để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG Image (*.png)|*.png|Bitmap Image (*.bmp)|*.bmp",
                FileName = $"QRCode_{ticket.MaVe}_{ticket.MaQR}.png"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _picQrCode.Image.Save(sfd.FileName, ImageFormat.Png);
                    MessageBox.Show($"Đã lưu ảnh mã QR vé {ticket.MaVe} thành công:\n{sfd.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lưu file ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnExportTxt_Click(object? sender, EventArgs e)
        {
            if (_tickets == null || _tickets.Count == 0)
            {
                MessageBox.Show("Không có vé để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("==================================================");
            sb.AppendLine("             DANH SÁCH VÉ SỰ KIỆN");
            sb.AppendLine($"Sự kiện: {_eventName}");
            sb.AppendLine($"Người mua: {_buyerName} - SĐT: {_buyerPhone}");
            sb.AppendLine($"Ngày xuất vé: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine("==================================================");

            foreach (var t in _tickets)
            {
                sb.AppendLine($"Mã vé     : {t.MaVe}");
                sb.AppendLine($"Họ tên    : {t.TenNguoiThamGia}");
                sb.AppendLine($"Hạng vé   : {t.TenHangVe}");
                sb.AppendLine($"Giá vé    : {t.GiaVe:N0} đ");
                sb.AppendLine($"Mã đơn    : {t.MaDangKy}");
                sb.AppendLine($"QR Token  : {t.MaQR}");
                sb.AppendLine("--------------------------------------------------");
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FileName = $"DanhSachVe_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show($"Đã lưu danh sách vé ra file thành công:\n{sfd.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lưu file text: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                _btnSaveQr.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _picQrCode?.Image?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
