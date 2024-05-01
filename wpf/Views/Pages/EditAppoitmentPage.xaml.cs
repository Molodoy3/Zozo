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

            //Заполнение таблицы зубов
            var oralCavity = db.context.OralCavity?.Where(x => x.AppointmentsId == idAppointment);
            TableCell targetCell;
            var row1Numbers = new HashSet<int>();
            var row3Numbers = new HashSet<int>();
            foreach (var item in oralCavity)
            {
                if (item.Number < 16)
                {
                    if (!row1Numbers.Contains(item.Number))
                    {
                        targetCell = row1.Cells[item.Number];
                        row1Numbers.Add(item.Number);
                    }
                    else
                        targetCell = row2.Cells[item.Number];
                    targetCell.ContentStart.InsertTextInRun(item.Value);
                }
                else
                {
                    if (!row3Numbers.Contains(item.Number))
                    {
                        targetCell = row3.Cells[item.Number - 16];
                        row3Numbers.Add(item.Number);
                    }
                    else
                        targetCell = row4.Cells[item.Number - 16];
                    targetCell.ContentStart.InsertTextInRun(item.Value);
                }
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

            string[,] dataOralCavity = new string[64, 2];
            //берем все верхние зубы
            for (int i = 0; i <= 15; i++)
            {
                TableCell targetCell = row1.Cells[i];
                Paragraph paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;

                if (paragraph != null && (paragraph.Inlines.FirstOrDefault() is Run run && run.Text != "0" && paragraph.Inlines.FirstOrDefault() is Run run2 && run2.Text != ""))
                {
                    dataOralCavity[i, 0] = i.ToString();
                    Run runValue = paragraph.Inlines.FirstOrDefault() as Run;
                    dataOralCavity[i, 1] = runValue.Text;
                }

                targetCell = row2.Cells[i];
                paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;
                if (paragraph != null && (paragraph.Inlines.FirstOrDefault() is Run run6 && run6.Text != "0" && paragraph.Inlines.FirstOrDefault() is Run run7 && run7.Text != ""))
                {
                    dataOralCavity[i + 15, 0] = i.ToString();
                    Run runValue = paragraph.Inlines.FirstOrDefault() as Run;
                    dataOralCavity[i + 15, 1] = runValue.Text;
                }
            }
            //берем все нижние зубы
            for (int i = 16; i <= 31; i++)
            {
                TableCell targetCell = row3.Cells[i - 16];
                Paragraph paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;

                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run && run.Text != "0" && paragraph.Inlines.FirstOrDefault() is Run run2 && run2.Text != "")
                {
                    dataOralCavity[i, 0] = i.ToString();
                    Run runValue = paragraph.Inlines.FirstOrDefault() as Run;
                    dataOralCavity[i, 1] = runValue.Text;
                }

                targetCell = row4.Cells[i - 16];
                paragraph = targetCell.Blocks.FirstOrDefault() as Paragraph;
                if (paragraph != null && paragraph.Inlines.FirstOrDefault() is Run run6 && run6.Text != "0" && paragraph.Inlines.FirstOrDefault() is Run run7 && run7.Text != "")
                {
                    dataOralCavity[i + 15, 0] = i.ToString();
                    Run runValue = paragraph.Inlines.FirstOrDefault() as Run;
                    dataOralCavity[i + 15, 1] = runValue.Text;
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
                string currentText = ((Run)paragraph.Inlines.First()).Text;
                if (currentText == "R")
                    ((Run)paragraph.Inlines.First()).Text = "C";
                else if (currentText == "C")
                    ((Run)paragraph.Inlines.First()).Text = "P";
                else if (currentText == "P")
                    ((Run)paragraph.Inlines.First()).Text = "Pt";
                else if (currentText == "Pt")
                    ((Run)paragraph.Inlines.First()).Text = "П";
                else if (currentText == "П")
                    ((Run)paragraph.Inlines.First()).Text = "A";
                else if (currentText == "A")
                    ((Run)paragraph.Inlines.First()).Text = "I";
                else if (currentText == "I")
                    ((Run)paragraph.Inlines.First()).Text = "II";
                else if (currentText == "II")
                    ((Run)paragraph.Inlines.First()).Text = "III";
                else if (currentText == "III")
                    ((Run)paragraph.Inlines.First()).Text = "К";
                else if (currentText == "К")
                    ((Run)paragraph.Inlines.First()).Text = "И";
                else if (currentText == "И")
                    ((Run)paragraph.Inlines.First()).Text = "0";
                else
                    ((Run)paragraph.Inlines.First()).Text = "R";
            }
            else
                paragraph.Inlines.Add("R");
        }
    }
}
