using System.Collections.Generic;
using AppointmentObjects;

namespace AppointmentDomain
{
    public interface IServiceDomain
    {
        Service GetService(int id);
        List<Service> GetServices();
        Service CreateService(Service service);
    }
}