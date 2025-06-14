using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class FormNhapNguyenLieu : Form
    {
        private readonly string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True";
        private int idPhieuNhapDangChon = -1;

        public FormNhapNguyenLieu()
        {
            InitializeComponent();
        }

        private void FormNhapNguyenLieu_Load(object sender, EventArgs e)
        {
            LoadTongTon();
            dtgvNguyenLieu.CellClick += dtgvNguyenLieu_CellClick;
            dtgvLichSuNhap.CellClick += dtgvLichSuNhap_CellClick;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNguyenLieu.Text) ||
                string.IsNullOrWhiteSpace(txtSoLuong.Text) ||
                string.IsNullOrWhiteSpace(txtGiaNhap.Text) ||
                string.IsNullOrWhiteSpace(txtDonViTinh.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nguyên liệu.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                int maNguyenLieu;

                SqlCommand cmdCheck = new SqlCommand("SELECT IDNguyenLieu FROM NguyenLieu WHERE TenNguyenLieu = @ten", conn);
                cmdCheck.Parameters.AddWithValue("@ten", txtTenNguyenLieu.Text);
                var result = cmdCheck.ExecuteScalar();

                if (result == null)
                {
                    SqlCommand cmdInsert = new SqlCommand(
                        "INSERT INTO NguyenLieu (TenNguyenLieu, DonViTinh, SoLuongTon, GiaNhap, NgayNhap) " +
                        "VALUES (@ten, @dvt, 0, 0, GETDATE()); SELECT SCOPE_IDENTITY();", conn);
                    cmdInsert.Parameters.AddWithValue("@ten", txtTenNguyenLieu.Text);
                    cmdInsert.Parameters.AddWithValue("@dvt", txtDonViTinh.Text);
                    maNguyenLieu = Convert.ToInt32(cmdInsert.ExecuteScalar());
                }
                else
                {
                    maNguyenLieu = Convert.ToInt32(result);
                }

                SqlCommand cmdNhap = new SqlCommand(
                    "INSERT INTO PhieuNhapNguyenLieu (MaNguyenLieu, SoLuong, GiaNhap, NgayNhap, GhiChu) " +
                    "VALUES (@ma, @sl, @gia, @ngay, @ghichu)", conn);
                cmdNhap.Parameters.AddWithValue("@ma", maNguyenLieu);
                cmdNhap.Parameters.AddWithValue("@sl", int.Parse(txtSoLuong.Text));
                cmdNhap.Parameters.AddWithValue("@gia", decimal.Parse(txtGiaNhap.Text));
                cmdNhap.Parameters.AddWithValue("@ngay", dtpNgayNhap.Value);
                cmdNhap.Parameters.AddWithValue("@ghichu", txtGhiChu.Text);
                cmdNhap.ExecuteNonQuery();

                SqlCommand cmdUpdateTon = new SqlCommand(
                    "UPDATE NguyenLieu SET SoLuongTon = ISNULL(SoLuongTon, 0) + @sl WHERE IDNguyenLieu = @id", conn);
                cmdUpdateTon.Parameters.AddWithValue("@sl", int.Parse(txtSoLuong.Text));
                cmdUpdateTon.Parameters.AddWithValue("@id", maNguyenLieu);
                cmdUpdateTon.ExecuteNonQuery();

                MessageBox.Show("Đã nhập kho nguyên liệu thành công!");

                LoadTongTon();
                LoadLichSuNhap(maNguyenLieu);

                txtTenNguyenLieu.Clear();
                txtDonViTinh.Clear();
                txtSoLuong.Clear();
                txtGiaNhap.Clear();
                txtGhiChu.Clear();
                dtpNgayNhap.Value = DateTime.Now;
            }
        }

        private void LoadTongTon()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT n.IDNguyenLieu AS [Mã], n.TenNguyenLieu AS [Tên nguyên liệu], n.DonViTinh AS [Đơn vị tính], ISNULL(n.SoLuongTon, 0) AS [Tổng tồn] FROM NguyenLieu n ORDER BY n.TenNguyenLieu";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dtgvNguyenLieu.DataSource = dt;
            }
        }

        private void dtgvNguyenLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvNguyenLieu.Rows[e.RowIndex];
                txtTenNguyenLieu.Text = row.Cells["Tên nguyên liệu"].Value.ToString();
                txtDonViTinh.Text = row.Cells["Đơn vị tính"].Value.ToString();
                txtSoLuong.Clear();
                txtGiaNhap.Clear();
                txtGhiChu.Clear();
                dtpNgayNhap.Value = DateTime.Now;
                int maNguyenLieu = Convert.ToInt32(row.Cells["Mã"].Value);
                LoadLichSuNhap(maNguyenLieu);
            }
        }

        private void LoadLichSuNhap(int maNguyenLieu)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT MaPhieuNhap, SoLuong AS [Số lượng], GiaNhap AS [Giá nhập], NgayNhap AS [Ngày nhập], GhiChu AS [Ghi chú] FROM PhieuNhapNguyenLieu WHERE MaNguyenLieu = @ma ORDER BY NgayNhap DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ma", maNguyenLieu);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dtgvLichSuNhap.DataSource = dt;
                dtgvLichSuNhap.Columns["MaPhieuNhap"].Visible = false;
            }
        }

        private void dtgvLichSuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvLichSuNhap.Rows[e.RowIndex];
                txtSoLuong.Text = row.Cells["Số lượng"].Value.ToString();
                txtGiaNhap.Text = row.Cells["Giá nhập"].Value.ToString();
                txtGhiChu.Text = row.Cells["Ghi chú"].Value?.ToString();
                dtpNgayNhap.Value = Convert.ToDateTime(row.Cells["Ngày nhập"].Value);
                idPhieuNhapDangChon = Convert.ToInt32(row.Cells["MaPhieuNhap"].Value);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (idPhieuNhapDangChon == -1)
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập để sửa.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE PhieuNhapNguyenLieu SET SoLuong = @sl, GiaNhap = @gia, GhiChu = @ghichu, NgayNhap = @ngay WHERE MaPhieuNhap = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sl", int.Parse(txtSoLuong.Text));
                cmd.Parameters.AddWithValue("@gia", decimal.Parse(txtGiaNhap.Text));
                cmd.Parameters.AddWithValue("@ghichu", txtGhiChu.Text);
                cmd.Parameters.AddWithValue("@ngay", dtpNgayNhap.Value);
                cmd.Parameters.AddWithValue("@id", idPhieuNhapDangChon);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Cập nhật phiếu nhập thành công!");
            LoadTongTon();
            LoadLichSuNhap(LayMaNguyenLieuDangChon());
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (idPhieuNhapDangChon == -1)
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập để xóa.");
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa phiếu nhập này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.No) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM PhieuNhapNguyenLieu WHERE MaPhieuNhap = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idPhieuNhapDangChon);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Đã xóa phiếu nhập.");
            LoadTongTon();
            LoadLichSuNhap(LayMaNguyenLieuDangChon());
        }

        private int LayMaNguyenLieuDangChon()
        {
            if (dtgvNguyenLieu.CurrentRow != null)
            {
                return Convert.ToInt32(dtgvNguyenLieu.CurrentRow.Cells["Mã"].Value);
            }
            return -1;
        }
    }
}
