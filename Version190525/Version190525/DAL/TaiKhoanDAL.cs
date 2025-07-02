﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class TaiKhoanDAL
    {
        private string connectionString = @"Data Source=DESKTOP-303SSFE;Initial Catalog=NhaSachDB;Integrated Security=True";

        public List<TaiKhoanDTO> LayDanhSachTaiKhoan()
        {
            List<TaiKhoanDTO> danhSach = new List<TaiKhoanDTO>();
            string query = "SELECT * FROM TaiKhoanNguoiDung";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TaiKhoanDTO tk = new TaiKhoanDTO
                    (
                        reader["MaTaiKhoan"].ToString(),
                        reader["TenDangNhap"].ToString(),
                        reader["MatKhau"].ToString(),
                        reader["VaiTro"].ToString(),
                        Convert.ToBoolean(reader["KichHoat"])
                    );
                    danhSach.Add(tk);
                }
            }
            return danhSach;
        }

        public bool ThemTaiKhoan(TaiKhoanDTO tk)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Check if the username already exists
                string checkQuery = "SELECT COUNT(*) FROM TaiKhoanNguoiDung WHERE TenDangNhap = @Ten";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Ten", tk.TenDangNhap);

                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    // Username already exists
                    throw new Exception($"Tên đăng nhập '{tk.TenDangNhap}' đã tồn tại.");
                }

                // Insert the new account
                string insertQuery = "INSERT INTO TaiKhoanNguoiDung (TenDangNhap, MatKhau, VaiTro, KichHoat) VALUES (@Ten, @MatKhau, @VaiTro, @KichHoat)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Ten", tk.TenDangNhap);
                insertCmd.Parameters.AddWithValue("@MatKhau", tk.MatKhau);
                insertCmd.Parameters.AddWithValue("@VaiTro", tk.VaiTro);
                insertCmd.Parameters.AddWithValue("@KichHoat", tk.KichHoat);

                return insertCmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CapNhatTaiKhoan(TaiKhoanDTO tk)
        {
            string query = "UPDATE TaiKhoanNguoiDung SET TenDangNhap=@Ten, MatKhau=@MatKhau, VaiTro=@VaiTro, KichHoat=@KichHoat WHERE MaTaiKhoan=@Ma";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ma", tk.MaTaiKhoan);
                cmd.Parameters.AddWithValue("@Ten", tk.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", tk.MatKhau);
                cmd.Parameters.AddWithValue("@VaiTro", tk.VaiTro);
                cmd.Parameters.AddWithValue("@KichHoat", tk.KichHoat);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public TaiKhoanDTO DangNhap(string tenDangNhap, string matKhau)
        {
            string query = "SELECT * FROM TaiKhoanNguoiDung WHERE TenDangNhap = @Ten AND MatKhau = @Mat";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", tenDangNhap);
                cmd.Parameters.AddWithValue("@Mat", matKhau);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    TaiKhoanDTO tk = new TaiKhoanDTO
                    {
                        MaTaiKhoan = reader["MaTaiKhoan"].ToString(),
                        TenDangNhap = reader["TenDangNhap"].ToString(),
                        MatKhau = reader["MatKhau"].ToString(),
                        VaiTro = reader["VaiTro"].ToString(),
                        KichHoat = Convert.ToBoolean(reader["KichHoat"])
                    };
                    return tk;
                }
                return null;
            }
        }
        public bool XoaTaiKhoan(string maTaiKhoan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM TaiKhoanNguoiDung WHERE MaTaiKhoan = @MaTaiKhoan";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public List<TaiKhoanDTO> TimKiemTaiKhoan(string tenDangNhap, string vaiTro)
        {
            List<TaiKhoanDTO> danhSach = new List<TaiKhoanDTO>();
            string query = "SELECT * FROM TaiKhoanNguoiDung WHERE LOWER(TenDangNhap) LIKE @Ten AND (@VaiTro IS NULL OR VaiTro = @VaiTro)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", $"%{tenDangNhap}%");
                cmd.Parameters.AddWithValue("@VaiTro", string.IsNullOrEmpty(vaiTro) ? (object)DBNull.Value : vaiTro);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TaiKhoanDTO tk = new TaiKhoanDTO
                    (
                        reader["MaTaiKhoan"].ToString(),
                        reader["TenDangNhap"].ToString(),
                        reader["MatKhau"].ToString(),
                        reader["VaiTro"].ToString(),
                        Convert.ToBoolean(reader["KichHoat"])
                    );
                    danhSach.Add(tk);
                }
            }
            return danhSach;
        }
        public List<TaiKhoanDTO> LayTaiKhoanChuaGanNhanVien()
        {
            List<TaiKhoanDTO> list = new List<TaiKhoanDTO>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM TaiKhoan WHERE MaTaiKhoan NOT IN (SELECT MaTaiKhoan FROM NhanVien WHERE MaTaiKhoan IS NOT NULL)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TaiKhoanDTO
                        {
                            MaTaiKhoan = reader["MaTaiKhoan"].ToString(), // Fixed conversion to string
                            TenDangNhap = reader["TenDangNhap"].ToString()
                            // Add other fields if necessary
                        });
                    }
                }
            }
            return list;
        }

    }

}
