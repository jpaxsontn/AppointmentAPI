using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;

namespace AppointmentRepository
{
    public interface IPatientsRepository
    {
        Patient GetPatient(int id);
        List<Patient> GetPatients();
        Patient CreatePatient(Patient newPatient);
    }
}
