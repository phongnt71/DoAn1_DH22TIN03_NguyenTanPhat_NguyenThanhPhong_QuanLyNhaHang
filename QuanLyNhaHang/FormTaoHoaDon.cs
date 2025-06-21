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
        private bool isAdding = false;
        private bool isEditing = false;

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
        private void LoadSoBan(int? selectedID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDBanAn, SoBan FROM BanAn WHERE TrangThai = N'Trống'";
                if (selectedID.HasValue)
                {
                    query += " OR IDBanAn = @IDBanAn";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (selectedID.HasValue)
                    cmd.Parameters.AddWithValue("@IDBanAn", selectedID.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbSoBan.DataSource = dt;
                cmbSoBan.DisplayMember = "SoBan";
                cmbSoBan.ValueMember = "IDBanAn";
                cmbSoBan.SelectedIndex = -1;
                cmbSoBan.Text = "";
            }
        }

        private void LoadBanDatTruoc(int? selectedID = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDBanAn, SoBan FROM BanAn WHERE TrangThai = N'Đã đặt'";
                if (selectedID.HasValue)
                {
                    query += " OR IDBanAn = @IDBanAn";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (selectedID.HasValue)
                    cmd.Parameters.AddWithValue("@IDBanAn", selectedID.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbBanDatTruoc.DataSource = dt;
                cmbBanDatTruoc.DisplayMember = "SoBan";
                cmbBanDatTruoc.ValueMember = "IDBanAn";
                cmbBanDatTruoc.SelectedIndex = -1;
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
            LoadHoaDonDaTao();
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

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtgvHoaDonDaTao.DataSource = dt;
                dtgvHoaDonDaTao.CellClick -= dtgvHoaDonDaTao_CellClick; // tránh double event
                dtgvHoaDonDaTao.CellClick += dtgvHoaDonDaTao_CellClick;
            }
        }
        private void dtgvHoaDonDaTao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy mã hóa đơn được chọn
                string maHoaDon = dtgvHoaDonDaTao.Rows[e.RowIndex].Cells["MaHoaDon"].Value.ToString();

                // Load chi tiết hóa đơn lên form
                LoadHoaDonLenForm(maHoaDon);

                // ==> ĐẶT TRẠNG THÁI CHỈNH SỬA
                isEditing = true;

                // ==> CẬP NHẬT TRẠNG THÁI BUTTON
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
                SqlCommand cmd = new SqlCommand(queryHD, conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                SqlDataReader reader = cmd.ExecuteReader();
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
                        LoadBanDatTruoc();
                        cmbSoBan.SelectedIndex = -1;
                        chkMangVe.Checked = true;
                    }

                    int idKH = (int)reader["IDKhachHang"];
                    reader.Close();

                    SqlCommand cmdKH = new SqlCommand("SELECT TenKH FROM KhachHang WHERE IDKhachHang = @id", conn);
                    cmdKH.Parameters.AddWithValue("@id", idKH);
                    object tenKH = cmdKH.ExecuteScalar();
                    txtKhachHang.Text = tenKH != null ? tenKH.ToString() : "";

                    string queryCT = @"SELECT c.IDMonAn, m.TenMon, c.SoLuong, c.DonGia
                                FROM ChiTietHoaDon c
                                JOIN MonAn m ON c.IDMonAn = m.IDMonAn
                                WHERE c.IDHoaDon = @IDHoaDon";
                    SqlCommand cmdCT = new SqlCommand(queryCT, conn);
                    cmdCT.Parameters.AddWithValue("@IDHoaDon", currentIdHoaDon);
                    SqlDataAdapter da = new SqlDataAdapter(cmdCT);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dt.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGia");
                    dtChiTietHD = dt;
                    dtgvChiTietHD.DataSource = dtChiTietHD;
                }
            }
            TinhTongTien();
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
        }

        private void SetButtonState(bool isEditingEnabled)
        {
            btnLuu.Enabled = !isEditingEnabled;
            btnCapNhat.Enabled = isEditingEnabled;
            btnHuy.Enabled = true;
            btnThemHD.Enabled = true;
        }

        private void btnThemHD_Click(object sender, EventArgs e)
        {
            isAdding = true;
            isEditing = false;

            ResetForm();
            SetButtonState(false);
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
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    // Xóa chi tiết cũ
                    string deleteQuery = "DELETE FROM ChiTietHoaDon WHERE IDHoaDon = @IDHoaDon";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction);
                    deleteCmd.Parameters.AddWithValue("@IDHoaDon", currentIdHoaDon);
                    deleteCmd.ExecuteNonQuery();

                    // Thêm chi tiết mới
                    foreach (DataRow row in dtChiTietHD.Rows)
                    {
                        string insertQuery = @"
                    INSERT INTO ChiTietHoaDon (IDHoaDon, IDMonAn, SoLuong, DonGia) 
                    VALUES (@IDHoaDon, @IDMonAn, @SoLuong, @DonGia)";
                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction);
                        insertCmd.Parameters.AddWithValue("@IDHoaDon", currentIdHoaDon);
                        insertCmd.Parameters.AddWithValue("@IDMonAn", (int)row["IDMonAn"]);
                        insertCmd.Parameters.AddWithValue("@SoLuong", (int)row["SoLuong"]);
                        insertCmd.Parameters.AddWithValue("@DonGia", (decimal)row["DonGia"]);
                        insertCmd.ExecuteNonQuery();
                    }

                    // Cập nhật tổng tiền, ghi chú
                    string updateQuery = @"
                UPDATE HoaDon
                SET TongTien = @TongTien, GhiChu = @GhiChu
                WHERE IDHoaDon = @IDHoaDon";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@TongTien", decimal.Parse(txtTongTien.Text));
                    updateCmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());
                    updateCmd.Parameters.AddWithValue("@IDHoaDon", currentIdHoaDon);
                    updateCmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Cập nhật hóa đơn thành công!");
                    LoadHoaDonDaTao();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi khi cập nhật hóa đơn: " + ex.Message);
                }
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

            string ghiChu = txtGhiChu.Text.Trim();  // Lấy giá trị ghi chú

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string maHoaDon = txtMaHoaDon.Text;

                    // Lấy ID bàn nếu không phải mang về
                    int? idBan = null;
                    if (!chkMangVe.Checked)
                    {
                        if (cmbBanDatTruoc.SelectedValue != null)
                            idBan = (int)cmbBanDatTruoc.SelectedValue;
                        else if (cmbSoBan.SelectedValue != null)
                            idBan = (int)cmbSoBan.SelectedValue;
                    }

                    // Insert hóa đơn
                    string insertHD = @"
                INSERT INTO HoaDon (IDNhanVien, IDKhachHang, NgayLap, TongTien, TrangThai, MaHoaDon, SoBan, GhiChu)
                VALUES (@IDNhanVien, @IDKhachHang, @NgayLap, @TongTien, @TrangThai, @MaHoaDon, @SoBan, @GhiChu);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdHD = new SqlCommand(insertHD, conn, transaction);
                    cmdHD.Parameters.AddWithValue("@IDNhanVien", cmbNhanVien.SelectedValue);
                    cmdHD.Parameters.AddWithValue("@IDKhachHang", idKhachHang.Value);
                    cmdHD.Parameters.AddWithValue("@NgayLap", dtpNgayLap.Value);
                    cmdHD.Parameters.AddWithValue("@TongTien", decimal.Parse(txtTongTien.Text));
                    cmdHD.Parameters.AddWithValue("@TrangThai", "Chưa thanh toán");
                    cmdHD.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    cmdHD.Parameters.AddWithValue("@SoBan", (object)idBan ?? DBNull.Value);
                    cmdHD.Parameters.AddWithValue("@GhiChu", ghiChu);

                    int newIDHoaDon = Convert.ToInt32(cmdHD.ExecuteScalar());

                    // Insert chi tiết hóa đơn
                    foreach (DataRow row in dtChiTietHD.Rows)
                    {
                        string insertCTHD = @"
                    INSERT INTO ChiTietHoaDon (IDHoaDon, IDMonAn, SoLuong, DonGia)
                    VALUES (@IDHoaDon, @IDMonAn, @SoLuong, @DonGia)";
                        SqlCommand cmdCTHD = new SqlCommand(insertCTHD, conn, transaction);
                        cmdCTHD.Parameters.AddWithValue("@IDHoaDon", newIDHoaDon);
                        cmdCTHD.Parameters.AddWithValue("@IDMonAn", (int)row["IDMonAn"]);
                        cmdCTHD.Parameters.AddWithValue("@SoLuong", (int)row["SoLuong"]);
                        cmdCTHD.Parameters.AddWithValue("@DonGia", (decimal)row["DonGia"]);

                        cmdCTHD.ExecuteNonQuery();
                    }

                    // Cập nhật trạng thái bàn nếu không mang về
                    if (!chkMangVe.Checked && idBan.HasValue)
                    {
                        string updateBan = "UPDATE BanAn SET TrangThai = N'Có người' WHERE IDBanAn = @IDBanAn";
                        SqlCommand cmdUpdateBan = new SqlCommand(updateBan, conn, transaction);
                        cmdUpdateBan.Parameters.AddWithValue("@IDBanAn", idBan.Value);
                        cmdUpdateBan.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    // Trừ nguyên liệu
                    CapNhatNguyenLieuSauKhiTaoHoaDon(newIDHoaDon);

                    // Tự động load lại hóa đơn vừa thêm
                    LoadHoaDonDaTao();

                    MessageBox.Show("Lưu hóa đơn thành công!");

                    // Reset form
                    dtChiTietHD.Clear();
                    txtTongTien.Text = "0";
                    txtKhachHang.Text = "";
                    txtGhiChu.Text = "";
                    txtMaHoaDon.Text = GenerateInvoiceCode();
                    LoadSoBan();
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
        }
    }
}