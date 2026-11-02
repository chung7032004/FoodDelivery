import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useMemo, useState } from "react";
import { ArrowLeft, Minus, Plus, ShoppingBag, Trash2 } from "lucide-react";
import { Link, useNavigate } from "react-router-dom";
import { Footer } from "@/components/layout/Footer";
import { HeaderNav } from "@/components/layout/header/HeaderNav";
import { cartQueryKey, cartService } from "@/services/cart.service";
import { getImageUrl } from "@/utils/image";

function formatVnd(value: number) {
  return value.toLocaleString("vi-VN") + "đ";
}

export function CartPage() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const [selectedCartItemIds, setSelectedCartItemIds] = useState<Set<string> | null>(null);
  const { data: cartResult, isLoading } = useQuery({
    queryKey: cartQueryKey,
    queryFn: cartService.getCart,
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

  const items = useMemo(() => cartResult?.data?.data ?? [], [cartResult?.data?.data]);
  useEffect(() => {
    if (items.length === 0) {
      setSelectedCartItemIds(null);
      return;
    }

    setSelectedCartItemIds((current) => {
      if (current === null) return current;
      const currentItemIds = new Set(items.map((item) => item.id));
      return new Set([...current].filter((id) => currentItemIds.has(id)));
    });
  }, [items]);

  const selectedIds = selectedCartItemIds ?? new Set(items.map((item) => item.id));
  const selectedItems = items.filter((item) => selectedIds.has(item.id));
  const hasMixedRestaurants = new Set(selectedItems.map((item) => item.restaurantId)).size > 1;
  const total = selectedItems.reduce((sum, item) => sum + item.price * item.quantity, 0);

  return (
    <div className="min-h-screen bg-surface text-slate-900 flex flex-col">
      <HeaderNav />
      <main className="w-full max-w-5xl mx-auto px-5 lg:px-8 py-10 flex-1">
        <Link to="/products" className="inline-flex items-center gap-2 text-sm text-slate-500 hover:text-[#fb6900]">
          <ArrowLeft size={16} /> Tiếp tục mua sắm
        </Link>
        <h1 className="mt-5 mb-8 text-3xl font-display font-bold">Giỏ hàng của bạn</h1>

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
              <label className="flex cursor-pointer items-center gap-2 py-4 text-sm font-medium">
                <input
                  type="checkbox"
                  checked={selectedItems.length === items.length}
                  onChange={() => setSelectedCartItemIds(
                    selectedItems.length === items.length ? new Set() : null,
                  )}
                  aria-label="Chọn tất cả sản phẩm trong giỏ hàng"
                  className="h-4 w-4 accent-[#fb6900]"
                />
                Chọn tất cả ({items.length})
              </label>
              {items.map((item) => (
                <article key={item.id} className="flex items-center gap-4 py-5">
                  <input
                    type="checkbox"
                    checked={selectedIds.has(item.id)}
                    onChange={(event) => setSelectedCartItemIds((current) => {
                      const next = new Set(current ?? items.map((cartItem) => cartItem.id));
                      if (event.target.checked) next.add(item.id);
                      else next.delete(item.id);
                      return next;
                    })}
                    aria-label={`Chọn ${item.productName} để đặt hàng`}
                    className="h-4 w-4 shrink-0 accent-[#fb6900]"
                  />
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
              <h2 className="font-semibold text-lg">Tóm tắt giỏ hàng</h2>
              <div className="mt-4 flex justify-between text-sm text-slate-600">
                <span>Tạm tính ({selectedItems.reduce((sum, item) => sum + item.quantity, 0)} món)</span>
                <span>{formatVnd(total)}</span>
              </div>
              {selectedItems.length === 0 && (
                <p className="mt-3 text-sm text-slate-500">Vui lòng chọn ít nhất một sản phẩm để tiếp tục.</p>
              )}
              {hasMixedRestaurants && (
                <p className="mt-3 text-sm text-red-600">Các món đã chọn thuộc nhiều nhà hàng. Vui lòng chọn món từ một nhà hàng để đặt.</p>
              )}
              <p className="mt-4 text-sm text-slate-500">
                Ở bước tiếp theo, bạn sẽ xác nhận địa chỉ, kiểu giao hàng và phương thức thanh toán.
              </p>
              <button
                disabled={selectedItems.length === 0 || hasMixedRestaurants}
                onClick={() => navigate(`/checkout?cartItemIds=${selectedItems.map((item) => item.id).join(",")}`)}
                className="mt-6 w-full h-11 rounded-lg bg-[#fb6900] text-white font-semibold transition-colors hover:bg-[#e35f00] disabled:cursor-not-allowed disabled:bg-slate-200 disabled:text-slate-500"
              >
                Xác nhận đơn hàng
              </button>
            </aside>
          </div>
        )}
      </main>
      <Footer />
    </div>
  );
}