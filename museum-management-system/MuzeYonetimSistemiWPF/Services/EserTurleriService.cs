using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EserTurleriService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<EserTurleri> GetAllEserTurleri()
        {
            List<EserTurleri> turler = new List<EserTurleri>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM EserTurleri", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    turler.Add(new EserTurleri
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Aciklama = reader["Aciklama"].ToString()
                    });
                }
            }
            return turler;
        }

        public void Add(EserTurleri tur)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO EserTurleri (Ad, Aciklama) VALUES (@Ad, @Aciklama)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", tur.Ad);
                cmd.Parameters.AddWithValue("@Aciklama", tur.Aciklama);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(EserTurleri tur)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE EserTurleri SET Ad = @Ad, Aciklama = @Aciklama WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", tur.ID);
                cmd.Parameters.AddWithValue("@Ad", tur.Ad);
                cmd.Parameters.AddWithValue("@Aciklama", tur.Aciklama);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM EserTurleri WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}