namespace OOFM.Ui.Caching;
internal abstract class CacheBase<TKey, TValue> : ICache<TKey, TValue>
    where TKey : notnull
{
    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly Dictionary<TKey, CacheItem<TValue>> _cache = new();

    public TimeSpan PersistanceTime { get; set; }

    public async Task<TValue> Get(TKey key, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync();

        try
        {
            if (_cache.TryGetValue(key, out var cacheItem))
            {
                if (cacheItem.IsValid(PersistanceTime))
                {
                    return cacheItem.Value;
                }
            }

            return (_cache[key] = new CacheItem<TValue>(await ObtainValue(key, cancellationToken), DateTime.Now)).Value;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void ClearCache()
    {
        _cache.Clear();
    }

    protected abstract Task<TValue> ObtainValue(TKey key, CancellationToken cancellationToken);

    struct CacheItem<T>
    {
        public DateTime DateCached;
        public T Value;

        public CacheItem(T value, DateTime dateCached)
        {
            Value = value;
            DateCached = dateCached;
        }

        public bool IsValid(TimeSpan persistanceTime)
        {
            return DateTime.Now.Subtract(DateCached) < persistanceTime;
        }
    }
}
