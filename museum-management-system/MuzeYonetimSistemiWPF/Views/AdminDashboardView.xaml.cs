using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using MuzeYonetimSistemiWPF.Models;


namespace MuzeYonetimSistemiWPF.Views
{
    /// <summary>
    /// AdminDashboardView.xaml etkileşim mantığı
    /// </summary>
    public partial class AdminDashboardView : Window
    {
        private readonly Admin _admin;
        public AdminDashboardView(Admin admin)
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            _admin = admin;

           

        }

     

        private void BtnEser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new EserYonetimiView(_admin) { Owner = this }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Sergi Yönetimi penceresi açılırken hata");
            }
        }

        private void BtnSergi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SergiYonetimiView(_admin) { Owner = this }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Sergi Yönetimi penceresi açılırken hata");
            }
        }

        private void BtnEtkinlik_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new EtkinlikYonetimiView(_admin) { Owner = this }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Etkinlik Yönetimi penceresi açılırken hata");
            }
        }
        private void BtnSanatci_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SanatciYonetimiView(_admin) { Owner = this }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),
                                "Sanatçı Yönetimi penceresi açılırken hata");
            }
        }
        private void BtnPersonel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new PersonelYonetimiView(_admin) { Owner = this }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),
                                "Personel Yönetimi penceresi açılırken hata");
            }
        }

        private void BtnGelir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new GelirGiderYonetimiView(_admin) { Owner = this }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),
                                "Gelir-Gider Yönetimi penceresi açılırken hata");
            }
            
        }

        

        private void BtnSanatAkim_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SanatAkimiYonetimiView (_admin) { Owner = this }.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Sanat Akımı Yönetimi penceresi açılırken hata");
            }
        }

        private void BtnZiyaretci_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new ZiyaretciYonetimiView(_admin) { Owner = this }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),
                                "Ziyaretçi Yönetimi penceresi açılırken hata");
            }

         }

        private void BtnEserBakim_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new EserBakimYonetimiView(_admin) { Owner = this }.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),
                                "Eser Bakim Yönetimi penceresi açılırken hata");
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblEserSayisi.Content = GetCount("Eserler");
            lblBagisSayisi.Content = GetCount("Bagislar");
            lblSergiSayisi.Content = GetCount("Sergiler");
            lblKullanici.Content = $"Hoş geldiniz, {_admin.Ad} ({_admin.YetkiSeviyesi})";

            switch (_admin.YetkiSeviyesi)
            {
                case "Tam Yetki":
                    // Admin her şeye erişebilir
                    break;

                case "Yönetici":
                    // Manager → INSERT var, ama DELETE yok
                    BtnPersonel.IsEnabled = false; // Silme varsa kapat
                    BtnGelir.IsEnabled = false;    // örnek sınırlama
                    break;

                case "Sınırlı":
                    // Employee → Sadece okuma
                   

                    BtnPersonel.IsEnabled = false;
                    BtnGelir.IsEnabled = false;
                    BtnSanatAkim.IsEnabled = false;
                    BtnZiyaretci.IsEnabled = false;
                    break;
            }
        }

        private int GetCount(string tableName)
        {
            int count = 0;
            string connectionString = "Server=DESKTOP-1LQQS16\\SQLDEVELOPER;Database=Museum;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 🔐 AdminID'yi session context'e ayarla
                using (SqlCommand setSessionCmd = new SqlCommand("EXEC sp_set_session_context @key, @value", conn))
                {
                    setSessionCmd.Parameters.AddWithValue("@key", "AdminID");
                    setSessionCmd.Parameters.AddWithValue("@value", _admin.ID);  // _admin.ID doğruysa kullan
                    setSessionCmd.ExecuteNonQuery();
                }

                // 🔢 Sayı çek
                string query = $"SELECT COUNT(*) FROM {tableName}";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    count = (int)cmd.ExecuteScalar();
                }
            }

            return count;
        }


        // İleride gerekirse kullanacağınız boş buton
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO: başka bir modül açılacak
        }
    }
}
