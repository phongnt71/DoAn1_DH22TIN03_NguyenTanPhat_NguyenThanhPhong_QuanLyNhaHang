﻿using System;
using System.Windows.Forms;
using System.IO;

namespace QuanLyNhaHang
{
    public partial class FormMain : Form
    {
        public string QuyenHienTai { get; set; } = "";

        // Biến lưu FormQuanLyBan hiện tại để truyền tham số cho FormDSHoaDon
        private FormQuanLyBan? formQuanLyBanInstance;

        public FormMain()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            string imagePath = Path.Combine(Application.StartupPath, "Images", "delicious-food-buffet-bottom-focus.jpg");

            foreach (Control ctl in this.Controls)
            {
                if (ctl is MdiClient client)
                {
                    client.BackgroundImage = Image.FromFile(imagePath);
                    client.BackgroundImageLayout = ImageLayout.Stretch; // 👈 giữ bố cục ban đầu
                    client.BackColor = Color.Black; // tránh nền trắng
                    break;
                }
            }
        }



        private void menuQuanLyBan_Click(object sender, EventArgs e)
        {
            OpenFormQuanLyBan();
        }

        private void OpenFormQuanLyBan()
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FormQuanLyBan)
                {
                    frm.Focus();
                    formQuanLyBanInstance = (FormQuanLyBan)frm;
                    return;
                }
            }

            formQuanLyBanInstance = new FormQuanLyBan();
            formQuanLyBanInstance.MdiParent = this;
            formQuanLyBanInstance.Show();
        }

        private void quảnLíBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is FormQuanLyBan)
                {
                    f.Activate();
                    formQuanLyBanInstance = (FormQuanLyBan)f;
                    return;
                }
            }

            formQuanLyBanInstance = new FormQuanLyBan();
            formQuanLyBanInstance.MdiParent = this;
            formQuanLyBanInstance.Show();
        }

        private void mónĂnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FormMonAn)
                {
                    frm.Activate();
                    return;
                }
            }
            FormMonAn formMonAn = new FormMonAn();
            formMonAn.MdiParent = this;
            formMonAn.Show();
        }

        private void tạoMớiHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FormTaoHoaDon)
                {
                    frm.Activate();
                    return;
                }
            }
            FormTaoHoaDon formTaoHoaDon = new FormTaoHoaDon();
            formTaoHoaDon.MdiParent = this;
            formTaoHoaDon.Show();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is FormKhachHang)
                {
                    child.Focus();
                    return;
                }
            }
            FormKhachHang formKH = new FormKhachHang();
            formKH.MdiParent = this;
            formKH.Show();
        }

        private void đặtBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FormDatBan)
                {
                    frm.Activate();
                    return;
                }
            }
            FormDatBan formDatBan = new FormDatBan();
            formDatBan.MdiParent = this;
            formDatBan.Show();
        }

        private void danhSáchHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FormDSHoaDon)
                {
                    frm.Activate();
                    return;
                }
            }

            // Tạo mới FormDSHoaDon và truyền formQuanLyBanInstance làm tham số
            FormDSHoaDon formDSHoaDon = new FormDSHoaDon(formQuanLyBanInstance ?? new FormQuanLyBan());
            formDSHoaDon.MdiParent = this;
            formDSHoaDon.Show();
        }

        private void báoCáoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FormBaoCao)
                {
                    frm.Activate(); // Kích hoạt form đã mở
                    return;         // Thoát không tạo form mới
                }
            }
            // Nếu chưa có form thì tạo mới
            FormBaoCao formBaoCao = new FormBaoCao();
            formBaoCao.MdiParent = this; // Gán form cha
            formBaoCao.Show();           // Hiển thị form
        }


        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (QuyenHienTai == "Admin" || QuyenHienTai == "QuanLy")
            {
                foreach (Form frm in this.MdiChildren)
                {
                    if (frm is FormNhanVien)
                    {
                        frm.Activate();
                        return;
                    }
                }
                FormNhanVien formNhanVien = new FormNhanVien();
                formNhanVien.MdiParent = this;
                formNhanVien.Show();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // 1. Ẩn FormMain
                this.Hide();

                // 2. Gọi lại form đăng nhập
                FormLogin loginForm = new FormLogin();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Nếu đăng nhập lại thành công → hiện lại FormMain
                    this.Show();
                }
                else
                {
                    // Nếu không → thoát hẳn ứng dụng
                    Application.Exit();
                }
            }
        }

        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FormTaiKhoan)
                {
                    frm.Activate(); // Nếu đã mở rồi thì focus lại
                    return;
                }
            }

            FormTaiKhoan formTaiKhoan = new FormTaiKhoan();
            formTaiKhoan.MdiParent = this; // Gán MDI cha là FormMain
            formTaiKhoan.Show(); // Mở form như MDI child
        }

        private void danhSáchNguyênLiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FormNguyenLieu)
                {
                    frm.Activate();
                    return;
                }
            }
            FormNguyenLieu formNguyenLieu = new FormNguyenLieu();
            formNguyenLieu.MdiParent = this;
            formNguyenLieu.Show();
        }

        private void nhậpNguyênLiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm is FormNhapNguyenLieu)
                {
                    frm.Activate();
                    return;
                }
            }
            FormNhapNguyenLieu formNhap = new FormNhapNguyenLieu();
            formNhap.MdiParent = this;
            formNhap.Show();
        }

    }
}
