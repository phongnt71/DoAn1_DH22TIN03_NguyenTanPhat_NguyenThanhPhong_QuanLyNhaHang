namespace QuanLyNhaHang
{
    partial class FormDSHoaDon
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
            picQR = new PictureBox();
            cmbNhanVien = new ComboBox();
            label5 = new Label();
            dtpNgayLap = new DateTimePicker();
            label3 = new Label();
            txtKhachHang = new TextBox();
            label2 = new Label();
            txtTongTien = new TextBox();
            label7 = new Label();
            txtMaHoaDon = new TextBox();
            label4 = new Label();
            btnLamMoi = new Button();
            btnInHoaDon = new Button();
            dtgvDSHoaDon = new DataGridView();
            dtgvChiTietMonAn = new DataGridView();
            btnXoa = new Button();
            ((System.ComponentModel.ISupportInitialize)picQR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtgvDSHoaDon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtgvChiTietMonAn).BeginInit();
            SuspendLayout();
            // 
            // picQR
            // 
            picQR.Location = new Point(649, 175);
            picQR.Name = "picQR";
            picQR.Size = new Size(139, 130);
            picQR.TabIndex = 62;
            picQR.TabStop = false;
            // 
            // cmbNhanVien
            // 
            cmbNhanVien.FormattingEnabled = true;
            cmbNhanVien.Location = new Point(471, 310);
            cmbNhanVien.Name = "cmbNhanVien";
            cmbNhanVien.Size = new Size(220, 28);
            cmbNhanVien.TabIndex = 61;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(371, 318);
            label5.Name = "label5";
            label5.Size = new Size(75, 20);
            label5.TabIndex = 60;
            label5.Text = "Nhân viên";
            // 
            // dtpNgayLap
            // 
            dtpNgayLap.Location = new Point(112, 377);
            dtpNgayLap.Name = "dtpNgayLap";
            dtpNgayLap.Size = new Size(220, 27);
            dtpNgayLap.TabIndex = 59;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 384);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 58;
            label3.Text = "Ngày lập";
            // 
            // txtKhachHang
            // 
            txtKhachHang.Location = new Point(112, 344);
            txtKhachHang.Name = "txtKhachHang";
            txtKhachHang.Size = new Size(220, 27);
            txtKhachHang.TabIndex = 57;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 351);
            label2.Name = "label2";
            label2.Size = new Size(86, 20);
            label2.TabIndex = 56;
            label2.Text = "Khách hàng";
            // 
            // txtTongTien
            // 
            txtTongTien.Location = new Point(471, 344);
            txtTongTien.Name = "txtTongTien";
            txtTongTien.Size = new Size(220, 27);
            txtTongTien.TabIndex = 55;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(374, 351);
            label7.Name = "label7";
            label7.Size = new Size(72, 20);
            label7.TabIndex = 54;
            label7.Text = "Tổng tiền";
            // 
            // txtMaHoaDon
            // 
            txtMaHoaDon.Location = new Point(112, 311);
            txtMaHoaDon.Name = "txtMaHoaDon";
            txtMaHoaDon.Size = new Size(220, 27);
            txtMaHoaDon.TabIndex = 53;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 318);
            label4.Name = "label4";
            label4.Size = new Size(89, 20);
            label4.TabIndex = 52;
            label4.Text = "Mã hóa đơn";
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(471, 384);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 29);
            btnLamMoi.TabIndex = 51;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnInHoaDon
            // 
            btnInHoaDon.Location = new Point(371, 384);
            btnInHoaDon.Name = "btnInHoaDon";
            btnInHoaDon.Size = new Size(94, 29);
            btnInHoaDon.TabIndex = 50;
            btnInHoaDon.Text = "In hóa đơn";
            btnInHoaDon.UseVisualStyleBackColor = true;
            btnInHoaDon.Click += btnInHoaDon_Click;
            // 
            // dtgvDSHoaDon
            // 
            dtgvDSHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvDSHoaDon.Location = new Point(12, 12);
            dtgvDSHoaDon.Name = "dtgvDSHoaDon";
            dtgvDSHoaDon.RowHeadersWidth = 51;
            dtgvDSHoaDon.Size = new Size(776, 157);
            dtgvDSHoaDon.TabIndex = 49;
            // 
            // dtgvChiTietMonAn
            // 
            dtgvChiTietMonAn.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvChiTietMonAn.Location = new Point(12, 175);
            dtgvChiTietMonAn.Name = "dtgvChiTietMonAn";
            dtgvChiTietMonAn.RowHeadersWidth = 51;
            dtgvChiTietMonAn.Size = new Size(631, 130);
            dtgvChiTietMonAn.TabIndex = 63;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(571, 384);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 66;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // FormDSHoaDon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnXoa);
            Controls.Add(dtgvChiTietMonAn);
            Controls.Add(picQR);
            Controls.Add(cmbNhanVien);
            Controls.Add(label5);
            Controls.Add(dtpNgayLap);
            Controls.Add(label3);
            Controls.Add(txtKhachHang);
            Controls.Add(label2);
            Controls.Add(txtTongTien);
            Controls.Add(label7);
            Controls.Add(txtMaHoaDon);
            Controls.Add(label4);
            Controls.Add(btnLamMoi);
            Controls.Add(btnInHoaDon);
            Controls.Add(dtgvDSHoaDon);
            Name = "FormDSHoaDon";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormDSHoaDon";
            Load += FormDSHoaDon_Load;
            ((System.ComponentModel.ISupportInitialize)picQR).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtgvDSHoaDon).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtgvChiTietMonAn).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picQR;
        private ComboBox cmbNhanVien;
        private Label label5;
        private DateTimePicker dtpNgayLap;
        private Label label3;
        private TextBox txtKhachHang;
        private Label label2;
        private TextBox txtTongTien;
        private Label label7;
        private TextBox txtMaHoaDon;
        private Label label4;
        private Button btnLamMoi;
        private Button btnInHoaDon;
        private DataGridView dtgvDSHoaDon;
        private DataGridView dtgvChiTietMonAn;
        private Button btnXoa;
    }
}