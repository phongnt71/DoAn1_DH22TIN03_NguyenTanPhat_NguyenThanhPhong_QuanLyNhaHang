namespace QuanLyNhaHang
{
    partial class FormMonAn
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dtgvMonAn = new DataGridView();
            btnSua = new Button();
            btnThem = new Button();
            btnLuu = new Button();
            btnXoa = new Button();
            btnHuy = new Button();
            label3 = new Label();
            cmbLoaiMon = new ComboBox();
            txtMoTa = new TextBox();
            label4 = new Label();
            txtTenMon = new TextBox();
            label1 = new Label();
            txtGiaTien = new TextBox();
            label2 = new Label();
            dtgvNguyenLieu = new DataGridView();
            btnXoaNguyenLieu = new Button();
            btnThemNguyenLieu = new Button();
            nudSoLuongTon = new NumericUpDown();
            cmbNguyenLieu = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dtgvMonAn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtgvNguyenLieu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSoLuongTon).BeginInit();
            SuspendLayout();
            // 
            // dtgvMonAn
            // 
            dtgvMonAn.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvMonAn.Location = new Point(12, 12);
            dtgvMonAn.Name = "dtgvMonAn";
            dtgvMonAn.RowHeadersWidth = 51;
            dtgvMonAn.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvMonAn.Size = new Size(776, 158);
            dtgvMonAn.TabIndex = 0;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(112, 409);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 11;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(12, 409);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 10;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(312, 409);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(94, 29);
            btnLuu.TabIndex = 13;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(212, 409);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 12;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnHuy
            // 
            btnHuy.Location = new Point(412, 409);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 14;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(328, 303);
            label3.Name = "label3";
            label3.Size = new Size(71, 20);
            label3.TabIndex = 19;
            label3.Text = "Loại món";
            // 
            // cmbLoaiMon
            // 
            cmbLoaiMon.FormattingEnabled = true;
            cmbLoaiMon.Location = new Point(412, 295);
            cmbLoaiMon.Name = "cmbLoaiMon";
            cmbLoaiMon.Size = new Size(151, 28);
            cmbLoaiMon.TabIndex = 18;
            cmbLoaiMon.SelectedIndexChanged += cmbLoaiMon_SelectedIndexChanged;
            // 
            // txtMoTa
            // 
            txtMoTa.Location = new Point(94, 328);
            txtMoTa.Multiline = true;
            txtMoTa.Name = "txtMoTa";
            txtMoTa.Size = new Size(224, 58);
            txtMoTa.TabIndex = 17;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 335);
            label4.Name = "label4";
            label4.Size = new Size(48, 20);
            label4.TabIndex = 16;
            label4.Text = "Mô tả";
            // 
            // txtTenMon
            // 
            txtTenMon.Location = new Point(94, 295);
            txtTenMon.Name = "txtTenMon";
            txtTenMon.Size = new Size(224, 27);
            txtTenMon.TabIndex = 21;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 302);
            label1.Name = "label1";
            label1.Size = new Size(66, 20);
            label1.TabIndex = 20;
            label1.Text = "Tên món";
            // 
            // txtGiaTien
            // 
            txtGiaTien.Location = new Point(412, 329);
            txtGiaTien.Name = "txtGiaTien";
            txtGiaTien.Size = new Size(151, 27);
            txtGiaTien.TabIndex = 23;
            txtGiaTien.TextChanged += txtGiaTien_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(328, 336);
            label2.Name = "label2";
            label2.Size = new Size(60, 20);
            label2.TabIndex = 22;
            label2.Text = "Giá tiền";
            // 
            // dtgvNguyenLieu
            // 
            dtgvNguyenLieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvNguyenLieu.Location = new Point(10, 176);
            dtgvNguyenLieu.Name = "dtgvNguyenLieu";
            dtgvNguyenLieu.RowHeadersWidth = 51;
            dtgvNguyenLieu.Size = new Size(596, 113);
            dtgvNguyenLieu.TabIndex = 24;
            // 
            // btnXoaNguyenLieu
            // 
            btnXoaNguyenLieu.Location = new Point(612, 281);
            btnXoaNguyenLieu.Name = "btnXoaNguyenLieu";
            btnXoaNguyenLieu.Size = new Size(151, 31);
            btnXoaNguyenLieu.TabIndex = 26;
            btnXoaNguyenLieu.Text = "Xóa nguyên liệu";
            btnXoaNguyenLieu.UseVisualStyleBackColor = true;
            btnXoaNguyenLieu.Click += btnXoaNguyenLieu_Click;
            // 
            // btnThemNguyenLieu
            // 
            btnThemNguyenLieu.Location = new Point(613, 243);
            btnThemNguyenLieu.Name = "btnThemNguyenLieu";
            btnThemNguyenLieu.Size = new Size(150, 32);
            btnThemNguyenLieu.TabIndex = 25;
            btnThemNguyenLieu.Text = "Thêm Nguyên Liệu";
            btnThemNguyenLieu.UseVisualStyleBackColor = true;
            btnThemNguyenLieu.Click += btnThemNguyenLieu_Click;
            // 
            // nudSoLuongTon
            // 
            nudSoLuongTon.Location = new Point(613, 210);
            nudSoLuongTon.Name = "nudSoLuongTon";
            nudSoLuongTon.Size = new Size(150, 27);
            nudSoLuongTon.TabIndex = 27;
            // 
            // cmbNguyenLieu
            // 
            cmbNguyenLieu.FormattingEnabled = true;
            cmbNguyenLieu.Location = new Point(612, 176);
            cmbNguyenLieu.Name = "cmbNguyenLieu";
            cmbNguyenLieu.Size = new Size(151, 28);
            cmbNguyenLieu.TabIndex = 28;
            // 
            // FormMonAn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cmbNguyenLieu);
            Controls.Add(nudSoLuongTon);
            Controls.Add(btnXoaNguyenLieu);
            Controls.Add(btnThemNguyenLieu);
            Controls.Add(dtgvNguyenLieu);
            Controls.Add(txtGiaTien);
            Controls.Add(label2);
            Controls.Add(txtTenMon);
            Controls.Add(label1);
            Controls.Add(label3);
            Controls.Add(cmbLoaiMon);
            Controls.Add(txtMoTa);
            Controls.Add(label4);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(dtgvMonAn);
            Name = "FormMonAn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormMonAn";
            Load += FormMonAn_Load;
            ((System.ComponentModel.ISupportInitialize)dtgvMonAn).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtgvNguyenLieu).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSoLuongTon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dtgvMonAn;
        private Button btnSua;
        private Button btnThem;
        private Button btnLuu;
        private Button btnXoa;
        private Button btnHuy;
        private Label label3;
        private ComboBox cmbLoaiMon;
        private TextBox txtMoTa;
        private Label label4;
        private TextBox txtTenMon;
        private Label label1;
        private TextBox txtGiaTien;
        private Label label2;
        private DataGridView dtgvNguyenLieu;
        private Button btnXoaNguyenLieu;
        private Button btnThemNguyenLieu;
        private NumericUpDown nudSoLuongTon;
        private ComboBox cmbNguyenLieu;
    }
}