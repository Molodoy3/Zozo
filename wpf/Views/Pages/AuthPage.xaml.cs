using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using wpf.Model;
using wpf.Views.Pages.Client;

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        Core db = new Core();
        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthButtonClick(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;

            try
            {

                UsersController usersController = new UsersController();

                string userStatus = usersController.CheckAuth(login, password);
                if (userStatus != "")
                {
                    usersController.SetLocalSettings(userStatus);
                    this.NavigationService.Navigate(new MainPage());
                }
                //if (userStatus == "client")
                //    this.NavigationService.Navigate(new ClientPersonalCabinet());
                //else if (usersController.CheckAuth(login, password) == "specialist")
                //    this.NavigationService.Navigate(new ClientPersonalCabinet());
                //else if (usersController.CheckAuth(login, password) == "manager")
                //    this.NavigationService.Navigate(new ClientPersonalCabinet());
                //else if (usersController.CheckAuth(login, password) == "admin")
                //    this.NavigationService.Navigate(new ClientPersonalCabinet());

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
