using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyNhaHang
{
    public partial class FormTaoHoaDon : Form
    {
        private int currentIdHoaDon = -1;
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;Trust Server Certificate=True";
        private DataTable dtChiTietHD;

        public FormTaoHoaDon()
        {
            InitializeComponent();
            LoadBanDatTruoc();
            this.Load += FormTaoHoaDon_Load;
            btnThem.Click += btnThem_Click;
            btnXoa.Click += btnXoa_Click;
            btnSua.Click += btnSua_Click;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;
            chkMangVe.CheckedChanged += chkMangVe_CheckedChanged;
        }

        // Cập nhật nguyên liệu sau khi lưu hóa đơn thành công
        private void CapNhatNguyenLieuSauKhiTaoHoaDon(int idHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string queryChiTiet = @"
                    SELECT ct.IDMonAn, ct.SoLuong
                    FROM ChiTietHoaDon ct
                    WHERE ct.IDHoaDon = @IDHoaDon";

                SqlCommand cmdChiTiet = new SqlCommand(queryChiTiet, conn);
                cmdChiTiet.Parameters.AddWithValue("@IDHoaDon", idHoaDon);

                SqlDataReader reader = cmdChiTiet.ExecuteReader();

                DataTable dtChiTiet = new DataTable();
                dtChiTiet.Load(reader);

                reader.Close();

                foreach (DataRow row in dtChiTiet.Rows)
                {
                    int idMonAn = Convert.ToInt32(row["IDMonAn"]);
                    int soLuongMonAn = Convert.ToInt32(row["SoLuong"]);

                    // Sửa trường cập nhật thành SoLuongTon đúng với database
                    string queryTruNguyenLieu = @"
                        UPDATE NguyenLieu
                        SET SoLuongTon = SoLuongTon - (ctnl.SoLuongDung * @SoLuongMonAn)
                        FROM ChiTietNguyenLieuMonAn ctnl
                        WHERE NguyenLieu.IDNguyenLieu = ctnl.IDNguyenLieu
                        AND ctnl.IDMonAn = @IDMonAn";

                    SqlCommand cmdTruNguyenLieu = new SqlCommand(queryTruNguyenLieu, conn);
                    cmdTruNguyenLieu.Parameters.AddWithValue("@SoLuongMonAn", soLuongMonAn);
                    cmdTruNguyenLieu.Parameters.AddWithValue("@IDMonAn", idMonAn);

                    cmdTruNguyenLieu.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        private void chkMangVe_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMangVe.Checked)
            {
                cmbSoBan.Enabled = false;
                cmbSoBan.SelectedIndex = -1;
                cmbSoBan.Text = "";

                cmbBanDatTruoc.Enabled = false;
                cmbBanDatTruoc.SelectedIndex = -1;
                cmbBanDatTruoc.Text = "";
            }
            else
            {
                cmbSoBan.Enabled = true;
                cmbBanDatTruoc.Enabled = true;
            }
        }

        // Kiểm tra nguyên liệu đủ cho món ăn chưa
        private bool KiemTraNguyenLieuDu(int idMonAn, int soLuongMon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

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
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void LoadBanDatTruoc()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDBanAn, SoBan FROM BanAn WHERE TrangThai = N'Đã đặt'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbBanDatTruoc.DataSource = dt;
                cmbBanDatTruoc.DisplayMember = "SoBan";
                cmbBanDatTruoc.ValueMember = "IDBanAn";
                cmbBanDatTruoc.SelectedIndex = -1;
            }
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
            cmbBanDatTruoc.SelectedIndexChanged += cmbBanDatTruoc_SelectedIndexChanged;
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

        private void cmbBanDatTruoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBanDatTruoc.SelectedValue != null && int.TryParse(cmbBanDatTruoc.SelectedValue.ToString(), out int idBanAn))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT k.TenKH
                             FROM DatBan d
                             JOIN KhachHang k ON d.IDKhachHang = k.IDKhachHang
                             WHERE d.IDBanAn = @IDBanAn AND d.TrangThai = N'Đã đặt'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IDBanAn", idBanAn);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        txtKhachHang.Text = result.ToString();
                        cmbSoBan.SelectedValue = idBanAn;
                    }
                }
            }
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
                cmbNhanVien.DisplayMember = "TenNV";
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

                return prefix + (count + 1).ToString("D3");
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

            if (!KiemTraNguyenLieuDu(idMonAn, soLuong))
            {
                MessageBox.Show("Nguyên liệu không đủ để phục vụ món này.");
                return;
            }

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
            if ((cmbSoBan.SelectedIndex == -1 && cmbBanDatTruoc.SelectedIndex == -1) && !chkMangVe.Checked)
            {
                MessageBox.Show("Vui lòng chọn bàn hoặc tích vào 'Mang về' trước khi lưu hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string maHoaDon = txtMaHoaDon.Text;

                    // Lấy giá trị số bàn
                    int? idBan = null;

                    if (!chkMangVe.Checked)
                    {
                        if (cmbBanDatTruoc.SelectedValue != null)
                            idBan = (int)cmbBanDatTruoc.SelectedValue;
                        else if (cmbSoBan.SelectedValue != null)
                            idBan = (int)cmbSoBan.SelectedValue;
                    }

                    string insertHD = @"INSERT INTO HoaDon (IDNhanVien, IDKhachHang, NgayLap, TongTien, TrangThai, MaHoaDon, SoBan)
                                VALUES (@IDNhanVien, @IDKhachHang, @NgayLap, @TongTien, @TrangThai, @MaHoaDon, @SoBan);
                                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdHD = new SqlCommand(insertHD, conn, transaction);
                    cmdHD.Parameters.AddWithValue("@IDNhanVien", cmbNhanVien.SelectedValue);
                    cmdHD.Parameters.AddWithValue("@IDKhachHang", idKhachHang.Value);
                    cmdHD.Parameters.AddWithValue("@NgayLap", dtpNgayLap.Value);
                    cmdHD.Parameters.AddWithValue("@TongTien", decimal.Parse(txtTongTien.Text));
                    cmdHD.Parameters.AddWithValue("@TrangThai", "Chưa thanh toán");
                    cmdHD.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    cmdHD.Parameters.AddWithValue("@SoBan", (object)idBan ?? DBNull.Value);

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

                    if (!chkMangVe.Checked)
                    {
                        if (idBan.HasValue)
                        {
                            string updateBan = "UPDATE BanAn SET TrangThai = N'Có người' WHERE IDBanAn = @IDBanAn";
                            SqlCommand cmdUpdateBan = new SqlCommand(updateBan, conn, transaction);
                            cmdUpdateBan.Parameters.AddWithValue("@IDBanAn", idBan.Value);
                            cmdUpdateBan.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();

                    // Cập nhật nguyên liệu sau khi lưu hóa đơn thành công
                    CapNhatNguyenLieuSauKhiTaoHoaDon(newIDHoaDon);

                    MessageBox.Show("Lưu hóa đơn thành công!");
                    dtChiTietHD.Clear();
                    txtTongTien.Text = "0";
                    txtKhachHang.Text = "";
                    txtMaHoaDon.Text = GenerateInvoiceCode();
                    LoadSoBan();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch { }
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
