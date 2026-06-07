import {useState} from 'react'
import {CITIES, type City} from './constants/cities'
import {useScrape} from './hooks/useScrape'
import CitySelector from './components/CitySelector'
import SolicitorList from './components/SolicitorList'
import FilterSelector from './components/FilterSelector.tsx'

export default function App() {
  const [city, setCity] = useState<City>(CITIES[0])
  const {data, loading, error, run} = useScrape()
  const [ratingFilter, setFilter] = useState<number>()
  const [filteredCount, setFilteredCount] = useState<number>(0)

  return (
    <div className="app">
      <header className="app-header">
        <h1>Solicitor <span>Directory</span></h1>
      </header>

      <div className="controls">
        <CitySelector value={city} onChange={setCity} disabled={loading} />
        <FilterSelector value={ratingFilter} onChange={setFilter} />
        <button className="btn" onClick={() => run(city)} disabled={loading}>
          {loading ? 'Scraping…' : 'Run Scrape'}
        </button>
      </div>

      {error && <p className="error-banner">{error}</p>}

      {data && (
        <>
          <p className="result-meta">
            Showing {filteredCount} of {data.totalFound} solicitors found from{' '}
            <a href={data.sourceUrl} target="_blank" rel="noreferrer">
              {data.sourceUrl}
            </a>
          </p>
          <SolicitorList
            solicitors={data.solicitors}
            ratingFilter={ratingFilter}
            onFilteredCountChange={setFilteredCount}
          />
        </>
      )}
    </div>
  )
}