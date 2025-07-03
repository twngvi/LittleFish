using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DTO
{
    public class TaiKhoanDTO
    {
        public string MaTaiKhoan { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; }
        public bool KichHoat { get; set; }
       


        public TaiKhoanDTO() { } // 

        public TaiKhoanDTO(string ma, string ten, string matKhau, string vaiTro, bool kichHoat)
        {
            MaTaiKhoan = ma;
            TenDangNhap = ten;
            MatKhau = matKhau;
            VaiTro = vaiTro;
            KichHoat = kichHoat;
        }
    }
}
