using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentObjects
{
    public class Appointment
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Provider Provider { get; set; }
        public Service Service { get; set; }
        public string ReasonForVisit { get; set; }
        public TimeSpan PlannedDuration { get { return this.Service.Duration; } }
        public DateTime RequestedAppointmentDate { get; set; }

        public Appointment()
        {
            Patient = new Patient();
            Provider = new Provider();
            Service = new Service();
        }
    }
}
