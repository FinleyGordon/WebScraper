namespace WebScraper.Exceptions;

public sealed class CityNotSupportedException(string city)
    : Exception($"Solicitor listings are not available for '{city}'.")
{
    public string City { get; } = city;
}