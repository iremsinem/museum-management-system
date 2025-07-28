using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EtkinlikKayitService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<EtkinlikKaydi> GetAllEtkinlikKayit()
        {
            List<EtkinlikKaydi> kayitlar = new List<EtkinlikKaydi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM EtkinlikKayitlari", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    kayitlar.Add(new EtkinlikKaydi
                    {
                        ID = (int)reader["ID"],
                        ZiyaretciID = (int)reader["ZiyaretciID"],
                        EtkinlikID = (int)reader["EtkinlikID"],
                        KayitTarihi = (DateTime)reader["KayitTarihi"]
                    });
                }
            }
            return kayitlar;
        }

        public void Add(EtkinlikKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO EtkinlikKayitlari 
                                (ZiyaretciID, EtkinlikID, KayitTarihi) 
                                VALUES 
                                (@ZiyaretciID, @EtkinlikID, @KayitTarihi)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ZiyaretciID", kayit.ZiyaretciID);
                cmd.Parameters.AddWithValue("@EtkinlikID", kayit.EtkinlikID);
                cmd.Parameters.AddWithValue("@KayitTarihi", kayit.KayitTarihi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(EtkinlikKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE EtkinlikKayitlari SET 
                                ZiyaretciID = @ZiyaretciID,
                                EtkinlikID = @EtkinlikID,
                                KayitTarihi = @KayitTarihi
                                WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", kayit.ID);
                cmd.Parameters.AddWithValue("@ZiyaretciID", kayit.ZiyaretciID);
                cmd.Parameters.AddWithValue("@EtkinlikID", kayit.EtkinlikID);
                cmd.Parameters.AddWithValue("@KayitTarihi", kayit.KayitTarihi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddWithSP(EtkinlikKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_ZiyaretciEtkinlikKaydiYap", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ZiyaretciID", kayit.ZiyaretciID);
                cmd.Parameters.AddWithValue("@EtkinlikID", kayit.EtkinlikID);
                cmd.Parameters.AddWithValue("@KayitTarihi", kayit.KayitTarihi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM EtkinlikKayitlari WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
