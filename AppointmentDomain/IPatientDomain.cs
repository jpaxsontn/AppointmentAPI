using System.Collections.Generic;
using AppointmentObjects;

namespace AppointmentDomain
{
    public interface IPatientDomain
    {
        Patient GetPatient(int id);
        List<Patient> GetPatients();
        Patient CreatePatient(Patient patient);
    }
}