﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
using wpf.Properties;

namespace wpf.Views.Pages.Client
{
    /// <summary>
    /// Логика взаимодействия для ClientPersonalCabinet.xaml
    /// </summary>
    public partial class ClientPersonalCabinet : Page
    {
        int idUser;
        string statusUsingUser = Properties.Settings.Default.StatusUser;
        Core db = new Core();
        List<Appointments> arrayHistoryAppoitments;
        List<Appointments> arrayPlannedAppoitments;
        public ClientPersonalCabinet(int userID)
        {
            InitializeComponent();
            idUser = userID;

            AppoitmentsController appoitmentsController = new AppoitmentsController();
            arrayHistoryAppoitments = appoitmentsController.GetHistoryAppointments(idUser);
            HistoryAppoitmentsListView.ItemsSource = arrayHistoryAppoitments;

            arrayPlannedAppoitments = appoitmentsController.GetPlanedAppointments(idUser);
            PlanedAppoitmentsListView.ItemsSource= arrayPlannedAppoitments;

            //кнопку дневник врача показываем только если пользователь является врачом
            UsersController usersController = new UsersController();
            if (usersController.UserIsDoctor(idUser))
            {
                DoctorDiaryButton.Visibility = Visibility.Visible;
            }
            

            //Properties.Settings.Default.IdUser = 10;
            //idUser = 10;

            mainTitle.Text += " " + db.context.Users.FirstOrDefault(x => x.idUser == idUser).Login;
            if (idUser == Properties.Settings.Default.IdUser)
            {
                ChangeIconUserButton.Visibility = Visibility.Visible;
                ExitButton.Visibility = Visibility.Visible;
            } else if (usersController.UserIsDoctor(idUser))
                NewAppointmentButton.Visibility = Visibility.Visible;

            if (statusUsingUser != "admin")
            {
                deleteUserButton.Visibility = Visibility.Collapsed;
            }
            editDataUserButton.Visibility= Visibility.Collapsed;
            editPasswordUserButton.Visibility= Visibility.Collapsed;
            if (idUser == Properties.Settings.Default.IdUser || statusUsingUser == "admin" || statusUsingUser == "manager" || statusUsingUser == "HeadsDepartment")
            {
                editDataUserButton.Visibility = Visibility.Visible;
                editPasswordUserButton.Visibility = Visibility.Visible;
            }

            Users user = db.context.Users.FirstOrDefault(x => x.idUser == idUser);
            DataContext = user;
            if (user != null)
            {
                Login.Text = user.Login;
                Lastname.Text = user.Lastname;
                Firstname.Text = user.Firstname;
                Patronymic.Text = user.Patronymic;
                Status.Text = user.StatusUser;
                NumberOfVisits.Text = user.NumberOfVisits;
                if (user.Sex == 1)
                    Sex.Text = "Мужской";
                else Sex.Text = "Женский";
                DateOfBirthday.Text = user.Date?.ToString("dd.MM.yy");
                Geolocation.Text = user.Geolocation;
                Telephon.Text = user.Telephon;
                Profession.Text = user.Profession;
            } else
                MessageBox.Show("ваш id не найден в БД. Возможно учетная запись была удалена", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public ClientPersonalCabinet()
        {
        }

        private void editDataUserButtonClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EditingPersonalInformation(idUser));
        }

        private void editPasswordUserButtonClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EditingPassword(idUser));
        }

        private void deleteUserButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите удалить аккаунт?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                bool isThatUser = idUser == Properties.Settings.Default.IdUser;
                if (isThatUser)
                {
                    Properties.Settings.Default.IdUser = 0;
                    Properties.Settings.Default.StatusUser = "";
                    Settings.Default.Save();
                }
                UsersController usersController = new UsersController();
                usersController.DeleteUser(idUser);
                MessageBox.Show("Аккаунт удален!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                if (isThatUser)
                    this.NavigationService.Navigate(new AuthPage());
                else this.NavigationService.Navigate(new MainPage());
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DownloadMedicalRecordsButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Settings.Default.IdUser = 0;
                Settings.Default.StatusUser = "";
                Settings.Default.Save();
                MessageBox.Show("Вы успешно вышли!\nПеренаправление на страницу авторизации.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.Navigate(new AuthPage());
            }

        }

        private void AppoitmentButtonClick(object sender, RoutedEventArgs e)
        {
            Button activeElement = sender as Button;
            Appointments activeAppointment = activeElement.DataContext as Appointments;
            int idAppointment = activeAppointment.IdAppointment;
            NavigationService.Navigate(new AppointmentPage(idAppointment, idUser));
        }

        private void DoctorDiaryClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DoctorDiary(idUser));
        }

        private void ChangeIconUserButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = idUser + ".jpg";

                string appFolderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string projectFolderPath = Directory.GetParent(appFolderPath).Parent.FullName;
                string targetPath = System.IO.Path.Combine(projectFolderPath, "Assets/img/userIcons/", fileName);

                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath); // Удаляем старый файл, если он уже существует
                }

                File.Copy(openFileDialog.FileName, targetPath);
                MessageBox.Show("Изображение успешно загружено");
            }
        }

        private void NewAppointmentButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAppointmentClient(idUser));
        }
    }
}
