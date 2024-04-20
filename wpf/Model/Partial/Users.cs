using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf.Model
{
    partial class Users
    {
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
    }
}
