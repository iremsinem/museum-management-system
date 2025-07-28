using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EtkinlikService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<Etkinlik> GetAllEtkinlik()
        {
            List<Etkinlik> etkinlikler = new List<Etkinlik>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT e.ID, e.Ad, e.Tur, e.BaslangicTarihi, e.BitisTarihi, e.Aciklama, e.TurID, t.Ad AS EtkinlikTuruAd FROM Etkinlikler e LEFT JOIN EtkinlikTurleri t ON e.TurID = t.ID", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    etkinlikler.Add(new Etkinlik
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Tur = reader["Tur"].ToString(),
                        BaslangicTarihi = (DateTime)reader["BaslangicTarihi"],
                        BitisTarihi = (DateTime)reader["BitisTarihi"],
                        Aciklama = reader["Aciklama"].ToString(),
                        TurID = reader["TurID"] as int?,
                        EtkinlikTuruAd = reader["EtkinlikTuruAd"]?.ToString() ?? "—"
                    });
                }
            }
            return etkinlikler;
        }

        public void Add(Etkinlik etkinlik)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Etkinlikler 
                                (Ad, Tur, BaslangicTarihi, BitisTarihi, Aciklama, TurID) 
                                VALUES 
                                (@Ad, @Tur, @BaslangicTarihi, @BitisTarihi, @Aciklama, @TurID)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", etkinlik.Ad);
                cmd.Parameters.AddWithValue("@Tur", etkinlik.Tur);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", etkinlik.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", etkinlik.BitisTarihi);
                cmd.Parameters.AddWithValue("@Aciklama", etkinlik.Aciklama ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TurID", etkinlik.TurID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Etkinlik etkinlik)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Etkinlikler SET 
                                Ad = @Ad,
                                Tur = @Tur,
                                BaslangicTarihi = @BaslangicTarihi,
                                BitisTarihi = @BitisTarihi,
                                Aciklama = @Aciklama,
                                TurID=@TurID
                                WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", etkinlik.ID);
                cmd.Parameters.AddWithValue("@Ad", etkinlik.Ad);
                cmd.Parameters.AddWithValue("@Tur", etkinlik.Tur);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", etkinlik.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", etkinlik.BitisTarihi);
                cmd.Parameters.AddWithValue("@Aciklama", etkinlik.Aciklama ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TurID", etkinlik.TurID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddWithSP(Etkinlik etkinlik)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_EtkinlikEkle", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ad", etkinlik.Ad);
                cmd.Parameters.AddWithValue("@Tur", etkinlik.Tur);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", etkinlik.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", etkinlik.BitisTarihi);
                cmd.Parameters.AddWithValue("@Aciklama", etkinlik.Aciklama ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TurID", etkinlik.TurID ?? (object)DBNull.Value);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Etkinlikler WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}