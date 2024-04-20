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
        private readonly Core db = new Core();
        public AppointmentPage(int idAppointment)
        {
            InitializeComponent();
            appointmentId = idAppointment;
            AppoitmentsController appoitmentsController = new AppoitmentsController();
            Appointments appointment = appoitmentsController.GetAppointment(appointmentId);
            DataContext = appointment;

            //если запись только запланированная, то есть возможность ее отмены
            if (appointment.Date >=  DateTime.Now)
            {
                AppoitmentCancelButton.Visibility = Visibility.Visible;
            }

            if (Properties.Settings.Default.StatusUser != "client")
            {
                AppoitmentCancelButton.Visibility = Visibility.Visible;
            }

            //берем все позиции зубов для этой записи
            var oralCavity = db.context.OralCavity?.Where(x => x.AppointmentsId == idAppointment);

            //берем позиции зубов для номера позиции 1 (левые верхние зубы)
            var oralCavityPos1 = oralCavity.Where(x => x.Position == 1);
            for (int i = 1; i <= 8; i++)
            {
                var f = oralCavityPos1?.FirstOrDefault(x => x.Number == i && x.Hygiene == 1);
                TableCell targetCell = Hygiene.Cells[i];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos1?.FirstOrDefault(x => x.Number == i && x.DentalDystopia == 1);
                targetCell = DentalDystopia.Cells[i];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos1?.FirstOrDefault(x => x.Number == i && x.GingivalRecession == 1);
                targetCell = GingivalRecession.Cells[i];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos1?.FirstOrDefault(x => x.Number == i && x.GMA == 1);
                targetCell = GMA.Cells[i];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");
            }
            //берем позиции зубов для номера позиции 2 (правые верхние зубы)
            var oralCavityPos2 = oralCavity.Where(x => x.Position == 2);
            for (int i = 1; i <= 8; i++)
            {
                var f = oralCavityPos2?.FirstOrDefault(x => x.Number == i && x.Hygiene == 1);
                TableCell targetCell = Hygiene.Cells[i + 8];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos2?.FirstOrDefault(x => x.Number == i && x.DentalDystopia == 1);
                targetCell = DentalDystopia.Cells[i + 8];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos2?.FirstOrDefault(x => x.Number == i && x.GingivalRecession == 1);
                targetCell = GingivalRecession.Cells[i + 8];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos2?.FirstOrDefault(x => x.Number == i && x.GMA == 1);
                targetCell = GMA.Cells[i + 8];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");
            }
            //берем позиции зубов для номера позиции 3 (левые нижние зубы)
            var oralCavityPos3 = oralCavity.Where(x => x.Position == 3);
            for (int i = 1; i <= 8; i++)
            {
                var f = oralCavityPos3?.FirstOrDefault(x => x.Number == i && x.Hygiene == 1);
                TableCell targetCell = Hygiene2.Cells[i];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos3?.FirstOrDefault(x => x.Number == i && x.DentalDystopia == 1);
                targetCell = DentalDystopia2.Cells[i];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos3?.FirstOrDefault(x => x.Number == i && x.GingivalRecession == 1);
                targetCell = GingivalRecession2.Cells[i];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos3?.FirstOrDefault(x => x.Number == i && x.GMA == 1);
                targetCell = GMA2.Cells[i];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");
            }
            //берем позиции зубов для номера позиции 2 (правые верхние зубы)
            var oralCavityPos4 = oralCavity.Where(x => x.Position == 4);
            for (int i = 1; i <= 8; i++)
            {
                var f = oralCavityPos4?.FirstOrDefault(x => x.Number == i && x.Hygiene == 1);
                TableCell targetCell = Hygiene2.Cells[i + 8];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos4?.FirstOrDefault(x => x.Number == i && x.DentalDystopia == 1);
                targetCell = DentalDystopia2.Cells[i + 8];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos4?.FirstOrDefault(x => x.Number == i && x.GingivalRecession == 1);
                targetCell = GingivalRecession2.Cells[i + 8];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");

                f = oralCavityPos4?.FirstOrDefault(x => x.Number == i && x.GMA == 1);
                targetCell = GMA2.Cells[i + 8];
                if (f != null)
                    targetCell.ContentStart.InsertTextInRun("+");
            }


            //дочерние приемы
            var daughterAppotintments = db.context.Appointments.Where(x => x.IdParrentAppointments == idAppointment).ToList();
            if (daughterAppotintments.Count > 0)
            {
                foreach (var item in daughterAppotintments)
                {
                    TableRow tableRow = new TableRow();

                    TableCell tableCellDate = new TableCell();
                    tableCellDate.Blocks.Add(new Paragraph(new Run(Convert.ToString(item.Date))));
                    TableCell tableCellDiagnosis = new TableCell();
                    tableCellDiagnosis.Blocks.Add(new Paragraph(new Run(item.Diagnosis)));
                    TableCell tableCellDoctorName = new TableCell();
                    tableCellDoctorName.Blocks.Add(new Paragraph(new Run(item.GetDoctorNameById)));

                    tableRow.Cells.Add(tableCellDate);
                    tableRow.Cells.Add(tableCellDiagnosis);
                    tableRow.Cells.Add(tableCellDoctorName);

                    DaughterAppotintmentsTableRowGroup.Rows.Add(tableRow);
                }
            }
            else
            {
                DaughterAppotintmentsFlowDocumentReader.Visibility = Visibility.Collapsed;
            }
        }

        private void AppoitmentCancelButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите отменить запись?", "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                AppoitmentsController appoitmentsController = new AppoitmentsController();
                appoitmentsController.DeleteAppoitments(appointmentId);
                NavigationService.Refresh();
            }

        }

        private void AppoitmentChangeButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
