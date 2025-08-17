using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

// Alias tránh nhầm Timer
using WinFormsTimer = System.Windows.Forms.Timer;

namespace QuanLyNhaHang
{
    public partial class FormDatBan : Form
    {
        private readonly string connectionString =
            @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True";

        private WinFormsTimer timer; // kiểm tra định kỳ

        // ====== cờ chặn vòng lặp sự kiện khi snap ======
        private bool _snappingNud = false;
        private bool _snappingTime = false;

        public FormDatBan()
        {
            InitializeComponent();

            this.Load += FormDatBan_Load;
            dtgvDatBan.CellClick += DtgvDatBan_CellClick;

            // Hiển thị 24h; UI không hiển thị giây
            dtpGioDat.Format = DateTimePickerFormat.Custom;
            dtpGioDat.CustomFormat = "HH:mm";
            dtpGioDat.ShowUpDown = true;

            // ====== cấu hình bước 15 phút cho thời lượng ======
            // (nếu đã set trong Designer thì giữ nguyên; đoạn này đảm bảo có giá trị fallback)
            nudThoiLuong.Minimum = 15;
            nudThoiLuong.Maximum = 600;    // 10 tiếng, tùy chỉnh
            nudThoiLuong.Increment = 15;
            nudThoiLuong.ValueChanged += NudThoiLuong_ValueChanged;

            // Snap giờ đặt về bội số 15
            dtpGioDat.ValueChanged += DtpGioDat_ValueChanged;

            // Timer 60s
            timer = new WinFormsTimer();
            timer.Interval = 60000;
            timer.Tick += Timer_Tick;
        }

        private void FormDatBan_Load(object sender, EventArgs e)
        {
            CheckAndUpdateBanStatus();
            LoadTatCaBan();      // ⬅️ không lọc trạng thái
            LoadDatBan();
            timer.Start();

            // Giá trị mặc định cho thời lượng
            if (nudThoiLuong.Minimum <= 60 && nudThoiLuong.Maximum >= 60)
                nudThoiLuong.Value = 60;

            // Snap ngay lần đầu hiển thị
            SnapTimePickerTo15();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckAndUpdateBanStatus();
            LoadTatCaBan();
            LoadDatBan();
        }

        /// <summary>
        /// Snap NumericUpDown về bội số 15 (0, 15, 30, 45, 60, ...)
        /// </summary>
        private void NudThoiLuong_ValueChanged(object sender, EventArgs e)
        {
            if (_snappingNud) return;
            _snappingNud = true;

            int step = 15;
            int v = (int)nudThoiLuong.Value;
            int snapped = (int)Math.Round(v / (double)step) * step;
            if (snapped < nudThoiLuong.Minimum) snapped = (int)nudThoiLuong.Minimum;
            if (snapped > nudThoiLuong.Maximum) snapped = (int)nudThoiLuong.Maximum;

            if (snapped != v) nudThoiLuong.Value = snapped;

            _snappingNud = false;
        }

        /// <summary>
        /// Snap DateTimePicker (giờ đặt) về bội số 15 phút
        /// </summary>
        private void DtpGioDat_ValueChanged(object sender, EventArgs e)
        {
            SnapTimePickerTo15();
        }

        private void SnapTimePickerTo15()
        {
            if (_snappingTime) return;
            _snappingTime = true;

            DateTime v = dtpGioDat.Value;
            int step = 15;
            int snappedMinute = (int)Math.Round(v.Minute / (double)step) * step;
            if (snappedMinute == 60)
            {
                v = v.AddHours(1);
                snappedMinute = 0;
            }
            DateTime snapped = new DateTime(v.Year, v.Month, v.Day, v.Hour, snappedMinute, 0);

            if (snapped != dtpGioDat.Value)
                dtpGioDat.Value = snapped;

            _snappingTime = false;
        }

        /// <summary>
        /// Đến đúng giờ đặt (tính theo phút) và bàn đang 'Trống' => set 'Đã đặt'
        /// </summary>
        private void CheckAndUpdateBanStatus()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string selectDue = @"
SELECT d.IDBanAn
FROM DatBan d
JOIN BanAn b ON b.IDBanAn = d.IDBanAn
WHERE d.TrangThai = N'Đã đặt'
  AND DATEDIFF(MINUTE, (CONVERT(datetime, d.NgayDat) + CAST(d.GioDat AS datetime)), GETDATE()) >= 0
  AND b.TrangThai = N'Trống'
GROUP BY d.IDBanAn";

                var idsToUpdate = new List<int>();
                using (var cmd = new SqlCommand(selectDue, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        idsToUpdate.Add(rd.GetInt32(0));
                }

                foreach (var idBan in idsToUpdate)
                {
                    string updateBan = @"UPDATE BanAn SET TrangThai = N'Đã đặt' WHERE IDBanAn = @IDBanAn AND TrangThai = N'Trống'";
                    using (var cmdUp = new SqlCommand(updateBan, conn))
                    {
                        cmdUp.Parameters.AddWithValue("@IDBanAn", idBan);
                        cmdUp.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// HIỂN THỊ TẤT CẢ BÀN (không lọc). DisplayMember = 'SoBan - TrangThai'
        /// </summary>
        private void LoadTatCaBan()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
SELECT 
    IDBanAn, 
    SoBan, 
    TrangThai,
    CAST(SoBan AS nvarchar(10)) + N' - ' + ISNULL(TrangThai, N'') AS TenHienThi
FROM BanAn
ORDER BY SoBan";
                var da = new SqlDataAdapter(query, conn);
                var dt = new DataTable();
                da.Fill(dt);

                cmbBanAn.DataSource = dt;
                cmbBanAn.DisplayMember = "TenHienThi";
                cmbBanAn.ValueMember = "IDBanAn";
                cmbBanAn.SelectedIndex = -1;
            }
        }

        private void LoadDatBan()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var query = @"
SELECT 
    d.IDDatBan,
    b.SoBan,
    k.TenKH,
    d.NgayDat,
    d.GioDat,
    d.ThoiLuongPhut,
    DATEADD(MINUTE, ISNULL(d.ThoiLuongPhut,60), CAST(d.NgayDat AS datetime)+CAST(d.GioDat AS datetime)) AS GioKetThuc,
    d.TrangThai AS TrangThaiDat,   -- trạng thái của bản ghi đặt bàn (Đã đặt / Giữ chỗ / Đã nhận bàn)
    b.TrangThai AS TrangThaiBan    -- trạng thái hiện tại của bàn (Trống / Đã đặt / Có người)
FROM DatBan d
JOIN BanAn b ON d.IDBanAn = b.IDBanAn
JOIN KhachHang k ON d.IDKhachHang = k.IDKhachHang
ORDER BY d.NgayDat DESC, d.GioDat DESC";

                var da = new SqlDataAdapter(query, conn);
                var dt = new DataTable();
                da.Fill(dt);

                dtgvDatBan.DataSource = dt;

                // Ẩn ID
                if (dtgvDatBan.Columns.Contains("IDDatBan"))
                    dtgvDatBan.Columns["IDDatBan"].Visible = false;

                // Đặt tiêu đề cột
                if (dtgvDatBan.Columns.Contains("SoBan"))
                    dtgvDatBan.Columns["SoBan"].HeaderText = "Số bàn";
                if (dtgvDatBan.Columns.Contains("TenKH"))
                    dtgvDatBan.Columns["TenKH"].HeaderText = "Tên khách hàng";
                if (dtgvDatBan.Columns.Contains("NgayDat"))
                    dtgvDatBan.Columns["NgayDat"].HeaderText = "Ngày đặt";
                if (dtgvDatBan.Columns.Contains("GioDat"))
                    dtgvDatBan.Columns["GioDat"].HeaderText = "Giờ bắt đầu";
                if (dtgvDatBan.Columns.Contains("ThoiLuongPhut"))
                    dtgvDatBan.Columns["ThoiLuongPhut"].HeaderText = "Thời lượng (phút)";
                if (dtgvDatBan.Columns.Contains("GioKetThuc"))
                    dtgvDatBan.Columns["GioKetThuc"].HeaderText = "Giờ kết thúc (dự kiến)";
                if (dtgvDatBan.Columns.Contains("TrangThaiDat"))
                    dtgvDatBan.Columns["TrangThaiDat"].HeaderText = "Trạng thái đặt";
                if (dtgvDatBan.Columns.Contains("TrangThaiBan"))
                    dtgvDatBan.Columns["TrangThaiBan"].HeaderText = "Trạng thái bàn";
            }
        }

        // ---------- TIỆN ÍCH & KIỂM TRA CHỒNG GIỜ ----------

        private int GetSoBanById(int idBanAn)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT SoBan FROM BanAn WHERE IDBanAn=@id", conn);
            cmd.Parameters.AddWithValue("@id", idBanAn);
            var rs = cmd.ExecuteScalar();
            return rs != null ? Convert.ToInt32(rs) : -1;
        }

        /// <summary>
        /// Kiểm tra chồng giờ theo khoảng: (StartA < EndB) && (StartB < EndA)
        /// So với cả các bản ghi Đặt bàn khác (Đã đặt/Giữ chỗ) và các Hóa đơn đang mở (Chưa thanh toán/Nợ).
        /// </summary>
        private bool HasOverlap(int idBanAn, DateTime ngayDat, TimeSpan gioDat, int thoiLuongPhut, int? ignoreIdDatBan = null)
        {
            DateTime start = ngayDat.Date.Add(gioDat);
            DateTime end = start.AddMinutes(thoiLuongPhut);

            using var conn = new SqlConnection(connectionString);
            conn.Open();

            // 1) Đụng các slot ĐẶT BÀN khác
            string sqlDatBan = @"
SELECT COUNT(*)
FROM DatBan d
WHERE d.IDBanAn = @IDBanAn
  AND (@IgnoreId IS NULL OR d.IDDatBan <> @IgnoreId)
  AND d.TrangThai IN (N'Đã đặt', N'Giữ chỗ')
  AND (
        @Start < DATEADD(MINUTE, ISNULL(d.ThoiLuongPhut,60), CAST(d.NgayDat AS datetime) + CAST(d.GioDat AS datetime))
    AND (CAST(d.NgayDat AS datetime) + CAST(d.GioDat AS datetime)) < @End
  )";

            using var cmd1 = new SqlCommand(sqlDatBan, conn);
            cmd1.Parameters.AddWithValue("@IDBanAn", idBanAn);
            cmd1.Parameters.AddWithValue("@IgnoreId", (object?)ignoreIdDatBan ?? DBNull.Value);
            cmd1.Parameters.AddWithValue("@Start", start);
            cmd1.Parameters.AddWithValue("@End", end);
            int c1 = Convert.ToInt32(cmd1.ExecuteScalar());

            // 2) Đụng các HÓA ĐƠN đang mở/nợ (đang chiếm bàn)
            int soBan = GetSoBanById(idBanAn);
            string sqlHoaDon = @"
SELECT COUNT(*)
FROM HoaDon h
WHERE h.SoBan = @SoBan
  AND h.TrangThai IN (N'Chưa thanh toán', N'Nợ')
  AND (
        @Start < DATEADD(MINUTE, ISNULL(h.ThoiLuongPhut,60), h.NgayLap)
    AND h.NgayLap < @End
  )";

            using var cmd2 = new SqlCommand(sqlHoaDon, conn);
            cmd2.Parameters.AddWithValue("@SoBan", soBan);
            cmd2.Parameters.AddWithValue("@Start", start);
            cmd2.Parameters.AddWithValue("@End", end);
            int c2 = Convert.ToInt32(cmd2.ExecuteScalar());

            return (c1 + c2) > 0;
        }

        // ---------- KHÁCH HÀNG ----------

        private int? GetKhachHangIdByName(string tenKhachHang)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var query = "SELECT IDKhachHang FROM KhachHang WHERE TenKH = @TenKH";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenKH", tenKhachHang);
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
        }

        // ---------- FORM/BTN HANDLERS ----------

        private void ClearForm()
        {
            txtTenKhachHang.Text = "";
            cmbBanAn.SelectedIndex = -1;
            dtpNgayDat.Value = DateTime.Now;
            dtpGioDat.Value = DateTime.Now;
            SnapTimePickerTo15();

            if (nudThoiLuong.Minimum <= 60 && nudThoiLuong.Maximum >= 60)
                nudThoiLuong.Value = 60;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var tenKhachHang = txtTenKhachHang.Text.Trim();
            if (string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbBanAn.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idBanAn = (int)cmbBanAn.SelectedValue;
            DateTime ngayDat = dtpNgayDat.Value.Date;
            var gioDat = new TimeSpan(dtpGioDat.Value.Hour, dtpGioDat.Value.Minute, 0);
            int thoiLuong = (int)nudThoiLuong.Value;

            if (thoiLuong <= 0)
            {
                MessageBox.Show("Thời lượng sử dụng phải lớn hơn 0 phút.");
                return;
            }

            DateTime thoiGianDat = ngayDat.Add(gioDat);
            if (thoiGianDat < DateTime.Now)
            {
                MessageBox.Show("Không thể đặt bàn ở thời điểm trong quá khứ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra chồng giờ theo khoảng
            if (HasOverlap(idBanAn, ngayDat, gioDat, thoiLuong))
            {
                MessageBox.Show("Khoảng thời gian này đã có người đặt hoặc bàn đang được sử dụng vui lòng chọn khoảng thời gian khác hoặc bàn khác.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? idKhachHang = GetKhachHangIdByName(tenKhachHang);
            if (idKhachHang == null)
            {
                var dr = MessageBox.Show("Khách hàng chưa tồn tại. Bạn có muốn thêm khách hàng mới không?",
                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (FormKhachHang formThemKH = new FormKhachHang(tenKhachHang))
                    {
                        if (formThemKH.ShowDialog() == DialogResult.OK)
                            idKhachHang = formThemKH.NewCustomerId;
                        else
                            return;
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng tồn tại hoặc thêm mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var insert = @"
INSERT INTO DatBan (IDBanAn, IDKhachHang, NgayDat, GioDat, ThoiLuongPhut, TrangThai)
VALUES (@IDBanAn, @IDKhachHang, @NgayDat, @GioDat, @ThoiLuongPhut, N'Đã đặt')";
                using (var cmd = new SqlCommand(insert, conn))
                {
                    cmd.Parameters.AddWithValue("@IDBanAn", idBanAn);
                    cmd.Parameters.AddWithValue("@IDKhachHang", idKhachHang.Value);
                    cmd.Parameters.AddWithValue("@NgayDat", ngayDat);
                    cmd.Parameters.AddWithValue("@GioDat", gioDat);
                    cmd.Parameters.AddWithValue("@ThoiLuongPhut", thoiLuong);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Đặt bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CheckAndUpdateBanStatus();
            LoadTatCaBan();
            LoadDatBan();
            ClearForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dtgvDatBan.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đặt bàn để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idDatBan = Convert.ToInt32(dtgvDatBan.CurrentRow.Cells["IDDatBan"].Value);
            string tenKhachHang = txtTenKhachHang.Text.Trim();
            if (string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbBanAn.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int newIdBanAn = (int)cmbBanAn.SelectedValue;
            DateTime ngayDat = dtpNgayDat.Value.Date;
            var gioDat = new TimeSpan(dtpGioDat.Value.Hour, dtpGioDat.Value.Minute, 0);
            int thoiLuong = (int)nudThoiLuong.Value;

            if (thoiLuong <= 0)
            {
                MessageBox.Show("Thời lượng sử dụng phải lớn hơn 0 phút.");
                return;
            }

            DateTime thoiGianDat = ngayDat.Add(gioDat);
            if (thoiGianDat < DateTime.Now)
            {
                MessageBox.Show("Thời gian đặt không hợp lệ. Vui lòng chọn hiện tại hoặc tương lai.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? idKhachHang = GetKhachHangIdByName(tenKhachHang);
            if (idKhachHang == null)
            {
                MessageBox.Show("Khách hàng không tồn tại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chặn chồng giờ (bỏ qua chính record đang sửa)
            if (HasOverlap(newIdBanAn, ngayDat, gioDat, thoiLuong, idDatBan))
            {
                MessageBox.Show("Khoảng thời gian này đã bị chiếm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var update = @"
UPDATE DatBan
SET IDBanAn = @IDBanAn,
    IDKhachHang = @IDKhachHang,
    NgayDat = @NgayDat,
    GioDat = @GioDat,
    ThoiLuongPhut = @ThoiLuongPhut
WHERE IDDatBan = @IDDatBan";
                using (var cmd = new SqlCommand(update, conn))
                {
                    cmd.Parameters.AddWithValue("@IDDatBan", idDatBan);
                    cmd.Parameters.AddWithValue("@IDBanAn", newIdBanAn);
                    cmd.Parameters.AddWithValue("@IDKhachHang", idKhachHang.Value);
                    cmd.Parameters.AddWithValue("@NgayDat", ngayDat);
                    cmd.Parameters.AddWithValue("@GioDat", gioDat);
                    cmd.Parameters.AddWithValue("@ThoiLuongPhut", thoiLuong);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Sửa đặt bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CheckAndUpdateBanStatus();
            LoadTatCaBan();
            LoadDatBan();
            ClearForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dtgvDatBan.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đặt bàn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idDatBan = Convert.ToInt32(dtgvDatBan.CurrentRow.Cells["IDDatBan"].Value);

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var delete = "DELETE FROM DatBan WHERE IDDatBan = @IDDatBan";
                using (var cmd = new SqlCommand(delete, conn))
                {
                    cmd.Parameters.AddWithValue("@IDDatBan", idDatBan);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Xóa đặt bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CheckAndUpdateBanStatus();
            LoadTatCaBan();
            LoadDatBan();
            ClearForm();
        }

        private void DtgvDatBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dtgvDatBan.Rows[e.RowIndex];

            txtTenKhachHang.Text = row.Cells["TenKH"]?.Value?.ToString() ?? string.Empty;
            // Chỉ set text hiển thị; Value sẽ chọn lại khi người dùng chọn từ combobox
            cmbBanAn.Text = row.Cells["SoBan"]?.Value?.ToString() ?? string.Empty;

            if (row.Cells["NgayDat"]?.Value != DBNull.Value && row.Cells["NgayDat"]?.Value != null)
                dtpNgayDat.Value = Convert.ToDateTime(row.Cells["NgayDat"].Value);

            if (row.Cells["GioDat"]?.Value != DBNull.Value && row.Cells["GioDat"]?.Value != null)
            {
                if (row.Cells["GioDat"].Value is TimeSpan ts)
                {
                    var tsMinute = new TimeSpan(ts.Hours, ts.Minutes, 0);
                    dtpGioDat.Value = dtpNgayDat.Value.Date.Add(tsMinute);
                }
                else
                {
                    var dt = Convert.ToDateTime(row.Cells["GioDat"].Value);
                    var tsMinute = new TimeSpan(dt.Hour, dt.Minute, 0);
                    dtpGioDat.Value = dtpNgayDat.Value.Date.Add(tsMinute);
                }
            }

            if (row.Cells["ThoiLuongPhut"]?.Value != DBNull.Value && row.Cells["ThoiLuongPhut"]?.Value != null)
            {
                var v = Convert.ToDecimal(row.Cells["ThoiLuongPhut"].Value);
                v = Math.Min(Math.Max(v, nudThoiLuong.Minimum), nudThoiLuong.Maximum);
                nudThoiLuong.Value = v;
            }

            // Snap lại cho đẹp
            SnapTimePickerTo15();
            NudThoiLuong_ValueChanged(null, EventArgs.Empty);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            CheckAndUpdateBanStatus();
            LoadTatCaBan();
            LoadDatBan();
            ClearForm();
        }
    }
}
