namespace WebScraper.Services;

public interface IHttpFetcher
{
    Task<string> FetchAsync(string url, CancellationToken cancellationToken = default);
}
