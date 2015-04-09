using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppointmentDomain;
using AppointmentObjects;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace AppointmentAPI
{
    public class PatientController: NancyModule
    {
        
        public PatientController(IPatientDomain patientDomain) : base("/patient")
        {
            Get["/"] = parameters => new JsonResponse(patientDomain.GetPatients(), new DefaultJsonSerializer());
            Get["/{id}"] = parameters => new JsonResponse(patientDomain.GetPatient(parameters.id), new DefaultJsonSerializer());
            Post["/"] = parameters =>
            {
                var patient = this.Bind<Patient>();
                patientDomain.CreatePatient(patient);

                string url = string.Format("{0}/{1}", this.Context.Request.Url, patient.Id);

                return new Response()
                {
                    StatusCode = HttpStatusCode.Accepted
                }.WithHeader("Location", url);
            };
        }
    }
}