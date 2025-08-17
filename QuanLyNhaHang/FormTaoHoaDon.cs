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
        private string connectionString =
            @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;Trust Server Certificate=True";

        private DataTable dtChiTietHD;
        private bool isAdding = false;
        private bool isEditing = false;

        // Chặn vòng lặp sự kiện khi bật/tắt control
        private bool _updatingUI = false;

        public FormTaoHoaDon()
        {
            InitializeComponent();

            // Sự kiện
            this.Load += FormTaoHoaDon_Load;
            btnThem.Click += btnThem_Click;
            btnXoa.Click += btnXoa_Click;
            btnSua.Click += btnSua_Click;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnCapNhat.Click += btnCapNhat_Click;

            // Ràng buộc 3 lựa chọn
            chkMangVe.CheckedChanged += chkMangVe_CheckedChanged;
            cmbBanDatTruoc.SelectedIndexChanged += cmbBanDatTruoc_SelectedIndexChanged;
            cmbSoBan.SelectedIndexChanged += cmbSoBan_SelectedIndexChanged;

            // Gợi ý cho thời gian dự kiến (nếu control đã kéo vào form)
            if (nudThoiGianDuKien != null)
            {
                nudThoiGianDuKien.Minimum = 15;
                nudThoiGianDuKien.Maximum = 480;
                nudThoiGianDuKien.Increment = 15;
                if (nudThoiGianDuKien.Value < 15) nudThoiGianDuKien.Value = 60;
            }
        }

        // ================== BẬT/TẮT CÁC LỰA CHỌN & THỜI GIAN DỰ KIẾN ==================
        private void ApplySelectionMode()
        {
            if (_updatingUI) return;
            _updatingUI = true;

            bool hasDatTruoc = cmbBanDatTruoc.SelectedIndex >= 0;
            bool hasSoBan = cmbSoBan.SelectedIndex >= 0;
            bool isTakeaway = chkMangVe.Checked;

            if (isTakeaway)
            {
                cmbBanDatTruoc.SelectedIndex = -1;
                cmbSoBan.SelectedIndex = -1;
                cmbBanDatTruoc.Enabled = false;
                cmbSoBan.Enabled = false;

                if (nudThoiGianDuKien != null)
                {
                    nudThoiGianDuKien.Enabled = false;
                    nudThoiGianDuKien.Value = 60;
                }
                chkMangVe.Enabled = true;
            }
            else if (hasDatTruoc)
            {
                cmbSoBan.SelectedIndex = -1;
                cmbSoBan.Enabled = false;
                chkMangVe.Checked = false;
                chkMangVe.Enabled = false;
                cmbBanDatTruoc.Enabled = true;

                if (nudThoiGianDuKien != null)
                {
                    nudThoiGianDuKien.Enabled = false; // theo bàn đặt trước, không dùng thời lượng tự chọn
                }
            }
            else if (hasSoBan)
            {
                cmbBanDatTruoc.SelectedIndex = -1;
                cmbBanDatTruoc.Enabled = false;
                chkMangVe.Checked = false;
                chkMangVe.Enabled = false;
                cmbSoBan.Enabled = true;

                if (nudThoiGianDuKien != null)
                {
                    nudThoiGianDuKien.Enabled = true;  // chỉ dùng khi chọn Số bàn trống
                    if (nudThoiGianDuKien.Value < 15) nudThoiGianDuKien.Value = 60;
                }
            }
            else
            {
                cmbBanDatTruoc.Enabled = true;
                cmbSoBan.Enabled = true;
                chkMangVe.Enabled = true;

                if (nudThoiGianDuKien != null)
                {
                    nudThoiGianDuKien.Enabled = false;
                    nudThoiGianDuKien.Value = 60;
                }
            }

            _updatingUI = false;
        }

        private void chkMangVe_CheckedChanged(object sender, EventArgs e) => ApplySelectionMode();
        private void cmbSoBan_SelectedIndexChanged(object sender, EventArgs e) => ApplySelectionMode();

        // ================== NGUYÊN LIỆU SAU LƯU ==================
        private void CapNhatNguyenLieuSauKhiTaoHoaDon(int idHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string queryChiTiet = @"
                    SELECT ct.IDMonAn, ct.SoLuong
                    FROM ChiTietHoaDon ct
                    WHERE ct.IDHoaDon = @IDHoaDon";

                using (var cmdChiTiet = new SqlCommand(queryChiTiet, conn))
                {
                    cmdChiTiet.Parameters.AddWithValue("@IDHoaDon", idHoaDon);
                    using (var reader = cmdChiTiet.ExecuteReader())
                    {
                        var dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            int idMonAn = Convert.ToInt32(row["IDMonAn"]);
                            int soLuongMonAn = Convert.ToInt32(row["SoLuong"]);

                            string queryTru = @"
                                UPDATE NguyenLieu
                                SET SoLuongTon = SoLuongTon - (ctnl.SoLuongDung * @SoLuongMonAn)
                                FROM ChiTietNguyenLieuMonAn ctnl
                                WHERE NguyenLieu.IDNguyenLieu = ctnl.IDNguyenLieu
                                AND ctnl.IDMonAn = @IDMonAn";

                            using (var cmdTru = new SqlCommand(queryTru, conn))
                            {
                                cmdTru.Parameters.AddWithValue("@SoLuongMonAn", soLuongMonAn);
                                cmdTru.Parameters.AddWithValue("@IDMonAn", idMonAn);
                                cmdTru.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        // ================== KIỂM TRA NGUYÊN LIỆU ==================
        private bool KiemTraNguyenLieuDu(int idMonAn, int soLuongMon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
            SELECT ct.IDNguyenLieu, (ct.SoLuongDung * @SoLuongMon) AS TongSoLuongCan
            FROM ChiTietNguyenLieuMonAn ct
            WHERE ct.IDMonAn = @IDMonAn";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDMonAn", idMonAn);
                    cmd.Parameters.AddWithValue("@SoLuongMon", soLuongMon);

                    using (var reader = cmd.ExecuteReader())
                    {
                        var req = new DataTable();
                        req.Load(reader);

                        foreach (DataRow r in req.Rows)
                        {
                            int idNguyenLieu = (int)r["IDNguyenLieu"];
                            decimal soLuongCan = (decimal)r["TongSoLuongCan"];

                            using (var cmdKho = new SqlCommand(
                                "SELECT SoLuongTon FROM NguyenLieu WHERE IDNguyenLieu = @ID", conn))
                            {
                                cmdKho.Parameters.AddWithValue("@ID", idNguyenLieu);
                                object rs = cmdKho.ExecuteScalar();
                                decimal soLuongTrongKho = rs != null ? Convert.ToDecimal(rs) : 0m;

                                if (soLuongTrongKho < soLuongCan)
                                    return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        // ================== LOAD DỮ LIỆU BÀN ==================
        private void LoadSoBan()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                const string query = "SELECT IDBanAn, SoBan FROM BanAn WHERE TrangThai = N'Trống'";
                var da = new SqlDataAdapter(query, conn);
                var dt = new DataTable();
                da.Fill(dt);

                cmbSoBan.DataSource = dt;
                cmbSoBan.DisplayMember = "SoBan";
                cmbSoBan.ValueMember = "IDBanAn";
                cmbSoBan.SelectedIndex = -1;
                cmbSoBan.Text = "";
            }
        }

        private void LoadSoBan(int? selectedID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDBanAn, SoBan FROM BanAn WHERE TrangThai = N'Trống'";
                if (selectedID.HasValue) query += " OR IDBanAn = @IDBanAn";

                var cmd = new SqlCommand(query, conn);
                if (selectedID.HasValue) cmd.Parameters.AddWithValue("@IDBanAn", selectedID.Value);

                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);

                cmbSoBan.DataSource = dt;
                cmbSoBan.DisplayMember = "SoBan";
                cmbSoBan.ValueMember = "IDBanAn";
                cmbSoBan.SelectedIndex = -1;
                cmbSoBan.Text = "";
            }
        }

        // Bàn đặt trước: CHỈ hiện các slot ĐÃ ĐẶT nhưng bàn CHƯA “Có người”
        private void LoadBanDatTruoc(int? keepSelectedIdBanAn = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
SELECT 
    d.IDDatBan,
    b.IDBanAn,
    b.SoBan,
    d.NgayDat,
    d.GioDat,
    k.TenKH
FROM DatBan d
JOIN BanAn b ON b.IDBanAn = d.IDBanAn
JOIN KhachHang k ON k.IDKhachHang = d.IDKhachHang
WHERE d.TrangThai = N'Đã đặt'
  AND (b.TrangThai IS NULL OR b.TrangThai <> N'Có người')
ORDER BY d.NgayDat ASC, d.GioDat ASC, b.SoBan ASC";

                var da = new SqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);

                if (!dt.Columns.Contains("Display"))
                    dt.Columns.Add("Display", typeof(string));

                foreach (DataRow r in dt.Rows)
                {
                    int soBan = Convert.ToInt32(r["SoBan"]);
                    DateTime ngay = Convert.ToDateTime(r["NgayDat"]);
                    TimeSpan gio = (TimeSpan)r["GioDat"];
                    r["Display"] = $"Bàn {soBan} - {ngay:dd/MM/yyyy} {gio.Hours:00}:{gio.Minutes:00}";
                }

                cmbBanDatTruoc.DataSource = dt;
                cmbBanDatTruoc.DisplayMember = "Display";
                cmbBanDatTruoc.ValueMember = "IDBanAn"; // dùng IDBanAn làm Value
                cmbBanDatTruoc.SelectedIndex = -1;

                if (keepSelectedIdBanAn.HasValue)
                    cmbBanDatTruoc.SelectedValue = keepSelectedIdBanAn.Value;
            }
        }

        // ================== FORM LOAD ==================
        private void FormTaoHoaDon_Load(object sender, EventArgs e)
        {
            InitDataTable();
            LoadNhanVien();
            LoadMonAn();
            LoadSoBan();
            LoadBanDatTruoc(null);

            dtpNgayLap.Value = DateTime.Now;
            txtMaHoaDon.Text = GenerateInvoiceCode();
            LoadHoaDonDaTao();

            // Clear sạch khi vào form
            ResetForm();

            ApplySelectionMode();
        }

        private void LoadHoaDonDaTao()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
SELECT h.MaHoaDon, k.TenKH, h.NgayLap, h.TongTien
FROM HoaDon h
JOIN KhachHang k ON h.IDKhachHang = k.IDKhachHang
WHERE h.TrangThai = N'Nợ' OR h.TrangThai = N'Chưa thanh toán'";

                var da = new SqlDataAdapter(query, conn);
                var dt = new DataTable();
                da.Fill(dt);

                dtgvHoaDonDaTao.DataSource = dt;
                dtgvHoaDonDaTao.CellClick -= dtgvHoaDonDaTao_CellClick;
                dtgvHoaDonDaTao.CellClick += dtgvHoaDonDaTao_CellClick;
            }
        }

        private void dtgvHoaDonDaTao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maHoaDon = dtgvHoaDonDaTao.Rows[e.RowIndex].Cells["MaHoaDon"].Value.ToString();
                LoadHoaDonLenForm(maHoaDon);
                isEditing = true;
                SetButtonState(true);
            }
        }

        private void LoadHoaDonLenForm(string maHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string queryHD = @"SELECT IDHoaDon, IDNhanVien, IDKhachHang, NgayLap, TongTien, SoBan, GhiChu
                                   FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
                using (var cmd = new SqlCommand(queryHD, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentIdHoaDon = (int)reader["IDHoaDon"];
                            cmbNhanVien.SelectedValue = reader["IDNhanVien"];
                            dtpNgayLap.Value = Convert.ToDateTime(reader["NgayLap"]);
                            txtTongTien.Text = ((decimal)reader["TongTien"]).ToString("N0");
                            txtMaHoaDon.Text = maHoaDon;
                            txtGhiChu.Text = reader["GhiChu"].ToString();

                            int? soBan = reader["SoBan"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SoBan"]) : null;
                            if (soBan.HasValue)
                            {
                                LoadSoBan(soBan);
                                LoadBanDatTruoc(soBan);
                                cmbSoBan.SelectedValue = soBan;
                                chkMangVe.Checked = false;
                            }
                            else
                            {
                                LoadSoBan();
                                LoadBanDatTruoc(null);
                                cmbSoBan.SelectedIndex = -1;
                                chkMangVe.Checked = true;
                            }

                            int idKH = (int)reader["IDKhachHang"];
                            reader.Close();

                            using (var cmdKH = new SqlCommand("SELECT TenKH FROM KhachHang WHERE IDKhachHang=@id", conn))
                            {
                                cmdKH.Parameters.AddWithValue("@id", idKH);
                                object tenKH = cmdKH.ExecuteScalar();
                                txtKhachHang.Text = tenKH != null ? tenKH.ToString() : "";
                            }

                            string queryCT = @"SELECT c.IDMonAn, m.TenMon, c.SoLuong, c.DonGia
                                               FROM ChiTietHoaDon c
                                               JOIN MonAn m ON c.IDMonAn = m.IDMonAn
                                               WHERE c.IDHoaDon = @IDHoaDon";
                            using (var cmdCT = new SqlCommand(queryCT, conn))
                            {
                                cmdCT.Parameters.AddWithValue("@IDHoaDon", currentIdHoaDon);
                                var da = new SqlDataAdapter(cmdCT);
                                var dt = new DataTable();
                                da.Fill(dt);

                                dt.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGia");
                                dtChiTietHD = dt;
                                dtgvChiTietHD.DataSource = dtChiTietHD;
                            }
                        }
                    }
                }
            }

            TinhTongTien();
            ApplySelectionMode();
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

        // Khi chọn slot “Bàn đặt trước”: set số bàn tương ứng + tên KH (nếu có)
        private void cmbBanDatTruoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBanDatTruoc.SelectedIndex >= 0 && cmbBanDatTruoc.SelectedItem is DataRowView drv)
            {
                int idBanAn = Convert.ToInt32(drv["IDBanAn"]);
                cmbSoBan.SelectedValue = idBanAn;

                string tenKH = drv["TenKH"]?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(tenKH)) txtKhachHang.Text = tenKH;
            }
            ApplySelectionMode();
        }

        private void LoadNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var daNV = new SqlDataAdapter("SELECT IDNhanVien, TenNV FROM NhanVien", conn);
                var dtNV = new DataTable();
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
                var daMon = new SqlDataAdapter("SELECT IDMonAn, TenMon, GiaTien FROM MonAn", conn);
                var dtMon = new DataTable();
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
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM HoaDon WHERE MaHoaDon LIKE @prefix + '%'", conn))
                {
                    cmd.Parameters.AddWithValue("@prefix", prefix);
                    int count = (int)cmd.ExecuteScalar();
                    return prefix + (count + 1).ToString("D3");
                }
            }
        }

        private void TinhTongTien()
        {
            decimal tongTien = 0;
            if (dtChiTietHD != null && dtChiTietHD.Rows.Count > 0)
                tongTien = dtChiTietHD.AsEnumerable().Sum(r => r.Field<decimal>("ThanhTien"));
            txtTongTien.Text = tongTien.ToString("N0");
        }

        // ================== CHI TIẾT MÓN ==================
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

            DataRow existing = dtChiTietHD.AsEnumerable().FirstOrDefault(r => r.Field<int>("IDMonAn") == idMonAn);
            if (existing != null)
                existing["SoLuong"] = (int)existing["SoLuong"] + soLuong;
            else
                dtChiTietHD.Rows.Add(idMonAn, tenMon, soLuong, donGia);

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
            if (rowToDelete != null) dtChiTietHD.Rows.Remove(rowToDelete);

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
            DataRow row = dtChiTietHD.AsEnumerable().FirstOrDefault(r => r.Field<int>("IDMonAn") == idMonAn);
            if (row != null)
            {
                if (nudSoLuong.Value <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0.");
                    return;
                }
                row["SoLuong"] = (int)nudSoLuong.Value;
                TinhTongTien();
            }
        }

        // ================== LƯU/CẬP NHẬT HÓA ĐƠN ==================
        private int? GetKhachHangIdByName(string tenKhachHang)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT IDKhachHang FROM KhachHang WHERE TenKH = @TenKH", conn))
                {
                    cmd.Parameters.AddWithValue("@TenKH", tenKhachHang);
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
        }

        private void ResetForm()
        {
            txtKhachHang.Text = "";
            txtGhiChu.Text = "";
            txtTongTien.Text = "0";
            txtMaHoaDon.Text = GenerateInvoiceCode();
            dtChiTietHD.Clear();

            cmbSoBan.SelectedIndex = -1;
            cmbBanDatTruoc.SelectedIndex = -1;
            chkMangVe.Checked = false;
            cmbNhanVien.SelectedIndex = -1;

            if (nudThoiGianDuKien != null)
            {
                nudThoiGianDuKien.Value = 60;
                nudThoiGianDuKien.Enabled = false;
            }

            ApplySelectionMode();
        }

        private void SetButtonState(bool isEditingEnabled)
        {
            btnLuu.Enabled = !isEditingEnabled;
            btnCapNhat.Enabled = isEditingEnabled;
            btnHuy.Enabled = true;
            btnLamMoi.Enabled = true;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            isAdding = false;
            isEditing = false;

            ResetForm();
            LoadSoBan();
            LoadBanDatTruoc(null);
            SetButtonState(false);
            ApplySelectionMode();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (currentIdHoaDon <= 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần chỉnh sửa từ danh sách.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                try
                {
                    using (var deleteCmd = new SqlCommand("DELETE FROM ChiTietHoaDon WHERE IDHoaDon=@ID", conn, transaction))
                    {
                        deleteCmd.Parameters.AddWithValue("@ID", currentIdHoaDon);
                        deleteCmd.ExecuteNonQuery();
                    }

                    foreach (DataRow row in dtChiTietHD.Rows)
                    {
                        string insertQuery = @"
                            INSERT INTO ChiTietHoaDon (IDHoaDon, IDMonAn, SoLuong, DonGia) 
                            VALUES (@IDHoaDon, @IDMonAn, @SoLuong, @DonGia)";
                        using (var insertCmd = new SqlCommand(insertQuery, conn, transaction))
                        {
                            insertCmd.Parameters.AddWithValue("@IDHoaDon", currentIdHoaDon);
                            insertCmd.Parameters.AddWithValue("@IDMonAn", (int)row["IDMonAn"]);
                            insertCmd.Parameters.AddWithValue("@SoLuong", (int)row["SoLuong"]);
                            insertCmd.Parameters.AddWithValue("@DonGia", (decimal)row["DonGia"]);
                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    using (var updateCmd = new SqlCommand(
                        "UPDATE HoaDon SET TongTien=@TongTien, GhiChu=@GhiChu WHERE IDHoaDon=@ID", conn, transaction))
                    {
                        updateCmd.Parameters.AddWithValue("@TongTien", decimal.Parse(txtTongTien.Text));
                        updateCmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());
                        updateCmd.Parameters.AddWithValue("@ID", currentIdHoaDon);
                        updateCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Cập nhật hóa đơn thành công!");
                    LoadHoaDonDaTao();
                }
                catch (Exception ex)
                {
                    try { transaction.Rollback(); } catch { }
                    MessageBox.Show("Lỗi khi cập nhật hóa đơn: " + ex.Message);
                }
            }
        }

        // KIỂM TRA thời lượng vãng lai có đụng lịch đặt trước không
        // Overlap nếu [Start, End] giao với [ResStart, ResEnd] (chạm mép cũng KHÔNG CHO)
        private bool IsWalkInDurationOkay(int idBanAn, DateTime start, int minutes)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(@"
DECLARE @StartDT datetime = @Start;
DECLARE @EndDT   datetime = DATEADD(MINUTE, @Minutes, @StartDT);

SELECT COUNT(*)
FROM DatBan d
WHERE d.IDBanAn = @IDBanAn
  AND (d.TrangThai IS NULL OR d.TrangThai IN (N'Đã đặt', N'Giữ chỗ'))
  AND @EndDT   >= (CONVERT(datetime, d.NgayDat) + CAST(d.GioDat AS datetime))
  AND @StartDT <= DATEADD(MINUTE, ISNULL(d.ThoiLuongPhut, 60), (CONVERT(datetime, d.NgayDat) + CAST(d.GioDat AS datetime)))
", conn);

                cmd.Parameters.AddWithValue("@IDBanAn", idBanAn);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Minutes", minutes);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count == 0; // không trùng -> OK
            }
        }

        private bool isCustomerAdded = false;

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if ((cmbSoBan.SelectedIndex == -1 && cmbBanDatTruoc.SelectedIndex == -1) && !chkMangVe.Checked)
            {
                MessageBox.Show("Vui lòng chọn bàn hoặc tích 'Mang về' trước khi lưu hóa đơn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                DialogResult dr = MessageBox.Show("Khách hàng chưa tồn tại. Thêm mới?", "Thông báo", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    using (FormKhachHang formThemKH = new FormKhachHang(tenKhachHang))
                    {
                        if (formThemKH.ShowDialog() == DialogResult.OK)
                        {
                            idKhachHang = formThemKH.NewCustomerId;
                            txtKhachHang.Text = tenKhachHang;
                            isCustomerAdded = true;
                        }
                        else return;
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập KH tồn tại hoặc thêm mới.");
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                try
                {
                    string maHoaDon = txtMaHoaDon.Text;

                    // LẤY ID BÀN + IDĐẶT BÀN (nếu chọn từ combobox đặt trước)
                    int? idBan = null;
                    int? idDatBan = null;
                    if (!chkMangVe.Checked)
                    {
                        if (cmbBanDatTruoc.SelectedIndex >= 0 && cmbBanDatTruoc.SelectedItem is DataRowView drv)
                        {
                            idBan = Convert.ToInt32(drv["IDBanAn"]);
                            idDatBan = Convert.ToInt32(drv["IDDatBan"]); // để cập nhật 'Đã nhận bàn'
                        }
                        else if (cmbSoBan.SelectedValue != null)
                        {
                            idBan = (int)cmbSoBan.SelectedValue;
                        }
                    }

                    // *** KIỂM TRA THỜI GIAN DỰ KIẾN khi chọn Số bàn trống (không mang về, không theo lịch đặt trước)
                    if (!chkMangVe.Checked && idBan.HasValue && !idDatBan.HasValue)
                    {
                        int minutes = (nudThoiGianDuKien != null) ? (int)nudThoiGianDuKien.Value : 60;
                        DateTime start = dtpNgayLap.Value; // giờ vào
                        if (!IsWalkInDurationOkay(idBan.Value, start, minutes))
                        {
                            MessageBox.Show(
                                "Thời lượng dự kiến bạn chọn sẽ trùng với một lịch đặt bàn sắp tới của bàn này.\n" +
                                "Vui lòng rút ngắn thời lượng hoặc chọn bàn khác.",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // INSERT HÓA ĐƠN
                    string insertHD = @"
INSERT INTO HoaDon (IDNhanVien, IDKhachHang, NgayLap, TongTien, TrangThai, MaHoaDon, SoBan, GhiChu)
VALUES (@IDNhanVien, @IDKhachHang, @NgayLap, @TongTien, @TrangThai, @MaHoaDon, @SoBan, @GhiChu);
SELECT SCOPE_IDENTITY();";

                    int newIDHoaDon;
                    using (var cmdHD = new SqlCommand(insertHD, conn, transaction))
                    {
                        cmdHD.Parameters.AddWithValue("@IDNhanVien", cmbNhanVien.SelectedValue);
                        cmdHD.Parameters.AddWithValue("@IDKhachHang", idKhachHang.Value);
                        cmdHD.Parameters.AddWithValue("@NgayLap", dtpNgayLap.Value);
                        cmdHD.Parameters.AddWithValue("@TongTien", decimal.Parse(txtTongTien.Text));
                        cmdHD.Parameters.AddWithValue("@TrangThai", "Chưa thanh toán");
                        cmdHD.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                        cmdHD.Parameters.AddWithValue("@SoBan", (object)idBan ?? DBNull.Value);
                        cmdHD.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());
                        newIDHoaDon = Convert.ToInt32(cmdHD.ExecuteScalar());
                    }

                    // INSERT CHI TIẾT
                    foreach (DataRow row in dtChiTietHD.Rows)
                    {
                        string insertCTHD = @"
INSERT INTO ChiTietHoaDon (IDHoaDon, IDMonAn, SoLuong, DonGia)
VALUES (@IDHoaDon, @IDMonAn, @SoLuong, @DonGia)";
                        using (var cmdCTHD = new SqlCommand(insertCTHD, conn, transaction))
                        {
                            cmdCTHD.Parameters.AddWithValue("@IDHoaDon", newIDHoaDon);
                            cmdCTHD.Parameters.AddWithValue("@IDMonAn", (int)row["IDMonAn"]);
                            cmdCTHD.Parameters.AddWithValue("@SoLuong", (int)row["SoLuong"]);
                            cmdCTHD.Parameters.AddWithValue("@DonGia", (decimal)row["DonGia"]);
                            cmdCTHD.ExecuteNonQuery();
                        }
                    }

                    // CẬP NHẬT TRẠNG THÁI BÀN
                    if (!chkMangVe.Checked && idBan.HasValue)
                    {
                        using (var cmdUpdateBan =
                               new SqlCommand("UPDATE BanAn SET TrangThai = N'Có người' WHERE IDBanAn = @ID", conn, transaction))
                        {
                            cmdUpdateBan.Parameters.AddWithValue("@ID", idBan.Value);
                            cmdUpdateBan.ExecuteNonQuery();
                        }
                    }

                    // Nếu lấy từ đặt trước -> đổi DatBan thành 'Đã nhận bàn'
                    if (idDatBan.HasValue)
                    {
                        using (var cmdUpdateDat =
                               new SqlCommand("UPDATE DatBan SET TrangThai = N'Đã nhận bàn' WHERE IDDatBan = @ID", conn, transaction))
                        {
                            cmdUpdateDat.Parameters.AddWithValue("@ID", idDatBan.Value);
                            cmdUpdateDat.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();

                    // Trừ nguyên liệu
                    CapNhatNguyenLieuSauKhiTaoHoaDon(newIDHoaDon);

                    // Reload nguồn combobox (slot đã nhận bàn sẽ biến mất vì không còn trạng thái 'Đã đặt')
                    LoadHoaDonDaTao();
                    LoadSoBan();
                    LoadBanDatTruoc(null);

                    MessageBox.Show("Lưu hóa đơn thành công!");

                    // Reset form sau lưu
                    ResetForm();
                }
                catch (Exception ex)
                {
                    try { transaction.Rollback(); } catch { }
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
            ApplySelectionMode();
        }
    }
}
