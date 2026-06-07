import {CITIES, type City} from '../constants/cities'

interface Props {
  value: City
  onChange: (city: City) => void
  disabled?: boolean
}

export default function CitySelector({value, onChange, disabled}: Props) {
  return (
    <select
      value={value}
      onChange={e => onChange(e.target.value as City)}
      disabled={disabled}
      style={{padding: '0.5rem', fontSize: '1rem', cursor: disabled ? 'not-allowed' : 'pointer'}}
    >
      {CITIES.map(c => (
        <option key={c} value={c}>
          {c}
        </option>
      ))}
    </select>
  )
}
