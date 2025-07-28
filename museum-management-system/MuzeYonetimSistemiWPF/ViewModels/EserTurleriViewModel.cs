using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class EserTurleriViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EserTurleri> _eserTurleri;
        public ObservableCollection<EserTurleri> EserTurleri
        {
            get => _eserTurleri;
            set
            {
                _eserTurleri = value;
                OnPropertyChanged(nameof(EserTurleri));
            }
        }

        public EserTurleriViewModel()
        {
            var service = new EserTurleriService();
            EserTurleri = new ObservableCollection<EserTurleri>(service.GetAllEserTurleri());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
