interface Props {
    value: number | undefined,
    onChange: (ratingFilter: number | undefined) => void,
    disabled?: boolean
}


export default function FilterSelector({ value, onChange, disabled }: Props) {
    return (
        <select
            value={value}
            onChange={e => onChange(Number(e.target.value))}
            disabled={disabled}
            style={{ padding: '0.5rem', fontSize: '1rem', cursor: disabled ? 'not-allowed' : 'pointer' }}
            defaultValue={0}
        >
            <option value={0}>Any</option>
            <option value={5}>5 stars ⭐️</option>
            <option value={4}>4+ stars ⭐️</option>
            <option value={3}>3+ stars ⭐️</option>
            <option value={2}>2+ stars ⭐️</option>
        </select>
        


)
}