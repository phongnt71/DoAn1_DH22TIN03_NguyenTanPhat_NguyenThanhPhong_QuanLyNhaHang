namespace QuanLyNhaHang
{
    partial class FormKhachHang
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
            txtDiaChi = new TextBox();
            label3 = new Label();
            txtSDT = new TextBox();
            label2 = new Label();
            txtTenKH = new TextBox();
            label1 = new Label();
            btnHuy = new Button();
            btnLuu = new Button();
            btnXoa = new Button();
            btnSua = new Button();
            btnThem = new Button();
            dtgvKhachHang = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dtgvKhachHang).BeginInit();
            SuspendLayout();
            // 
            // txtDiaChi
            // 
            txtDiaChi.Location = new Point(132, 78);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(224, 27);
            txtDiaChi.TabIndex = 33;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 85);
            label3.Name = "label3";
            label3.Size = new Size(55, 20);
            label3.TabIndex = 32;
            label3.Text = "Địa chỉ";
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(132, 45);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(224, 27);
            txtSDT.TabIndex = 31;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 52);
            label2.Name = "label2";
            label2.Size = new Size(97, 20);
            label2.TabIndex = 30;
            label2.Text = "Số điện thoại";
            // 
            // txtTenKH
            // 
            txtTenKH.Location = new Point(132, 12);
            txtTenKH.Name = "txtTenKH";
            txtTenKH.Size = new Size(224, 27);
            txtTenKH.TabIndex = 29;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 19);
            label1.Name = "label1";
            label1.Size = new Size(111, 20);
            label1.TabIndex = 28;
            label1.Text = "Tên khách hàng";
            // 
            // btnHuy
            // 
            btnHuy.Location = new Point(403, 111);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 39;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(303, 111);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(94, 29);
            btnLuu.TabIndex = 38;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(203, 111);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(94, 29);
            btnXoa.TabIndex = 37;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(103, 111);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(94, 29);
            btnSua.TabIndex = 36;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(3, 111);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(94, 29);
            btnThem.TabIndex = 35;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // dtgvKhachHang
            // 
            dtgvKhachHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvKhachHang.Location = new Point(3, 146);
            dtgvKhachHang.Name = "dtgvKhachHang";
            dtgvKhachHang.RowHeadersWidth = 51;
            dtgvKhachHang.Size = new Size(785, 292);
            dtgvKhachHang.TabIndex = 40;
            // 
            // FormKhachHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dtgvKhachHang);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(txtDiaChi);
            Controls.Add(label3);
            Controls.Add(txtSDT);
            Controls.Add(label2);
            Controls.Add(txtTenKH);
            Controls.Add(label1);
            Name = "FormKhachHang";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormKhachHang";
            ((System.ComponentModel.ISupportInitialize)dtgvKhachHang).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtDiaChi;
        private Label label3;
        private TextBox txtSDT;
        private Label label2;
        private TextBox txtTenKH;
        private Label label1;
        private Button btnHuy;
        private Button btnLuu;
        private Button btnXoa;
        private Button btnSua;
        private Button btnThem;
        private DataGridView dtgvKhachHang;
    }
}