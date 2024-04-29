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
                    } else
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
                if (((Run)paragraph.Inlines.First()).Text == "+")
                {
                    ((Run)paragraph.Inlines.First()).Text = "";
                }
                else
                {
                    ((Run)paragraph.Inlines.First()).Text = "+";
                }
            } else
            {
                paragraph.Inlines.Add("+");
            }

        }
    }
}
