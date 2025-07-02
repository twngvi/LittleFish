using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DAL;
using DTO;

namespace BUS
{
    public class TaiKhoanBUS
    {
        private TaiKhoanDAL dal = new TaiKhoanDAL();

        public List<TaiKhoanDTO> LayTatCaTaiKhoan()
        {
            return dal.LayDanhSachTaiKhoan();
        }

        public bool ThemTaiKhoan(TaiKhoanDTO tk)
        {
            return dal.ThemTaiKhoan(tk);
        }

        public bool CapNhatTaiKhoan(TaiKhoanDTO tk)
        {
            return dal.CapNhatTaiKhoan(tk);
        }
        public TaiKhoanDTO Login(string username, string password)
        {
            return dal.DangNhap(username, password);
        }
        public bool XoaTaiKhoan(string maTaiKhoan)
        {
            return dal.XoaTaiKhoan(maTaiKhoan);
        }
        public List<TaiKhoanDTO> TimKiemTaiKhoan(string tenDangNhap, string vaiTro)
        {
            return dal.TimKiemTaiKhoan(tenDangNhap, vaiTro);
        }

    }

}
