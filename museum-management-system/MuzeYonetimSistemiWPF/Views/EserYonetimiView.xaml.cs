using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using MuzeYonetimSistemiWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public partial class EserYonetimiView : Window
    {

        // ─── Hizmetler ────────────────────────────────────────────────
        private readonly EserService _eserService = new();
        private readonly EserTurleriService _turService = new();

        // ─── ViewModel ────────────────────────────────────────────────
        private readonly EserViewModel _viewModel;
        private readonly Admin _admin;
        public EserYonetimiView(Admin admin)
        {
            InitializeComponent();

            _viewModel = new EserViewModel();
            DataContext = _viewModel;
            _admin = admin;

            Loaded += EserYonetimiView_Loaded;
            Loaded += Window_Loaded;          // Sayfa açılınca verileri yükle
        }

        // ░░░ SAYFA İLK AÇILIŞ ░░░
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTurCombo();   // tür listesi
            RefreshList();    // tüm eserler
        }

        // ░░░ COMBOBOX’I DOLDUR ░░░
        private void LoadTurCombo()
        {
            cbTurFiltre.Items.Clear();
            cbTurFiltre.Items.Add("Tümü");                    // ilk satır
            foreach (var tur in _turService.GetAllEserTurleri())
                cbTurFiltre.Items.Add(tur);                   // Ad + ID
           
            cbTurFiltre.SelectedIndex = 0;
        }

        // ░░░ LİSTEYİ YENİLE ░░░
        private void RefreshList()
        {
            var eserler = _eserService.GetAllEserler();

            // “Tümü” hariç seçim varsa filtrele
            if (cbTurFiltre.SelectedItem is EserTurleri seciliTur)
                eserler = eserler.Where(e => e.Tur_ID == seciliTur.ID).ToList();

            _viewModel.Eserler = new ObservableCollection<Eser>(eserler);
            dgEserler.ItemsSource = _viewModel.Eserler;
        }

        // ░░░ FİLTRE DEĞİŞTİ ░░░
        private void cbTurFiltre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;   // pencere tam yüklenmeden tetiklenmesin
            RefreshList();
        }

        // ░░░ TEMİZLE ░░░
        private void BtnFiltreyiTemizle_Click(object sender, RoutedEventArgs e)
        {
            cbTurFiltre.SelectedIndex = 0;   // “Tümü”
            RefreshList();
        }

        // ░░░ YENİ ESER EKLE ░░░
        private void BtnYeniEkle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                MessageBox.Show("Bu işlemi yapma yetkiniz yok (Sınırlı kullanıcı).");
                return;
            }


            if (string.IsNullOrWhiteSpace(txtTurID.Text.Trim()) ||
    string.IsNullOrWhiteSpace(txtSanatciID.Text.Trim()) ||
    string.IsNullOrWhiteSpace(txtYapimYili.Text.Trim()))
            {
                MessageBox.Show("Lütfen Tür ID, Sanatçı ID ve Yapım Yılı alanlarını doldurun.", "Eksik Bilgi");
                return;
            }

            if (!int.TryParse(txtTurID.Text.Trim(), out int turId) ||
                !int.TryParse(txtSanatciID.Text.Trim(), out int sanatciId) ||
                !int.TryParse(txtYapimYili.Text.Trim(), out int yapimYili))
            {
                MessageBox.Show("Tür ID, Sanatçı ID ve Yapım Yılı sayısal olmalıdır.", "Geçersiz Giriş");
                return;
            }

            try
            {
                var yeni = new Eser
                {
                    Ad = txtAd.Text,
                    Tur_ID = turId,
                    Sanatci_ID = sanatciId,
                    YapimYili = yapimYili,
                    BulunduguMuze = txtMuze.Text,
                    MevcutDurum = txtDurum.Text,
                    DijitalKoleksiyonURL = txtURL.Text
                };

                int newId = _eserService.AddWithSP(yeni);
                yeni.ID = newId;

                _viewModel.Eserler.Add(yeni);
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eser eklenirken hata: " + ex.Message);
            }
        }




        // ░░░ GÜNCELLE ░░░
        private void BtnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Sadece yöneticiler bu işlemi yapabilir.");
                return;
            }

            if (dgEserler.SelectedItem is not Eser secilen)
            {
                MessageBox.Show("Lütfen güncellenecek bir eser seçin.", "Uyarı",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                secilen.Ad = txtAd.Text;
                secilen.Tur_ID = int.Parse(txtTurID.Text);
                secilen.Sanatci_ID = int.Parse(txtSanatciID.Text);
                secilen.YapimYili = int.Parse(txtYapimYili.Text);
                secilen.BulunduguMuze = txtMuze.Text;
                secilen.MevcutDurum = txtDurum.Text;
                secilen.DijitalKoleksiyonURL = txtURL.Text;

                _eserService.Update(secilen);
                RefreshList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası: " + ex.Message);
            }
        }

        // ░░░ SİL ░░░
        private void BtnSil_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Sadece yöneticiler bu işlemi yapabilir.");
                return;
            }

            if (dgEserler.SelectedItem is not Eser secilen)
            {
                MessageBox.Show("Lütfen silinecek bir eser seçin.", "Uyarı",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Silmek istiyor musunuz?", "Onay",
                                MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes)
                return;

            _eserService.Delete(secilen.ID);
            ClearInputs();
            RefreshList();
        }

        // ░░░ SATIR SEÇİLİNCE KUTULARI DOLDUR ░░░
        private void dgEserler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEserler.SelectedItem is not Eser secilen) return;

            txtAd.Text = secilen.Ad;
            txtTurID.Text = secilen.Tur_ID.ToString();
            txtSanatciID.Text = secilen.Sanatci_ID.ToString();
            txtYapimYili.Text = secilen.YapimYili.ToString();
            txtMuze.Text = secilen.BulunduguMuze;
            txtDurum.Text = secilen.MevcutDurum;
            txtURL.Text = secilen.DijitalKoleksiyonURL;
        }

        // ░░░ FORM TEMİZLE ░░░
        private void ClearInputs()
        {
            txtAd.Clear();
            txtTurID.Clear();
            txtSanatciID.Clear();
            txtYapimYili.Clear();
            txtMuze.Clear();
            txtDurum.Clear();
            txtURL.Clear();
        }

        private void EserYonetimiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                BtnEkle.IsEnabled = false;
                BtnGuncelle.IsEnabled = false;
                BtnSil.Visibility = Visibility.Collapsed;
            }
            else if (_admin.YetkiSeviyesi == "Yönetici")
            {
                BtnGuncelle.IsEnabled = false;
                BtnSil.Visibility = Visibility.Collapsed;
            }
        }
    }
}