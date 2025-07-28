using LiveCharts.Wpf;
using LiveCharts;
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
    /// GelirGiderView.xaml etkileşim mantığı
    /// </summary>
    public partial class GelirGiderYonetimiView : Window
    {
        // ── Servisler ─────────────────────────────────────────────
        private readonly MuzeGeliriService _gelirSrv = new();
        private readonly MuzeGideriService _giderSrv = new();

        // ── Koleksiyonlar (ekranda gösterilen) ───────────────────
        public ObservableCollection<MuzeGeliri> Gelirler { get; } = new();
        public ObservableCollection<MuzeGideri> Giderler { get; } = new();
        private SeriesCollection ChartSeries { get; set; }

        private readonly Admin _admin;

        public GelirGiderYonetimiView(Admin admin)
        {
            InitializeComponent();

            _admin = admin;

            SetupChart();

            foreach (var g in _gelirSrv.GetAllMuzeGeliri()) Gelirler.Add(g);
            foreach (var g in _giderSrv.GetAllMuzeGideri()) Giderler.Add(g);

            dgGelir.ItemsSource = Gelirler;
            dgGider.ItemsSource = Giderler;

            UpdateSummary();

            Loaded += GelirGiderYonetimiView_Loaded;
        }

        private void GelirGiderYonetimiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                // GELİR
                BtnGelirEkle.IsEnabled = false;
                BtnGelirGuncelle.IsEnabled = false;
                BtnGelirSil.Visibility = Visibility.Collapsed;

                // GİDER
                BtnGiderEkle.IsEnabled = false;
                BtnGiderGuncelle.IsEnabled = false;
                BtnGiderSil.Visibility = Visibility.Collapsed;

                // Opsiyonel: grafikler vs. erişime açık kalabilir
            }
        }

        // ╔════════════════ GELİR ════════════════╗
        private void BtnGelirEkle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            try
            {
                var culture = System.Globalization.CultureInfo.GetCultureInfo("tr-TR");

                string tutarStr = txtGelirTutar.Text.Trim().Replace(",", ".");
                if (string.IsNullOrWhiteSpace(txtGelirKaynak.Text)
                    || !decimal.TryParse(tutarStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var tutar))
                {
                    MessageBox.Show($"Parse edilemedi: {tutarStr}");
                    return;
                }


                var g = new MuzeGeliri
                {
                    KaynakTuru = txtGelirKaynak.Text.Trim(),
                    Tutar = tutar,
                    Tarih = dpGelirTarih.SelectedDate ?? DateTime.Today
                };

                
                g.ID = _gelirSrv.Add(g);
                Gelirler.Add(g);


                txtGelirKaynak.Clear();
                txtGelirTutar.Clear();
                dpGelirTarih.SelectedDate = null;

                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu:\n" + ex.Message, "HATA", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        

   

private void BtnGelirFiltre_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            Gelirler.Clear();
            var basla = dpGelirBasla.SelectedDate ?? DateTime.MinValue;
            var bit = dpGelirBit.SelectedDate ?? DateTime.MaxValue;

            foreach (var g in _gelirSrv.GetAllMuzeGeliri()
                         .Where(x => x.Tarih >= basla && x.Tarih <= bit))
                Gelirler.Add(g);

            UpdateSummary();
        }

        private void BtnGelirGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            if (dgGelir.SelectedItem is MuzeGeliri secilen)
            {
                _gelirSrv.AddOrUpdate(secilen);
                MessageBox.Show("Gelir güncellendi.");
                UpdateSummary();
            }
        }

        // GELİR – Sil
        private void BtnGelirSil_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            if (dgGelir.SelectedItem is MuzeGeliri secilen)
            {
                var onay = MessageBox.Show("Bu geliri silmek istediğinize emin misiniz?", "Onay", MessageBoxButton.YesNo);
                if (onay == MessageBoxResult.Yes)
                {
                    _gelirSrv.Delete(secilen.ID);
                    Gelirler.Remove(secilen);
                    UpdateSummary();
                }
            }
        }
        private void dgGelir_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            if (e.Row.Item is MuzeGeliri g) _gelirSrv.AddOrUpdate(g);
            UpdateSummary();
        }

        // ╔════════════════ GİDER ════════════════╗
        private void BtnGiderEkle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtGiderAciklama.Text)
                || !decimal.TryParse(txtGiderTutar.Text, out var tutar))
            {
                MessageBox.Show("Açıklama ve tutar zorunludur."); return;
            }

            var g = new MuzeGideri
            {
                Aciklama = txtGiderAciklama.Text.Trim(),
                Tutar = tutar,
                Tarih = dpGiderTarih.SelectedDate ?? DateTime.Today
            };

           
            g.ID = _giderSrv.Add(g);
            Giderler.Add(g);


            txtGiderAciklama.Clear(); txtGiderTutar.Clear(); dpGiderTarih.SelectedDate = null;

            UpdateSummary();
        }

        private void BtnGiderFiltre_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            Giderler.Clear();
            var basla = dpGiderBasla.SelectedDate ?? DateTime.MinValue;
            var bit = dpGiderBit.SelectedDate ?? DateTime.MaxValue;

            foreach (var g in _giderSrv.GetAllMuzeGideri()
                         .Where(x => x.Tarih >= basla && x.Tarih <= bit))
                Giderler.Add(g);

            UpdateSummary();
        }

        private void dgGider_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            if (e.Row.Item is MuzeGideri g) _giderSrv.AddOrUpdate(g);
            UpdateSummary();
        }

        // GİDER – Güncelle
        private void BtnGiderGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            if (dgGider.SelectedItem is MuzeGideri secilen)
            {
                _giderSrv.AddOrUpdate(secilen);
                MessageBox.Show("Gider güncellendi.");
                UpdateSummary();
            }
        }

        // GİDER – Sil
        private void BtnGiderSil_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.");
                return;
            }

            if (dgGider.SelectedItem is MuzeGideri secilen)
            {
                var onay = MessageBox.Show("Bu gideri silmek istediğinize emin misiniz?", "Onay", MessageBoxButton.YesNo);
                if (onay == MessageBoxResult.Yes)
                {
                    _giderSrv.Delete(secilen.ID);
                    Giderler.Remove(secilen);
                    UpdateSummary();
                }
            }
        }
        // ╔═════════════ LiveCharts Kurulumu ═════════════╗
        private void SetupChart()
        {
            ChartSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title  = "Gelir",
                    Values = new ChartValues<decimal> { 0m }
                },
                new ColumnSeries
                {
                    Title  = "Gider",
                    Values = new ChartValues<decimal> { 0m }
                }
            };
            chartGelirGider.Series = ChartSeries;
        }

       

        // ╔════════════════ ÖZET & GRAFİK ═══════════════╗
        private void UpdateSummary()
        {
            decimal toplamGelir = Gelirler.Sum(x => x.Tutar.GetValueOrDefault());
            decimal toplamGider = Giderler.Sum(x => x.Tutar.GetValueOrDefault());
            decimal net = toplamGelir - toplamGider;

            txtToplamGelir.Text = toplamGelir.ToString("C2");
            txtToplamGider.Text = toplamGider.ToString("C2");
            txtNet.Text = net.ToString("C2");

            // Grafik sütunlarını güncelle
            ChartSeries[0].Values[0] = toplamGelir;
            ChartSeries[1].Values[0] = toplamGider;
        }
    }
}