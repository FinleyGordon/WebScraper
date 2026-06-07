namespace WebScraper.Models;

/// <summary>
/// Wraps scrape output with metadata about the operation.
/// Callers get context (source, timing, count) alongside the data.
/// </summary>
public sealed class ScrapeResult
{
    public string SourceUrl { get; init; } = string.Empty;
    public DateTime ScrapedAt { get; init; } = DateTime.UtcNow;
    public IReadOnlyList<Solicitor> Solicitors { get; init; } = [];
}
