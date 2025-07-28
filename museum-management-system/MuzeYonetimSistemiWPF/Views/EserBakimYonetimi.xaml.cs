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
    /// EserBakımYonetimi.xaml etkileşim mantığı
    /// </summary>
    public partial class EserBakimYonetimiView : Window
    {
        /* ── Servis ───────────────────────────────────────────── */
        private readonly EserBakimKaydiService _bakimSrv = new();

        /* ── Koleksiyon (UI Binding) ─────────────────────────── */
        public ObservableCollection<EserBakimKaydi> Bakimlar { get; } = new();

        private readonly Admin _admin;
        public EserBakimYonetimiView(Admin admin)
        {
            InitializeComponent();

            foreach (var b in _bakimSrv.GetAllEserBakimKaydi())
                Bakimlar.Add(b);

            dgBakim.ItemsSource = Bakimlar;
            _admin = admin;

            Loaded += EserBakimYonetimiView_Loaded;
           
        }

        

        /* ===============  BAKIM  =============================== */

        private void BtnBakimEkle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                MessageBox.Show("Bu işlemi yapma yetkiniz yok.");
                return;
            }

            if (!int.TryParse(txtBakimEserID.Text, out int eserId) ||
                !int.TryParse(txtBakimPersonelID.Text, out int perId) ||
                !dpBakimTarih.SelectedDate.HasValue)
            {
                MessageBox.Show("Alanları kontrol edin.");
                return;
            }

            var b = new EserBakimKaydi
            {
                EserID = eserId,
                PersonelID = perId,
                BakimTarihi = dpBakimTarih.SelectedDate.Value,
                YapilanIslem = txtIslem.Text.Trim(),
                
            };

            _bakimSrv.AddWithSP(b); // <-- SP ile ekleme işlemi

            Bakimlar.Add(b); // UI güncellemesi

            // Formu temizle
            txtBakimEserID.Clear();
            txtBakimPersonelID.Clear();
            txtIslem.Clear();
            dpBakimTarih.SelectedDate = null;
        }


        private void BtnBakimGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Sadece yöneticiler bu işlemi yapabilir.");
                return;
            }

            if (dgBakim.SelectedItem is not EserBakimKaydi b)
            {
                MessageBox.Show("Seçim yapın.");
                return;
            }

            _bakimSrv.Update(b);
            dgBakim.Items.Refresh();
        }

        private void BtnBakimSil_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Sadece yöneticiler bu işlemi yapabilir.");
                return;
            }

            if (dgBakim.SelectedItem is not EserBakimKaydi b)
            {
                MessageBox.Show("Seçim yapın.");
                return;
            }

            if (MessageBox.Show("Silinsin mi?", "Onay",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

            _bakimSrv.Delete(b.ID);
            Bakimlar.Remove(b);
        }

        private void dgBakim_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            if (e.Row.Item is not EserBakimKaydi b) return;

            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem sadece Admin tarafından yapılabilir.");
                return;
            }

            if (b.ID == 0)
                _bakimSrv.Add(b);
            else
                _bakimSrv.Update(b);
        }
        private void EserBakimYonetimiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                BtnBakimEkle.IsEnabled = false;
                BtnBakimGuncelle.IsEnabled = false;
                BtnBakimSil.Visibility = Visibility.Collapsed;
            }
            else if (_admin.YetkiSeviyesi == "Yönetici")
            {
                BtnBakimGuncelle.IsEnabled = false;
                BtnBakimSil.Visibility = Visibility.Collapsed;
            }
        }
    }
}
