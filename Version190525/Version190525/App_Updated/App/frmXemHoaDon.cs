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

namespace App
{
    public partial class frmXemHoaDon : BaseForm
    {
        private HoaDonBUS hoaDonBUS = new HoaDonBUS();
        private BindingList<HoaDonDTO> _hoaDonGoc;


        public frmXemHoaDon()
        {
            InitializeComponent();
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            btnHuyTimKiem.Click += btnHuyTimKiem_Click;
            dtpNgayTao.ValueChanged += dtpNgayTao_ValueChanged;


        }

        private void drmXemHoaDon_Load(object sender, EventArgs e)
        {
            var list = hoaDonBUS.LayTatCaHoaDon();
            _hoaDonGoc = new BindingList<HoaDonDTO>(list);
            dgvLichSuHoaDon.DataSource = _hoaDonGoc;

            // Autocomplete cho tìm kiếm
            var autoSource = new AutoCompleteStringCollection();
            autoSource.AddRange(list
                .SelectMany(x => new string[] {
                x.MaHoaDon.ToString(),
                x.TongTien.ToString(),
                x.ThoiGianTao.ToString("dd/MM/yyyy"),
                x.TenNhanVien
                })
                .Distinct()
                .ToArray());
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = autoSource;

            dgvLichSuHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }

        }

        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin admin = new frmAdmin();
            admin.Show();
            this.Close();
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Close();
        }
        private void mnSach_Click(object sender, EventArgs e)
        {
            frmSach sachForm = new frmSach();
            sachForm.Show();
            this.Close();
        }

        private void mnNhapSach_Click(object sender, EventArgs e)
        {
            frmXemPhieuNhapSach nhapsachForm = new frmXemPhieuNhapSach();
            nhapsachForm.Show();
            this.Close();
        }

        private void mnNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanVienForm = new frmNhanVien();
            nhanVienForm.Show();
            this.Close();
        }

        private void mnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoanForm = new frmTaiKhoan();
            taiKhoanForm.Show();
            this.Close();
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
           frmDoanhthu doanhthuForm = new frmDoanhthu();
            doanhthuForm.Show();
            this.Close();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LocVaTimKiem();
        }

        private void btnHuyTimKiem_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            // Đặt lại ngày về giá trị mặc định (ví dụ: ngày hôm nay hoặc ngày nhỏ nhất trong danh sách)
            if (_hoaDonGoc != null && _hoaDonGoc.Count > 0)
            {
                // Nếu muốn trả về toàn bộ, chọn ngày nhỏ nhất trong danh sách
                var minDate = _hoaDonGoc.Min(x => x.ThoiGianTao).Date;
                dtpNgayTao.Value = minDate;
            }
            else
            {
                dtpNgayTao.Value = DateTime.Now;
            }
            // Gọi lại hàm lọc để hiển thị toàn bộ
            LocVaTimKiem();

        }
        private void LocVaTimKiem()
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            DateTime selectedDate = dtpNgayTao.Value.Date;
            DateTime minDate = _hoaDonGoc.Min(x => x.ThoiGianTao).Date;

            var filtered = _hoaDonGoc.Where(x =>
                (string.IsNullOrEmpty(keyword) ||
                    x.MaHoaDon.ToString().Contains(keyword) ||
                    x.TongTien.ToString().Contains(keyword) ||
                    x.ThoiGianTao.ToString("dd/MM/yyyy").Contains(keyword) ||
                    (x.TenNhanVien != null && x.TenNhanVien.ToLower().Contains(keyword))
                )
                && (selectedDate == minDate || x.ThoiGianTao.Date == selectedDate)
            ).ToList();

            dgvLichSuHoaDon.DataSource = new BindingList<HoaDonDTO>(filtered);
        }
        private void dtpNgayTao_ValueChanged(object sender, EventArgs e)
        {
            LocVaTimKiem();
        }

        private void dgvLichSuHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvLichSuHoaDon.Rows[e.RowIndex];
                int maHoaDon = Convert.ToInt32(row.Cells["MaHoaDon"].Value);
                frmChiTietHoaDon frm = new frmChiTietHoaDon(maHoaDon);
                frm.ShowDialog();
            }
        }
    }
}
