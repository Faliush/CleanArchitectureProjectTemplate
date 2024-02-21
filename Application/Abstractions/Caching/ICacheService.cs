namespace Application.Abstractions.Caching;

public interface ICacheService
{
    Task<T> GetOrCreateAsync<T> (
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? experation = null,
        CancellationToken cancellationToken = default);
}
