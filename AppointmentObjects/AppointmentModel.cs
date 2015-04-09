using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentObjects
{
    public class AppointmentModel
    {
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public int ServiceId { get; set; }
        public string ReasonForVisit { get; set; }
        public DateTime RequestedAppointmentDate { get; set; }
    }
}
