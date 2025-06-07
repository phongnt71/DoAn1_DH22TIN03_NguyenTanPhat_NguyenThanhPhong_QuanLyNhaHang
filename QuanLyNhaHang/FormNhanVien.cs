using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyNhaHang
{
    public partial class FormNhanVien : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;Trust Server Certificate=True";
        private DataTable dtNhanVien;
        private bool isSaving = false;

        public FormNhanVien()
        {
            InitializeComponent();
            Load += FormNhanVien_Load; 

            dtgvNhanVien.CellClick += DtgvNhanVien_CellClick;

            SetControlsEnabled(true);
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            LoadQuyen();
            LoadDataNhanVien();
            SetHeaders();
            ClearInputs();
        }

        private void LoadQuyen()
        {
            cmbQuyen.Items.Clear();
            cmbQuyen.Items.Add("Admin");
            cmbQuyen.Items.Add("QuanLy");
            cmbQuyen.Items.Add("NhanVien");
            cmbQuyen.SelectedIndex = -1;
        }

        private void LoadDataNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT IDNhanVien, TenNV, TaiKhoan, MatKhau, Quyen FROM NhanVien";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                dtNhanVien = new DataTable();
                adapter.Fill(dtNhanVien);
                dtgvNhanVien.DataSource = dtNhanVien;
            }
        }

        private void SetHeaders()
        {
            dtgvNhanVien.Columns["TenNV"].HeaderText = "Tên nhân viên";
            dtgvNhanVien.Columns["TaiKhoan"].HeaderText = "Tài khoản";
            dtgvNhanVien.Columns["MatKhau"].HeaderText = "Mật khẩu";
            dtgvNhanVien.Columns["Quyen"].HeaderText = "Quyền";
        }

        private void DtgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvNhanVien.Rows[e.RowIndex];
                txtIDNhanVien.Text = row.Cells["IDNhanVien"].Value?.ToString();
                txtTenNhanVien.Text = row.Cells["TenNV"].Value?.ToString();
                txtTaiKhoan.Text = row.Cells["TaiKhoan"].Value?.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value?.ToString();
                cmbQuyen.Text = row.Cells["Quyen"].Value?.ToString();

                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnLuu.Enabled = false;
                btnHuy.Enabled = true;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtIDNhanVien.Enabled = false;
            txtTenNhanVien.Enabled = true;
            txtTaiKhoan.Enabled = true;
            txtMatKhau.Enabled = true;
            cmbQuyen.Enabled = true;

            ClearInputs();

            btnLuu.Enabled = true;
            btnHuy.Enabled = true;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDNhanVien.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SetControlsEnabled(false);
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;

            btnThem.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dtgvNhanVien.CurrentRow == null || dtgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idNhanVien = Convert.ToInt32(dtgvNhanVien.CurrentRow.Cells["IDNhanVien"].Value);

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM NhanVien WHERE IDNhanVien = @IDNhanVien";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IDNhanVien", idNhanVien);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                LoadDataNhanVien();
                ClearInputs();
                SetControlsEnabled(true);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (isSaving) return;

            if (!KiemTraThongTin())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                isSaving = true;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (string.IsNullOrWhiteSpace(txtIDNhanVien.Text)) // Thêm mới
                    {
                        string queryInsert = @"INSERT INTO NhanVien (TenNV, TaiKhoan, MatKhau, Quyen) 
                                   VALUES (@TenNV, @TaiKhoan, @MatKhau, @Quyen)";
                        SqlCommand cmd = new SqlCommand(queryInsert, conn);
                        cmd.Parameters.AddWithValue("@TenNV", txtTenNhanVien.Text);
                        cmd.Parameters.AddWithValue("@TaiKhoan", txtTaiKhoan.Text);
                        cmd.Parameters.AddWithValue("@MatKhau", txtMatKhau.Text);
                        cmd.Parameters.AddWithValue("@Quyen", cmbQuyen.Text);
                        cmd.ExecuteNonQuery();
                    }
                    else // Cập nhật
                    {
                        string queryUpdate = @"UPDATE NhanVien SET TenNV = @TenNV, TaiKhoan = @TaiKhoan, MatKhau = @MatKhau, Quyen = @Quyen
                                   WHERE IDNhanVien = @IDNhanVien";
                        SqlCommand cmd = new SqlCommand(queryUpdate, conn);
                        cmd.Parameters.AddWithValue("@TenNV", txtTenNhanVien.Text);
                        cmd.Parameters.AddWithValue("@TaiKhoan", txtTaiKhoan.Text);
                        cmd.Parameters.AddWithValue("@MatKhau", txtMatKhau.Text);
                        cmd.Parameters.AddWithValue("@Quyen", cmbQuyen.Text);
                        cmd.Parameters.AddWithValue("@IDNhanVien", int.Parse(txtIDNhanVien.Text));
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                LoadDataNhanVien();
                ClearInputs();
                SetControlsEnabled(true);

                btnLuu.Enabled = false;
                btnHuy.Enabled = false;

                MessageBox.Show("Đã lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isSaving = false;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearInputs();
            SetControlsEnabled(true);
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }

        private void ClearInputs()
        {
            txtIDNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            cmbQuyen.SelectedIndex = -1;

            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtTenNhanVien.Enabled = enabled;
            txtTaiKhoan.Enabled = enabled;
            txtMatKhau.Enabled = enabled;
            cmbQuyen.Enabled = enabled;

            btnThem.Enabled = enabled;
            btnSua.Enabled = enabled;
            btnXoa.Enabled = enabled;
        }

        private bool KiemTraThongTin()
        {
            if (string.IsNullOrWhiteSpace(txtTenNhanVien.Text) ||
                string.IsNullOrWhiteSpace(txtTaiKhoan.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhau.Text) ||
                cmbQuyen.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }
    }
}
