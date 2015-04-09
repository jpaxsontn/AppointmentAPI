using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;

namespace AppointmentRepository
{
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly ICacheManager _cacheManager;

        public AppointmentsRepository()
        {
            _cacheManager = new CacheManager();
        }

        public Appointment SetAppointment(Appointment newAppointment)
        {
            var appointments = GetAppointments();
            var id = 1;
            if (appointments != null)
            {
                id = appointments.Max(p => p.Id) + 1;
            }
            else
            {
                appointments = new List<Appointment>();
            }
            newAppointment.Id = id;
            appointments.Add(newAppointment);
            _cacheManager.PutCacheObject(CacheKeys.Appointments, appointments);
            return newAppointment;
        }

        public Appointment GetAppointment(int id)
        {
            var appointments = GetAppointments();
            var appointment = appointments.FirstOrDefault(a => a.Id == id);
            return appointment;
        }

        public List<Appointment> GetAppointments()
        {
            var appointments = _cacheManager.GetCacheObject<List<Appointment>>(CacheKeys.Appointments);
            return appointments;
        }

    }
}
