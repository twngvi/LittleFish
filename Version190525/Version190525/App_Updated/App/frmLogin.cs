using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DTO;




namespace App
{
    public partial class frmLogin : BaseForm
    {
        private TaiKhoanBUS _taiKhoanBUS;
        private NhanVienBUS _nhanVienBUS = new NhanVienBUS();
        private NhanVienDTO _nhanVienDTO = new NhanVienDTO();
        public frmLogin()
        {
            InitializeComponent();
            _taiKhoanBUS = new TaiKhoanBUS();

        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            var user = _taiKhoanBUS.Login(username, password);

            if (user != null)
            {
                Session.TaiKhoanHienTai = user;

                if (int.TryParse(user.MaTaiKhoan, out int maTaiKhoan))
                {
                    Session.NhanVienHienTai = _nhanVienBUS.LayNhanVienTheoMaTaiKhoan(maTaiKhoan);
                }
                else
                {
                    MessageBox.Show("Lỗi: MaTaiKhoan không hợp lệ!");
                    return;
                }
                Form nextForm = null;

                if (user.VaiTro == "Admin")
                    nextForm = new frmAdmin();
                else if (user.VaiTro == "Kho")
                    nextForm = new frmNhapSach();
                else if (user.VaiTro == "BanHang")
                    nextForm = new frmHoaDon();

                if (nextForm != null)
                {
                    nextForm.Show();
                    this.Hide();
                }
            }
            else
            {
                lblError.Text = "Sai tên đăng nhập hoặc mật khẩu!";
            }
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            ptbAnh.SizeMode = PictureBoxSizeMode.StretchImage;
            txtUsername.KeyDown += txtUsername_KeyDown;
            txtPassword.KeyDown += txtPassword_KeyDown;
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void ptbAnh_Click(object sender, EventArgs e)
        {

        }
    }
}