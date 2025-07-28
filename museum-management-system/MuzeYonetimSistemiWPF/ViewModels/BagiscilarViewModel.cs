using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class BagiscilarViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Bagisci> _bagiscilar;
        public ObservableCollection<Bagisci> Bagiscilar
        {
            get => _bagiscilar;
            set
            {
                _bagiscilar = value;
                OnPropertyChanged(nameof(Bagiscilar));
            }
        }

        public BagiscilarViewModel()
        {
            var service = new BagisciService();
            Bagiscilar = new ObservableCollection<Bagisci>(service.GetAllBagisci());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
