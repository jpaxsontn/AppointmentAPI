using System.Collections.Generic;
using AppointmentObjects;

namespace AppointmentRepository
{
    public interface IServicesRepository
    {
        List<Service> GetServices();
        Service GetService(int id);
        Service CreateService(Service newService);
    }
}