using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class ZiyaretciGirisKayitlariViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ZiyaretciGirisKaydi> _girisKayitlari;
        public ObservableCollection<ZiyaretciGirisKaydi> GirisKayitlari
        {
            get => _girisKayitlari;
            set
            {
                _girisKayitlari = value;
                OnPropertyChanged(nameof(GirisKayitlari));
            }
        }

        public ZiyaretciGirisKayitlariViewModel()
        {
            var service = new ZiyaretciGirisKaydiService();
            GirisKayitlari = new ObservableCollection<ZiyaretciGirisKaydi>(service.GetAllZiyaretciGirisKaydi());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
