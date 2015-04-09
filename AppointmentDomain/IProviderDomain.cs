using System.Collections.Generic;
using AppointmentObjects;

namespace AppointmentDomain
{
    public interface IProviderDomain
    {
        Provider GetProvider(int id);
        List<Provider> GetProviders();
        Provider CreateProvider(Provider provider);
    }
}