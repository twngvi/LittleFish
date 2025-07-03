using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class frmChiTietHoaDon : Form
    {
        private int _maHoaDon;
        private ChiTietHoaDonBUS chiTietHoaDonBUS = new ChiTietHoaDonBUS();
        private bool isExiting = false; // ✅ Biến kiểm soát xác nhận thoát
        public frmChiTietHoaDon(int maHoaDon)
        {
            InitializeComponent();
            _maHoaDon = maHoaDon;

            // ✅ Gắn sự kiện FormClosing khi khởi tạo
            this.FormClosing += frmChiTietHoaDon_FormClosing;
        }

        // ✅ Hàm xử lý xác nhận khi người dùng bấm nút X
        private void frmChiTietHoaDon_FormClosing(object sender, FormClosingEventArgs e)
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
                    Application.ExitThread(); // ✅ Thoát toàn bộ chương trình
                }
                else
                {
                    e.Cancel = true; // ❌ Huỷ nếu người dùng chọn Cancel
                }
            }
        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            var list = chiTietHoaDonBUS.LayChiTietHoaDonTheoMaHoaDon(_maHoaDon);
            dgvChiTietHoaDon.DataSource = null;
            dgvChiTietHoaDon.DataSource = list;
            dgvChiTietHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgvChiTietHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Không cần xử lý gì tại đây nếu không có nhu cầu
        }
    }
}