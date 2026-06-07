namespace WebScraper.Configuration;

public sealed class ScraperConfiguration
{
    public const string SectionName = "Scraper";

    public string BaseUrl { get; init; } = "https://www.solicitors.com/";
    
    public string SolicitorsLocationUrl { get; init; } = "https://solicitors.com/{0}-solicitors.html";
}
