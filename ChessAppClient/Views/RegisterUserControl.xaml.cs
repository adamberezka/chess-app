using System;
using System.Windows;
using System.Windows.Controls;
using ChessApp.Api;
using ChessAppClient.Communication;
using ChessAppClient.ViewModels;

namespace ChessAppClient;

public partial class RegisterUserControl : UserControl
{
    public RegisterUserControl()
    {
        InitializeComponent();
    }

    private void Register_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var registrationSuccessfull = RequestHandler.Register(new CreateUserRequest(
                UsernameTextBox.Text,
                PasswordBox.Password,
                Int32.Parse(RatingTextBox.Text)
            ));
            if (registrationSuccessfull)
            {
                Application.Current.MainWindow.DataContext = new GameViewModel();
            }
            else
            {
                MessageBox.Show(
                    "Username already used!",
                    "Could not create account!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.None
                );
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                exception.Message,
                "Error encountered while creating user!",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.None
            );
        }
    }

    private void BackToLogin_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow.DataContext = new LoginViewModel();
    }
}