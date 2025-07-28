using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System;
using System.Collections.Generic;
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


namespace MuzeYonetimSistemiWPF.Views
{
    /// <summary>
    /// LoginWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class LoginWindow : Window
    {
        private AdminService _adminService = new AdminService();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnGirisYap_Click(object sender, RoutedEventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Password;

            // Şifre hashleniyorsa burada hash fonksiyonunu kullan:
            // string sifreHash = SifreHasher.Hash(sifre);
            string sifreHash = sifre; // geçici olarak hash yoksa düz yazıyoruz

            Admin girisYapan = _adminService.GirisYap(kullaniciAdi, sifreHash);

            if (girisYapan != null)
            {
                MessageBox.Show($"Giriş başarılı! Yetki: {girisYapan.YetkiSeviyesi}");

                // Yetkiye göre yönlendirme
                if (girisYapan.YetkiSeviyesi == "Tam Yetki")
                {
                    var panel = new AdminDashboardView(girisYapan);
                    panel.Show();
                }
                else if (girisYapan.YetkiSeviyesi == "Sınırlı")
                {
                    var panel = new AdminDashboardView(girisYapan);
                    panel.Show();
                }

                this.Close(); // Login ekranı kapanır
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre.");
            }
        }
    }
}
