export interface CategoryQueryParams {
  searchQuery?: string;
  page?: number;
  pageSize?: number;
}

export interface Category {
  id: string;
  name: string;
  description: string;
  displayOrder: number;
  imageUrl: string | null;
}