using WebScraper.Configuration;
using WebScraper.Models;
using WebScraper.Parsers;

namespace WebScraper.Services;

/// <summary>
/// Top-level service for scraping.
/// </summary>
public sealed class SolicitorScraperService(
    IHttpFetcher fetcher,
    IHtmlParser<Solicitor> parser,
    ScraperConfiguration config,
    ISolicitorCache cache,
    ILogger<SolicitorScraperService> logger)
{
    public async Task<ScrapeResult> RetrieveSolicitors(string city, CancellationToken cancellationToken = default)
    {
        var normalisedCity = city.Trim();

        if (cache.TryGet(normalisedCity, out var cached) && cached is not null)
        {
            logger.LogDebug("Cache hit for city {City}", normalisedCity);
            return cached;
        }

        var url = string.Format(config.SolicitorsLocationUrl, Uri.EscapeDataString(normalisedCity));
        var result = await LoadScrapeResults(url, cancellationToken);

        cache.Set(normalisedCity, result);
        return result;
    }

    private async Task<ScrapeResult> LoadScrapeResults(string url, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching solicitors from {Url}", url);

        string html;
        try
        {
            html = await fetcher.FetchAsync(url, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to fetch solicitors from {Url}", url);
            throw new ScrapeException("Unable to retrieve solicitor listings from the upstream directory.", ex);
        }

        var solicitors = parser.Parse(html);

        return new ScrapeResult
        {
            SourceUrl  = url,
            ScrapedAt  = DateTime.UtcNow,
            Solicitors = solicitors,
        };
    }
}