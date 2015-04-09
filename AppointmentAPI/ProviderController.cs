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
    public class ProviderController : NancyModule
    {
        public ProviderController(IProviderDomain providerDomain) : base("/provider")
        {
            Get["/"] = parameters => new JsonResponse(providerDomain.GetProviders(), new DefaultJsonSerializer());
            Get["/{id}"] = parameters => new JsonResponse(providerDomain.GetProvider(parameters.id), new DefaultJsonSerializer());
            Post["/"] = parameters =>
            {
                var provider = this.Bind<Provider>();
                providerDomain.CreateProvider(provider);

                string url = string.Format("{0}/{1}", this.Context.Request.Url, provider.Id);

                return new Response()
                {
                    StatusCode = HttpStatusCode.Accepted
                }.WithHeader("Location", url);
            };
        }
    }
}