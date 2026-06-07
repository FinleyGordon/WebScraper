namespace WebScraper.Configuration;

public sealed class ScraperConfiguration
{
    public const string SectionName = "Scraper";

    /// <summary>Milliseconds to wait between page fetches</summary>
    public int FetchDelayMs { get; init; } = 500;

    /// <summary>Base URL of the directory site being scraped.</summary>
    public string BaseUrl { get; init; } = "https://www.solicitors.com/";
}
