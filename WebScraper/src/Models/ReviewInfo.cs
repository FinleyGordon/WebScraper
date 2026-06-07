namespace WebScraper.Models;

/// <summary>
/// Value object encapsulating review data.
/// Kept separate from Solicitor so review concerns stay cohesive.
/// </summary>
public sealed record ReviewInfo(double StarRating, int ReviewCount)
{
    public override string ToString() => $"{StarRating:F1} stars ({ReviewCount} reviews)";
}
