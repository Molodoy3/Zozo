using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using wpf.Model;

namespace wpf.Controllers
{
    internal class AppoitmentsController
    {
        private Core db = new Core();
        public List<Appointments> GetAllAppoitmentsForDoctor(int idDoctor)
        {
            return db.context.Appointments.Where(x => x.IdDoctor == idDoctor).ToList();
        }
        public List<Appointments> GetHistoryAppointments(int idUser)
        {
            return db.context.Appointments.Where(x => x.IdPatient == idUser && x.Date < DateTime.Now).ToList();
        }
        public List<Appointments> GetHistoryAppointmentsForDoctor(int idDoctor)
        {
            return db.context.Appointments.Where(x => x.IdDoctor == idDoctor && x.Date < DateTime.Now).ToList();
        }
        public List<Appointments> GetPlanedAppointments(int idUser)
        {
            return db.context.Appointments.Where(x => x.IdPatient == idUser && x.Date >= DateTime.Now).ToList();
        }
        public List<Appointments> GetPlanedAppointmentsForDoctor(int idDoctor)
        {
            return db.context.Appointments.Where(x => x.IdDoctor == idDoctor && x.Date >= DateTime.Now).ToList();
        }
        public Appointments GetAppointment(int idAppointment)
        {
            return db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment);
        }
        public void DeleteAppoitments(int idAppointment)
        {
            Appointments appointment = db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment);
            if (appointment != null)
            {
                db.context.Appointments.Remove(appointment);
                db.context.SaveChanges();
            }
            else
            {
                try
                {
                    throw new Exception("Произошла ошибка!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public void ValidateAppointmentData(object[] dataAppointment)
        {
            //проверка даты
            if (dataAppointment[0] != null)
            {
                if ((DateTime)dataAppointment[0] < DateTime.Now)
                    throw new Exception("Дата должна быть позже текущей");
            } else
                throw new Exception("Дата не выбрана");

            //проверка айдишников
            if (dataAppointment[2] == null)
                throw new Exception("Пациент не выбран");
            if (dataAppointment[3] == null)
                throw new Exception("Заведующий отделением не выбран");

            //проверка всех текстовых полей на длину до 100 символов
            for (int i = 5; i < dataAppointment.Length; i++) {
                string item = (string)dataAppointment[i];
                if (item.Length > 100)
                    throw new Exception("Ни одно текстовое поле не может иметь длину текста более 100 символов");
            }
        }
        public void AddAppointment(object[] dataAppointment, int[,] dataOralCavity)
        {
            Appointments appointment = new Appointments() {
                Date = (DateTime)dataAppointment[0],
                IdParrentAppointments = dataAppointment[1] as int? ?? 0,
                ReferralText = dataAppointment[5] as string,
                IdPatient = (int)dataAppointment[2],
                IdDoctor = (int)dataAppointment[4],
                Diagnosis = dataAppointment[6] as string,
                DevelopmentRealDisease = dataAppointment[7] as string,
                ObjectiveResearchData = dataAppointment[8] as string,
                Bite = dataAppointment[9] as string,
                ConditionCavity = dataAppointment[10] as string,
                DataXrayStudies = dataAppointment[11] as string,
                Treatment = dataAppointment[12] as string,
                TreatmentResults = dataAppointment[13] as string,
                Instructions = dataAppointment[14] as string,
                IdheadsDepartment = (int)dataAppointment[3],
            };
            db.context.Appointments.Add(appointment);
            int idAppointment = appointment.IdAppointment;

            for (int i = 0; i < 32; i++)
            {
                if (dataOralCavity[i, 0] == 0)
                    continue;

                OralCavity oralCavity = new OralCavity()
                {
                    AppointmentsId = idAppointment,
                    Position = dataOralCavity[i, 0],
                    Number = dataOralCavity[i, 1],
                    Hygiene = dataOralCavity[i, 2],
                    DentalDystopia = dataOralCavity[i, 3],
                    GingivalRecession = dataOralCavity[i, 4],
                    GMA = dataOralCavity[i, 5],
                };
                db.context.OralCavity.Add(oralCavity);
            }
            db.context.SaveChanges();
        }
    }
}
