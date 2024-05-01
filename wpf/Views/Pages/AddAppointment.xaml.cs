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
using System.Xml.Linq;
using wpf.Controllers;
using wpf.Model;

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddAppointment.xaml
    /// </summary>
    public partial class AddAppointment : Page
    {
        public int doctorID;
        AppoitmentsController appoitmentsController = new AppoitmentsController();
        public AddAppointment(int idDoctor)
        {
            InitializeComponent();
            doctorID = idDoctor;

            var appoitments = appoitmentsController.GetAllAppoitmentsForDoctor(doctorID);
            ParrentAppointmentsComboBox.ItemsSource = appoitments;
            ParrentAppointmentsComboBox.DisplayMemberPath = "idAndReferralText";
            ParrentAppointmentsComboBox.SelectedValuePath = "IdAppointment";

            UsersController usersController = new UsersController();
            var users = usersController.getAllUsers();
            PatientsComboBox.ItemsSource = users;
            PatientsComboBox.DisplayMemberPath = "FIO";
            PatientsComboBox.SelectedValuePath = "idUser";

            var usersHeadsDepartment = usersController.GetAllUsersHeadsDepartment();
            headsDepartmentComboBox.ItemsSource = usersHeadsDepartment;
            headsDepartmentComboBox.DisplayMemberPath = "FIO";
            headsDepartmentComboBox.SelectedValuePath = "idUser";

        }

        private void AddAppointmentClick(object sender, RoutedEventArgs e)
        {

            object[] dataAppointment = new object[16];
            dataAppointment[0] = dateOfAppointmentCalendar.SelectedDate;
            dataAppointment[1] = ParrentAppointmentsComboBox.SelectedValue;
            dataAppointment[2] = PatientsComboBox.SelectedValue;
            dataAppointment[3] = headsDepartmentComboBox.SelectedValue;
            dataAppointment[4] = doctorID;
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
                appoitmentsController.ValidateAppointmentData(dataAppointment, new List<int>());
                appoitmentsController.AddAppointment(dataAppointment, dataOralCavity);
                MessageBox.Show("Запись добавлена!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new DoctorDiary(doctorID));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
            } else
                paragraph.Inlines.Add("R");

        }
    }
}
