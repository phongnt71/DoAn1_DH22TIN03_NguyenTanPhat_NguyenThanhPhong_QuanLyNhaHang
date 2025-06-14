using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyNhaHang
{
    public partial class FormMonAn : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;Trust Server Certificate=True";
        private DataTable dtMonAn;

        private bool isAdding = false;
        private bool isEditing = false;

        public FormMonAn()
        {
            InitializeComponent();
            LoadLoaiMon();
            cmbLoaiMon.DropDownStyle = ComboBoxStyle.DropDownList;
            InitControls();
            LoadData();
            dtgvMonAn.SelectionChanged += dtgvMonAn_SelectionChanged;
            LoadDanhSachNguyenLieu();
            dtgvNguyenLieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvMonAn.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadDanhSachNguyenLieu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT IDNguyenLieu, TenNguyenLieu FROM NguyenLieu";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbNguyenLieu.DataSource = dt;
                cmbNguyenLieu.DisplayMember = "TenNguyenLieu"; // hiển thị tên nguyên liệu
                cmbNguyenLieu.ValueMember = "IDNguyenLieu";   // giá trị là ID nguyên liệu
                cmbNguyenLieu.SelectedIndex = -1; // chưa chọn gì ban đầu
            }
        }

        private void LoadNguyenLieu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT IDNguyenLieu, TenNguyenLieu, SoLuongTon, DonViTinh FROM NguyenLieu";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtgvNguyenLieu.DataSource = dt;
                dtgvNguyenLieu.Columns["IDNguyenLieu"].HeaderText = "Mã nguyên liệu";
                dtgvNguyenLieu.Columns["TenNguyenLieu"].HeaderText = "Tên nguyên liệu";
                dtgvNguyenLieu.Columns["SoLuongTon"].HeaderText = "Số lượng";
                dtgvNguyenLieu.Columns["DonViTinh"].HeaderText = "Đơn vị tính";
            }
        }

        private void LoadNguyenLieuTheoMonAn(int idMonAn)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT c.IDNguyenLieu, n.TenNguyenLieu, c.SoLuongDung, n.DonViTinh
                    FROM ChiTietNguyenLieuMonAn c
                    JOIN NguyenLieu n ON c.IDNguyenLieu = n.IDNguyenLieu
                    WHERE c.IDMonAn = @IDMonAn";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@IDMonAn", idMonAn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dtgvNguyenLieu.DataSource = dt;
                dtgvNguyenLieu.Columns["IDNguyenLieu"].HeaderText = "Mã nguyên liệu";
                dtgvNguyenLieu.Columns["TenNguyenLieu"].HeaderText = "Tên nguyên liệu";
                dtgvNguyenLieu.Columns["SoLuongDung"].HeaderText = "Số lượng";
                dtgvNguyenLieu.Columns["DonViTinh"].HeaderText = "Đơn vị tính";
            }
        }

        private void dtgvMonAn_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgvMonAn.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dtgvMonAn.SelectedRows[0];

                txtTenMon.Text = row.Cells["TenMon"].Value?.ToString();
                cmbLoaiMon.SelectedItem = row.Cells["LoaiMon"].Value?.ToString();
                txtGiaTien.Text = row.Cells["GiaTien"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();

                object value = row.Cells["IDMonAn"].Value;
                if (value != DBNull.Value && value != null && int.TryParse(value.ToString(), out int idMonAn))
                {
                    LoadNguyenLieuTheoMonAn(idMonAn);
                }
                else
                {
                    ClearInputFields();
                    dtgvNguyenLieu.DataSource = null;
                }
            }
            else
            {
                ClearInputFields();
                dtgvNguyenLieu.DataSource = null;
            }
        }


        private void InitControls()
        {
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += BtnHuy_Click;

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }

        private void LoadLoaiMon()
        {
            cmbLoaiMon.Items.Clear();
            cmbLoaiMon.Items.Add("Món chính");
            cmbLoaiMon.Items.Add("Nước");
            cmbLoaiMon.Items.Add("Món phụ");
            cmbLoaiMon.Items.Add("Tráng miệng");
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT IDMonAn, TenMon, LoaiMon, GiaTien, MoTa FROM dbo.MonAn";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    dtMonAn = new DataTable();
                    da.Fill(dtMonAn);
                    dtgvMonAn.DataSource = dtMonAn;

                    dtgvMonAn.Columns["IDMonAn"].HeaderText = "Mã Món";
                    dtgvMonAn.Columns["TenMon"].HeaderText = "Tên Món";
                    dtgvMonAn.Columns["LoaiMon"].HeaderText = "Loại Món";
                    dtgvMonAn.Columns["GiaTien"].HeaderText = "Giá Tiền";
                    dtgvMonAn.Columns["MoTa"].HeaderText = "Mô Tả";

                    ClearInputFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message);
            }
        }

        private void ClearInputFields()
        {
            txtTenMon.Text = "";
            cmbLoaiMon.SelectedIndex = -1;
            txtGiaTien.Text = "";
            txtMoTa.Text = "";
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            ClearInputFields();
            isAdding = true;
            isEditing = false;

            btnLuu.Enabled = true;
            btnHuy.Enabled = true;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            txtTenMon.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dtgvMonAn.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món ăn cần sửa.");
                return;
            }

            isAdding = false;
            isEditing = true;

            btnLuu.Enabled = true;
            btnHuy.Enabled = true;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            txtTenMon.Focus();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dtgvMonAn.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món ăn cần xóa.");
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa món ăn này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string idMonAn = dtgvMonAn.SelectedRows[0].Cells["IDMonAn"].Value.ToString();
                DeleteMonAn(idMonAn);
                LoadData();
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            if (isAdding)
            {
                InsertMonAn();
            }
            else if (isEditing)
            {
                UpdateMonAn();
            }

            LoadData();
            ResetButtons();
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            ResetButtons();
            if (dtgvMonAn.SelectedRows.Count > 0)
            {
                dtgvMonAn_SelectionChanged(null, null);
            }
            else
            {
                ClearInputFields();
            }
        }

        private void ResetButtons()
        {
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;

            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;

            isAdding = false;
            isEditing = false;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Tên món không được để trống.");
                txtTenMon.Focus();
                return false;
            }
            if (cmbLoaiMon.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại món.");
                cmbLoaiMon.Focus();
                return false;
            }
            if (!decimal.TryParse(txtGiaTien.Text, out _))
            {
                MessageBox.Show("Giá tiền phải là số hợp lệ.");
                txtGiaTien.Focus();
                return false;
            }
            return true;
        }

        private void InsertMonAn()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO MonAn (TenMon, LoaiMon, GiaTien, MoTa) VALUES (@TenMon, @LoaiMon, @GiaTien, @MoTa)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenMon", txtTenMon.Text);
                    cmd.Parameters.AddWithValue("@LoaiMon", cmbLoaiMon.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@GiaTien", decimal.Parse(txtGiaTien.Text));
                    cmd.Parameters.AddWithValue("@MoTa", txtMoTa.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm món ăn thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm món ăn: " + ex.Message);
            }
        }

        private void UpdateMonAn()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string idMonAn = dtgvMonAn.SelectedRows[0].Cells["IDMonAn"].Value.ToString();

                    string query = "UPDATE MonAn SET TenMon = @TenMon, LoaiMon = @LoaiMon, GiaTien = @GiaTien, MoTa = @MoTa WHERE IDMonAn = @IDMonAn";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenMon", txtTenMon.Text);
                    cmd.Parameters.AddWithValue("@LoaiMon", cmbLoaiMon.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@GiaTien", decimal.Parse(txtGiaTien.Text));
                    cmd.Parameters.AddWithValue("@MoTa", txtMoTa.Text);
                    cmd.Parameters.AddWithValue("@IDMonAn", idMonAn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật món ăn thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật món ăn: " + ex.Message);
            }
        }

        private void DeleteMonAn(string idMonAn)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM MonAn WHERE IDMonAn = @IDMonAn";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IDMonAn", idMonAn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa món ăn thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa món ăn: " + ex.Message);
            }
        }

        private void btnThemNguyenLieu_Click(object sender, EventArgs e)
        {
            if (dtgvMonAn.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món ăn trước.");
                return;
            }

            int idMonAn = Convert.ToInt32(dtgvMonAn.SelectedRows[0].Cells["IDMonAn"].Value);
            int idNguyenLieu = int.Parse(cmbNguyenLieu.SelectedValue.ToString());
            decimal soLuongTon = nudSoLuongTon.Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Kiểm tra nguyên liệu đã tồn tại trong món ăn chưa
                string checkQuery = "SELECT COUNT(*) FROM ChiTietNguyenLieuMonAn WHERE IDMonAn = @IDMonAn AND IDNguyenLieu = @IDNguyenLieu";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@IDMonAn", idMonAn);
                checkCmd.Parameters.AddWithValue("@IDNguyenLieu", idNguyenLieu);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    // Nếu đã tồn tại thì cập nhật số lượng
                    string updateQuery = "UPDATE ChiTietNguyenLieuMonAn SET SoLuongDung = SoLuongDung + @SoLuongDung WHERE IDMonAn = @IDMonAn AND IDNguyenLieu = @IDNguyenLieu";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@SoLuongDung", soLuongTon);
                    updateCmd.Parameters.AddWithValue("@IDMonAn", idMonAn);
                    updateCmd.Parameters.AddWithValue("@IDNguyenLieu", idNguyenLieu);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    // Nếu chưa tồn tại thì chèn mới
                    string insertQuery = "INSERT INTO ChiTietNguyenLieuMonAn (IDMonAn, IDNguyenLieu, SoLuongDung) VALUES (@IDMonAn, @IDNguyenLieu, @SoLuongDung)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@IDMonAn", idMonAn);
                    insertCmd.Parameters.AddWithValue("@IDNguyenLieu", idNguyenLieu);
                    insertCmd.Parameters.AddWithValue("@SoLuongDung", soLuongTon);
                    insertCmd.ExecuteNonQuery();
                }
            }

            LoadNguyenLieuTheoMonAn(idMonAn);
        }

        private void btnXoaNguyenLieu_Click(object sender, EventArgs e)
        {
            if (dtgvNguyenLieu.SelectedRows.Count == 0 || dtgvMonAn.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu và món ăn để xóa.");
                return;
            }

            int idMonAn = Convert.ToInt32(dtgvMonAn.SelectedRows[0].Cells["IDMonAn"].Value);
            int idNguyenLieu = Convert.ToInt32(dtgvNguyenLieu.SelectedRows[0].Cells["IDNguyenLieu"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM ChiTietNguyenLieuMonAn WHERE IDMonAn = @IDMonAn AND IDNguyenLieu = @IDNguyenLieu";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDMonAn", idMonAn);
                cmd.Parameters.AddWithValue("@IDNguyenLieu", idNguyenLieu);
                cmd.ExecuteNonQuery();
            }

            LoadNguyenLieuTheoMonAn(idMonAn);
        }
        private void FormMonAn_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cmbLoaiMon_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void txtGiaTien_TextChanged(object sender, EventArgs e)
        {
            
        }

    }
}
