using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class EtkinlikKayitlariViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EtkinlikKaydi> _etkinlikKayitlari;
        public ObservableCollection<EtkinlikKaydi> EtkinlikKayitlari
        {
            get => _etkinlikKayitlari;
            set
            {
                _etkinlikKayitlari = value;
                OnPropertyChanged(nameof(EtkinlikKayitlari));
            }
        }

        public EtkinlikKayitlariViewModel()
        {
            var service = new EtkinlikKayitService();
            EtkinlikKayitlari = new ObservableCollection<EtkinlikKaydi>(service.GetAllEtkinlikKayit());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
