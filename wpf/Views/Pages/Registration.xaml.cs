using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();

            int idUser = Properties.Settings.Default.IdUser;
            string statusUserThat = Properties.Settings.Default.StatusUser;
            if (statusUserThat == "specialist" || statusUserThat == "manager" || statusUserThat == "admin" || statusUserThat == "HeadsDepartment")
            {
                StatusTextBox.Visibility = Visibility.Visible;
                StatusComboBox.Visibility = Visibility.Visible;
                StatusComboBox.SelectedIndex = 0;
                if (statusUserThat == "specialist")
                {
                    SpecComboBoxItem.Visibility = Visibility.Collapsed;
                    ManagerComboBoxItem.Visibility = Visibility.Collapsed;
                    AdminComboBoxItem.Visibility = Visibility.Collapsed;
                }
                if (statusUserThat == "manager" || statusUserThat == "HeadsDepartment")
                {
                    ManagerComboBoxItem.Visibility = Visibility.Collapsed;
                    AdminComboBoxItem.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void DateOfbirthdayCalendarChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            string statusUser = "client";
            string statusUserThat = Properties.Settings.Default.StatusUser;
            if (statusUserThat == "specialist" || statusUserThat == "manager" || statusUserThat == "admin" || statusUserThat == "HeadsDepartment")
            {
                TextBlock selectedTextBlock = (TextBlock)StatusComboBox.SelectedItem;
                switch (selectedTextBlock.Text)
                {
                    case "Специалист":
                        statusUser = "specialist";
                        break;
                    case "Менеджер":
                        statusUser = "manager";
                        break;
                    case "Администратор":
                        statusUser = "admin";
                        break;
                    case "Заведующий отделением":
                        statusUser = "HeadsDepartment";
                        break;
                    default:
                        statusUser = "client";
                        break;
                }
            }
            Dictionary<string, string> dataRegistration = new Dictionary<string, string>
            {
                ["статус"] = statusUser,
                ["имя"] = NameTextBox.Text,
                ["фамилия"] = LastnameTextBox.Text,
                ["отчество"] = PatronymicTextBox.Text,
                ["адрес проживания"] = GeolocationTextBox.Text,
                ["профессия"] = ProfessionTextBox.Text,
                ["пароль"] = PasswordTextBox.Password,
                ["повторение пароля"] = PasswordDoubleTextBox.Password,
                ["логин"] = LoginTextBox.Text,
                ["телефон"] = TelephonTextBox.Text,
                ["пол"] = SexComboBox.Text,
                ["id"] = Convert.ToString(Properties.Settings.Default.IdUser)
            };
            DateTime? dateOfBirthdayUser = DateOfbirthdayCalendar.SelectedDate;

            UsersController usersController = new UsersController();
            if (usersController.ValidateDataRegistration(dataRegistration, dateOfBirthdayUser))
            {
                try
                {
                    bool isAnotherUser = Properties.Settings.Default.IdUser is int && Properties.Settings.Default.IdUser > 0 ? true : false;
                    usersController.RegistrationUser(dataRegistration, dateOfBirthdayUser, true, isAnotherUser);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Произошла неизвестная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.NavigationService.Navigate(new MainPage());
            }

        }
    }
}
