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
    /// ZiyaretciView.xaml etkileşim mantığı
    /// </summary>
    public partial class ZiyaretciYonetimiView : Window
    {
        private readonly ZiyaretciService _ziySrv = new();

        public ObservableCollection<Ziyaretci> Ziyaretciler { get; } = new();

        private readonly Admin _admin;

        public ZiyaretciYonetimiView(Admin admin)
        {
            InitializeComponent();
            _admin = admin;

            foreach (var z in _ziySrv.GetAllZiyaretci())
                Ziyaretciler.Add(z);

            dgZiyaretci.ItemsSource = Ziyaretciler;

            Loaded += ZiyaretciYonetimiView_Loaded;
        }

        private void ZiyaretciYonetimiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                BtnZiyEkle.IsEnabled = false;
                BtnZiyGuncelle.IsEnabled = false;
                BtnZiySil.Visibility = Visibility.Collapsed;
            }
            else if (_admin.YetkiSeviyesi == "Yönetici")
            {
                BtnZiySil.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnZiyEkle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                MessageBox.Show("Bu işlemi yapma yetkiniz yok (Sınırlı kullanıcı).");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtSoyad.Text))
            {
                MessageBox.Show("Ad ve Soyad zorunludur.");
                return;
            }

            var z = new Ziyaretci
            {
                Ad = txtAd.Text.Trim(),
                Soyad = txtSoyad.Text.Trim(),
                DogumTarihi = dpDogumTarihi.SelectedDate ?? DateTime.Today,
                Email = txtEmail.Text.Trim(),
                UyelikDurumu = chkUyelik.IsChecked == true
            };

            _ziySrv.Add(z);
            Ziyaretciler.Add(z);

            txtAd.Clear();
            txtSoyad.Clear();
            txtEmail.Clear();
            chkUyelik.IsChecked = false;
            dpDogumTarihi.SelectedDate = null;
        }

        private void BtnZiyGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                MessageBox.Show("Bu işlemi yapma yetkiniz yok (Sınırlı kullanıcı).");
                return;
            }
            if (dgZiyaretci.SelectedItem is not Ziyaretci z)
            {
                MessageBox.Show("Seçim yapın");
                return;
            }

            _ziySrv.Update(z);
            dgZiyaretci.Items.Refresh();
        }

        private void BtnZiySil_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                MessageBox.Show("Bu işlemi yapma yetkiniz yok (Sınırlı kullanıcı).");
                return;
            }
            if (dgZiyaretci.SelectedItem is not Ziyaretci z)
            {
                MessageBox.Show("Seçim yapın");
                return;
            }

            if (MessageBox.Show("Silinsin mi?", "Onay", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            _ziySrv.Delete(z.ID);
            Ziyaretciler.Remove(z);
        }

        private void dgZiyaretci_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            if (e.Row.Item is not Ziyaretci z) return;

            if (z.ID == 0)
                _ziySrv.Add(z);
            else
                _ziySrv.Update(z);
        }

        private void dgZiyaretci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgZiyaretci.SelectedItem is Ziyaretci z)
            {
                MessageBox.Show("Seçilen ziyaretçi ID: " + z.ID);
            }
        }
    }
}