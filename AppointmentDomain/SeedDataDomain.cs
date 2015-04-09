using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;
using AppointmentRepository;

namespace AppointmentDomain
{
    public class SeedDataDomain
    {
        private readonly IAppointmentsDomain _appointmentsDomain;
        private readonly IPatientDomain _patientDomain;
        private readonly IProviderDomain _providerDomain;
        private readonly IServiceDomain _serviceDomain;

        public SeedDataDomain()
        {
            _appointmentsDomain = new AppointmentsDomain(new AppointmentsRepository());
            _patientDomain = new PatientDomain(new PatientsRepository());
            _providerDomain = new ProviderDomain(new ProvidersRepository());
            _serviceDomain = new ServiceDomain(new ServicesRepository());
        }

        public void SeedData()
        {
            var provider_one = new Provider
            {
                Name = "Dr. Oz",
                CertificationLevel = 10
            };

            var provider_two = new Provider
            {
                Name = "Dr. Jekyll",
                CertificationLevel = 10
            };

            var provider_three = new Provider
            {
                Name = "Dr. Houser",
                CertificationLevel = 1
            };

            var patient_one = new Patient
            {
                Name = "Dorothy",
                Age = 16
            };

            var patient_two = new Patient
            {
                Name = "Glenda",
                Age = 99
            };

            var service_one = new Service
            {
                Name = "A New Heart",
                Duration = TimeSpan.FromHours(5),
                MinimumRequiredAge = 16,
                RequiredCertificationLevel = 10
            };

            var service_two = new Service
            {
                Name = "Consultation",
                Duration = TimeSpan.FromMinutes(30),
                MinimumRequiredAge = 18,
                RequiredCertificationLevel = 10
            };

            provider_one = _providerDomain.CreateProvider(provider_one);
            provider_two = _providerDomain.CreateProvider(provider_two);
            provider_three = _providerDomain.CreateProvider(provider_three);
            patient_one = _patientDomain.CreatePatient(patient_one);
            patient_two = _patientDomain.CreatePatient(patient_two);
            service_one = _serviceDomain.CreateService(service_one);
            service_two = _serviceDomain.CreateService(service_two);

            var appointment = new Appointment
            {
                Patient = patient_one,
                Provider = provider_one,
                Service = service_one,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000"),
            };

            _appointmentsDomain.SetAppointment(appointment);
        }
    }
}
