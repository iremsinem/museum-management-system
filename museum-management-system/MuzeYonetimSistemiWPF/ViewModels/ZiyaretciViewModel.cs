using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class ZiyaretciViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Ziyaretci> _ziyaretciler;
        public ObservableCollection<Ziyaretci> Ziyaretciler
        {
            get => _ziyaretciler;
            set
            {
                _ziyaretciler = value;
                OnPropertyChanged(nameof(Ziyaretciler));
            }
        }

        public ZiyaretciViewModel()
        {
            var service = new ZiyaretciService();
            Ziyaretciler = new ObservableCollection<Ziyaretci>(service.GetAllZiyaretci());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
