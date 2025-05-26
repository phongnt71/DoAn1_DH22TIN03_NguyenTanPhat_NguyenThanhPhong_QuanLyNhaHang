namespace QuanLyNhaHang
{
    partial class FormNguyenLieu
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
            dgvNguyenLieu = new DataGridView();
            txtTenNguyenLieu = new TextBox();
            label1 = new Label();
            txtSoLuongTon = new TextBox();
            label2 = new Label();
            txtDonViTinh = new TextBox();
            label3 = new Label();
            txtGhiChu = new TextBox();
            label4 = new Label();
            btnHuy = new Button();
            btnLuu = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvNguyenLieu).BeginInit();
            SuspendLayout();
            // 
            // dgvNguyenLieu
            // 
            dgvNguyenLieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNguyenLieu.Location = new Point(12, 12);
            dgvNguyenLieu.Name = "dgvNguyenLieu";
            dgvNguyenLieu.RowHeadersWidth = 51;
            dgvNguyenLieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNguyenLieu.Size = new Size(776, 240);
            dgvNguyenLieu.TabIndex = 1;
            // 
            // txtTenNguyenLieu
            // 
            txtTenNguyenLieu.Location = new Point(132, 258);
            txtTenNguyenLieu.Name = "txtTenNguyenLieu";
            txtTenNguyenLieu.Size = new Size(224, 27);
            txtTenNguyenLieu.TabIndex = 23;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 265);
            label1.Name = "label1";
            label1.Size = new Size(112, 20);
            label1.TabIndex = 22;
            label1.Text = "Tên nguyên liệu";
            // 
            // txtSoLuongTon
            // 
            txtSoLuongTon.Location = new Point(132, 291);
            txtSoLuongTon.Name = "txtSoLuongTon";
            txtSoLuongTon.Size = new Size(224, 27);
            txtSoLuongTon.TabIndex = 25;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 298);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 24;
            label2.Text = "Số lượng";
            // 
            // txtDonViTinh
            // 
            txtDonViTinh.Location = new Point(132, 324);
            txtDonViTinh.Name = "txtDonViTinh";
            txtDonViTinh.Size = new Size(224, 27);
            txtDonViTinh.TabIndex = 27;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 331);
            label3.Name = "label3";
            label3.Size = new Size(81, 20);
            label3.TabIndex = 26;
            label3.Text = "Đơn vị tính";
            // 
            // txtGhiChu
            // 
            txtGhiChu.Location = new Point(132, 357);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(224, 27);
            txtGhiChu.TabIndex = 29;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 364);
            label4.Name = "label4";
            label4.Size = new Size(62, 20);
            label4.TabIndex = 28;
            label4.Text = "Ghi chú ";
            // 
            // btnHuy
            // 
            btnHuy.Location = new Point(414, 409);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 34;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(314, 409);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(94, 29);
            btnLuu.TabIndex = 33;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(214, 409);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 32;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(114, 409);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 31;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(14, 409);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 30;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // FormNguyenLieu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(txtGhiChu);
            Controls.Add(label4);
            Controls.Add(txtDonViTinh);
            Controls.Add(label3);
            Controls.Add(txtSoLuongTon);
            Controls.Add(label2);
            Controls.Add(txtTenNguyenLieu);
            Controls.Add(label1);
            Controls.Add(dgvNguyenLieu);
            Name = "FormNguyenLieu";
            Text = "FormNguyenLieu";
            Load += FormNguyenLieu_Load;
            ((System.ComponentModel.ISupportInitialize)dgvNguyenLieu).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvNguyenLieu;
        private TextBox txtTenNguyenLieu;
        private Label label1;
        private TextBox txtSoLuongTon;
        private Label label2;
        private TextBox txtDonViTinh;
        private Label label3;
        private TextBox txtGhiChu;
        private Label label4;
        private Button btnHuy;
        private Button btnLuu;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
    }
}