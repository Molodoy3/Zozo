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
        }

        private void DateOfbirthdayCalendarChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> dataRegistration = new Dictionary<string, string>();
            dataRegistration["имя"] = NameTextBox.Text;
            dataRegistration["фамилия"] = LastnameTextBox.Text;
            dataRegistration["отчество"] = PatronymicTextBox.Text;
            dataRegistration["адрес проживания"] = GeolocationTextBox.Text;
            dataRegistration["профессия"] = ProfessionTextBox.Text;
            dataRegistration["пароль"] = PasswordTextBox.Password;
            dataRegistration["повторение пароля"] = PasswordDoubleTextBox.Password;
            dataRegistration["логин"] = LoginTextBox.Text;
            dataRegistration["телефон"] = TelephonTextBox.Text;
            dataRegistration["пол"] = SexComboBox.Text;
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
