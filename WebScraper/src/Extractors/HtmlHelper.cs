using System.Net;
using System.Text.RegularExpressions;

namespace WebScraper.Extractors;

/// <summary>
/// Low-level HTML utilities shared across extractors.
/// </summary>
public static class HtmlHelper
{
    private static readonly Regex TagPattern =
        new(@"<[^>]+>", RegexOptions.Compiled);

    private static readonly Regex WhitespacePattern =
        new(@"\s{2,}", RegexOptions.Compiled);

    /// <summary>Strips all HTML tags then decodes HTML entities.</summary>
    public static string DecodeAndStrip(string html)
    {
        var stripped = TagPattern.Replace(html, string.Empty);
        var decoded  = WebUtility.HtmlDecode(stripped);
        return WhitespacePattern.Replace(decoded, " ").Trim();
    }

    public static IEnumerable<string> SplitIntoBlocks(string html, string blockPattern)
    {
        var matches = Regex.Matches(html, blockPattern,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
        return matches.Select(m => m.Value);
    }
}
