using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class ZiyaretciService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;"; // SQL bağlantı string'i
        public List<Ziyaretci> GetAllZiyaretci()
        {
            List<Ziyaretci> ziyaretciler = new List<Ziyaretci>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ziyaretciler", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ziyaretciler.Add(new Ziyaretci
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        DogumTarihi = Convert.ToDateTime(reader["DogumTarihi"]),
                        Email = reader["Email"].ToString(),
                        UyelikDurumu = (bool)reader["UyelikDurumu"]
                    });
                }
            }
            return ziyaretciler;
        }

        public void Add(Ziyaretci ziyaretci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Ziyaretciler (Ad, Soyad, DogumTarihi, Email, UyelikDurumu) VALUES (@Ad, @Soyad, @DogumTarihi, @Email, @UyelikDurumu)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", ziyaretci.Ad);
                cmd.Parameters.AddWithValue("@Soyad", ziyaretci.Soyad);
                cmd.Parameters.AddWithValue("@DogumTarihi", ziyaretci.DogumTarihi);
                cmd.Parameters.AddWithValue("@Email", ziyaretci.Email);
                cmd.Parameters.AddWithValue("@UyelikDurumu", ziyaretci.UyelikDurumu);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Ziyaretci ziyaretci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Ziyaretciler SET Ad = @Ad, Soyad = @Soyad, DogumTarihi = @DogumTarihi, Email = @Email, UyelikDurumu = @UyelikDurumu WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", ziyaretci.ID);
                cmd.Parameters.AddWithValue("@Ad", ziyaretci.Ad);
                cmd.Parameters.AddWithValue("@Soyad", ziyaretci.Soyad);
                cmd.Parameters.AddWithValue("@DogumTarihi", ziyaretci.DogumTarihi);
                cmd.Parameters.AddWithValue("@Email", ziyaretci.Email);
                cmd.Parameters.AddWithValue("@UyelikDurumu", ziyaretci.UyelikDurumu);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddWithSP(Ziyaretci ziyaretci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_ZiyaretciEkle", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ad", ziyaretci.Ad);
                cmd.Parameters.AddWithValue("@Soyad", ziyaretci.Soyad);
                cmd.Parameters.AddWithValue("@DogumTarihi", ziyaretci.DogumTarihi);
                cmd.Parameters.AddWithValue("@Email", ziyaretci.Email);
                cmd.Parameters.AddWithValue("@UyelikDurumu", ziyaretci.UyelikDurumu);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Ziyaretciler WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}