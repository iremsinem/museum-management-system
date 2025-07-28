using System.Windows;
using MuzeYonetimSistemiWPF.Helpers;

namespace MuzeYonetimSistemiWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!SqlHelper.BaglantiTest())
            {
                MessageBox.Show("Veritabanı bağlantısı başarısız! Uygulama kapatılıyor.");
                Current.Shutdown();
            }
        }
    }
}
