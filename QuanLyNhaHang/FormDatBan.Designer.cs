namespace QuanLyNhaHang
{
    partial class FormDatBan
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
            txtTenKhachHang = new TextBox();
            label2 = new Label();
            cmbBanAn = new ComboBox();
            label3 = new Label();
            label1 = new Label();
            dtpNgayDat = new DateTimePicker();
            dtpGioDat = new DateTimePicker();
            label4 = new Label();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            dtgvDatBan = new DataGridView();
            btnLamMoi = new Button();
            ((System.ComponentModel.ISupportInitialize)dtgvDatBan).BeginInit();
            SuspendLayout();
            // 
            // txtTenKhachHang
            // 
            txtTenKhachHang.Location = new Point(133, 12);
            txtTenKhachHang.Name = "txtTenKhachHang";
            txtTenKhachHang.Size = new Size(151, 27);
            txtTenKhachHang.TabIndex = 25;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 19);
            label2.Name = "label2";
            label2.Size = new Size(111, 20);
            label2.TabIndex = 24;
            label2.Text = "Tên khách hàng";
            // 
            // cmbBanAn
            // 
            cmbBanAn.FormattingEnabled = true;
            cmbBanAn.Location = new Point(133, 45);
            cmbBanAn.Name = "cmbBanAn";
            cmbBanAn.Size = new Size(151, 28);
            cmbBanAn.TabIndex = 26;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 53);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 27;
            label3.Text = "Chọn bàn";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 87);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 29;
            label1.Text = "Ngày đặt";
            // 
            // dtpNgayDat
            // 
            dtpNgayDat.Format = DateTimePickerFormat.Short;
            dtpNgayDat.Location = new Point(133, 79);
            dtpNgayDat.Name = "dtpNgayDat";
            dtpNgayDat.Size = new Size(151, 27);
            dtpNgayDat.TabIndex = 30;
            // 
            // dtpGioDat
            // 
            dtpGioDat.Format = DateTimePickerFormat.Time;
            dtpGioDat.Location = new Point(133, 112);
            dtpGioDat.Name = "dtpGioDat";
            dtpGioDat.Size = new Size(151, 27);
            dtpGioDat.TabIndex = 32;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 120);
            label4.Name = "label4";
            label4.Size = new Size(58, 20);
            label4.TabIndex = 31;
            label4.Text = "Giờ đặt";
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(210, 145);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 35;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(110, 145);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 34;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(10, 145);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 33;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // dtgvDatBan
            // 
            dtgvDatBan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvDatBan.Location = new Point(10, 180);
            dtgvDatBan.Name = "dtgvDatBan";
            dtgvDatBan.RowHeadersWidth = 51;
            dtgvDatBan.Size = new Size(778, 258);
            dtgvDatBan.TabIndex = 36;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(310, 145);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(94, 29);
            btnLamMoi.TabIndex = 52;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // FormDatBan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnLamMoi);
            Controls.Add(dtgvDatBan);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(dtpGioDat);
            Controls.Add(label4);
            Controls.Add(dtpNgayDat);
            Controls.Add(label1);
            Controls.Add(label3);
            Controls.Add(cmbBanAn);
            Controls.Add(txtTenKhachHang);
            Controls.Add(label2);
            Name = "FormDatBan";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormDatBan";
            Load += FormDatBan_Load;
            ((System.ComponentModel.ISupportInitialize)dtgvDatBan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTenKhachHang;
        private Label label2;
        private ComboBox cmbBanAn;
        private Label label3;
        private Label label1;
        private DateTimePicker dtpNgayDat;
        private DateTimePicker dtpGioDat;
        private Label label4;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
        private DataGridView dtgvDatBan;
        private Button btnLamMoi;
    }
}