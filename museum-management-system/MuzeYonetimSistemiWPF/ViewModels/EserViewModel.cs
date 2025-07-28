using MuzeYonetimSistemiWPF.Models;
using MuzeYonetimSistemiWPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.ViewModels
{
    public class EserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Eser> _eserler;
        public ObservableCollection<Eser> Eserler
        {
            get => _eserler;
            set
            {
                _eserler = value;
                OnPropertyChanged(nameof(Eserler));
            }
        }

        public EserViewModel()
        {
            var service = new EserService();
            Eserler = new ObservableCollection<Eser>(service.GetAllEserler());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
