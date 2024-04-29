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
    /// Логика взаимодействия для EditAppoitmentPage.xaml
    /// </summary>
    public partial class EditAppoitmentPage : Page
    {
        private int idAppointment;
        private readonly Core db = new Core();
        private int idDoctor;
        private List<int> arrayIdDaughterAppointmentsForDelete = new List<int>();
        private List<int> arrayIdDaughterAppointmentsForAdd = new List<int> ();
        AppoitmentsController appoitmentsController = new AppoitmentsController();
        public EditAppoitmentPage(int appoitmentId, int doctorId)
        {
            InitializeComponent();
            idDoctor = doctorId;
            idAppointment = appoitmentId;

            Appointments appointment = appoitmentsController.GetAppointment(idAppointment);
            DataContext = appointment;

            //выбор родительской запси
            var appoitments = appoitmentsController.GetAllAppoitmentsForDoctor(idDoctor);
            ParrentAppointmentsComboBox.ItemsSource = appoitments;
            ParrentAppointmentsComboBox.DisplayMemberPath = "idAndReferralText";
            ParrentAppointmentsComboBox.SelectedValuePath = "IdAppointment";
            int idParrentAppointment = appoitmentsController.GetIdParrentAppointment(idAppointment);
            ParrentAppointmentsComboBox.SelectedValue = idParrentAppointment;

            //выбор пациента
            UsersController usersController = new UsersController();
            var users = usersController.getAllUsers();
            PatientsComboBox.ItemsSource = users;
            PatientsComboBox.DisplayMemberPath = "FIO";
            PatientsComboBox.SelectedValuePath = "idUser";
            int idPatientAppointment = appoitmentsController.GetIdPatientAppointment(idAppointment);
            PatientsComboBox.SelectedValue = idPatientAppointment;

            //выбор зав отделением
            var usersHeadsDepartment = usersController.GetAllUsersHeadsDepartment();
            headsDepartmentComboBox.ItemsSource = usersHeadsDepartment;
            headsDepartmentComboBox.DisplayMemberPath = "FIO";
            headsDepartmentComboBox.SelectedValuePath = "idUser";
            int idHeadDepartmentAppointment = appoitmentsController.GetIdHeadDepartmentAppointment(idAppointment);
            headsDepartmentComboBox.SelectedValue = idHeadDepartmentAppointment;

            //выбор доктора
            string statusCurrentUser = Properties.Settings.Default.StatusUser;
            if (statusCurrentUser == "specialist" || statusCurrentUser == "admin" || statusCurrentUser == "HeadsDepartment")
            {
                DoctorTextBlock.Visibility = Visibility.Visible;
                DoctorComboBox.Visibility = Visibility.Visible;
            }
            var usersSpecialistes = usersController.GetAllSpecialistes();
            DoctorComboBox.ItemsSource = usersSpecialistes;
            DoctorComboBox.DisplayMemberPath = "FIO";
            DoctorComboBox.SelectedValuePath = "idUser";
            int idDoctorAppointment = appoitmentsController.GetIdDoctorAppointment(idAppointment);
            DoctorComboBox.SelectedValue = idDoctorAppointment;

            //добавление дочерних приемов
            InitDaughterAppointments(DaughterAppointmentComboBox1);
            InitDaughterAppointments(DaughterAppointmentComboBox2);
            InitDaughterAppointments(DaughterAppointmentComboBox3);

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
            var daughterAppotintments = appoitmentsController.GetDaughterAppotintments(idAppointment);
            if (daughterAppotintments.Count > 0)
            {
                foreach (var item in daughterAppotintments)
                {
                    TableRow tableRow = new TableRow();

                    TableCell tableCellDate = new TableCell();
                    tableCellDate.Blocks.Add(new Paragraph(new Run(Convert.ToString(item.Date))));
                    TableCell tableCellDiagnosis = new TableCell();
                    tableCellDiagnosis.Blocks.Add(new Paragraph(new Run(item.ReferralText)));
                    TableCell tableCellDoctorName = new TableCell();
                    tableCellDoctorName.Blocks.Add(new Paragraph(new Run(item.GetDoctorNameById)));
                    TableCell tableCellDelete = new TableCell();
                    tableCellDelete.Blocks.Add(new Paragraph(new Run("Удалить")));
                    tableCellDelete.Tag = item.IdAppointment;
                    tableCellDelete.MouseLeftButtonDown += TableCellDeleteMouseLeftButtonDown;

                    tableRow.Cells.Add(tableCellDate);
                    tableRow.Cells.Add(tableCellDiagnosis);
                    tableRow.Cells.Add(tableCellDoctorName);
                    tableRow.Cells.Add(tableCellDelete);

                    DaughterAppotintmentsTableRowGroup.Rows.Add(tableRow);
                }
            }
            else
            {
                DaughterAppotintmentsFlowDocumentReader.Visibility = Visibility.Collapsed;
            }
        }

        private void AppoitmentSaveButtonClick(object sender, RoutedEventArgs e)
        {
            object[] dataAppointment = new object[17];
            dataAppointment[0] = dateOfAppointmentCalendar.SelectedDate;
            dataAppointment[1] = ParrentAppointmentsComboBox.SelectedValue;
            dataAppointment[2] = PatientsComboBox.SelectedValue;
            dataAppointment[3] = headsDepartmentComboBox.SelectedValue;
            dataAppointment[4] = DoctorComboBox.SelectedValue;
            dataAppointment[5] = ReferralTextTextBox.Text;
            dataAppointment[6] = DiagnosisTextBox.Text;
            dataAppointment[7] = PastAndConcurrentIllnessesTextBox.Text;
            dataAppointment[8] = DevelopmentRealDiseaseTextBox.Text;
            dataAppointment[9] = ObjectiveResearchDataTextBox.Text;
            dataAppointment[10] = BiteDataTextBox.Text;
            dataAppointment[11] = ConditionCavityTextBox.Text;
            dataAppointment[12] = DataXrayStudiesTextBox.Text;
            dataAppointment[13] = TreatmentTextBox.Text;
            dataAppointment[14] = TreatmentResultsTextBox.Text;
            dataAppointment[15] = InstructionsTextBox.Text;
            dataAppointment[16] = idAppointment;

            //масив id записей для добавления в дочерние

            arrayIdDaughterAppointmentsForAdd.Clear();
            arrayIdDaughterAppointmentsForAdd = new List<int>();
            if (DaughterAppointmentComboBox1.SelectedValue != null)
                arrayIdDaughterAppointmentsForAdd.Add((int)DaughterAppointmentComboBox1.SelectedValue);
            if (DaughterAppointmentComboBox2.SelectedValue != null)
                arrayIdDaughterAppointmentsForAdd.Add((int)DaughterAppointmentComboBox2.SelectedValue);
            if (DaughterAppointmentComboBox3.SelectedValue != null)
                arrayIdDaughterAppointmentsForAdd.Add((int)DaughterAppointmentComboBox3.SelectedValue);

            int[,] dataOralCavity = new int[32, 6];
            //берем все верхние зубы
            for (int i = 1; i <= 16; i++)
            {
                int pos = 1;
                int num = 0;

                TableCell targetCell = Hygiene.Cells[i];
                Paragraph paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;

                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run && run.Text == "+")
                {
                    if (i > 8)
                    {
                        pos = 2;
                        num = i - 8;
                    }
                    else
                    {
                        num = i;
                        pos = 1;
                    }
                    dataOralCavity[i - 1, 0] = pos;
                    dataOralCavity[i - 1, 1] = num;
                    dataOralCavity[i - 1, 2] = 1;
                    dataOralCavity[i - 1, 3] = 0;
                    dataOralCavity[i - 1, 4] = 0;
                    dataOralCavity[i - 1, 5] = 0;
                }

                targetCell = DentalDystopia.Cells[i];
                paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;
                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run2 && run2.Text == "+")
                {
                    if (i > 8)
                    {
                        pos = 2;
                        num = i - 8;
                    }
                    else
                    {
                        pos = 1;
                        num = i;
                    }

                    if (dataOralCavity[i - 1, 0] > 0)
                    {
                        dataOralCavity[i - 1, 3] = 1;
                    }
                    else
                    {
                        dataOralCavity[i - 1, 0] = pos;
                        dataOralCavity[i - 1, 1] = num;
                        dataOralCavity[i - 1, 2] = 0;
                        dataOralCavity[i - 1, 3] = 1;
                        dataOralCavity[i - 1, 4] = 0;
                        dataOralCavity[i - 1, 5] = 0;
                    }
                }

                targetCell = GingivalRecession.Cells[i];
                paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;

                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run3 && run3.Text == "+")
                {
                    if (i > 8)
                    {
                        pos = 2;
                        num = i - 8;
                    }
                    else
                    {
                        pos = 1;
                        num = i;
                    }

                    if (dataOralCavity[i - 1, 0] > 0)
                    {
                        dataOralCavity[i - 1, 4] = 1;
                    }
                    else
                    {
                        dataOralCavity[i - 1, 0] = pos;
                        dataOralCavity[i - 1, 1] = num;
                        dataOralCavity[i - 1, 2] = 0;
                        dataOralCavity[i - 1, 3] = 0;
                        dataOralCavity[i - 1, 4] = 1;
                        dataOralCavity[i - 1, 5] = 0;
                    }
                }

                targetCell = GMA.Cells[i];
                paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;

                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run4 && run4.Text == "+")
                {
                    if (i > 8)
                    {
                        pos = 2;
                        num = i - 8;
                    }
                    else
                    {
                        pos = 1;
                        num = i;
                    }

                    if (dataOralCavity[i - 1, 0] > 0)
                    {
                        dataOralCavity[i - 1, 5] = 1;
                    }
                    else
                    {
                        dataOralCavity[i - 1, 0] = pos;
                        dataOralCavity[i - 1, 1] = num;
                        dataOralCavity[i - 1, 2] = 0;
                        dataOralCavity[i - 1, 3] = 0;
                        dataOralCavity[i - 1, 4] = 0;
                        dataOralCavity[i - 1, 5] = 1;
                    }
                }
            }
            //берем все нижние зубы
            for (int i = 1; i <= 16; i++)
            {
                int pos = 3;
                int num = 0;

                TableCell targetCell = Hygiene2.Cells[i];
                Paragraph paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;

                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run && run.Text == "+")
                {
                    if (i > 8)
                    {
                        pos = 4;
                        num = i - 8;
                    }
                    else
                    {
                        pos = 3;
                        num = i;
                    }
                    dataOralCavity[i - 1, 0] = pos;
                    dataOralCavity[i - 1, 1] = num;
                    dataOralCavity[i - 1, 2] = 1;
                    dataOralCavity[i - 1, 3] = 0;
                    dataOralCavity[i - 1, 4] = 0;
                    dataOralCavity[i - 1, 5] = 0;
                }

                targetCell = DentalDystopia2.Cells[i];
                paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;
                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run2 && run2.Text == "+")
                {
                    if (i > 8)
                    {
                        pos = 4;
                        num = i - 8;
                    }
                    else
                    {
                        pos = 3;
                        num = i;
                    }

                    if (dataOralCavity[i - 1, 0] > 0)
                    {
                        dataOralCavity[i - 1, 3] = 1;
                    }
                    else
                    {
                        dataOralCavity[i - 1, 0] = pos;
                        dataOralCavity[i - 1, 1] = num;
                        dataOralCavity[i - 1, 2] = 0;
                        dataOralCavity[i - 1, 3] = 1;
                        dataOralCavity[i - 1, 4] = 0;
                        dataOralCavity[i - 1, 5] = 0;
                    }
                }

                targetCell = GingivalRecession2.Cells[i];
                paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;

                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run3 && run3.Text == "+")
                {
                    if (i > 8)
                    {
                        pos = 4;
                        num = i - 8;
                    }
                    else
                    {
                        pos = 3;
                        num = i;
                    }

                    if (dataOralCavity[i - 1, 0] > 0)
                    {
                        dataOralCavity[i - 1, 4] = 1;
                    }
                    else
                    {
                        dataOralCavity[i - 1, 0] = pos;
                        dataOralCavity[i - 1, 1] = num;
                        dataOralCavity[i - 1, 2] = 0;
                        dataOralCavity[i - 1, 3] = 0;
                        dataOralCavity[i - 1, 4] = 1;
                        dataOralCavity[i - 1, 5] = 0;
                    }
                }

                targetCell = GMA2.Cells[i];
                paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;

                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run4 && run4.Text == "+")
                {
                    if (i > 8)
                    {
                        pos = 4;
                        num = i - 8;
                    }
                    else
                    {
                        pos = 3;
                        num = i;
                    }

                    if (dataOralCavity[i - 1, 0] > 0)
                    {
                        dataOralCavity[i - 1, 5] = 1;
                    }
                    else
                    {
                        dataOralCavity[i - 1, 0] = pos;
                        dataOralCavity[i - 1, 1] = num;
                        dataOralCavity[i - 1, 2] = 0;
                        dataOralCavity[i - 1, 3] = 0;
                        dataOralCavity[i - 1, 4] = 0;
                        dataOralCavity[i - 1, 5] = 1;
                    }
                }
            }

            try
            {
                appoitmentsController.ValidateAppointmentData(dataAppointment, arrayIdDaughterAppointmentsForAdd, false);

                MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите изменить запись?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    appoitmentsController.EditAppointment(dataAppointment, dataOralCavity, arrayIdDaughterAppointmentsForDelete, arrayIdDaughterAppointmentsForAdd);
                    MessageBox.Show("Запись сохранена!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new AppointmentPage(idAppointment, idDoctor));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void TableCellDeleteMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TableCell tableCell = sender as TableCell;
   
            if (tableCell.Background == Brushes.Red)
            {
                tableCell.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00F0FF"));
                arrayIdDaughterAppointmentsForDelete.Remove((int)tableCell.Tag);
            }
            else
            {
                arrayIdDaughterAppointmentsForDelete.Add((int)tableCell.Tag);
                Console.WriteLine("AAAAA " + tableCell.Tag);
                tableCell.Background = Brushes.Red;
            }
        }
        private void InitDaughterAppointments(ComboBox comboBox)
        {
            var appoitmentsWithoutParrent = appoitmentsController.GetAppoitmentsWithoutParrentForDoctor(idDoctor, idAppointment);
            comboBox.ItemsSource = appoitmentsWithoutParrent;
            comboBox.DisplayMemberPath = "idAndReferralText"; 
            comboBox.SelectedValuePath = "IdAppointment";
        }
        private void MouseLeftButtonDownTableCell(object sender, MouseButtonEventArgs e)
        {
            TableCell tableCell = sender as TableCell;
            Paragraph paragraph = tableCell.Blocks.First() as Paragraph;
            if (paragraph.Inlines.Count > 0)
            {
                if (((Run)paragraph.Inlines.First()).Text == "+")
                {
                    ((Run)paragraph.Inlines.First()).Text = "";
                }
                else
                {
                    ((Run)paragraph.Inlines.First()).Text = "+";
                }
            }
            else
            {
                paragraph.Inlines.Add("+");
            }
        }
    }
}
