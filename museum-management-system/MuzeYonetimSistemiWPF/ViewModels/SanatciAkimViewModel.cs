using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class SanatciAkimViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SanatciAkim> _sanatciAkimlar;
        public ObservableCollection<SanatciAkim> SanatciAkimlar
        {
            get => _sanatciAkimlar;
            set
            {
                _sanatciAkimlar = value;
                OnPropertyChanged(nameof(SanatciAkimlar));
            }
        }

        public SanatciAkimViewModel()
        {
            var service = new SanatciAkimService();
            SanatciAkimlar = new ObservableCollection<SanatciAkim>(service.GetAllSanatciAkim());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
