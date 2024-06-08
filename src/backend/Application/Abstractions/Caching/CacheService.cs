using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Application.Abstractions.Caching;

internal sealed class CacheService(IDistributedCache distributedCache) 
    : ICacheService
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static readonly ConcurrentDictionary<string, bool> _cacheKeys = [];
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
            await _semaphore.WaitAsync(cancellationToken);

            var data = await factory();

            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(data), cancellationToken);
            _cacheKeys.TryAdd(key, true);

            return data;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);

        _cacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        var tasks = _cacheKeys.Keys
            .Where(k => k.StartsWith(prefix))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
