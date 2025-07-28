using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class BagisciService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;"; // SQL bağlantı string'i
        public List<Bagisci> GetAllBagisci()
        {
            List<Bagisci> bagiscilar = new List<Bagisci>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Bagiscilar", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bagiscilar.Add(new Bagisci
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        Kurum = reader["Kurum"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefon = reader["Telefon"].ToString()
                    });
                }
            }
            return bagiscilar;
        }

        public void Add(Bagisci bagisci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Bagiscilar (Ad, Soyad, Kurum, Email, Telefon) 
                                VALUES (@Ad, @Soyad, @Kurum, @Email, @Telefon)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", bagisci.Ad);
                cmd.Parameters.AddWithValue("@Soyad", bagisci.Soyad);
                cmd.Parameters.AddWithValue("@Kurum", bagisci.Kurum);
                cmd.Parameters.AddWithValue("@Email", bagisci.Email);
                cmd.Parameters.AddWithValue("@Telefon", bagisci.Telefon);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Bagisci bagisci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Bagiscilar SET 
                                Ad = @Ad,
                                Soyad = @Soyad,
                                Kurum = @Kurum,
                                Email = @Email,
                                Telefon = @Telefon
                                WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", bagisci.ID);
                cmd.Parameters.AddWithValue("@Ad", bagisci.Ad);
                cmd.Parameters.AddWithValue("@Soyad", bagisci.Soyad);
                cmd.Parameters.AddWithValue("@Kurum", bagisci.Kurum);
                cmd.Parameters.AddWithValue("@Email", bagisci.Email);
                cmd.Parameters.AddWithValue("@Telefon", bagisci.Telefon);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Bagiscilar WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}