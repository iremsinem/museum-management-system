using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class EtkinlikTurleriViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EtkinlikTuru> _etkinlikTurleri;
        public ObservableCollection<EtkinlikTuru> EtkinlikTurleri
        {
            get => _etkinlikTurleri;
            set
            {
                _etkinlikTurleri = value;
                OnPropertyChanged(nameof(EtkinlikTurleri));
            }
        }

        public EtkinlikTurleriViewModel()
        {
            var service = new EtkinlikTuruService();
            EtkinlikTurleri = new ObservableCollection<EtkinlikTuru>(service.GetAllEtkinlikTuru());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
