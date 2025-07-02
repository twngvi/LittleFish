using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BUS;
using DTO;

namespace App
{
    public partial class frmDoanhthu : BaseForm
    {
        public frmDoanhthu()
        {
            InitializeComponent();
        }

        private void frmDoanhthu_Load(object sender, EventArgs e)
        {
            LoadThongKe();
            AutoResizeListViewColumns(lvTopBanChay);
            AutoResizeListViewColumns(lvTopBanIt);

            cbThang.Items.Clear();
            cbThang.Items.Add("Tất cả");
            for (int i = 1; i <= 12; i++)
                cbThang.Items.Add("Tháng " + i);
            cbThang.SelectedIndex = 0;
            // Xóa cột cũ nếu có
            lvTopBanChay.Columns.Clear();
            lvTopBanIt.Columns.Clear();

            // Thêm đủ 4 cột
            lvTopBanChay.Columns.Add("Mã sách");
            lvTopBanChay.Columns.Add("Tên sách");
            lvTopBanChay.Columns.Add("SL bán");
            lvTopBanChay.Columns.Add("Tổng tiền");

            lvTopBanIt.Columns.Add("Mã sách");
            lvTopBanIt.Columns.Add("Tên sách");
            lvTopBanIt.Columns.Add("SL bán");
            lvTopBanIt.Columns.Add("Tổng tiền");
            // Tự động điều chỉnh kích thước cột
            AutoResizeListViewColumns(lvTopBanChay);
            AutoResizeListViewColumns(lvTopBanIt);
            VeBieuDoDoanhThu();

            if(Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }   

        }

        private void AutoResizeListViewColumns(ListView listView)
        {
            if (listView.Columns.Count == 0) return;
            int totalWidth = listView.ClientSize.Width;
            int colWidth = totalWidth / listView.Columns.Count;
            for (int i = 0; i < listView.Columns.Count; i++)
            {
                if (i == listView.Columns.Count - 1)
                    listView.Columns[i].Width = totalWidth - colWidth * i;
                else
                    listView.Columns[i].Width = colWidth;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AutoResizeListViewColumns(lvTopBanChay);
            AutoResizeListViewColumns(lvTopBanIt);
        }

        private void LoadThongKe(DateTime? from = null, DateTime? to = null)
        {
            DateTime fromDate, toDate;

            if (from == null && to == null)
            {
                // Lấy toàn bộ dữ liệu từ đầu năm đến cuối năm hiện tại
                int year = DateTime.Today.Year;
                fromDate = new DateTime(year, 1, 1);
                toDate = new DateTime(year, 12, 31, 23, 59, 59, 999);
            }
            else if (from != null && to != null)
            {
                fromDate = from.Value;
                toDate = to.Value;
            }
            else
            {
                // fallback: lấy tháng hiện tại
                int month = from?.Month ?? DateTime.Today.Month;
                int year = from?.Year ?? DateTime.Today.Year;
                fromDate = new DateTime(year, month, 1);
                toDate = fromDate.AddMonths(1).AddTicks(-1);
            }

            var thongKe = SachBUS.ThongKeDoanhThu(fromDate, toDate);

            txtDoanhThu.Text = thongKe.DoanhThu.ToString("N0", CultureInfo.InvariantCulture);
            txtLoiNhuan.Text = thongKe.LoiNhuan.ToString("N0", CultureInfo.InvariantCulture);
            txtSoHoaDon.Text = thongKe.SoHoaDon.ToString();
            txtSoSachBan.Text = thongKe.SoSachBan.ToString();
            foreach (var sach in thongKe.TopBanChay.Take(10))
            {
                var item = new ListViewItem(new string[] {
                sach.MaSach,
                sach.TenSach,
                sach.SoLuongBan.ToString(),
                sach.TongTienBan.ToString("N0", CultureInfo.InvariantCulture)
                });
                lvTopBanChay.Items.Add(item);
            }

            foreach (var sach in thongKe.TopBanIt.Take(10))
            {
                var item = new ListViewItem(new string[] {
                sach.MaSach,
                sach.TenSach,
                sach.SoLuongBan.ToString(),
                sach.TongTienBan.ToString("N0", CultureInfo.InvariantCulture)
                });
                lvTopBanIt.Items.Add(item);
            }

        }

        private void btnHuyLoc_Click(object sender, EventArgs e)
        {
            dtpNgay.Value = DateTime.Today;
            cbThang.SelectedIndex = 0;
            LoadThongKe();
        }

        private void cbThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = DateTime.Today.Year;
            if (cbThang.SelectedIndex <= 0)
            {
                LoadThongKe(new DateTime(year, 1, 1), new DateTime(year, 12, 31));
                return;
            }
            int month = cbThang.SelectedIndex;
            DateTime from = new DateTime(year, month, 1);
            DateTime to = from.AddMonths(1).AddTicks(-1);
            LoadThongKe(from, to);
        }


        private void btnLoc3Ngay_Click(object sender, EventArgs e)
        {
            DateTime to = DateTime.Today.AddDays(1).AddTicks(-1);
            DateTime from = DateTime.Today.AddDays(-2);
            LoadThongKe(from, to);
        }

        private void btnLoc7Ngay_Click(object sender, EventArgs e)
        {
            DateTime to = DateTime.Today.AddDays(1).AddTicks(-1);
            DateTime from = DateTime.Today.AddDays(-6);
            LoadThongKe(from, to);
        }

        private void VeBieuDoDoanhThu(DateTime? from = null, DateTime? to = null)
        {
            if (from == null || to == null)
            {
                int year = DateTime.Today.Year;
                from = new DateTime(year, 1, 1);
                to = new DateTime(year, 12, 31, 23, 59, 59, 999);
            }

            var hoaDonDal = new DAL.HoaDonDAL();
            var hoaDons = hoaDonDal.LayTatCaHoaDon()
                .Where(hd => hd.ThoiGianTao >= from && hd.ThoiGianTao <= to)
                .ToList();
            var doanhThuTheoThang = hoaDons
                .GroupBy(hd => new { hd.ThoiGianTao.Year, hd.ThoiGianTao.Month })
                .Select(g => new
                {
                    Thang = $"Tháng {g.Key.Month}/{g.Key.Year}",
                    TongTien = g.Sum(x => x.TongTien)
                })
                .OrderBy(x => x.Thang)
                .ToList();
            chartDoanhThu.Series.Clear();
            chartDoanhThu.ChartAreas.Clear();
            chartDoanhThu.ChartAreas.Add(new ChartArea("MainArea"));

            chartDoanhThu.Legends.Clear();
            chartDoanhThu.Legends.Add(new Legend("Default")); // Đặt tên legend là "Default"

            var seriesThang = new Series("Doanh thu");
            seriesThang.ChartType = SeriesChartType.Column;
            seriesThang.Legend = "Default"; // Gán legend cho series

            foreach (var item in doanhThuTheoThang)
            {
                seriesThang.Points.AddXY(item.Thang, item.TongTien);
            }

            chartDoanhThu.Series.Add(seriesThang);
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
            frmDoanhthu doanhthu = new frmDoanhthu();
            doanhthu.Show();
            this.Hide();
        }

        private void mnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemHoaDon xemHoaDon = new frmXemHoaDon();
            xemHoaDon.Show();
            this.Hide();
        }

        private void mnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoan = new frmTaiKhoan();
            taiKhoan.Show();
            this.Hide();
        }

        private void mnNhanVien_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoan = new frmTaiKhoan();
            taiKhoan.Show();
            this.Hide();
        }

        private void mnNhapSach_Click(object sender, EventArgs e)
        {
            frmXemPhieuNhapSach xemPhieuNhapSach = new frmXemPhieuNhapSach();
            xemPhieuNhapSach.Show();
            this.Hide();
        }

        private void mnSach_Click(object sender, EventArgs e)
        {
            frmSach sach = new frmSach();
            sach.Show();
            this.Hide();
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

        private void lvTopBanIt_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpNgay_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lvTopBanChay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chartDoanhThu_Click(object sender, EventArgs e)
        {

        }
    }
}
