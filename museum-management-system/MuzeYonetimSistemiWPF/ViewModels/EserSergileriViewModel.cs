using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class EserSergileriViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EserSergi> _eserSergileri;
        public ObservableCollection<EserSergi> EserSergileri
        {
            get => _eserSergileri;
            set
            {
                _eserSergileri = value;
                OnPropertyChanged(nameof(EserSergileri));
            }
        }

        public EserSergileriViewModel()
        {
            var service = new EserSergileriService();
            EserSergileri = new ObservableCollection<EserSergi>(service.GetAllEserSergi());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
