using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyNhaHang
{
    public partial class FormLogin : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True";

        public FormLogin()
        {
            InitializeComponent();
            this.Load += FormLogin_Load;
            txtUsername.KeyDown += TxtUsername_KeyDown;
            txtPassword.KeyDown += TxtPassword_KeyDown;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();

            string logoPath = Path.Combine(Application.StartupPath, "Images", "LogoLogin.png");
            if (File.Exists(logoPath))
            {
                pictureBoxLogo.Image = Image.FromFile(logoPath);
            }
        }

        private void TxtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtPassword.Focus();
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnLogin_Click(sender, EventArgs.Empty);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Quyen, PhaiDoiMatKhau FROM NhanVien WHERE TaiKhoan = @tk AND MatKhau = @mk";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tk", username);
                        cmd.Parameters.AddWithValue("@mk", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string quyen = reader["Quyen"]?.ToString() ?? "";
                                bool phaiDoiMK = false;

                                if (reader["PhaiDoiMatKhau"] != DBNull.Value)
                                {
                                    phaiDoiMK = Convert.ToBoolean(reader["PhaiDoiMatKhau"]);
                                }

                                // Ghi nhớ thông tin đăng nhập
                                Program.TaiKhoanDangNhap = username;
                                Program.VaiTroDangNhap = quyen;

                                if (phaiDoiMK)
                                {
                                    MessageBox.Show("Bạn cần đổi mật khẩu trước khi tiếp tục.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Hide();
                                    FormTaiKhoan doiMK = new FormTaiKhoan(username);
                                    doiMK.ShowDialog();
                                    this.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Đăng nhập thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Hide();
                                    FormMain frmMain = new FormMain();
                                    frmMain.ShowDialog();
                                    this.Show();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void linkQuenMatKhau_Click(object sender, EventArgs e)
        {
            FormQuenMatKhau frm = new FormQuenMatKhau();
            frm.ShowDialog();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}
