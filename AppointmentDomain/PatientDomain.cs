using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;
using AppointmentRepository;

namespace AppointmentDomain
{
    public class PatientDomain : IPatientDomain
    {
        private readonly IPatientsRepository _patientsRepository;

        public PatientDomain(IPatientsRepository patientsRepository)
        {
            _patientsRepository = patientsRepository;
        }

        public Patient GetPatient(int id)
        {
            var patient = _patientsRepository.GetPatient(id);
            return patient;
        }

        public List<Patient> GetPatients()
        {
            var patients = _patientsRepository.GetPatients();
            return patients;
        }

        public Patient CreatePatient(Patient patient)
        {
            var savedPatient = _patientsRepository.CreatePatient(patient);
            return savedPatient;
        }
    }
}
