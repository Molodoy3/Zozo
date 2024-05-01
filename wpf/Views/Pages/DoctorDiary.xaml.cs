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
using wpf.Model;
using wpf.Views.Pages.Client;

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для DoctorDiary.xaml
    /// </summary>
    public partial class DoctorDiary : Page
    {
        public int idDoctor;
        public DoctorDiary(int doctorID)
        {
            InitializeComponent();
            idDoctor = doctorID;

            AppoitmentsController appoitmentsController = new AppoitmentsController();
            var arrayPlannedAppoitments = appoitmentsController.GetPlanedAppointmentsForDoctor(idDoctor);
            PlanedAppoitmentsListView.ItemsSource = arrayPlannedAppoitments;

            var arrayHistoryAppoitments = appoitmentsController.GetHistoryAppointmentsForDoctor(idDoctor);
            HistoryAppoitmentsListView.ItemsSource = arrayHistoryAppoitments;

            UsersController usersController = new UsersController();
            var arrayClintsContactDoctor = usersController.getClintsContactDoctor(doctorID);
            ClientsListView.ItemsSource = arrayClintsContactDoctor;
        }

        private void AddAppointmentButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAppointment(idDoctor));
        }

        private void AppoitmentButtonClick(object sender, RoutedEventArgs e)
        {
            Button activeElement = sender as Button;
            Appointments activeAppointment = activeElement.DataContext as Appointments;
            int idAppointment = activeAppointment.IdAppointment;
            NavigationService.Navigate(new AppointmentPage(idAppointment, idDoctor));
        }

        private void UserButtonClick(object sender, RoutedEventArgs e)
        {
            Button activeElement = sender as Button;
            Users activeUser = activeElement.DataContext as Users;
            int idUser = activeUser.idUser;
            NavigationService.Navigate(new ClientPersonalCabinet(idUser));
        }

        private void ButtonSearcheClick(object sender, RoutedEventArgs e)
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
                case "Заведующий отделением":
                    statusUser = "HeadsDepartment";
                    break;
                case "Клиент":
                    statusUser = "client";
                    break;
                default:
                    statusUser = "all";
                    break;
            }

            string date = CalendarForSearch.DisplayDate.ToString("dd.MM.yy");

            string textForSearch = SearchUserTextBox.Text;
            UsersController usersController = new UsersController();
            List<Users> searchingUsers = usersController.GetUsersForSearch(textForSearch, statusUser, date);
            ClientsListView.ItemsSource = searchingUsers;

            if (searchingUsers.Count > 0)
                StatictickFoundTextBlock.Text = "Найдено " + searchingUsers.Count;
            else
                StatictickFoundTextBlock.Text = "Ничего не найдено";
        }
    }
}

