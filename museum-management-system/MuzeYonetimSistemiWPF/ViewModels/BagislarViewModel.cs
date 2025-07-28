using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class BagislarViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Bagis> _bagislar;
        public ObservableCollection<Bagis> Bagislar
        {
            get => _bagislar;
            set
            {
                _bagislar = value;
                OnPropertyChanged(nameof(Bagislar));
            }
        }

        public BagislarViewModel()
        {
            var service = new BagisService();
            Bagislar = new ObservableCollection<Bagis>(service.GetAllBagis());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
