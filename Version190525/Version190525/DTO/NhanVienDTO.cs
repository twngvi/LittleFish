using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NhanVienDTO
    {
        public int MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string ChucVu { get; set; }
        public string LoaiNhanVien { get; set; }
        public int MaTaiKhoan { get; set; }

    }
    public static class Session
    {
        public static NhanVienDTO NhanVienHienTai { get; set; }
        public static TaiKhoanDTO TaiKhoanHienTai { get; set; }
    }


}
