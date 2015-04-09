using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace AppointmentAPI.Models
{
    public class PatientModel
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Response ValidateModel()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Name");
            }
            if (Age <= 0)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid Age");
            }

            return null;
        }
    }
}