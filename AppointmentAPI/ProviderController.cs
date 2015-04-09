using AppointmentAPI.Models;
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
                var providerModel = this.Bind<ProviderModel>();
                var response = providerModel.ValidateModel();
                if (response != null)
                {
                    return response;
                }

                var provider = new Provider
                {
                    Name = providerModel.Name,
                    CertificationLevel = providerModel.CertificationLevel
                };

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