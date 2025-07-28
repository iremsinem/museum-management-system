using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class BiletTuruService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;"; // SQL bağlantı string'i
        public List<BiletTuru> GetAllBiletTuru()
        {
            List<BiletTuru> biletTurleri = new List<BiletTuru>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM BiletTurleri", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    biletTurleri.Add(new BiletTuru
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Fiyat = (decimal)reader["Fiyat"],
                        GecerlilikSuresi = (int)reader["GecerlilikSuresi"]
                    });
                }
            }
            return biletTurleri;
        }

        public void Add(BiletTuru biletTuru)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO BiletTurleri (Ad, Fiyat, GecerlilikSuresi) VALUES (@Ad, @Fiyat, @GecerlilikSuresi)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", biletTuru.Ad);
                cmd.Parameters.AddWithValue("@Fiyat", biletTuru.Fiyat);
                cmd.Parameters.AddWithValue("@GecerlilikSuresi", biletTuru.GecerlilikSuresi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(BiletTuru biletTuru)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE BiletTurleri SET Ad = @Ad, Fiyat = @Fiyat, GecerlilikSuresi = @GecerlilikSuresi WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", biletTuru.ID);
                cmd.Parameters.AddWithValue("@Ad", biletTuru.Ad);
                cmd.Parameters.AddWithValue("@Fiyat", biletTuru.Fiyat);
                cmd.Parameters.AddWithValue("@GecerlilikSuresi", biletTuru.GecerlilikSuresi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM BiletTurleri WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}