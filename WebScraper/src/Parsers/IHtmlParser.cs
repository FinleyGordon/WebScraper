namespace WebScraper.Parsers;

public interface IHtmlParser<out T>
{
    IReadOnlyList<T> Parse(string html);
}
