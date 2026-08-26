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

export interface ProductImageDto {
  id: string;
  imageUrl: string;
  isMain: boolean;
  displayOrder: number;
}

export interface ProductDetailResponseDto {
  id: string;
  name: string;
  description: string;
  price: number;
  isAvailable: boolean;
  isFeatured: boolean;
  displayOrder: number;
  createdAt: string;
  updatedAt: string | null;

  images: ProductImageDto[];

  categoryId: string;
  categoryName: string;

  restaurantId: string;
  restaurantName: string;

  averageRating: number;
  reviewCount: number;
}