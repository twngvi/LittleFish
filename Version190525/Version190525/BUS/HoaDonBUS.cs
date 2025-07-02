using System;
using System.Collections.Generic;
using DTO;
using DAL; // Ensure this namespace is included

public class HoaDonBUS
{
    private HoaDonDAL hoaDonDAL = new HoaDonDAL();

    public bool ThemHoaDon(HoaDonDTO hoaDon)
    {
        return hoaDonDAL.ThemHoaDon(hoaDon);
    }
    public List<HoaDonDTO> LayTatCaHoaDon()
    {
        return hoaDonDAL.LayTatCaHoaDon();
    }
}
