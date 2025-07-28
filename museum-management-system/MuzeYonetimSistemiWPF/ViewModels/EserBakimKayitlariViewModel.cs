using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class EserBakimKayitlariViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EserBakimKaydi> _eserBakimKayitlari;
        public ObservableCollection<EserBakimKaydi> EserBakimKayitlari
        {
            get => _eserBakimKayitlari;
            set
            {
                _eserBakimKayitlari = value;
                OnPropertyChanged(nameof(EserBakimKayitlari));
            }
        }

        public EserBakimKayitlariViewModel()
        {
            var service = new EserBakimKaydiService();
            EserBakimKayitlari = new ObservableCollection<EserBakimKaydi>(service.GetAllEserBakimKaydi());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
