using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentObjects
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RequiredCertificationLevel { get; set; }
        public TimeSpan Duration { get; set; }
        public int MinimumRequiredAge { get; set; }
    }
}
