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
    /// <summary>
    /// SanatAkimiYonetimi.xaml etkileşim mantığı
    /// </summary>
    public partial class SanatAkimiYonetimiView : Window
    {
        private readonly SanatAkimiService _akimService = new();
        private readonly SanatciAkimService _sanatciAkimService = new();

        public ObservableCollection<SanatAkimi> Akimlar { get; } = new();

        // ─── Koleksiyon ───────────────────────────────────────────
        public ObservableCollection<SanatAkimi> Akim { get; } = new();

        private readonly Admin _admin;

        public SanatAkimiYonetimiView(Admin admin)
        {
            InitializeComponent();
            _admin = admin;

            foreach (var a in _akimService.GetAllSanatAkimi())
                Akimlar.Add(a);

            dgAkimlar.ItemsSource = Akimlar;

            Loaded += SanatAkimiYonetimiView_Loaded;
        }
        private void SanatAkimiYonetimiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                btnAkimEkle.IsEnabled = false;
                btnAkimSil.Visibility = Visibility.Collapsed;
                btnSanatciAkimEkle.IsEnabled = false;
                btnSanatciAkimSil.Visibility = Visibility.Collapsed;
            }
            else if (_admin.YetkiSeviyesi == "Yönetici")
            {
                btnAkimSil.Visibility = Visibility.Collapsed;
                btnSanatciAkimSil.Visibility = Visibility.Collapsed;
            }
        }


        private void btnAkimEkle_Click(object sender, RoutedEventArgs e)
        {
            string ad = txtAkimAd.Text.Trim();
            string aciklama = txtAkimAciklama.Text.Trim();

            if (string.IsNullOrWhiteSpace(ad))
            {
                MessageBox.Show("Lütfen bir sanat akımı adı girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var yeni = new SanatAkimi { Ad = ad, Aciklama = aciklama };

            // ✅ ID’yi geri al ve ata
            yeni.ID = _akimService.Add(yeni);
            Akimlar.Add(yeni);

            txtAkimAd.Clear();
            txtAkimAciklama.Clear();
        }


        // ░░░ AKIM SİL ░░░
        private void btnAkimSil_Click(object sender, RoutedEventArgs e)
        {
            if (dgAkimlar.SelectedItem is not SanatAkimi secili)
            {
                MessageBox.Show("Lütfen silinecek bir akım seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show($"“{secili.Ad}” akımını silmek istiyor musunuz?", "Onay", MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes) return;

            _akimService.DeleteSanatAkimi(secili.ID);
            Akimlar.Remove(secili);
        }

        // ░░░ DATAGRID DÜZENLEME ░░░
        private void dgAkimlar_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            if (e.Row.Item is not SanatAkimi akim) return;

            if (akim.ID == 0)
            {
                _akimService.AddSanatAkimi(akim);
            }
            else
            {
                _akimService.UpdateSanatAkimi(akim);
            }
        }

        // ░░░ SANATÇIYA AKIM EKLE ░░░
        private void btnSanatciAkimEkle_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtSanatciID.Text.Trim(), out int sanatciId) ||
                !int.TryParse(txtAkimID.Text.Trim(), out int akimId))
            {
                MessageBox.Show("Lütfen geçerli bir Sanatçı ID ve Akım ID girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _sanatciAkimService.AddSanatciAkim(sanatciId, akimId);
            MessageBox.Show("Sanatçıya akım başarıyla eklendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

            txtSanatciID.Clear();
            txtAkimID.Clear();
        }

        // ░░░ SANATÇI-AKIM SİL ░░░
        private void btnSanatciAkimSil_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtSanatciID.Text.Trim(), out int sanatciId) ||
                !int.TryParse(txtAkimID.Text.Trim(), out int akimId))
            {
                MessageBox.Show("Lütfen geçerli bir Sanatçı ID ve Akım ID girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _sanatciAkimService.DeleteSanatciAkim(sanatciId, akimId);
            MessageBox.Show("İlişki silindi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

            txtSanatciID.Clear();
            txtAkimID.Clear();
        }
    }
}