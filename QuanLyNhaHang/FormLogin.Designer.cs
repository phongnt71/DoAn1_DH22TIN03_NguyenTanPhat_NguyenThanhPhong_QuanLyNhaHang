namespace QuanLyNhaHang
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pictureBoxLogo = new PictureBox();
            labelTitle = new Label();
            labelUsername = new Label();
            labelPassword = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnExit = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Location = new Point(320, 20);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(150, 150);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 0;
            pictureBoxLogo.TabStop = false;
            // 
            // labelTitle
            // 
            labelTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelTitle.ForeColor = Color.FromArgb(34, 112, 52);
            labelTitle.Location = new Point(0, 180);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(800, 40);
            labelTitle.TabIndex = 1;
            labelTitle.Text = "ĐĂNG NHẬP";
            labelTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelUsername
            // 
            labelUsername.Font = new Font("Segoe UI", 10F);
            labelUsername.Location = new Point(226, 240);
            labelUsername.Name = "labelUsername";
            labelUsername.Size = new Size(131, 25);
            labelUsername.TabIndex = 2;
            labelUsername.Text = "Tên đăng nhập:";
            // 
            // labelPassword
            // 
            labelPassword.Font = new Font("Segoe UI", 10F);
            labelPassword.Location = new Point(226, 280);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(131, 25);
            labelPassword.TabIndex = 3;
            labelPassword.Text = "Mật khẩu:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(367, 238);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(197, 27);
            txtUsername.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(367, 278);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(197, 27);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(34, 112, 52);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(296, 320);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(131, 41);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += BtnLogin_Click;
            // 
            // btnExit
            // 
            btnExit.Font = new Font("Segoe UI", 10F);
            btnExit.Location = new Point(433, 320);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(131, 41);
            btnExit.TabIndex = 3;
            btnExit.Text = "Thoát";
            btnExit.Click += BtnExit_Click;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 420);
            Controls.Add(pictureBoxLogo);
            Controls.Add(labelTitle);
            Controls.Add(labelUsername);
            Controls.Add(txtUsername);
            Controls.Add(labelPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnExit);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormLogin";
            Load += FormLogin_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
    }
}
