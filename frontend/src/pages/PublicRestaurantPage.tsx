import { useQuery } from "@tanstack/react-query";
import { ArrowLeft, MapPin, Phone, Store, Utensils } from "lucide-react";
import { Link, useParams } from "react-router-dom";
import { Footer } from "@/components/layout/Footer";
import { HeaderNav } from "@/components/layout/header/HeaderNav";
import { productService } from "@/services/product.service";
import { restaurantService } from "@/services/restaurant.service";
import { getImageUrl } from "@/utils/image";
import { useState } from "react";
import { addressService } from "@/services/address.service";
import { useAuth } from "@/contexts/AuthContext";
import { RestaurantLocationModal } from "@/components/modals/RestaurantLocationModal";
import { orderService } from "@/services/order.service";
import Button from "@/components/ui/Button";

function formatVnd(value: number) {
  return value.toLocaleString("vi-VN") + "đ";
}

export function PublicRestaurantPage() {
  const { restaurantId } = useParams<{ restaurantId: string }>();
  const restaurantQuery = useQuery({
    queryKey: ["public-restaurant", restaurantId],
    queryFn: () => restaurantService.getPublicProfile(restaurantId!),
    enabled: Boolean(restaurantId),
  });
  const productsQuery = useQuery({
    queryKey: ["restaurant-products", restaurantId],
    queryFn: () => productService.getAll({ restaurantId, page: 1, pageSize: 100 }),
    enabled: Boolean(restaurantId),
  });
  
  
  

  const [showRestaurantLocation, setShowRestaurantLocation] = useState(false);
  const { isAuthenticated } = useAuth();
  const restaurant = restaurantQuery.data?.data;
  const products = productsQuery.data?.data?.data ?? [];

  const customerAddressQuery = useQuery({
    queryKey: ["my-addresses"],
    queryFn: addressService.getMine,
    enabled: showRestaurantLocation && isAuthenticated,
  });
  const customerAddresses = customerAddressQuery.data?.data ?? [];
  const defaultAddress = customerAddresses.find((address) => address.isDefault) ?? customerAddresses[0];
  const shippingDistanceQuery = useQuery({
      queryKey: ["restaurant-distance", restaurantId, defaultAddress?.id],
      queryFn: () => {
        if (!defaultAddress || !restaurantId) {
          throw new Error("Không tìm thấy địa chỉ mặc định hoặc nhà hàng.");
        }
        return orderService.calculateShippingFee(defaultAddress.id, restaurantId, 0);
      },
      enabled: showRestaurantLocation && Boolean(defaultAddress && restaurantId),
    });
  return (
    <div className="flex min-h-screen flex-col bg-surface text-slate-900">
      <HeaderNav />
      <main className="mx-auto w-full max-w-6xl flex-1 px-5 py-8 lg:px-8">
        <Link to="/products" className="inline-flex items-center gap-2 text-sm text-slate-500 hover:text-orange-600">
          <ArrowLeft size={16} /> Quay lại danh sách món
        </Link>

        {restaurantQuery.isPending ? (
          <p className="py-20 text-center text-slate-500">Đang tải thông tin nhà hàng...</p>
        ) : restaurantQuery.isError || !restaurantQuery.data?.isSuccess || !restaurant ? (
          <div role="alert" className="my-10 border-y border-red-200 py-6 text-center text-red-700">
            {restaurantQuery.data?.message ?? "Không thể tải thông tin nhà hàng."}
          </div>
        ) : (
          <>
            <section className="mt-6 rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
              <div className="flex flex-col gap-5 sm:flex-row sm:items-start sm:justify-between">
                <div>
                  <div className="flex items-center gap-3">
                    <span className="flex h-12 w-12 items-center justify-center rounded-xl bg-orange-100 text-orange-700">
                      <Store size={24} />
                    </span>
                    <div>
                      <h1 className="font-display text-2xl font-bold">{restaurant.name}</h1>
                      <p className={`mt-1 text-sm font-medium ${restaurant.isOpen ? "text-emerald-700" : "text-slate-500"}`}>
                        {restaurant.isOpen ? "Đang mở cửa" : restaurant.closingMessage || "Đang đóng cửa"}
                      </p>
                    </div>
                  </div>
                </div>
                <Button variant="outline" size="sm" onClick={() => setShowRestaurantLocation(true)} className="inline-flex items-center gap-2">
                  <MapPin size={16} /> Xem địa chỉ
                </Button>
              </div>

              <div className="mt-6 grid gap-3 border-t border-slate-100 pt-5 text-sm text-slate-600 sm:grid-cols-2">
                <p className="flex items-start gap-2">
                  <MapPin size={17} className="mt-0.5 shrink-0 text-orange-600" />
                  {restaurant.address}
                </p>
                <p className="flex items-center gap-2">
                  <Phone size={16} className="shrink-0 text-orange-600" />
                  {restaurant.phone}
                </p>
                <p className="text-slate-500">
                  Giờ hoạt động: {restaurant.openTime.slice(0, 5)} – {restaurant.closeTime.slice(0, 5)}
                </p>
              </div>
            </section>

            <section className="mt-10">
              <div className="mb-5 flex items-end justify-between gap-4">
                <div>
                  <p className="text-xs font-semibold uppercase tracking-wider text-orange-600">Thực đơn</p>
                  <h2 className="mt-1 font-display text-2xl font-bold">Món ăn của nhà hàng</h2>
                </div>
                {!productsQuery.isPending && <span className="text-sm text-slate-500">{productsQuery.data?.data?.meta.totalCount ?? products.length} món</span>}
              </div>

              {productsQuery.isPending ? (
                <p className="py-12 text-center text-slate-500">Đang tải thực đơn...</p>
              ) : productsQuery.isError || !productsQuery.data?.isSuccess ? (
                <p role="alert" className="py-8 text-center text-red-600">Không thể tải thực đơn nhà hàng.</p>
              ) : products.length === 0 ? (
                <div className="border-y border-slate-200 py-12 text-center text-slate-500">
                  <Utensils size={30} className="mx-auto text-slate-300" />
                  <p className="mt-3">Nhà hàng chưa có món ăn.</p>
                </div>
              ) : (
                <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4">
                  {products.map((product) => (
                    <Link key={product.id} to={`/product/${product.id}`} className="group overflow-hidden rounded-xl border border-slate-200 bg-white transition hover:-translate-y-0.5 hover:shadow-md">
                      <div className="aspect-square bg-slate-100">
                        {product.mainImageUrl ? (
                          <img src={getImageUrl(product.mainImageUrl)} alt={product.name} className="h-full w-full object-cover transition group-hover:scale-105" />
                        ) : (
                          <div className="flex h-full items-center justify-center text-slate-300"><Utensils size={32} /></div>
                        )}
                      </div>
                      <div className="p-3">
                        <h3 className="line-clamp-2 min-h-10 text-sm font-semibold text-slate-800 group-hover:text-orange-600">{product.name}</h3>
                        <p className="mt-2 font-semibold text-orange-600">{formatVnd(product.price)}</p>
                        {!product.isAvailable && <p className="mt-1 text-xs text-slate-500">Tạm hết món</p>}
                      </div>
                    </Link>
                  ))}
                </div>
              )}
            </section>
          </>
        )}
      </main>
      <Footer />
      <RestaurantLocationModal
        isOpen={showRestaurantLocation}
        onClose={() => setShowRestaurantLocation(false)}
        restaurant={restaurantQuery.data?.data ?? null}
        defaultAddress={defaultAddress ?? null}
        isAuthenticated={isAuthenticated}
        isLoadingRestaurant={restaurantQuery.isPending}
        isLoadingAddress={customerAddressQuery.isPending}
        restaurantError={restaurantQuery.error ? "Không thể tải địa chỉ nhà hàng." : null}
        shippingDistance={shippingDistanceQuery.data?.data ?? null}
        shippingLoading={shippingDistanceQuery.isPending}
        distanceError={shippingDistanceQuery.data && !shippingDistanceQuery.data.isSuccess ? shippingDistanceQuery.data.message : null}
      />
    </div>
  );
  
}
