using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;
using AppointmentRepository;

namespace AppointmentDomain
{
    public class ProviderDomain : IProviderDomain
    {
        private readonly IProvidersRepository _providersRepository;

        public ProviderDomain(IProvidersRepository providersRepository)
        {
            _providersRepository = providersRepository;
        }

        public Provider GetProvider(int id)
        {
            var provider = _providersRepository.GetProvider(id);
            return provider;
        }

        public List<Provider> GetProviders()
        {
            var providers = _providersRepository.GetProviders();
            return providers;
        }

        public Provider CreateProvider(Provider provider)
        {
            var savedProvider = _providersRepository.CreateProvider(provider);
            return savedProvider;
        }
    }
}
