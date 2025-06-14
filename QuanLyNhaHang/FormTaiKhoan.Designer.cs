namespace QuanLyNhaHang
{
    partial class FormTaiKhoan
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
            groupBox1 = new GroupBox();
            txtQuyen = new TextBox();
            label2 = new Label();
            txtTenTK = new TextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            btnDoiMK = new Button();
            txtNhapLaiMK = new TextBox();
            label5 = new Label();
            txtMKMoi = new TextBox();
            label3 = new Label();
            txtMKCu = new TextBox();
            label4 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtQuyen);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtTenTK);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(776, 106);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tài khoản";
            // 
            // txtQuyen
            // 
            txtQuyen.Location = new Point(151, 59);
            txtQuyen.Name = "txtQuyen";
            txtQuyen.ReadOnly = true;
            txtQuyen.Size = new Size(224, 27);
            txtQuyen.TabIndex = 35;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 66);
            label2.Name = "label2";
            label2.Size = new Size(104, 20);
            label2.TabIndex = 34;
            label2.Text = "Quyền hiện tại";
            // 
            // txtTenTK
            // 
            txtTenTK.Location = new Point(151, 26);
            txtTenTK.Name = "txtTenTK";
            txtTenTK.ReadOnly = true;
            txtTenTK.Size = new Size(224, 27);
            txtTenTK.TabIndex = 33;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 33);
            label1.Name = "label1";
            label1.Size = new Size(137, 20);
            label1.TabIndex = 32;
            label1.Text = "Thông tin tài khoản";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnDoiMK);
            groupBox2.Controls.Add(txtNhapLaiMK);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(txtMKMoi);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(txtMKCu);
            groupBox2.Controls.Add(label4);
            groupBox2.Location = new Point(12, 124);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(776, 300);
            groupBox2.TabIndex = 36;
            groupBox2.TabStop = false;
            groupBox2.Text = "Đổi mật khẩu";
            // 
            // btnDoiMK
            // 
            btnDoiMK.Location = new Point(151, 125);
            btnDoiMK.Name = "btnDoiMK";
            btnDoiMK.Size = new Size(224, 29);
            btnDoiMK.TabIndex = 38;
            btnDoiMK.Text = "Đổi mật khẩu";
            btnDoiMK.UseVisualStyleBackColor = true;
            // 
            // txtNhapLaiMK
            // 
            txtNhapLaiMK.Location = new Point(151, 92);
            txtNhapLaiMK.Name = "txtNhapLaiMK";
            txtNhapLaiMK.Size = new Size(224, 27);
            txtNhapLaiMK.TabIndex = 37;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 99);
            label5.Name = "label5";
            label5.Size = new Size(130, 20);
            label5.TabIndex = 36;
            label5.Text = "Nhập lại mật khẩu";
            // 
            // txtMKMoi
            // 
            txtMKMoi.Location = new Point(151, 59);
            txtMKMoi.Name = "txtMKMoi";
            txtMKMoi.Size = new Size(224, 27);
            txtMKMoi.TabIndex = 35;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 66);
            label3.Name = "label3";
            label3.Size = new Size(100, 20);
            label3.TabIndex = 34;
            label3.Text = "Mật khẩu mới";
            // 
            // txtMKCu
            // 
            txtMKCu.Location = new Point(151, 26);
            txtMKCu.Name = "txtMKCu";
            txtMKCu.Size = new Size(224, 27);
            txtMKCu.TabIndex = 33;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 33);
            label4.Name = "label4";
            label4.Size = new Size(89, 20);
            label4.TabIndex = 32;
            label4.Text = "Mật khẩu cũ";
            // 
            // FormTaiKhoan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "FormTaiKhoan";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormTaiKhoan";
            Load += FormTaiKhoan_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox txtQuyen;
        private Label label2;
        private TextBox txtTenTK;
        private Label label1;
        private GroupBox groupBox2;
        private TextBox txtNhapLaiMK;
        private Label label5;
        private TextBox txtMKMoi;
        private Label label3;
        private TextBox txtMKCu;
        private Label label4;
        private Button btnDoiMK;
    }
}