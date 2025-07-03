using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace App
{
    public partial class frmAdmin : Form
    {
        private NhanVienDTO _nhanVienDTO = new NhanVienDTO();
        private bool isExiting = false; // ✅ Biến kiểm soát tránh lặp hộp thoại xác nhận

        public frmAdmin()
        {
            InitializeComponent();

            // ✅ Gắn sự kiện FormClosing
            this.FormClosing += frmAdmin_FormClosing;
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.LightSkyBlue;
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = SystemColors.Control;
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            btnSach.MouseEnter += Button_MouseEnter;
            btnSach.MouseLeave += Button_MouseLeave;
            btnNhap.MouseEnter += Button_MouseEnter;
            btnNhap.MouseLeave += Button_MouseLeave;
            btnNhanVien.MouseEnter += Button_MouseEnter;
            btnNhanVien.MouseLeave += Button_MouseLeave;
            btnTaiKhoan.MouseEnter += Button_MouseEnter;
            btnTaiKhoan.MouseLeave += Button_MouseLeave;
            btnDoanhthu.MouseEnter += Button_MouseEnter;
            btnDoanhthu.MouseLeave += Button_MouseLeave;
            btnHoaDon.MouseEnter += Button_MouseEnter;
            btnHoaDon.MouseLeave += Button_MouseLeave;

            if (Session.NhanVienHienTai != null)
            {
                mn1.Text = Session.NhanVienHienTai.TenNhanVien;
            }
        }

        private void btnSach_Click(object sender, EventArgs e)
        {
            frmSach sachForm = new frmSach();
            sachForm.Show();
            this.Hide();
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {
            frmXemPhieuNhapSach nhapSachForm = new frmXemPhieuNhapSach();
            nhapSachForm.Show();
            this.Hide();
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanVienForm = new frmNhanVien();
            nhanVienForm.Show();
            this.Hide();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoanForm = new frmTaiKhoan();
            taiKhoanForm.Show();
            this.Hide();
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemHoaDon xemHoaDonForm = new frmXemHoaDon();
            xemHoaDonForm.Show();
            this.Hide();
        }

        private void btnDoanhthu_Click(object sender, EventArgs e)
        {
            frmDoanhthu doanhthuForm = new frmDoanhthu();
            doanhthuForm.Show();
            this.Hide();
        }

        // ✅ Sửa sự kiện FormClosing
        private void frmAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Chỉ hiển thị xác nhận nếu người dùng bấm nút X (CloseReason.UserClosing)
            if (!isExiting && e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thoát chương trình?",
                    "Xác nhận thoát",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.OK)
                {
                    isExiting = true;
                    Application.ExitThread(); // ✅ Đóng toàn bộ chương trình ngay lập tức
                }
                else
                {
                    e.Cancel = true; // ❌ Huỷ nếu chọn Cancel
                }
            }
        }

        private void ptDangXuat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
               "Bạn có chắc chắn muốn đăng xuất không?",
               "Xác nhận đăng xuất",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                frmLogin frm = new frmLogin();
                frm.Show();
                Session.NhanVienHienTai = null;
            }
        }

        private void txtTenNhanVien_TextChanged(object sender, EventArgs e)
        {
        }

        private void Menu1(object sender, EventArgs e)
        {
        }

        private void mn1_Click(object sender, EventArgs e)
        {
        }

        private void đĂNGXUẤTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
            this.Hide();
        }
    }
}
