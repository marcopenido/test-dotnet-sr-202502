using System.Threading.Tasks;

namespace ApplicantTracking.Domain.Services.Caching;

public interface ICacheService
{
    Task SetAsync<T>(string key, T obj);
    Task<T> GetAsync<T>(string key);
}
