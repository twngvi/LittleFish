using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SachDTO
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public bool NgungKinhDoanh { get; set; }
        public int SoLuongBan { get; set; }      
        public decimal TongTienBan { get; set; }
    }
    public class ThongKeDoanhThuResult
    {
        public decimal DoanhThu { get; set; }
        public decimal LoiNhuan { get; set; }
        public int SoHoaDon { get; set; }
        public int SoSachBan { get; set; }
        public List<SachThongKeDTO> TopBanChay { get; set; } = new List<SachThongKeDTO>();
        public List<SachThongKeDTO> TopBanIt { get; set; } = new List<SachThongKeDTO>();
    }

    public class SachThongKeDTO
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuongBan { get; set; }
        public decimal TongTienBan { get; set; } 
    }


}
