using System.Windows;
using ChessAppClient.ViewModels;

namespace ChessAppClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow app = new MainWindow();
            // ProvideServerAddressModel context = new ProvideServerAddressModel();
            GameViewModel context = new ();
            app.DataContext = context;
            app.Show();
        }
    }
}