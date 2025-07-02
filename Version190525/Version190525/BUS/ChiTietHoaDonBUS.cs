using System;
using System.Collections.Generic;
using DTO;

public class ChiTietHoaDonBUS
{
    private ChiTietHoaDonDAL chiTietHoaDonDAL = new ChiTietHoaDonDAL();

    public bool ThemChiTietHoaDon(ChiTietHoaDonDTO chiTiet)
    {
        return chiTietHoaDonDAL.ThemChiTietHoaDon(chiTiet);
    }

    public List<ChiTietHoaDonDTO> LayChiTietHoaDonTheoMaHoaDon(int maHoaDon)
    {
        return chiTietHoaDonDAL.LayChiTietHoaDonTheoMaHoaDon(maHoaDon);
    }
}
