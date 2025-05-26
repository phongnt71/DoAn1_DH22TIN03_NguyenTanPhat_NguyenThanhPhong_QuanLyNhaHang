using System;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
#nullable enable

namespace QuanLyNhaHang
{
    public partial class FormLogin : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True";

        public FormLogin()
        {
            InitializeComponent();
            this.Load += FormLogin_Load;

            // Bắt sự kiện nhấn phím trong txtPassword
            txtUsername.KeyDown += TxtUsername_KeyDown;
            txtPassword.KeyDown += TxtPassword_KeyDown;
        }

        // Khi ở txtUsername, nhấn Enter sẽ chuyển sang txtPassword
        private void TxtUsername_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // tránh tiếng bip
                txtPassword.Focus();       // chuyển con trỏ xuống mật khẩu
            }
        }

        // Khi ở txtPassword, nhấn Enter sẽ đăng nhập
        private void TxtPassword_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // tránh tiếng bip
                BtnLogin_Click(sender, EventArgs.Empty); // gọi nút đăng nhập với EventArgs.Empty
            }
        }

        private void FormLogin_Load(object? sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (IsLoginValid(username, password))
            {
                // Đăng nhập thành công
                this.Hide(); // Ẩn form login

                FormMain mainForm = new FormMain();
                mainForm.ShowDialog(); // Hiện form chính dạng modal

                this.Close(); // Đóng form login sau khi form chính đóng
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private bool IsLoginValid(string username, string password)
        {
            bool isValid = false;
            string query = "SELECT COUNT(*) FROM NhanVien WHERE TaiKhoan=@username AND MatKhau=@password";

            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                conn.Open();
                int count = (int)cmd.ExecuteScalar()!;
                isValid = count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isValid;
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
