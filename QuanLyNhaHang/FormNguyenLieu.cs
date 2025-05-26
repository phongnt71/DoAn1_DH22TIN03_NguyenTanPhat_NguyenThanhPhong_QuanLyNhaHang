using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyNhaHang
{
    public partial class FormNguyenLieu : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;Trust Server Certificate=True";
        private DataTable dtNguyenLieu;

        private bool isAdding = false;
        private bool isEditing = false;

        public FormNguyenLieu()
        {
            InitializeComponent();
            InitControls();
            LoadData();
        }

        private void InitControls()
        {
            dgvNguyenLieu.SelectionChanged += DgvNguyenLieu_SelectionChanged;
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += BtnHuy_Click;

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM NguyenLieu";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    dtNguyenLieu = new DataTable();
                    da.Fill(dtNguyenLieu);
                    dgvNguyenLieu.DataSource = dtNguyenLieu;

                    dgvNguyenLieu.Columns["IDNguyenLieu"].HeaderText = "Mã";
                    dgvNguyenLieu.Columns["TenNguyenLieu"].HeaderText = "Tên nguyên liệu";
                    dgvNguyenLieu.Columns["SoLuongTon"].HeaderText = "Số lượng";
                    dgvNguyenLieu.Columns["DonViTinh"].HeaderText = "Đơn vị tính";
                    dgvNguyenLieu.Columns["GhiChu"].HeaderText = "Ghi chú";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message);
                }
            }
        }

        private void DgvNguyenLieu_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNguyenLieu.CurrentRow != null && dgvNguyenLieu.CurrentRow.Index >= 0)
            {
                var row = dgvNguyenLieu.CurrentRow;
                txtTenNguyenLieu.Text = row.Cells["TenNguyenLieu"].Value?.ToString();
                txtSoLuongTon.Text = row.Cells["SoLuongTon"].Value?.ToString();
                txtDonViTinh.Text = row.Cells["DonViTinh"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearInput();
            isAdding = true;
            isEditing = false;
            SetButtonState();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvNguyenLieu.CurrentRow != null)
            {
                isAdding = false;
                isEditing = true;
                SetButtonState();
            }
        }
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNguyenLieu.CurrentRow == null) return;

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa nguyên liệu này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dr != DialogResult.Yes) return;

            string id = dgvNguyenLieu.CurrentRow.Cells["IDNguyenLieu"].Value.ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "DELETE FROM NguyenLieu WHERE IDNguyenLieu = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Xóa nguyên liệu thành công!");
                    LoadData();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        MessageBox.Show("Không thể xóa nguyên liệu này vì đang được sử dụng trong món ăn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd;

                if (isAdding)
                {
                    string query = "INSERT INTO NguyenLieu (TenNguyenLieu, SoLuongTon, DonViTinh, GhiChu) VALUES (@Ten, @SoLuong, @DonVi, @GhiChu)";
                    cmd = new SqlCommand(query, conn);
                }
                else
                {
                    string id = dgvNguyenLieu.CurrentRow.Cells["IDNguyenLieu"].Value.ToString();
                    string query = "UPDATE NguyenLieu SET TenNguyenLieu=@Ten, SoLuongTon=@SoLuong, DonViTinh=@DonVi, GhiChu=@GhiChu WHERE IDNguyenLieu=@id";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                }

                cmd.Parameters.AddWithValue("@Ten", txtTenNguyenLieu.Text);
                cmd.Parameters.AddWithValue("@SoLuong", int.Parse(txtSoLuongTon.Text));
                cmd.Parameters.AddWithValue("@DonVi", txtDonViTinh.Text);
                cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
                cmd.ExecuteNonQuery();
            }

            LoadData();
            ResetState();
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            ResetState();
            DgvNguyenLieu_SelectionChanged(null, null);
        }

        private void ClearInput()
        {
            txtTenNguyenLieu.Text = "";
            txtSoLuongTon.Text = "";
            txtDonViTinh.Text = "";
            txtGhiChu.Text = "";
        }

        private void SetButtonState()
        {
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void ResetState()
        {
            isAdding = false;
            isEditing = false;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenNguyenLieu.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nguyên liệu.");
                return false;
            }
            if (!int.TryParse(txtSoLuongTon.Text, out _))
            {
                MessageBox.Show("Số lượng tồn phải là số nguyên.");
                return false;
            }
            return true;
        }
        private void FormNguyenLieu_Load(object sender, EventArgs e)
        {
            // Nếu bạn muốn load lại dữ liệu từ đây:
            LoadData();
        }

    }
}
