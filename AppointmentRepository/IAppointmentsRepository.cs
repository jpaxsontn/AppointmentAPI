using System.Collections.Generic;
using AppointmentObjects;

namespace AppointmentRepository
{
    public interface IAppointmentsRepository
    {
        Appointment SetAppointment(Appointment newAppointment);
        Appointment GetAppointment(int id);
        List<Appointment> GetAppointments();
    }
}
