using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace MuzeYonetimSistemiWPF.Helpers
{
    public static class SqlHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MuzeDB"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static bool BaglantiTest()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return conn.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı hatası: " + ex.Message);
                return false;
            }
        }
    }
}
