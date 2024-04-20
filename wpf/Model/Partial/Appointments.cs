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
                return core.context.Users.FirstOrDefault(x => x.idUser == IdDoctor)?.FIO;
            }
   
        }
        public string DiagnosisParrentAppointments
        {
            get
            {
                Core core = new Core();
                return core.context.Appointments.FirstOrDefault(x => x.IdAppointment == IdParrentAppointments)?.Diagnosis;
            }
        }
        public string PatientAppointment
        {
            get
            {
                Core core = new Core();
                return core.context.Users.FirstOrDefault(x => x.idUser == IdPatient)?.FIO;
            }
        }
        public string headsDepartmentAppointment
        {
            get
            {
                Core core = new Core();
                return core.context.Users.FirstOrDefault(x => x.idUser == IdheadsDepartment)?.FIO;
            }
        }
        public string idAndDiagnosis
        { 
            get
            {
                return "id " + IdAppointment + ": " + Diagnosis;
            }
        }
    }
}
