export const CITIES = [
  'London',
  'Birmingham',
  'Leeds',
  'Manchester',
  'Sheffield',
  'Bradford',
  'Liverpool',
  'Bristol',
] as const

export type City = (typeof CITIES)[number]