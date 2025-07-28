using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EserBakimKaydiService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<EserBakimKaydi> GetAllEserBakimKaydi()
        {
            List<EserBakimKaydi> bakimKayitlari = new List<EserBakimKaydi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM EserBakimKayitlari", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bakimKayitlari.Add(new EserBakimKaydi
                    {
                        ID = (int)reader["ID"],
                        EserID = (int)reader["EserID"],
                        PersonelID = reader["PersonelID"] != DBNull.Value ? (int)reader["PersonelID"] : 0,

                        BakimTarihi = (DateTime)reader["BakimTarihi"],
                        YapilanIslem = reader["YapilanIslem"].ToString() ?? string.Empty

                    });
                }
            }
            return bakimKayitlari;
        }

        public IEnumerable<EserBakimKaydi> GetBakimByPersonel(int personelId)
        {
            var list = new List<EserBakimKaydi>();
            using var con = new SqlConnection(connectionString);
            const string sql = @"SELECT * FROM EserBakimKayitlari WHERE PersonelID = @id";
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", personelId);
            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(new EserBakimKaydi
                {
                    ID = (int)reader["ID"],
                    EserID = (int)reader["EserID"],
                    PersonelID = (int)reader["PersonelID"],
                    BakimTarihi = (DateTime)reader["BakimTarihi"],
                    YapilanIslem = reader["YapilanIslem"].ToString() ?? string.Empty
                });
            return list;
        }
        public void Add(EserBakimKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO EserBakimKayitlari (EserID, PersonelID, BakimTarihi, YapilanIslem)
                                 VALUES (@EserID, @PersonelID, @BakimTarihi, @YapilanIslem)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EserID", kayit.EserID);
                cmd.Parameters.AddWithValue("@PersonelID", kayit.PersonelID);
                cmd.Parameters.AddWithValue("@BakimTarihi", kayit.BakimTarihi);
                cmd.Parameters.AddWithValue("@YapilanIslem", kayit.YapilanIslem);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(EserBakimKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE EserBakimKayitlari SET 
                                 EserID = @EserID,
                                 PersonelID = @PersonelID,
                                 BakimTarihi = @BakimTarihi,
                                 YapilanIslem = @YapilanIslem
                                 WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", kayit.ID);
                cmd.Parameters.AddWithValue("@EserID", kayit.EserID);
                cmd.Parameters.AddWithValue("@PersonelID", kayit.PersonelID);
                cmd.Parameters.AddWithValue("@BakimTarihi", kayit.BakimTarihi);
                cmd.Parameters.AddWithValue("@YapilanIslem", kayit.YapilanIslem);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddWithSP(EserBakimKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_EserBakimiEkle", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EserID", kayit.EserID);
                cmd.Parameters.AddWithValue("@PersonelID", kayit.PersonelID);
                cmd.Parameters.AddWithValue("@BakimTarihi", kayit.BakimTarihi);
                cmd.Parameters.AddWithValue("@YapilanIslem", kayit.YapilanIslem);
                

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM EserBakimKayitlari WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}