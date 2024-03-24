using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
using wpf.Views.Pages.Client;

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditingPersonalInformation.xaml
    /// </summary>
    public partial class EditingPersonalInformation : Page
    {
        readonly Core db = new Core();
        readonly int IdUser;
        public EditingPersonalInformation(int userID)
        {
            InitializeComponent();
            IdUser = userID;

            Users user = db.context.Users.FirstOrDefault(x => x.idUser == IdUser);

            string statusUser = Properties.Settings.Default.StatusUser;
            //Если редачит менеджер или админ, может менять логин
            if (statusUser == "admin" || statusUser == "manager")
                LoginTextBox.IsEnabled = true;
            //менеджер не может изменить другого менеджера или админа
            if (statusUser == "manager" && (user.Status == "manager" || user.Status == "admin"))
                LoginTextBox.IsEnabled = false;

            if (user != null)
            {
                if (IdUser != 0 && statusUser != "client")
                {
                    StatusTextBox.Visibility = Visibility.Visible;
                    StatusComboBox.Visibility = Visibility.Visible;

                    int indexComboBoxStatusUser;
                    switch (user.Status)
                    {
                        case "specialist":
                            indexComboBoxStatusUser = 1;
                            break;
                        case "manager":
                            indexComboBoxStatusUser = 2;
                            break;
                        case "admin":
                            indexComboBoxStatusUser = 3;
                            break;
                        default:
                            indexComboBoxStatusUser = 0;
                            break;
                    }

                    StatusComboBox.SelectedIndex = indexComboBoxStatusUser;
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
                }


                LoginTextBox.Text = user.Login;
                LastnameTextBox.Text = user.Lastname;
                NameTextBox.Text = user.Firstname;
                PatronymicTextBox.Text = user.Patronymic;
                if (user.Sex == 1)
                    SexComboBox.SelectedIndex = 0;
                else SexComboBox.SelectedIndex = 1;
                DateOfbirthdayCalendar.SelectedDate = user.Date;
                DateOfbirthdayCalendar.DisplayDate = (DateTime)user.Date;
                GeolocationTextBox.Text = user.Geolocation;
                TelephonTextBox.Text = user.Telephon;
                ProfessionTextBox.Text = user.Profession;
            }
            else
                MessageBox.Show("ваш id не найден в БД. Возможно учетная запись была удалена", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
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
                ["id"] = Convert.ToString(IdUser),
                ["логин"] = LoginTextBox.Text,
                ["имя"] = NameTextBox.Text,
                ["фамилия"] = LastnameTextBox.Text,
                ["отчество"] = PatronymicTextBox.Text,
                ["адрес проживания"] = GeolocationTextBox.Text,
                ["профессия"] = ProfessionTextBox.Text,
                ["телефон"] = TelephonTextBox.Text,
                ["пол"] = SexComboBox.Text
            };
            DateTime? dateOfBirthdayUser = DateOfbirthdayCalendar.SelectedDate;

            UsersController usersController = new UsersController();
            if (usersController.ValidateDataRegistration(dataRegistration, dateOfBirthdayUser, false))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите внести изменения?", "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (messageBoxResult.ToString() == "Yes")
                {
                    try
                    {
                        usersController.RegistrationUser(dataRegistration, dateOfBirthdayUser, false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Произошла неизвестная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    MessageBox.Show("Данные успешно сохранены!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.NavigationService.Navigate(new ClientPersonalCabinet(IdUser));
                }
            }
        }
    }
}
