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
            this.pictureBoxLogo = new PictureBox();
            this.labelTitle = new Label();
            this.labelUsername = new Label();
            this.labelPassword = new Label();
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.btnExit = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();

            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = Image.FromFile("Images\\logo.png"); // <-- Đảm bảo file tồn tại ở đường dẫn này
            this.pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.Location = new Point(320, 20);
            this.pictureBoxLogo.Size = new Size(150, 150);
            this.pictureBoxLogo.TabStop = false;

            // 
            // labelTitle
            // 
            this.labelTitle.Text = "ĐĂNG NHẬP";
            this.labelTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.labelTitle.ForeColor = Color.SteelBlue;
            this.labelTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.labelTitle.Location = new Point(0, 180);
            this.labelTitle.Size = new Size(800, 40);

            // 
            // labelUsername
            // 
            this.labelUsername.Text = "Tên đăng nhập:";
            this.labelUsername.Font = new Font("Segoe UI", 10F);
            this.labelUsername.Location = new Point(250, 240);
            this.labelUsername.Size = new Size(120, 25);

            // 
            // txtUsername
            // 
            this.txtUsername.Location = new Point(380, 238);
            this.txtUsername.Size = new Size(170, 27);

            // 
            // labelPassword
            // 
            this.labelPassword.Text = "Mật khẩu:";
            this.labelPassword.Font = new Font("Segoe UI", 10F);
            this.labelPassword.Location = new Point(250, 280);
            this.labelPassword.Size = new Size(120, 25);

            // 
            // txtPassword
            // 
            this.txtPassword.Location = new Point(380, 278);
            this.txtPassword.Size = new Size(170, 27);
            this.txtPassword.UseSystemPasswordChar = true;

            // 
            // btnLogin
            // 
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnLogin.BackColor = Color.SteelBlue;
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.Location = new Point(380, 320);
            this.btnLogin.Size = new Size(100, 35);
            this.btnLogin.Click += BtnLogin_Click;

            // 
            // btnExit
            // 
            this.btnExit.Text = "Thoát";
            this.btnExit.Font = new Font("Segoe UI", 10F);
            this.btnExit.Location = new Point(490, 320);
            this.btnExit.Size = new Size(60, 35);
            this.btnExit.Click += BtnExit_Click;

            // 
            // FormLogin
            // 
            this.AcceptButton = this.btnLogin;
            this.ClientSize = new Size(800, 420);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập hệ thống";
            this.Load += FormLogin_Load;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxLogo;
        private Label labelTitle;
        private Label labelUsername;
        private Label labelPassword;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
    }
}
