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
using DAL;


namespace App
{
    public partial class frmXemPhieuNhapSach : BaseForm
    {
        private NhapSachDAL _nhapSachDAL = new NhapSachDAL();
        private BindingList<NhapSachDTO> _lichSuNhapGoc;
        
        public frmXemPhieuNhapSach()
        {
            InitializeComponent();
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            btnHuyTimKiem.Click += btnHuyTimKiem_Click;
            dtpNgayNhap.ValueChanged += dtpNgayNhap_ValueChanged;
        }
       
        private void frmXemPhieuNhapSach_Load(object sender, EventArgs e)
        {
            var list = _nhapSachDAL.LayLichSuNhap();
            _lichSuNhapGoc = new BindingList<NhapSachDTO>(list);
            dgvLichSuNhap.DataSource = _lichSuNhapGoc;
            foreach (DataGridViewColumn col in dgvLichSuNhap.Columns)
                col.Visible = false;
            dgvLichSuNhap.Columns["MaNhap"].Visible = true;
            dgvLichSuNhap.Columns["TenSach"].Visible = true;
            dgvLichSuNhap.Columns["NgayNhap"].Visible = true;
            dgvLichSuNhap.Columns["SoLuong"].Visible = true;
            dgvLichSuNhap.Columns["GiaNhap"].Visible = true;
            dgvLichSuNhap.Columns["TenNhanVien"].Visible = true;

            dgvLichSuNhap.Columns["MaNhap"].DisplayIndex = 0;
            dgvLichSuNhap.Columns["TenSach"].DisplayIndex = 1;
            dgvLichSuNhap.Columns["NgayNhap"].DisplayIndex = 2;
            dgvLichSuNhap.Columns["SoLuong"].DisplayIndex = 3;
            dgvLichSuNhap.Columns["GiaNhap"].DisplayIndex = 4;
            dgvLichSuNhap.Columns["TenNhanVien"].DisplayIndex = 5;
            dgvLichSuNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            var autoSource = new AutoCompleteStringCollection();
            autoSource.AddRange(list
                .SelectMany(x => new string[] {
                x.TenSach, x.MaNhap, x.SoLuong.ToString(), x.GiaNhap.ToString(), x.TenNhanVien
                })
                .Distinct()
                .ToArray());
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = autoSource;
            if(Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }
        }
       
        private void btnHuyTimKiem_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            dgvLichSuNhap.DataSource = _lichSuNhapGoc;
            
        }
        
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                dgvLichSuNhap.DataSource = _lichSuNhapGoc;
                return;
            }

            var filtered = _lichSuNhapGoc.Where(x =>
                (x.TenSach != null && x.TenSach.ToLower().Contains(keyword)) ||
                (x.MaNhap != null && x.MaNhap.ToLower().Contains(keyword)) ||
                x.SoLuong.ToString().Contains(keyword) ||
                x.GiaNhap.ToString().Contains(keyword) ||
                (x.TenNhanVien != null && x.TenNhanVien.ToLower().Contains(keyword))
            ).ToList();

            dgvLichSuNhap.DataSource = new BindingList<NhapSachDTO>(filtered);
        }
      
        private void dtpNgayNhap_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dtpNgayNhap.Value.Date;
            var filtered = _lichSuNhapGoc.Where(x => x.NgayNhap.Date == selectedDate).ToList();
            dgvLichSuNhap.DataSource = new BindingList<NhapSachDTO>(filtered);
        }
        
        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin admin = new frmAdmin();
            admin.Show();
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
            frmXemPhieuNhapSach nhapSach = new frmXemPhieuNhapSach();
            nhapSach.Show();
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

        private void mnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoan = new frmTaiKhoan();
            taiKhoan.Show();
            this.Hide();
        }

        private void mnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemHoaDon hoaDon = new frmXemHoaDon();
            hoaDon.Show();
            this.Hide();
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
            frmDoanhthu doanhthu = new frmDoanhthu();
            doanhthu.Show();
            this.Hide();
        }

        private void mnTenNhanVien_Click(object sender, EventArgs e)
        {

        }

        private void dgvLichSuNhap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}