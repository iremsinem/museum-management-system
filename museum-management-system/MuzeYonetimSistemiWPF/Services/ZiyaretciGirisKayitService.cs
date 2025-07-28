using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class ZiyaretciGirisKaydiService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<ZiyaretciGirisKaydi> GetAllZiyaretciGirisKaydi()
        {
            List<ZiyaretciGirisKaydi> kayitlar = new List<ZiyaretciGirisKaydi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ZiyaretciGirisKayitlari", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    kayitlar.Add(new ZiyaretciGirisKaydi
                    {
                        ID = (int)reader["ID"],
                        ZiyaretciID = (int)reader["ZiyaretciID"],
                        GirisTarihi = (DateTime)reader["GirisTarihi"],
                        CikisTarihi = reader["CikisTarihi"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["CikisTarihi"],
                        BiletTuru = (int)reader["BiletTuru"],
                        SergiID = reader["SergiID"] == DBNull.Value ? (int?)null : (int)reader["SergiID"],
                        EtkinlikID = reader["EtkinlikID"] == DBNull.Value ? (int?)null : (int)reader["EtkinlikID"]
                    });
                }
            }
            return kayitlar;
        }

        public void Add(ZiyaretciGirisKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO ZiyaretciGirisKayitlari 
                                (ZiyaretciID, GirisTarihi, CikisTarihi, BiletTuru, SergiID, EtkinlikID)
                                 VALUES (@ZiyaretciID, @GirisTarihi, @CikisTarihi, @BiletTuru, @SergiID, @EtkinlikID)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ZiyaretciID", kayit.ZiyaretciID);
                cmd.Parameters.AddWithValue("@GirisTarihi", kayit.GirisTarihi);
                cmd.Parameters.AddWithValue("@CikisTarihi", (object?)kayit.CikisTarihi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BiletTuru", kayit.BiletTuru);
                cmd.Parameters.AddWithValue("@SergiID", (object?)kayit.SergiID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EtkinlikID", (object?)kayit.EtkinlikID ?? DBNull.Value);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(ZiyaretciGirisKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE ZiyaretciGirisKayitlari SET 
                                ZiyaretciID = @ZiyaretciID,
                                GirisTarihi = @GirisTarihi,
                                CikisTarihi = @CikisTarihi,
                                BiletTuru = @BiletTuru,
                                SergiID = @SergiID,
                                EtkinlikID = @EtkinlikID
                                WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", kayit.ID);
                cmd.Parameters.AddWithValue("@ZiyaretciID", kayit.ZiyaretciID);
                cmd.Parameters.AddWithValue("@GirisTarihi", kayit.GirisTarihi);
                cmd.Parameters.AddWithValue("@CikisTarihi", (object?)kayit.CikisTarihi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BiletTuru", kayit.BiletTuru);
                cmd.Parameters.AddWithValue("@SergiID", (object?)kayit.SergiID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EtkinlikID", (object?)kayit.EtkinlikID ?? DBNull.Value);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<ZiyaretciGirisKaydi> GetByZiyaretci(int ziyaretciId)
{
    var list = new List<ZiyaretciGirisKaydi>();

    using var con = new SqlConnection(connectionString);
    const string sql = @"SELECT * FROM ZiyaretciGirisKayitlari WHERE ZiyaretciID = @id";
    using var cmd = new SqlCommand(sql, con);
    cmd.Parameters.AddWithValue("@id", ziyaretciId);

    con.Open();
    using var rdr = cmd.ExecuteReader();
    while (rdr.Read())
    {
        list.Add(new ZiyaretciGirisKaydi
        {
            ID          = (int)rdr["ID"],
            ZiyaretciID = (int)rdr["ZiyaretciID"],
            GirisTarihi = (DateTime)rdr["GirisTarihi"],
            CikisTarihi = rdr["CikisTarihi"] == DBNull.Value ? null : (DateTime?)rdr["CikisTarihi"],
            BiletTuru   = (int)rdr["BiletTuru"],               // ← string
            SergiID     = rdr["SergiID"]    == DBNull.Value ? null : (int?)rdr["SergiID"],
            EtkinlikID  = rdr["EtkinlikID"] == DBNull.Value ? null : (int?)rdr["EtkinlikID"]
        });
    }
    return list;
}

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ZiyaretciGirisKayitlari WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}