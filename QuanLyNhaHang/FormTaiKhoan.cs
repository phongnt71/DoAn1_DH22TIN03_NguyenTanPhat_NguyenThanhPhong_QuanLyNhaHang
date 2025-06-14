using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyNhaHang
{
    public partial class FormTaiKhoan : Form
    {
        public FormTaiKhoan()
        {
            InitializeComponent();
            this.Load += FormTaiKhoan_Load;
            this.btnDoiMK.Click += btnDoiMK_Click;
        }

        private void FormTaiKhoan_Load(object sender, EventArgs e)
        {
            txtTenTK.Text = Program.TaiKhoanDangNhap;
            txtQuyen.Text = Program.VaiTroDangNhap;
            txtTenTK.ReadOnly = true;
            txtQuyen.ReadOnly = true;
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            string user = txtTenTK.Text.Trim();
            string role = txtQuyen.Text.Trim();
            string mkCu = txtMKCu.Text.Trim();
            string mkMoi = txtMKMoi.Text.Trim();
            string mkLai = txtNhapLaiMK.Text.Trim();

            if (string.IsNullOrEmpty(mkCu) || string.IsNullOrEmpty(mkMoi) || string.IsNullOrEmpty(mkLai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DatabaseHelper.KiemTraMatKhauCu(user, mkCu))
            {
                MessageBox.Show("Mật khẩu cũ không đúng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (mkMoi != mkLai)
            {
                MessageBox.Show("Mật khẩu mới không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DatabaseHelper.CapNhatMatKhauMoi(user, mkMoi))
            {
                MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Đổi mật khẩu thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public static class DatabaseHelper
    {
        private static string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True";

        public static bool KiemTraMatKhauCu(string username, string oldPassword)
        {
            string query = "SELECT COUNT(*) FROM NhanVien WHERE TaiKhoan=@user AND MatKhau=@pass";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", oldPassword);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public static bool CapNhatMatKhauMoi(string username, string newPassword)
        {
            string query = "UPDATE NhanVien SET MatKhau=@newPass WHERE TaiKhoan=@user";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@newPass", newPassword);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
