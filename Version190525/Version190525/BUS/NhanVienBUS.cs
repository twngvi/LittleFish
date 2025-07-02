using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BUS
{
    public class NhanVienBUS
    {
        private NhanVienDAL dal = new NhanVienDAL();

        public DataTable LayDanhSachNhanVien() => dal.LayDanhSachNhanVien();
        public bool ThemNhanVien(NhanVienDTO nv) => dal.ThemNhanVien(nv);
        public bool CapNhatNhanVien(NhanVienDTO nv) => dal.CapNhatNhanVien(nv);
        public bool XoaNhanVien(int maNV) => dal.XoaNhanVien(maNV);
        
        private NhanVienDAL _dal = new NhanVienDAL();

        public NhanVienDTO LayNhanVienTheoMaTaiKhoan(int maTaiKhoan)
        {
            return _dal.LayTheoMaTaiKhoan(maTaiKhoan);
        }
    }

}
