export interface ProductResponseDto {
  id: string;
  name: string;
  price: number;
  mainImageUrl: string | null;
  isAvailable: boolean;
  isFeatured: boolean;
  displayOrder: number;
  createdAt: string; // ISO string, parse bằng new Date() khi cần
  categoryId: string;
  categoryName: string;
  restaurantId: string;
}

export interface ProductQueryParams {
  categoryId?: string;
  searchQuery?: string;
  page?: number;
  pageSize?: number;
}