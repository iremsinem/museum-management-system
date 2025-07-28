using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EserTransferiService
    {
        private string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;        "; // SQL bağlantı string'i
        public List<EserTransferi> GetAllEserTransferi()
        {
            List<EserTransferi> transferler = new List<EserTransferi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EserTransferleri";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    transferler.Add(new EserTransferi
                    {
                        ID = (int)reader["ID"],
                        EserID = (int)reader["EserID"],
                        KaynakMuze = reader["KaynakMuze"].ToString(),
                        HedefMuze = reader["HedefMuze"].ToString(),
                        Tarih = (DateTime)reader["Tarih"],
                        TransferDurumu = reader["TransferDurumu"].ToString()
                    });
                }
            }
            return transferler;
        }

        public void Add(EserTransferi transfer)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO EserTransferleri (EserID, KaynakMuze, HedefMuze, Tarih, TransferDurumu)
                                 VALUES (@EserID, @KaynakMuze, @HedefMuze, @Tarih, @TransferDurumu)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EserID", transfer.EserID);
                cmd.Parameters.AddWithValue("@KaynakMuze", transfer.KaynakMuze);
                cmd.Parameters.AddWithValue("@HedefMuze", transfer.HedefMuze);
                cmd.Parameters.AddWithValue("@Tarih", transfer.Tarih);
                cmd.Parameters.AddWithValue("@TransferDurumu", transfer.TransferDurumu);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(EserTransferi transfer)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE EserTransferleri SET
                                 EserID = @EserID,
                                 KaynakMuze = @KaynakMuze,
                                 HedefMuze = @HedefMuze,
                                 Tarih = @Tarih,
                                 TransferDurumu = @TransferDurumu
                                 WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", transfer.ID);
                cmd.Parameters.AddWithValue("@EserID", transfer.EserID);
                cmd.Parameters.AddWithValue("@KaynakMuze", transfer.KaynakMuze);
                cmd.Parameters.AddWithValue("@HedefMuze", transfer.HedefMuze);
                cmd.Parameters.AddWithValue("@Tarih", transfer.Tarih);
                cmd.Parameters.AddWithValue("@TransferDurumu", transfer.TransferDurumu);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM EserTransferleri WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}