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

namespace wpf.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditingPassword.xaml
    /// </summary>
    public partial class EditingPassword : Page
    {
        int UserID;
        public EditingPassword(int idUser)
        {
            InitializeComponent();
            UserID = idUser;
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            string[] passwords =
            {
                OldPasswordTextBox.Password,
                PasswordTextBox.Password,
                PasswordDoubleTextBox.Password
            };
            try
            {
                UsersController usersController = new UsersController();
                usersController.SavePassword(UserID, passwords);
                MessageBox.Show("Данные сохранены. Перенаправление...", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}
