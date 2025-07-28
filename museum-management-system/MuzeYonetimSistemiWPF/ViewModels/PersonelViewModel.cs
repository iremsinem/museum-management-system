using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class PersonelViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Personel> _personeller;
        public ObservableCollection<Personel> Personeller
        {
            get => _personeller;
            set { _personeller = value; OnPropertyChanged(nameof(Personeller)); }
        }
        

        public PersonelViewModel()
        {
            var service = new PersonelService();
            Personeller = new ObservableCollection<Personel>(service.GetAllPersonel());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
