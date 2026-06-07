namespace WebScraper.Services;

public sealed class ScrapeException(string message, Exception innerException)
    : Exception(message, innerException);