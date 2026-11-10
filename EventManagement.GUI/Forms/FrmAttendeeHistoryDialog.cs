using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Hộp thoại tra cứu Lịch sử tham gia sự kiện của người tham dự (Form F10 - Hướng 2 Popup)
    /// </summary>
    public class FrmAttendeeHistoryDialog : Form
    {
        private readonly NguoiThamGia _attendee;
        private readonly List<AttendeeEventHistoryDto> _history;

        private Panel _pnlHeader = null!;
        private Label _lblTitle = null!;
        private Label _lblSubtitle = null!;
        private DataGridView _dgvHistory = null!;
        private Panel _pnlFooter = null!;
        private Label _lblSummary = null!;
        private Button _btnClose = null!;

        public FrmAttendeeHistoryDialog(NguoiThamGia attendee, List<AttendeeEventHistoryDto> history)
        {
            _attendee = attendee ?? throw new ArgumentNullException(nameof(attendee));
            _history = history ?? new List<AttendeeEventHistoryDto>();

            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = $"Lịch sử tham gia - {_attendee.HoTen} ({_attendee.MaNguoiThamGia})";
            this.Size = new Size(880, 520);
            this.MinimumSize = new Size(780, 420);
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
                Height = 70,
                BackColor = Color.FromArgb(30, 58, 138),
                Padding = new Padding(20, 10, 20, 10)
            };

            _lblTitle = new Label
            {
                Text = $"LỊCH SỬ THAM GIA SỰ KIỆN: {_attendee.HoTen.ToUpper()}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25
            };

            string donViStr = string.IsNullOrWhiteSpace(_attendee.DonViCongTac) ? "Không có" : _attendee.DonViCongTac;
            string dobStr = _attendee.NgaySinh.HasValue ? _attendee.NgaySinh.Value.ToString("dd/MM/yyyy") : "Chưa cập nhật";
            _lblSubtitle = new Label
            {
                Text = $"Mã: {_attendee.MaNguoiThamGia}  |  Ngày sinh: {dobStr}  |  SĐT: {_attendee.SoDienThoai}  |  Email: {_attendee.Email}  |  Đơn vị: {donViStr}",
                ForeColor = Color.FromArgb(224, 231, 255),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Dock = DockStyle.Fill
            };

            _pnlHeader.Controls.Add(_lblSubtitle);
            _pnlHeader.Controls.Add(_lblTitle);

            // 2. DataGridView Panel
            _dgvHistory = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(226, 232, 240),
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoGenerateColumns = false,
                EnableHeadersVisualStyles = false
            };

            _dgvHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            _dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            _dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);
            _dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _dgvHistory.ColumnHeadersHeight = 36;
            _dgvHistory.RowTemplate.Height = 32;
            _dgvHistory.DefaultCellStyle.BackColor = Color.White;
            _dgvHistory.DefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);
            _dgvHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            _dgvHistory.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            _dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            SetupColumns();
            _dgvHistory.DataBindingComplete += (s, e) =>
            {
                foreach (DataGridViewColumn col in _dgvHistory.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            };

            // 3. Footer Panel
            _pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            _lblSummary = new Label
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 58, 138),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _btnClose = new Button
            {
                Text = "Đóng",
                Dock = DockStyle.Right,
                Width = 100,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(15, 23, 42),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.Click += (s, e) => this.Close();

            _pnlFooter.Controls.Add(_lblSummary);
            _pnlFooter.Controls.Add(_btnClose);

            var gridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 10, 15, 10)
            };
            gridContainer.Controls.Add(_dgvHistory);

            this.Controls.Add(gridContainer);
            this.Controls.Add(_pnlFooter);
            this.Controls.Add(_pnlHeader);
        }

        private void SetupColumns()
        {
            _dgvHistory.Columns.Clear();
            _dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            _dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSuKien",
                HeaderText = "Mã sự kiện",
                Width = 95,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            _dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenSuKien",
                HeaderText = "Tên sự kiện",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            });

            _dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenHangVe",
                HeaderText = "Hạng vé",
                Width = 160,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            _dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayDangKy",
                HeaderText = "Ngày đăng ký",
                Width = 110,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            _dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TongTien",
                HeaderText = "Tổng tiền",
                Width = 115,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "#,##0 VNĐ",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            _dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThaiThanhToan",
                HeaderText = "Thanh toán",
                Width = 115,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            _dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThaiCheckIn",
                HeaderText = "Trạng thái",
                Width = 110,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            foreach (DataGridViewColumn col in _dgvHistory.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void LoadData()
        {
            _dgvHistory.DataSource = null;
            _dgvHistory.DataSource = _history;

            decimal totalSpent = _history.Sum(x => x.TongTien);
            int checkedInCount = _history.Count(x => x.TrangThaiCheckIn == "Đã check-in");

            _lblSummary.Text = $"Tổng số sự kiện đã đăng ký: {_history.Count}  |  Đã check-in: {checkedInCount}  |  Tổng chi: {totalSpent:#,##0} VNĐ";
        }
    }
}
