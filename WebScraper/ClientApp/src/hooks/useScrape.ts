import {useState} from 'react'
import type {ScrapeResult} from '../types'
import type {City} from '../constants/cities'

interface ScrapeState {
  data: ScrapeResult | null
  loading: boolean
  error: string | null
}

export function useScrape() {
  const [state, setState] = useState<ScrapeState>({ data: null, loading: false, error: null })

  const run = async (city: City) => {
    setState({ data: null, loading: true, error: null })
    try {
      const res = await fetch(`/api/solicitors?city=${encodeURIComponent(city)}`)
      if (!res.ok) throw new Error(`Server returned ${res.status}`)
      const data: ScrapeResult = await res.json()
      setState({ data, loading: false, error: null })
    } catch (e) {
      setState({ data: null, loading: false, error: e instanceof Error ? e.message : 'Unknown error' })
    }
  }

  return { ...state, run }
}