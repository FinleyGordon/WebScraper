using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using WebScraper.Extractors;
using WebScraper.Models;
using HtmlHelper = WebScraper.Extractors.HtmlHelper;

namespace WebScraper.Parsers;

/// <summary>
/// Parses a solicitor directory results page into a list of <see cref="Solicitor"/> objects.
/// </summary>
public sealed partial class SolicitorHtmlParser(IEnumerable<IFieldExtractor> extractors) : IHtmlParser<Solicitor>
{
    private const string BlockPattern =
        """<div class="result-item[^"]*">.*?(?=<div class="result-item|<div class="banner|</div>\s*</div>\s*</div>)""";

    private readonly IReadOnlyList<IFieldExtractor> _extractors = extractors.ToList();

    public IReadOnlyList<Solicitor> Parse(string html)
    {
        var blocks     = HtmlHelper.SplitIntoBlocks(html, BlockPattern);
        var seen       = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var solicitors = new List<Solicitor>();

        foreach (var block in blocks)
        {
            var fields = ExtractFields(block);

            var name  = fields.GetValueOrDefault("Name");
            var phone = fields.GetValueOrDefault("Phone");

            if (string.IsNullOrWhiteSpace(name)) continue;

            var key = $"{name}|{phone}";
            if (!seen.Add(key)) continue;

            solicitors.Add(BuildSolicitor(fields));
        }

        return solicitors.AsReadOnly();
    }

    private Dictionary<string, string> ExtractFields(string block)
    {
        var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var extractor in _extractors)
        {
            var value = extractor.Extract(block);
            if (!string.IsNullOrWhiteSpace(value))
                fields[extractor.FieldName] = value;
        }

        return fields;
    }

    private static Solicitor BuildSolicitor(Dictionary<string, string> fields) =>
        new()
        {
            Name         = fields.GetValueOrDefault("Name", string.Empty),
            Address      = fields.GetValueOrDefault("Address", string.Empty),
            Phone        = fields.GetValueOrDefault("Phone", string.Empty),
            Website      = fields.GetValueOrDefault("Website"),
            ProfileUrl   = fields.GetValueOrDefault("ProfileUrl"),
            Description  = fields.GetValueOrDefault("Description"),
            Reviews      = ParseReviews(fields.GetValueOrDefault("Reviews")),
            QualityMarks = ParseQualityMarks(fields.GetValueOrDefault("QualityMarks")),
        };

    private static ReviewInfo? ParseReviews(string? raw)
    {
        if (raw is null) return null;
        var parts = raw.Split('|');
        if (parts.Length != 2) return null;

        return double.TryParse(parts[0], out var rating) &&
               int.TryParse(parts[1], out var count)
            ? new ReviewInfo(rating, count)
            : null;
    }

    private static ReadOnlyCollection<string> ParseQualityMarks(string? raw)
    {
        if (raw is null) return [];

        return raw
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => line.Length > 0 && !QualityMarksPretenseRegex().IsMatch(line))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList()
            .AsReadOnly();
    }

    [GeneratedRegex(@"hold the following", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex QualityMarksPretenseRegex();
}
