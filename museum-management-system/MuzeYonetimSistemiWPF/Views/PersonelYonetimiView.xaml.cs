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
    /// PersonelYonetimiView.xaml etkileşim mantığı
    /// </summary>
    public partial class PersonelYonetimiView : Window
    {
        private readonly PersonelService _personelService = new();

        // ─── Koleksiyonlar ───────────────────────────────────────
        public ObservableCollection<Personel> Personeller { get; } = new();

        private readonly Admin _admin;

        public PersonelYonetimiView(Admin admin)
        {
            InitializeComponent();
            _admin = admin;

            foreach (var p in _personelService.GetAllPersonel())
                Personeller.Add(p);
            dgPersonel.ItemsSource = Personeller;

            Loaded += PersonelYonetimiView_Loaded;
        }
        private void PersonelYonetimiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                BtnPerEkle.IsEnabled = false;
                BtnPerGuncelle.IsEnabled = false;
                BtnPerSil.Visibility = Visibility.Collapsed;
            }
        }


        private void BtnPerEkle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtSoyad.Text))
            { MessageBox.Show("Ad ve Soyad zorunludur."); return; }

            if (!decimal.TryParse(txtMaas.Text, out var maas))
            { MessageBox.Show("Maaş sayısal olmalıdır."); return; }

            if (!DateTime.TryParse(txtBaslama.Text, out var baslama))
            { MessageBox.Show("Başlama tarihi geçersiz."); return; }

            var yeni = new Personel
            {
                Ad = txtAd.Text.Trim(),
                Soyad = txtSoyad.Text.Trim(),
                Gorev = txtGorev.Text.Trim(),
                Maas = maas,
                IseBaslamaTarihi = baslama,
                DogumTarihi = dpDogum.SelectedDate.Value
            };

            yeni.ID = _personelService.Add(yeni);  // ✅ ID atanmış olacak
            Personeller.Add(yeni);


            txtAd.Clear(); txtSoyad.Clear(); txtGorev.Clear();
            txtMaas.Clear(); txtBaslama.Clear();
        }

        private void BtnPerGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            if (dgPersonel.SelectedItem is not Personel secili)
            { MessageBox.Show("Güncellenecek personeli seçin."); return; }

            _personelService.Update(secili);
            dgPersonel.Items.Refresh();
        }

        private void BtnPerSil_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            if (dgPersonel.SelectedItem is not Personel secili)
            { MessageBox.Show("Silinecek personeli seçin."); return; }

            if (MessageBox.Show($"“{secili.Ad} {secili.Soyad}” silinsin mi?",
                                "Onay", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

            _personelService.Delete(secili.ID);
            Personeller.Remove(secili);
        }

        private void dgPersonel_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            if (e.Row.Item is not Personel p) return;

            if (p.ID == 0) _personelService.Add(p);
            else _personelService.Update(p);
        }
    }
}