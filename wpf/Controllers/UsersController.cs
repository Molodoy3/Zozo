using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf.Model;
using System.Security.Cryptography;
using System.Windows;
using System.Text.RegularExpressions;
using wpf.Properties;
using wpf.Views.Pages;

namespace wpf.Controllers
{
    internal class UsersController
    {
        private Core db = new Core();
        public Users GetUser(int idUser)
        {
            return db.context.Users.FirstOrDefault(x => x.idUser == idUser);
        }
        public List<Users> GetUsersForSearch(string textForSearch, string statusUser, string date = "")
        {
            if (date == "")
            {
                if (statusUser == "all")
                    return db.context.Users.Where(x => x.Lastname.Contains(textForSearch)).ToList();
                else
                    return db.context.Users.Where(x => x.Lastname.Contains(textForSearch) && x.Status == statusUser).ToList();
            } else
            {
                if (statusUser == "all")
                    return db.context.Users.ToList().Where(x => x.Lastname.Contains(textForSearch) && x.LastDayAppointment == date).ToList();
                else
                    return db.context.Users.ToList().Where(x => x.Lastname.Contains(textForSearch) && x.Status == statusUser && x.LastDayAppointment == date).ToList();
            }

        }
        public string GetSexUser(int idAppointment)
        {
            int idPatient = db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdPatient;
            return db.context.Users.FirstOrDefault(x => x.idUser == idPatient).Sex == 0 ? "Женский" : "Мужской";
        }
        public string GetAge(int idAppointment)
        {
            int idPatient = db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdPatient;
            return db.context.Users.FirstOrDefault(x => x.idUser == idPatient).Age;
        }
        public string GetDoctor(int idAppointment)
        {
            int idDoctor = db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdDoctor;
            return db.context.Users.FirstOrDefault(x => x.idUser == idDoctor).FIO;
        }
        public string GetIdheadsDepartment(int idAppointment)
        {
            int idheadsDepartment = db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdheadsDepartment;
            return db.context.Users.FirstOrDefault(x => x.idUser == idheadsDepartment).FIO;
        }
        public string GetGeo(int idAppointment)
        {
            int idPatient = db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdPatient;
            return db.context.Users.FirstOrDefault(x => x.idUser == idPatient).Geolocation;
        }
        public string GetTel(int idAppointment)
        {
            int idPatient = db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdPatient;
            return db.context.Users.FirstOrDefault(x => x.idUser == idPatient).Telephon;
        }
        public string GetProf(int idAppointment)
        {
            int idPatient = db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdPatient;
            return db.context.Users.FirstOrDefault(x => x.idUser == idPatient).Profession;
        }
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
            Console.WriteLine("AAAAAAA " + idUser);
            Settings.Default.IdUser = idUser;
            Settings.Default.StatusUser = statusUser;
            Settings.Default.Save();
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
        public bool ValidateDataRegistration(Dictionary<string, string> dataRegistration, DateTime? dateOfBirthdayUser, bool needPassword = true)
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
                if (dateOfBirthdayUser > DateTime.Today.AddYears(-18))
                    throw new Exception($"Вам должно быть минимум 18 лет!");
                //Логин до 20 символов
                if (dataRegistration["логин"].Length > 20)
                    throw new Exception($"Поле логин не может быть больше 20 символов!");
                //правильность заполнения телефона
                string patternTel = @"^(\+?7|8)?\d{10}$";
                if (!Regex.IsMatch(dataRegistration["телефон"], patternTel))
                    throw new Exception($"Номер телефона написан неправильно! Он должен содержать 10 цифр и начинаться на +7, 7 или 8");
                //Сложность пароля
                if (needPassword)
                {
                    //Проверка дублирования логина
                    string loginValue = dataRegistration["логин"];
                    var logins = db.context.Users.Where(x => x.Login == loginValue).ToList();
                    if (logins.Count > 0)
                        throw new Exception($"Такой логин уже используется!");
                    string patternPass = @"^(?=.*[A-Za-z])(?=.*\d).{6,}$";
                    if (!Regex.IsMatch(dataRegistration["пароль"], patternPass))
                        throw new Exception($"Пароль слишком слабый! Он должен состоять минимум из 6 символов и содержать минимум 1 цифру и 1 латинскую букву.");
                    //Совпадение паролей
                    if (dataRegistration["пароль"] != dataRegistration["повторение пароля"])
                        throw new Exception($"Пароли не совпадают");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        public void RegistrationUser(Dictionary<string, string> dataRegistration, DateTime? dateOfBirthdayUser, bool needPassword = true, bool isAnotherUser = false)
        {
            int sexValue = 2;
            if (dataRegistration["пол"] == "Мужчина")
                sexValue = 1;

            if (needPassword)
            {
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
                    Status = dataRegistration["статус"]
                };
                db.context.Users.Add(user);
                db.context.SaveChanges();
                //тестить
                if (!isAnotherUser)
                    SetLocalSettings(user.idUser, dataRegistration["статус"]);
            } else
            {
                // Находим пользователя по id
                Users userToUpdate = db.context.Users.Find(Convert.ToInt32(dataRegistration["id"]));
                if (userToUpdate != null)
                {
                    userToUpdate.Login = dataRegistration["логин"];
                    userToUpdate.Firstname = dataRegistration["имя"];
                    userToUpdate.Lastname = dataRegistration["фамилия"];
                    userToUpdate.Patronymic = dataRegistration["отчество"];
                    userToUpdate.Geolocation = dataRegistration["адрес проживания"];
                    userToUpdate.Profession = dataRegistration["профессия"];
                    userToUpdate.Telephon = dataRegistration["телефон"];
                    userToUpdate.Sex = sexValue;
                    userToUpdate.Date = (DateTime)dateOfBirthdayUser;
                    userToUpdate.Status = dataRegistration["статус"];
                }
                else throw new Exception("Пользователь не был найден, скорей всего он удален!");
            }


            db.context.SaveChanges();
        }
        public bool SavePassword(int idUser, string[] passwords)
        {
            string patternPass = @"^(?=.*[A-Za-z])(?=.*\d).{6,}$";
            string hashPassword = GetHash(passwords[0]);
            if (db.context.Users.FirstOrDefault(x => x.Password == hashPassword) == null)
            {
                throw new Exception($"Не верно введен нынешний пароль!");
                return false;
            }
            if (!Regex.IsMatch(passwords[1], patternPass))
            {
                throw new Exception($"Пароль слишком слабый! Он должен состоять минимум из 6 символов и содержать минимум 1 цифру и 1 латинскую букву.");
                return false;
            }
            if (passwords[1] != passwords[2])
            {
                throw new Exception($"Пароли не совпадают!");
                return false;
            }
            var user = db.context.Users.FirstOrDefault(x => x.idUser == idUser);
            if (user != null)
            {
                user.Password = passwords[1];
            }
            db.context.SaveChanges();
            return true;
        }
        public void DeleteUser(int idUser)
        {
            var user = db.context.Users.FirstOrDefault(x => x.idUser == idUser);
            if (user != null)
            {
                db.context.Users.Remove(user);
                db.context.SaveChanges();
            }
        }
        public bool UserIsDoctor(int idUser)
        {
            return db.context.Users.FirstOrDefault(x =>x.idUser == idUser).Status == "specialist";
        }
        public List<Users> getClintsContactDoctor(int idDoctor)
        {
            var appointments = db.context.Appointments.Where(x => x.IdDoctor == idDoctor).ToList();
            var users = new HashSet<Users>();

            foreach (var appointment in appointments)
            {
                var user = db.context.Users.FirstOrDefault(x => x.idUser == appointment.IdPatient);
                if (user != null)
                {
                    users.Add(user);
                }
            }

            return users.ToList();
        }
        public List<Users> getAllUsers()
        {
            return db.context.Users.ToList();
        }
        public List<Users> GetAllUsersHeadsDepartment()
        {
            return db.context.Users.Where(x => x.Status == "HeadsDepartment").ToList();
        }
        public List<Users> GetAllSpecialistes()
        {
            return db.context.Users.Where(x => x.Status == "Specialistes").ToList();
        }
    }
}
