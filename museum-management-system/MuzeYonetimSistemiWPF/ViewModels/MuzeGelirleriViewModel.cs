using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class MuzeGelirleriViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MuzeGeliri> _muzeGelirleri;
        public ObservableCollection<MuzeGeliri> MuzeGelirleri
        {
            get => _muzeGelirleri;
            set
            {
                _muzeGelirleri = value;
                OnPropertyChanged(nameof(MuzeGelirleri));
            }
        }

        public MuzeGelirleriViewModel()
        {
            var service = new MuzeGeliriService();
            MuzeGelirleri = new ObservableCollection<MuzeGeliri>(service.GetAllMuzeGeliri());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
