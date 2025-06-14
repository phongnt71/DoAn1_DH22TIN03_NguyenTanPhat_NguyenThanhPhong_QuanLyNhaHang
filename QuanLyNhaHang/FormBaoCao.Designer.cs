namespace QuanLyNhaHang
{
    partial class FormBaoCao
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
            groupBox1 = new GroupBox();
            dtpNhapHangTo = new DateTimePicker();
            dtpNhapHangFrom = new DateTimePicker();
            label2 = new Label();
            label1 = new Label();
            txtTongTienNhapHang = new TextBox();
            btnThongKeNhapHang = new Button();
            groupBox2 = new GroupBox();
            dtpHoaDonTo = new DateTimePicker();
            txtTongTienHoaDonDaThanhToan = new TextBox();
            dtpHoaDonFrom = new DateTimePicker();
            label3 = new Label();
            btnXemTongHoaDon = new Button();
            label4 = new Label();
            groupBox3 = new GroupBox();
            btnThongKeTheoKhoangThoiGian = new Button();
            dtpThuTo = new DateTimePicker();
            dtgvTongThuTheoNgay = new DataGridView();
            dtpThuFrom = new DateTimePicker();
            label6 = new Label();
            label5 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtgvTongThuTheoNgay).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dtpNhapHangTo);
            groupBox1.Controls.Add(dtpNhapHangFrom);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtTongTienNhapHang);
            groupBox1.Controls.Add(btnThongKeNhapHang);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(792, 115);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tiền nhập hàng";
            // 
            // dtpNhapHangTo
            // 
            dtpNhapHangTo.Location = new Point(313, 78);
            dtpNhapHangTo.Name = "dtpNhapHangTo";
            dtpNhapHangTo.Size = new Size(250, 27);
            dtpNhapHangTo.TabIndex = 5;
            // 
            // dtpNhapHangFrom
            // 
            dtpNhapHangFrom.Location = new Point(313, 36);
            dtpNhapHangFrom.Name = "dtpNhapHangFrom";
            dtpNhapHangFrom.Size = new Size(250, 27);
            dtpNhapHangFrom.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(235, 85);
            label2.Name = "label2";
            label2.Size = new Size(72, 20);
            label2.TabIndex = 3;
            label2.Text = "Đến ngày";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(235, 43);
            label1.Name = "label1";
            label1.Size = new Size(62, 20);
            label1.TabIndex = 2;
            label1.Text = "Từ ngày";
            // 
            // txtTongTienNhapHang
            // 
            txtTongTienNhapHang.Location = new Point(6, 69);
            txtTongTienNhapHang.Multiline = true;
            txtTongTienNhapHang.Name = "txtTongTienNhapHang";
            txtTongTienNhapHang.ReadOnly = true;
            txtTongTienNhapHang.Size = new Size(191, 36);
            txtTongTienNhapHang.TabIndex = 1;
            // 
            // btnThongKeNhapHang
            // 
            btnThongKeNhapHang.Location = new Point(6, 26);
            btnThongKeNhapHang.Name = "btnThongKeNhapHang";
            btnThongKeNhapHang.Size = new Size(191, 37);
            btnThongKeNhapHang.TabIndex = 0;
            btnThongKeNhapHang.Text = "Thống kê tiền nhập hàng";
            btnThongKeNhapHang.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dtpHoaDonTo);
            groupBox2.Controls.Add(txtTongTienHoaDonDaThanhToan);
            groupBox2.Controls.Add(dtpHoaDonFrom);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(btnXemTongHoaDon);
            groupBox2.Controls.Add(label4);
            groupBox2.Location = new Point(12, 133);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(792, 114);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tổng tiền hóa đơn đã thanh toán";
            // 
            // dtpHoaDonTo
            // 
            dtpHoaDonTo.Location = new Point(313, 78);
            dtpHoaDonTo.Name = "dtpHoaDonTo";
            dtpHoaDonTo.Size = new Size(250, 27);
            dtpHoaDonTo.TabIndex = 9;
            // 
            // txtTongTienHoaDonDaThanhToan
            // 
            txtTongTienHoaDonDaThanhToan.Location = new Point(6, 69);
            txtTongTienHoaDonDaThanhToan.Multiline = true;
            txtTongTienHoaDonDaThanhToan.Name = "txtTongTienHoaDonDaThanhToan";
            txtTongTienHoaDonDaThanhToan.ReadOnly = true;
            txtTongTienHoaDonDaThanhToan.Size = new Size(191, 36);
            txtTongTienHoaDonDaThanhToan.TabIndex = 3;
            // 
            // dtpHoaDonFrom
            // 
            dtpHoaDonFrom.Location = new Point(313, 36);
            dtpHoaDonFrom.Name = "dtpHoaDonFrom";
            dtpHoaDonFrom.Size = new Size(250, 27);
            dtpHoaDonFrom.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(235, 85);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 7;
            label3.Text = "Đến ngày";
            // 
            // btnXemTongHoaDon
            // 
            btnXemTongHoaDon.Location = new Point(6, 26);
            btnXemTongHoaDon.Name = "btnXemTongHoaDon";
            btnXemTongHoaDon.Size = new Size(191, 37);
            btnXemTongHoaDon.TabIndex = 2;
            btnXemTongHoaDon.Text = "Xem tổng hóa đơn";
            btnXemTongHoaDon.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(235, 43);
            label4.Name = "label4";
            label4.Size = new Size(62, 20);
            label4.TabIndex = 6;
            label4.Text = "Từ ngày";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnThongKeTheoKhoangThoiGian);
            groupBox3.Controls.Add(dtpThuTo);
            groupBox3.Controls.Add(dtgvTongThuTheoNgay);
            groupBox3.Controls.Add(dtpThuFrom);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(label5);
            groupBox3.Location = new Point(12, 253);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(776, 185);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Tổng tiền thu theo ngày";
            // 
            // btnThongKeTheoKhoangThoiGian
            // 
            btnThongKeTheoKhoangThoiGian.Location = new Point(539, 26);
            btnThongKeTheoKhoangThoiGian.Name = "btnThongKeTheoKhoangThoiGian";
            btnThongKeTheoKhoangThoiGian.Size = new Size(231, 38);
            btnThongKeTheoKhoangThoiGian.TabIndex = 10;
            btnThongKeTheoKhoangThoiGian.Text = "Thống kê theo khoảng thời gian";
            btnThongKeTheoKhoangThoiGian.UseVisualStyleBackColor = true;
            // 
            // dtpThuTo
            // 
            dtpThuTo.Format = DateTimePickerFormat.Short;
            dtpThuTo.Location = new Point(617, 112);
            dtpThuTo.Name = "dtpThuTo";
            dtpThuTo.Size = new Size(153, 27);
            dtpThuTo.TabIndex = 9;
            // 
            // dtgvTongThuTheoNgay
            // 
            dtgvTongThuTheoNgay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvTongThuTheoNgay.Location = new Point(6, 26);
            dtgvTongThuTheoNgay.Name = "dtgvTongThuTheoNgay";
            dtgvTongThuTheoNgay.RowHeadersWidth = 51;
            dtgvTongThuTheoNgay.Size = new Size(527, 159);
            dtgvTongThuTheoNgay.TabIndex = 0;
            // 
            // dtpThuFrom
            // 
            dtpThuFrom.Format = DateTimePickerFormat.Short;
            dtpThuFrom.Location = new Point(617, 70);
            dtpThuFrom.Name = "dtpThuFrom";
            dtpThuFrom.Size = new Size(153, 27);
            dtpThuFrom.TabIndex = 8;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(539, 77);
            label6.Name = "label6";
            label6.Size = new Size(62, 20);
            label6.TabIndex = 6;
            label6.Text = "Từ ngày";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(539, 119);
            label5.Name = "label5";
            label5.Size = new Size(72, 20);
            label5.TabIndex = 7;
            label5.Text = "Đến ngày";
            // 
            // FormBaoCao
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "FormBaoCao";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormBaoCao";
            Load += FormBaoCao_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dtgvTongThuTheoNgay).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpNhapHangTo;
        private System.Windows.Forms.DateTimePicker dtpNhapHangFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTongTienNhapHang;
        private System.Windows.Forms.Button btnThongKeNhapHang;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpHoaDonTo;
        private System.Windows.Forms.TextBox txtTongTienHoaDonDaThanhToan;
        private System.Windows.Forms.DateTimePicker dtpHoaDonFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnXemTongHoaDon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnThongKeTheoKhoangThoiGian;
        private System.Windows.Forms.DateTimePicker dtpThuTo;
        private System.Windows.Forms.DataGridView dtgvTongThuTheoNgay;
        private System.Windows.Forms.DateTimePicker dtpThuFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}
