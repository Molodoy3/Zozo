using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using wpf.Model;
using wpf.Views.Pages;

namespace wpf.Controllers
{
    internal class AppoitmentsController
    {
        private Core db = new Core();
        public int GetIdPatientAppoitment(int idAppointment)
        {
            return db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdPatient;
        }
        public List<Appointments> GetDaughterAppotintments(int idAppointment)
        {
            return db.context.Appointments.Where(x => x.IdParrentAppointments == idAppointment).ToList();
        }
        public int GetIdParrentAppointment(int idAppointment)
        {
            if (db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdParrentAppointments == null)
                return 0;
            else
                return (int)db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdParrentAppointments;
        }
        public int GetIdDoctorAppointment(int idAppointment)
        {
            return db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdDoctor;
        }
        public int GetIdPatientAppointment(int idAppointment)
        {
            return db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdPatient;
        }
        public int GetIdHeadDepartmentAppointment(int idAppointment)
        {
            return db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdheadsDepartment;
        }
        public List<Appointments> GetAllAppoitmentsForDoctor(int idDoctor)
        {
            return db.context.Appointments.Where(x => x.IdDoctor == idDoctor).ToList();
        }
        public List<Appointments> GetAppoitmentsWithoutParrentForDoctor(int idDoctor, int idAppointment)
        {
            return db.context.Appointments.Where(x => x.IdDoctor == idDoctor && x.IdParrentAppointments == 0 && x.IdAppointment != idAppointment).ToList();
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
        public string GetFIOPatient(int idAppointment)
        {
            int idPatient = db.context.Appointments.FirstOrDefault(x => x.IdAppointment == idAppointment).IdPatient;
            Users user = db.context.Users.FirstOrDefault(x => x.idUser == idPatient);
            return user.Lastname + " " + user.Firstname + " " + user.Patronymic;
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
        public void ValidateAppointmentData(object[] dataAppointment, List<int> arrayIdDaughterAppointmentsForAdd, bool futureDateRequirement = true)
        {
            if (arrayIdDaughterAppointmentsForAdd.Count() != 0)
            {
                bool hasDuplicates = arrayIdDaughterAppointmentsForAdd.Count != arrayIdDaughterAppointmentsForAdd.Distinct().Count();
                if (hasDuplicates)
                    throw new Exception("Дочерние записи не могут повторяться");
            }

            if (dataAppointment[5].ToString().Length < 10)
                throw new Exception("Жалоба должна быть от 10 до 100 символов");

            //проверка даты
            if (futureDateRequirement)
            {
                if (dataAppointment[0] != null)
                {
                    if ((DateTime)dataAppointment[0] < DateTime.Now)
                        throw new Exception("Дата должна быть позже текущей");
                }
                else
                    throw new Exception("Дата не выбрана");
            }


            //проверка айдишников
            if (dataAppointment[2] == null)
                throw new Exception("Пациент не выбран");
            if (dataAppointment[3] == null)
                throw new Exception("Заведующий отделением не выбран");

            //проверка всех текстовых полей на длину до 100 символов
            for (int i = 5; i < 16; i++) {
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
                PastAndConcurrentIllnesses = dataAppointment[7] as string,
                DevelopmentRealDisease = dataAppointment[8] as string,
                ObjectiveResearchData = dataAppointment[9] as string,
                Bite = dataAppointment[10] as string,
                ConditionCavity = dataAppointment[11] as string,
                DataXrayStudies = dataAppointment[12] as string,
                Treatment = dataAppointment[13] as string,
                TreatmentResults = dataAppointment[14] as string,
                Instructions = dataAppointment[15] as string,
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
        public void EditAppointment(object[] dataAppointment, int[,] dataOralCavity, List<int> arrayIdDaughterAppointmentsForDelete, List<int> arrayIdDaughterAppointmentsForAdd)
        {
            //сама запись
            Appointments appointment = db.context.Appointments.Find(dataAppointment[16]);
            if (appointment != null)
            {
                appointment.Date = (DateTime)dataAppointment[0];
                appointment.IdParrentAppointments = dataAppointment[1] as int? ?? 0;
                appointment.IdPatient = (int)dataAppointment[2];
                appointment.IdheadsDepartment = (int)dataAppointment[3];
                appointment.IdDoctor = (int)dataAppointment[4];
                appointment.ReferralText = (string)dataAppointment[5];
                appointment.Diagnosis = (string)dataAppointment[6];
                appointment.PastAndConcurrentIllnesses = (string)dataAppointment[7];
                appointment.DevelopmentRealDisease = (string)dataAppointment[8];
                appointment.ObjectiveResearchData = (string)dataAppointment[9];
                appointment.Bite = (string)dataAppointment[10];
                appointment.ConditionCavity = (string)dataAppointment[11];
                appointment.DataXrayStudies = (string)dataAppointment[12];
                appointment.Treatment = (string)dataAppointment[13];
                appointment.TreatmentResults = (string)dataAppointment[14];
                appointment.Instructions = (string)dataAppointment[15];
            }
            else
                throw new Exception("Запись не была найдена!");

            //зубная таблица
            //сначала удаление старых
            var oldOralCavityItems = db.context.OralCavity.Where(x => x.AppointmentsId == appointment.IdAppointment);
            db.context.OralCavity.RemoveRange(oldOralCavityItems);
            //добавление новых
            for (int i = 0; i < 32; i++)
            {
                if (dataOralCavity[i, 0] == 0)
                    continue;

                OralCavity oralCavity = new OralCavity()
                {
                    AppointmentsId = appointment.IdAppointment,
                    Position = dataOralCavity[i, 0],
                    Number = dataOralCavity[i, 1],
                    Hygiene = dataOralCavity[i, 2],
                    DentalDystopia = dataOralCavity[i, 3],
                    GingivalRecession = dataOralCavity[i, 4],
                    GMA = dataOralCavity[i, 5],
                };
                db.context.OralCavity.Add(oralCavity);
            }

            //далее дочерние приемы
            //удаление
            foreach (var id in arrayIdDaughterAppointmentsForDelete)
            {
                db.context.Appointments.FirstOrDefault(x => x.IdAppointment == id).IdParrentAppointments = null;
            }
            //добавление
            foreach (var id in arrayIdDaughterAppointmentsForAdd)
            {
                db.context.Appointments.FirstOrDefault(x => x.IdAppointment == id).IdParrentAppointments = appointment.IdAppointment;
            }
            db.context.SaveChanges();

        }
    }
}
