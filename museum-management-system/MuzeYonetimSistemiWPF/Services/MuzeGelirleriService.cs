using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class MuzeGeliriService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<MuzeGeliri> GetAllMuzeGeliri()
        {
            List<MuzeGeliri> gelirler = new List<MuzeGeliri>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM MuzeGelirleri", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    gelirler.Add(new MuzeGeliri
                    {
                        ID = (int)reader["ID"],
                        KaynakTuru = reader["KaynakTuru"].ToString(),
                        Tutar = (decimal)reader["Tutar"],
                        Tarih = (DateTime)reader["Tarih"]
                    });
                }
            }
            return gelirler;
        }

        public int Add(MuzeGeliri gelir)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO MuzeGelirleri (KaynakTuru, Tutar, Tarih) 
            VALUES (@KaynakTuru, @Tutar, @Tarih);
            SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@KaynakTuru", gelir.KaynakTuru);
                cmd.Parameters.AddWithValue("@Tutar", gelir.Tutar);
                cmd.Parameters.AddWithValue("@Tarih", gelir.Tarih);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }



        public void Update(MuzeGeliri gelir)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE MuzeGelirleri SET 
                                KaynakTuru = @KaynakTuru,
                                Tutar = @Tutar,
                                Tarih = @Tarih
                                WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", gelir.ID);
                cmd.Parameters.AddWithValue("@KaynakTuru", gelir.KaynakTuru);
                cmd.Parameters.AddWithValue("@Tutar", gelir.Tutar);
                cmd.Parameters.AddWithValue("@Tarih", gelir.Tarih);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void AddWithSP(MuzeGeliri gelir, int adminId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GelirEkle_Logla", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@KaynakTuru", gelir.KaynakTuru);
                cmd.Parameters.AddWithValue("@Tutar", gelir.Tutar);
                cmd.Parameters.AddWithValue("@Tarih", gelir.Tarih);
                cmd.Parameters.AddWithValue("@AdminID", adminId); // Log için
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddOrUpdate(MuzeGeliri g)
        {
            if (g.ID == 0) Add(g); else Update(g);
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM MuzeGelirleri WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}