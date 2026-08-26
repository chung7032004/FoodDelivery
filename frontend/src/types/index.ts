export interface Dish {
  id: string;              // BE trả uuid, không phải number
  name: string;
  price: number;
  mainImageUrl?: string | null;
  restaurant?: string;     // BE chưa có -> optional
  distance?: string;       // BE chưa có -> optional
  rating?: number;
  reviewCount?: number;
  emoji?: string;
  tag?: string;
  tagColor?: string;
  bgGradient?: string;
  isFavorite?: boolean;
}


export interface PromoCard {
  id: number;
  title: string;
  subtitle: string;
  btnLabel: string;
  bgClass: string;
}

export interface HeroStat {
  value: string;
  label: string;
}

export interface CartItem {
  dishId: number;
  quantity: number;
}

export interface ApiResult<T> {
    data: T
    isSuccess: boolean
    errorCode: string | null
    message: string | null
    statusCode: number
}