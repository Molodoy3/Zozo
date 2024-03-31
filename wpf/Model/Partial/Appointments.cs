using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf.Model
{
    public partial class Appointments
    {
        public string GetDoctorNameById
        {
            get
            {
                Core core = new Core();
                return "Врач: " + core.context.Users.FirstOrDefault(x => x.idUser == IdDoctor).FIO;
            }
   
        }
    }
}
