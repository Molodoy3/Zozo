using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf.Model;
using System.Security.Cryptography;

namespace wpf.Controllers
{
    internal class UsersController
    {
        Core db = new Core();
        public string CheckAuth(string login, string password)
        {
            string textError = "Данные введены не верно";
            string result = "";
            if (string.IsNullOrEmpty(password))
                throw new Exception("Вы не ввели пароль");
            if (string.IsNullOrEmpty(login))
                throw new Exception("Вы не ввели логин");

            var user = db.context.Users.FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                Console.WriteLine(getHash(password), user.Password);
                if (getHash(password) == user.Password)
                {
                    result = "client";
                }
                else
                    throw new Exception(textError);
            }
            else
            {
                var specialist = db.context.Specialistes.FirstOrDefault(x => x.Login == login);
                if (specialist != null)
                {
                    if (getHash(password) == specialist.Password)
                    {
                        switch (specialist.Status)
                        {
                            case "specialist":
                                result = "specialist";
                                break;
                            case "manager":
                                result = "manager";
                                break;
                            case "admin":
                                result = "admin";
                                break;
                            default:
                                break;
                        }
                    }
                } else
                   throw new Exception("f");
            }

            return result;
        }
        public void SetLocalSettings(string status)
        {
            //Properties.Settings.Default.idRole = user.IdRole;
            //Properties.Settings.Default.idClient = user.IdUser;
            //Properties.Settings.Default.Save();
        }
        private string getHash(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] b = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(b);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var a in hash)
                stringBuilder.Append(a.ToString("X2"));
            return stringBuilder.ToString();
        }
    }
}
