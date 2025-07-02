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
    public partial class frmTaiKhoan : BaseForm
    {
        TaiKhoanBUS bus = new TaiKhoanBUS();
        public string LoggedInUser { get; set; }

        private void frmTaiKhoan_Load(object sender, EventArgs e)
        {
            dgvTaiKhoan.DataSource = bus.LayTatCaTaiKhoan();
            dgvTaiKhoan.Columns["KichHoat"].Visible = false; 
            dgvTaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if(Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }

        }
        public frmTaiKhoan()
        {
            InitializeComponent();
           
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                TaiKhoanDTO tk = new TaiKhoanDTO(
                    txtMa.Text,
                    txtTen.Text,
                    txtMatKhau.Text,
                    cboVaiTro.SelectedItem.ToString(),
                    chkKichHoat.Checked
                );

                if (bus.ThemTaiKhoan(tk))
                {
                    MessageBox.Show("Thêm thành công");
                    dgvTaiKhoan.DataSource = bus.LayTatCaTaiKhoan();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO tk = new TaiKhoanDTO(
           txtMa.Text,
           txtTen.Text,
           txtMatKhau.Text,
           cboVaiTro.SelectedItem.ToString(),
           chkKichHoat.Checked
            );
            if (bus.CapNhatTaiKhoan(tk))
            {
                MessageBox.Show("Cập nhật thành công");
                dgvTaiKhoan.DataSource = bus.LayTatCaTaiKhoan();
            }
            else
                MessageBox.Show("Cập nhật thất bại");
        }

        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTaiKhoan.Rows[e.RowIndex];

              
                txtMa.Text = row.Cells["MaTaiKhoan"].Value?.ToString();
                txtTen.Text = row.Cells["TenDangNhap"].Value?.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value?.ToString();
                cboVaiTro.SelectedItem = row.Cells["VaiTro"].Value?.ToString();
                chkKichHoat.Checked = row.Cells["KichHoat"].Value != null && (bool)row.Cells["KichHoat"].Value;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMa.Text))
            {
                MessageBox.Show("Vui lòng chọn tài khoản để xóa.");
                return;
            }

            // Prevent deletion of the currently logged-in account
            if (txtTen.Text == LoggedInUser)
            {
                MessageBox.Show("Bạn không thể xóa tài khoản đang đăng nhập.");
                return;
            }

            // Prevent Admin from deleting their own account
            if (cboVaiTro.SelectedItem?.ToString() == "Admin" && txtTen.Text == LoggedInUser)
            {
                MessageBox.Show("Admin không thể tự xóa chính mình.");
                return;
            }

            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?",
                                                "Xác nhận xóa",
                                                MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (bus.XoaTaiKhoan(txtMa.Text))
                {
                    MessageBox.Show("Xóa thành công.");
                    dgvTaiKhoan.DataSource = bus.LayTatCaTaiKhoan(); // Refresh the DataGridView
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.");
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = txtSearchName.Text.Trim().ToLower();
            string searchRole = cboSearchRole.SelectedItem?.ToString();
            List<TaiKhoanDTO> filteredAccounts = bus.TimKiemTaiKhoan(searchName, searchRole);
            dgvTaiKhoan.DataSource = filteredAccounts;
        }

        private void CancelSearch_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = string.Empty;
            cboSearchRole.SelectedIndex = -1;
            dgvTaiKhoan.DataSource = bus.LayTatCaTaiKhoan();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtMa.Text = string.Empty;
            txtTen.Text = string.Empty;
            txtMatKhau.Text = string.Empty;
            cboVaiTro.SelectedIndex = -1;
            chkKichHoat.Checked = false;
        }

        private void menuSach_Click(object sender, EventArgs e)
        {
            frmSach sach = new frmSach();
            sach.Show();
        }

        private void menuNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanVien = new frmNhanVien();
            nhanVien.Show();
        }

        private void menuTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoan = new frmTaiKhoan();
            taiKhoan.Show();
            this.Hide();
        }

        private void mnTenNhanVien_Click(object sender, EventArgs e)
        {
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }

        }

        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin home = new frmAdmin();
            home.Show();
            this.Hide();
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void mnNhapSach_Click(object sender, EventArgs e)
        {
            frmNhapSach nhapsach = new frmNhapSach();
            nhapsach.Show();
            this.Hide();
        }

        private void mnSach_Click(object sender, EventArgs e)
        {
            frmSach sach = new frmSach();
            sach.Show();
            this.Hide();
        }

        private void mnNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanVien = new frmNhanVien();
            nhanVien.Show();
            this.Hide();
        }

        private void mnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemHoaDon xemhoadon = new frmXemHoaDon();
            xemhoadon.Show();
            this.Hide();
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
            frmDoanhthu doanhthu = new frmDoanhthu();
            doanhthu.Show();
            this.Hide();
        }

        private void dgvTaiKhoan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
