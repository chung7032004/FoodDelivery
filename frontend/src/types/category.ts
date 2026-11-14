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

export interface CategoryResponse {
  id: string;
  name: string;
  description: string;
  displayOrder: number;
  imageUrl: string | null;
  createdAt: string;
  restaurantId: string | null;
}

export interface CategoryForm {
  name: string;
  description: string;
  displayOrder: number;
  imageFile?: File | null;
  imagePreview?: string | null;
  image?: File | null;
  currentImageUrl?: string | null;
}
