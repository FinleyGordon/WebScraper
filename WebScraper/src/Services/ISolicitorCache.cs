using WebScraper.Models;

namespace WebScraper.Services;

public interface ISolicitorCache
{
    bool TryGet(string? city, out ScrapeResult? result);
    void Set(string city, ScrapeResult result);
}