using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class SergiViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Sergi> _sergiler;
        public ObservableCollection<Sergi> Sergiler
        {
            get => _sergiler;
            set
            {
                _sergiler = value;
                OnPropertyChanged(nameof(Sergiler));
            }
        }

        public SergiViewModel()
        {
            var service = new SergilerService();
            Sergiler = new ObservableCollection<Sergi>(service.GetAllSergiler());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
