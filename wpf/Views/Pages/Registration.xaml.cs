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
            string statusUser = Properties.Settings.Default.StatusUser;
            //if (idUser != 0 && statusUser != "client")
            //{
                StatusTextBox.Visibility = Visibility.Visible;
                StatusComboBox.Visibility = Visibility.Visible;
                StatusComboBox.SelectedIndex = 0;
                if (statusUser == "specialist")
                {
                    SpecComboBoxItem.Visibility = Visibility.Collapsed;
                    ManagerComboBoxItem.Visibility = Visibility.Collapsed;
                    AdminComboBoxItem.Visibility = Visibility.Collapsed;
                }
                if (statusUser == "manager")
                {
                    ManagerComboBoxItem.Visibility = Visibility.Collapsed;
                    AdminComboBoxItem.Visibility = Visibility.Collapsed;
                }
            //}
        }

        private void DateOfbirthdayCalendarChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            string statusUser;
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
                default:
                    statusUser = "client";
                    break;
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
                ["пол"] = SexComboBox.Text
            };
            DateTime? dateOfBirthdayUser = DateOfbirthdayCalendar.SelectedDate;

            UsersController usersController = new UsersController();
            if (usersController.ValidateDataRegistration(dataRegistration, dateOfBirthdayUser))
            {
                try
                {
                    usersController.RegistrationUser(dataRegistration, dateOfBirthdayUser);
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
