using System;
using System.Diagnostics.Eventing.Reader;
using AppointmentDomain;
using AppointmentObjects;
using AppointmentRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppointmentTests
{
    [TestClass]
    public class AppointmentsDomainTests
    {
        public void Setup()
        {
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());

            var provider = new Provider
            {
                Name = "Dr. Oz",
                CertificationLevel = 10
            };

            var differentProvider = new Provider
            {
                Name = "Dr. Jekyll",
                CertificationLevel = 10
            };

            var patient = new Patient
            {
                Name = "Dorothy",
                Age = 16
            };

            var differentPatient = new Patient
            {
                Name = "Glenda",
                Age = 99
            };

            var service = new Service
            {
                Name = "A New Heart",
                Duration = TimeSpan.FromHours(5),
                MinimumRequiredAge = 16,
                RequiredCertificationLevel = 10
            };

            var shortService = new Service
            {
                Name = "Consultation",
                Duration = TimeSpan.FromMinutes(30),
                MinimumRequiredAge = 16,
                RequiredCertificationLevel = 10
            };

            providerDomain.CreateProvider(provider);
            providerDomain.CreateProvider(differentProvider);
            patientDomain.CreatePatient(patient);
            patientDomain.CreatePatient(differentPatient);
            serviceDomain.CreateService(service);
            serviceDomain.CreateService(shortService);
        }

        [TestMethod]
        public void Scheduling_Appointment_HappyPath()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedProvider = providerDomain.GetProvider(1);
            var savedPatient = patientDomain.GetPatient(1);
            var savedService = serviceDomain.GetService(1);

            var appointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000"),
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);

            //Assert
            Assert.IsTrue(savedAppointment.Id != 0);
        }

        [TestMethod]
        public void Scheduling_Appointment_For_Provider_More_Than_Five_Minutes_After_Previous_Booking_Should_Pass()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedProvider = providerDomain.GetProvider(1);
            var savedService = serviceDomain.GetService(1);
            var savedPatient = patientDomain.GetPatient(1);

            var appointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000"),
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);

            var nextAppointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000").AddHours(savedAppointment.PlannedDuration.Hours).AddMinutes(6),
                ReasonForVisit = "A New Brain"
            };

            var savedNextAppointment = appointmentDomain.SetAppointment(nextAppointment);

            //Assert
            Assert.IsTrue(savedNextAppointment.Id != 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Scheduling_Appointment_Before_Clinic_Hours_Should_Fail()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedProvider = providerDomain.GetProvider(1);
            var savedPatient = patientDomain.GetPatient(1);
            var savedService = serviceDomain.GetService(1);

            var appointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 08:59:59.000"),
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Scheduling_Appointment_After_Clinic_Hours_Should_Fail()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedProvider = providerDomain.GetProvider(1);
            var savedPatient = patientDomain.GetPatient(1);
            var savedService = serviceDomain.GetService(1);

            var appointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 16:00:01.000"),
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Scheduling_Appointment_For_Underage_Patients_Should_Fail()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedProvider = providerDomain.GetProvider(1);
            var savedService = serviceDomain.GetService(1);
            var underagePatient = new Patient
            {
                Age = 15,
                Name = "Lion"
            };

            underagePatient = patientDomain.CreatePatient(underagePatient);

            var appointment = new Appointment
            {
                Patient = underagePatient,
                Provider = savedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000"),
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Scheduling_Appointment_For_Unqualified_Provider_Should_Fail()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedService = serviceDomain.GetService(1);
            var savedPatient = patientDomain.GetPatient(1);
            var unqualifiedProvider = new Provider
            {
                Name = "Dr Quack.",
                CertificationLevel = 1
            };

            unqualifiedProvider = providerDomain.CreateProvider(unqualifiedProvider);

            var appointment = new Appointment
            {
                Patient = savedPatient,
                Provider = unqualifiedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000"),
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Scheduling_Appointment_For_Provider_Less_Than_Five_Minutes_After_Previous_Booking_Should_Fail()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedProvider = providerDomain.GetProvider(1);
            var savedService = serviceDomain.GetService(1);
            var savedPatient = patientDomain.GetPatient(1);

            var appointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000"),
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);

            var overlappingAppointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000").AddHours(savedAppointment.PlannedDuration.Hours).AddMinutes(5),
                ReasonForVisit = "A New Brain"
            };

            var savedOverlappingAppointment = appointmentDomain.SetAppointment(overlappingAppointment);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Scheduling_Appointment_For_Provider_More_Than_Five_Minutes_After_Previous_Booking_But_Overlapping_Next_Booking_Duration_Should_Fail()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedProvider = providerDomain.GetProvider(1);
            var savedService = serviceDomain.GetService(1);
            var savedPatient = patientDomain.GetPatient(1);

            var appointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000"),
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);

            var nextAppointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000").AddHours(savedAppointment.PlannedDuration.Hours + 1),
                ReasonForVisit = "A New Brain"
            };

            var savedNextAppointment = appointmentDomain.SetAppointment(nextAppointment);

            var overlapppingAppointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000").AddHours(savedAppointment.PlannedDuration.Hours).AddMinutes(6),
                ReasonForVisit = "A New Backbone"
            };

            var savedOverlappingAppointment = appointmentDomain.SetAppointment(overlapppingAppointment);
        }


        [TestMethod]
        public void Scheduling_Appointment_For_Provider_More_Than_Five_Minutes_After_Previous_Booking_But_Not_Overlapping_Next_Booking_Should_Pass()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedProvider = providerDomain.GetProvider(1);
            var savedService = serviceDomain.GetService(1);
            var savedPatient = patientDomain.GetPatient(1);
            var savedShortService = serviceDomain.GetService(2);
            

            var appointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000"),
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);

            var nextAppointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000").AddHours(savedAppointment.PlannedDuration.Hours + 1),
                ReasonForVisit = "A New Brain"
            };

            var savedNextAppointment = appointmentDomain.SetAppointment(nextAppointment);

            var inBetweenAppointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedShortService,
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000").AddHours(savedAppointment.PlannedDuration.Hours).AddMinutes(6),
                ReasonForVisit = "I just want to go home"
            };

            var savedInBetweenAppointment = appointmentDomain.SetAppointment(inBetweenAppointment);

            Assert.IsTrue(savedInBetweenAppointment.Id != 0);
        }


        [TestMethod]
        public void Scheduling_Appointment_For_Two_Providers_At_Same_Time_Should_Pass()
        {
            //Arrange
            Setup();
            var providerDomain = new ProviderDomain(new ProvidersRepository());
            var patientDomain = new PatientDomain(new PatientsRepository());
            var serviceDomain = new ServiceDomain(new ServicesRepository());
            var appointmentDomain = new AppointmentsDomain(new AppointmentsRepository());

            var savedProvider = providerDomain.GetProvider(1);
            var savedOtherProvider = providerDomain.GetProvider(2);
            var savedService = serviceDomain.GetService(1);
            var savedPatient = patientDomain.GetPatient(1);

            var appointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000")
            };

            var otherAppointment = new Appointment
            {
                Patient = savedPatient,
                Provider = savedOtherProvider,
                Service = savedService,
                ReasonForVisit = "There's no place like home",
                RequestedAppointmentDate = DateTime.Parse("2000/01/01 09:00:00.000")
            };

            //Act
            var savedAppointment = appointmentDomain.SetAppointment(appointment);
            var savedOtherAppointment = appointmentDomain.SetAppointment(otherAppointment);

            //Assert
            Assert.IsTrue(savedAppointment.Id != 0);
            Assert.IsTrue(savedOtherAppointment.Id != 0);
            Assert.IsTrue(savedAppointment.Id != savedOtherAppointment.Id);
        }
    }
}
