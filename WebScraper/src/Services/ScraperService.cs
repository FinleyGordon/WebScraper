using WebScraper.Configuration;
using WebScraper.Models;
using WebScraper.Parsers;

namespace WebScraper.Services;

/// <summary>
/// Top-level orchestrator for the scrape pipeline.
///
/// Knows WHAT to do and in WHAT ORDER — delegates every detail:
///   - Fetching HTML  → <see cref="IHttpFetcher"/>
///   - Parsing HTML   → <see cref="IHtmlParser{T}"/>
///   - Config values  → <see cref="ScraperConfiguration"/>
///
/// Contains no regex, no file I/O, no CSV columns.
/// Adding a new output format or parser requires zero changes here.
/// </summary>
public sealed class ScraperService(
    IHttpFetcher fetcher,
    IHtmlParser<Solicitor> parser,
    ScraperConfiguration config)
{
    /// <summary>Runs the full pipeline: fetch → parse → export.</summary>
    private async Task<ScrapeResult> LoadScrapeResults(
        string url,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"  Fetching: {url}");

        var html = await fetcher.FetchAsync(url, cancellationToken);

        var solicitors = parser.Parse(html);

        var result = new ScrapeResult
        {
            SourceUrl  = url,
            ScrapedAt  = DateTime.UtcNow,
            Solicitors = solicitors,
        };

        return result;
    }

    public Task<ScrapeResult> RunAsync(string city, CancellationToken cancellationToken = default)
    {
        var url = $"{config.BaseUrl.TrimEnd('/')}/{city.ToLowerInvariant()}-solicitors.html";
        return LoadScrapeResults(url, cancellationToken);
    }
}
