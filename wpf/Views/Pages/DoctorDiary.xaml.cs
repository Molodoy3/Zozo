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
            NavigationService.Navigate(new AppointmentPage(idAppointment));
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
