using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;
namespace DAL
{
    public class HoaDonDAL
    {
        private string connectionString = @"Data Source=DESKTOP-303SSFE;Initial Catalog=NhaSachDB;Integrated Security=True";

        public bool ThemHoaDon(HoaDonDTO hoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO HoaDon (TongTien, ThoiGianTao, TenNhanVien, MaNhanVien)
                         VALUES (@TongTien, @ThoiGianTao, @TenNhanVien, @MaNhanVien);
                         SELECT SCOPE_IDENTITY();"; // Lấy ID tự động tạo
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TongTien", hoaDon.TongTien);
                cmd.Parameters.AddWithValue("@ThoiGianTao", hoaDon.ThoiGianTao);
                cmd.Parameters.AddWithValue("@TenNhanVien", hoaDon.TenNhanVien);
                cmd.Parameters.AddWithValue("@MaNhanVien", hoaDon.MaNhanVien);

                conn.Open();
                hoaDon.MaHoaDon = Convert.ToInt32(cmd.ExecuteScalar()); 
                return hoaDon.MaHoaDon > 0;
            }
        }
        public List<HoaDonDTO> LayTatCaHoaDon()
        {
            List<HoaDonDTO> list = new List<HoaDonDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaHoaDon, TongTien, ThoiGianTao, TenNhanVien, MaNhanVien FROM HoaDon";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HoaDonDTO hd = new HoaDonDTO
                    {
                        MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                        TongTien = Convert.ToDecimal(reader["TongTien"]),
                        ThoiGianTao = Convert.ToDateTime(reader["ThoiGianTao"]),
                        TenNhanVien = reader["TenNhanVien"].ToString(),
                        MaNhanVien = Convert.ToInt32(reader["MaNhanVien"])
                    };
                    list.Add(hd);
                }
                reader.Close();
            }
            return list;
        }
    }
}