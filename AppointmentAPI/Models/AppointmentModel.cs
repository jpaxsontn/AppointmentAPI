using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace AppointmentAPI.Models
{
    public class AppointmentModel
    {
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public int ServiceId { get; set; }
        public string ReasonForVisit { get; set; }
        public string RequestedAppointmentDate { get; set; }

        public Response ValidateModel()
        {
            if (PatientId <= 0)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid PatientId");
            }
            if (ServiceId <= 0)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid ServiceId");
            }
            if (ProviderId <= 0)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid ProviderId");
            }
            if (string.IsNullOrEmpty(ReasonForVisit))
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Reason For Visit");
            }

            DateTime validDate;
            if (!DateTime.TryParse(RequestedAppointmentDate, out validDate))
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Appointment Date");
            }
            RequestedAppointmentDate = validDate.ToString();

            if (DateTime.Parse(RequestedAppointmentDate) < DateTime.Today)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Appointment Date Cannot Be In The Past");
            }
            
            return null;
        }
    }
}
