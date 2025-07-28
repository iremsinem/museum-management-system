using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class SanatciAkimService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;"; // SQL bağlantı string'i

        public List<SanatciAkim> GetAllSanatciAkim()
        {
            List<SanatciAkim> records = new List<SanatciAkim>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT SanatciID, AkimID, s.Ad AS SanatciAd, a.Ad AS AkimAd FROM SanatciAkim sa INNER JOIN Sanatcilar s ON sa.SanatciID = s.ID INNER JOIN SanatAkimlari a ON sa.AkimID = a.ID", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    records.Add(new SanatciAkim
                    {
                        SanatciID = (int)reader["SanatciID"],
                        AkimID = (int)reader["AkimID"],
                        SanatciAd = reader["SanatciAd"].ToString(),
                        AkimAd = reader["AkimAd"].ToString()
                    });
                }
            }

            return records;
        }

        public List<SanatciAkim> GetSanatciAkimlariBySanatciID(int sanatciID)
        {
            List<SanatciAkim> akimlar = new List<SanatciAkim>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT SanatciID, AkimID FROM SanatciAkim WHERE SanatciID = @SanatciID", con);
                cmd.Parameters.AddWithValue("@SanatciID", sanatciID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    akimlar.Add(new SanatciAkim
                    {
                        SanatciID = (int)reader["SanatciID"],
                        AkimID = (int)reader["AkimID"]
                    });
                }
            }
            return akimlar;
        }

        public void Add(int sanatciID, int akimID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO SanatciAkim (SanatciID, AkimID)
                                 VALUES (@SanatciID, @AkimID)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SanatciID", sanatciID);
                cmd.Parameters.AddWithValue("@AkimID", akimID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddSanatciAkim(int sanatciID, int akimID) => Add(sanatciID, akimID);

        public void AddSanatciAkim(SanatciAkim iliski)
        {
            Add(iliski.SanatciID, iliski.AkimID);
        }
        public void Delete(int sanatciID, int akimID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM SanatciAkim WHERE SanatciID = @SanatciID AND AkimID = @AkimID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SanatciID", sanatciID);
                cmd.Parameters.AddWithValue("@AkimID", akimID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void AddWithSP(int sanatciID, int akimID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_SanatciyaAkimEkle", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SanatciID", sanatciID);
                cmd.Parameters.AddWithValue("@AkimID", akimID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void DeleteSanatciAkim(int sanatciID, int akimID) => Delete(sanatciID, akimID);
    }
    
    
    }