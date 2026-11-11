import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { ArrowLeft, CheckCircle2, Clock3, MapPin, Truck } from "lucide-react";
import { useState } from "react";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { toast } from "sonner";
import { Footer } from "@/components/layout/Footer";
import { HeaderNav } from "@/components/layout/header/HeaderNav";
import { addressService } from "@/services/address.service";
import { cartQueryKey, cartService } from "@/services/cart.service";
import {
  orderService,
  type CreatedOrder,
  type PaymentMethod,
  type ShippingService,
} from "@/services/order.service";
import { productService } from "@/services/product.service";
import { getImageUrl } from "@/utils/image";

function formatVnd(value: number) {
  return value.toLocaleString("vi-VN") + "đ";
}

export function CheckoutPage() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const productId = searchParams.get("productId");
  const quantity = Math.max(1, Math.floor(Number(searchParams.get("quantity")) || 1));
  const requestedCartItemIds = searchParams.get("cartItemIds")?.split(",").filter(Boolean);
  const [addressId, setAddressId] = useState("");
  const [shippingService, setShippingService] = useState<ShippingService>("Standard");
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>(0);
  const [note, setNote] = useState("");
  const [placedOrder, setPlacedOrder] = useState<CreatedOrder | null>(null);


  const cartQuery = useQuery({
    queryKey: cartQueryKey,
    queryFn: cartService.getCart,
    enabled: !productId,
  });
  const productQuery = useQuery({
    queryKey: ["product-detail", productId],
    queryFn: () => productService.getDetailById(productId!),
    enabled: Boolean(productId),
  });
  const addressQuery = useQuery({
    queryKey: ["my-addresses"],
    queryFn: addressService.getMine,
  });

  const allCartItems = cartQuery.data?.data?.data ?? [];
  const selectedItems = requestedCartItemIds
    ? allCartItems.filter((item) => requestedCartItemIds.includes(item.id))
    : allCartItems;
  const product = productQuery.data?.data;
  const addresses = addressQuery.data?.data ?? [];
  const selectedAddress = addresses.find((address) => address.id === addressId)
    ?? addresses.find((address) => address.isDefault)
    ?? addresses[0];
  const restaurantIds = new Set(selectedItems.map((item) => item.restaurantId));
  const hasMixedRestaurants = restaurantIds.size > 1;
  const restaurantId = product?.restaurantId
    ?? (restaurantIds.size === 1 ? selectedItems[0]?.restaurantId : undefined);
  const subtotal = product
    ? product.price * quantity
    : selectedItems.reduce((sum, item) => sum + item.price * item.quantity, 0);

  const shippingFeeQuery = useQuery({
    queryKey: ["shipping-fee", selectedAddress?.id, restaurantId, subtotal, shippingService],
    queryFn: () => {
      if (!selectedAddress || !restaurantId) {
        throw new Error("Vui lòng chọn địa chỉ và sản phẩm từ cùng một nhà hàng.");
      }
      return orderService.calculateShippingFee(
        selectedAddress.id,
        restaurantId,
        subtotal,
        shippingService,
      );
    },
    enabled: Boolean(selectedAddress && restaurantId && subtotal > 0 && !hasMixedRestaurants),
  });
  const shippingEstimate = shippingFeeQuery.data?.isSuccess
    ? shippingFeeQuery.data.data
    : null;
  const shippingFeeError = shippingFeeQuery.data && !shippingFeeQuery.data.isSuccess
    ? shippingFeeQuery.data.message
    : null;
  const total = subtotal + (shippingEstimate?.shippingFee ?? 0);

  const placeOrderMutation = useMutation({
    mutationFn: () => {
      if (!selectedAddress || !shippingEstimate) {
        throw new Error("Vui lòng chọn địa chỉ và chờ tính phí giao hàng.");
      }
      if (productId) {
        return orderService.buyNow({
          productId,
          quantity,
          addressId: selectedAddress.id,
          shippingService,
          paymentMethod,
          note: note.trim() || undefined,
        });
      }
      return orderService.checkout({
        addressId: selectedAddress.id,
        cartItemIds: selectedItems.map((item) => item.id),
        shippingService,
        paymentMethod,
        note: note.trim() || undefined,
      });
    },
    onSuccess: async (result) => {
      if (!result.isSuccess || !result.data) {
        toast.error(result.message ?? "Không thể tạo đơn hàng. Vui lòng thử lại.");
        return;
      }
      // 1. Luôn xóa cache giỏ hàng và cập nhật lịch sử đơn
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: cartQueryKey }),
        queryClient.invalidateQueries({ queryKey: ["my-order-history"] }),
      ]);
      // 2. NẾU LÀ VNPAY: Có paymentUrl thì chuyển hướng ngay sang cổng thanh toán
      if (result.data.paymentUrl) {
        toast.success("Đang chuyển hướng tới cổng thanh toán VNPAY...");
        window.location.href = result.data.paymentUrl;
        return;
      }
      // 3. Đơn hàng COD thông thường: hiển thị màn hình đặt hàng thành công
      setPlacedOrder(result.data);
      toast.success(`Đặt hàng thành công: ${result.data.orderCode}`);
    },
    onError: (error: Error) => toast.error(error.message || "Không thể tạo đơn hàng. Vui lòng thử lại."),
  });

  const isLoading = productId ? productQuery.isPending : cartQuery.isPending;
  const hasItems = productId ? Boolean(product) : selectedItems.length > 0;
  const dataFailed = productId
    ? productQuery.isError || (productQuery.data !== undefined && !productQuery.data.isSuccess)
    : cartQuery.isError || (cartQuery.data !== undefined && !cartQuery.data.isSuccess);
  const canPlaceOrder = Boolean(
    hasItems
    && selectedAddress
    && !hasMixedRestaurants
    && shippingEstimate
    && !shippingFeeQuery.isPending
    && !placeOrderMutation.isPending
    && (productId ? product?.isAvailable : true),
  );

  return (
    <div className="min-h-screen bg-surface text-slate-900 flex flex-col">
      <HeaderNav />
      <main className="w-full max-w-5xl mx-auto px-5 lg:px-8 py-10 flex-1">
        <Link to={productId ? `/product/${productId}` : "/cart"} className="inline-flex items-center gap-2 text-sm text-slate-500 hover:text-[#fb6900]">
          <ArrowLeft size={16} /> Quay lại
        </Link>
        <h1 className="mt-5 text-3xl font-display font-bold">Xác nhận đơn hàng</h1>
        <p className="mt-2 mb-8 text-slate-500">Kiểm tra lại món và chọn cách giao hàng trước khi đặt.</p>

        {placedOrder ? (
          <section role="status" className="border border-emerald-200 bg-emerald-50 p-6 text-emerald-900">
            <CheckCircle2 size={32} className="text-emerald-600" />
            <h2 className="mt-3 text-xl font-semibold">Đặt hàng thành công</h2>
            <p className="mt-2">Mã đơn: {placedOrder.orderCode}</p>
            <p className="mt-1">Tiền món: {formatVnd(placedOrder.subtotal)} · Phí giao hàng: {formatVnd(placedOrder.shippingFee)}</p>
            <p className="mt-1 font-semibold">Tổng thanh toán: {formatVnd(placedOrder.totalAmount)}</p>
            <p className="mt-1">Dự kiến giao: {new Date(placedOrder.estimatedDeliveryTime).toLocaleString("vi-VN")}</p>
            <button
              onClick={() => navigate("/account?tab=orders")}
              className="mt-5 rounded-lg bg-[#fb6900] px-5 py-2.5 font-semibold text-white hover:bg-[#e35f00]"
            >
              Xem đơn hàng
            </button>
          </section>
        ) : isLoading || addressQuery.isPending ? (
          <p className="py-16 text-center text-slate-500">Đang tải thông tin đơn hàng...</p>
        ) : dataFailed ? (
          <p role="alert" className="border-y border-red-200 py-5 text-red-600">
            {productQuery.data?.message ?? cartQuery.data?.message ?? "Không thể tải thông tin đơn hàng. Vui lòng thử lại."}
          </p>
        ) : !hasItems ? (
          <div className="border-y border-slate-200 py-12 text-center">
            <p className="text-slate-600">Không tìm thấy sản phẩm đã chọn để thanh toán.</p>
            <Link to="/cart" className="mt-4 inline-flex font-semibold text-[#fb6900] hover:underline">Quay lại giỏ hàng</Link>
          </div>
        ) : (
          <div className="grid gap-8 lg:grid-cols-[1fr_340px]">
            <div className="space-y-7">
              <section>
                <h2 className="flex items-center gap-2 text-lg font-semibold"><MapPin size={19} /> Địa chỉ giao hàng</h2>
                {addressQuery.isError || (addressQuery.data && !addressQuery.data.isSuccess) ? (
                  <p role="alert" className="mt-3 text-sm text-red-600">
                    {addressQuery.data?.message ?? "Không thể tải địa chỉ giao hàng. Vui lòng thử lại."}
                  </p>
                ) : addresses.length === 0 ? (
                  <div className="mt-3 border border-amber-200 bg-amber-50 p-4 text-sm text-slate-700">
                    <p>Bạn chưa có địa chỉ giao hàng.</p>
                    <Link to="/account?tab=addresses" className="mt-2 inline-flex font-semibold text-[#fb6900] hover:underline">Thêm địa chỉ</Link>
                  </div>
                ) : (
                  <fieldset className="mt-3 space-y-2">
                    <legend className="sr-only">Chọn địa chỉ giao hàng</legend>
                    {addresses.map((address) => (
                      <label key={address.id} className={`block cursor-pointer border p-4 transition-colors ${selectedAddress?.id === address.id ? "border-[#fb6900] bg-[#fb6900]/5" : "border-slate-200 hover:border-slate-300"
                        }`}>
                        <span className="flex items-start gap-3">
                          <input
                            type="radio"
                            name="deliveryAddress"
                            checked={selectedAddress?.id === address.id}
                            onChange={() => setAddressId(address.id)}
                            className="mt-1 accent-[#fb6900]"
                          />
                          <span className="min-w-0 text-sm">
                            <span className="font-semibold">{address.label || "Địa chỉ"}{address.isDefault ? " · Mặc định" : ""}</span>
                            <span className="mt-1 block text-slate-600">{address.fullAddress}</span>
                            <span className="mt-1 block text-slate-500">{address.receiverName} · {address.phoneNumber}</span>
                          </span>
                        </span>
                      </label>
                    ))}
                    <Link to="/account?tab=addresses" className="inline-flex text-sm font-medium text-[#fb6900] hover:underline">Quản lý địa chỉ</Link>
                  </fieldset>
                )}
              </section>

              <section>
                <h2 className="flex items-center gap-2 text-lg font-semibold"><Truck size={19} /> Kiểu giao hàng</h2>
                <fieldset className="mt-3 grid gap-3 sm:grid-cols-2">
                  <legend className="sr-only">Chọn kiểu giao hàng</legend>
                  {([
                    { value: "Standard", label: "Giao tiêu chuẩn", description: "Phí giao hàng tiết kiệm" },
                    { value: "Express", label: "Giao nhanh", description: "Ưu tiên giao hàng" },
                  ] as const).map((option) => (
                    <label key={option.value} className={`cursor-pointer border p-4 transition-colors ${shippingService === option.value ? "border-[#fb6900] bg-[#fb6900]/5" : "border-slate-200 hover:border-slate-300"
                      }`}>
                      <span className="flex items-start gap-3">
                        <input
                          type="radio"
                          name="shippingService"
                          value={option.value}
                          checked={shippingService === option.value}
                          onChange={() => setShippingService(option.value)}
                          className="mt-1 accent-[#fb6900]"
                        />
                        <span>
                          <span className="block font-semibold">{option.label}</span>
                          <span className="mt-1 block text-sm text-slate-500">{option.description}</span>
                          {shippingService === option.value && shippingEstimate && (
                            <span className="mt-2 block text-sm text-slate-600">
                              {formatVnd(shippingEstimate.shippingFee)} · khoảng {shippingEstimate.estimatedMinutes} phút
                            </span>
                          )}
                        </span>
                      </span>
                    </label>
                  ))}
                </fieldset>
                {shippingFeeQuery.isPending && <p className="mt-3 text-sm text-slate-500">Đang tính phí giao hàng...</p>}
                {(shippingFeeQuery.isError || shippingFeeError) && (
                  <p role="alert" className="mt-3 text-sm text-red-600">
                    {shippingFeeError || "Không thể tính phí giao hàng. Vui lòng thử lại."}
                  </p>
                )}
                {shippingEstimate && (
                  <p className="mt-2 flex items-center gap-1.5 text-sm text-slate-500">
                    <Clock3 size={15} /> Khoảng {shippingEstimate.estimatedMinutes} phút · {shippingEstimate.distance.toFixed(1)} km
                  </p>
                )}
              </section>

              <section>
                <h2 className="flex items-center gap-2 text-lg font-semibold"><Truck size={19} /> Phương thức thanh toán</h2>
                <div className="space-y-3">
                  {/* Lựa chọn 1: Tiền mặt */}
                  <label
                    onClick={() => setPaymentMethod(0)}
                    className={`flex cursor-pointer items-start gap-3 rounded-2xl border p-4 transition-all ${paymentMethod === 0 ? "border-[#fb6900] bg-orange-50/40" : "border-slate-200 hover:bg-slate-50"
                      }`}
                  >
                    <input type="radio" name="paymentMethod" checked={paymentMethod === 0} onChange={() => setPaymentMethod(0)} className="mt-1 accent-[#fb6900]" />
                    <span>
                      <span className="block font-semibold">Thanh toán khi nhận hàng (COD)</span>
                      <span className="mt-1 block text-sm text-slate-500">Thanh toán bằng tiền mặt khi shipper giao món.</span>
                    </span>
                  </label>

                  {/* Lựa chọn 2: VNPAY */}
                  <label
                    onClick={() => setPaymentMethod(3)}
                    className={`flex cursor-pointer items-start gap-3 rounded-2xl border p-4 transition-all ${paymentMethod === 3 ? "border-[#fb6900] bg-orange-50/40" : "border-slate-200 hover:bg-slate-50"
                      }`}
                  >
                    <input type="radio" name="paymentMethod" checked={paymentMethod === 3} onChange={() => setPaymentMethod(3)} className="mt-1 accent-[#fb6900]" />
                    <span>
                      <span className="block font-semibold flex items-center gap-2">
                        Thanh toán qua VNPAY
                        <span className="rounded bg-blue-100 px-2 py-0.5 text-xs font-bold text-blue-700">QR / Thẻ ATM</span>
                      </span>
                      <span className="mt-1 block text-sm text-slate-500">Quét mã VNPAY-QR hoặc thẻ ATM / Internet Banking nội địa.</span>
                    </span>
                  </label>
                </div>
              </section>

              <section>
                <label htmlFor="checkout-note" className="block text-lg font-semibold">Ghi chú cho nhà hàng</label>
                <textarea
                  id="checkout-note"
                  value={note}
                  onChange={(event) => setNote(event.target.value)}
                  rows={3}
                  maxLength={500}
                  placeholder="Ví dụ: ít cay, gọi trước khi giao..."
                  className="mt-3 w-full resize-y border border-slate-300 bg-white px-3 py-2 text-sm outline-none focus:border-[#fb6900]"
                />
              </section>
            </div>

            <aside className="self-start border border-slate-200 bg-white p-5">
              <h2 className="text-lg font-semibold">Đơn hàng của bạn</h2>
              <div className="mt-4 max-h-64 space-y-4 overflow-y-auto">
                {product ? (
                  <div className="flex items-center gap-3">
                    <img src={getImageUrl(product.images.find((image) => image.isMain)?.imageUrl ?? product.images[0]?.imageUrl)} alt={product.name} className="h-14 w-14 rounded-lg object-cover" />
                    <div className="min-w-0 flex-1">
                      <p className="truncate text-sm font-medium">{product.name}</p>
                      <p className="mt-1 text-xs text-slate-500">Số lượng: {quantity}</p>
                    </div>
                    <span className="text-sm font-semibold">{formatVnd(product.price * quantity)}</span>
                  </div>
                ) : selectedItems.map((item) => (
                  <div key={item.id} className="flex items-center gap-3">
                    <img src={getImageUrl(item.imageUrl)} alt={item.productName} className="h-14 w-14 rounded-lg object-cover" />
                    <div className="min-w-0 flex-1">
                      <p className="truncate text-sm font-medium">{item.productName}</p>
                      <p className="mt-1 text-xs text-slate-500">Số lượng: {item.quantity}</p>
                    </div>
                    <span className="text-sm font-semibold">{formatVnd(item.price * item.quantity)}</span>
                  </div>
                ))}
              </div>
              {hasMixedRestaurants && <p role="alert" className="mt-4 text-sm text-red-600">Vui lòng chọn món từ cùng một nhà hàng.</p>}
              {!product?.isAvailable && productId && <p role="alert" className="mt-4 text-sm text-red-600">Món này hiện không khả dụng.</p>}
              <div className="mt-5 space-y-3 border-t border-slate-200 pt-4 text-sm">
                <div className="flex justify-between gap-3 text-slate-600"><span>Tạm tính</span><span>{formatVnd(subtotal)}</span></div>
                <div className="flex justify-between gap-3 text-slate-600">
                  <span>Phí giao hàng</span>
                  <span>{shippingEstimate ? formatVnd(shippingEstimate.shippingFee) : "—"}</span>
                </div>
                <div className="flex justify-between gap-3 border-t border-slate-200 pt-4 text-base font-bold">
                  <span>Tổng cộng</span><span className="text-[#fb6900]">{formatVnd(total)}</span>
                </div>
              </div>
              <button
                disabled={!canPlaceOrder}
                onClick={() => placeOrderMutation.mutate()}
                className="mt-5 h-11 w-full rounded-lg bg-[#fb6900] font-semibold text-white transition-colors hover:bg-[#e35f00] disabled:cursor-not-allowed disabled:bg-slate-200 disabled:text-slate-500"
              >
                {placeOrderMutation.isPending ? "Đang đặt hàng..." : "Đặt hàng"}
              </button>
            </aside>
          </div>
        )}
      </main>
      <Footer />
    </div>
  );
}
