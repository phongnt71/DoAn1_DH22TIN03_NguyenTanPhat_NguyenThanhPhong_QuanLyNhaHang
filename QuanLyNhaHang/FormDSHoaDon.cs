using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.Threading.Tasks;

namespace QuanLyNhaHang
{
    public partial class FormDSHoaDon : Form
    {
        private readonly string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;Trust Server Certificate=True";

        private FormQuanLyBan? formQuanLyBan;
        private int currentIdHoaDon = -1;

        public FormDSHoaDon(FormQuanLyBan formQLBan)
        {
            InitializeComponent();
            formQuanLyBan = formQLBan;

            this.Load += FormDSHoaDon_Load;
            dtgvDSHoaDon.CellClick += dtgvDSHoaDon_CellClick;
            picQR.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void FormDSHoaDon_Load(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
            LoadNhanVienVaoCombobox();
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

            // Load lại danh sách bàn để cập nhật UI
        }


        private void LoadDanhSachHoaDon()
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            string query = @"
                SELECT 
                    HD.IDHoaDon, 
                    HD.MaHoaDon, 
                    HD.NgayLap, 
                    HD.TongTien, 
                    HD.TrangThai,
                    BA.SoBan
                FROM HoaDon HD
                LEFT JOIN BanAn BA ON HD.SoBan = BA.IDBanAn";

            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dtgvDSHoaDon.DataSource = dt;
            dtgvDSHoaDon.ClearSelection();

            // Đổi tên cột hiển thị
            if (dtgvDSHoaDon.Columns["IDHoaDon"] != null) dtgvDSHoaDon.Columns["IDHoaDon"].HeaderText = "ID Hóa Đơn";
            if (dtgvDSHoaDon.Columns["MaHoaDon"] != null) dtgvDSHoaDon.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
            if (dtgvDSHoaDon.Columns["NgayLap"] != null) dtgvDSHoaDon.Columns["NgayLap"].HeaderText = "Ngày Lập";
            if (dtgvDSHoaDon.Columns["TongTien"] != null) dtgvDSHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền";
            if (dtgvDSHoaDon.Columns["TrangThai"] != null) dtgvDSHoaDon.Columns["TrangThai"].HeaderText = "Trạng Thái";
            if (dtgvDSHoaDon.Columns["SoBan"] != null) dtgvDSHoaDon.Columns["SoBan"].HeaderText = "Số Bàn";
        }

        private void LoadNhanVienVaoCombobox()
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT IDNhanVien, TenNV FROM NhanVien";

            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            cmbNhanVien.DataSource = dt;
            cmbNhanVien.DisplayMember = "TenNV";
            cmbNhanVien.ValueMember = "IDNhanVien";
            cmbNhanVien.SelectedIndex = -1;
        }

        private System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

        private async void dtgvDSHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvDSHoaDon.Rows[e.RowIndex];
                if (row.Cells["IDHoaDon"]?.Value != null && int.TryParse(row.Cells["IDHoaDon"].Value.ToString(), out int idHoaDon))
                {
                    currentIdHoaDon = idHoaDon;

                    picQR.Image = null; // Xóa ngay ảnh cũ để UI không bị đơ hoặc nhầm lẫn

                    var chiTiet = await LoadChiTietHoaDonAsync(idHoaDon);
                    LoadChiTietMonAn(idHoaDon);

                    if (chiTiet != null)
                    {
                        // Cập nhật UI
                        txtMaHoaDon.Text = chiTiet.MaHoaDon;
                        txtKhachHang.Text = chiTiet.TenKhachHang;
                        dtpNgayLap.Value = chiTiet.NgayLap;
                        cmbNhanVien.SelectedValue = chiTiet.IDNhanVien;
                        txtTongTien.Text = chiTiet.TongTien + " VND";

                        // Hiển thị ảnh loading tạm thời nếu có (nếu bạn có ảnh loading)
                        // picQR.Image = Properties.Resources.loading;

                        await LoadQRCodeAsync(chiTiet.MaHoaDon, chiTiet.TongTien);
                    }
                }
                else
                {
                    MessageBox.Show("Giá trị IDHoaDon không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task LoadQRCodeAsync(string maHoaDon, decimal tongTien)
        {
            string soTaiKhoan = "77777777997";
            int soTien = (int)Math.Round(tongTien);
            string urlQR = $"https://img.vietqr.io/image/TCB-{soTaiKhoan}-compact.png?amount={soTien}&addInfo={maHoaDon}";

            try
            {
                var imageBytes = await httpClient.GetByteArrayAsync(urlQR);
                using var ms = new System.IO.MemoryStream(imageBytes);
                picQR.Image = System.Drawing.Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                picQR.Image = null;
                MessageBox.Show("Lỗi tải ảnh QR: " + ex.Message);
            }
        }


        private async Task<HoaDonChiTiet?> LoadChiTietHoaDonAsync(int idHoaDon)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            string query = @"
                SELECT hd.MaHoaDon, hd.NgayLap, CAST(hd.TongTien AS INT) AS TongTien, kh.TenKH, nv.IDNhanVien
                FROM HoaDon hd
                LEFT JOIN KhachHang kh ON hd.IDKhachHang = kh.IDKhachHang
                LEFT JOIN NhanVien nv ON hd.IDNhanVien = nv.IDNhanVien
                WHERE hd.IDHoaDon = @IDHoaDon";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDHoaDon", idHoaDon);

            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new HoaDonChiTiet
                {
                    MaHoaDon = reader["MaHoaDon"].ToString() ?? "",
                    NgayLap = reader["NgayLap"] != DBNull.Value ? Convert.ToDateTime(reader["NgayLap"]) : DateTime.Now,
                    TongTien = reader["TongTien"] != DBNull.Value ? Convert.ToInt32(reader["TongTien"]) : 0,
                    TenKhachHang = reader["TenKH"].ToString() ?? "",
                    IDNhanVien = reader["IDNhanVien"] != DBNull.Value ? Convert.ToInt32(reader["IDNhanVien"]) : -1
                };
            }
            return null;
        }

        private void LoadChiTietMonAn(int idHoaDon)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            string query = @"
                SELECT ct.IDMonAn, ma.TenMon, ct.SoLuong, ct.DonGia
                FROM ChiTietHoaDon ct
                INNER JOIN MonAn ma ON ct.IDMonAn = ma.IDMonAn
                WHERE ct.IDHoaDon = @IDHoaDon";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDHoaDon", idHoaDon);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dtgvChiTietMonAn.DataSource = dt;
        }

        private async Task TaoQRCodeAsync(string maHoaDon, int tongTien)
        {
            string soTaiKhoan = "77777777997";
            string urlQR = $"https://img.vietqr.io/image/TCB-{soTaiKhoan}-compact.png?amount={tongTien}&addInfo={maHoaDon}";

            try
            {
                using HttpClient httpClient = new HttpClient();
                var imageBytes = await httpClient.GetByteArrayAsync(urlQR);
                using MemoryStream ms = new MemoryStream(imageBytes);
                picQR.Image = System.Drawing.Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                picQR.Image = null;
                MessageBox.Show("Lỗi tải ảnh QR: " + ex.Message);
            }
        }

        // Hàm in hóa đơn, cập nhật trạng thái, xóa hóa đơn giữ nguyên như bạn có

        private async Task TaoHoaDonWordAsync(string filePath, string maHoaDon, string tenKhachHang, DateTime ngayLap, string tenNhanVien, decimal tongTien, string urlQr)
        {
            using WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
            MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            mainPart.Document = new Document();
            Body body = mainPart.Document.AppendChild(new Body());

            void ThemDoan(string text, bool bold = false, JustificationValues? justify = null)
            {
                var actualJustify = justify ?? JustificationValues.Left;
                Paragraph para = new Paragraph();
                Run run = new Run();
                if (bold)
                    run.AppendChild(new Bold());
                run.AppendChild(new Text(text));
                para.AppendChild(run);
                para.AppendChild(new ParagraphProperties(new Justification() { Val = actualJustify }));
                body.AppendChild(para);
            }

            ThemDoan("HÓA ĐƠN BÁN HÀNG", true, JustificationValues.Center);
            ThemDoan($"Mã hóa đơn: {maHoaDon}", true);
            ThemDoan($"Tên khách hàng: {tenKhachHang}");
            ThemDoan($"Ngày lập: {ngayLap.ToShortDateString()}");
            ThemDoan($"Tên nhân viên: {tenNhanVien}");
            ThemDoan($"Tổng tiền: {tongTien} VND");

            body.AppendChild(new Paragraph(new Run(new Text(""))));

            if (!string.IsNullOrEmpty(urlQr))
            {
                await AddImageFromUrlAsync(mainPart, urlQr);
            }

            mainPart.Document.Save();
        }

        private async Task AddImageFromUrlAsync(MainDocumentPart mainPart, string imageUrl)
        {
            using HttpClient httpClient = new HttpClient();
            byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);
            using MemoryStream stream = new MemoryStream(imageBytes);
            imagePart.FeedData(stream);

            AddImageToBody(mainPart.GetIdOfPart(imagePart), mainPart.Document.Body);
        }

        private void AddImageToBody(string relationshipId, Body body)
        {
            var element =
                 new Paragraph(
                     new Run(
                         new Drawing(
                             new DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline(
                                 new DocumentFormat.OpenXml.Drawing.Wordprocessing.Extent() { Cx = 990000L, Cy = 990000L },
                                 new DocumentFormat.OpenXml.Drawing.Wordprocessing.EffectExtent()
                                 {
                                     LeftEdge = 0L,
                                     TopEdge = 0L,
                                     RightEdge = 0,
                                     BottomEdge = 0
                                 },
                                 new DocumentFormat.OpenXml.Drawing.Wordprocessing.DocProperties()
                                 {
                                     Id = (UInt32Value)1U,
                                     Name = "QR Code"
                                 },
                                 new DocumentFormat.OpenXml.Drawing.Wordprocessing.NonVisualGraphicFrameDrawingProperties(
                                     new DocumentFormat.OpenXml.Drawing.GraphicFrameLocks() { NoChangeAspect = true }),
                                 new DocumentFormat.OpenXml.Drawing.Graphic(
                                     new DocumentFormat.OpenXml.Drawing.GraphicData(
                                         new DocumentFormat.OpenXml.Drawing.Pictures.Picture(
                                             new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureProperties(
                                                 new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualDrawingProperties()
                                                 {
                                                     Id = (UInt32Value)0U,
                                                     Name = "QRCode.png"
                                                 },
                                                 new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureDrawingProperties()),
                                             new DocumentFormat.OpenXml.Drawing.Pictures.BlipFill(
                                                 new DocumentFormat.OpenXml.Drawing.Blip()
                                                 {
                                                     Embed = relationshipId,
                                                     CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print
                                                 },
                                                 new DocumentFormat.OpenXml.Drawing.Stretch(
                                                     new DocumentFormat.OpenXml.Drawing.FillRectangle())),
                                             new DocumentFormat.OpenXml.Drawing.ShapeProperties(
                                                 new DocumentFormat.OpenXml.Drawing.Transform2D(
                                                     new DocumentFormat.OpenXml.Drawing.Offset() { X = 0L, Y = 0L },
                                                     new DocumentFormat.OpenXml.Drawing.Extents() { Cx = 990000L, Cy = 990000L }),
                                                 new DocumentFormat.OpenXml.Drawing.PresetGeometry(
                                                     new DocumentFormat.OpenXml.Drawing.AdjustValueList()
                                                 )
                                                 { Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle }))

                                         )
                                     { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                             )
                             { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U })));

            body.AppendChild(element);
        }

        // Khi in hóa đơn xong, cập nhật trạng thái hóa đơn và bàn ăn
        private async void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (currentIdHoaDon < 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Word Document|*.docx";
                saveFileDialog.FileName = $"{txtMaHoaDon.Text}.docx";
                saveFileDialog.Title = "Chọn vị trí lưu hóa đơn";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    string maHoaDon = txtMaHoaDon.Text;
                    string tenKhachHang = txtKhachHang.Text;
                    DateTime ngayLap = dtpNgayLap.Value;
                    string tenNhanVien = cmbNhanVien.Text;
                    decimal tongTien = decimal.Parse(txtTongTien.Text.Replace("VND", "").Trim());
                    string qrUrl = picQR.ImageLocation;

                    try
                    {
                        await System.Threading.Tasks.Task.Run(() =>
                            TaoHoaDonWordAsync(filePath, maHoaDon, tenKhachHang,
                            ngayLap, tenNhanVien,
                            tongTien, qrUrl)
                        );

                        // Cập nhật trạng thái hóa đơn
                        CapNhatTrangThaiHoaDon(currentIdHoaDon);

                        // Lấy số bàn từ hóa đơn
                        string soBan = LaySoBanTheoHoaDon(currentIdHoaDon);
                        int idBanAn = LayIDBanAnTheoSoBan(soBan);

                        // Cập nhật trạng thái bàn thành "Trống"
                        if (formQuanLyBan != null && idBanAn > 0)
                        {
                            formQuanLyBan.CapNhatTrangThaiBanTheoIDBanAn(idBanAn, "Trống");

                            // Load lại danh sách bàn trong form bàn ăn để cập nhật UI
                            formQuanLyBan.LoadDanhSachBan();
                        }

                        LoadDanhSachHoaDon();

                        MessageBox.Show($"Đã xuất hóa đơn ra file:\n{filePath}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi in hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Khi xóa hóa đơn, cũng cập nhật trạng thái bàn
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (currentIdHoaDon < 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xóa");
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa hóa đơn này không?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No) return;

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            // Xóa chi tiết hóa đơn trước
            string deleteChiTiet = "DELETE FROM ChiTietHoaDon WHERE IDHoaDon = @IDHoaDon";
            SqlCommand delCTCmd = new SqlCommand(deleteChiTiet, conn);
            delCTCmd.Parameters.AddWithValue("@IDHoaDon", currentIdHoaDon);
            delCTCmd.ExecuteNonQuery();

            // Xóa hóa đơn
            string deleteHoaDon = "DELETE FROM HoaDon WHERE IDHoaDon = @IDHoaDon";
            SqlCommand delCmd = new SqlCommand(deleteHoaDon, conn);
            delCmd.Parameters.AddWithValue("@IDHoaDon", currentIdHoaDon);
            int result = delCmd.ExecuteNonQuery();

            if (result > 0)
            {
                string soBan = LaySoBanTheoHoaDon(currentIdHoaDon);
                int idBanAn = LayIDBanAnTheoSoBan(soBan);

                if (formQuanLyBan != null && idBanAn > 0)
                {
                    formQuanLyBan.CapNhatTrangThaiBanTheoIDBanAn(idBanAn, "Trống");

                    // Load lại danh sách bàn để cập nhật UI
                    formQuanLyBan.LoadDanhSachBan();
                }

                MessageBox.Show("Xóa hóa đơn thành công!");
                LoadDanhSachHoaDon();
                btnLamMoi_Click(null, null);
            }
            else
            {
                MessageBox.Show("Xóa hóa đơn thất bại.");
            }
        }



        private void CapNhatTrangThaiHoaDon(int idHoaDon)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            string query = "UPDATE HoaDon SET TrangThai = N'Đã thanh toán' WHERE IDHoaDon = @IDHoaDon";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDHoaDon", idHoaDon);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private string LaySoBanTheoHoaDon(int idHoaDon)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT SoBan FROM HoaDon WHERE IDHoaDon = @IDHoaDon";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDHoaDon", idHoaDon);

            conn.Open();
            object result = cmd.ExecuteScalar();
            conn.Close();

            return result?.ToString() ?? string.Empty;
        }

        private int LayIDBanAnTheoSoBan(string soBan)
        {
            if (string.IsNullOrEmpty(soBan)) return -1;

            using SqlConnection conn = new SqlConnection(connectionString);
            string query = "SELECT IDBanAn FROM BanAn WHERE SoBan = @SoBan";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SoBan", soBan);

            conn.Open();
            object result = cmd.ExecuteScalar();
            conn.Close();

            if (result != null && int.TryParse(result.ToString(), out int idBanAn))
            {
                return idBanAn;
            }
            return -1;
        }


        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaHoaDon.Clear();
            txtKhachHang.Clear();
            dtpNgayLap.Value = DateTime.Now;
            cmbNhanVien.SelectedIndex = -1;
            txtTongTien.Clear();
            picQR.Image = null;
            dtgvDSHoaDon.ClearSelection();

            LoadDanhSachHoaDon();
        }

        private void CapNhatTrangThaiBanSauThanhToanHoacXoaHoaDon()
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string resetTrangThaiQuery = "UPDATE BanAn SET TrangThai = N'Trống'";
            SqlCommand resetCmd = new SqlCommand(resetTrangThaiQuery, conn);
            resetCmd.ExecuteNonQuery();

            string datBanQuery = @"
                UPDATE BanAn
                SET TrangThai = N'Đã đặt'
                WHERE IDBanAn IN (SELECT IDBanAn FROM DatBan)";
            SqlCommand datBanCmd = new SqlCommand(datBanQuery, conn);
            datBanCmd.ExecuteNonQuery();

            string chuaThanhToanQuery = @"
                UPDATE BanAn
                SET TrangThai = N'Chưa thanh toán'
                WHERE IDBanAn IN (SELECT SoBan FROM HoaDon WHERE TrangThai = N'Chưa thanh toán')";
            SqlCommand chuaThanhToanCmd = new SqlCommand(chuaThanhToanQuery, conn);
            chuaThanhToanCmd.ExecuteNonQuery();

            conn.Close();
        }

        private class HoaDonChiTiet
        {
            public string MaHoaDon { get; set; }
            public DateTime NgayLap { get; set; }
            public int TongTien { get; set; }
            public string TenKhachHang { get; set; }
            public int IDNhanVien { get; set; }
        }
    }
}
