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
        private Core db = new Core();
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
                    SetLocalSettings(client.idUser);
                    result = true;
                }
                else
                    throw new Exception(textError);
            }
            else
            {
                var specialist = db.context.Specialistes.FirstOrDefault(x => x.Login == login);
                if (specialist != null)
                {
                    if (GetHash(password) == specialist.Password)
                    {
                        SetLocalSettings(specialist.Idspecialist, specialist.Status);
                        result = true;
                    }
                } else
                   throw new Exception("f");
            }
            return result;
        }
        public void SetLocalSettings(int idUser, string statusUser = "client")
        {
            Properties.Settings.Default.IdUser = idUser;
            Properties.Settings.Default.StatusUser = statusUser;
            Properties.Settings.Default.Save();
        }
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
    }
}
