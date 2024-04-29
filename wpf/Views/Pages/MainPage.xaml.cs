using DocumentFormat.OpenXml.Wordprocessing;
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
using wpf.Model;
using wpf.Properties;
using wpf.Views.Pages.Client;

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            Console.WriteLine(Properties.Settings.Default.StatusUser);
            Console.WriteLine(Properties.Settings.Default.IdUser);

            UsersController usersController = new UsersController();
            var arrayClintsContact = usersController.getAllUsers();
            ClientsListView.ItemsSource = arrayClintsContact;

            string statusUsingUser = Properties.Settings.Default.StatusUser;
            if (statusUsingUser == "admin" || statusUsingUser == "manager" || statusUsingUser == "HeadsDepartment" || statusUsingUser == "specialist")
                AddNewUserButton.Visibility = Visibility.Visible;
        }

        private void AddNewUserButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }

        private void UserButtonClick(object sender, RoutedEventArgs e)
        {
            Button activeElement = sender as Button;
            Users activeUser = activeElement.DataContext as Users;
            int idUser = activeUser.idUser;
            NavigationService.Navigate(new ClientPersonalCabinet(idUser));
        }
    }
}
