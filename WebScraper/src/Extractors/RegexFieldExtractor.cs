using System.Text.RegularExpressions;

namespace WebScraper.Extractors;

/// <summary>
/// Base class for regex-based field extractors.
/// Subclasses declare their pattern; this class handles matching
/// </summary>
public abstract class RegexFieldExtractor : IFieldExtractor
{
    public abstract string FieldName { get; }

    /// <summary>Regex pattern. Capture group 1 is the extracted value.</summary>
    protected abstract string Pattern { get; }

    private static RegexOptions Options =>
        RegexOptions.IgnoreCase | RegexOptions.Singleline;

    public virtual string? Extract(string htmlBlock)
    {
        var match = Regex.Match(htmlBlock, Pattern, Options);
        return match.Success ? Sanitise(match.Groups[1].Value) : null;
    }

    /// <summary>
    /// Post-processes the raw captured value.
    /// Override for field-specific cleaning (e.g. phone normalisation).
    /// </summary>
    protected virtual string Sanitise(string raw) =>
        HtmlHelper.DecodeAndStrip(raw).Trim();
}
