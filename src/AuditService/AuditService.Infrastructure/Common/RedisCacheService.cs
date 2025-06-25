using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using AuditService.Application.Common;

namespace AuditService.Infrastructure.Common;

public class RedisCacheService(IDistributedCache distributedCache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await distributedCache.GetAsync(key, cancellationToken);
        return value is null ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default)
    {
        DistributedCacheEntryOptions cacheOptions = new();
        if (absoluteExpiration.HasValue && absoluteExpiration.Value > TimeSpan.Zero)
            cacheOptions.SetAbsoluteExpiration(absoluteExpiration.Value);
        var json = JsonSerializer.Serialize(value);
        await distributedCache.SetStringAsync(key, json, cacheOptions, cancellationToken);
    }

    public async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);
        var exists = await ExistsAsync(key, cancellationToken);
        return !exists;
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        byte[]? value = await distributedCache.GetAsync(key, cancellationToken);
        return value is not null;
    }
}
