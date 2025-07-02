using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using Microsoft.VisualBasic;
using BUS;
using System.Configuration;
using DAL;
namespace App
{
    public partial class frmSach : BaseForm
    {
        SachBUS sachBUS = new SachBUS();
        string ConnectionString = @"Data Source=DESKTOP-303SSFE;Initial Catalog=NhaSachDB;Integrated Security=True";

        public frmSach()
        {
            InitializeComponent();
            this.txtTuKhoa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTuKhoa_KeyDown);
        }

        private void frmSach_Load(object sender, EventArgs e)
        {
            // Lấy danh sách sách từ BUS (DataTable)
            DataTable dt = sachBUS.LayDanhSachSach();

            dgvSach.DataSource = dt;

            var dsSach = sachBUS.LayDanhSachSach()
            .AsEnumerable()
            .Where(row => !row.IsNull("NgungKinhDoanh") && !Convert.ToBoolean(row["NgungKinhDoanh"]))
            .CopyToDataTable();


            // Hiển thị các cột cần thiết
            foreach (DataGridViewColumn column in dgvSach.Columns)
            {
                column.Visible = false;
            }
            if (dgvSach.Columns.Contains("MaSach")) dgvSach.Columns["MaSach"].Visible = true;
            if (dgvSach.Columns.Contains("TenSach")) dgvSach.Columns["TenSach"].Visible = true;

            // Thêm cột trạng thái nếu chưa có
            if (!dgvSach.Columns.Contains("TrangThai"))
            {
                dgvSach.Columns.Add("TrangThai", "Trạng Thái");
            }

            // Cập nhật trạng thái cho từng dòng
            foreach (DataGridViewRow row in dgvSach.Rows)
            {
                if (row == null || row.IsNewRow) continue; 
                                                           
                bool ngungKinhDoanh = false;
                if (dt.Columns.Contains("NgungKinhDoanh") && row.Cells["NgungKinhDoanh"].Value != DBNull.Value)
                {
                    ngungKinhDoanh = Convert.ToBoolean(row.Cells["NgungKinhDoanh"].Value);
                }
                if (ngungKinhDoanh)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.Cells["TrangThai"].Value = "Ngừng kinh doanh";
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.Cells["TrangThai"].Value = "Đang kinh doanh";
                }

              

            }

            // Xóa nội dung các TextBox khi mở form
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTacGia.Clear();
            txtGiaNhap.Clear();
            txtGiaBan.Clear();
            if (cboTheLoai.Items.Count > 0) cboTheLoai.SelectedIndex = -1;
            nudSoLuong.Value = 0;

            // Không chọn dòng nào trên DataGridView
            dgvSach.ClearSelection();
            txtTuKhoa.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTuKhoa.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var autoComplete = new AutoCompleteStringCollection();
            foreach (DataRow dataRow in sachBUS.LayDanhSachSach().Rows)
            {
                autoComplete.Add(dataRow["TenSach"].ToString());
            }
            txtTuKhoa.AutoCompleteCustomSource = autoComplete;
            dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Thiết lập combobox thể loại
            if (cboTheLoai.Items.Count > 0)
                cboTheLoai.SelectedIndex = 0;

            nudSoLuong.Minimum = 0;
            nudSoLuong.Maximum = 1000000;

            // Đảm bảo không có dòng nào được chọn và xóa thông tin textbox
            dgvSach.SelectionChanged -= dgvSach_SelectionChanged;
            dgvSach.ClearSelection();
            dgvSach.SelectionChanged += dgvSach_SelectionChanged;

            txtGiaBan.ReadOnly = true;
            txtGiaNhap.ReadOnly = true;
            if (Session.NhanVienHienTai != null)
            {
                mnTenNhanVien.Text = Session.NhanVienHienTai.TenNhanVien;
            }

            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = false;

            // Xử lý trạng thái nút kinh doanh lại/ngừng kinh doanh
            btnKinhDoanhLai.Enabled = false;
            btnNgungKinhDoanh.Enabled = false;

            if (dgvSach.CurrentRow != null && dgvSach.CurrentRow.Index >= 0)
            {
                bool ngungKinhDoanh = false;
                var cellValue = dgvSach.CurrentRow.Cells["NgungKinhDoanh"].Value;
                if (cellValue != null && cellValue != DBNull.Value)
                {
                    ngungKinhDoanh = Convert.ToBoolean(cellValue);
                }

                // Nếu ngừng kinh doanh thì chỉ bật nút "Kinh doanh lại"
                btnKinhDoanhLai.Enabled = ngungKinhDoanh;
                // Nếu đang kinh doanh thì chỉ bật nút "Ngừng kinh doanh"
                btnNgungKinhDoanh.Enabled = !ngungKinhDoanh;
            }
        }
  

        private void LoadSach()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT MaSach, TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, NgungKinhDoanh FROM Sach";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSach.DataSource = dt;
            }
        }


        private void ClearForm()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTacGia.Clear();
            txtGiaNhap.Clear();
            txtGiaBan.Clear();
            txtGiaBan.ReadOnly = true;

            cboTheLoai.SelectedIndex = -1;
            nudSoLuong.Value = 0;
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu bắt buộc
            if (string.IsNullOrWhiteSpace(txtTenSach.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sách.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(cboTheLoai.Text))
            {
                MessageBox.Show("Vui lòng chọn thể loại.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal donGia = 0;
            if (!string.IsNullOrWhiteSpace(txtGiaNhap.Text))
            {
                if (!decimal.TryParse(txtGiaNhap.Text, out donGia))
                {
                    MessageBox.Show("Đơn giá không hợp lệ. Vui lòng nhập số.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                donGia = 0; // Giá nhập mặc định là 0 nếu không có giá trị
            }
            txtGiaBan.ReadOnly = true;
            txtGiaBan.BackColor = SystemColors.Control;

            // Số lượng từ NumericUpDown (đã mặc định = 0 nên không cần kiểm tra)
            int soLuong = (int)nudSoLuong.Value;

            // Thực hiện chèn vào cơ sở dữ liệu
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Sach (TenSach, TacGia, TheLoai, SoLuong)
                         VALUES (@TenSach, @TacGia, @TheLoai, @SoLuong)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenSach", txtTenSach.Text.Trim());
                        cmd.Parameters.AddWithValue("@TacGia", txtTacGia.Text.Trim());
                        cmd.Parameters.AddWithValue("@TheLoai", cboTheLoai.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Thêm sách thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSach();  // Tải lại danh sách sách sau khi thêm
                            ClearForm(); // Xóa các trường nhập liệu sau khi thêm
                        }
                        else
                        {
                            MessageBox.Show("Thêm sách thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnThem.Enabled = false;
        }
        private void btnThemTheLoai_Click(object sender, EventArgs e)
        {
            string theLoaiMoi = Microsoft.VisualBasic.Interaction.InputBox(
        "Nhập tên thể loại mới:", "Thêm thể loại", "");

            if (!string.IsNullOrWhiteSpace(theLoaiMoi) && !cboTheLoai.Items.Contains(theLoaiMoi))
            {
                cboTheLoai.Items.Add(theLoaiMoi);
                cboTheLoai.SelectedItem = theLoaiMoi;
            }
        }

        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThem.Enabled = false;
            // Kiểm tra xem người dùng có nhấp vào ô dữ liệu hay không
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Lấy thông tin sách từ dòng đã chọn
                DataGridViewRow row = dgvSach.Rows[e.RowIndex];
                txtMaSach.Text = row.Cells["MaSach"].Value.ToString();
                txtTenSach.Text = row.Cells["TenSach"].Value.ToString();
                txtTacGia.Text = row.Cells["TacGia"].Value.ToString();
                cboTheLoai.SelectedItem = row.Cells["TheLoai"].Value.ToString();
                nudSoLuong.Value = Convert.ToDecimal(row.Cells["SoLuong"].Value);
                txtGiaNhap.Text = row.Cells["GiaNhap"].Value.ToString();
                txtGiaBan.Text = row.Cells["GiaBan"].Value.ToString();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có sách nào được chọn để sửa hay không
            if (string.IsNullOrEmpty(txtMaSach.Text))
            {
                MessageBox.Show("Vui lòng chọn sách cần sửa từ danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng SachDTO để cập nhật thông tin sách
            SachDTO s = new SachDTO
            {
                MaSach = txtMaSach.Text,
                TenSach = txtTenSach.Text,
                TacGia = txtTacGia.Text,
                TheLoai = cboTheLoai.SelectedItem.ToString(),
                GiaNhap = string.IsNullOrWhiteSpace(txtGiaNhap.Text) ? 0 : decimal.Parse(txtGiaNhap.Text),
                GiaBan = string.IsNullOrWhiteSpace(txtGiaBan.Text) ? 0 : decimal.Parse(txtGiaBan.Text),
                SoLuong = (int)nudSoLuong.Value,
            };

            // Kiểm tra nếu sửa thành công
            if (sachBUS.SuaSach(s))
            {
                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvSach.DataSource = sachBUS.LayDanhSachSach(); // Cập nhật lại danh sách

                // Lấy lại thông tin sách vừa sửa và cập nhật lên textbox
                var sachMoi = sachBUS.LaySachTheoMa(s.MaSach);
                if (sachMoi != null)
                {
                    txtMaSach.Text = sachMoi.MaSach;
                    txtTenSach.Text = sachMoi.TenSach;
                    txtTacGia.Text = sachMoi.TacGia;
                    cboTheLoai.SelectedItem = sachMoi.TheLoai;
                    nudSoLuong.Value = sachMoi.SoLuong;
                    txtGiaNhap.Text = sachMoi.GiaNhap.ToString();
                    txtGiaBan.Text = sachMoi.GiaBan.ToString();
                }
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtGiaBan.ReadOnly = true;
            txtGiaBan.BackColor = SystemColors.Control;

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maSach = txtMaSach.Text;
            if (string.IsNullOrEmpty(maSach))
            {
                MessageBox.Show("Vui lòng chọn sách cần xóa từ danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sách này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (sachBUS.XoaSach(maSach))
                {
                    MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvSach.DataSource = sachBUS.LayDanhSachSach();
                }
                else
                {
                    MessageBox.Show("Không thể xóa sách này vì đã có thông tin trong nhập sách hoặc hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void dgvSach_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSach.CurrentRow == null) return;

            DataGridViewRow row = dgvSach.CurrentRow;
            txtMaSach.Text = row.Cells["MaSach"].Value?.ToString();
            txtTenSach.Text = row.Cells["TenSach"].Value?.ToString();
            txtTacGia.Text = row.Cells["TacGia"].Value?.ToString();
            cboTheLoai.SelectedItem = row.Cells["TheLoai"].Value?.ToString();
            nudSoLuong.Value = row.Cells["SoLuong"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["SoLuong"].Value);
            txtGiaNhap.Text = row.Cells["GiaNhap"].Value?.ToString();
            txtGiaBan.Text = row.Cells["GiaBan"].Value?.ToString();

            btnKinhDoanhLai.Enabled = false;
            btnNgungKinhDoanh.Enabled = false;
            bool ngungKinhDoanh = false;
            if (dgvSach.Columns.Contains("NgungKinhDoanh"))
            {
                var cellValue = dgvSach.CurrentRow.Cells["NgungKinhDoanh"].Value;
                if (cellValue != null && cellValue != DBNull.Value)
                {
                    ngungKinhDoanh = Convert.ToBoolean(cellValue);
                }
            }
            btnKinhDoanhLai.Enabled = ngungKinhDoanh;
            btnNgungKinhDoanh.Enabled = !ngungKinhDoanh;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            ClearForm(); // Gọi phương thức ClearForm để xóa nội dung các TextBox và ComboBox
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text;
            dgvSach.DataSource = sachBUS.TimKiemSach(tuKhoa);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtTuKhoa.Text = string.Empty;

            dgvSach.DataSource = sachBUS.LayDanhSachSach();
            ClearFormTimkiem();
        }
        private void ClearFormTimkiem()
        {
            txtTuKhoa.Clear();
        }

        private void menuSach_Click(object sender, EventArgs e)
        {

        }

        private void menuNhanVien_Click(object sender, EventArgs e)
        {
            frmXemHoaDon nhanVienForm = new frmXemHoaDon();
            nhanVienForm.Show();
        }

        private void menuTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan taiKhoanForm = new frmTaiKhoan();
            taiKhoanForm.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            txtGiaBan.ReadOnly = false;
            txtGiaBan.BackColor = Color.White; // Đổi màu nền để dễ nhận biết
            txtGiaBan.Focus();
        }


        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin adminForm = new frmAdmin();
            adminForm.Show();
            this.Hide();
        }

        private void đĂNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
            this.Hide();
        }

        private void mnNhapSach_Click(object sender, EventArgs e)
        {
            frmXemPhieuNhapSach nhapSachForm = new frmXemPhieuNhapSach();
            nhapSachForm.Show();
            this.Hide();
        }

        private void mnNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanVienForm = new frmNhanVien();
            nhanVienForm.Show();
            this.Hide();
        }

        private void mnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmTaiKhoan frmTaiKhoan = new frmTaiKhoan();
            frmTaiKhoan.Show();
            this.Hide();
        }

        private void mnHoaDon_Click(object sender, EventArgs e)
        {
            frmXemPhieuNhapSach hoaDonForm = new frmXemPhieuNhapSach();
            hoaDonForm.Show();
            this.Hide();
        }

        private void mnDoanhThu_Click(object sender, EventArgs e)
        {
            frmDoanhthu doanhthuForm = new frmDoanhthu();
            doanhthuForm.Show();
            this.Hide();
        }

        private void btnKinhDoanhLai_Click(object sender, EventArgs e)
        {
            if (dgvSach.CurrentRow == null) return;
            var rowView = dgvSach.CurrentRow.DataBoundItem as DataRowView;
            if (rowView != null && Convert.ToBoolean(rowView["NgungKinhDoanh"]))
            {
                string maSach = rowView["MaSach"].ToString();
                if (sachBUS.KinhDoanhLai(maSach))
                {
                    MessageBox.Show("Đã chuyển sách về trạng thái kinh doanh!");
                    DataTable dt = sachBUS.LayDanhSachSach();
                    dgvSach.DataSource = dt;

                    CapNhatTrangThaiSach(); 
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!");
                }
            }
        }

        private void CapNhatTrangThaiSach()
        {
            // Kiểm tra nếu cột chưa tồn tại thì thêm
            if (!dgvSach.Columns.Contains("TrangThai"))
            {
                dgvSach.Columns.Add("TrangThai", "Trạng Thái");
            }

            foreach (DataGridViewRow row in dgvSach.Rows)
            {
                if (row.IsNewRow) continue;

      
                if (row.Cells["NgungKinhDoanh"].Value != null && row.Cells["NgungKinhDoanh"].Value != DBNull.Value)
                {
                    bool ngung = Convert.ToBoolean(row.Cells["NgungKinhDoanh"].Value);
                    row.DefaultCellStyle.BackColor = ngung ? Color.LightGray : Color.White;
                    row.Cells["TrangThai"].Value = ngung ? "Ngừng kinh doanh" : "Đang kinh doanh";
                }
            }
        }


        private void btnNgungKinhDoanh_Click(object sender, EventArgs e)
        {
            if (dgvSach.CurrentRow == null) return;
            var rowView = dgvSach.CurrentRow.DataBoundItem as DataRowView;
            if (rowView != null && !Convert.ToBoolean(rowView["NgungKinhDoanh"]))
            {
                string maSach = rowView["MaSach"].ToString();
                if (sachBUS.CapNhatTrangThaiNgungKinhDoanh(maSach, true))
                {
                    MessageBox.Show("Đã chuyển sách sang trạng thái ngừng kinh doanh!");
                    DataTable dt = sachBUS.LayDanhSachSach();
                    dgvSach.DataSource = dt;
                    CapNhatTrangThaiSach(); 
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!");
                }
            }
        }


        private void txtTuKhoa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dgvSach.DataSource = sachBUS.TimKiemSach(txtTuKhoa.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}



