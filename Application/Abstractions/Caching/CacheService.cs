using Microsoft.Extensions.Caching.Memory;

namespace Application.Abstractions.Caching;

public sealed class CacheService : ICacheService
{
    private static readonly TimeSpan DefaultExperationTime = TimeSpan.FromMinutes(5);

    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
        => _memoryCache = memoryCache;

    public async Task<T> GetOrCreateAsync<T>(
        string key, 
        Func<CancellationToken, Task<T>> factory, 
        TimeSpan? experation = null, 
        CancellationToken cancellationToken = default)
    {
        T result = await _memoryCache.GetOrCreateAsync(
            key,
            enitry =>
            {
                enitry.SetAbsoluteExpiration(experation ?? DefaultExperationTime);

                return factory(cancellationToken);
            });

        return result;
    }
}
