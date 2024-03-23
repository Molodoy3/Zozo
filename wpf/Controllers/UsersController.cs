using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf.Model;
using System.Security.Cryptography;
using System.Windows;
using System.Text.RegularExpressions;

namespace wpf.Controllers
{
    internal class UsersController
    {
        private Core db = new Core();

        /// <summary>
        /// проверка соответсвия логина и пароля для входа
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool CheckAuth(string login, string password)
        {
            bool result = false;
            string textError = "Данные введены не верно";
            if (string.IsNullOrEmpty(password))
                throw new Exception("Вы не ввели пароль");
            if (string.IsNullOrEmpty(login))
                throw new Exception("Вы не ввели логин");

            var client = db.context.Users.FirstOrDefault(x => x.Login == login);
            if (client != null)
            {
                if (GetHash(password) == client.Password)
                {
                    SetLocalSettings(client.idUser, client.Status);
                    result = true;
                }
                else
                    throw new Exception(textError);
            }
            else
            {
                //var specialist = db.context.Specialistes.FirstOrDefault(x => x.Login == login);
                //if (specialist != null)
                //{
                //    if (GetHash(password) == specialist.Password)
                //    {
                //        SetLocalSettings(specialist.Idspecialist, specialist.Status);
                //        result = true;
                //    }
                //} else
                throw new Exception(textError);
            }
            return result;
        }
        /// <summary>
        /// установка локальных переменных для сохранения сессии пользователя
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="statusUser"></param>
        public void SetLocalSettings(int idUser, string statusUser = "client")
        {
            Properties.Settings.Default.IdUser = idUser;
            Properties.Settings.Default.StatusUser = statusUser;
            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// получение хеша строки(пароля) для безопастности его хранения
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string GetHash(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] b = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(b);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var a in hash)
                stringBuilder.Append(a.ToString("X2"));
            return Convert.ToString(stringBuilder);
        }
        /// <summary>
        /// Проверка введенных данных для регистрации   
        /// </summary>
        /// <param name="dataRegistration"></param>
        /// <param name="dateOfBirthdayUser"></param>
        /// <returns>возврат true, если нет ошибок</returns>
        public bool ValidateDataRegistration(Dictionary<string, string> dataRegistration, DateTime? dateOfBirthdayUser)
        {
            try
            {
                int incr = 0;
                foreach (var dataElement in dataRegistration)
                {
                    //Пустые поля
                    if (incr != 4 && String.IsNullOrEmpty(dataElement.Value))
                        throw new Exception($"Поле {dataElement.Key} не может быть пустым!");
                    //Длина до 100 симв
                    if (incr <= 5)
                    {
                        if (dataElement.Value.Length > 100)
                        {
                            throw new Exception($"Поле {dataElement.Key} не может быть больше 100 символов!");
                        }
                    }
                    incr++;
                }
                //Дата
                if (dateOfBirthdayUser == null)
                    throw new Exception($"Поле дата рождения не может быть пустым!");
                //Логин до 20 символов
                if (dataRegistration["логин"].Length > 20)
                    throw new Exception($"Поле логин не может быть больше 20 символов!");
                //правильность заполнения телефона
                string patternTel = @"^8\d{10}$";
                if (Regex.IsMatch(dataRegistration["телефон"], patternTel))
                    throw new Exception($"Номер телефона написан неправльно! Он должен содержать 10 цифр и начинаться на +7, 7 или 8");
                //Сложность пароля
                string patternPass = @"^(?=.*[A-Za-z])(?=.*\d).{6,}$";
                if (!Regex.IsMatch(dataRegistration["пароль"], patternPass))
                    throw new Exception($"Пароль слишком слабый! Он должен состоять минимум из 6 символов и содержать минимум 1 цифру и 1 латинскую букву.");
                //Совпадение паролей
                if (dataRegistration["пароль"] != dataRegistration["повторение пароля"])
                    throw new Exception($"Пароли не совпадают");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return true;
        }
        public void RegistrationUser(Dictionary<string, string> dataRegistration, DateTime? dateOfBirthdayUser)
        {
            int sexValue = 2;
            if (dataRegistration["пол"] == "Мужчина")
                sexValue = 1;
            Users user = new Users()
            {
                Login = dataRegistration["логин"],
                Firstname = dataRegistration["имя"],
                Lastname = dataRegistration["фамилия"],
                Patronymic = dataRegistration["отчество"],
                Geolocation = dataRegistration["адрес проживания"],
                Profession = dataRegistration["профессия"],
                Password = GetHash(dataRegistration["пароль"]),
                Telephon = dataRegistration["телефон"],
                Sex = sexValue,
                Date = (DateTime)dateOfBirthdayUser,
                Status = "user"
            };

            db.context.Users.Add(user);
            db.context.SaveChanges();
        }
    }
}
