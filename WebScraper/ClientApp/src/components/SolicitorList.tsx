import {useEffect, useMemo} from 'react'
import type {Solicitor} from '../types'

interface Props {
  solicitors: Solicitor[]
  ratingFilter: number | undefined
  onFilteredCountChange?: (count: number) => void
}

export default function SolicitorList({solicitors, ratingFilter, onFilteredCountChange}: Props) {
  const filtered = useMemo(
    () =>
      solicitors.filter(s => {
        if (ratingFilter == undefined || ratingFilter == 0) return true
        if (s.reviews?.starRating == undefined) return false
        return s.reviews.starRating >= ratingFilter
      }),
    [solicitors, ratingFilter]
  )

  useEffect(() => {
    onFilteredCountChange?.(filtered.length)
  }, [filtered.length])

  return (
    <ul style={{listStyle: 'none', padding: 0, margin: 0}}>
      {filtered.map((s, i) => (
        <li
          key={i}
          style={{
            background: '#fff',
            border: '1px solid #ddd',
            borderRadius: 6,
            padding: '1rem 1.25rem',
            marginBottom: '0.75rem',
          }}
        >
          <strong style={{fontSize: '1.05rem'}}>{s.name}</strong>

          <p style={{margin: '0.35rem 0 0', color: '#444'}}>{s.address}</p>
          <p style={{margin: '0.2rem 0 0', color: '#444'}}>{s.phone}</p>

          {s.website && (
            <p style={{margin: '0.2rem 0 0'}}>
              <a href={s.website} target="_blank" rel="noreferrer">
                {s.website}
              </a>
            </p>
          )}

          {s.reviews && (
            <p style={{margin: '0.35rem 0 0', color: '#666', fontSize: '0.9rem'}}>
              {s.reviews.starRating.toFixed(1)} stars ({s.reviews.reviewCount} reviews)
            </p>
          )}

          {s.qualityMarks.length > 0 && (
            <p style={{margin: '0.35rem 0 0', fontSize: '0.85rem', color: '#2a7'}}>
              {s.qualityMarks.join(' · ')}
            </p>
          )}
        </li>
      ))}
    </ul>
  )
}
