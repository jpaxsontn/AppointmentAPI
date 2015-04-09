using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;

namespace AppointmentRepository
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly ICacheManager _cacheManager;

        public ServicesRepository()
        {
            _cacheManager = new CacheManager();
        }

        public List<Service> GetServices()
        {
            var services = _cacheManager.GetCacheObject<List<Service>>(CacheKeys.Services);
            return services;
        }


        public Service GetService(int id)
        {
            var services = GetServices();
            var service = services.FirstOrDefault(p => p.Id == id);
            return service;
        }

        public Service CreateService(Service newService)
        {
            var services = GetServices();
            var id = 1;
            if (services != null)
            {
                id = services.Max(p => p.Id) + 1;
            }
            else
            {
                services = new List<Service>();
            }
            newService.Id = id;
            services.Add(newService);
            _cacheManager.PutCacheObject(CacheKeys.Services, services);
            return newService;
        }
    }
}
