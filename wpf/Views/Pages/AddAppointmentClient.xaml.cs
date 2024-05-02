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
using wpf.Views.Pages.Client;

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddAppointmentClient.xaml
    /// </summary>
    public partial class AddAppointmentClient : Page
    {
        int idDoctor;
        public AddAppointmentClient(int doctorID)
        {
            InitializeComponent();
            idDoctor = doctorID;
            UsersController usersController = new UsersController();
            DoctorNameTextBlock.Text = usersController.GetUser(idDoctor).FIO;

            var usersHeadsDepartment = usersController.GetAllUsersHeadsDepartment();
            headsDepartmentComboBox.ItemsSource = usersHeadsDepartment;
            headsDepartmentComboBox.DisplayMemberPath = "FIO";
            headsDepartmentComboBox.SelectedValuePath = "idUser";

        }
        private void AddAppointmentClick(object sender, RoutedEventArgs e)
        {
            object[] dataAppointment = new object[5];
            dataAppointment[0] = dateOfAppointmentCalendar.SelectedDate;
            dataAppointment[1] = ReferralTextTextBox.Text;
            dataAppointment[2] = headsDepartmentComboBox.SelectedValue;
            dataAppointment[3] = idDoctor;
            dataAppointment[4] = Properties.Settings.Default.IdUser;

            try
            {
                AppoitmentsController appoitmentsController = new AppoitmentsController();
                appoitmentsController.ValidateAppointmentDataClient(dataAppointment);
                MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите записаться?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    appoitmentsController.AddAppointmentClient(dataAppointment);
                    MessageBox.Show("Запись добавлена!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new ClientPersonalCabinet(idDoctor));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
