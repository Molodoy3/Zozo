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
using wpf.Controllers;
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

            if (!(Settings.Default.IdUser > 0))
                MainFrame.NavigationService.Navigate(new AuthPage());
            else
            {
                MainFrame.NavigationService.Navigate(new MainPage());
                UsersController usersController = new UsersController();
                var user = usersController.GetUser(Settings.Default.IdUser);
                DataContext = user;
            }
        }

        private void IconUserMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Settings.Default.IdUser != 0)
                MainFrame.NavigationService.Navigate(new ClientPersonalCabinet(Settings.Default.IdUser));
            else
                MessageBox.Show("Вы еще не авторизованы!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void LogoClick(object sender, MouseButtonEventArgs e)
        {
            if (Settings.Default.IdUser != 0)
                MainFrame.NavigationService.Navigate(new MainPage());
            else
                MessageBox.Show("Вы еще не авторизованы!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
