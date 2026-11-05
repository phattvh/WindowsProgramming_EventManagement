namespace EventManagement.GUI.Forms
{
    partial class FrmRolePermission
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTopFilter = new System.Windows.Forms.Panel();
            this.btnXoaNhom = new System.Windows.Forms.Button();
            this.btnThemNhom = new System.Windows.Forms.Button();
            this.cboNhomQuyen = new System.Windows.Forms.ComboBox();
            this.lblChonNhom = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblGhiChuAdmin = new System.Windows.Forms.Label();
            this.btnDong = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnBoChonTatCa = new System.Windows.Forms.Button();
            this.btnChonTatCa = new System.Windows.Forms.Button();
            this.dgvPhanQuyen = new System.Windows.Forms.DataGridView();
            this.colMaChucNang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenChucNang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaForm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCoQuyen = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlHeader.SuspendLayout();
            this.pnlTopFilter.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhanQuyen)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlHeader.Controls.Add(this.lblSubTitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(834, 60);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSubTitle.Location = new System.Drawing.Point(16, 34);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(437, 15);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Quản lý danh mục vai trò và thiết lập ma trận phân quyền truy cập chức năng động";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTitle.Location = new System.Drawing.Point(14, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(374, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "F20 – QUẢN LÝ NHÓM QUYỀN & PHÂN QUYỀN ĐỘNG";
            // 
            // pnlTopFilter
            // 
            this.pnlTopFilter.BackColor = System.Drawing.Color.White;
            this.pnlTopFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTopFilter.Controls.Add(this.btnXoaNhom);
            this.pnlTopFilter.Controls.Add(this.btnThemNhom);
            this.pnlTopFilter.Controls.Add(this.cboNhomQuyen);
            this.pnlTopFilter.Controls.Add(this.lblChonNhom);
            this.pnlTopFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopFilter.Location = new System.Drawing.Point(0, 60);
            this.pnlTopFilter.Name = "pnlTopFilter";
            this.pnlTopFilter.Size = new System.Drawing.Size(834, 52);
            this.pnlTopFilter.TabIndex = 1;
            // 
            // btnXoaNhom
            // 
            this.btnXoaNhom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnXoaNhom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoaNhom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaNhom.ForeColor = System.Drawing.Color.White;
            this.btnXoaNhom.Location = new System.Drawing.Point(520, 11);
            this.btnXoaNhom.Name = "btnXoaNhom";
            this.btnXoaNhom.Size = new System.Drawing.Size(100, 28);
            this.btnXoaNhom.TabIndex = 3;
            this.btnXoaNhom.Text = "🗑️ Xóa vai trò";
            this.btnXoaNhom.UseVisualStyleBackColor = false;
            this.btnXoaNhom.Click += new System.EventHandler(this.btnXoaNhom_Click);
            // 
            // btnThemNhom
            // 
            this.btnThemNhom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnThemNhom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemNhom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemNhom.ForeColor = System.Drawing.Color.White;
            this.btnThemNhom.Location = new System.Drawing.Point(395, 11);
            this.btnThemNhom.Name = "btnThemNhom";
            this.btnThemNhom.Size = new System.Drawing.Size(115, 28);
            this.btnThemNhom.TabIndex = 2;
            this.btnThemNhom.Text = "➕ Thêm vai trò";
            this.btnThemNhom.UseVisualStyleBackColor = false;
            this.btnThemNhom.Click += new System.EventHandler(this.btnThemNhom_Click);
            // 
            // cboNhomQuyen
            // 
            this.cboNhomQuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhomQuyen.FormattingEnabled = true;
            this.cboNhomQuyen.Location = new System.Drawing.Point(155, 14);
            this.cboNhomQuyen.Name = "cboNhomQuyen";
            this.cboNhomQuyen.Size = new System.Drawing.Size(220, 23);
            this.cboNhomQuyen.TabIndex = 1;
            this.cboNhomQuyen.SelectedIndexChanged += new System.EventHandler(this.cboNhomQuyen_SelectedIndexChanged);
            // 
            // lblChonNhom
            // 
            this.lblChonNhom.AutoSize = true;
            this.lblChonNhom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblChonNhom.Location = new System.Drawing.Point(16, 17);
            this.lblChonNhom.Name = "lblChonNhom";
            this.lblChonNhom.Size = new System.Drawing.Size(130, 15);
            this.lblChonNhom.TabIndex = 0;
            this.lblChonNhom.Text = "Chọn vai trò cấu hình:";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottom.Controls.Add(this.lblGhiChuAdmin);
            this.pnlBottom.Controls.Add(this.btnDong);
            this.pnlBottom.Controls.Add(this.btnLuu);
            this.pnlBottom.Controls.Add(this.btnBoChonTatCa);
            this.pnlBottom.Controls.Add(this.btnChonTatCa);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 501);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(834, 50);
            this.pnlBottom.TabIndex = 2;
            // 
            // lblGhiChuAdmin
            // 
            this.lblGhiChuAdmin.AutoSize = true;
            this.lblGhiChuAdmin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(83)))), ((int)(((byte)(9)))));
            this.lblGhiChuAdmin.Location = new System.Drawing.Point(230, 17);
            this.lblGhiChuAdmin.Name = "lblGhiChuAdmin";
            this.lblGhiChuAdmin.Size = new System.Drawing.Size(260, 15);
            this.lblGhiChuAdmin.TabIndex = 4;
            this.lblGhiChuAdmin.Text = "ℹ️ Quyền mới có hiệu lực ở phiên đăng nhập sau.";
            // 
            // btnDong
            // 
            this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDong.Location = new System.Drawing.Point(732, 10);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(88, 28);
            this.btnDong.TabIndex = 3;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(595, 10);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(130, 28);
            this.btnLuu.TabIndex = 2;
            this.btnLuu.Text = "💾 Lưu ma trận";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnBoChonTatCa
            // 
            this.btnBoChonTatCa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBoChonTatCa.Location = new System.Drawing.Point(115, 10);
            this.btnBoChonTatCa.Name = "btnBoChonTatCa";
            this.btnBoChonTatCa.Size = new System.Drawing.Size(95, 28);
            this.btnBoChonTatCa.TabIndex = 1;
            this.btnBoChonTatCa.Text = "Bỏ chọn tất cả";
            this.btnBoChonTatCa.UseVisualStyleBackColor = true;
            this.btnBoChonTatCa.Click += new System.EventHandler(this.btnBoChonTatCa_Click);
            // 
            // btnChonTatCa
            // 
            this.btnChonTatCa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChonTatCa.Location = new System.Drawing.Point(14, 10);
            this.btnChonTatCa.Name = "btnChonTatCa";
            this.btnChonTatCa.Size = new System.Drawing.Size(95, 28);
            this.btnChonTatCa.TabIndex = 0;
            this.btnChonTatCa.Text = "Chọn tất cả";
            this.btnChonTatCa.UseVisualStyleBackColor = true;
            this.btnChonTatCa.Click += new System.EventHandler(this.btnChonTatCa_Click);
            // 
            // dgvPhanQuyen
            // 
            this.dgvPhanQuyen.AllowUserToAddRows = false;
            this.dgvPhanQuyen.AllowUserToDeleteRows = false;
            this.dgvPhanQuyen.BackgroundColor = System.Drawing.Color.White;
            this.dgvPhanQuyen.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPhanQuyen.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPhanQuyen.ColumnHeadersHeight = 32;
            this.dgvPhanQuyen.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaChucNang,
            this.colTenChucNang,
            this.colMaForm,
            this.colCoQuyen});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPhanQuyen.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPhanQuyen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhanQuyen.EnableHeadersVisualStyles = false;
            this.dgvPhanQuyen.Location = new System.Drawing.Point(0, 112);
            this.dgvPhanQuyen.MultiSelect = false;
            this.dgvPhanQuyen.Name = "dgvPhanQuyen";
            this.dgvPhanQuyen.RowHeadersVisible = false;
            this.dgvPhanQuyen.RowTemplate.Height = 28;
            this.dgvPhanQuyen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhanQuyen.Size = new System.Drawing.Size(834, 389);
            this.dgvPhanQuyen.TabIndex = 3;
            // 
            // colMaChucNang
            // 
            this.colMaChucNang.DataPropertyName = "MaChucNang";
            this.colMaChucNang.HeaderText = "Mã Chức Năng";
            this.colMaChucNang.Name = "colMaChucNang";
            this.colMaChucNang.ReadOnly = true;
            this.colMaChucNang.Width = 120;
            // 
            // colTenChucNang
            // 
            this.colTenChucNang.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTenChucNang.DataPropertyName = "TenChucNang";
            this.colTenChucNang.HeaderText = "Tên Chức Năng / Nghiệp Vụ";
            this.colTenChucNang.Name = "colTenChucNang";
            this.colTenChucNang.ReadOnly = true;
            // 
            // colMaForm
            // 
            this.colMaForm.DataPropertyName = "MaForm";
            this.colMaForm.HeaderText = "Mã Form";
            this.colMaForm.Name = "colMaForm";
            this.colMaForm.ReadOnly = true;
            this.colMaForm.Width = 100;
            // 
            // colCoQuyen
            // 
            this.colCoQuyen.DataPropertyName = "CoQuyen";
            this.colCoQuyen.HeaderText = "Có Quyền Truy Cập";
            this.colCoQuyen.Name = "colCoQuyen";
            this.colCoQuyen.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCoQuyen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCoQuyen.Width = 140;
            // 
            // FrmRolePermission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 551);
            this.Controls.Add(this.dgvPhanQuyen);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTopFilter);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FrmRolePermission";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "F20 – Quản lý Nhóm Quyền & Ma Trận Phân Quyền";
            this.Load += new System.EventHandler(this.FrmRolePermission_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlTopFilter.ResumeLayout(false);
            this.pnlTopFilter.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhanQuyen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlTopFilter;
        private System.Windows.Forms.Label lblChonNhom;
        private System.Windows.Forms.ComboBox cboNhomQuyen;
        private System.Windows.Forms.Button btnThemNhom;
        private System.Windows.Forms.Button btnXoaNhom;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnChonTatCa;
        private System.Windows.Forms.Button btnBoChonTatCa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Label lblGhiChuAdmin;
        private System.Windows.Forms.DataGridView dgvPhanQuyen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaChucNang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenChucNang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaForm;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCoQuyen;
    }
}
