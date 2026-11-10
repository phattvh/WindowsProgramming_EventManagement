using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Entities;
using EventManagement.GUI.Common;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Form F12: Quản lý các hạng vé sự kiện và kho vé (Áp dụng Layout Mẫu 2)
    /// Đảm trách các quy tắc: BR-08 (Bảo vệ số vé phát hành >= đã bán), BR-09 (Tồn kho khả dụng)
    /// </summary>
    public class FrmTicketTier : FrmBase
    {
        private readonly TicketTierService _tierService;
        private List<HangVe> _ticketTiers;
        private FormMode _currentMode = FormMode.View;

        // UI Controls
        private Panel _pnlHeader = null!;
        private Label _lblTitle = null!;
        private Panel _pnlSearch = null!;
        private ComboBox _cboEventFilter = null!;
        private TextBox _txtSearch = null!;
        private Button _btnSearch = null!;
        private Button _btnResetSearch = null!;
        private DataGridView _dgvTiers = null!;
        private GroupBox _grpDetails = null!;
        private TextBox _txtMa = null!;
        private TextBox _txtMaSuKien = null!;
        private TextBox _txtTen = null!;
        private NumericUpDown _nudGiaVe = null!;
        private NumericUpDown _nudPhatHanh = null!;
        private TextBox _txtDaBan = null!;
        private TextBox _txtConLai = null!;
        private DateTimePicker _dtpMoBan = null!;
        private DateTimePicker _dtpDongBan = null!;
        private Panel _pnlButtons = null!;
        private Button _btnAdd = null!;
        private Button _btnEdit = null!;
        private Button _btnDelete = null!;
        private Button _btnSave = null!;
        private Button _btnCancel = null!;
        private ErrorProvider _errorProvider = null!;

        public DataGridView DgvTiers => _dgvTiers;
        public Button BtnAdd => _btnAdd;
        public Button BtnEdit => _btnEdit;
        public Button BtnDelete => _btnDelete;
        public Button BtnSave => _btnSave;
        public Button BtnCancel => _btnCancel;
        public GroupBox GrpDetails => _grpDetails;

        public FrmTicketTier()
        {
            _tierService = new TicketTierService();
            _ticketTiers = GetInitialSeedData();

            InitializeComponent();
            LoadDataToGrid(_ticketTiers);
            ApplyMode(FormMode.View);
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.Size = new Size(1140, 720);
            this.MinimumSize = new Size(1020, 640);

            _errorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            // 1. Header
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = ColorPrimary,
                Padding = new Padding(20, 0, 0, 0)
            };
            _lblTitle = new Label
            {
                Text = "QUẢN LÝ HẠNG VÉ & KHO VÉ",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _pnlHeader.Controls.Add(_lblTitle);

            // 2. Search & Filter (Khoảng cách thông thoáng, không bị che mất viền ô nhập)
            _pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = ColorSurface,
                Padding = new Padding(15, 10, 15, 10)
            };

            var lblEvent = new Label { Text = "Sự kiện:", AutoSize = true, Location = new Point(15, 15) };
            _cboEventFilter = new ComboBox { Location = new Point(78, 12), Width = 230, DropDownStyle = ComboBoxStyle.DropDownList };
            _cboEventFilter.Items.AddRange(new object[] { "Tất cả sự kiện", "EVT202601 (Vietnam Tech Summit)", "EVT202602 (Workshop GenAI)" });
            _cboEventFilter.SelectedIndex = 0;
            _cboEventFilter.SelectedIndexChanged += (s, e) => DoFilter();

            var lblSearch = new Label { Text = "Tên vé:", AutoSize = true, Location = new Point(328, 15) };
            _txtSearch = new TextBox { Location = new Point(388, 12), Width = 180, PlaceholderText = "Tìm tên hạng vé..." };
            _txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) DoFilter(); };

            _btnSearch = CreateStyledButton("Lọc vé", ColorPrimaryLight, Color.White, 80, 28);
            _btnSearch.Location = new Point(582, 11);
            _btnSearch.Click += (s, e) => DoFilter();

            _btnResetSearch = CreateStyledButton("Làm mới", Color.FromArgb(241, 245, 249), ColorText, 85, 28);
            _btnResetSearch.Location = new Point(672, 11);
            _btnResetSearch.Click += (s, e) =>
            {
                _cboEventFilter.SelectedIndex = 0;
                _txtSearch.Clear();
                LoadDataToGrid(_ticketTiers);
            };

            _pnlSearch.Controls.AddRange(new Control[] { lblEvent, _cboEventFilter, lblSearch, _txtSearch, _btnSearch, _btnResetSearch });

            // 3. DataGridView
            _dgvTiers = new DataGridView { Dock = DockStyle.Fill, AutoGenerateColumns = false };
            StyleDataGridView(_dgvTiers);
            SetupColumns();
            _dgvTiers.SelectionChanged += DgvTiers_SelectionChanged;
            _dgvTiers.CellFormatting += DgvTiers_CellFormatting;

            // 4. Details GroupBox
            _grpDetails = new GroupBox
            {
                Text = "Thông tin hạng vé & Thiết lập số lượng phát hành (BR-08, BR-09)",
                Dock = DockStyle.Bottom,
                Height = 175,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(15)
            };

            var lblMa = new Label { Text = "Mã hạng vé:", Location = new Point(20, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtMa = new TextBox { Location = new Point(115, 25), Width = 110, ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };

            var lblEventCode = new Label { Text = "Mã sự kiện (*):", Location = new Point(245, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtMaSuKien = new TextBox { Location = new Point(340, 25), Width = 120 };

            var lblTen = new Label { Text = "Tên hạng vé (*):", Location = new Point(480, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtTen = new TextBox { Location = new Point(590, 25), Width = 210 };

            var lblGia = new Label { Text = "Giá vé (VNĐ):", Location = new Point(820, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _nudGiaVe = new NumericUpDown { Location = new Point(910, 25), Width = 110, Maximum = 100000000, Increment = 50000 };

            var lblPhatHanh = new Label { Text = "Số phát hành (*):", Location = new Point(20, 68), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _nudPhatHanh = new NumericUpDown { Location = new Point(125, 65), Width = 100, Maximum = 100000, Minimum = 1, Value = 100 };

            var lblDaBan = new Label { Text = "Đã bán ra:", Location = new Point(245, 68), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtDaBan = new TextBox { Location = new Point(320, 65), Width = 80, ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };

            var lblConLai = new Label { Text = "Còn khả dụng:", Location = new Point(420, 68), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtConLai = new TextBox { Location = new Point(515, 65), Width = 80, ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };

            var lblMoBan = new Label { Text = "Mở bán từ:", Location = new Point(20, 108), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _dtpMoBan = new DateTimePicker { Location = new Point(95, 105), Width = 145, Format = DateTimePickerFormat.Short };

            var lblDongBan = new Label { Text = "Đến hết ngày:", Location = new Point(260, 108), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _dtpDongBan = new DateTimePicker { Location = new Point(350, 105), Width = 145, Format = DateTimePickerFormat.Short };

            _grpDetails.Controls.AddRange(new Control[] {
                lblMa, _txtMa, lblEventCode, _txtMaSuKien, lblTen, _txtTen, lblGia, _nudGiaVe,
                lblPhatHanh, _nudPhatHanh, lblDaBan, _txtDaBan, lblConLai, _txtConLai,
                lblMoBan, _dtpMoBan, lblDongBan, _dtpDongBan
            });

            // 5. Buttons Panel (Chuẩn hóa Layout Mẫu 2: Nút CRUD bên trái, Lưu & Hủy bên phải)
            _pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                BackColor = ColorSurface,
                Padding = new Padding(20, 10, 20, 10)
            };

            _btnAdd = CreateStyledButton("Thêm mới", ColorSuccess, Color.White, 95, 32);
            _btnAdd.Location = new Point(20, 11);
            _btnAdd.Click += BtnAdd_Click;

            _btnEdit = CreateStyledButton("Chỉnh sửa", ColorPrimaryLight, Color.White, 95, 32);
            _btnEdit.Location = new Point(125, 11);
            _btnEdit.Click += BtnEdit_Click;

            _btnDelete = CreateStyledButton("Xóa hạng vé", ColorDanger, Color.White, 105, 32);
            _btnDelete.Location = new Point(230, 11);
            _btnDelete.Click += BtnDelete_Click;

            _btnSave = CreateStyledButton("Lưu dữ liệu", ColorPrimary, Color.White, 115, 32);
            _btnSave.Location = new Point(780, 11);
            _btnSave.Click += BtnSave_Click;

            _btnCancel = CreateStyledButton("Hủy bỏ", Color.FromArgb(203, 213, 225), ColorText, 95, 32);
            _btnCancel.Location = new Point(905, 11);
            _btnCancel.Click += BtnCancel_Click;

            // Định vị lại nút Lưu và Hủy luôn bám sát lề phải panel bất kể kích thước màn hình
            _pnlButtons.Resize += (s, e) =>
            {
                int rightEdge = _pnlButtons.ClientSize.Width - 20;
                _btnCancel.Location = new Point(rightEdge - _btnCancel.Width, 11);
                _btnSave.Location = new Point(_btnCancel.Left - _btnSave.Width - 10, 11);
            };

            _pnlButtons.Controls.AddRange(new Control[] { _btnAdd, _btnEdit, _btnDelete, _btnSave, _btnCancel });

            // Layout
            var centerPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15, 5, 15, 5) };
            centerPanel.Controls.Add(_dgvTiers);

            this.Controls.Add(centerPanel);
            this.Controls.Add(_grpDetails);
            this.Controls.Add(_pnlButtons);
            this.Controls.Add(_pnlSearch);
            this.Controls.Add(_pnlHeader);
        }

        private void SetupColumns()
        {
            _dgvTiers.Columns.Clear();
            _dgvTiers.EnableHeadersVisualStyles = false;
            _dgvTiers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            var colMa = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaHangVe",
                HeaderText = "Mã vé",
                Width = 75,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            var colMaSK = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSuKien",
                HeaderText = "Mã sự kiện",
                Width = 90,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            var colTen = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenHangVe",
                HeaderText = "Tên hạng vé",
                Width = 200,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };

            var colGia = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaVe",
                HeaderText = "Đơn giá (VNĐ)",
                Width = 115,
                DefaultCellStyle = { Format = "#,##0", Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            var colPhatHanh = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuongPhatHanh",
                HeaderText = "Số vé phát hành",
                Width = 120,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            var colDaBan = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuongDaBan",
                HeaderText = "Số vé đã bán",
                Width = 110,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            var colConLai = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuongConLai",
                HeaderText = "Số vé còn lại",
                Width = 110,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            var colMoBan = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThoiGianMoBan",
                HeaderText = "Mở bán",
                Width = 95,
                DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            var colDongBan = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThoiGianDongBan",
                HeaderText = "Đóng bán",
                Width = 95,
                DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            var colTrangThai = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThaiKho",
                HeaderText = "Trạng thái kho",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

            _dgvTiers.Columns.AddRange(colMa, colMaSK, colTen, colGia, colPhatHanh, colDaBan, colConLai, colMoBan, colDongBan, colTrangThai);

            foreach (DataGridViewColumn col in _dgvTiers.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void DgvTiers_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _dgvTiers.Rows.Count) return;

            string colName = _dgvTiers.Columns[e.ColumnIndex].DataPropertyName;
            if (colName == "TrangThaiKho" || colName == "TrangThai")
            {
                string status = e.Value?.ToString() ?? string.Empty;
                switch (status)
                {
                    case "Đang mở bán":
                        e.CellStyle!.ForeColor = Color.FromArgb(22, 163, 74); // Xanh lá
                        e.CellStyle.Font = new Font(_dgvTiers.Font, FontStyle.Bold);
                        break;
                    case "Sắp hết vé":
                        e.CellStyle!.ForeColor = Color.FromArgb(234, 88, 12); // Cam
                        e.CellStyle.Font = new Font(_dgvTiers.Font, FontStyle.Bold);
                        break;
                    case "Hết vé":
                        e.CellStyle!.ForeColor = Color.FromArgb(220, 38, 38); // Đỏ
                        e.CellStyle.Font = new Font(_dgvTiers.Font, FontStyle.Bold);
                        break;
                    case "Đã đóng bán":
                        e.CellStyle!.ForeColor = Color.FromArgb(100, 116, 139); // Xám
                        break;
                    case "Chưa mở bán":
                        e.CellStyle!.ForeColor = Color.FromArgb(37, 99, 235); // Xanh dương
                        break;
                }
            }
        }

        private static int GetTierStatusPriority(HangVe t)
        {
            return t.TrangThaiKho switch
            {
                "Đang mở bán" => 1,
                "Sắp hết vé" => 1,
                "Chưa mở bán" => 2,
                "Hết vé" => 3,
                "Đã đóng bán" => 4,
                _ => 5
            };
        }

        private void LoadDataToGrid(IEnumerable<HangVe> data)
        {
            var sorted = data
                .OrderBy(GetTierStatusPriority)
                .ThenBy(t => t.MaSuKien)
                .ThenBy(t => t.MaHangVe)
                .ToList();

            _dgvTiers.DataSource = null;
            _dgvTiers.DataSource = sorted;
            if (_dgvTiers.Rows.Count > 0)
            {
                _dgvTiers.Rows[0].Selected = true;
                BindRowToDetails((HangVe)_dgvTiers.Rows[0].DataBoundItem);
            }
            else
            {
                ClearDetails();
            }

            ApplyMode(_currentMode);
        }

        private void DgvTiers_SelectionChanged(object? sender, EventArgs e)
        {
            if (_currentMode == FormMode.View)
            {
                if (_dgvTiers.CurrentRow?.DataBoundItem is HangVe item)
                {
                    BindRowToDetails(item);
                    _btnEdit.Enabled = true;
                    _btnDelete.Enabled = true;
                }
                else
                {
                    ClearDetails();
                    _btnEdit.Enabled = false;
                    _btnDelete.Enabled = false;
                }
            }
        }

        private void BindRowToDetails(HangVe item)
        {
            _txtMa.Text = item.MaHangVe;
            _txtMaSuKien.Text = item.MaSuKien;
            _txtTen.Text = item.TenHangVe;
            _nudGiaVe.Value = Math.Min(item.GiaVe, _nudGiaVe.Maximum);
            _nudPhatHanh.Value = Math.Min(item.SoLuongPhatHanh, _nudPhatHanh.Maximum);
            _txtDaBan.Text = item.SoLuongDaBan.ToString();
            _txtConLai.Text = item.SoLuongConLai.ToString();
            _dtpMoBan.Value = item.ThoiGianMoBan ?? DateTime.Now.Date;
            _dtpDongBan.Value = item.ThoiGianDongBan ?? DateTime.Now.Date.AddMonths(1);
        }

        private void ClearDetails()
        {
            _txtMa.Clear();
            _txtMaSuKien.Text = "EVT202602";
            _txtTen.Clear();
            _nudGiaVe.Value = 0;
            _nudPhatHanh.Value = 100;
            _txtDaBan.Text = "0";
            _txtConLai.Text = "100";
            _dtpMoBan.Value = DateTime.Now.Date;
            _dtpDongBan.Value = DateTime.Now.Date.AddDays(30);
            _errorProvider.Clear();
        }

        private void ApplyMode(FormMode mode)
        {
            _currentMode = mode;
            bool isEditing = (mode == FormMode.Add || mode == FormMode.Edit);
            bool hasSelection = _dgvTiers.CurrentRow != null && _dgvTiers.CurrentRow.DataBoundItem is HangVe;

            _txtMaSuKien.ReadOnly = !isEditing;
            _txtTen.ReadOnly = !isEditing;
            _nudGiaVe.Enabled = isEditing;
            _nudPhatHanh.Enabled = isEditing;
            _dtpMoBan.Enabled = isEditing;
            _dtpDongBan.Enabled = isEditing;
            _dgvTiers.Enabled = !isEditing;

            _btnAdd.Enabled = !isEditing;
            _btnEdit.Enabled = !isEditing && hasSelection;
            _btnDelete.Enabled = !isEditing && hasSelection;

            _btnSave.Enabled = isEditing;
            _btnCancel.Enabled = isEditing;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            ClearDetails();
            _txtMa.Text = $"HV{(_ticketTiers.Count + 1):D3}";
            ApplyMode(FormMode.Add);
            _txtTen.Focus();
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (_dgvTiers.CurrentRow == null) return;
            ApplyMode(FormMode.Edit);
            _txtTen.Focus();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_dgvTiers.CurrentRow?.DataBoundItem is HangVe item)
            {
                if (item.SoLuongDaBan > 0)
                {
                    MessageBox.Show(
                        $"Không thể xóa hạng vé '{item.TenHangVe}' vì đã có {item.SoLuongDaBan} vé được bán ra.",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa hạng vé '{item.TenHangVe}' ({item.MaHangVe}) không?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    _ticketTiers.Remove(item);
                    LoadDataToGrid(_ticketTiers);
                    MessageBox.Show("Đã xóa hạng vé thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            _errorProvider.Clear();

            var tier = new HangVe
            {
                MaHangVe = _txtMa.Text.Trim(),
                MaSuKien = _txtMaSuKien.Text.Trim(),
                TenHangVe = _txtTen.Text.Trim(),
                GiaVe = _nudGiaVe.Value,
                SoLuongPhatHanh = (int)_nudPhatHanh.Value,
                ThoiGianMoBan = _dtpMoBan.Value,
                ThoiGianDongBan = _dtpDongBan.Value
            };

            // Thẩm định cơ bản
            var validation = _tierService.ValidateTicketTierInfo(tier);
            if (!validation.IsSuccess)
            {
                MessageBox.Show(validation.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_currentMode == FormMode.Add)
            {
                tier.SoLuongDaBan = 0;
                _ticketTiers.Add(tier);
                MessageBox.Show("Thêm mới hạng vé thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (_currentMode == FormMode.Edit)
            {
                var existing = _ticketTiers.FirstOrDefault(t => t.MaHangVe == tier.MaHangVe);
                if (existing != null)
                {
                    // Kiểm tra quy tắc BR-08: Không giảm phát hành < đã bán
                    var checkBr08 = _tierService.ValidateTierCapacityUpdate(existing, tier.SoLuongPhatHanh);
                    if (!checkBr08.IsSuccess)
                    {
                        _errorProvider.SetError(_nudPhatHanh, checkBr08.Message);
                        MessageBox.Show(checkBr08.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    existing.MaSuKien = tier.MaSuKien;
                    existing.TenHangVe = tier.TenHangVe;
                    existing.GiaVe = tier.GiaVe;
                    existing.SoLuongPhatHanh = tier.SoLuongPhatHanh;
                    existing.ThoiGianMoBan = tier.ThoiGianMoBan;
                    existing.ThoiGianDongBan = tier.ThoiGianDongBan;
                }
                MessageBox.Show("Cập nhật hạng vé thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ApplyMode(FormMode.View);
            LoadDataToGrid(_ticketTiers);
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            _errorProvider.Clear();
            ApplyMode(FormMode.View);
            if (_dgvTiers.CurrentRow?.DataBoundItem is HangVe item)
                BindRowToDetails(item);
            else
                ClearDetails();
        }

        private void DoFilter()
        {
            var query = _ticketTiers.AsEnumerable();

            if (_cboEventFilter.SelectedIndex == 1)
                query = query.Where(t => t.MaSuKien == "EVT202601");
            else if (_cboEventFilter.SelectedIndex == 2)
                query = query.Where(t => t.MaSuKien == "EVT202602");

            if (!string.IsNullOrWhiteSpace(_txtSearch.Text))
            {
                var kw = _txtSearch.Text.Trim().ToLower();
                query = query.Where(t => t.TenHangVe.ToLower().Contains(kw) || t.MaHangVe.ToLower().Contains(kw));
            }

            LoadDataToGrid(query);
        }

        private List<HangVe> GetInitialSeedData()
        {
            return new List<HangVe>
            {
                new HangVe { MaHangVe = "HV001", MaSuKien = "EVT202601", TenHangVe = "Standard", GiaVe = 200000, SoLuongPhatHanh = 800, SoLuongDaBan = 159, ThoiGianMoBan = new DateTime(2026, 11, 1), ThoiGianDongBan = new DateTime(2026, 12, 14) },
                new HangVe { MaHangVe = "HV002", MaSuKien = "EVT202601", TenHangVe = "VIP", GiaVe = 800000, SoLuongPhatHanh = 200, SoLuongDaBan = 50, ThoiGianMoBan = new DateTime(2026, 11, 1), ThoiGianDongBan = new DateTime(2026, 12, 14) },
                new HangVe { MaHangVe = "HV005", MaSuKien = "EVT202601", TenHangVe = "Early Bird", GiaVe = 150000, SoLuongPhatHanh = 150, SoLuongDaBan = 150, ThoiGianMoBan = new DateTime(2026, 10, 15), ThoiGianDongBan = new DateTime(2026, 10, 31) },
                new HangVe { MaHangVe = "HV003", MaSuKien = "EVT202602", TenHangVe = "Standard", GiaVe = 300000, SoLuongPhatHanh = 100, SoLuongDaBan = 98, ThoiGianMoBan = new DateTime(2026, 10, 1), ThoiGianDongBan = new DateTime(2026, 11, 2) },
                new HangVe { MaHangVe = "HV004", MaSuKien = "EVT202602", TenHangVe = "VIP", GiaVe = 600000, SoLuongPhatHanh = 50, SoLuongDaBan = 10, ThoiGianMoBan = new DateTime(2026, 10, 1), ThoiGianDongBan = new DateTime(2026, 11, 2) },
                new HangVe { MaHangVe = "HV006", MaSuKien = "EVT202602", TenHangVe = "Early Bird", GiaVe = 250000, SoLuongPhatHanh = 50, SoLuongDaBan = 50, ThoiGianMoBan = new DateTime(2026, 9, 15), ThoiGianDongBan = new DateTime(2026, 9, 30) }
            };
        }
    }
}
