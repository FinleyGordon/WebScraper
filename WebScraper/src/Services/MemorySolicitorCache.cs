using Microsoft.Extensions.Caching.Memory;
using WebScraper.Models;

namespace WebScraper.Services;

public sealed class MemorySolicitorCache(IMemoryCache cache) : ISolicitorCache
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(30);

    public bool TryGet(string city, out ScrapeResult? result) =>
        cache.TryGetValue(Key(city), out result);

    public void Set(string city, ScrapeResult result) =>
        cache.Set(Key(city), result, Ttl);

    private static string Key(string city) => $"solicitors:{city.ToLowerInvariant()}";
}