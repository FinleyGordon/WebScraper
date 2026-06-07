export interface ReviewInfo {
  starRating: number
  reviewCount: number
}

export interface Solicitor {
  name: string
  address: string
  phone: string
  website?: string
  profileUrl?: string
  description?: string
  reviews?: ReviewInfo
  qualityMarks: string[]
}

export interface ScrapeResult {
  sourceUrl: string
  scrapedAt: string
  solicitors: Solicitor[]
  totalFound: number
  isSuccess: boolean
}
