using WebScraper.Extractors;

namespace WebScraper.Tests.Extractors;

public sealed class FieldExtractorTests
{
    [Fact]
    public void NameExtractor_DecodesEntitiesAndCollapsesWhitespace()
    {
        var name = new NameExtractor()
            .Extract("""<span class="h2">   Acme   &amp;   Partners   </span>""");

        Assert.Equal("Acme & Partners", name);
    }

    [Theory]
    [InlineData("+44 (0)20 7946 0123", "+44 020 7946 0123")]
    [InlineData("  0113 496 0000  ", "0113 496 0000")]
    public void PhoneExtractor_Sanitises(string raw, string expected)
    {
        var phone = new PhoneExtractor().Extract($"""<a href="tel:{raw}">call</a>""");

        Assert.Equal(expected, phone);
    }

    [Fact]
    public void ReviewExtractor_CombinesFullAndHalfStarsWithCount()
    {
        var fragment =
            """<span class="rev-results"><i class="star-full"></i><i class="star-half"></i> (17)</span>""";

        var result = new ReviewExtractor().Extract(fragment);

        Assert.Equal("1.5|17", result);
    }

    [Fact]
    public void ReviewExtractor_ReturnsNullWhenNoReviewBlock()
    {
        Assert.Null(new ReviewExtractor().Extract("<div>no reviews here</div>"));
    }

    [Fact]
    public void WebsiteExtractor_RequiresNofollowLink()
    {
        var withNofollow =
            """<a href="https://example.com" rel="nofollow">site</a>""";
        var withoutNofollow =
            """<a href="https://example.com">site</a>""";

        Assert.Equal("https://example.com", new WebsiteExtractor().Extract(withNofollow));
        Assert.Null(new WebsiteExtractor().Extract(withoutNofollow));
    }

    [Fact]
    public void Extract_ReturnsNullWhenPatternDoesNotMatch()
    {
        Assert.Null(new AddressExtractor().Extract("<div>no address element</div>"));
    }
}