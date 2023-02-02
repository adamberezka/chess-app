using System.Windows;
using ChessAppClient.Communication;
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
            ProvideServerAddressModel context = new ProvideServerAddressModel();
            app.DataContext = context;
            app.Show();
        }
    }
}