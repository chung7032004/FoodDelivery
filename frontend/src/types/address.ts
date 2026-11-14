export interface CustomerAddress {
  id: string;
  fullAddress: string | null;
  receiverName: string | null;
  phoneNumber: string | null;
  label: string | null;
  isDefault: boolean;
  latitude: number;
  longitude: number;
}

export interface AddressRequest {
  fullAddress: string;
  label: string;
  receiverName: string;
  phoneNumber: string;
  latitude: number;
  longitude: number;
  isDefault: boolean;
}

export interface SavedAddressLocation {
  id?: string;
  label: string | null;
  latitude: number;
  longitude: number;
  isDefault?: boolean;
  fullAddress?: string | null;
}

export interface MapLocation {
  id?: string | number;
  lat: number;
  lng: number;
  label?: string;
  isDefault?: boolean;
}

export interface PhotonFeature {
  geometry: { coordinates: [number, number] };
  properties: Record<string, string | undefined>;
}

export interface NominatimFeature {
  lat: string;
  lon: string;
  display_name: string;
}
