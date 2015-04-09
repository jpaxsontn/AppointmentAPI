using System;
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
            Post["/"] = parameters =>
            {
                var appointmentModel = this.Bind<AppointmentModel>();
                var appointment = new Appointment
                {
                    Patient = patientDomain.GetPatient(appointmentModel.PatientId),
                    Provider = providerDomain.GetProvider(appointmentModel.ProviderId),
                    Service = serviceDomain.GetService(appointmentModel.ServiceId),
                    ReasonForVisit = appointmentModel.ReasonForVisit,
                    RequestedAppointmentDate = appointmentModel.RequestedAppointmentDate
                };

                try
                {
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
    }
}