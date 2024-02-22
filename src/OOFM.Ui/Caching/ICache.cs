namespace OOFM.Ui.Caching;
internal interface ICache<TKey, TValue>
{
    TimeSpan PersistanceTime { get; set; }

    Task<TValue> Get(TKey key, CancellationToken cancellationToken = default);
    void ClearCache();
}
