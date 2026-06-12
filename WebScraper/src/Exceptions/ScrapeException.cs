namespace WebScraper.Exceptions;

public sealed class ScrapeException(string message, Exception innerException)
    : Exception(message, innerException);