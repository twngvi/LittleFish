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


    public partial class frmNhapSach : BaseForm
    {
        private NhapSachBUS bus = new NhapSachBUS();
        private SachBUS sachBUS = new SachBUS();
        private List<NhapSachDTO> danhSachTam = new List<NhapSachDTO>();


        private NhanVienDTO _nhanVienDTO = new NhanVienDTO();
        public frmNhapSach()
        {
            InitializeComponent();
        }

        private void frmNhapSach_Load(object sender, EventArgs e)
        {
            // Lấy danh sách sách chưa ngừng kinh doanh
            var dsSach = sachBUS.LayDanhSachSach()
                .AsEnumerable()
                .Where(row => !row.IsNull("NgungKinhDoanh") && !Convert.ToBoolean(row["NgungKinhDoanh"]))
                .CopyToDataTable();

            cboSach.DataSource = dsSach;
            cboSach.DisplayMember = "TenSach";
            cboSach.ValueMember = "MaSach";

            // Enable AutoComplete for cboSach
            cboSach.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSach.AutoCompleteSource = AutoCompleteSource.ListItems;

            // Ensure no item is selected at startup
            cboSach.SelectedIndex = -1;

            dgvChiTietNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Hiển thị cảnh báo tồn kho (SL < 10)
            var dsCanhBao = dsSach.AsEnumerable()
                .Where(s => s.Field<int>("SoLuong") < 10)
                .Select(s => new
                {
                    MaSach = s.Field<int>("MaSach"),
                    TenSach = s.Field<string>("TenSach"),
                    SoLuong = s.Field<int>("SoLuong")
                })
                .ToList();
            dgvCanhBaoTonKho.DataSource = dsCanhBao;
            dgvCanhBaoTonKho.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }
        }


        private void cboSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSach.SelectedValue == null || cboSach.SelectedValue is DataRowView)
                return;

            string maSach = cboSach.SelectedValue.ToString();
            var sach = sachBUS.LaySachTheoMa(maSach);

            if (sach != null)
            {
                if (sach.GiaNhap > 0)
                {
                    txtGiaNhap.Text = sach.GiaNhap.ToString("N0");
                    txtGiaNhap.Enabled = false;
                    btnChinhSuaGia.Enabled = true;
                }
                else
                {
                    txtGiaNhap.Clear();
                    txtGiaNhap.Enabled = true;
                    btnChinhSuaGia.Enabled = false;
                }
            }
        }

        private void btnChinhSuaGia_Click(object sender, EventArgs e)
        {
            txtGiaNhap.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboSach.SelectedValue == null || txtSoLuong.Text == "" || txtGiaNhap.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Lấy tên sách từ combobox
            string tenSach = "";
            if (cboSach.SelectedItem is DataRowView drv)
            {
                tenSach = drv["TenSach"].ToString();
            }
            else if (cboSach.SelectedItem != null)
            {
                tenSach = cboSach.Text;
            }

            NhapSachDTO ns = new NhapSachDTO()
            {
                MaNhap = "NS" + DateTime.Now.Ticks.ToString(),
                MaSach = cboSach.SelectedValue.ToString(),
                SoLuong = int.Parse(txtSoLuong.Text),
                NgayNhap = DateTime.Now.Date, // chỉ lấy ngày
                GiaNhap = decimal.Parse(txtGiaNhap.Text),
                TenNhanVien = Session.NhanVienHienTai.TenNhanVien,
                MaNhanVien = Session.NhanVienHienTai.MaNhanVien.ToString(),
                TenSach = tenSach
            };
            danhSachTam.Add(ns);
            HienThiDanhSachTam();

            txtSoLuong.Clear();
            txtGiaNhap.Clear();
        }
        private void HienThiDanhSachTam()
        {
            dgvChiTietNhap.DataSource = null;
            dgvChiTietNhap.DataSource = danhSachTam;

            dgvChiTietNhap.Columns["MaNhanVien"].Visible = false;
            foreach (DataGridViewColumn col in dgvChiTietNhap.Columns)
            {
                if (col.Name != "TenSach" && col.Name != "SoLuong" && col.Name != "GiaNhap")
                {
                    col.Visible = false;
                }
            }
        }

        private void btnTaoPhieuNhap_Click(object sender, EventArgs e)
        {
            int thanhCong = 0;
            foreach (var ns in danhSachTam)
            {
                var giaHienTai = bus.LayGiaSach(ns.MaSach);
                if (!giaHienTai.HasValue || ns.GiaNhap != giaHienTai.Value)
                {
                    bus.CapNhatGiaSach(ns.MaSach, ns.GiaNhap);
                }

                if (bus.ThemNhapSach(ns))
                {
                    thanhCong++;
                }
            }
            MessageBox.Show($"Đã nhập thành công {thanhCong}/{danhSachTam.Count} dòng!");
            danhSachTam.Clear();
            HienThiDanhSachTam();
            cboSach.SelectedIndex = -1;
        }

        private void btnXoaDong_Click(object sender, EventArgs e)
        {
            if (dgvChiTietNhap.CurrentRow != null)
            {
                var maNhap = dgvChiTietNhap.CurrentRow.Cells["MaNhap"].Value.ToString();
                danhSachTam = danhSachTam.Where(x => x.MaNhap != maNhap).ToList();
                HienThiDanhSachTam();
            }
        }

        private void dgvChiTietNhap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.ShowDialog();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void mnLichSuNhap_Click(object sender, EventArgs e)
        {
           
        }

        private void mnSach_Click(object sender, EventArgs e)
        {
            frmKhoXemTTSach frm = new frmKhoXemTTSach();
            frm.Show();
            this.Hide();
        }

        private void mnPhieuNhap_Click(object sender, EventArgs e)
        {

        }
    }
}
