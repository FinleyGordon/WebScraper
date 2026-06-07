using Microsoft.Extensions.Logging.Abstractions;
using WebScraper.Configuration;
using WebScraper.Models;
using WebScraper.Parsers;
using WebScraper.Services;

namespace WebScraper.Tests.Services;

public sealed class SolicitorScraperServiceTests
{
    private static readonly ScraperConfiguration Config = new()
    {
        SolicitorsLocationUrl = "https://solicitors.com/{0}-solicitors.html",
    };

    private static SolicitorScraperService CreateService(
        IHttpFetcher fetcher,
        ISolicitorCache cache,
        IHtmlParser<Solicitor>? parser = null) =>
        new(
            fetcher,
            parser ?? new StubParser([new Solicitor { Name = "Acme Law" }]),
            Config,
            cache,
            NullLogger<SolicitorScraperService>.Instance);

    [Fact]
    public async Task RetrieveSolicitors_ReturnsCachedResultWithoutFetching()
    {
        var cached = new ScrapeResult { SourceUrl = "cached" };
        var fetcher = new StubFetcher();
        var service = CreateService(fetcher, new StubCache(cached));

        var result = await service.RetrieveSolicitors("London");

        Assert.Same(cached, result);
        Assert.Equal(0, fetcher.CallCount);
    }

    [Fact]
    public async Task RetrieveSolicitors_FetchesParsesAndCachesOnMiss()
    {
        var fetcher = new StubFetcher("<html/>");
        var cache = new StubCache();
        var service = CreateService(fetcher, cache);

        var result = await service.RetrieveSolicitors("London");

        Assert.Equal(1, fetcher.CallCount);
        Assert.Single(result.Solicitors);
        Assert.NotNull(cache.LastSet);
        Assert.Same(result, cache.LastSet);
    }

    [Fact]
    public async Task RetrieveSolicitors_TrimsAndUrlEncodesCity()
    {
        var fetcher = new StubFetcher("<html/>");
        var service = CreateService(fetcher, new StubCache());

        await service.RetrieveSolicitors("  New York  ");

        Assert.Equal("https://solicitors.com/New%20York-solicitors.html", fetcher.LastUrl);
    }

    [Fact]
    public async Task RetrieveSolicitors_WrapsFetchFailureInScrapeException()
    {
        var fetcher = new StubFetcher(new HttpRequestException("boom"));
        var service = CreateService(fetcher, new StubCache());

        var ex = await Assert.ThrowsAsync<ScrapeException>(
            () => service.RetrieveSolicitors("London"));

        Assert.IsType<HttpRequestException>(ex.InnerException);
    }

    private sealed class StubFetcher : IHttpFetcher
    {
        private readonly string _html;
        private readonly Exception? _toThrow;

        public StubFetcher(string html = "") => _html = html;
        public StubFetcher(Exception toThrow) => (_html, _toThrow) = ("", toThrow);

        public int CallCount { get; private set; }
        public string? LastUrl { get; private set; }

        public Task<string> FetchAsync(string url, CancellationToken cancellationToken = default)
        {
            CallCount++;
            LastUrl = url;
            return _toThrow is not null
                ? Task.FromException<string>(_toThrow)
                : Task.FromResult(_html);
        }
    }

    private sealed class StubCache(ScrapeResult? seeded = null) : ISolicitorCache
    {
        public ScrapeResult? LastSet { get; private set; }

        public bool TryGet(string city, out ScrapeResult? result)
        {
            result = seeded;
            return seeded is not null;
        }

        public void Set(string city, ScrapeResult result) => LastSet = result;
    }

    private sealed class StubParser(IReadOnlyList<Solicitor> solicitors) : IHtmlParser<Solicitor>
    {
        public IReadOnlyList<Solicitor> Parse(string html) => solicitors;
    }
}