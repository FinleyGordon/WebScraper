namespace WebScraper.Configuration;

public sealed class ScraperConfiguration
{
    public const string SectionName = "Scraper";

    public string BaseUrl { get; init; } = "https://www.solicitors.com/";
}
