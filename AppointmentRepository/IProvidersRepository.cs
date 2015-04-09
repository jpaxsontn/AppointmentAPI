using System.Collections.Generic;
using AppointmentObjects;

namespace AppointmentRepository
{
    public interface IProvidersRepository
    {
        List<Provider> GetProviders();
        Provider GetProvider(int id);
        Provider CreateProvider(Provider newProvider);
    }
}
