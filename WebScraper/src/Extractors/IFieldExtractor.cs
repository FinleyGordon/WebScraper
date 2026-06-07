namespace WebScraper.Extractors;

public interface IFieldExtractor
{
    string FieldName { get; }
    string? Extract(string htmlBlock);
}
