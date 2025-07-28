using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class EtkinlikViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Etkinlik> _etkinlikler;
        public ObservableCollection<Etkinlik> Etkinlikler
        {
            get => _etkinlikler;
            set { _etkinlikler = value; OnPropertyChanged(nameof(Etkinlikler)); }
        }
        

        public EtkinlikViewModel()
        {
            var service = new EtkinlikService();
            Etkinlikler = new ObservableCollection<Etkinlik>(service.GetAllEtkinlik());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
