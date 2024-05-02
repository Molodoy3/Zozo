using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using wpf.Controllers;
using wpf.Model;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using Row = DocumentFormat.OpenXml.Spreadsheet.Row;
using Microsoft.Office.Interop.Excel;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
//using DocumentFormat.OpenXml.Wordprocessing;

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AppointmentPage.xaml
    /// </summary>
    public partial class AppointmentPage : System.Windows.Controls.Page
    {
        private int appointmentId;
        private readonly Core db = new Core();
        private int idDoctor;
        AppoitmentsController appoitmentsController = new AppoitmentsController();
        UsersController usersController = new UsersController();
        public AppointmentPage(int idAppointment, int doctorId)
        {
            InitializeComponent();
            idDoctor = doctorId;
            appointmentId = idAppointment;
            Appointments appointment = appoitmentsController.GetAppointment(appointmentId);
            DataContext = appointment;

            if (DateTime.Now > appointment.Date)
                AppoitmentExtractButton.Visibility = Visibility.Visible;

            //если запись только запланированная, то есть возможность ее отмены
            if (appointment.Date >=  DateTime.Now)
            {
                if (Properties.Settings.Default.StatusUser != "client")
                {
                    AppoitmentCancelButton.Visibility = Visibility.Visible;
                } else
                //если клиент, то проверяем ему ли принадлежит запись и в зависимости от этого выводим кнопку отмены
                {
                    int idPatientAppoitment = appoitmentsController.GetIdPatientAppoitment(appointmentId);
                    if (idPatientAppoitment == Properties.Settings.Default.IdUser)
                        AppoitmentCancelButton.Visibility = Visibility.Visible;
                }
            }
            //если не клиент можно редактировать запись
            if (Properties.Settings.Default.StatusUser != "client")
            {
                AppoitmentChangeButton.Visibility = Visibility.Visible;
            }

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
                    } else
                        targetCell = row2.Cells[item.Number];
                    targetCell.ContentStart.InsertTextInRun(item.Value);
                } else
                {
                    if (!row3Numbers.Contains(item.Number))
                    {
                        targetCell = row3.Cells[item.Number - 16];
                        row3Numbers.Add(item.Number);
                    } else
                        targetCell = row4.Cells[item.Number - 16];
                    targetCell.ContentStart.InsertTextInRun(item.Value);
                }
            }


            //дочерние приемы
            var daughterAppotintments = db.context.Appointments.Where(x => x.IdParrentAppointments == idAppointment).ToList();
            if (daughterAppotintments.Count > 0)
            {
                foreach (var item in daughterAppotintments)
                {
                    TableRow tableRow = new TableRow();

                    TableCell tableCellDate = new TableCell();
                    tableCellDate.Blocks.Add(new Paragraph(new System.Windows.Documents.Run(Convert.ToString(item.Date))));
                    TableCell tableCellDiagnosis = new TableCell();
                    tableCellDiagnosis.Blocks.Add(new Paragraph(new System.Windows.Documents.Run(item.ReferralText)));
                    TableCell tableCellDoctorName = new TableCell();
                    tableCellDoctorName.Blocks.Add(new Paragraph(new System.Windows.Documents.Run(item.GetDoctorNameById)));

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
                appoitmentsController.DeleteAppoitments(appointmentId);
                NavigationService.Refresh();
            }

        }

        private void AppoitmentChangeButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditAppoitmentPage(appointmentId, idDoctor));
        }

        private void AppoitmentExtractButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Appointments appointment = appoitmentsController.GetAppointment(appointmentId);

                string appFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string projectFolderPath = Directory.GetParent(appFolderPath).Parent.FullName;
                string sourceFilePath = Path.Combine(projectFolderPath, "Assets/sampleCertificate.xlsx");
                string targetFilePath = Path.Combine(Path.GetTempPath(), "Запись.xlsx");

                // Копирование исходного файла в новое местоположение
                File.Copy(sourceFilePath, targetFilePath, true);

                // Открытие копии файла Excel
                Excel.Application objApp = new Excel.Application();
                Excel.Workbook objWorkbook = objApp.Workbooks.Open(targetFilePath);
                Excel.Sheets objSheets = objWorkbook.Worksheets;
                _Worksheet objSheet = (_Worksheet)objSheets.get_Item(1);

                Range cell = (Range)objSheet.Cells[1, 3];
                if (cell != null)
                    cell.Value2 = "Код формы по ОКУД 0402008";

                cell = (Range)objSheet.Cells[2, 3];
                if (cell != null)
                    cell.Value2 = "Код учреждения по ОКПО ОК 007- 93";

                cell = (Range)objSheet.Cells[6, 1];
                if (cell != null)
                    cell.Value2 = "Стоматологическая клиника \"Зузу\"";
                cell = (Range)objSheet.Cells[10, 1];
                if (cell != null)
                    cell.Value2 = $"N {appointment.IdAppointment} {DateTime.Now.Year}Г.";
                cell = (Range)objSheet.Cells[12, 2];
                if (cell != null)
                    cell.Value2 = appoitmentsController.GetFIOPatient(appointment.IdAppointment);
                cell = (Range)objSheet.Cells[6, 1];
                if (cell != null)
                    cell.Value2 = usersController.GetSexUser(appointment.IdAppointment);
                cell = (Range)objSheet.Cells[13, 1];
                if (cell != null)
                    cell.Value2 = "Пол " + usersController.GetSexUser(appointment.IdAppointment);
                cell = (Range)objSheet.Cells[13, 3];
                if (cell != null)
                    cell.Value2 = usersController.GetAge(appointment.IdAppointment);    
                cell = (Range)objSheet.Cells[14, 2];
                if (cell != null)
                    cell.Value2 = usersController.GetGeo(appointment.IdAppointment);
                cell = (Range)objSheet.Cells[15, 2];
                if (cell != null)
                    cell.Value2 = usersController.GetProf(appointment.IdAppointment);
                cell = (Range)objSheet.Cells[16, 2];
                if (cell != null)
                    cell.Value2 = appointment.Diagnosis;
                cell = (Range)objSheet.Cells[17, 2];
                if (cell != null)
                    cell.Value2 = appointment.ReferralText;
                cell = (Range)objSheet.Cells[19, 2];
                if (cell != null)
                    cell.Value2 = appointment.PastAndConcurrentIllnesses;
                cell = (Range)objSheet.Cells[22, 2];
                if (cell != null)
                    cell.Value2 = appointment.DevelopmentRealDisease;

                //Страница вторая
                _Worksheet objSheet2 = (_Worksheet)objSheets.get_Item(2);
                cell = (Range)objSheet2.Cells[3, 2];
                if (cell != null)
                    cell.Value2 = appointment.ObjectiveResearchData;
                cell = (Range)objSheet2.Cells[15, 2];
                if (cell != null)
                    cell.Value2 = appointment.Bite;
                cell = (Range)objSheet2.Cells[16, 3];
                if (cell != null)
                    cell.Value2 = appointment.ConditionCavity;
                cell = (Range)objSheet2.Cells[19, 2];
                if (cell != null)
                    cell.Value2 = appointment.DataXrayStudies;
                //Карта зубов
                var oralCavity = db.context.OralCavity?.Where(x => x.AppointmentsId == appointmentId);
                var row1Numbers = new HashSet<int>();
                var row3Numbers = new HashSet<int>();
                foreach (var item in oralCavity)
                {
                    if (item.Number < 16)
                    {
                        if (!row1Numbers.Contains(item.Number))
                        {
                            cell = (Range)objSheet2.Cells[8, item.Number + 2];
                            row1Numbers.Add(item.Number);
                        }
                        else
                            cell = (Range)objSheet2.Cells[9, item.Number + 2];
                        cell.Value2 = item.Value;
                    }
                    else
                    {
                        if (!row3Numbers.Contains(item.Number))
                        {
                            cell = (Range)objSheet2.Cells[11, item.Number - 16 + 2];
                            row3Numbers.Add(item.Number);
                        }
                        else
                            cell = (Range)objSheet2.Cells[12, item.Number - 16 + 2];
                        cell.Value2 = item.Value;
                    }
                }

                //Страница третья
                _Worksheet objSheet3 = (_Worksheet)objSheets.get_Item(3);
                cell = (Range)objSheet3.Cells[2, 2];
                if (cell != null)
                    cell.Value2 = appointment.Treatment;
                cell = (Range)objSheet3.Cells[17, 2];
                if (cell != null)
                    cell.Value2 = appointment.TreatmentResults;
                cell = (Range)objSheet3.Cells[21, 2];
                if (cell != null)
                    cell.Value2 = appointment.Instructions;
                cell = (Range)objSheet3.Cells[25, 1];
                if (cell != null)
                    cell.Value2 = "Лечащий врач " + usersController.GetDoctor(appointment.IdAppointment);
                cell = (Range)objSheet3.Cells[25, 3];
                if (cell != null)
                    cell.Value2 = usersController.GetIdheadsDepartment(appointment.IdAppointment);

                //дочерние приемы
                var daughterAppotintments = appoitmentsController.GetDaughterAppotintments(appointmentId);
                int i = 4;
                foreach (var daughterAppotintment in daughterAppotintments)
                {
                    cell = (Range)objSheet3.Cells[i, 1];
                    if (cell != null)
                        cell.Value2 = daughterAppotintment.Date.ToString("dd.MM.yyyy");
                    cell = (Range)objSheet3.Cells[i, 2];
                    if (cell != null)
                        cell.Value2 = daughterAppotintment.ReferralText;
                    cell = (Range)objSheet3.Cells[i, 3];
                    if (cell != null)
                        cell.Value2 = usersController.GetDoctor(daughterAppotintment.IdAppointment);
                }


                // Сохранение изменений
                objWorkbook.Save();

                // Закрытие и освобождение ресурсов
                objWorkbook.Close();
                objApp.Quit();

                // ... (код для копирования, изменения и сохранения файла)

                // Получение байтов измененного файла
                byte[] fileBytes = File.ReadAllBytes(targetFilePath);

                // Удаление скопированного файла
                File.Delete(targetFilePath);

                // Предложение пользователю сохранить файл
                SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save modified Excel File";
                saveFileDialog.FileName = "sampleCertificate_modified.xlsx";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllBytes(filePath, fileBytes);
                    MessageBox.Show("Файл успешно сохранен!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                //using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, true))
                //{
                //    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                //    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                //    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();



                //    // Изменение значения ячейки (например, A1)
                //    Cell cell = sheetData.GetFirstChild<Row>().Elements<Cell>().FirstOrDefault(c => c.CellReference == "A1");

                //    cell.CellValue = new CellValue("Код формы по ОКУД " + appointment.IdAppointment);
                //    cell.DataType = new EnumValue<CellValues>(CellValues.String);

                //    Cell cellC1 = sheetData.GetFirstChild<Row>().Elements<Cell>().FirstOrDefault(c => c.CellReference == "C1");

                //    // Теперь вы можете обновить значение ячейки, если необходимо
                //    if (cellC1 != null)
                //    {
                //        cellC1.CellValue = new CellValue("Новое значение для C1");
                //        cellC1.DataType = new EnumValue<CellValues>(CellValues.String);
                //    }

                //    // Сохранение изменений
                //    worksheetPart.Worksheet.Save();
                //}
                //// Открытие диалогового окна для сохранения файла
                //SaveFileDialog saveDialog = new SaveFileDialog();
                //saveDialog.FileName = "Новый_файл.xlsx";
                //saveDialog.DefaultExt = ".xlsx";
                //saveDialog.FileName = filePath;
                //saveDialog.Filter = "Excel документы (.xlsx)|*.xlsx";

                //// Проверка, что пользователь нажал "Сохранить"
                //if (saveDialog.ShowDialog() == true)
                //{
                //    File.Copy(filePath, saveDialog.FileName, true);
                //    MessageBox.Show("Файл успешно сохранен");
                //}
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
