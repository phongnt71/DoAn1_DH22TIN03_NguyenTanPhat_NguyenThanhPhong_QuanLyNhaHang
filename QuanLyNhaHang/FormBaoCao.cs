using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class FormBaoCao : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True";

        public FormBaoCao()
        {
            InitializeComponent();

            btnThongKeNhapHang.Click += BtnThongKeNhapHang_Click;
            btnXemTongHoaDon.Click += BtnXemTongHoaDon_Click;
            btnThongKeTheoKhoangThoiGian.Click += BtnThongKeTheoKhoangThoiGian_Click;
        }

        private void BtnThongKeNhapHang_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpNhapHangFrom.Value.Date;
            DateTime toDate = dtpNhapHangTo.Value.Date;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT SUM(GiaNhap * SoLuongTon) 
                    FROM NguyenLieu
                    WHERE NgayNhap BETWEEN @FromDate AND @ToDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);

                object result = cmd.ExecuteScalar();
                decimal tongTien = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                txtTongTienNhapHang.Text = string.Format("{0:N0} VNĐ", tongTien);
            }
        }

        private void BtnXemTongHoaDon_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpHoaDonFrom.Value.Date;
            DateTime toDate = dtpHoaDonTo.Value.Date;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT SUM(TongTien)
                    FROM HoaDon
                    WHERE TrangThai = N'Đã thanh toán'
                      AND CAST(NgayLap AS DATE) BETWEEN @FromDate AND @ToDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);

                object result = cmd.ExecuteScalar();
                decimal tongTien = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                txtTongTienHoaDonDaThanhToan.Text = string.Format("{0:N0} VNĐ", tongTien);
            }
        }

        private void BtnThongKeTheoKhoangThoiGian_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpThuFrom.Value.Date;
            DateTime toDate = dtpThuTo.Value.Date;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT CAST(NgayLap AS DATE) AS Ngay,
                           SUM(TongTien) AS TongTienThu
                    FROM HoaDon
                    WHERE TrangThai = N'Đã thanh toán'
                      AND CAST(NgayLap AS DATE) BETWEEN @FromDate AND @ToDate
                    GROUP BY CAST(NgayLap AS DATE)
                    ORDER BY Ngay";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtgvTongThuTheoNgay.DataSource = dt;

                dtgvTongThuTheoNgay.Columns["TongTienThu"].DefaultCellStyle.Format = "N0";
                dtgvTongThuTheoNgay.Columns["Ngay"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
    }
}
