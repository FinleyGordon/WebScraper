using WebScraper.Extractors;
using WebScraper.Models;
using WebScraper.Parsers;

namespace WebScraper.Tests.Parsers;

public sealed class SolicitorHtmlParserTests
{
    private static SolicitorHtmlParser CreateParser() =>
        new(
        [
            new NameExtractor(),
            new AddressExtractor(),
            new PhoneExtractor(),
            new WebsiteExtractor(),
            new DescriptionExtractor(),
            new ReviewExtractor(),
            new QualityMarksExtractor(),
        ]);

    private const string SampleHtml =
        """
        <html><body>
        <div class="results">

        <div class="result-item highlighted">
            <span class="h2"> Smith &amp; Co Solicitors </span>
            <address>1 High Street, London, EC1A 1BB</address>
            <a href="tel:+44 20 7946 0123">Call</a>
            <a href="https://smithco.example" target="_blank" rel="nofollow">Website</a>
            <p>Specialists in commercial law.</p>
            <span class="rev-results"><i class="star-full"></i><i class="star-full"></i><i class="star-full"></i><i class="star-full"></i><i class="star-half"></i> (42)</span>
            <span class="greentick lexcel" title="Lexcel Accredited"></span>
        </div>

        <div class="result-item">
            <span class="h2">Jones Legal</span>
            <address>22 Market Square, Leeds, LS1 5AB</address>
            <a href="tel:0113 496 0000">Call</a>
            <p>Family and divorce law.</p>
            <span class="rev-results"><i class="star-full"></i><i class="star-full"></i><i class="star-full"></i> (8)</span>
        </div>

        <div class="result-item">
            <span class="h2"> Smith &amp; Co Solicitors </span>
            <address>1 High Street, London, EC1A 1BB</address>
            <a href="tel:+44 20 7946 0123">Call</a>
        </div>

        <div class="result-item">
            <address>Unnamed firm, nowhere</address>
            <a href="tel:01234 567890">Call</a>
        </div>

        <div class="banner">advertisement</div>

        </div>
        </body></html>
        """;

    [Fact]
    public void Parse_DeduplicatesAndSkipsNamelessBlocks()
    {
        var result = CreateParser().Parse(SampleHtml);

        Assert.Equal(2, result.Count);
        Assert.Equal(["Smith & Co Solicitors", "Jones Legal"], result.Select(s => s.Name));
    }

    [Fact]
    public void Parse_ExtractsAllFieldsForFullyPopulatedFirm()
    {
        var smith = CreateParser().Parse(SampleHtml)[0];

        Assert.Equal("Smith & Co Solicitors", smith.Name);
        Assert.Equal("1 High Street, London, EC1A 1BB", smith.Address);
        Assert.Equal("+44 20 7946 0123", smith.Phone);
        Assert.Equal("https://smithco.example", smith.Website);
        Assert.Equal("Specialists in commercial law.", smith.Description);
        Assert.Equal(new ReviewInfo(4.5, 42), smith.Reviews);
        Assert.Equal(["Lexcel Accredited"], smith.QualityMarks);
    }

    [Fact]
    public void Parse_LeavesOptionalFieldsNullWhenAbsent()
    {
        var jones = CreateParser().Parse(SampleHtml)[1];

        Assert.Equal(new ReviewInfo(3.0, 8), jones.Reviews);
        Assert.Null(jones.Website);
        Assert.Empty(jones.QualityMarks);
    }

    [Fact]
    public void Parse_ReturnsEmptyForHtmlWithNoResults()
    {
        var result = CreateParser().Parse("<html><body><p>No matches found.</p></body></html>");

        Assert.Empty(result);
    }
}