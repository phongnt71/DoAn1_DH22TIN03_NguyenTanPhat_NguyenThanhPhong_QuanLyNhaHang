
using System.Net.Mail;
using System.Net;

namespace QuanLyNhaHang
{
    partial class FormQuenMatKhau
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.Label lblTaiKhoan;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.TextBox txtTaiKhoan;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnGuiMatKhauMoi;

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
            lblHoTen = new Label();
            txtHoTen = new TextBox();
            lblTaiKhoan = new Label();
            txtTaiKhoan = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            btnGuiMatKhauMoi = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // lblHoTen
            // 
            lblHoTen.AutoSize = true;
            lblHoTen.Location = new Point(40, 30);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(57, 20);
            lblHoTen.TabIndex = 0;
            lblHoTen.Text = "Họ tên:";
            // 
            // txtHoTen
            // 
            txtHoTen.Location = new Point(160, 27);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(250, 27);
            txtHoTen.TabIndex = 1;
            // 
            // lblTaiKhoan
            // 
            lblTaiKhoan.AutoSize = true;
            lblTaiKhoan.Location = new Point(40, 70);
            lblTaiKhoan.Name = "lblTaiKhoan";
            lblTaiKhoan.Size = new Size(100, 20);
            lblTaiKhoan.TabIndex = 2;
            lblTaiKhoan.Text = "Tên tài khoản:";
            // 
            // txtTaiKhoan
            // 
            txtTaiKhoan.Location = new Point(160, 67);
            txtTaiKhoan.Name = "txtTaiKhoan";
            txtTaiKhoan.Size = new Size(250, 27);
            txtTaiKhoan.TabIndex = 3;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(40, 110);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(126, 20);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "Email đã đăng ký:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(160, 107);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(250, 27);
            txtEmail.TabIndex = 5;
            // 
            // btnGuiMatKhauMoi
            // 
            btnGuiMatKhauMoi.Location = new Point(160, 150);
            btnGuiMatKhauMoi.Name = "btnGuiMatKhauMoi";
            btnGuiMatKhauMoi.Size = new Size(152, 35);
            btnGuiMatKhauMoi.TabIndex = 6;
            btnGuiMatKhauMoi.Text = "Gửi mật khẩu mới";
            btnGuiMatKhauMoi.UseVisualStyleBackColor = true;
            btnGuiMatKhauMoi.Click += btnGuiMatKhauMoi_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(318, 150);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(92, 35);
            btnExit.TabIndex = 7;
            btnExit.Text = "Đóng";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += BtnExit_Click;
            // 
            // FormQuenMatKhau
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 220);
            Controls.Add(btnExit);
            Controls.Add(lblHoTen);
            Controls.Add(txtHoTen);
            Controls.Add(lblTaiKhoan);
            Controls.Add(txtTaiKhoan);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(btnGuiMatKhauMoi);
            Name = "FormQuenMatKhau";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quên mật khẩu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnExit;
    }
}

