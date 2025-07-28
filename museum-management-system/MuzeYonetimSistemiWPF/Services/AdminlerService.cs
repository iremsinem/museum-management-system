using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class AdminService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;"; // SQL bağlantı string'i
        public List<Admin> GetAllAdmin()
        {
            List<Admin> adminler = new List<Admin>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Adminler", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    adminler.Add(new Admin
                    {
                        ID = (int)reader["ID"],
                        KullaniciAdi = reader["KullaniciAdi"].ToString(),
                        SifreHash = reader["SifreHash"].ToString(),
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        Email = reader["Email"].ToString(),
                        YetkiSeviyesi = reader["YetkiSeviyesi"].ToString()
                    });
                }
            }
            return adminler;
        }

        public void Add(Admin admin)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Adminler (KullaniciAdi, SifreHash, Ad, Soyad, Email, YetkiSeviyesi)
                             VALUES (@KullaniciAdi, @SifreHash, @Ad, @Soyad, @Email, @YetkiSeviyesi)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@KullaniciAdi", admin.KullaniciAdi);
                cmd.Parameters.AddWithValue("@SifreHash", admin.SifreHash);
                cmd.Parameters.AddWithValue("@Ad", admin.Ad);
                cmd.Parameters.AddWithValue("@Soyad", admin.Soyad);
                cmd.Parameters.AddWithValue("@Email", admin.Email);
                cmd.Parameters.AddWithValue("@YetkiSeviyesi", admin.YetkiSeviyesi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Admin admin)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Adminler SET 
                             KullaniciAdi = @KullaniciAdi,
                             SifreHash = @SifreHash,
                             Ad = @Ad,
                             Soyad = @Soyad,
                             Email = @Email,
                             YetkiSeviyesi = @YetkiSeviyesi
                             WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", admin.ID);
                cmd.Parameters.AddWithValue("@KullaniciAdi", admin.KullaniciAdi);
                cmd.Parameters.AddWithValue("@SifreHash", admin.SifreHash);
                cmd.Parameters.AddWithValue("@Ad", admin.Ad);
                cmd.Parameters.AddWithValue("@Soyad", admin.Soyad);
                cmd.Parameters.AddWithValue("@Email", admin.Email);
                cmd.Parameters.AddWithValue("@YetkiSeviyesi", admin.YetkiSeviyesi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Adminler WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Admin GirisYap(string kullaniciAdi, string sifreHash)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Adminler WHERE KullaniciAdi = @KullaniciAdi AND SifreHash = @SifreHash";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@SifreHash", sifreHash);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Admin
                    {
                        ID = (int)reader["ID"],
                        KullaniciAdi = reader["KullaniciAdi"].ToString(),
                        SifreHash = reader["SifreHash"].ToString(),
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        Email = reader["Email"].ToString(),
                        YetkiSeviyesi = reader["YetkiSeviyesi"].ToString()
                    };
                }

                return null;
            }
        }


    }
}