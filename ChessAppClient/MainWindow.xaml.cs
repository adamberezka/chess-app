
using System.Windows;
using System.Windows.Controls;


namespace ChessAppClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UserControl UserControl { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}