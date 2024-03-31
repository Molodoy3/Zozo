using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf.Model;

namespace wpf.Controllers
{
    internal class AppoitmentsController
    {
        private Core db = new Core();
        public List<Appointments> GetHistoryAppointments(int idUser)
        {
            return db.context.Appointments.Where(x => x.IdPatient == idUser && x.Date < DateTime.Now).ToList();
        }
        public List<Appointments> GetPlanedAppointments(int idUser)
        {
            return db.context.Appointments.Where(x => x.IdPatient == idUser && x.Date >= DateTime.Now).ToList();
        }
        public Appointments GetAppointment(int idAppointment)
        {
            return db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment);
        }
    }
}
