namespace WebScraper.Extractors;

/// <summary>
/// Contract for extracting a single named field from an HTML block.
/// </summary>
public interface IFieldExtractor
{
    string FieldName { get; }
    string? Extract(string htmlBlock);
}
