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
using wpf.Model;

namespace wpf.Views.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для ClientPersonalCabinet.xaml
    /// </summary>
    public partial class ClientPersonalCabinet : Page
    {
        int idUser;
        string statusUsingUser = Properties.Settings.Default.StatusUser;
        Core db = new Core();
        public ClientPersonalCabinet(int userID)
        {
            InitializeComponent();
            idUser = userID;
            mainTitle.Text += " " + db.context.Users.FirstOrDefault(x => x.idUser == idUser).Login;
            if (statusUsingUser != "admin" && statusUsingUser != "manager")
            {
                deleteUserButton.Visibility = Visibility.Collapsed;
            }
            editDataUserButton.Visibility= Visibility.Collapsed;
            editPasswordUserButton.Visibility= Visibility.Collapsed;
            if (idUser == Properties.Settings.Default.IdUser || statusUsingUser == "admin" || statusUsingUser == "manager")
            {
                editDataUserButton.Visibility = Visibility.Visible;
                editPasswordUserButton.Visibility = Visibility.Visible;
            }
        }

        private void editDataUserButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void editPasswordUserButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void deleteUserButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DownloadMedicalRecordsButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Properties.Settings.Default.IdUser = 0;
                Properties.Settings.Default.StatusUser = "";
                MessageBox.Show("Вы успешно вышли!\nПеренаправление на страницу авторизации.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.Navigate(new AuthPage());
            }

        }
    }
}
