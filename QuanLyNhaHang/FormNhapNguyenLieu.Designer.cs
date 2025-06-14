namespace QuanLyNhaHang
{
    partial class FormNhapNguyenLieu
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
            txtGhiChu = new TextBox();
            label4 = new Label();
            txtDonViTinh = new TextBox();
            label3 = new Label();
            txtSoLuong = new TextBox();
            label2 = new Label();
            txtTenNguyenLieu = new TextBox();
            label1 = new Label();
            dtpNgayNhap = new DateTimePicker();
            label6 = new Label();
            txtGiaNhap = new TextBox();
            label5 = new Label();
            btnThem = new Button();
            dtgvNguyenLieu = new DataGridView();
            dtgvLichSuNhap = new DataGridView();
            btnXoa = new Button();
            btnSua = new Button();
            ((System.ComponentModel.ISupportInitialize)dtgvNguyenLieu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtgvLichSuNhap).BeginInit();
            SuspendLayout();
            // 
            // txtGhiChu
            // 
            txtGhiChu.Location = new Point(495, 12);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(224, 27);
            txtGhiChu.TabIndex = 37;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(377, 19);
            label4.Name = "label4";
            label4.Size = new Size(62, 20);
            label4.TabIndex = 36;
            label4.Text = "Ghi chú ";
            // 
            // txtDonViTinh
            // 
            txtDonViTinh.Location = new Point(136, 78);
            txtDonViTinh.Name = "txtDonViTinh";
            txtDonViTinh.Size = new Size(224, 27);
            txtDonViTinh.TabIndex = 35;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 85);
            label3.Name = "label3";
            label3.Size = new Size(81, 20);
            label3.TabIndex = 34;
            label3.Text = "Đơn vị tính";
            // 
            // txtSoLuong
            // 
            txtSoLuong.Location = new Point(136, 45);
            txtSoLuong.Name = "txtSoLuong";
            txtSoLuong.Size = new Size(224, 27);
            txtSoLuong.TabIndex = 33;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 52);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 32;
            label2.Text = "Số lượng";
            // 
            // txtTenNguyenLieu
            // 
            txtTenNguyenLieu.Location = new Point(136, 12);
            txtTenNguyenLieu.Name = "txtTenNguyenLieu";
            txtTenNguyenLieu.Size = new Size(224, 27);
            txtTenNguyenLieu.TabIndex = 31;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 19);
            label1.Name = "label1";
            label1.Size = new Size(112, 20);
            label1.TabIndex = 30;
            label1.Text = "Tên nguyên liệu";
            // 
            // dtpNgayNhap
            // 
            dtpNgayNhap.Location = new Point(495, 78);
            dtpNgayNhap.Name = "dtpNgayNhap";
            dtpNgayNhap.Size = new Size(224, 27);
            dtpNgayNhap.TabIndex = 42;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(377, 85);
            label6.Name = "label6";
            label6.Size = new Size(81, 20);
            label6.TabIndex = 41;
            label6.Text = "Ngày nhập";
            // 
            // txtGiaNhap
            // 
            txtGiaNhap.Location = new Point(495, 45);
            txtGiaNhap.Name = "txtGiaNhap";
            txtGiaNhap.Size = new Size(224, 27);
            txtGiaNhap.TabIndex = 40;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(377, 52);
            label5.Name = "label5";
            label5.Size = new Size(68, 20);
            label5.TabIndex = 39;
            label5.Text = "Giá nhập";
            // 
            // btnThem
            // 
            btnThem.Location = new Point(18, 112);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 28);
            btnThem.TabIndex = 43;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // dtgvNguyenLieu
            // 
            dtgvNguyenLieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvNguyenLieu.Location = new Point(18, 146);
            dtgvNguyenLieu.Name = "dtgvNguyenLieu";
            dtgvNguyenLieu.RowHeadersWidth = 51;
            dtgvNguyenLieu.Size = new Size(770, 147);
            dtgvNguyenLieu.TabIndex = 44;
            // 
            // dtgvLichSuNhap
            // 
            dtgvLichSuNhap.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvLichSuNhap.Location = new Point(18, 299);
            dtgvLichSuNhap.Name = "dtgvLichSuNhap";
            dtgvLichSuNhap.RowHeadersWidth = 51;
            dtgvLichSuNhap.Size = new Size(770, 142);
            dtgvLichSuNhap.TabIndex = 45;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(218, 111);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 47;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(118, 111);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 46;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // FormNhapNguyenLieu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(dtgvLichSuNhap);
            Controls.Add(dtgvNguyenLieu);
            Controls.Add(btnThem);
            Controls.Add(dtpNgayNhap);
            Controls.Add(label6);
            Controls.Add(txtGiaNhap);
            Controls.Add(label5);
            Controls.Add(txtGhiChu);
            Controls.Add(label4);
            Controls.Add(txtDonViTinh);
            Controls.Add(label3);
            Controls.Add(txtSoLuong);
            Controls.Add(label2);
            Controls.Add(txtTenNguyenLieu);
            Controls.Add(label1);
            Name = "FormNhapNguyenLieu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormNhapNguyenLieu";
            Load += FormNhapNguyenLieu_Load;
            ((System.ComponentModel.ISupportInitialize)dtgvNguyenLieu).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtgvLichSuNhap).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtGhiChu;
        private Label label4;
        private TextBox txtDonViTinh;
        private Label label3;
        private TextBox txtSoLuong;
        private Label label2;
        private TextBox txtTenNguyenLieu;
        private Label label1;
        private DateTimePicker dtpNgayNhap;
        private Label label6;
        private TextBox txtGiaNhap;
        private Label label5;
        private Button btnThem;
        private DataGridView dtgvNguyenLieu;
        private DataGridView dtgvLichSuNhap;
        private Button btnXoa;
        private Button btnSua;
    }
}