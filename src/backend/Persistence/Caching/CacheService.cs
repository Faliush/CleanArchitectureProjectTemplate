using System.Collections.Concurrent;
using System.Text.Json;
using Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Persistence.Caching;

internal sealed class CacheService(IDistributedCache distributedCache) 
    : ICacheService
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = [];
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) 
        where T : class
    {
        var cachedData = await _distributedCache.GetStringAsync(key, cancellationToken);

        if(cachedData is not null)
        {
            return JsonSerializer.Deserialize<T>(cachedData)!;
        }

        try
        {
            await Semaphore.WaitAsync(cancellationToken);

            var data = await factory();

            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(data), cancellationToken);
            CacheKeys.TryAdd(key, true);

            return data;
        }
        finally
        {
            Semaphore.Release();
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        var tasks = CacheKeys.Keys
            .Where(k => k.StartsWith(prefix))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
