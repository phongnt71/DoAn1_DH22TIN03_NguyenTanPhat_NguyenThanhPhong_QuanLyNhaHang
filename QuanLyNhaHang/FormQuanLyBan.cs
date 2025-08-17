using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class FormQuanLyBan : Form
    {
        private readonly string connectionString =
            @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True";

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
                string query = "SELECT IDBanAn, SoBan, TrangThai FROM BanAn ORDER BY SoBan";
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

                    string trangThai = row["TrangThai"]?.ToString() ?? string.Empty;
                    if (trangThai == "Trống")
                        btnBan.BackColor = Color.Aqua;
                    else if (trangThai == "Có người")
                        btnBan.BackColor = Color.LightCoral;
                    else if (trangThai == "Đã đặt")
                        btnBan.BackColor = Color.Gold;
                    else
                        btnBan.BackColor = Color.Gray;

                    btnBan.Click += BtnBan_Click;          // click: mở form
                    btnBan.MouseEnter += BtnBan_MouseEnter; // hover: điền thông tin

                    flpBanAn.Controls.Add(btnBan);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách bàn: " + ex.Message);
            }
        }

        // Hover: tự điền Mã số bàn + Trạng thái
        private void BtnBan_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not DataRow row) return;

            string soBan = row["SoBan"]?.ToString() ?? string.Empty;
            string trangThai = row["TrangThai"]?.ToString() ?? string.Empty;

            txtMaSoBan.Text = soBan;

            int idx = cmbTrangThai.FindStringExact(trangThai);
            cmbTrangThai.SelectedIndex = idx >= 0 ? idx : -1;
        }

        private void BtnBan_Click(object? sender, EventArgs e)
        {
            if (sender is not Button clickedButton || clickedButton.Tag is not DataRow row) return;

            int idBanAn = Convert.ToInt32(row["IDBanAn"]);
            string soBan = row["SoBan"]?.ToString() ?? "";
            string trangThai = row["TrangThai"]?.ToString() ?? "";

            if (trangThai == "Có người")
            {
                int idHoaDon = LayHoaDonChuaThanhToanTheoSoBan(soBan);

                if (idHoaDon <= 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn đang mở cho bàn này.\nHệ thống đã chuyển bàn về trạng thái 'Trống'.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CapNhatTrangThaiBanTheoSoBan(soBan, "Trống");
                    clickedButton.Text = $"Bàn {soBan}\nTrống";
                    clickedButton.BackColor = Color.Aqua;
                    row["TrangThai"] = "Trống";
                    clickedButton.Tag = row;
                    return;
                }

                FormDSHoaDon frm = new FormDSHoaDon(this)
                {
                    MdiParent = this.MdiParent
                };
                frm.Show();
                frm.BringToFront();
                frm.LoadChiTietTheoIDHoaDon(idHoaDon);
                return;
            }

            if (trangThai == "Trống")
            {
                FormTaoHoaDon f = new FormTaoHoaDon
                {
                    MdiParent = this.MdiParent
                };
                f.Show();
                f.BringToFront();
                f.PresetSoBanTrong(idBanAn);
                return;
            }

            if (trangThai == "Đã đặt")
            {
                var datTruoc = TimDatBanGanNhatChoBan(idBanAn);
                if (datTruoc == null)
                {
                    // Không còn đặt bàn hiệu lực ➜ tự trả bàn về Trống
                    CapNhatTrangThaiBanTheoIDBanAn(idBanAn, "Trống");

                    clickedButton.Text = $"Bàn {soBan}\nTrống";
                    clickedButton.BackColor = Color.Aqua;
                    row["TrangThai"] = "Trống";
                    clickedButton.Tag = row;

                    MessageBox.Show(
                        "Không tìm thấy bản ghi đặt bàn đang hiệu lực cho bàn này.\nHệ thống đã chuyển bàn về trạng thái 'Trống'.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                FormTaoHoaDon f = new FormTaoHoaDon
                {
                    MdiParent = this.MdiParent
                };
                f.Show();
                f.BringToFront();
                f.PresetBanDatTruoc(datTruoc.Value.idBanAn, datTruoc.Value.idDatBan);
            }
        }

        private (int idBanAn, int idDatBan)? TimDatBanGanNhatChoBan(int idBanAn)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string sqlUpcoming = @"
SELECT TOP 1 d.IDDatBan, d.IDBanAn
FROM DatBan d
WHERE d.IDBanAn = @IDBanAn AND d.TrangThai = N'Đã đặt'
  AND (CAST(d.NgayDat AS datetime) + CAST(d.GioDat AS datetime)) >= GETDATE()
ORDER BY (CAST(d.NgayDat AS datetime) + CAST(d.GioDat AS datetime)) ASC";

            using (var cmd = new SqlCommand(sqlUpcoming, conn))
            {
                cmd.Parameters.AddWithValue("@IDBanAn", idBanAn);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    int idDat = rd.GetInt32(0);
                    int idB = rd.GetInt32(1);
                    rd.Close();
                    return (idB, idDat);
                }
                rd.Close();
            }

            string sqlLatest = @"
SELECT TOP 1 d.IDDatBan, d.IDBanAn
FROM DatBan d
WHERE d.IDBanAn = @IDBanAn AND d.TrangThai = N'Đã đặt'
ORDER BY (CAST(d.NgayDat AS datetime) + CAST(d.GioDat AS datetime)) DESC";

            using (var cmd2 = new SqlCommand(sqlLatest, conn))
            {
                cmd2.Parameters.AddWithValue("@IDBanAn", idBanAn);
                var rd2 = cmd2.ExecuteReader();
                if (rd2.Read())
                {
                    int idDat = rd2.GetInt32(0);
                    int idB = rd2.GetInt32(1);
                    rd2.Close();
                    return (idB, idDat);
                }
                rd2.Close();
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

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa bàn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                using SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // Khóa hàng cần xóa và đọc trạng thái ngay trong giao dịch
                    string sqlSelect = @"
SELECT IDBanAn, ISNULL(TrangThai, N'') AS TrangThai
FROM BanAn WITH (UPDLOCK, HOLDLOCK)
WHERE SoBan = @SoBan";
                    using SqlCommand cmdSel = new SqlCommand(sqlSelect, conn, tran);
                    cmdSel.Parameters.AddWithValue("@SoBan", txtMaSoBan.Text.Trim());

                    int idBanAn;
                    string trangThai;
                    using (var rd = cmdSel.ExecuteReader())
                    {
                        if (!rd.Read())
                        {
                            MessageBox.Show("Không tìm thấy bàn để xóa.");
                            rd.Close();
                            tran.Rollback();
                            return;
                        }
                        idBanAn = rd.GetInt32(0);
                        trangThai = rd.GetString(1);
                    }

                    // RÀNG BUỘC: chỉ cho xóa khi Trạng thái = Trống
                    if (!string.Equals(trangThai, "Trống", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Không thể xóa bàn đang ở trạng thái 'Có người' hoặc 'Đã đặt'.\n" +
                                        "Vui lòng hoàn tất hóa đơn/huỷ đặt bàn và chuyển bàn về 'Trống' trước.",
                                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tran.Rollback();
                        return;
                    }

                    // (Giữ nguyên logic dọn dữ liệu lịch sử đặt bàn nếu có)
                    string deleteDatBanQuery = "DELETE FROM DatBan WHERE IDBanAn = @ID";
                    using (SqlCommand deleteDatBanCmd = new SqlCommand(deleteDatBanQuery, conn, tran))
                    {
                        deleteDatBanCmd.Parameters.AddWithValue("@ID", idBanAn);
                        deleteDatBanCmd.ExecuteNonQuery();
                    }

                    string deleteBanAnQuery = "DELETE FROM BanAn WHERE IDBanAn = @ID";
                    using (SqlCommand deleteBanAnCmd = new SqlCommand(deleteBanAnQuery, conn, tran))
                    {
                        deleteBanAnCmd.Parameters.AddWithValue("@ID", idBanAn);
                        int rows = deleteBanAnCmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            tran.Commit();
                            MessageBox.Show("Xóa bàn thành công.");
                            LoadDanhSachBan();
                        }
                        else
                        {
                            tran.Rollback();
                            MessageBox.Show("Không tìm thấy bàn để xóa.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    try { tran.Rollback(); } catch { }
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
}
