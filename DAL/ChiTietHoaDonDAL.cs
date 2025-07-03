using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

public class ChiTietHoaDonDAL
{
    private string connectionString = @"Data Source=DESKTOP-303SSFE;Initial Catalog=NhaSachDB;Integrated Security=True";

    public bool ThemChiTietHoaDon(ChiTietHoaDonDTO chiTiet)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"INSERT INTO ChiTietHoaDon (MaHoaDon, MaSach, SoLuong, GiaBan)
                         VALUES (@MaHoaDon, @MaSach, @SoLuong, @GiaBan)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaHoaDon", chiTiet.MaHoaDon);
            cmd.Parameters.AddWithValue("@MaSach", chiTiet.MaSach);
            cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
            cmd.Parameters.AddWithValue("@GiaBan", chiTiet.GiaBan);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public List<ChiTietHoaDonDTO> LayChiTietHoaDonTheoMaHoaDon(int maHoaDon)
    {
        List<ChiTietHoaDonDTO> list = new List<ChiTietHoaDonDTO>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT cthd.MaChiTiet, cthd.MaHoaDon, cthd.MaSach, s.TenSach, cthd.SoLuong, cthd.GiaBan
                             FROM ChiTietHoaDon cthd
                             JOIN Sach s ON cthd.MaSach = s.MaSach
                             WHERE cthd.MaHoaDon = @MaHoaDon";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ct = new ChiTietHoaDonDTO
                {
                    MaChiTiet = Convert.ToInt32(reader["MaChiTiet"]),
                    MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                    MaSach = reader["MaSach"].ToString(),
                    TenSach = reader["TenSach"].ToString(),
                    SoLuong = Convert.ToInt32(reader["SoLuong"]),
                    GiaBan = Convert.ToDecimal(reader["GiaBan"])
                };
                list.Add(ct);
            }
            reader.Close();
        }
        return list;
    }

    // Lấy tất cả chi tiết hóa đơn
    public List<ChiTietHoaDonDTO> LayTatCaChiTietHoaDon()
    {
        List<ChiTietHoaDonDTO> list = new List<ChiTietHoaDonDTO>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT cthd.MaChiTiet, cthd.MaHoaDon, cthd.MaSach, s.TenSach, cthd.SoLuong, cthd.GiaBan
                             FROM ChiTietHoaDon cthd
                             JOIN Sach s ON cthd.MaSach = s.MaSach";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ct = new ChiTietHoaDonDTO
                {
                    MaChiTiet = Convert.ToInt32(reader["MaChiTiet"]),
                    MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                    MaSach = reader["MaSach"].ToString(),
                    TenSach = reader["TenSach"].ToString(),
                    SoLuong = Convert.ToInt32(reader["SoLuong"]),
                    GiaBan = Convert.ToDecimal(reader["GiaBan"])
                };
                list.Add(ct);
            }
            reader.Close();
        }
        return list;
    }

    // Xóa chi tiết hóa đơn theo mã chi tiết
    public bool XoaChiTietHoaDon(int maChiTiet)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM ChiTietHoaDon WHERE MaChiTiet = @MaChiTiet";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaChiTiet", maChiTiet);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    // Sửa chi tiết hóa đơn
    public bool SuaChiTietHoaDon(ChiTietHoaDonDTO chiTiet)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"UPDATE ChiTietHoaDon 
                             SET MaHoaDon = @MaHoaDon, MaSach = @MaSach, SoLuong = @SoLuong, GiaBan = @GiaBan
                             WHERE MaChiTiet = @MaChiTiet";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaChiTiet", chiTiet.MaChiTiet);
            cmd.Parameters.AddWithValue("@MaHoaDon", chiTiet.MaHoaDon);
            cmd.Parameters.AddWithValue("@MaSach", chiTiet.MaSach);
            cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
            cmd.Parameters.AddWithValue("@GiaBan", chiTiet.GiaBan);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

}
