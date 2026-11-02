// src/components/RestaurantLocationModal.tsx
import { useEffect } from "react";
import { X, MapPin } from "lucide-react";
import { Link } from "react-router-dom";
import { MapContainer, TileLayer, CircleMarker, Tooltip, useMap } from "react-leaflet";
import "leaflet/dist/leaflet.css";

interface RestaurantLocationModalProps {
  isOpen: boolean;
  onClose: () => void;
  restaurant: {
    name?: string;
    address?: string;
    latitude?: number;
    longitude?: number;
  } | null;
  defaultAddress: {
    id?: string | number;
    label?: string | null;
    fullAddress?: string | null;
    latitude?: number;
    longitude?: number;
  } | null;
  isAuthenticated: boolean;
  isLoadingRestaurant: boolean;
  isLoadingAddress: boolean;
  restaurantError: string | null;
  shippingDistance: { distance: number } | null;
  shippingLoading: boolean;
  distanceError: string | null;
}

function FitRestaurantAndCustomerLocations({ locations }: { locations: [number, number][] }) {
  const map = useMap();
  useEffect(() => {
    if (locations.length > 1) {
      map.fitBounds(locations, { padding: [32, 32], maxZoom: 13 });
    }
  }, [locations, map]);
  return null;
}

export function RestaurantLocationModal({
  isOpen,
  onClose,
  restaurant,
  defaultAddress,
  isAuthenticated,
  isLoadingRestaurant,
  isLoadingAddress,
  restaurantError,
  shippingDistance,
  shippingLoading,
  distanceError,
}: RestaurantLocationModalProps) {
  if (!isOpen) return null;

  const hasRestaurantCoords = Boolean(
    restaurant && Number.isFinite(restaurant.latitude) && Number.isFinite(restaurant.longitude) && (restaurant.latitude !== 0 || restaurant.longitude !== 0)
  );
  
  const hasCustomerCoords = Boolean(
    defaultAddress && Number.isFinite(defaultAddress.latitude) && Number.isFinite(defaultAddress.longitude) && (defaultAddress.latitude !== 0 || defaultAddress.longitude !== 0)
  );

  const mapLocations: [number, number][] =
    hasRestaurantCoords && hasCustomerCoords
      ? [
          [restaurant!.latitude!, restaurant!.longitude!],
          [defaultAddress!.latitude!, defaultAddress!.longitude!],
        ]
      : [];

  return (
    <div className="fixed inset-0 z-[110] flex items-center justify-center bg-slate-950/60 p-4 backdrop-blur-sm">
      <section role="dialog" aria-modal="true" aria-labelledby="restaurant-location-title" className="max-h-[90vh] w-full max-w-2xl overflow-y-auto rounded-2xl bg-white p-5 shadow-xl">
        <div className="flex items-start justify-between gap-4">
          <div>
            <h2 id="restaurant-location-title" className="text-xl font-bold">Địa chỉ và khoảng cách</h2>
            <p className="mt-1 text-sm text-slate-500">{restaurant?.name}</p>
          </div>
          <button type="button" aria-label="Đóng" onClick={onClose} className="rounded-lg p-2 text-slate-500 hover:bg-slate-100 transition">
            <X size={18} />
          </button>
        </div>

        {isLoadingRestaurant ? (
          <p className="py-8 text-center text-sm text-slate-500">Đang tải địa chỉ nhà hàng...</p>
        ) : restaurantError || !restaurant ? (
          <p role="alert" className="mt-5 text-sm text-red-600">
            {restaurantError || "Không thể tải địa chỉ nhà hàng."}
          </p>
        ) : (
          <>
            <div className="mt-4 space-y-3 rounded-xl bg-slate-50 p-4 text-sm">
              <p className="flex items-start gap-2">
                <MapPin size={17} className="mt-0.5 shrink-0 text-orange-600" />
                <span><strong>Nhà hàng:</strong> {restaurant.address}</span>
              </p>
              {isAuthenticated ? (
                isLoadingAddress ? (
                  <p className="text-slate-500">Đang tải địa chỉ giao hàng của bạn...</p>
                ) : defaultAddress ? (
                  <p className="flex items-start gap-2">
                    <MapPin size={17} className="mt-0.5 shrink-0 text-sky-600" />
                    <span><strong>Địa chỉ của bạn ({defaultAddress.label || "Mặc định"}):</strong> {defaultAddress.fullAddress}</span>
                  </p>
                ) : (
                  <p className="text-slate-600">
                    Bạn chưa lưu địa chỉ giao hàng. <Link to="/account?tab=addresses" className="font-semibold text-orange-600 underline">Thêm địa chỉ</Link>
                  </p>
                )
              ) : (
                <p className="text-slate-600">
                  <Link to="/login" className="font-semibold text-orange-600 underline">Đăng nhập</Link> để xem khoảng cách tới địa chỉ giao hàng mặc định.
                </p>
              )}
            </div>

            {hasRestaurantCoords && hasCustomerCoords && (
              <div className="mt-4 h-72 overflow-hidden rounded-xl border border-slate-200">
                <MapContainer
                  center={mapLocations[0]}
                  zoom={12}
                  scrollWheelZoom
                  style={{ width: "100%", height: "100%" }}
                >
                  <FitRestaurantAndCustomerLocations locations={mapLocations} />
                  <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://tile.openstreetmap.de/{z}/{x}/{y}.png"
                  />
                  <CircleMarker center={[restaurant.latitude!, restaurant.longitude!]} radius={9} pathOptions={{ color: "#fff", weight: 3, fillColor: "#fb6900", fillOpacity: 1 }}>
                    <Tooltip>Nhà hàng</Tooltip>
                  </CircleMarker>
                  <CircleMarker center={[defaultAddress!.latitude!, defaultAddress!.longitude!]} radius={9} pathOptions={{ color: "#fff", weight: 3, fillColor: "#0284c7", fillOpacity: 1 }}>
                    <Tooltip>Địa chỉ của bạn</Tooltip>
                  </CircleMarker>
                </MapContainer>
              </div>
            )}

            {isAuthenticated && defaultAddress && (
              shippingLoading ? (
                <p className="mt-4 text-sm text-slate-500">Đang tính khoảng cách...</p>
              ) : shippingDistance ? (
                <p className="mt-4 rounded-lg bg-orange-50 px-4 py-3 text-sm font-semibold text-orange-800">
                  Khoảng cách giao hàng ước tính: {shippingDistance.distance.toFixed(1)} km
                </p>
              ) : (
                <p role="alert" className="mt-4 text-sm text-red-600">
                  {distanceError || "Không thể tính khoảng cách đến địa chỉ này."}
                </p>
              )
            )}
          </>
        )}
      </section>
    </div>
  );
}