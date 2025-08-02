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

            cmbTrangThai.Items.AddRange(new string[] { "Trống", "Có người", "Đã đặt" });

            LoadDanhSachBan();

            btnThemBan.Click += BtnThemBan_Click;
            btnSuaBan.Click += BtnSuaBan_Click;
            btnXoaBan.Click += BtnXoaBan_Click;
            btnLamMoi.Click += btnLamMoi_Click;
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
                string soBan = row["SoBan"].ToString() ?? "";
                string trangThai = row["TrangThai"].ToString() ?? "";

                txtMaSoBan.Text = soBan;
                cmbTrangThai.Text = trangThai;

                if (trangThai == "Có người")
                {
                    int idHoaDon = LayHoaDonChuaThanhToanTheoSoBan(soBan);

                    if (idHoaDon <= 0)
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn đang mở cho bàn này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        CapNhatTrangThaiBanTheoSoBan(soBan, "Trống");
                        clickedButton.Text = $"Bàn {soBan}\nTrống";
                        clickedButton.BackColor = Color.Aqua;
                        row["TrangThai"] = "Trống";
                        clickedButton.Tag = row;
                        return;
                    }

                    FormDSHoaDon frm = new FormDSHoaDon(this);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                    frm.BringToFront();
                    frm.LoadChiTietTheoIDHoaDon(idHoaDon);
                }
                else if (trangThai == "Đã đặt")
                {
                    var thongTin = LayThongTinDatBan(soBan);
                    if (thongTin != null)
                    {
                        MessageBox.Show(
                            $"Tên khách: {thongTin.TenKhach}\n" +
                            $"SĐT: {thongTin.SoDienThoai}\n" +
                            $"Thời gian đặt: {thongTin.ThoiGianDat:HH:mm dd/MM/yyyy}",
                            $"Thông tin đặt bàn {soBan}",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin đặt bàn.", "Thông báo");

                        CapNhatTrangThaiBanTheoSoBan(soBan, "Trống");
                        clickedButton.Text = $"Bàn {soBan}\nTrống";
                        clickedButton.BackColor = Color.Aqua;
                        row["TrangThai"] = "Trống";
                        clickedButton.Tag = row;
                    }
                }
            }
        }


        private ThongTinDatBan? LayThongTinDatBan(string soBan)
        {
            string query = @"
        SELECT TOP 1 
            k.TenKH AS TenKhach, 
            k.SDT AS SoDienThoai,
            DATEADD(SECOND, DATEPART(SECOND, d.GioDat),
                DATEADD(MINUTE, DATEPART(MINUTE, d.GioDat),
                    DATEADD(HOUR, DATEPART(HOUR, d.GioDat), CAST(d.NgayDat AS DATETIME)))) AS ThoiGianDat
        FROM DatBan d
        JOIN BanAn b ON d.IDBanAn = b.IDBanAn
        JOIN KhachHang k ON d.IDKhachHang = k.IDKhachHang
        WHERE b.SoBan = @SoBan AND d.TrangThai = N'Đã đặt'
        ORDER BY d.NgayDat DESC, d.GioDat DESC";

            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SoBan", soBan);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ThongTinDatBan
                {
                    TenKhach = reader["TenKhach"].ToString() ?? "",
                    SoDienThoai = reader["SoDienThoai"].ToString() ?? "",
                    ThoiGianDat = Convert.ToDateTime(reader["ThoiGianDat"])
                };
            }

            return null;
        }


        private int LayHoaDonChuaThanhToanTheoSoBan(string soBan)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT TOP 1 IDHoaDon FROM HoaDon WHERE SoBan = (SELECT IDBanAn FROM BanAn WHERE SoBan = @SoBan) AND TrangThai = N'Chưa thanh toán'";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SoBan", soBan);
            conn.Open();

            object result = cmd.ExecuteScalar();
            conn.Close();

            if (result != null && int.TryParse(result.ToString(), out int idHoaDon))
                return idHoaDon;

            return -1;
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

                string checkQuery = "SELECT COUNT(*) FROM BanAn WHERE SoBan = @SoBan";
                using SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@SoBan", txtMaSoBan.Text.Trim());

                int count = (int)(checkCmd.ExecuteScalar() ?? 0);
                if (count > 0)
                {
                    MessageBox.Show("Bàn này đã tồn tại, vui lòng chọn số bàn khác.");
                    return;
                }

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

        public void CapNhatTrangThaiBanTheoIDBanAn(int idBanAn, string trangThai)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            string query = "UPDATE BanAn SET TrangThai = @TrangThai WHERE IDBanAn = @IDBanAn";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TrangThai", trangThai);
            cmd.Parameters.AddWithValue("@IDBanAn", idBanAn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            LoadDanhSachBan();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSoBan.Clear();
            cmbTrangThai.SelectedIndex = -1;
            LoadDanhSachBan();
        }
    }

    public class ThongTinDatBan
    {
        public string TenKhach { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime ThoiGianDat { get; set; }
    }
}