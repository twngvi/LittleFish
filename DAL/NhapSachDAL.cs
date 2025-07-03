using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class NhapSachDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        public bool ThemNhapSach(NhapSachDTO ns)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO NhapSach (MaSach, SoLuong, NgayNhap, GiaNhap,TenNhanVien, MaNhanVien) " +
                               "VALUES ( @MaSach, @SoLuong, @NgayNhap, @GiaNhap, @TenNhanVien, @MaNhanVien); " +
                               "UPDATE Sach SET SoLuong = SoLuong + @SoLuong, GiaNhap = @GiaNhap " +
                               "WHERE MaSach = @MaSach;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSach", ns.MaSach);
                cmd.Parameters.AddWithValue("@SoLuong", ns.SoLuong);
                cmd.Parameters.AddWithValue("@NgayNhap", ns.NgayNhap);
                cmd.Parameters.AddWithValue("@GiaNhap", ns.GiaNhap);
                cmd.Parameters.AddWithValue("@TenNhanVien", ns.TenNhanVien);
                cmd.Parameters.AddWithValue("@MaNhanVien", ns.MaNhanVien);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public decimal? LayGiaSach(string maSach)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT GiaNhap FROM Sach WHERE MaSach = @MaSach"; // Hoặc "GiaBan" nếu cần
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSach", maSach);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? (decimal?)Convert.ToDecimal(result) : null;
            }
        }

        public void CapNhatGiaSach(string maSach, decimal giaNhap, decimal giaBan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Sach SET GiaNhap = @GiaNhap, GiaBan = @GiaBan WHERE MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@GiaNhap", giaNhap);
                cmd.Parameters.AddWithValue("@GiaBan", giaBan);
                cmd.Parameters.AddWithValue("@MaSach", maSach);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<NhapSachDTO> LayLichSuNhap()
        {
            var list = new List<NhapSachDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT ns.MaNhap, s.TenSach, ns.SoLuong, ns.GiaNhap, ns.TenNhanVien, ns.NgayNhap, ns.MaSach, ns.MaNhanVien
                 FROM NhapSach ns
                 JOIN Sach s ON ns.MaSach = s.MaSach
                 ORDER BY ns.NgayNhap DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new NhapSachDTO
                        {
                            MaNhap = reader["MaNhap"].ToString(),
                            TenSach = reader["TenSach"].ToString(),
                            SoLuong = Convert.ToInt32(reader["SoLuong"]),
                            GiaNhap = Convert.ToDecimal(reader["GiaNhap"]),
                            NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                            TenNhanVien = reader["TenNhanVien"].ToString()
                        });
                    }
                }
            }
            return list;
        }


    }
}
