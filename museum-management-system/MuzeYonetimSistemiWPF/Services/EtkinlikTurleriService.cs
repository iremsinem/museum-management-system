using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EtkinlikTuruService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<EtkinlikTuru> GetAllEtkinlikTuru()
        {
            List<EtkinlikTuru> etkinlikTurleriListesi = new List<EtkinlikTuru>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EtkinlikTurleri";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    etkinlikTurleriListesi.Add(new EtkinlikTuru
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Aciklama = reader["Aciklama"].ToString()
                    });
                }
            }
            return etkinlikTurleriListesi;
        }

        public void Add(EtkinlikTuru etkinlikTuru)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO EtkinlikTurleri (Ad, Aciklama)
                                 VALUES (@Ad, @Aciklama)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", etkinlikTuru.Ad);
                cmd.Parameters.AddWithValue("@Aciklama", etkinlikTuru.Aciklama);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(EtkinlikTuru etkinlikTuru)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE EtkinlikTurleri SET
                                 Ad = @Ad,
                                 Aciklama = @Aciklama
                                 WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", etkinlikTuru.ID);
                cmd.Parameters.AddWithValue("@Ad", etkinlikTuru.Ad);
                cmd.Parameters.AddWithValue("@Aciklama", etkinlikTuru.Aciklama);
                con.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM EtkinlikTurleri WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}