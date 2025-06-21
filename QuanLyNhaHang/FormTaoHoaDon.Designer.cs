namespace QuanLyNhaHang
{
    partial class FormTaoHoaDon
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
            label1 = new Label();
            cmbNhanVien = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtKhachHang = new TextBox();
            txtMaHoaDon = new TextBox();
            dtpNgayLap = new DateTimePicker();
            cmbMonAn = new ComboBox();
            label5 = new Label();
            nudSoLuong = new NumericUpDown();
            label6 = new Label();
            txtTongTien = new TextBox();
            label7 = new Label();
            dtgvChiTietHD = new DataGridView();
            btnHuy = new Button();
            btnLuu = new Button();
            btnSua = new Button();
            btnThem = new Button();
            btnXoa = new Button();
            cmbSoBan = new ComboBox();
            label8 = new Label();
            chkMangVe = new CheckBox();
            cmbBanDatTruoc = new ComboBox();
            label9 = new Label();
            txtGhiChu = new TextBox();
            label10 = new Label();
            dtgvHoaDonDaTao = new DataGridView();
            btnCapNhat = new Button();
            btnThemHD = new Button();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtgvChiTietHD).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtgvHoaDonDaTao).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 0;
            label1.Text = "Nhân viên";
            // 
            // cmbNhanVien
            // 
            cmbNhanVien.FormattingEnabled = true;
            cmbNhanVien.Location = new Point(112, 11);
            cmbNhanVien.Name = "cmbNhanVien";
            cmbNhanVien.Size = new Size(250, 28);
            cmbNhanVien.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 52);
            label2.Name = "label2";
            label2.Size = new Size(86, 20);
            label2.TabIndex = 2;
            label2.Text = "Khách hàng";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 85);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 3;
            label3.Text = "Ngày lập";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 118);
            label4.Name = "label4";
            label4.Size = new Size(89, 20);
            label4.TabIndex = 4;
            label4.Text = "Mã hóa đơn";
            // 
            // txtKhachHang
            // 
            txtKhachHang.Location = new Point(112, 45);
            txtKhachHang.Name = "txtKhachHang";
            txtKhachHang.Size = new Size(250, 27);
            txtKhachHang.TabIndex = 5;
            // 
            // txtMaHoaDon
            // 
            txtMaHoaDon.Location = new Point(112, 111);
            txtMaHoaDon.Name = "txtMaHoaDon";
            txtMaHoaDon.ReadOnly = true;
            txtMaHoaDon.Size = new Size(250, 27);
            txtMaHoaDon.TabIndex = 6;
            // 
            // dtpNgayLap
            // 
            dtpNgayLap.Location = new Point(112, 78);
            dtpNgayLap.Name = "dtpNgayLap";
            dtpNgayLap.Size = new Size(250, 27);
            dtpNgayLap.TabIndex = 7;
            // 
            // cmbMonAn
            // 
            cmbMonAn.FormattingEnabled = true;
            cmbMonAn.Location = new Point(112, 144);
            cmbMonAn.Name = "cmbMonAn";
            cmbMonAn.Size = new Size(189, 28);
            cmbMonAn.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 152);
            label5.Name = "label5";
            label5.Size = new Size(59, 20);
            label5.TabIndex = 8;
            label5.Text = "Món ăn";
            // 
            // nudSoLuong
            // 
            nudSoLuong.Location = new Point(382, 146);
            nudSoLuong.Name = "nudSoLuong";
            nudSoLuong.Size = new Size(96, 27);
            nudSoLuong.TabIndex = 10;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(307, 149);
            label6.Name = "label6";
            label6.Size = new Size(69, 20);
            label6.TabIndex = 11;
            label6.Text = "Số lượng";
            // 
            // txtTongTien
            // 
            txtTongTien.Location = new Point(112, 411);
            txtTongTien.Name = "txtTongTien";
            txtTongTien.Size = new Size(250, 27);
            txtTongTien.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(15, 418);
            label7.Name = "label7";
            label7.Size = new Size(72, 20);
            label7.TabIndex = 12;
            label7.Text = "Tổng tiền";
            // 
            // dtgvChiTietHD
            // 
            dtgvChiTietHD.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvChiTietHD.Location = new Point(12, 173);
            dtgvChiTietHD.Name = "dtgvChiTietHD";
            dtgvChiTietHD.RowHeadersWidth = 51;
            dtgvChiTietHD.Size = new Size(776, 109);
            dtgvChiTietHD.TabIndex = 14;
            // 
            // btnHuy
            // 
            btnHuy.Location = new Point(694, 409);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 16;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(594, 409);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(94, 29);
            btnLuu.TabIndex = 15;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(594, 139);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 18;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(494, 140);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 17;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(694, 139);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 19;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            // 
            // cmbSoBan
            // 
            cmbSoBan.FormattingEnabled = true;
            cmbSoBan.Location = new Point(532, 11);
            cmbSoBan.Name = "cmbSoBan";
            cmbSoBan.Size = new Size(107, 28);
            cmbSoBan.TabIndex = 21;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(412, 20);
            label8.Name = "label8";
            label8.Size = new Size(95, 20);
            label8.TabIndex = 20;
            label8.Text = "Số bàn trống";
            // 
            // chkMangVe
            // 
            chkMangVe.AutoSize = true;
            chkMangVe.Location = new Point(645, 10);
            chkMangVe.Name = "chkMangVe";
            chkMangVe.Size = new Size(88, 24);
            chkMangVe.TabIndex = 22;
            chkMangVe.Text = "Mang về";
            chkMangVe.UseVisualStyleBackColor = true;
            // 
            // cmbBanDatTruoc
            // 
            cmbBanDatTruoc.FormattingEnabled = true;
            cmbBanDatTruoc.Location = new Point(532, 47);
            cmbBanDatTruoc.Name = "cmbBanDatTruoc";
            cmbBanDatTruoc.Size = new Size(107, 28);
            cmbBanDatTruoc.TabIndex = 24;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(412, 55);
            label9.Name = "label9";
            label9.Size = new Size(99, 20);
            label9.TabIndex = 23;
            label9.Text = "Bàn đặt trước";
            // 
            // txtGhiChu
            // 
            txtGhiChu.Location = new Point(532, 85);
            txtGhiChu.Multiline = true;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(239, 48);
            txtGhiChu.TabIndex = 26;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(412, 88);
            label10.Name = "label10";
            label10.Size = new Size(58, 20);
            label10.TabIndex = 25;
            label10.Text = "Ghi chú";
            // 
            // dtgvHoaDonDaTao
            // 
            dtgvHoaDonDaTao.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvHoaDonDaTao.Location = new Point(12, 288);
            dtgvHoaDonDaTao.Name = "dtgvHoaDonDaTao";
            dtgvHoaDonDaTao.RowHeadersWidth = 51;
            dtgvHoaDonDaTao.Size = new Size(776, 115);
            dtgvHoaDonDaTao.TabIndex = 27;
            // 
            // btnCapNhat
            // 
            btnCapNhat.Location = new Point(494, 409);
            btnCapNhat.Name = "btnCapNhat";
            btnCapNhat.Size = new Size(94, 29);
            btnCapNhat.TabIndex = 29;
            btnCapNhat.Text = "Cập nhật";
            btnCapNhat.UseVisualStyleBackColor = true;
            btnCapNhat.Click += btnCapNhat_Click;
            // 
            // btnThemHD
            // 
            btnThemHD.Location = new Point(394, 409);
            btnThemHD.Name = "btnThemHD";
            btnThemHD.Size = new Size(94, 29);
            btnThemHD.TabIndex = 28;
            btnThemHD.Text = "Thêm";
            btnThemHD.UseVisualStyleBackColor = true;
            btnThemHD.Click += btnThemHD_Click;
            // 
            // FormTaoHoaDon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCapNhat);
            Controls.Add(btnThemHD);
            Controls.Add(dtgvHoaDonDaTao);
            Controls.Add(txtGhiChu);
            Controls.Add(label10);
            Controls.Add(cmbBanDatTruoc);
            Controls.Add(label9);
            Controls.Add(chkMangVe);
            Controls.Add(cmbSoBan);
            Controls.Add(label8);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(dtgvChiTietHD);
            Controls.Add(txtTongTien);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(nudSoLuong);
            Controls.Add(cmbMonAn);
            Controls.Add(label5);
            Controls.Add(dtpNgayLap);
            Controls.Add(txtMaHoaDon);
            Controls.Add(txtKhachHang);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cmbNhanVien);
            Controls.Add(label1);
            Name = "FormTaoHoaDon";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormTaoHoaDon";
            Load += FormTaoHoaDon_Load;
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtgvChiTietHD).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtgvHoaDonDaTao).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbNhanVien;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtKhachHang;
        private TextBox txtMaHoaDon;
        private DateTimePicker dtpNgayLap;
        private ComboBox cmbMonAn;
        private Label label5;
        private NumericUpDown nudSoLuong;
        private Label label6;
        private TextBox txtTongTien;
        private Label label7;
        private DataGridView dtgvChiTietHD;
        private Button btnHuy;
        private Button btnLuu;
        private Button btnSua;
        private Button btnThem;
        private Button btnXoa;
        private ComboBox cmbSoBan;
        private Label label8;
        private CheckBox chkMangVe;
        private ComboBox cmbBanDatTruoc;
        private Label label9;
        private TextBox txtGhiChu;
        private Label label10;
        private DataGridView dtgvHoaDonDaTao;
        private Button btnCapNhat;
        private Button btnThemHD;
    }
}