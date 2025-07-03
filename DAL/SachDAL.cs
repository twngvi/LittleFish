using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;

public class SachDAL
{
    private string connectionString = @"Data Source=DESKTOP-303SSFE;Initial Catalog=NhaSachDB;Integrated Security=True";

    public DataTable LayDanhSachSach()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT MaSach, TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, NgungKinhDoanh FROM Sach";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    public DataTable LayDanhSachSachDangKinhDoanh()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT MaSach, TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan FROM Sach WHERE NgungKinhDoanh = 0";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    public bool ThemSach(SachDTO s)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"INSERT INTO Sach (TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, NgungKinhDoanh)
                             VALUES (@TenSach, @TacGia, @TheLoai, @SoLuong, @GiaNhap, @GiaBan, 0)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TenSach", s.TenSach);
            cmd.Parameters.AddWithValue("@TacGia", s.TacGia);
            cmd.Parameters.AddWithValue("@TheLoai", s.TheLoai);
            cmd.Parameters.AddWithValue("@SoLuong", s.SoLuong);
            cmd.Parameters.AddWithValue("@GiaNhap", s.GiaNhap);
            cmd.Parameters.AddWithValue("@GiaBan", s.GiaBan);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public bool SuaSach(SachDTO s)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"UPDATE Sach SET TenSach = @TenSach, TacGia = @TacGia, TheLoai = @TheLoai,
                             SoLuong = @SoLuong, GiaNhap = @GiaNhap, GiaBan = @GiaBan
                             WHERE MaSach = @MaSach";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaSach", s.MaSach);
            cmd.Parameters.AddWithValue("@TenSach", s.TenSach);
            cmd.Parameters.AddWithValue("@TacGia", s.TacGia);
            cmd.Parameters.AddWithValue("@TheLoai", s.TheLoai);
            cmd.Parameters.AddWithValue("@SoLuong", s.SoLuong);
            cmd.Parameters.AddWithValue("@GiaNhap", s.GiaNhap);
            cmd.Parameters.AddWithValue("@GiaBan", s.GiaBan);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public bool XoaSach(string maSach)
    {
        if (!int.TryParse(maSach, out int maSachInt))
            return false;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Sach WHERE MaSach = @MaSach";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaSach", maSachInt);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public List<SachDTO> TimKiemSach(string tuKhoa)
    {
        List<SachDTO> list = new List<SachDTO>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT MaSach, TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, NgungKinhDoanh 
                             FROM Sach WHERE TenSach LIKE @TuKhoa OR TacGia LIKE @TuKhoa OR TheLoai LIKE @TuKhoa";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SachDTO
                {
                    MaSach = reader["MaSach"].ToString(),
                    TenSach = reader["TenSach"].ToString(),
                    TacGia = reader["TacGia"].ToString(),
                    TheLoai = reader["TheLoai"].ToString(),
                    SoLuong = Convert.ToInt32(reader["SoLuong"]),
                    GiaNhap = reader["GiaNhap"] != DBNull.Value ? Convert.ToDecimal(reader["GiaNhap"]) : 0,
                    GiaBan = reader["GiaBan"] != DBNull.Value ? Convert.ToDecimal(reader["GiaBan"]) : 0,
                    NgungKinhDoanh = Convert.ToBoolean(reader["NgungKinhDoanh"])
                });
            }
            reader.Close();
        }
        return list;
    }

    public SachDTO LaySachTheoMa(string maSach)
    {
        if (!int.TryParse(maSach, out int maSachInt))
            return null;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM Sach WHERE MaSach = @MaSach";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaSach", maSachInt);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new SachDTO
                {
                    MaSach = reader["MaSach"].ToString(),
                    TenSach = reader["TenSach"].ToString(),
                    TacGia = reader["TacGia"].ToString(),
                    TheLoai = reader["TheLoai"].ToString(),
                    SoLuong = Convert.ToInt32(reader["SoLuong"]),
                    GiaNhap = reader["GiaNhap"] != DBNull.Value ? Convert.ToDecimal(reader["GiaNhap"]) : 0,
                    GiaBan = reader["GiaBan"] != DBNull.Value ? Convert.ToDecimal(reader["GiaBan"]) : 0,
                    NgungKinhDoanh = Convert.ToBoolean(reader["NgungKinhDoanh"])
                };
            }
        }
        return null;
    }

    public bool CapNhatSoLuong(string maSach, int soLuongMoi)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "UPDATE Sach SET SoLuong = @SoLuongMoi WHERE MaSach = @MaSach";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SoLuongMoi", soLuongMoi);
            cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public bool CapNhatTrangThaiNgungKinhDoanh(string maSach, bool ngung)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "UPDATE Sach SET NgungKinhDoanh = @Ngung WHERE MaSach = @MaSach";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Ngung", ngung);
            cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public bool CoNhapSach(string maSach)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT COUNT(*) FROM NhapSach WHERE MaSach = @MaSach";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
            conn.Open();
            return (int)cmd.ExecuteScalar() > 0;
        }
    }

    public bool CoBanSach(string maSach)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaSach = @MaSach";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
            conn.Open();
            return (int)cmd.ExecuteScalar() > 0;
        }
    }

    public bool CoLienKetNhapSachOrHoaDon(string maSach)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string query1 = "SELECT COUNT(*) FROM NhapSach WHERE MaSach = @MaSach";
            using (SqlCommand cmd = new SqlCommand(query1, conn))
            {
                cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
                if ((int)cmd.ExecuteScalar() > 0)
                    return true;
            }

            string query2 = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaSach = @MaSach";
            using (SqlCommand cmd = new SqlCommand(query2, conn))
            {
                cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
                if ((int)cmd.ExecuteScalar() > 0)
                    return true;
            }
        }
        return false;
    }
    public List<SachDTO> ThongKeSachBan(DateTime from, DateTime to)
    {
        var result = new List<SachDTO>();
        using (var conn = new SqlConnection(connectionString))
        {
            string query = @"
        SELECT 
            s.MaSach, 
            s.TenSach, 
            SUM(ct.SoLuong) AS SoLuongBan,
            SUM(ct.SoLuong * ct.DonGia) AS TongTienBan
        FROM 
            HoaDon hd
            JOIN ChiTietHoaDon ct ON hd.MaHoaDon = ct.MaHoaDon
            JOIN Sach s ON ct.MaSach = s.MaSach
        WHERE 
            hd.ThoiGianTao >= @from AND hd.ThoiGianTao <= @to
        GROUP BY 
            s.MaSach, s.TenSach
    ";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new SachDTO
                {
                    MaSach = reader["MaSach"].ToString(),
                    TenSach = reader["TenSach"].ToString(),
                    SoLuongBan = reader["SoLuongBan"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongBan"]) : 0,
                    TongTienBan = reader["TongTienBan"] != DBNull.Value ? Convert.ToDecimal(reader["TongTienBan"]) : 0
                });
            }
        }
        return result;
    }




}
