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
    <div style={{maxWidth: 900, margin: '0 auto', padding: '2rem'}}>
      <h1 style={{marginBottom: '0.5rem'}}>Solicitor Directory</h1>

      <div style={{display: 'flex', gap: '0.75rem', alignItems: 'center', marginBottom: '1.5rem'}}>
        <CitySelector value={city} onChange={setCity} disabled={loading} />
        <FilterSelector value={ratingFilter} onChange={setFilter} />
        <button
          onClick={() => run(city)}
          disabled={loading}
          style={{padding: '0.5rem 1.25rem', cursor: loading ? 'not-allowed' : 'pointer'}}
        >
          {loading ? 'Scraping…' : 'Run Scrape'}
        </button>
      </div>

      {error && (
        <p style={{color: '#c00', background: '#fee', padding: '0.75rem', borderRadius: 4}}>
          {error}
        </p>
      )}

      {data && (
        <>
          <p style={{color: '#555'}}>
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
