using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
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
    /// <summary>
    /// SanatciView.xaml etkileşim mantığı
    /// </summary>
    public partial class SanatciYonetimiView : Window
    {
        private readonly SanatcilarService _sanatciService = new();
        private readonly SanatciAkimService _sanatciAkimService = new();   // ilişkiler için

        // ─── Koleksiyonlar ────────────────────────────────────────
        public ObservableCollection<Sanatci> Sanatcilar { get; } = new();
        public ObservableCollection<SanatciAkim> SanatciAkimlari { get; } = new();

        private readonly Admin _admin;

        public SanatciYonetimiView(Admin admin)
        {
            InitializeComponent();
            _admin = admin;

            foreach (var s in _sanatciService.GetAllSanatcilar())
                Sanatcilar.Add(s);
            dgSanatcilar.ItemsSource = Sanatcilar;

            RefreshRelationshipGrid();
            dgSanatciAkim.ItemsSource = SanatciAkimlari;

            cbUlkeFiltre.ItemsSource = Sanatcilar.Select(s => s.Ulke).Distinct().ToList();
            cbUlkeFiltre.SelectedIndex = -1;

            Loaded += SanatciYonetimiView_Loaded;
        }
        private void SanatciYonetimiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                BtnSanatciEkle.IsEnabled = false;
                BtnSanatciSil.Visibility = Visibility.Collapsed;
                BtnAkimSil.Visibility = Visibility.Collapsed;
            }
            else if (_admin.YetkiSeviyesi == "Yönetici")
            {
                BtnSanatciSil.Visibility = Visibility.Collapsed;
                BtnAkimSil.Visibility = Visibility.Collapsed;
            }
        }


        // ░░░ YENİ SANATÇI EKLE ░░░
        private void BtnEkle_Click(object sender, RoutedEventArgs e)
        {
            var ad = txtSanatciAd.Text.Trim();
            if (string.IsNullOrWhiteSpace(ad))
            {
                MessageBox.Show("Lütfen sanatçı adını girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var yeni = new Sanatci
            {
                Ad = ad,
                DogumTarihi = dpDogumTarihi.SelectedDate,
                OlumTarihi = dpOlumTarihi.SelectedDate,
                Ulke = txtUlke.Text.Trim(),
                Biyografi = txtBiyografi.Text.Trim()
            };

            // ✅ ID’yi SP’den al
            yeni.ID = _sanatciService.Add(yeni);
            Sanatcilar.Add(yeni);

            // Temizle
            txtSanatciAd.Clear();
            dpDogumTarihi.SelectedDate = null;
            dpOlumTarihi.SelectedDate = null;
            txtUlke.Clear();
            txtBiyografi.Clear();
        }


        // ░░░ GÜNCELLE ░░░
        private void BtnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (dgSanatcilar.SelectedItem is not Sanatci secili)
            {
                MessageBox.Show("Lütfen güncellenecek bir sanatçı seçin.", "Uyarı",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _sanatciService.Update(secili);
            dgSanatcilar.Items.Refresh();     // anlık yansıt
            RefreshRelationshipGrid();        // olası ad değişiklerini ilişki listesine yansıt
        }

        // ░░░ SİL ░░░
        private void BtnSil_Click(object sender, RoutedEventArgs e)
        {
            if (dgSanatcilar.SelectedItem is not Sanatci secili)
            {
                MessageBox.Show("Lütfen silinecek bir sanatçı seçin.", "Uyarı",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show($"“{secili.Ad}” sanatçısını silmek istiyor musunuz?",
                                "Onay", MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes) return;

            _sanatciService.Delete(secili.ID);
            Sanatcilar.Remove(secili);
            RefreshRelationshipGrid();

            
        }

        // ░░░ SATIR-İÇİ DÜZENLEME (DataGrid) ░░░
        private void dgSanatcilar_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            if (e.Row.Item is not Sanatci sanatci) return;

            if (sanatci.ID == 0)            // yeni satır
            {
                _sanatciService.Add(sanatci);
            }
            else                            // mevcut satır
            {
                _sanatciService.Update(sanatci);
            }

            RefreshRelationshipGrid();
        }

        private void cbUlkeFiltre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbUlkeFiltre.SelectedItem is not string secilenUlke) return;

            dgSanatcilar.ItemsSource = Sanatcilar
                .Where(s => s.Ulke == secilenUlke)
                .ToList();
        }

        private void BtnTemizleFiltre_Click(object sender, RoutedEventArgs e)
        {
            cbUlkeFiltre.SelectedIndex = -1;
            dgSanatcilar.ItemsSource = Sanatcilar;
        }

        private void cbAkimFiltre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbAkimFiltre.SelectedItem is string secilenAkim && !string.IsNullOrWhiteSpace(secilenAkim))
            {
                dgSanatciAkim.ItemsSource = SanatciAkimlari
                    .Where(x => x.AkimAd == secilenAkim)
                    .ToList();
            }
        }

        private void BtnTemizleAkimFiltre_Click(object sender, RoutedEventArgs e)
        {
            cbAkimFiltre.SelectedIndex = -1;
            dgSanatciAkim.ItemsSource = SanatciAkimlari;
        }

        private void RefreshRelationshipGrid()
        {
            SanatciAkimlari.Clear();
            foreach (var rel in _sanatciAkimService.GetAllSanatciAkim())
                SanatciAkimlari.Add(rel);

            // Akım filtre listesini de güncelle
            cbAkimFiltre.ItemsSource = SanatciAkimlari.Select(x => x.AkimAd).Distinct().ToList();
        }

        private void BtnAkimGuncelle_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Akım güncelleme işlemi bu sürümde desteklenmiyor.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAkimSil_Click(object sender, RoutedEventArgs e)
        {
            if (dgSanatciAkim.SelectedItem is not SanatciAkim secili)
            {
                MessageBox.Show("Lütfen silinecek bir kayıt seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _sanatciAkimService.Delete(secili.SanatciID, secili.AkimID);
            RefreshRelationshipGrid();
        }
    }
}