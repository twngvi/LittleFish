using System;
using System.Collections.Generic;
using System.Data;
using DTO;
using DAL; // Đảm bảo có using đúng namespace chứa SachDAL và ThongKeDoanhThuResult
using System.Linq;

public class SachBUS
{
    private SachDAL dal = new SachDAL();

    public DataTable LayDanhSachSach()
    {
        return dal.LayDanhSachSach();
    }

    public bool ThemSach(SachDTO s) => dal.ThemSach(s);
    public bool SuaSach(SachDTO s) => dal.SuaSach(s);

    public bool XoaSach(string maSach)
    {
        if (CoLienKetNhapSachOrHoaDon(maSach))
            return false; // Không xóa nếu có liên kết

        return dal.XoaSach(maSach);
    }

    public bool KinhDoanhLai(string maSach)
    {
        return dal.CapNhatTrangThaiNgungKinhDoanh(maSach, false);
    }

    public List<SachDTO> TimKiemSach(string tuKhoa)
    {
        return dal.TimKiemSach(tuKhoa);
    }

    public SachDTO LaySachTheoMa(string maSach)
    {
        return dal.LaySachTheoMa(maSach);
    }

    public bool CapNhatSoLuong(string maSach, int soLuongMoi)
    {
        return dal.CapNhatSoLuong(maSach, soLuongMoi);
    }

    public DataTable LayDanhSachSachDangKinhDoanh()
    {
        return dal.LayDanhSachSachDangKinhDoanh();
    }

    public bool CapNhatTrangThaiNgungKinhDoanh(string maSach, bool ngungKinhDoanh)
    {
        return dal.CapNhatTrangThaiNgungKinhDoanh(maSach, ngungKinhDoanh);
    }

    public bool CoLienKetNhapSachOrHoaDon(string maSach)
    {
        return dal.CoLienKetNhapSachOrHoaDon(maSach);
    }


    public static ThongKeDoanhThuResult ThongKeDoanhThu(DateTime? from = null, DateTime? to = null)
    {
        var hoaDonDal = new HoaDonDAL();
        var hoaDons = hoaDonDal.LayTatCaHoaDon();

        // Lọc hóa đơn theo ngày nếu có from/to
        if (from.HasValue && to.HasValue)
        {
            hoaDons = hoaDons
            .Where(hd => hd.ThoiGianTao >= from.Value && hd.ThoiGianTao <= to.Value)
            .ToList();
        }

        var chiTietHoaDonDal = new ChiTietHoaDonDAL();
        var chiTietHoaDons = chiTietHoaDonDal.LayTatCaChiTietHoaDon();

        // Lọc chi tiết hóa đơn theo các hóa đơn đã lọc
        var hoaDonIds = hoaDons.Select(hd => hd.MaHoaDon).ToHashSet();
        chiTietHoaDons = chiTietHoaDons
            .Where(ct => hoaDonIds.Contains(ct.MaHoaDon))
            .ToList();

        int soHoaDon = hoaDons.Count;
        int soSachBan = chiTietHoaDons.Sum(ct => ct.SoLuong);
        decimal doanhThu = chiTietHoaDons.Sum(ct => ct.GiaBan * ct.SoLuong);

        decimal loiNhuan = 0;
        var sachDal = new SachDAL();
        foreach (var ct in chiTietHoaDons)
        {
            var sach = sachDal.LaySachTheoMa(ct.MaSach);
            if (sach == null || sach.GiaNhap < 0 || ct.GiaBan < sach.GiaNhap)
                continue;
            loiNhuan += (ct.GiaBan - sach.GiaNhap) * ct.SoLuong;
        }

        // Tính top bán chạy và bán ít
        var sachBanGroup = chiTietHoaDons
            .GroupBy(ct => ct.MaSach)
            .Select(g => new
            {
                MaSach = g.Key,
                SoLuongBan = g.Sum(x => x.SoLuong)
            })
            .ToList();

        var allSach = sachDal.LayDanhSachSachDangKinhDoanh();
        var sachThongKeList = allSach
    .AsEnumerable()
    .Select(s => {
        var maSach = s.Field<int>("MaSach").ToString();
        var soLuongBan = sachBanGroup.FirstOrDefault(x => x.MaSach == maSach)?.SoLuongBan ?? 0;
        var giaBan = s.Field<decimal>("GiaBan");

        return new SachThongKeDTO
        {
            MaSach = maSach,
            TenSach = s.Field<string>("TenSach"),
            SoLuongBan = soLuongBan,
            TongTienBan = soLuongBan * giaBan
        };
    })
    .ToList();
        var topBanChay = sachThongKeList.OrderByDescending(s => s.SoLuongBan).Take(5).ToList();
        var topBanIt = sachThongKeList.OrderBy(s => s.SoLuongBan).Take(5).ToList();

        return new ThongKeDoanhThuResult
        {
            SoHoaDon = soHoaDon,
            SoSachBan = soSachBan,
            DoanhThu = doanhThu,
            LoiNhuan = loiNhuan,
            TopBanChay = topBanChay,
            TopBanIt = topBanIt
        };
    }

}