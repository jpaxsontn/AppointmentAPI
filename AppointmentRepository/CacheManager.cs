using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Web;

namespace AppointmentRepository
{
    public class CacheManager : ICacheManager
    {
        private static readonly CacheManager Manager = new CacheManager();
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        private const int CacheTimeToLive = 3600000;

        public static CacheManager GetInstance()
        {
            return Manager;
        }

        public T GetCacheObject<T>(string token) where T : class
        {
            var cachedDictionary = GetDictionary(token);

            if (cachedDictionary != null)
            {
                object returnedObject;
                var retrievedSuccessfully = cachedDictionary.TryGetValue(typeof (T), out returnedObject);

                if (retrievedSuccessfully)
                {
                    return returnedObject as T;
                }
            }

            return null;
        }

        public void PutCacheObject<T>(string token, T objectType) where T : class
        {
            var cachedDictionary = GetDictionary(token) ?? new ConcurrentDictionary<Type, object>();

            cachedDictionary.AddOrUpdate(typeof (T), objectType, (key, existingVal) => objectType);
            PutDictionary(token, cachedDictionary);
        }

        public void PutCacheObject<T>(string key, T objectType, int expirationMinutes) where T : class
        {
            var cachedDictionary = GetDictionary(key) ?? new ConcurrentDictionary<Type, object>();

            cachedDictionary.AddOrUpdate(typeof (T), objectType, (key1, existingVal) => objectType);
            PutDictionary(key, cachedDictionary, expirationMinutes);
        }

        private ConcurrentDictionary<Type, object> GetDictionary(string token)
        {
            Locker.EnterReadLock();
            var appCache = HttpRuntime.Cache;
            var cachedDictionary = (ConcurrentDictionary<Type, object>) appCache[token];
            Locker.ExitReadLock();

            return cachedDictionary;

        }

        private void PutDictionary(string token, ConcurrentDictionary<Type, object> cachedDictionary)
        {
            Locker.EnterWriteLock();
            var appCache = HttpRuntime.Cache;

            appCache.Insert(
                token, cachedDictionary, null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                TimeSpan.FromMilliseconds(CacheTimeToLive));

            Locker.ExitWriteLock();
        }

        private void PutDictionary(string key, ConcurrentDictionary<Type, object> cachedDictionary, int expirationMinutes)
        {
            Locker.EnterWriteLock();
            var appCache = HttpRuntime.Cache;

            try
            {
                appCache.Insert(
                    key, cachedDictionary, null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(expirationMinutes));
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }
    }
}
