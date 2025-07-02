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
    public partial class frmQuickAddAccount : Form
    {
        private TaiKhoanBUS taiKhoanBUS;
        public frmQuickAddAccount()
        {
            InitializeComponent();
            taiKhoanBUS = new TaiKhoanBUS();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO newAccount = new TaiKhoanDTO
            {
                TenDangNhap = txtTenDangNhap.Text.Trim(),
                MatKhau = txtMatKhau.Text.Trim(),
                VaiTro = cboVaiTro.Text,
                KichHoat = chkKichHoat.Checked
            };

            // Gọi BUS để thêm tài khoản  
            if (taiKhoanBUS.ThemTaiKhoan(newAccount))
            {
                MessageBox.Show("Thêm tài khoản thành công!");
                this.DialogResult = DialogResult.OK; // Đóng form và trả về kết quả OK  
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại!");
            }
        }

        private void cboVaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmQuickAddAccount_Load(object sender, EventArgs e)
        {
            cboVaiTro.Items.Add("Admin");
            cboVaiTro.Items.Add("Kho");
            cboVaiTro.Items.Add("BanHang");
            cboVaiTro.SelectedIndex = 0;
        }
    }
}
