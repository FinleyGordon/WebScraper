namespace WebScraper.Parsers;

public interface IHtmlParser<T>
{
    IReadOnlyList<T> Parse(string html);
}
