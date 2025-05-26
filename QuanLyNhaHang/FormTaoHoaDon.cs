using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyNhaHang
{
    public partial class FormTaoHoaDon : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;Trust Server Certificate=True";
        private DataTable dtChiTietHD;

        public FormTaoHoaDon()
        {
            InitializeComponent();

            this.Load += FormTaoHoaDon_Load;
            btnThem.Click += btnThem_Click;
            btnXoa.Click += btnXoa_Click;
            btnSua.Click += btnSua_Click;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;

        }


        private bool KiemTraNguyenLieuDu(int idMonAn, int soLuongMon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Lấy danh sách nguyên liệu và số lượng cần dùng
                string query = @"
                SELECT ct.IDNguyenLieu, (ct.SoLuongDung * @SoLuongMon) AS TongSoLuongCan
                FROM ChiTietNguyenLieuMonAn ct
                WHERE ct.IDMonAn = @IDMonAn";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDMonAn", idMonAn);
                cmd.Parameters.AddWithValue("@SoLuongMon", soLuongMon);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idNguyenLieu = (int)reader["IDNguyenLieu"];
                        decimal soLuongCan = (decimal)reader["TongSoLuongCan"];

                        // Kiểm tra số lượng trong kho
                        decimal soLuongTrongKho = 0m;
                        using (SqlConnection conn2 = new SqlConnection(connectionString))
                        {
                            conn2.Open();
                            string queryKho = "SELECT SoLuongTon FROM NguyenLieu WHERE IDNguyenLieu = @IDNguyenLieu";
                            SqlCommand cmdKho = new SqlCommand(queryKho, conn2);
                            cmdKho.Parameters.AddWithValue("@IDNguyenLieu", idNguyenLieu);
                            var result = cmdKho.ExecuteScalar();
                            soLuongTrongKho = result != null ? Convert.ToDecimal(result) : 0m;
                        }

                        if (soLuongTrongKho < soLuongCan)
                        {
                            return false; // Không đủ nguyên liệu
                        }
                    }
                }
            }
            return true; // Nguyên liệu đủ
        }


        private void LoadSoBan()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDBanAn, SoBan FROM BanAn WHERE TrangThai = N'Trống'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbSoBan.DataSource = dt;
                cmbSoBan.DisplayMember = "SoBan";
                cmbSoBan.ValueMember = "IDBanAn";

                cmbSoBan.SelectedIndex = -1;
                cmbSoBan.Text = "";
            }
        }
        private void FormTaoHoaDon_Load(object sender, EventArgs e)
        {
            InitDataTable();
            LoadNhanVien();
            LoadMonAn();
            LoadSoBan();
            dtpNgayLap.Value = DateTime.Now;
            txtMaHoaDon.Text = GenerateInvoiceCode();
        }

        private void InitDataTable()
        {
            dtChiTietHD = new DataTable();
            dtChiTietHD.Columns.Add("IDMonAn", typeof(int));
            dtChiTietHD.Columns.Add("TenMon", typeof(string));
            dtChiTietHD.Columns.Add("SoLuong", typeof(int));
            dtChiTietHD.Columns.Add("DonGia", typeof(decimal));
            dtChiTietHD.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGia");

            dtgvChiTietHD.DataSource = dtChiTietHD;
        }

        private void LoadNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter daNV = new SqlDataAdapter("SELECT IDNhanVien, TenNV FROM NhanVien", conn);
                DataTable dtNV = new DataTable();
                daNV.Fill(dtNV);

                cmbNhanVien.DataSource = dtNV;
                cmbNhanVien.DisplayMember = "TenNV";  // Hiển thị đúng cột tên nhân viên
                cmbNhanVien.ValueMember = "IDNhanVien";
            }
        }

        private void LoadMonAn()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter daMon = new SqlDataAdapter("SELECT IDMonAn, TenMon, GiaTien FROM MonAn", conn);
                DataTable dtMon = new DataTable();
                daMon.Fill(dtMon);

                cmbMonAn.DataSource = dtMon;
                cmbMonAn.DisplayMember = "TenMon";
                cmbMonAn.ValueMember = "IDMonAn";
            }
        }

        private string GenerateInvoiceCode()
        {
            string prefix = "HD" + DateTime.Now.ToString("yyyyMMdd") + "_";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM HoaDon WHERE MaHoaDon LIKE @prefix + '%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@prefix", prefix);
                int count = (int)cmd.ExecuteScalar();

                return prefix + (count + 1).ToString("D3");  // VD: HD20250525_001
            }
        }

        private void TinhTongTien()
        {
            decimal tongTien = 0;
            if (dtChiTietHD != null && dtChiTietHD.Rows.Count > 0)
            {
                tongTien = dtChiTietHD.AsEnumerable().Sum(row => row.Field<decimal>("ThanhTien"));
            }
            txtTongTien.Text = tongTien.ToString("N0");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cmbMonAn.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn món ăn.");
                return;
            }
            if (nudSoLuong.Value <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0.");
                return;
            }

            int idMonAn = (int)cmbMonAn.SelectedValue;
            int soLuong = (int)nudSoLuong.Value;

            // Kiểm tra nguyên liệu
            if (!KiemTraNguyenLieuDu(idMonAn, soLuong))
            {
                MessageBox.Show("Nguyên liệu không đủ để phục vụ món này.");
                return;
            }

            // Phần thêm món bình thường
            string tenMon = cmbMonAn.Text;
            decimal donGia = (decimal)((DataRowView)cmbMonAn.SelectedItem)["GiaTien"];

            DataRow existingRow = dtChiTietHD.AsEnumerable().FirstOrDefault(r => r.Field<int>("IDMonAn") == idMonAn);
            if (existingRow != null)
            {
                existingRow["SoLuong"] = (int)existingRow["SoLuong"] + soLuong;
            }
            else
            {
                dtChiTietHD.Rows.Add(idMonAn, tenMon, soLuong, donGia);
            }

            TinhTongTien();
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dtgvChiTietHD.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn món để xóa.");
                return;
            }

            int idMonAn = (int)dtgvChiTietHD.CurrentRow.Cells["IDMonAn"].Value;
            DataRow rowToDelete = dtChiTietHD.AsEnumerable().FirstOrDefault(r => r.Field<int>("IDMonAn") == idMonAn);
            if (rowToDelete != null)
            {
                dtChiTietHD.Rows.Remove(rowToDelete);
            }

            TinhTongTien();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dtgvChiTietHD.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn món để sửa.");
                return;
            }

            int idMonAn = (int)dtgvChiTietHD.CurrentRow.Cells["IDMonAn"].Value;
            DataRow rowToEdit = dtChiTietHD.AsEnumerable().FirstOrDefault(r => r.Field<int>("IDMonAn") == idMonAn);
            if (rowToEdit != null)
            {
                if (nudSoLuong.Value <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0.");
                    return;
                }

                rowToEdit["SoLuong"] = (int)nudSoLuong.Value;
                TinhTongTien();
            }
        }

        // Kiểm tra khách hàng đã có chưa
        private int? GetKhachHangIdByName(string tenKhachHang)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDKhachHang FROM KhachHang WHERE TenKH = @TenKH";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenKH", tenKhachHang);

                var result = cmd.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt32(result);
                else
                    return null;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenKhachHang = txtKhachHang.Text.Trim();
            if (string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.");
                return;
            }

            int? idKhachHang = GetKhachHangIdByName(tenKhachHang);

            if (idKhachHang == null)
            {
                DialogResult dr = MessageBox.Show("Khách hàng chưa tồn tại. Bạn có muốn thêm khách hàng mới không?", "Thông báo", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    using (FormKhachHang formThemKH = new FormKhachHang(tenKhachHang))
                    {
                        if (formThemKH.ShowDialog() == DialogResult.OK)
                        {
                            idKhachHang = formThemKH.NewCustomerId;
                        }
                        else
                        {
                            MessageBox.Show("Bạn chưa thêm khách hàng, không thể lưu hóa đơn.");
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng tồn tại hoặc thêm mới.");
                    return;
                }
            }

            // Tiếp tục lưu hóa đơn với idKhachHang.Value
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string maHoaDon = txtMaHoaDon.Text;

                    string insertHD = @"INSERT INTO HoaDon (IDNhanVien, IDKhachHang, NgayLap, TongTien, TrangThai, MaHoaDon)
                        VALUES (@IDNhanVien, @IDKhachHang, @NgayLap, @TongTien, @TrangThai, @MaHoaDon);
                        SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdHD = new SqlCommand(insertHD, conn, transaction);
                    cmdHD.Parameters.AddWithValue("@IDNhanVien", cmbNhanVien.SelectedValue);
                    cmdHD.Parameters.AddWithValue("@IDKhachHang", idKhachHang.Value);
                    cmdHD.Parameters.AddWithValue("@NgayLap", dtpNgayLap.Value);
                    cmdHD.Parameters.AddWithValue("@TongTien", decimal.Parse(txtTongTien.Text));
                    cmdHD.Parameters.AddWithValue("@TrangThai", "Chưa thanh toán");
                    cmdHD.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                    int newIDHoaDon = Convert.ToInt32(cmdHD.ExecuteScalar());

                    foreach (DataRow row in dtChiTietHD.Rows)
                    {
                        string insertCTHD = @"INSERT INTO ChiTietHoaDon (IDHoaDon, IDMonAn, SoLuong, DonGia)
                              VALUES (@IDHoaDon, @IDMonAn, @SoLuong, @DonGia)";
                        SqlCommand cmdCTHD = new SqlCommand(insertCTHD, conn, transaction);
                        cmdCTHD.Parameters.AddWithValue("@IDHoaDon", newIDHoaDon);
                        cmdCTHD.Parameters.AddWithValue("@IDMonAn", (int)row["IDMonAn"]);
                        cmdCTHD.Parameters.AddWithValue("@SoLuong", (int)row["SoLuong"]);
                        cmdCTHD.Parameters.AddWithValue("@DonGia", (decimal)row["DonGia"]);

                        cmdCTHD.ExecuteNonQuery();
                    }

                    // --- Phần thêm mới: cập nhật trạng thái bàn ---
                    if (cmbSoBan.SelectedValue != null)
                    {
                        int idBan = (int)cmbSoBan.SelectedValue;
                        string updateBan = "UPDATE BanAn SET TrangThai = 'Có người' WHERE IDBanAn = @IDBanAn";
                        SqlCommand cmdUpdateBan = new SqlCommand(updateBan, conn, transaction);
                        cmdUpdateBan.Parameters.AddWithValue("@IDBanAn", idBan);
                        cmdUpdateBan.ExecuteNonQuery();
                    }
                    // --- Kết thúc phần cập nhật trạng thái bàn ---

                    transaction.Commit();
                    MessageBox.Show("Lưu hóa đơn thành công!");
                    dtChiTietHD.Clear();
                    txtTongTien.Text = "0";
                    txtKhachHang.Text = "";
                    txtMaHoaDon.Text = GenerateInvoiceCode();

                    LoadSoBan(); // Load lại danh sách bàn còn trống (nếu có hàm này)
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            dtChiTietHD.Clear();
            txtTongTien.Text = "0";
            txtKhachHang.Text = "";
            txtMaHoaDon.Text = GenerateInvoiceCode();
        }
    }
}
