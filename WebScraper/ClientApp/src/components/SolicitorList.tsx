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
    <ul className="solicitor-list">
      {filtered.map((s, i) => (
        <li key={i} className="solicitor-card">
          <p className="name">{s.name}</p>
          {s.address && <p className="detail">{s.address}</p>}
          {s.phone && <p className="detail">{s.phone}</p>}
          {s.website && (
            <p className="detail website">
              <a href={s.website} target="_blank" rel="noreferrer">{s.website}</a>
            </p>
          )}
          {s.reviews && (
            <p className="rating">
              {s.reviews.starRating.toFixed(1)} stars ({s.reviews.reviewCount} reviews)
            </p>
          )}
          {s.qualityMarks.length > 0 && (
            <p className="quality-marks">{s.qualityMarks.join(' · ')}</p>
          )}
        </li>
      ))}
    </ul>
  )
}