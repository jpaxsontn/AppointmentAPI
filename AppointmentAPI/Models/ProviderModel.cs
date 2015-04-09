using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace AppointmentAPI.Models
{
    public class ProviderModel
    {
        public string Name { get; set; }
        public int CertificationLevel { get; set; }

        public Response ValidateModel()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Name");
            }
            if (CertificationLevel <= 0)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Certification Level");
            }

            return null;
        }
    }
}
