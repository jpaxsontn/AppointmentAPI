using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentObjects;

namespace AppointmentRepository
{
    
    public class ProvidersRepository : IProvidersRepository
    {
        private readonly ICacheManager _cacheManager;

        public ProvidersRepository()
        {
            _cacheManager = new CacheManager();
        }

        public List<Provider> GetProviders()
        {
            var providers = _cacheManager.GetCacheObject<List<Provider>>(CacheKeys.Providers);
            return providers;
        }


        public Provider GetProvider(int id)
        {
            var providers = GetProviders();
            var provider = providers.FirstOrDefault(p => p.Id == id);
            return provider;
        }

        public Provider CreateProvider(Provider newProvider)
        {
            var providers = GetProviders();
            var id = 1;
            if (providers != null)
            {
                id = providers.Max(p => p.Id) + 1;
            }
            else
            {
                providers = new List<Provider>();
            }
            
            newProvider.Id = id;
            providers.Add(newProvider);
            _cacheManager.PutCacheObject(CacheKeys.Providers, providers);
            return newProvider;
        }
    }
}
