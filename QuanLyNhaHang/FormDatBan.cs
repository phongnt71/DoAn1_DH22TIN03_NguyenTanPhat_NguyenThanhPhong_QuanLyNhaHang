using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class FormDatBan : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;TrustServerCertificate=True";

        public FormDatBan()
        {
            InitializeComponent();
            this.Load += FormDatBan_Load;
            dtgvDatBan.CellClick += DtgvDatBan_CellClick;
        }

        private void FormDatBan_Load(object sender, EventArgs e)
        {
            LoadBanAn();
            LoadDatBan();
        }

        private void LoadBanAn()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDBanAn, SoBan FROM BanAn WHERE TrangThai = N'Trống'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbBanAn.DataSource = dt;
                cmbBanAn.DisplayMember = "SoBan";
                cmbBanAn.ValueMember = "IDBanAn";
                cmbBanAn.SelectedIndex = -1;
            }
        }

        private void LoadDatBan()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT d.IDDatBan, b.SoBan, k.TenKH, d.NgayDat, d.GioDat, d.TrangThai
                                 FROM DatBan d
                                 JOIN BanAn b ON d.IDBanAn = b.IDBanAn
                                 JOIN KhachHang k ON d.IDKhachHang = k.IDKhachHang";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtgvDatBan.DataSource = dt;
                dtgvDatBan.Columns["IDDatBan"].Visible = false;
                dtgvDatBan.Columns["TrangThai"].Visible = false;
                dtgvDatBan.Columns["SoBan"].HeaderText = "Số bàn";
                dtgvDatBan.Columns["TenKH"].HeaderText = "Tên khách hàng";
                dtgvDatBan.Columns["NgayDat"].HeaderText = "Ngày đặt";
                dtgvDatBan.Columns["GioDat"].HeaderText = "Giờ đặt";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string tenKhachHang = txtTenKhachHang.Text.Trim();
            if (string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.");
                return;
            }

            if (cmbBanAn.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bàn.");
                return;
            }

            int idBanAn = (int)cmbBanAn.SelectedValue;
            DateTime ngayDat = dtpNgayDat.Value.Date;
            DateTime gioDat = dtpGioDat.Value;
            DateTime thoiGianDat = ngayDat.Add(gioDat.TimeOfDay);

            if (thoiGianDat < DateTime.Now)
            {
                MessageBox.Show("Không thể đặt bàn ở thời điểm trong quá khứ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                            MessageBox.Show("Bạn chưa thêm khách hàng, không thể đặt bàn.");
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

                string insert = @"INSERT INTO DatBan (IDBanAn, IDKhachHang, NgayDat, GioDat, TrangThai)
                                  VALUES (@IDBanAn, @IDKhachHang, @NgayDat, @GioDat, N'Đã đặt')";
                SqlCommand cmd = new SqlCommand(insert, conn);
                cmd.Parameters.AddWithValue("@IDBanAn", idBanAn);
                cmd.Parameters.AddWithValue("@IDKhachHang", idKhachHang.Value);
                cmd.Parameters.AddWithValue("@NgayDat", ngayDat);
                cmd.Parameters.AddWithValue("@GioDat", gioDat);
                cmd.ExecuteNonQuery();

                string updateBan = "UPDATE BanAn SET TrangThai = N'Đã đặt' WHERE IDBanAn = @IDBanAn";
                SqlCommand cmdUpdate = new SqlCommand(updateBan, conn);
                cmdUpdate.Parameters.AddWithValue("@IDBanAn", idBanAn);
                cmdUpdate.ExecuteNonQuery();
            }

            MessageBox.Show("Đặt bàn thành công!");
            LoadBanAn();
            LoadDatBan();
            ClearForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dtgvDatBan.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đặt bàn để sửa.");
                return;
            }

            int idDatBan = (int)dtgvDatBan.CurrentRow.Cells["IDDatBan"].Value;
            string tenKhachHang = txtTenKhachHang.Text.Trim();

            if (string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.");
                return;
            }

            if (cmbBanAn.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bàn.");
                return;
            }

            int newIdBanAn = (int)cmbBanAn.SelectedValue;
            DateTime ngayDat = dtpNgayDat.Value.Date;
            DateTime gioDat = dtpGioDat.Value;
            DateTime thoiGianDat = ngayDat.Add(gioDat.TimeOfDay);

            if (thoiGianDat < DateTime.Now)
            {
                MessageBox.Show("Thời gian đặt không hợp lệ. Vui lòng chọn thời điểm hiện tại hoặc trong tương lai.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? idKhachHang = GetKhachHangIdByName(tenKhachHang);
            if (idKhachHang == null)
            {
                MessageBox.Show("Khách hàng không tồn tại.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string queryCurrentBan = "SELECT IDBanAn FROM DatBan WHERE IDDatBan = @IDDatBan";
                SqlCommand cmdCurrentBan = new SqlCommand(queryCurrentBan, conn);
                cmdCurrentBan.Parameters.AddWithValue("@IDDatBan", idDatBan);
                int currentIdBanAn = (int)cmdCurrentBan.ExecuteScalar();

                string update = @"UPDATE DatBan 
                          SET IDBanAn = @IDBanAn, IDKhachHang = @IDKhachHang, NgayDat = @NgayDat, GioDat = @GioDat 
                          WHERE IDDatBan = @IDDatBan";
                SqlCommand cmd = new SqlCommand(update, conn);
                cmd.Parameters.AddWithValue("@IDDatBan", idDatBan);
                cmd.Parameters.AddWithValue("@IDBanAn", newIdBanAn);
                cmd.Parameters.AddWithValue("@IDKhachHang", idKhachHang.Value);
                cmd.Parameters.AddWithValue("@NgayDat", ngayDat);
                cmd.Parameters.AddWithValue("@GioDat", gioDat);
                cmd.ExecuteNonQuery();

                if (newIdBanAn != currentIdBanAn)
                {
                    string updateNewBan = "UPDATE BanAn SET TrangThai = N'Đã đặt' WHERE IDBanAn = @IDBanAn";
                    SqlCommand cmdUpdateNewBan = new SqlCommand(updateNewBan, conn);
                    cmdUpdateNewBan.Parameters.AddWithValue("@IDBanAn", newIdBanAn);
                    cmdUpdateNewBan.ExecuteNonQuery();

                    string updateOldBan = "UPDATE BanAn SET TrangThai = N'Trống' WHERE IDBanAn = @IDBanAn";
                    SqlCommand cmdUpdateOldBan = new SqlCommand(updateOldBan, conn);
                    cmdUpdateOldBan.Parameters.AddWithValue("@IDBanAn", currentIdBanAn);
                    cmdUpdateOldBan.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Sửa đặt bàn thành công!");
            LoadBanAn();
            LoadDatBan();
            ClearForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dtgvDatBan.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đặt bàn để xóa.");
                return;
            }

            int idDatBan = (int)dtgvDatBan.CurrentRow.Cells["IDDatBan"].Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string queryBan = "SELECT IDBanAn FROM DatBan WHERE IDDatBan = @IDDatBan";
                SqlCommand cmdBan = new SqlCommand(queryBan, conn);
                cmdBan.Parameters.AddWithValue("@IDDatBan", idDatBan);
                int idBanAn = (int)cmdBan.ExecuteScalar();

                string delete = "DELETE FROM DatBan WHERE IDDatBan = @IDDatBan";
                SqlCommand cmd = new SqlCommand(delete, conn);
                cmd.Parameters.AddWithValue("@IDDatBan", idDatBan);
                cmd.ExecuteNonQuery();

                string updateBan = "UPDATE BanAn SET TrangThai = N'Trống' WHERE IDBanAn = @IDBanAn";
                SqlCommand cmdUpdate = new SqlCommand(updateBan, conn);
                cmdUpdate.Parameters.AddWithValue("@IDBanAn", idBanAn);
                cmdUpdate.ExecuteNonQuery();
            }

            MessageBox.Show("Xóa đặt bàn thành công!");
            LoadBanAn();
            LoadDatBan();
            ClearForm();
        }

        private void DtgvDatBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvDatBan.Rows[e.RowIndex];
                txtTenKhachHang.Text = row.Cells["TenKH"]?.Value?.ToString() ?? "";
                cmbBanAn.Text = row.Cells["SoBan"]?.Value?.ToString() ?? "";

                if (row.Cells["NgayDat"].Value != DBNull.Value)
                    dtpNgayDat.Value = Convert.ToDateTime(row.Cells["NgayDat"].Value);

                if (row.Cells["GioDat"].Value != DBNull.Value)
                    dtpGioDat.Value = dtpNgayDat.Value.Date.Add((TimeSpan)row.Cells["GioDat"].Value);
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
                return result != null ? Convert.ToInt32(result) : (int?)null;
            }
        }

        private void ClearForm()
        {
            txtTenKhachHang.Text = "";
            cmbBanAn.SelectedIndex = -1;
            dtpNgayDat.Value = DateTime.Now;
            dtpGioDat.Value = DateTime.Now;
        }
    }
}
