using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class FormKhachHang : Form
    {
        private string connectionString = @"Data Source=DESKTOP-2024ZNN\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True;Trust Server Certificate=True";
        private bool isAdding = false;
        private bool isEditing = false;

        public int NewCustomerId { get; private set; } // Thêm property này để trả về ID khách hàng mới

        public FormKhachHang(string tenKhachHang = "")  // Thêm tham số mặc định rỗng để có thể gọi không tham số
        {
            InitializeComponent();
            LoadData();

            // Nếu có tên khách hàng truyền vào thì tự động điền vào textbox tên khách hàng
            if (!string.IsNullOrEmpty(tenKhachHang))
            {
                txtTenKH.Text = tenKhachHang;
                isAdding = true;  // Mặc định là thêm mới khi truyền tên khách hàng
                SetButtonState(false);  // Cho phép chỉnh sửa ngay
            }
            else
            {
                SetButtonState(true);
            }

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += BtnHuy_Click;
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT IDKhachHang, TenKH, SDT, DiaChi, TichDiem FROM KhachHang";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtgvKhachHang.DataSource = dt;
                dtgvKhachHang.Columns["TichDiem"].Visible = false;
            }
        }


        private void ClearInputs()
        {
            txtTenKH.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";
        }

        private void SetButtonState(bool defaultState)
        {
            btnThem.Enabled = defaultState;
            btnSua.Enabled = defaultState;
            btnXoa.Enabled = defaultState;
            btnLuu.Enabled = !defaultState;
            btnHuy.Enabled = !defaultState;

            txtTenKH.Enabled = !defaultState;
            txtSDT.Enabled = !defaultState;
            txtDiaChi.Enabled = !defaultState;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            isAdding = true;
            isEditing = false;
            ClearInputs();
            SetButtonState(false);
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dtgvKhachHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để sửa.");
                return;
            }

            isAdding = false;
            isEditing = true;
            SetButtonState(false);

            // Load dữ liệu vào input
            txtTenKH.Text = dtgvKhachHang.CurrentRow.Cells["TenKH"].Value.ToString();
            txtSDT.Text = dtgvKhachHang.CurrentRow.Cells["SDT"].Value.ToString();
            txtDiaChi.Text = dtgvKhachHang.CurrentRow.Cells["DiaChi"].Value.ToString();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dtgvKhachHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa.");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dtgvKhachHang.CurrentRow.Cells["IDKhachHang"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM KhachHang WHERE IDKhachHang = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }

                LoadData();
                ClearInputs();
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            string tenKH = txtTenKH.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            if (string.IsNullOrEmpty(tenKH))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                if (isAdding)
                {
                    cmd.CommandText = "INSERT INTO KhachHang (TenKH, SDT, DiaChi, TichDiem) VALUES (@TenKH, @SDT, @DiaChi, 0)";
                }
                else if (isEditing)
                {
                    int id = Convert.ToInt32(dtgvKhachHang.CurrentRow.Cells["IDKhachHang"].Value);
                    cmd.CommandText = "UPDATE KhachHang SET TenKH = @TenKH, SDT = @SDT, DiaChi = @DiaChi WHERE IDKhachHang = @ID";
                    cmd.Parameters.AddWithValue("@ID", id);
                }

                cmd.Parameters.AddWithValue("@TenKH", tenKH);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@DiaChi", diaChi);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Lưu khách hàng thành công!");
                    LoadData();
                    ClearInputs();
                    SetButtonState(true);
                    isAdding = false;
                    isEditing = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu khách hàng: " + ex.Message);
                }
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            ClearInputs();
            SetButtonState(true);
            isAdding = false;
            isEditing = false;
        }
        private void FormThemKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
            SetButtonState(true);
        }

    }
}
