using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class BagisService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;    "; // SQL bağlantı string'i
        public List<Bagis> GetAllBagis()
        {
            List<Bagis> bagislar = new List<Bagis>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Bagislar", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bagislar.Add(new Bagis
                    {
                        ID = (int)reader["ID"],
                        BagisciID = (int)reader["BagisciID"],
                        Miktar = (decimal)reader["Miktar"],
                        BagisTarihi = (DateTime)reader["BagisTarihi"],
                        KullanimAlani = reader["KullanimAlani"].ToString()
                    });
                }
            }
            return bagislar;
        }

        public void Add(Bagis bagis)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Bagislar (BagisciID, Miktar, BagisTarihi, KullanimAlani) 
                                VALUES (@BagisciID, @Miktar, @BagisTarihi, @KullanimAlani)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BagisciID", bagis.BagisciID);
                cmd.Parameters.AddWithValue("@Miktar", bagis.Miktar);
                cmd.Parameters.AddWithValue("@BagisTarihi", bagis.BagisTarihi);
                cmd.Parameters.AddWithValue("@KullanimAlani", bagis.KullanimAlani);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Bagis bagis)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Bagislar SET 
                                BagisciID = @BagisciID,
                                Miktar = @Miktar,
                                BagisTarihi = @BagisTarihi,
                                KullanimAlani = @KullanimAlani
                                WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", bagis.ID);
                cmd.Parameters.AddWithValue("@BagisciID", bagis.BagisciID);
                cmd.Parameters.AddWithValue("@Miktar", bagis.Miktar);
                cmd.Parameters.AddWithValue("@BagisTarihi", bagis.BagisTarihi);
                cmd.Parameters.AddWithValue("@KullanimAlani", bagis.KullanimAlani);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Bagislar WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}