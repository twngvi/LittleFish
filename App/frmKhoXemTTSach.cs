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
    public partial class frmKhoXemTTSach : Form
    {
        private SachBUS sachBUS = new SachBUS();
        private NhanVienDTO _nhanVienDTO = new NhanVienDTO();
        private bool isExiting = false; // ✅ Biến kiểm soát xác nhận thoát

        public frmKhoXemTTSach()
        {
            InitializeComponent();
            this.Load += frmKhoXemTTSach_Load;
            txtSearch.TextChanged += txtSearch_TextChanged;
            btnCancelSearch.Click += btnCancelSearch_Click;
            this.FormClosing += frmKhoXemTTSach_FormClosing; // ✅ Gắn sự kiện FormClosing
        }

        private void frmKhoXemTTSach_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isExiting && e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có muốn thoát chương trình?",
                    "Xác nhận",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    isExiting = true;
                    Application.ExitThread();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void frmKhoXemTTSach_Load(object sender, EventArgs e)
        {
            var dsSach = sachBUS.LayDanhSachSach();

            if (dsSach is DataTable dt)
            {
                var dtView = dt.DefaultView.ToTable(false, "MaSach", "TenSach", "SoLuong", "GiaNhap");
                dgvSach.DataSource = dtView;

                AutoCompleteStringCollection autoSource = new AutoCompleteStringCollection();
                foreach (DataRow row in dt.Rows)
                {
                    autoSource.Add(row["MaSach"].ToString());
                    autoSource.Add(row["TenSach"].ToString());
                }

                txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtSearch.AutoCompleteCustomSource = autoSource;
            }
            else if (dsSach is IEnumerable<SachDTO> list)
            {
                dgvSach.DataSource = null;
                dgvSach.DataSource = list.ToList();

                AutoCompleteStringCollection autoSource = new AutoCompleteStringCollection();
                foreach (var sach in list)
                {
                    autoSource.Add(sach.MaSach);
                    autoSource.Add(sach.TenSach);
                }

                txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtSearch.AutoCompleteCustomSource = autoSource;
            }

            dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSach.Columns["MaSach"].HeaderText = "Mã sách";
            dgvSach.Columns["TenSach"].HeaderText = "Tên sách";
            dgvSach.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvSach.Columns["GiaNhap"].HeaderText = "Giá nhập";

            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (dgvSach.DataSource is DataTable dt)
            {
                (dgvSach.DataSource as DataTable).DefaultView.RowFilter =
                    $"CONVERT(MaSach, 'System.String') LIKE '%{keyword}%' OR TenSach LIKE '%{keyword}%'";
            }
            else if (dgvSach.DataSource is List<SachDTO> list)
            {
                var filtered = list.Where(s =>
                    s.MaSach.ToLower().Contains(keyword) ||
                    s.TenSach.ToLower().Contains(keyword)).ToList();
                dgvSach.DataSource = null;
                dgvSach.DataSource = filtered;
            }
        }

        private void btnCancelSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            frmKhoXemTTSach_Load(null, null);
        }

        private void mnPhieuNhap_Click(object sender, EventArgs e)
        {
            frmNhapSach frmNhapSach = new frmNhapSach();
            frmNhapSach.Show();
            this.Hide();
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
            this.Hide();
        }

        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
