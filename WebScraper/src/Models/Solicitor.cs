namespace WebScraper.Models;

/// <summary>
/// Represents a solicitor firm scraped from a directory listing.
/// Pure data model — no parsing or formatting logic lives here.
/// </summary>
public sealed class Solicitor
{
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string? Website { get; init; }
    public string? ProfileUrl { get; init; }
    public string? Description { get; init; }
    public ReviewInfo? Reviews { get; init; }
    public IReadOnlyList<string> QualityMarks { get; init; } = [];

    public override string ToString() =>
        $"{Name} | {Phone} | {Address} | Reviews: {Reviews}";
}
