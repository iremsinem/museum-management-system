using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class SanatAkimiService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<SanatAkimi> GetAllSanatAkimi()
        {
            List<SanatAkimi> akimlar = new List<SanatAkimi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM SanatAkimlari", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    akimlar.Add(new SanatAkimi
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Aciklama = reader["Aciklama"].ToString()
                    });
                }
            }
            return akimlar;
        }

        public int Add(SanatAkimi akim)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_SanatAkimiEkle", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ad", akim.Ad);
                cmd.Parameters.AddWithValue("@Aciklama", akim.Aciklama ?? (object)DBNull.Value);

                con.Open();
                int newId = Convert.ToInt32(cmd.ExecuteScalar()); // ID'yi al
                return newId;
            }
        }


        public void AddSanatAkimi(SanatAkimi akim) => Add(akim);

        public void Update(SanatAkimi akim)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE SanatAkimlari SET 
                                 Ad = @Ad,
                                 Aciklama = @Aciklama
                                 WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", akim.ID);
                cmd.Parameters.AddWithValue("@Ad", akim.Ad);
                cmd.Parameters.AddWithValue("@Aciklama", akim.Aciklama);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSanatAkimi(SanatAkimi akim) => Update(akim);

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM SanatAkimlari WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteSanatAkimi(int id) => Delete(id);
    }
}