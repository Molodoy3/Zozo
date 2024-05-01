using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf.Model
{
    partial class Users
    {
        public string PathIconUser
        {
            get
            {
                string appFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string projectFolderPath = Directory.GetParent(appFolderPath).Parent.FullName;
                string targetPath = Path.Combine(projectFolderPath, "Assets/img/userIcons/", idUser + ".jpg");
                return targetPath;
            }
        }
        public string FIO
        {
            get
            {
                return Lastname + " " + Firstname.Substring(0, 1).ToUpper() + "." + Patronymic.Substring(0, 1).ToUpper() + ".";
            }
        }
        public string Age
        {
            get
            {
                return "Возвраст: " + (int)DateTime.Now.Subtract((DateTime)Date).Days / 365 + " лет";
            }
        }
        public string NumberOfVisits
        {
            get
            {
                Core db = new Core();
                return db.context.Appointments.Where(x => x.IdPatient == idUser).Count().ToString();
            }
        }
        public string StatusUser
        {
            get
            {
                string status = "";
                switch (Status)
                {
                    case "admin":
                        status = "Администратор";
                        break;
                    case "HeadsDepartment":
                        status = "Заведующий отделением";
                        break;
                    case "client":
                        status = "Клиент";
                        break;
                    case "specialist":
                        status = "Врач";
                        break;
                    default:
                        break;
                }
                return status;
            }
        }
        public string LastDayAppointment
        {
            get
            {
                Core db = new Core();
                DateTime? latestDate = db.context.Appointments
                .Where(x => x.IdPatient == idUser)
                .Select(x => x.Date)
                .OrderByDescending(x => x)
                .FirstOrDefault();

                // Форматируем дату в виде строки "yy.MM.dd"
                string formattedDate = latestDate.HasValue ? latestDate.Value.ToString("dd.MM.yy") : "отсутсвует";

                // Возвращаем строку с датой
                return formattedDate;
            }
        }
    }
}
