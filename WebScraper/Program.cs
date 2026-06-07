using WebScraper.Configuration;
using WebScraper.Extractors;
using WebScraper.Models;
using WebScraper.Parsers;
using WebScraper.Services;

var builder = WebApplication.CreateBuilder(args);

var scraperConfig = builder.Configuration
    .GetSection(ScraperConfiguration.SectionName)
    .Get<ScraperConfiguration>()!;

builder.Services.AddSingleton(scraperConfig);
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ISolicitorCache, MemorySolicitorCache>();
builder.Services.AddHttpClient<IHttpFetcher, HttpFetcher>();
builder.Services.AddTransient<IFieldExtractor, NameExtractor>();
builder.Services.AddTransient<IFieldExtractor, AddressExtractor>();
builder.Services.AddTransient<IFieldExtractor, PhoneExtractor>();
builder.Services.AddTransient<IFieldExtractor, WebsiteExtractor>();
builder.Services.AddTransient<IFieldExtractor, DescriptionExtractor>();
builder.Services.AddTransient<IFieldExtractor, ReviewExtractor>();
builder.Services.AddTransient<IFieldExtractor, QualityMarksExtractor>();
builder.Services.AddTransient<IHtmlParser<Solicitor>, SolicitorHtmlParser>();
builder.Services.AddTransient<SolicitorScraperService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/solicitors", async (SolicitorScraperService solicitorScraper, string? city, CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(city))
        return Results.BadRequest(new { error = "Query parameter 'city' is required." });

    try
    {
        var result = await solicitorScraper.RetrieveSolicitors(city, ct);
        return Results.Ok(result);
    }
    catch (ScrapeException ex)
    {
        return Results.Problem(
            title: "Upstream directory unavailable",
            detail: ex.Message,
            statusCode: StatusCodes.Status502BadGateway);
    }
});

app.MapFallbackToFile("index.html");

app.Run();
