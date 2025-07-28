using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EserSergileriService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<EserSergi> GetAllEserSergi()
        {
            List<EserSergi> eserSergileri = new List<EserSergi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM EserSergileri", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    eserSergileri.Add(new EserSergi
                    {
                        ID = (int)reader["ID"],
                        EserID = (int)reader["EserID"],
                        SergiID = (int)reader["SergiID"],
                        BaslangicTarihi = Convert.ToDateTime(reader["BaslangicTarihi"]),
                        BitisTarihi = Convert.ToDateTime(reader["BitisTarihi"])
                    });
                }
            }
            return eserSergileri;
        }

        public void Add(EserSergi eserSergi)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO EserSergileri (EserID, SergiID, BaslangicTarihi, BitisTarihi) VALUES (@EserID, @SergiID, @BaslangicTarihi, @BitisTarihi)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EserID", eserSergi.EserID);
                cmd.Parameters.AddWithValue("@SergiID", eserSergi.SergiID);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", eserSergi.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", eserSergi.BitisTarihi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(EserSergi eserSergi)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE EserSergileri SET EserID = @EserID, SergiID = @SergiID, BaslangicTarihi = @BaslangicTarihi, BitisTarihi = @BitisTarihi WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", eserSergi.ID);
                cmd.Parameters.AddWithValue("@EserID", eserSergi.EserID);
                cmd.Parameters.AddWithValue("@SergiID", eserSergi.SergiID);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", eserSergi.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", eserSergi.BitisTarihi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void AddWithSP(EserSergi eserSergi)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_EseriSergiyeEkle", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EserID", eserSergi.EserID);
                cmd.Parameters.AddWithValue("@SergiID", eserSergi.SergiID);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", eserSergi.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", eserSergi.BitisTarihi);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM EserSergileri WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<(int MappingID, int ID, string Ad, string SanatciAd)> GetEserlerBySergiID(int sergiID)
        {
            var liste = new List<(int MappingID, int ID, string Ad, string SanatciAd)>();

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
        SELECT 
            esg.ID AS MappingID,
            e.ID AS EserID,
            e.Ad,
            s.Ad AS SanatciAd
        FROM EserSergileri esg
        INNER JOIN Eserler e ON esg.EserID = e.ID
        LEFT JOIN Sanatcilar s ON e.Sanatci_ID = s.ID
        WHERE esg.SergiID = @SergiID
    ", conn))
            {
                cmd.Parameters.AddWithValue("@SergiID", sergiID);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add((
                            reader.GetInt32(reader.GetOrdinal("MappingID")),
                            reader.GetInt32(reader.GetOrdinal("EserID")),
                            reader.GetString(reader.GetOrdinal("Ad")),
                            reader.IsDBNull(reader.GetOrdinal("SanatciAd")) ? "" : reader.GetString(reader.GetOrdinal("SanatciAd"))
                        ));
                    }
                }
            }

            return liste;
        }
    }
}