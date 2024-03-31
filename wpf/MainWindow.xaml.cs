using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf.Properties;
using wpf.Views.Pages;
using wpf.Views.Pages.Client;

namespace wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!(Properties.Settings.Default.IdUser > 0))
                MainFrame.NavigationService.Navigate(new AuthPage());
            else 
                MainFrame.NavigationService.Navigate(new MainPage());
        }

        private void ClientPersonalCabinetLinkButtonClick(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.IdUser != 0)
                MainFrame.NavigationService.Navigate(new ClientPersonalCabinet(Properties.Settings.Default.IdUser));
            else 
                MessageBox.Show("Вы еще не авторизованы!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}
