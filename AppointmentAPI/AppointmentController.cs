using System;
using AppointmentAPI.Models;
using AppointmentDomain;
using AppointmentObjects;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace AppointmentAPI
{
    public class AppointmentController : NancyModule
    {
        
        public AppointmentController(IAppointmentsDomain appointmentsDomain, IPatientDomain patientDomain, IProviderDomain providerDomain, IServiceDomain serviceDomain) : base("/appointments")
        {
            Get["/"] = parameters => new JsonResponse(appointmentsDomain.GetAppointments(), new DefaultJsonSerializer());
            Get["/{id}"] = parameters => new JsonResponse(appointmentsDomain.GetAppointment(parameters.id), new DefaultJsonSerializer());
            Get["/patient/{id}"] = parameters => new JsonResponse(appointmentsDomain.GetPatientAppointments(parameters.id), new DefaultJsonSerializer());
            Get["/provider/{id}"] = parameters => new JsonResponse(appointmentsDomain.GetProviderAppointments(parameters.id), new DefaultJsonSerializer());
            Post["/"] = parameters =>
            {
                var appointmentModel = this.Bind<AppointmentModel>();
                var response = appointmentModel.ValidateModel();
                if (response != null)
                {
                    return response;
                }

                var appointment = new Appointment
                {
                    Patient = patientDomain.GetPatient(appointmentModel.PatientId),
                    Provider = providerDomain.GetProvider(appointmentModel.ProviderId),
                    Service = serviceDomain.GetService(appointmentModel.ServiceId),
                    ReasonForVisit = appointmentModel.ReasonForVisit,
                    RequestedAppointmentDate = DateTime.Parse(appointmentModel.RequestedAppointmentDate)
                };

                try
                {
                    var validationResponse = ValidateInputModel(appointment);
                    if (validationResponse != null)
                    {
                        return validationResponse;
                    }
                    appointmentsDomain.SetAppointment(appointment);
                }
                catch (ApplicationException ex)
                {
                    return new Response()
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    }.WithHeader("Error", ex.Message);
                }
                

                string url = string.Format("{0}/{1}", this.Context.Request.Url, appointment.Id);

                return new Response()
                {
                    StatusCode = HttpStatusCode.Accepted
                }.WithHeader("Location", url);
            };
        }

        public Response ValidateInputModel(Appointment appointment)
        {
            if (appointment.Patient == null)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid PatientId");
            }
            if (appointment.Provider == null)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid ProviderId");
            }
            if (appointment.Service == null)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }.WithHeader("Error", "Invalid ServiceId");
            }

            return null;
        }
    }
}