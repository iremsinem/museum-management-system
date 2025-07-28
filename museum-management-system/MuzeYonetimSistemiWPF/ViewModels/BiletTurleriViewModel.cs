using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class BiletTurleriViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BiletTuru> _biletTurleri;
        public ObservableCollection<BiletTuru> BiletTurleri
        {
            get => _biletTurleri;
            set
            {
                _biletTurleri = value;
                OnPropertyChanged(nameof(BiletTurleri));
            }
        }

        public BiletTurleriViewModel()
        {
            var service = new BiletTuruService();
            BiletTurleri = new ObservableCollection<BiletTuru>(service.GetAllBiletTuru());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
