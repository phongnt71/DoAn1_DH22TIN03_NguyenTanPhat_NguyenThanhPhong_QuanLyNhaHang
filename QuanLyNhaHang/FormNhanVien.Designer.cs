namespace QuanLyNhaHang
{
    partial class FormNhanVien
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
            txtTenNhanVien = new TextBox();
            label2 = new Label();
            txtIDNhanVien = new TextBox();
            label4 = new Label();
            txtMatKhau = new TextBox();
            label1 = new Label();
            txtTaiKhoan = new TextBox();
            label3 = new Label();
            cmbQuyen = new ComboBox();
            label5 = new Label();
            dtgvNhanVien = new DataGridView();
            btnHuy = new Button();
            btnLuu = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            txtDiaChi = new TextBox();
            label6 = new Label();
            txtEmail = new TextBox();
            label7 = new Label();
            txtSoDienThoai = new TextBox();
            label8 = new Label();
            cmbGioiTinh = new ComboBox();
            label9 = new Label();
            dtpNgaySinh = new DateTimePicker();
            label10 = new Label();
            ((System.ComponentModel.ISupportInitialize)dtgvNhanVien).BeginInit();
            SuspendLayout();
            // 
            // txtTenNhanVien
            // 
            txtTenNhanVien.Location = new Point(117, 45);
            txtTenNhanVien.Name = "txtTenNhanVien";
            txtTenNhanVien.Size = new Size(220, 27);
            txtTenNhanVien.TabIndex = 61;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 52);
            label2.Name = "label2";
            label2.Size = new Size(99, 20);
            label2.TabIndex = 60;
            label2.Text = "Tên nhân viên";
            // 
            // txtIDNhanVien
            // 
            txtIDNhanVien.Location = new Point(117, 12);
            txtIDNhanVien.Name = "txtIDNhanVien";
            txtIDNhanVien.ReadOnly = true;
            txtIDNhanVien.Size = new Size(220, 27);
            txtIDNhanVien.TabIndex = 59;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 19);
            label4.Name = "label4";
            label4.Size = new Size(97, 20);
            label4.TabIndex = 58;
            label4.Text = "Mã nhân viên";
            // 
            // txtMatKhau
            // 
            txtMatKhau.Location = new Point(117, 111);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(220, 27);
            txtMatKhau.TabIndex = 65;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 118);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 64;
            label1.Text = "Mật khẩu";
            // 
            // txtTaiKhoan
            // 
            txtTaiKhoan.Location = new Point(117, 78);
            txtTaiKhoan.Name = "txtTaiKhoan";
            txtTaiKhoan.Size = new Size(220, 27);
            txtTaiKhoan.TabIndex = 63;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 85);
            label3.Name = "label3";
            label3.Size = new Size(71, 20);
            label3.TabIndex = 62;
            label3.Text = "Tài khoản";
            // 
            // cmbQuyen
            // 
            cmbQuyen.FormattingEnabled = true;
            cmbQuyen.Location = new Point(117, 144);
            cmbQuyen.Name = "cmbQuyen";
            cmbQuyen.Size = new Size(139, 28);
            cmbQuyen.TabIndex = 67;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 152);
            label5.Name = "label5";
            label5.Size = new Size(51, 20);
            label5.TabIndex = 66;
            label5.Text = "Quyền";
            // 
            // dtgvNhanVien
            // 
            dtgvNhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvNhanVien.Location = new Point(12, 178);
            dtgvNhanVien.Name = "dtgvNhanVien";
            dtgvNhanVien.RowHeadersWidth = 51;
            dtgvNhanVien.Size = new Size(787, 225);
            dtgvNhanVien.TabIndex = 68;
            // 
            // btnHuy
            // 
            btnHuy.Location = new Point(414, 409);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 73;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            btnHuy.Click += btnHuy_Click;
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(314, 409);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(94, 29);
            btnLuu.TabIndex = 72;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            btnLuu.Click += btnLuu_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(214, 409);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 71;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(114, 409);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 70;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(14, 409);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 69;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // txtDiaChi
            // 
            txtDiaChi.Location = new Point(478, 78);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(220, 27);
            txtDiaChi.TabIndex = 79;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(375, 85);
            label6.Name = "label6";
            label6.Size = new Size(55, 20);
            label6.TabIndex = 78;
            label6.Text = "Địa chỉ";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(478, 45);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(220, 27);
            txtEmail.TabIndex = 77;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(375, 52);
            label7.Name = "label7";
            label7.Size = new Size(46, 20);
            label7.TabIndex = 76;
            label7.Text = "Email";
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Location = new Point(478, 12);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Size = new Size(220, 27);
            txtSoDienThoai.TabIndex = 75;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(375, 19);
            label8.Name = "label8";
            label8.Size = new Size(97, 20);
            label8.TabIndex = 74;
            label8.Text = "Số điện thoại";
            // 
            // cmbGioiTinh
            // 
            cmbGioiTinh.FormattingEnabled = true;
            cmbGioiTinh.Location = new Point(478, 111);
            cmbGioiTinh.Name = "cmbGioiTinh";
            cmbGioiTinh.Size = new Size(113, 28);
            cmbGioiTinh.TabIndex = 81;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(375, 119);
            label9.Name = "label9";
            label9.Size = new Size(65, 20);
            label9.TabIndex = 80;
            label9.Text = "Giới tính";
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.Location = new Point(478, 142);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(220, 27);
            dtpNgaySinh.TabIndex = 83;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(378, 149);
            label10.Name = "label10";
            label10.Size = new Size(74, 20);
            label10.TabIndex = 82;
            label10.Text = "Ngày sinh";
            // 
            // FormNhanVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dtpNgaySinh);
            Controls.Add(label10);
            Controls.Add(cmbGioiTinh);
            Controls.Add(label9);
            Controls.Add(txtDiaChi);
            Controls.Add(label6);
            Controls.Add(txtEmail);
            Controls.Add(label7);
            Controls.Add(txtSoDienThoai);
            Controls.Add(label8);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(dtgvNhanVien);
            Controls.Add(cmbQuyen);
            Controls.Add(label5);
            Controls.Add(txtMatKhau);
            Controls.Add(label1);
            Controls.Add(txtTaiKhoan);
            Controls.Add(label3);
            Controls.Add(txtTenNhanVien);
            Controls.Add(label2);
            Controls.Add(txtIDNhanVien);
            Controls.Add(label4);
            Name = "FormNhanVien";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormNhanVien";
            Load += FormNhanVien_Load;
            ((System.ComponentModel.ISupportInitialize)dtgvNhanVien).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTenNhanVien;
        private Label label2;
        private TextBox txtIDNhanVien;
        private Label label4;
        private TextBox txtMatKhau;
        private Label label1;
        private TextBox txtTaiKhoan;
        private Label label3;
        private ComboBox cmbQuyen;
        private Label label5;
        private DataGridView dtgvNhanVien;
        private Button btnHuy;
        private Button btnLuu;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
        private TextBox txtDiaChi;
        private Label label6;
        private TextBox txtEmail;
        private Label label7;
        private TextBox txtSoDienThoai;
        private Label label8;
        private ComboBox cmbGioiTinh;
        private Label label9;
        private DateTimePicker dtpNgaySinh;
        private Label label10;
    }
}