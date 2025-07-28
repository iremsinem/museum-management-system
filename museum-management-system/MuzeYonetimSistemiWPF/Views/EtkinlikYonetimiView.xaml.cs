using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using MuzeYonetimSistemiWPF.ViewModels;
using MuzeYonetimSistemiWPF.Views;
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
    public partial class EtkinlikYonetimiView : Window
    {
        private EtkinlikService _etkinlikService;
        private EtkinlikViewModel _viewModel;
        private EtkinlikTuruService etkinlikTuruService;
        private ObservableCollection<EtkinlikTuru> etkinlikTurleri;

        private readonly Admin _admin;

        public EtkinlikYonetimiView(Admin admin)
        {
            InitializeComponent();

            _admin = admin;
            _etkinlikService = new EtkinlikService();
            _viewModel = new EtkinlikViewModel();
            etkinlikTuruService = new EtkinlikTuruService();

            DataContext = _viewModel;

            LoadEtkinlikTurleri();
            ListeyiGuncelle();

            Loaded += EtkinlikYonetimiView_Loaded;
        }

        private void LoadEtkinlikTurleri()
        {
            var turler = etkinlikTuruService.GetAllEtkinlikTuru();
            etkinlikTurleri = new ObservableCollection<EtkinlikTuru>(turler);
            dgEtkinlikTurleri.ItemsSource = etkinlikTurleri;
            cmbEtkinlikTuru.ItemsSource = etkinlikTurleri;
        }

        private void ListeyiGuncelle()
        {
            var liste = _etkinlikService.GetAllEtkinlik();
            _viewModel.Etkinlikler = new ObservableCollection<Etkinlik>(liste);
            dgEtkinlikler.ItemsSource = _viewModel.Etkinlikler;
        }

        // Etkinlikler
        private void BtnEtkinlikEkle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                MessageBox.Show("Bu işlemi yapma yetkiniz yok.");
                return;
            }

            try
            {
                var yeniEtkinlik = new Etkinlik
                {
                    Ad = txtEtkinlikAd.Text,
                    Tur = txtTur.Text,
                    BaslangicTarihi = dpBaslangic.SelectedDate ?? DateTime.Now,
                    BitisTarihi = dpBitis.SelectedDate ?? DateTime.Now,
                    Aciklama = txtEtkinlikAciklama.Text,
                    TurID = cmbEtkinlikTuru.SelectedValue as int?  // ❗ BURASI EKLENDİ
                };


                _etkinlikService.Add(yeniEtkinlik);
                ListeyiGuncelle();
                MessageBox.Show("Etkinlik başarıyla eklendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme sırasında hata: " + ex.Message);
            }
        }

        private void BtnEtkinlikGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem sadece yöneticiler tarafından yapılabilir.");
                return;
            }

            if (dgEtkinlikler.SelectedItem is Etkinlik secilenEtkinlik)
            {
                try
                {
                    // 🔽 GÜNCELLEME: Etkinlik Türü ID'si de set ediliyor
                    secilenEtkinlik.Ad = txtEtkinlikAd.Text;
                    secilenEtkinlik.Tur = txtTur.Text;
                    secilenEtkinlik.BaslangicTarihi = dpBaslangic.SelectedDate ?? secilenEtkinlik.BaslangicTarihi;
                    secilenEtkinlik.BitisTarihi = dpBitis.SelectedDate ?? secilenEtkinlik.BitisTarihi;
                    secilenEtkinlik.Aciklama = txtEtkinlikAciklama.Text;

                    // 🟡 Eksik olan satır: Etkinlik Türü ID'si combobox'tan alınmalı
                    secilenEtkinlik.TurID = cmbEtkinlikTuru.SelectedValue as int?;

                    _etkinlikService.Update(secilenEtkinlik);
                    ListeyiGuncelle();

                    MessageBox.Show("Etkinlik başarıyla güncellendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Güncelleme sırasında hata: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek için bir etkinlik seçiniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void BtnEtkinlikSil_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem sadece yöneticiler tarafından yapılabilir.");
                return;
            }

            if (dgEtkinlikler.SelectedItem is Etkinlik secilenEtkinlik)
            {
                var result = MessageBox.Show($"'{secilenEtkinlik.Ad}' etkinliğini silmek istediğinize emin misiniz?", "Onay", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _etkinlikService.Delete(secilenEtkinlik.ID);
                        ListeyiGuncelle();
                        MessageBox.Show("Etkinlik başarıyla silindi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Silme sırasında hata: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir etkinlik seçiniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DgEtkinlikler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEtkinlikler.SelectedItem is Etkinlik secilen)
            {
                txtEtkinlikAd.Text = secilen.Ad;
                txtTur.Text = secilen.Tur;
                dpBaslangic.SelectedDate = secilen.BaslangicTarihi;
                dpBitis.SelectedDate = secilen.BitisTarihi;
                txtEtkinlikAciklama.Text = secilen.Aciklama;
            }
        }

        // Etkinlik Türleri
        private void DgEtkinlikTurleri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEtkinlikTurleri.SelectedItem is EtkinlikTuru secilenTur)
            {
                txtTur.Text = secilenTur.Ad;
                txtEtkinlikTuruAciklama.Text = secilenTur.Aciklama;
            }
        }

        private void BtnEtkinlikTuruEkle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                MessageBox.Show("Bu işlemi yapma yetkiniz yok.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTur.Text))
            {
                MessageBox.Show("Lütfen Tür adını girin.");
                return;
            }

            EtkinlikTuru yeniTur = new EtkinlikTuru
            {
                Ad = txtTur.Text.Trim(),
                Aciklama = txtEtkinlikTuruAciklama.Text.Trim()
            };

            try
            {
                etkinlikTuruService.Add(yeniTur);
                MessageBox.Show("Yeni etkinlik türü eklendi.");
                LoadEtkinlikTurleri();
                ClearTurInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }

        private void BtnEtkinlikTuruGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem sadece yöneticiler tarafından yapılabilir.");
                return;
            }

            if (dgEtkinlikTurleri.SelectedItem is not EtkinlikTuru secilenTur)
            {
                MessageBox.Show("Lütfen güncellenecek türü seçin.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTur.Text))
            {
                MessageBox.Show("Tür adı boş olamaz.");
                return;
            }

            secilenTur.Ad = txtTur.Text.Trim();
            secilenTur.Aciklama = txtEtkinlikTuruAciklama.Text.Trim();

            try
            {
                etkinlikTuruService.Update(secilenTur);
                MessageBox.Show("Etkinlik türü güncellendi.");
                LoadEtkinlikTurleri();
                ClearTurInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }

        private void BtnEtkinlikTuruSil_Click(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi != "Tam Yetki")
            {
                MessageBox.Show("Bu işlem sadece yöneticiler tarafından yapılabilir.");
                return;
            }

            if (dgEtkinlikTurleri.SelectedItem is not EtkinlikTuru secilenTur)
            {
                MessageBox.Show("Lütfen silinecek türü seçin.");
                return;
            }

            var result = MessageBox.Show($"{secilenTur.Ad} türünü silmek istediğinize emin misiniz?", "Onay", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    etkinlikTuruService.Delete(secilenTur.ID);
                    MessageBox.Show("Etkinlik türü silindi.");
                    LoadEtkinlikTurleri();
                    ClearTurInput();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata oluştu: {ex.Message}");
                }
            }
        }
        private void EtkinlikYonetimiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_admin.YetkiSeviyesi == "Sınırlı")
            {
                BtnEtkinlikEkle.IsEnabled = false;
                BtnEtkinlikGuncelle.IsEnabled = false;
                BtnEtkinlikSil.Visibility = Visibility.Collapsed;

                BtnEtkinlikTuruEkle.IsEnabled = false;
                BtnEtkinlikTuruGuncelle.IsEnabled = false;
                BtnEtkinlikTuruSil.Visibility = Visibility.Collapsed;
            }
            else if (_admin.YetkiSeviyesi == "Yönetici")
            {
                BtnEtkinlikGuncelle.IsEnabled = false;
                BtnEtkinlikSil.Visibility = Visibility.Collapsed;

                BtnEtkinlikTuruGuncelle.IsEnabled = false;
                BtnEtkinlikTuruSil.Visibility = Visibility.Collapsed;
            }
        }


        private void ClearTurInput()
        {
            txtTur.Text = "";
            txtEtkinlikTuruAciklama.Text = "";
            dgEtkinlikTurleri.SelectedItem = null;
        }
    }
}