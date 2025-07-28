using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class MuzeGiderleriViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MuzeGideri> _muzeGiderleri;
        public ObservableCollection<MuzeGideri> MuzeGiderleri
        {
            get => _muzeGiderleri;
            set
            {
                _muzeGiderleri = value;
                OnPropertyChanged(nameof(MuzeGiderleri));
            }
        }

        public MuzeGiderleriViewModel()
        {
            var service = new MuzeGideriService();
            MuzeGiderleri = new ObservableCollection<MuzeGideri>(service.GetAllMuzeGideri());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
