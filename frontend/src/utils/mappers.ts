import type { Dish } from '@/types';
import type { ProductResponseDto } from '@/types/product';

export function mapProductToDish(p: ProductResponseDto): Dish {
  return {
    id: p.id,
    name: p.name,
    price: p.price,
    mainImageUrl: p.mainImageUrl,
    restaurant: p.categoryName, // tạm dùng categoryName, chưa có tên quán
    emoji: '🍽️',                // BE chưa trả emoji, tạm hardcode
    rating: 0,
    reviewCount: 0,
  };
}