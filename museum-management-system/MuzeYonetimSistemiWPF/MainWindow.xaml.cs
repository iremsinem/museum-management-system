using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MuzeYonetimSistemiWPF.Views;


namespace MuzeYonetimSistemiWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void GirisButton_Click(object sender, RoutedEventArgs e)
    {
        // LoginView penceresini aç
        var loginWindow = new LoginWindow(); // Eğer LoginView bir Window ise
        loginWindow.Show();

        this.Close(); // MainWindow'u kapat (isteğe bağlı)
    }
}