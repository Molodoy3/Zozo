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

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AppointmentPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page
    {
        private int appointmentId;
        public AppointmentPage(int idAppointment)
        {
            InitializeComponent();
            appointmentId = idAppointment;
            AppoitmentsController appoitmentsController = new AppoitmentsController();
            Appointments appointment = appoitmentsController.GetAppointment(appointmentId);
            DataContext = appointment;
        }

        private void AppoitmentCancelButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void AppoitmentChangeButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
