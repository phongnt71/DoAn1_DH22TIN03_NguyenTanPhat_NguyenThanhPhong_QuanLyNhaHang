using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class FormQuanLyBan : Form
    {
        private readonly string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True";

        public FormQuanLyBan()
        {
            InitializeComponent();

            // Chỉ thêm 3 trạng thái mong muốn cho combobox
            cmbTrangThai.Items.AddRange(new string[] { "Trống", "Có người", "Đã đặt" });

            LoadDanhSachBan();

            btnThemBan.Click += BtnThemBan_Click;
            btnSuaBan.Click += BtnSuaBan_Click;
            btnXoaBan.Click += BtnXoaBan_Click;
            btnLamMoi.Click += btnLamMoi_Click; // Thêm sự kiện cho nút làm mới
        }

        public void LoadDanhSachBan()
        {
            flpBanAn.Controls.Clear();

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                string query = "SELECT * FROM BanAn";
                using SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    Button btnBan = new Button
                    {
                        Width = 100,
                        Height = 80,
                        Margin = new Padding(5),
                        Text = $"Bàn {row["SoBan"]}\n{row["TrangThai"]}",
                        Tag = row
                    };

                    string trangThai = row["TrangThai"].ToString() ?? string.Empty;
                    if (trangThai == "Trống")
                        btnBan.BackColor = Color.Aqua;
                    else if (trangThai == "Có người")
                        btnBan.BackColor = Color.LightCoral;
                    else if (trangThai == "Đã đặt")
                        btnBan.BackColor = Color.Gold;
                    else
                        btnBan.BackColor = Color.Gray;

                    btnBan.Click += BtnBan_Click;

                    flpBanAn.Controls.Add(btnBan);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách bàn: " + ex.Message);
            }
        }

        private void BtnBan_Click(object? sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is DataRow row)
            {
                txtMaSoBan.Text = row["SoBan"].ToString() ?? string.Empty;
                cmbTrangThai.Text = row["TrangThai"].ToString() ?? string.Empty;
            }
        }

        private void BtnThemBan_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSoBan.Text) || cmbTrangThai.SelectedItem is not string trangThai)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                // Kiểm tra bàn đã tồn tại chưa
                string checkQuery = "SELECT COUNT(*) FROM BanAn WHERE SoBan = @SoBan";
                using SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@SoBan", txtMaSoBan.Text.Trim());

                int count = (int)(checkCmd.ExecuteScalar() ?? 0);
                if (count > 0)
                {
                    MessageBox.Show("Bàn này đã tồn tại, vui lòng chọn số bàn khác.");
                    return;
                }

                // Thêm bàn mới
                string insertQuery = "INSERT INTO BanAn (SoBan, TrangThai) VALUES (@SoBan, @TrangThai)";
                using SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@SoBan", txtMaSoBan.Text.Trim());
                insertCmd.Parameters.AddWithValue("@TrangThai", trangThai);

                insertCmd.ExecuteNonQuery();

                MessageBox.Show("Thêm bàn thành công.");
                LoadDanhSachBan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm bàn: " + ex.Message);
            }
        }

        private void BtnSuaBan_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSoBan.Text) || cmbTrangThai.SelectedItem is not string trangThai)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                string query = "UPDATE BanAn SET TrangThai = @TrangThai WHERE SoBan = @SoBan";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoBan", txtMaSoBan.Text.Trim());
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Sửa bàn thành công.");
                    LoadDanhSachBan();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bàn để sửa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa bàn: " + ex.Message);
            }
        }

        private void BtnXoaBan_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSoBan.Text))
            {
                MessageBox.Show("Vui lòng nhập mã số bàn cần xóa.");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xóa bàn này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
                return;

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                using SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string deleteDatBanQuery = "DELETE FROM DatBan WHERE IDBanAn = (SELECT IDBanAn FROM BanAn WHERE SoBan = @SoBan)";
                    using SqlCommand deleteDatBanCmd = new SqlCommand(deleteDatBanQuery, conn, transaction);
                    deleteDatBanCmd.Parameters.AddWithValue("@SoBan", txtMaSoBan.Text.Trim());
                    deleteDatBanCmd.ExecuteNonQuery();

                    string deleteBanAnQuery = "DELETE FROM BanAn WHERE SoBan = @SoBan";
                    using SqlCommand deleteBanAnCmd = new SqlCommand(deleteBanAnQuery, conn, transaction);
                    deleteBanAnCmd.Parameters.AddWithValue("@SoBan", txtMaSoBan.Text.Trim());
                    int rows = deleteBanAnCmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        transaction.Commit();
                        MessageBox.Show("Xóa bàn thành công.");
                        LoadDanhSachBan();
                    }
                    else
                    {
                        transaction.Rollback();
                        MessageBox.Show("Không tìm thấy bàn để xóa.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi xóa bàn: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message);
            }
        }

        // Hàm cập nhật trạng thái bàn theo số bàn và trạng thái mới
        public void CapNhatTrangThaiBanTheoSoBan(string soBan, string trangThaiMoi)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                string query = "UPDATE BanAn SET TrangThai = @TrangThai WHERE SoBan = @SoBan";
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThaiMoi);
                cmd.Parameters.AddWithValue("@SoBan", soBan);

                cmd.ExecuteNonQuery();

                LoadDanhSachBan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật trạng thái bàn: " + ex.Message);
            }
        }

        // Hàm cập nhật trạng thái bàn theo IDBanAn (thêm mới)
        public void CapNhatTrangThaiBanTheoIDBanAn(int idBanAn, string trangThai)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE BanAn SET TrangThai = @TrangThai WHERE IDBanAn = @IDBanAn";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                cmd.Parameters.AddWithValue("@IDBanAn", idBanAn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Load lại danh sách bàn để cập nhật UI
            LoadDanhSachBan();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Xóa text ở textbox Mã số bàn
            txtMaSoBan.Clear();

            // Reset trạng thái combobox về chưa chọn
            cmbTrangThai.SelectedIndex = -1;

            // Tải lại danh sách bàn, để refresh hiển thị mới nhất
            LoadDanhSachBan();
        }

        private void FormQuanLyBan_Load(object sender, EventArgs e)
        {

        }
    }
}
