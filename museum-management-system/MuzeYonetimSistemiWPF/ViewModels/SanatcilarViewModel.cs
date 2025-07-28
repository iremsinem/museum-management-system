using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class SanatcilarViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Sanatci> _sanatcilar;
        public ObservableCollection<Sanatci> Sanatcilar
        {
            get => _sanatcilar;
            set
            {
                _sanatcilar = value;
                OnPropertyChanged(nameof(Sanatcilar));
            }
        }

        public SanatcilarViewModel()
        {
            var service = new SanatcilarService();
            Sanatcilar = new ObservableCollection<Sanatci>(service.GetAllSanatcilar());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
