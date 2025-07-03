using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BUS
{
    public class NhapSachBUS
    {
        NhapSachDAL dal = new NhapSachDAL();
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        public bool ThemNhapSach(NhapSachDTO ns)
        {
            return dal.ThemNhapSach(ns);
        }

        public decimal? LayGiaSach(string maSach)
        {
            return dal.LayGiaSach(maSach);
        }

        public void CapNhatGiaSach(string maSach, decimal giaNhap)
        {
            decimal giaBan = giaNhap + (giaNhap * 0.5m);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Sach SET GiaNhap = @GiaNhap WHERE MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@GiaNhap", giaNhap);
                cmd.Parameters.AddWithValue("@MaSach", maSach);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<NhapSachDTO> LayLichSuNhap()
        {
            return dal.LayLichSuNhap();
        }


    }

}
