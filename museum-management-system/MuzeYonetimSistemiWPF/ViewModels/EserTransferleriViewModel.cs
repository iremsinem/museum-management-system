using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class EserTransferleriViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EserTransferi> _eserTransferleri;
        public ObservableCollection<EserTransferi> EserTransferleri
        {
            get => _eserTransferleri;
            set
            {
                _eserTransferleri = value;
                OnPropertyChanged(nameof(EserTransferleri));
            }
        }

        public EserTransferleriViewModel()
        {
            var service = new EserTransferiService();
            EserTransferleri = new ObservableCollection<EserTransferi>(service.GetAllEserTransferi());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
