using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Admin> _adminler;
        public ObservableCollection<Admin> Adminler
        {
            get => _adminler;
            set
            {
                _adminler = value;
                OnPropertyChanged(nameof(Adminler));
            }
        }

        public AdminViewModel()
        {
            var service = new AdminService();
            Adminler = new ObservableCollection<Admin>(service.GetAllAdmin());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
