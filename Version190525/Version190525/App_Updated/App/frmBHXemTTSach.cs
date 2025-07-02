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
using BUS;

namespace App
{
    public partial class frmBHXemTTSach : BaseForm
    {
        private SachBUS sachBUS = new SachBUS();
        private List<SachDTO> danhSachSach = new List<SachDTO>();

        public frmBHXemTTSach()
        {
            InitializeComponent();
        }

        private void mnHoaDon_Click(object sender, EventArgs e)
        {
            frmHoaDon frm = new frmHoaDon();
            frm.Show();
            this.Hide();
        }

        private void frmThongTinSach_Load(object sender, EventArgs e)
        {
            LoadDanhSachSach();
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var autoComplete = new AutoCompleteStringCollection();
            foreach (var s in danhSachSach)
            {
                autoComplete.Add(s.TenSach);
                autoComplete.Add(s.TacGia);
                autoComplete.Add(s.TheLoai);
            }
            txtTimKiem.AutoCompleteCustomSource = autoComplete;
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }

        }

        private void LoadDanhSachSach()
        {
            var dt = sachBUS.LayDanhSachSach();
            danhSachSach = new List<SachDTO>();
            foreach (DataRow row in dt.Rows)
            {
                danhSachSach.Add(new SachDTO
                {
                    TenSach = row["TenSach"].ToString(),
                    SoLuong = row["SoLuong"] == DBNull.Value ? 0 : Convert.ToInt32(row["SoLuong"]),
                    GiaBan = row["GiaBan"] == DBNull.Value ? 0 : Convert.ToDecimal(row["GiaBan"]),
                    TacGia = row["TacGia"].ToString(),
                    TheLoai = row["TheLoai"].ToString()
                });
            }

            dgvSach.DataSource = null;
            dgvSach.DataSource = danhSachSach.Select(s => new
            {
                s.TenSach,
                s.SoLuong,
                GiaBan = s.GiaBan.ToString("N0"),
                s.TacGia,
                s.TheLoai
            }).ToList();
        }


        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
                TimKiemSach();
        }
        private void TimKiemSach()
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            var ketQua = danhSachSach.Where(s =>
                s.TenSach.ToLower().Contains(tuKhoa) ||
                s.TacGia.ToLower().Contains(tuKhoa) ||
                s.TheLoai.ToLower().Contains(tuKhoa) ||
                s.GiaBan.ToString("N0").Replace(",", "").Contains(tuKhoa.Replace(",", "")) ||
                s.SoLuong.ToString().Contains(tuKhoa)
            ).Select(s => new
            {
                s.TenSach,
                s.SoLuong,
                GiaBan = s.GiaBan.ToString("N0"),
                s.TacGia,
                s.TheLoai
            }).ToList();

            dgvSach.DataSource = null;
            dgvSach.DataSource = ketQua;
        }

        private void btnHuyTimKiem_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = string.Empty;
            LoadDanhSachSach();
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.Show();
            this.Hide();
        }

        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
