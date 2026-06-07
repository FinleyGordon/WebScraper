# WebScraper — Solicitor Directory

A React + ASP.NET Core app that scrapes and displays solicitor listings by city.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18+)

---

## Running in Development

In development you run the backend and frontend separately so that Vite's hot-reload works.

### 1. Backend (ASP.NET Core)

```bash
cd WebScraper
dotnet run
```

The API will be available at `http://localhost:5270`. The single endpoint is:

```
GET /api/solicitors?city=<city-name>
```

### 2. Frontend (React + Vite)

In a separate terminal:

```bash
cd WebScraper/ClientApp
npm install   # first time only
npm run dev
```

The app will be available at `http://localhost:5173`. API requests are proxied automatically to the backend on port 5270.

---

## Testing

Unit tests live in the `WebScraper.Tests` project (xUnit). Run them from the repository root:

```bash
dotnet test
```

The suite covers:

- **HTML parsing** — `SolicitorHtmlParser` is run end-to-end against an HTML fixture through the real field extractors, verifying field extraction, de-duplication, and that blocks without a name are skipped.
- **Field extractors** — name decoding, phone sanitising, review star parsing, and website matching.
- **Scrape pipeline** — `SolicitorScraperService` with stubbed dependencies, covering cache hits/misses, city normalisation, and upstream failures surfacing as a `ScrapeException`.

---

## Running in Production

A production build bundles the React app into `wwwroot/` and serves it from the .NET host — no separate frontend process needed.

```bash
cd WebScraper
dotnet publish -c Release
dotnet bin/Release/net10.0/WebScraper.dll
```

`npm install` and `npm run build` run automatically as part of the Release build.

The app is then served at `http://localhost:5270`.