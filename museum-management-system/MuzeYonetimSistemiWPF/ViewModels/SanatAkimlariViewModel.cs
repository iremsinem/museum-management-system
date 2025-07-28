using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class SanatAkimlariViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SanatAkimi> _sanatAkimlari;
        public ObservableCollection<SanatAkimi> SanatAkimlari
        {
            get => _sanatAkimlari;
            set
            {
                _sanatAkimlari = value;
                OnPropertyChanged(nameof(SanatAkimlari));
            }
        }

        public SanatAkimlariViewModel()
        {
            var service = new SanatAkimiService();
            SanatAkimlari = new ObservableCollection<SanatAkimi>(service.GetAllSanatAkimi());
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }
}
