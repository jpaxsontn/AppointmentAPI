using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;
using AppointmentRepository;

namespace AppointmentDomain
{
    public class AppointmentsDomain : IAppointmentsDomain
    {
        private readonly IAppointmentsRepository _appointmentsRepository;


        public AppointmentsDomain(IAppointmentsRepository appointmentsRepository)
        {
            _appointmentsRepository = appointmentsRepository;
        }

        public Appointment SetAppointment(Appointment appointment)
        {
            //1: Appointment must be between 9am and 4pm
            if (appointment.RequestedAppointmentDate.TimeOfDay < DateTime.Parse("2000/01/01 09:00:00.000").TimeOfDay || appointment.RequestedAppointmentDate.TimeOfDay > DateTime.Parse("2000/01/01 16:00:00.000").TimeOfDay)
            {
                throw new ApplicationException("Appointment times must be between 9:00am and 4:00pm.");
            }

            //2: Patient age must be at least MinimumRequiredAge
            if (appointment.Patient.Age < appointment.Service.MinimumRequiredAge)
            {
                throw new ApplicationException("Patient does not meet minimum age requirements for this service.");
            }

            //3: Provider Skill must be at least RequiredCertificationLevel
            if (appointment.Provider.CertificationLevel < appointment.Service.RequiredCertificationLevel)
            {
                throw new ApplicationException("Requested provider does not meet certification requirements.");
            }

            //4: Provider must be available at least 5 minutes before and have no overlapping appointments
            var appointmentAvailable = true;
            var allAppointments = GetAppointments();
            if (allAppointments != null)
            {
                var providerAppointments = GetProviderAppointments(appointment.Provider.Id);
                foreach (var iAppointment in providerAppointments)
                {
                    if (iAppointment.RequestedAppointmentDate.Date == appointment.RequestedAppointmentDate.Date)
                    {
                        var previousAppointmentEndTime = iAppointment.RequestedAppointmentDate.TimeOfDay + iAppointment.PlannedDuration + TimeSpan.FromMinutes(5);
                        var previousAppointmentStartTime = iAppointment.RequestedAppointmentDate.TimeOfDay;
                        var requestedAppointmentStartTime = appointment.RequestedAppointmentDate.TimeOfDay;
                        var requestedAppointmentEndTime = appointment.RequestedAppointmentDate.TimeOfDay + appointment.PlannedDuration + TimeSpan.FromMinutes(5);

                        if (TimeSpan.Compare(previousAppointmentEndTime, requestedAppointmentStartTime) == 1)
                        {
                            if (TimeSpan.Compare(previousAppointmentStartTime, requestedAppointmentEndTime) == -1)
                            {
                                appointmentAvailable = false;
                            }
                        }
                    }
                }
            }

            if (appointmentAvailable == false)
            {
                throw new ApplicationException("Requested appointment conflicts with providers currently scheduled appointments.");
            }

            var savedAppointment = _appointmentsRepository.SetAppointment(appointment);
            return savedAppointment;
        }

        public Appointment GetAppointment(int id)
        {
            var appointment = _appointmentsRepository.GetAppointment(id);
            return appointment;
        }

        public List<Appointment> GetAppointments()
        {
            var allAppointments = _appointmentsRepository.GetAppointments();
            return allAppointments;
        } 

        public List<Appointment> GetPatientAppointments(int patientId)
        {
            var allAppointments = GetAppointments();
            var patientAppointments = allAppointments.Where(p => p.Patient.Id == patientId);
            return patientAppointments.ToList();
        }

        public List<Appointment> GetProviderAppointments(int providerId)
        {
            var allAppointments = GetAppointments();
            var providerAppointments = allAppointments.Where(p => p.Provider.Id == providerId);
            return providerAppointments.ToList();
        }

    }
}
