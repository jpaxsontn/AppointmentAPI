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
    public class ServiceController : NancyModule
    {
        public ServiceController(IServiceDomain serviceDomain) : base("/service")
        {
            Get["/"] = parameters => new JsonResponse(serviceDomain.GetServices(), new DefaultJsonSerializer());
            Get["/{id}"] = parameters => new JsonResponse(serviceDomain.GetService(parameters.id), new DefaultJsonSerializer());
            Post["/"] = parameters =>
            {
                var service = this.Bind<Service>();
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