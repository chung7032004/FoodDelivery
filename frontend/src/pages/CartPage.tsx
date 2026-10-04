import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { toast } from "sonner";
import { ArrowLeft, CheckCircle2, Minus, Plus, ShoppingBag, Trash2 } from "lucide-react";
import { Link } from "react-router-dom";
import { Footer } from "@/components/layout/Footer";
import { HeaderNav } from "@/components/layout/HeaderNav";
import { cartQueryKey, cartService } from "@/services/cart.service";
import { addressService } from "@/services/address.service";
import { orderService } from "@/services/order.service";
import { getImageUrl } from "@/utils/image";

function formatVnd(value: number) {
  return value.toLocaleString("vi-VN") + "đ";
}

export function CartPage() {
  const queryClient = useQueryClient();
  const [selectedAddressId, setSelectedAddressId] = useState("");
  const [note, setNote] = useState("");
  const [placedOrder, setPlacedOrder] = useState<{
    orderCode: string;
    totalAmount: number;
    estimatedDeliveryTime: string;
  } | null>(null);
  const { data: cartResult, isLoading } = useQuery({
    queryKey: cartQueryKey,
    queryFn: cartService.getCart,
  });
  const { data: addressResult, isLoading: isAddressLoading, isError: isAddressError } = useQuery({
    queryKey: ["my-addresses"],
    queryFn: addressService.getMine,
  });
  const refreshCart = () => queryClient.invalidateQueries({ queryKey: cartQueryKey });
  const updateMutation = useMutation({
    mutationFn: ({ productId, quantity }: { productId: string; quantity: number }) =>
      cartService.updateQuantity(productId, quantity),
    onSuccess: refreshCart,
  });
  const removeMutation = useMutation({
    mutationFn: cartService.removeItem,
    onSuccess: refreshCart,
  });
  const checkoutMutation = useMutation({
    mutationFn: (addressId: string) => orderService.checkout({
      addressId,
      cartItemIds: items.map((item) => item.id),
      note: note.trim() || undefined,
    }),
    onSuccess: async (result) => {
      if (!result.isSuccess || !result.data) return;
      setPlacedOrder(result.data);
      toast.success(`Đặt hàng thành công: ${result.data.orderCode}`);
      await queryClient.invalidateQueries({ queryKey: cartQueryKey });
    },
  });

  const items = cartResult?.data?.data ?? [];
  const addresses = addressResult?.data ?? [];
  const selectedAddress = addresses.find((address) => address.id === selectedAddressId)
    ?? addresses.find((address) => address.isDefault)
    ?? addresses[0];
  const hasMixedRestaurants = new Set(items.map((item) => item.restaurantId)).size > 1;
  const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);

  return (
    <div className="min-h-screen bg-surface text-slate-900 flex flex-col">
      <HeaderNav />
      <main className="w-full max-w-5xl mx-auto px-5 lg:px-8 py-10 flex-1">
        <Link to="/products" className="inline-flex items-center gap-2 text-sm text-slate-500 hover:text-[#fb6900]">
          <ArrowLeft size={16} /> Tiếp tục mua sắm
        </Link>
        <h1 className="mt-5 mb-8 text-3xl font-display font-bold">Giỏ hàng của bạn</h1>

        {placedOrder && (
          <div role="status" className="mb-8 flex items-start gap-3 border-y border-emerald-200 bg-emerald-50 px-5 py-4 text-emerald-900">
            <CheckCircle2 size={21} className="mt-0.5 shrink-0 text-emerald-600" />
            <div>
              <p className="font-semibold">Đặt hàng thành công</p>
              <p className="mt-1 text-sm">Mã đơn: {placedOrder.orderCode} · Tổng tiền: {formatVnd(placedOrder.totalAmount)}</p>
              <p className="mt-1 text-sm">Dự kiến giao: {new Date(placedOrder.estimatedDeliveryTime).toLocaleString("vi-VN")}</p>
            </div>
          </div>
        )}

        {isLoading ? (
          <p className="py-16 text-center text-slate-500">Đang tải giỏ hàng...</p>
        ) : items.length === 0 ? (
          <div className="py-16 text-center border-y border-slate-200">
            <ShoppingBag size={40} className="mx-auto text-slate-300" />
            <p className="mt-4 text-slate-600">Giỏ hàng đang trống.</p>
            <Link to="/products" className="inline-flex mt-5 px-5 py-2.5 rounded-lg bg-[#fb6900] text-white font-semibold hover:bg-[#e35f00]">
              Khám phá món ăn
            </Link>
          </div>
        ) : (
          <div className="grid gap-10 lg:grid-cols-[1fr_300px]">
            <div className="divide-y divide-slate-200 border-y border-slate-200">
              {items.map((item) => (
                <article key={item.productId} className="flex gap-4 py-5">
                  <Link to={`/product/${item.productId}`} className="w-24 h-24 shrink-0 overflow-hidden rounded-lg bg-slate-100">
                    <img src={getImageUrl(item.imageUrl)} alt={item.productName} className="w-full h-full object-cover" />
                  </Link>
                  <div className="min-w-0 flex-1">
                    <Link to={`/product/${item.productId}`} className="font-semibold hover:text-[#fb6900]">{item.productName}</Link>
                    <p className="mt-1 text-sm text-slate-500">{item.categoryName}</p>
                    <p className="mt-2 font-semibold text-[#fb6900]">{formatVnd(item.price)}</p>
                    <div className="mt-3 flex items-center justify-between gap-3">
                      <div className="flex items-center border border-slate-300 rounded-md">
                        <button
                          aria-label={`Giảm số lượng ${item.productName}`}
                          disabled={updateMutation.isPending || removeMutation.isPending}
                          onClick={() => item.quantity === 1
                            ? removeMutation.mutate(item.productId)
                            : updateMutation.mutate({ productId: item.productId, quantity: item.quantity - 1 })}
                          className="w-8 h-8 flex items-center justify-center hover:bg-slate-100 disabled:opacity-50"
                        >
                          <Minus size={14} />
                        </button>
                        <span className="w-8 text-center text-sm">{item.quantity}</span>
                        <button
                          aria-label={`Tăng số lượng ${item.productName}`}
                          disabled={updateMutation.isPending || removeMutation.isPending}
                          onClick={() => updateMutation.mutate({ productId: item.productId, quantity: item.quantity + 1 })}
                          className="w-8 h-8 flex items-center justify-center hover:bg-slate-100 disabled:opacity-50"
                        >
                          <Plus size={14} />
                        </button>
                      </div>
                      <button
                        aria-label={`Xóa ${item.productName} khỏi giỏ hàng`}
                        disabled={removeMutation.isPending}
                        onClick={() => removeMutation.mutate(item.productId)}
                        className="p-2 text-slate-400 hover:text-red-600 disabled:opacity-50"
                      >
                        <Trash2 size={17} />
                      </button>
                    </div>
                  </div>
                </article>
              ))}
            </div>

            <aside className="self-start border-t border-slate-200 pt-5 lg:border-t-0 lg:pt-0">
              <h2 className="font-semibold text-lg">Địa chỉ giao hàng</h2>
              {isAddressLoading ? (
                <p className="mt-3 text-sm text-slate-500">Đang tải địa chỉ...</p>
              ) : isAddressError ? (
                <p className="mt-3 text-sm text-red-600">Không thể tải địa chỉ giao hàng. Vui lòng thử lại.</p>
              ) : addresses.length === 0 ? (
                <div className="mt-3 text-sm text-slate-500">
                  <p>Bạn chưa có địa chỉ giao hàng đã lưu.</p>
                  <Link to="/account?tab=addresses" className="mt-2 inline-flex font-medium text-[#fb6900] hover:underline">
                    Thêm địa chỉ giao hàng
                  </Link>
                </div>
              ) : (
                <fieldset className="mt-3 space-y-2">
                  <legend className="sr-only">Chọn địa chỉ giao hàng</legend>
                  {addresses.map((address) => (
                    <label key={address.id} className={`block cursor-pointer border p-3 text-sm transition-colors ${
                      (selectedAddress?.id === address.id)
                        ? "border-[#fb6900] bg-[#fb6900]/5"
                        : "border-slate-200 hover:border-slate-300"
                    }`}>
                      <span className="flex items-start gap-2">
                        <input
                          type="radio"
                          name="deliveryAddress"
                          value={address.id}
                          checked={selectedAddress?.id === address.id}
                          onChange={() => setSelectedAddressId(address.id)}
                          className="mt-1 accent-[#fb6900]"
                        />
                        <span className="min-w-0">
                          <span className="font-semibold">{address.label || "Địa chỉ"}</span>
                          {address.isDefault && <span className="ml-2 text-xs text-[#fb6900]">Mặc định</span>}
                          <span className="mt-1 block text-slate-600">{address.fullAddress}</span>
                          <span className="mt-1 block text-xs text-slate-500">{address.receiverName} · {address.phoneNumber}</span>
                        </span>
                      </span>
                    </label>
                  ))}
                </fieldset>
              )}

              <label htmlFor="checkout-note" className="mt-5 block text-sm font-medium">Ghi chú cho nhà hàng</label>
              <textarea
                id="checkout-note"
                value={note}
                onChange={(event) => setNote(event.target.value)}
                rows={3}
                maxLength={500}
                placeholder="Ví dụ: ít cay, gọi trước khi giao..."
                className="mt-2 w-full resize-y border border-slate-300 bg-white px-3 py-2 text-sm outline-none focus:border-[#fb6900]"
              />

              {hasMixedRestaurants && (
                <p className="mt-3 text-sm text-red-600">Giỏ hàng có món từ nhiều nhà hàng. Vui lòng đặt từng nhà hàng riêng.</p>
              )}

              <h2 className="font-semibold text-lg">Tổng đơn hàng</h2>
              <div className="mt-4 flex justify-between text-sm text-slate-600">
                <span>Tạm tính ({items.reduce((sum, item) => sum + item.quantity, 0)} món)</span>
                <span>{formatVnd(total)}</span>
              </div>
              <div className="mt-4 border-t border-slate-200 pt-4 flex justify-between font-bold">
                <span>Tổng cộng</span>
                <span className="text-[#fb6900]">{formatVnd(total)}</span>
              </div>
              <button
                disabled={!selectedAddress || hasMixedRestaurants || checkoutMutation.isPending}
                onClick={() => selectedAddress && checkoutMutation.mutate(selectedAddress.id)}
                className="mt-6 w-full h-11 rounded-lg bg-[#fb6900] text-white font-semibold transition-colors hover:bg-[#e35f00] disabled:cursor-not-allowed disabled:bg-slate-200 disabled:text-slate-500"
              >
                {checkoutMutation.isPending ? "Đang tạo đơn..." : "Tiến hành đặt hàng"}
              </button>
            </aside>
          </div>
        )}
      </main>
      <Footer />
    </div>
  );
}