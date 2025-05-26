using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaHang
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

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
                    return;
                }
            }

            FormQuanLyBan formBan = new FormQuanLyBan();
            formBan.MdiParent = this;  // Nếu FormMain là MDI container
            formBan.Show();
        }

        private void quảnLíBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Mở form quản lý bàn ăn
            foreach (Form f in this.MdiChildren)
            {
                if (f is FormQuanLyBan)
                {
                    f.Activate();
                    return;
                }
            }

            FormQuanLyBan formQuanLyBan = new FormQuanLyBan();
            formQuanLyBan.MdiParent = this;
            formQuanLyBan.Show();
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

        private void nguyenLieuToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void tạoMớiHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTaoHoaDon formTaoHoaDon = new FormTaoHoaDon();
            formTaoHoaDon.MdiParent = this; // nếu FormMain là MDI container
            formTaoHoaDon.Show();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu form quản lí khách hàng đã mở rồi thì focus lên đó, không tạo mới
            foreach (Form child in this.MdiChildren)
            {
                if (child is FormKhachHang)
                {
                    child.Focus();
                    return;
                }
            }

            // Tạo mới form quản lí khách hàng và đặt cha là FormMain (MDI Parent)
            FormKhachHang formKH = new FormKhachHang();
            formKH.MdiParent = this;
            formKH.Show();
        }


    }
}
