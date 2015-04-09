using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;
using AppointmentRepository;

namespace AppointmentDomain
{
    public class ServiceDomain : IServiceDomain
    {
        private readonly IServicesRepository _servicesRepository;

        public ServiceDomain(IServicesRepository servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        public Service GetService(int id)
        {
            var service = _servicesRepository.GetService(id);
            return service;
        }

        public List<Service> GetServices()
        {
            var services = _servicesRepository.GetServices();
            return services;
        }

        public Service CreateService(Service service)
        {
            var savedService = _servicesRepository.CreateService(service);
            return savedService;
        }
    }
}
