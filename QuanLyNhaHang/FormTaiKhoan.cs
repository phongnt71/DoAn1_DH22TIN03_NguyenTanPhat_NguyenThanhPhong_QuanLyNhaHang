using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyNhaHang
{
    public partial class FormTaiKhoan : Form
    {
        public FormTaiKhoan(string taiKhoan)
        {
            InitializeComponent();

            _taiKhoan = taiKhoan;
            this.Load += FormTaiKhoan_Load;
            this.btnDoiMK.Click += btnDoiMK_Click;
            txtTenTK.KeyDown += TxtTenTK_KeyDown;
            txtQuyen.KeyDown += TxtQuyen_KeyDown;
            txtTenNV.KeyDown += TxtTenNV_KeyDown;
            txtMKCu.KeyDown += TxtMKCu_KeyDown;
            txtMKMoi.KeyDown += TxtMKMoi_KeyDown;
            txtNhapLaiMK.KeyDown += TxtNhapLaiMK_KeyDown;
        }

        private string _taiKhoan;

        private void FormTaiKhoan_Load(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(@"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True"))
            {
                conn.Open();
                string query = "SELECT TenNV, Quyen FROM NhanVien WHERE TaiKhoan = @tk";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tk", _taiKhoan);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtTenTK.Text = _taiKhoan;
                            txtQuyen.Text = reader["Quyen"].ToString();
                            txtTenNV.Text = reader["TenNV"].ToString(); // Giả sử bạn có TextBox tên là txtTenNV
                        }
                    }
                }
            }

            txtTenTK.ReadOnly = true;
            txtQuyen.ReadOnly = true;
            txtTenNV.ReadOnly = true; // Nếu bạn có textbox tên
        }
        private void TxtTenTK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtQuyen.Focus(); // Chuyển qua txtQuyen
            }
        }

        private void TxtQuyen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtTenNV.Focus(); // Chuyển qua txtTenNV
            }
        }

        private void TxtTenNV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtMKCu.Focus(); // Chuyển qua txtMKCu
            }
        }

        private void TxtMKCu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtMKMoi.Focus(); // Chuyển qua txtMKMoi
            }
        }

        private void TxtMKMoi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtNhapLaiMK.Focus(); // Chuyển qua txtNhapLaiMK
            }
        }

        private void TxtNhapLaiMK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnDoiMK_Click(sender, EventArgs.Empty); // Thực hiện đổi mật khẩu
            }
        }


        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            string matKhauCu = txtMKCu.Text.Trim();
            string matKhauMoi = txtMKMoi.Text.Trim();
            string nhapLaiMK = txtNhapLaiMK.Text.Trim();

            if (string.IsNullOrWhiteSpace(matKhauCu) || string.IsNullOrWhiteSpace(matKhauMoi) || string.IsNullOrWhiteSpace(nhapLaiMK))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi != nhapLaiMK)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(@"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();

                    // Kiểm tra mật khẩu cũ
                    string queryCheck = "SELECT COUNT(*) FROM NhanVien WHERE TaiKhoan = @tk AND MatKhau = @mk";
                    using (var cmdCheck = new SqlCommand(queryCheck, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@tk", _taiKhoan);
                        cmdCheck.Parameters.AddWithValue("@mk", matKhauCu);
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("Mật khẩu cũ không đúng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Cập nhật mật khẩu mới và tắt chế độ bắt đổi mật khẩu
                    string queryUpdate = "UPDATE NhanVien SET MatKhau = @mkm, PhaiDoiMatKhau = 0 WHERE TaiKhoan = @tk";
                    using (var cmdUpdate = new SqlCommand(queryUpdate, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@tk", _taiKhoan);
                        cmdUpdate.Parameters.AddWithValue("@mkm", matKhauMoi);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đổi mật khẩu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
}
