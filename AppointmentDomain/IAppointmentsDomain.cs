using System.Collections.Generic;
using AppointmentObjects;

namespace AppointmentDomain
{
    public interface IAppointmentsDomain
    {
        Appointment SetAppointment(Appointment appointment);
        Appointment GetAppointment(int id);
        List<Appointment> GetAppointments();
        List<Appointment> GetPatientAppointments(int patientId);
        List<Appointment> GetProviderAppointments(int providerId);
    }
}