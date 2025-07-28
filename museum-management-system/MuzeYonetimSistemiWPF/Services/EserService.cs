using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EserService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;        "; // SQL bağlantı string'i

        public List<Eser> GetAllEserler()
        {
            List<Eser> eserler = new List<Eser>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Eserler", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    eserler.Add(new Eser
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Tur_ID = (int)reader["Tur_ID"],
                        Sanatci_ID = (int)reader["Sanatci_ID"],
                        YapimYili = (int)reader["YapimYili"],
                        BulunduguMuze = reader["BulunduguMuze"].ToString(),
                        MevcutDurum = reader["MevcutDurum"].ToString(),
                        DijitalKoleksiyonURL = reader["DijitalKoleksiyonURL"].ToString()
                    });
                }
            }
            return eserler;
        }

        public void Add(Eser eser)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Eserler (Ad, Tur_ID, Sanatci_ID, YapimYili, BulunduguMuze, MevcutDurum, DijitalKoleksiyonURL) VALUES (@Ad, @Tur_ID, @Sanatci_ID, @YapimYili, @BulunduguMuze, @MevcutDurum, @DijitalKoleksiyonURL)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", eser.Ad);
                cmd.Parameters.AddWithValue("@Tur_ID", eser.Tur_ID);
                cmd.Parameters.AddWithValue("@Sanatci_ID", eser.Sanatci_ID);
                cmd.Parameters.AddWithValue("@YapimYili", eser.YapimYili);
                cmd.Parameters.AddWithValue("@BulunduguMuze", eser.BulunduguMuze);
                cmd.Parameters.AddWithValue("@MevcutDurum", eser.MevcutDurum);
                cmd.Parameters.AddWithValue("@DijitalKoleksiyonURL", eser.DijitalKoleksiyonURL);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Eser eser)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Eserler SET Ad = @Ad, Tur_ID = @Tur_ID, Sanatci_ID = @Sanatci_ID, YapimYili = @YapimYili, BulunduguMuze = @BulunduguMuze, MevcutDurum = @MevcutDurum, DijitalKoleksiyonURL = @DijitalKoleksiyonURL WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", eser.ID);
                cmd.Parameters.AddWithValue("@Ad", eser.Ad);
                cmd.Parameters.AddWithValue("@Tur_ID", eser.Tur_ID);
                cmd.Parameters.AddWithValue("@Sanatci_ID", eser.Sanatci_ID);
                cmd.Parameters.AddWithValue("@YapimYili", eser.YapimYili);
                cmd.Parameters.AddWithValue("@BulunduguMuze", eser.BulunduguMuze);
                cmd.Parameters.AddWithValue("@MevcutDurum", eser.MevcutDurum);
                cmd.Parameters.AddWithValue("@DijitalKoleksiyonURL", eser.DijitalKoleksiyonURL);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public int AddWithSP(Eser eser)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_EserEkle", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ad", eser.Ad);
                cmd.Parameters.AddWithValue("@Tur_ID", eser.Tur_ID);
                cmd.Parameters.AddWithValue("@Sanatci_ID", eser.Sanatci_ID);
                cmd.Parameters.AddWithValue("@YapimYili", eser.YapimYili);
                cmd.Parameters.AddWithValue("@BulunduguMuze", eser.BulunduguMuze);
                cmd.Parameters.AddWithValue("@MevcutDurum", eser.MevcutDurum);
                cmd.Parameters.AddWithValue("@DijitalKoleksiyonURL", eser.DijitalKoleksiyonURL);

                con.Open();
                int newId = Convert.ToInt32(cmd.ExecuteScalar()); // 👈 ID geliyor
                return newId;
            }
        }


        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Eserler WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
