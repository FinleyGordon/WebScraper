using System.Text.RegularExpressions;

namespace WebScraper.Extractors;

public sealed class NameExtractor : RegexFieldExtractor
{
    public override string FieldName => "Name";

    protected override string Pattern =>
        @"<span class=""h2"">\s*([^<]+)";
}

public sealed class AddressExtractor : RegexFieldExtractor
{
    public override string FieldName => "Address";
    protected override string Pattern => @"<address>(.*?)</address>";
}

public sealed class PhoneExtractor : RegexFieldExtractor
{
    public override string FieldName => "Phone";

    protected override string Pattern => @"href=""tel:([^""]+)""";

    protected override string Sanitise(string raw) =>
        raw.Trim().TrimStart('(').Replace("(0)", "0");
}

public sealed class WebsiteExtractor : RegexFieldExtractor
{
    public override string FieldName => "Website";

    protected override string Pattern =>
        @"href=""(https?://[^""]+)""[^>]*rel=""nofollow""";

    protected override string Sanitise(string raw) => raw.Trim();
}

public sealed class DescriptionExtractor : RegexFieldExtractor
{
    public override string FieldName => "Description";
    protected override string Pattern => @"<p>(.*?)</p>";

    public override string? Extract(string htmlBlock)
    {
        var value = base.Extract(htmlBlock);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}

public sealed class ReviewExtractor : IFieldExtractor
{
    public string FieldName => "Reviews";

    private static readonly Regex StarFull  = new(@"star-full",  RegexOptions.Compiled);
    private static readonly Regex StarHalf  = new(@"star-half",  RegexOptions.Compiled);
    private static readonly Regex CountExpr = new(@"\((\d+)\)",  RegexOptions.Compiled);

    public string? Extract(string htmlBlock)
    {
        var revBlock = Regex.Match(htmlBlock,
            @"<span class=""rev-results"">(.*?)</span>",
            RegexOptions.Singleline);

        if (!revBlock.Success) return null;

        var fragment  = revBlock.Groups[1].Value;
        var fullStars = StarFull.Matches(fragment).Count;
        var halfStars = StarHalf.Matches(fragment).Count;
        var rating    = fullStars + (halfStars * 0.5);

        var countMatch = CountExpr.Match(fragment);
        return !countMatch.Success ? null : $"{rating}|{countMatch.Groups[1].Value}";
    }
}

public sealed class QualityMarksExtractor : IFieldExtractor
{
    public string FieldName => "QualityMarks";

    private static readonly Regex TitleAttr = new(
        @"class=""greentick[^""]*""\s+title=""([^""]+)""",
        RegexOptions.Singleline | RegexOptions.IgnoreCase);

    public string? Extract(string htmlBlock)
    {
        var match = TitleAttr.Match(htmlBlock);
        return match.Success ? match.Groups[1].Value.Trim() : null;
    }
}
