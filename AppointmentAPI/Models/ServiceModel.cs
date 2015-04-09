using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace AppointmentAPI.Models
{
    public class ServiceModel
    {
        public string Name { get; set; }
        public int RequiredCertificationLevel { get; set; }
        public string Duration { get; set; }
        public int MinimumRequiredAge { get; set; }

        public Response ValidateModel()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Name");
            }
            if (RequiredCertificationLevel <= 0)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Required Certification Level");
            }
            if (MinimumRequiredAge <= 0)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Minimum Required Age");
            }
            TimeSpan validDuration;
            if (!TimeSpan.TryParse(Duration, out validDuration))
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Service Duration");
            }
            Duration = validDuration.ToString();
            return null;
        }
    }
}
