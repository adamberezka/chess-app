using System;
using System.Windows;
using System.Windows.Controls;
using ChessAppClient.Communication;
using ChessAppClient.ViewModels;

namespace ChessAppClient.Views;

public partial class ProvideServerAddressUserControl : UserControl
{

    public ProvideServerAddressUserControl()
    {
        InitializeComponent();
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        var address = AddressTextBox.Text;
        bool isValid = RequestHandler.SetHostAddress(address);
        if (isValid && RequestHandler.IsServerListening())
        {
            Application.Current.MainWindow.DataContext = new LoginViewModel();
        }
        else
        {
            MessageBox.Show(
                $"Invalid Server Address: {address}",
                "Invalid Address!",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.None
            );
        }
    }
}