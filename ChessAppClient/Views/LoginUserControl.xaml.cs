using System.Windows;
using System.Windows.Controls;
using ChessApp.Api;
using ChessAppClient.Communication;
using ChessAppClient.ViewModels;

namespace ChessAppClient;

public partial class LoginUserControl : UserControl
{
    public LoginUserControl()
    {
        InitializeComponent();
    }

    private void Login_OnClick(object sender, RoutedEventArgs e)
    {
        var response = RequestHandler.Login(new LoginRequest(UsernameTextBox.Text, PasswordBox.Password));
        if (response != null)
        {
            UserHolder.Id = response.Id;
            UserHolder.Username = response.Username;
            UserHolder.Rating = response.Rating;
            Application.Current.MainWindow.DataContext = new GameViewModel();
        }
        else
        {
            MessageBox.Show(
                "Invalid login credentials!",
                "Invalid credentials!",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.None
            );
        }
    }

    private void CreateAccount_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow.DataContext = new RegisterViewModel();
    }
}