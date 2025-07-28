using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class SanatcilarService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;"; // SQL bağlantı string'i
        public List<Sanatci> GetAllSanatcilar()
        {
            List<Sanatci> sanatcilar = new List<Sanatci>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Sanatcilar", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sanatcilar.Add(new Sanatci
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        DogumTarihi = Convert.ToDateTime(reader["DogumTarihi"]),
                        OlumTarihi = reader["OlumTarihi"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["OlumTarihi"]),
                        Ulke = reader["Ulke"].ToString(),
                        Biyografi = reader["Biyografi"].ToString()
                    });
                }
            }
            return sanatcilar;
        }

        public int Add(Sanatci sanatci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_SanatciEkle", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ad", sanatci.Ad);
                cmd.Parameters.AddWithValue("@DogumTarihi", sanatci.DogumTarihi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@OlumTarihi", sanatci.OlumTarihi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Ulke", sanatci.Ulke ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Biyografi", sanatci.Biyografi ?? (object)DBNull.Value);

                con.Open();
                int newId = Convert.ToInt32(cmd.ExecuteScalar());  // ID döner
                return newId;
            }
        }


        public void Update(Sanatci sanatci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Sanatcilar SET Ad = @Ad, DogumTarihi = @DogumTarihi, OlumTarihi = @OlumTarihi, Ulke = @Ulke, Biyografi = @Biyografi WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", sanatci.ID);
                cmd.Parameters.AddWithValue("@Ad", sanatci.Ad);
                cmd.Parameters.AddWithValue("@DogumTarihi", sanatci.DogumTarihi);
                cmd.Parameters.AddWithValue("@OlumTarihi", (object?)sanatci.OlumTarihi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ulke", sanatci.Ulke);
                cmd.Parameters.AddWithValue("@Biyografi", sanatci.Biyografi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void AddWithSP(Sanatci sanatci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_SanatciEkle", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ad", sanatci.Ad);
                cmd.Parameters.AddWithValue("@DogumTarihi", sanatci.DogumTarihi);
                cmd.Parameters.AddWithValue("@OlumTarihi", (object?)sanatci.OlumTarihi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ulke", sanatci.Ulke);
                cmd.Parameters.AddWithValue("@Biyografi", sanatci.Biyografi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Sanatcilar WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}