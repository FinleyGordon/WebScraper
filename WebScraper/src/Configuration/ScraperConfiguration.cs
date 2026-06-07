namespace WebScraper.Configuration;

public sealed class ScraperConfiguration
{
    public const string SectionName = "Scraper";

    /// <summary>Base URL of the directory site being scraped.</summary>
    public string BaseUrl { get; init; } = "https://www.solicitors.com/";
}
