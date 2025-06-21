using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyNhaHang
{
    public partial class FormQuenMatKhau : Form
    {
        public FormQuenMatKhau()
        {
            InitializeComponent();
            txtHoTen.KeyDown += TxtHoTen_KeyDown;
            txtTaiKhoan.KeyDown += TxtTaiKhoan_KeyDown;
            txtEmail.KeyDown += TxtEmail_KeyDown;
        }
        private void TxtHoTen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;  // Ngừng âm thanh Enter
                txtTaiKhoan.Focus();        // Chuyển focus sang txtTaiKhoan
            }
        }

        private void TxtTaiKhoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtEmail.Focus();           // Chuyển focus sang txtEmail
            }
        }

        private void TxtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnGuiMatKhauMoi_Click(sender, EventArgs.Empty);  // Gọi sự kiện gửi mật khẩu mới
            }
        }


        private void btnGuiMatKhauMoi_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string taiKhoan = txtTaiKhoan.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(taiKhoan) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ họ tên, tài khoản và email.");
                return;
            }

            string matKhauMoi = TaoMatKhauNgauNhien(6);

            try
            {
                using (var conn = new SqlConnection(@"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();
                    string query = "UPDATE NhanVien SET MatKhau = @MatKhau, PhaiDoiMatKhau = 1 WHERE Email = @Email AND TenNV = @HoTen AND TaiKhoan = @TaiKhoan";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MatKhau", matKhauMoi);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            MessageBox.Show("Thông tin không chính xác. Không thể đặt lại mật khẩu.");
                            return;
                        }
                    }
                }

                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("poy28032004@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Mật khẩu mới - Quản lý nhà hàng";
                mail.Body = $"Mật khẩu mới của bạn là: {matKhauMoi} \nVui lòng đăng nhập và đổi mật khẩu ngay.";

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("poy28032004@gmail.com", "tngwbnuvbqjftusb");
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);

                MessageBox.Show("Mật khẩu mới đã được gửi đến email.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private string TaoMatKhauNgauNhien(int doDai)
        {
            string kyTu = "0123456789";
            Random rnd = new Random();
            char[] matKhau = new char[doDai];
            for (int i = 0; i < doDai; i++)
            {
                matKhau[i] = kyTu[rnd.Next(kyTu.Length)];
            }
            return new string(matKhau);
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
                this.Close();
        }
    }
}
