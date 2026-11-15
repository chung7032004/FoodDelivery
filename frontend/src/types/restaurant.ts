export type RestaurantApplicationStatus = 0 | 1 | 2;

export interface RestaurantRegistrationRequest {
  name: string;
  address: string;
  phone: string;
  latitude: number;
  longitude: number;
  openTime: string;
  closeTime: string;
}

export interface RestaurantApplication {
  restaurantId: string;
  ownerId: string;
  ownerName: string;
  ownerEmail: string;
  ownerPhone: string;
  name: string;
  address: string;
  phone: string;
  latitude: number;
  longitude: number;
  openTime: string;
  closeTime: string;
  isOpen: boolean;
  applicationStatus: RestaurantApplicationStatus;
  rejectionReason: string | null;
  createdAt: string;
  reviewedAt: string | null;
}

export interface MyRestaurantApplication {
  hasApplication: boolean;
  application: RestaurantApplication | null;
}

export interface RestaurantProfile {
  id: string;
  name: string;
  address: string;
  phone: string;
  isOpen: boolean;
  closingMessage: string | null;
  openTime: string;
  closeTime: string;
  latitude: number;
  longitude: number;
}

export type PublicRestaurantProfile = RestaurantProfile;

export interface RestaurantStatus {
  isOpen: boolean;
  closingMessage: string | null;
  updatedAt: string;
}

