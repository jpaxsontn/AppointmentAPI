using System;
using AppointmentAPI.Models;
using AppointmentDomain;
using AppointmentObjects;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace AppointmentAPI
{
    public class ServiceController : NancyModule
    {
        public ServiceController(IServiceDomain serviceDomain) : base("/service")
        {
            Get["/"] = parameters => new JsonResponse(serviceDomain.GetServices(), new DefaultJsonSerializer());
            Get["/{id}"] = parameters => new JsonResponse(serviceDomain.GetService(parameters.id), new DefaultJsonSerializer());
            Post["/"] = parameters =>
            {
                var serviceModel = this.Bind<ServiceModel>();
                var response = serviceModel.ValidateModel();
                if (response != null)
                {
                    return response;
                }

                var service = new Service
                {
                    Name = serviceModel.Name,
                    Duration = TimeSpan.Parse(serviceModel.Duration),
                    MinimumRequiredAge = serviceModel.MinimumRequiredAge,
                    RequiredCertificationLevel = serviceModel.MinimumRequiredAge
                };

                serviceDomain.CreateService(service);

                string url = string.Format("{0}/{1}", this.Context.Request.Url, service.Id);

                return new Response()
                {
                    StatusCode = HttpStatusCode.Accepted
                }.WithHeader("Location", url);
            };
        }
    }
}