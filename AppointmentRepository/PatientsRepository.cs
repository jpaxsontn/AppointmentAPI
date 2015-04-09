using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;

namespace AppointmentRepository
{
    public class PatientsRepository : IPatientsRepository
    {
        private readonly ICacheManager _cacheManager;

        public PatientsRepository()
        {
            _cacheManager = new CacheManager();
        }

        public Patient GetPatient(int id)
        {
            var patients = GetPatients();
            var patient = patients.FirstOrDefault(p => p.Id == id);
            return patient;
        }

        public List<Patient> GetPatients()
        {
            var patients = _cacheManager.GetCacheObject<List<Patient>>(CacheKeys.Patients);
            return patients;
        }

        public Patient CreatePatient(Patient newPatient)
        {
            var patients = GetPatients();
            var id = 1;
            if (patients != null)
            {
                id = patients.Max(p => p.Id) + 1;
            }
            else
            {
                patients = new List<Patient>();
            }
            newPatient.Id = id;
            patients.Add(newPatient);
            _cacheManager.PutCacheObject(CacheKeys.Patients, patients);
            return newPatient;
        }
    }
}
