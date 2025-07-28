using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using MuzeYonetimSistemiWPF.ViewModels;

namespace MuzeYonetimSistemiWPF.Views
{
    
    public partial class SergiYonetimiView : Window
    {
        private readonly SergilerService _sergiService = new();
        private readonly EserService _eserService = new();
        private readonly EserSergileriService _eserSergileriService = new();

        // ─────────────── Koleksiyonlar
        public ObservableCollection<Sergi> Sergiler { get; } = new();
        public ObservableCollection<Eser> Eserler { get; } = new();
        public ObservableCollection<SergiEserVM> SergiEserler { get; } = new();

        private ICollectionView _sergilerView;

        private readonly Admin _admin;

        public SergiYonetimiView(Admin admin)
        {
            InitializeComponent();

            // 1) Ana listeler
            foreach (var s in _sergiService.GetAllSergiler()) Sergiler.Add(s);
            foreach (var e in _eserService.GetAllEserler()) Eserler.Add(e);

            // 2) Mevcut Sergiler sekmesi
            _sergilerView = CollectionViewSource.GetDefaultView(Sergiler);
            dgSergiler.ItemsSource = _sergilerView;

            // 3) “Sergiye Eser Ekle” combobox’ları
            cbSergiSecAdd.ItemsSource = Sergiler;
            cbEserSecAdd.ItemsSource = Eserler;

            // 4) “Sergi-Eser Görüntüleme” grid’i
            dgSergiEserler.ItemsSource = SergiEserler;

            cbSergiSecView.ItemsSource = Sergiler;

            if (Sergiler.Count > 0)
            {
                cbSergiSecView.SelectedItem = Sergiler.First();
                cbSergiSecView_SelectionChanged(cbSergiSecView, null);
            }
            _admin = admin;
            Loaded += SergiYonetimiView_Loaded;

        }

        private void SergiYonetimiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                btnSergiEkle.IsEnabled = false;
                BtnSergiSil.Visibility = Visibility.Collapsed;
                BtnSergiGuncelle.IsEnabled = false;

                BtnEserSergiyeEkle.IsEnabled = false;
                BtnEserSergiSil.Visibility = Visibility.Collapsed;
                BtnEserSergiGuncelle.IsEnabled = false;
            }
            else if (_admin.YetkiSeviyesi == "Yönetici")
            {
                BtnSergiSil.Visibility = Visibility.Collapsed;
                BtnEserSergiSil.Visibility = Visibility.Collapsed;
            }
        }


        // ───────────────────────── Mevcut Sergiler – CRUD
        private void btnSergiEkle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                MessageBox.Show("Bu işlemi yapma yetkiniz yok (Sınırlı kullanıcı).");
                return;
            }

            try
            {
                // 1. Yeni sergi nesnesi oluştur
                var yeniSergi = new Sergi
                {
                    Ad = txtSergiAd.Text,
                    Konum = txtKonum.Text,
                    BaslangicTarihi = dpBaslangic.SelectedDate ?? DateTime.Now,
                    BitisTarihi = dpBitis.SelectedDate ?? DateTime.Now,
                    Aciklama = txtAciklama.Text
                };

                // 2. SP ile veritabanına ekle ve ID al
                var sergiService = new SergilerService();
                int yeniId = sergiService.AddWithSP(yeniSergi);
                yeniSergi.ID = yeniId;

                // 3. DataGrid'e güncel veriyi ekle
                Sergiler.Add(yeniSergi); // ObservableCollection<Sergi> olmalı

                // 4. Alanları temizle (isteğe bağlı)
                txtSergiAd.Clear();
                txtKonum.Clear();
                txtAciklama.Clear();
                dpBaslangic.SelectedDate = null;
                dpBitis.SelectedDate = null;

                MessageBox.Show("Sergi başarıyla eklendi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSergiSil_Click(object sender, RoutedEventArgs e)
        {
            if (dgSergiler.SelectedItem is not Sergi secilen) return;

            if (MessageBox.Show("Seçilen sergiyi silmek istediğinize emin misiniz?",
                                "Onay", MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes) return;

            _sergiService.Delete(secilen.ID);
            Sergiler.Remove(secilen);
        }

        private void BtnSergiGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (dgSergiler.SelectedItem is not Sergi secilen) return;

            secilen.Ad += " (Güncellendi)";
            _sergiService.Update(secilen);
            _sergilerView.Refresh();
        }

       

        private void BtnEserSergiSil_Click(object sender, RoutedEventArgs e)
        {
            if (dgSergiEserler.SelectedItem is not SergiEserVM secilen) return;

            try
            {
                (_eserSergileriService as dynamic).Delete(secilen.MappingID);
                SergiEserler.Remove(secilen);
            }
            catch
            {
                MessageBox.Show("Silme işlemi için EserSergileriService.Delete(int id) "
                              + "bulunamadı.", "Bilgi",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnEserSergiGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (dgSergiEserler.SelectedItem is not SergiEserVM secilen) return;

            var eser = Eserler.FirstOrDefault(e => e.ID == secilen.ID);
            if (eser == null) return;

            eser.Ad += " (Güncellendi)";

            try { _eserService.Update(eser); }  // varsa çalışır, yoksa yoksay
            catch { }

            secilen.Ad = eser.Ad;
            dgSergiEserler.Items.Refresh();
        }

        // ───────────────────────── Sergiye Eser Ekle
        private void BtnEserSergiyeEkle_Click(object sender, RoutedEventArgs e)
        {
            if (cbSergiSecAdd.SelectedItem is not Sergi sergi ||
                cbEserSecAdd.SelectedItem is not Eser eser)
            {
                MessageBox.Show("Lütfen sergi ve eseri seçin.", "Uyarı",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var yeni = new EserSergi
            {
                SergiID = sergi.ID,
                EserID = eser.ID,
                BaslangicTarihi = DateTime.Today,
                BitisTarihi = DateTime.Today.AddDays(30)
            };

            _eserSergileriService.Add(yeni);
            MessageBox.Show("Eser sergiye eklendi.", "Bilgi",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            
        }

        private void cbSergiSecView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSergiSecView.SelectedItem is not Sergi secilenSergi)
                return;

            SergiEserler.Clear();

            var eserler = _eserSergileriService.GetEserlerBySergiID(secilenSergi.ID);
            foreach (var eser in eserler)
            {
                SergiEserler.Add(new SergiEserVM
                {
                    MappingID = eser.MappingID,
                    ID = eser.ID,
                    Ad = eser.Ad,
                    Sanatci = eser.SanatciAd
                });
            }
        }

        private void BtnTemizleSergiSecView_Click(object sender, RoutedEventArgs e)
        {
            cbSergiSecView.SelectedIndex = -1;
            SergiEserler.Clear();
        }

        // ───────────────────────── Küçük view-model
        public class SergiEserVM : INotifyPropertyChanged
        {
            public int MappingID { get; set; }
            public int ID { get; set; }

            private string _ad;
            public string Ad
            {
                get => _ad;
                set { _ad = value; OnPropertyChanged(nameof(Ad)); }
            }

            public string Sanatci { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
            private void OnPropertyChanged(string prop) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}