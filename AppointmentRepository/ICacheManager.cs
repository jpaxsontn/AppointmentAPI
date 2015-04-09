
namespace AppointmentRepository
{
    public interface ICacheManager
    {
        T GetCacheObject<T>(string token) where T : class;
        void PutCacheObject<T>(string token, T objectType) where T : class;
        void PutCacheObject<T>(string key, T objectType, int expirationMinutes) where T : class;
    }
}
