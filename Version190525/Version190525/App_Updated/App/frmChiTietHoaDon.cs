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
    public partial class frmChiTietHoaDon : BaseForm
    {
        private int _maHoaDon;
        private ChiTietHoaDonBUS chiTietHoaDonBUS = new ChiTietHoaDonBUS();

        public frmChiTietHoaDon(int maHoaDon)
        {
            InitializeComponent();
            _maHoaDon = maHoaDon;
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

        }
    }
}
