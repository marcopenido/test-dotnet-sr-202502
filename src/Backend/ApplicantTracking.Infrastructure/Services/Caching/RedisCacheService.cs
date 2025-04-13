using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ApplicantTracking.Domain.Extensions;
using ApplicantTracking.Domain.Services.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ApplicantTracking.Infrastructure.Services.Caching;

public class RedisCacheService : ICacheService
{
    private readonly ILogger<RedisCacheService> _logger;
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
    {
        _logger = logger;
        _cache = cache;
    }

    private readonly DistributedCacheEntryOptions _options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        SlidingExpiration = TimeSpan.FromMinutes(2)
    };

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.Preserve,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public async Task<T> GetAsync<T>(string key)
    {
        try
        {
            var value = await _cache.GetStringAsync(key);

            if (value.Empty())
                return default!;

            var obj = JsonSerializer.Deserialize<T>(value!, _jsonOptions);

            return obj!;
        }
        catch (RedisConnectionException)
        {
            return default!;
        }
    }

    public async Task SetAsync<T>(string key, T obj)
    {
        try
        {
            var value = JsonSerializer.Serialize(obj, _jsonOptions);

            await _cache.SetStringAsync(key, value, _options);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Falha na conexão com a cache Redis para a chave: {CacheKey}", key);
        }
    }
}
