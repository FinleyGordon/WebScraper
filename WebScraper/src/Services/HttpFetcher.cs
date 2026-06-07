namespace WebScraper.Services;

public sealed class HttpFetcher : IHttpFetcher
{
    private readonly HttpClient _client;

    public HttpFetcher(HttpClient client)
    {
        _client = client;
        _client.DefaultRequestHeaders.TryAddWithoutValidation(
            "User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/124.0 Safari/537.36");
        _client.DefaultRequestHeaders.TryAddWithoutValidation(
            "Accept", "text/html,application/xhtml+xml");
    }

    public async Task<string> FetchAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
