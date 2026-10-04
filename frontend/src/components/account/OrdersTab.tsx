import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { ChevronDown, ChevronLeft, ChevronRight, ChevronUp, Package, RefreshCw } from "lucide-react";
import { getImageUrl } from "@/utils/image";
import { orderService, type OrderDetail, type OrderHistoryItem } from "@/services/order.service";

const PAGE_SIZE = 5;

const STATUS_CONFIG: Record<string, { label: string; className: string }> = {
  "1": { label: "Chờ xác nhận", className: "bg-amber-100 text-amber-800" },
  "2": { label: "Chờ bạn phản hồi", className: "bg-orange-100 text-orange-800" },
  "3": { label: "Đã xác nhận", className: "bg-sky-100 text-sky-800" },
  "4": { label: "Đang chuẩn bị", className: "bg-blue-100 text-blue-800" },
  "5": { label: "Chờ lấy hàng", className: "bg-indigo-100 text-indigo-800" },
  "6": { label: "Đang giao", className: "bg-orange-100 text-orange-800" },
  "7": { label: "Đã giao", className: "bg-emerald-100 text-emerald-800" },
  "8": { label: "Đã hủy", className: "bg-slate-100 text-slate-600" },
  Pending: { label: "Chờ xác nhận", className: "bg-amber-100 text-amber-800" },
  WaitingCustomerDecision: { label: "Chờ bạn phản hồi", className: "bg-orange-100 text-orange-800" },
  Confirmed: { label: "Đã xác nhận", className: "bg-sky-100 text-sky-800" },
  Preparing: { label: "Đang chuẩn bị", className: "bg-blue-100 text-blue-800" },
  ReadyForPickup: { label: "Chờ lấy hàng", className: "bg-indigo-100 text-indigo-800" },
  Shipping: { label: "Đang giao", className: "bg-orange-100 text-orange-800" },
  Completed: { label: "Đã giao", className: "bg-emerald-100 text-emerald-800" },
  Cancelled: { label: "Đã hủy", className: "bg-slate-100 text-slate-600" },
};

function formatVnd(value: number) {
  return `${value.toLocaleString("vi-VN")}đ`;
}

function formatDate(value: string | null) {
  if (!value) return "Chưa cập nhật";
  return new Intl.DateTimeFormat("vi-VN", { dateStyle: "medium", timeStyle: "short" }).format(new Date(value));
}

function getStatus(status: number | string) {
  const config = STATUS_CONFIG[String(status)];
  return config ?? { label: String(status), className: "bg-slate-100 text-slate-600" };
}

export function OrdersTab() {
  const [page, setPage] = useState(1);
  const [expandedOrderId, setExpandedOrderId] = useState<string | null>(null);
  const historyQuery = useQuery({
    queryKey: ["my-order-history", page],
    queryFn: () => orderService.getMyHistory(page, PAGE_SIZE),
  });
  const detailQuery = useQuery({
    queryKey: ["my-order-detail", expandedOrderId],
    queryFn: () => orderService.getDetail(expandedOrderId!),
    enabled: Boolean(expandedOrderId),
  });

  const orders = historyQuery.data?.data?.data ?? [];
  const pagination = historyQuery.data?.data?.meta;
  const totalPages = Math.max(1, pagination?.totalPages ?? 1);
  const detail = detailQuery.data?.data ?? null;

  return (
    <section className="space-y-5">
      <div className="flex flex-wrap items-end justify-between gap-3">
        <div>
          <h2 className="font-display text-lg font-bold text-black">Lịch sử đơn hàng</h2>
          <p className="mt-1 text-sm text-muted">{pagination?.totalCount ?? 0} đơn hàng</p>
        </div>
        <button type="button" onClick={() => void historyQuery.refetch()} disabled={historyQuery.isFetching} aria-label="Tải lại lịch sử đơn hàng" className="rounded-lg border border-slate-200 p-2 text-slate-600 transition hover:bg-slate-50 disabled:opacity-50">
          <RefreshCw size={16} className={historyQuery.isFetching ? "animate-spin" : ""} />
        </button>
      </div>

      {historyQuery.isLoading ? (
        <div className="flex min-h-40 items-center justify-center text-sm text-muted">Đang tải lịch sử đơn hàng...</div>
      ) : historyQuery.isError || !historyQuery.data?.isSuccess ? (
        <div role="alert" className="flex flex-wrap items-center justify-between gap-3 border-y border-red-200 py-4 text-sm text-red-700">
          <span>{historyQuery.data?.message ?? "Không tải được lịch sử đơn hàng."}</span>
          <button type="button" onClick={() => void historyQuery.refetch()} className="font-semibold underline">Thử lại</button>
        </div>
      ) : orders.length === 0 ? (
        <div className="border-y border-slate-200 py-14 text-center">
          <Package size={30} className="mx-auto mb-3 text-slate-400" />
          <p className="font-medium text-slate-800">Bạn chưa có đơn hàng nào.</p>
          <p className="mt-1 text-sm text-muted">Các đơn đã đặt sẽ xuất hiện tại đây.</p>
        </div>
      ) : (
        <>
          <div className="divide-y divide-slate-200 border-y border-slate-200">
            {orders.map((order) => (
              <OrderRow
                key={order.orderId}
                order={order}
                expanded={expandedOrderId === order.orderId}
                onToggle={() => setExpandedOrderId((current) => current === order.orderId ? null : order.orderId)}
                detail={expandedOrderId === order.orderId ? detail : null}
                isDetailLoading={expandedOrderId === order.orderId && detailQuery.isLoading}
                detailError={expandedOrderId === order.orderId && detailQuery.isError ? "Không tải được chi tiết đơn hàng." : ""}
              />
            ))}
          </div>

          {totalPages > 1 && (
            <div className="flex items-center justify-between gap-3">
              <p className="text-sm text-muted">Trang {page} / {totalPages}</p>
              <div className="flex gap-2">
                <button type="button" onClick={() => { setPage((current) => current - 1); setExpandedOrderId(null); }} disabled={page <= 1} aria-label="Trang trước" className="rounded-lg border border-slate-200 p-2 text-slate-600 transition hover:bg-slate-50 disabled:opacity-40">
                  <ChevronLeft size={17} />
                </button>
                <button type="button" onClick={() => { setPage((current) => current + 1); setExpandedOrderId(null); }} disabled={page >= totalPages} aria-label="Trang sau" className="rounded-lg border border-slate-200 p-2 text-slate-600 transition hover:bg-slate-50 disabled:opacity-40">
                  <ChevronRight size={17} />
                </button>
              </div>
            </div>
          )}
        </>
      )}
    </section>
  );
}

function OrderRow({
  order,
  expanded,
  onToggle,
  detail,
  isDetailLoading,
  detailError,
}: {
  order: OrderHistoryItem;
  expanded: boolean;
  onToggle: () => void;
  detail: OrderDetail | null;
  isDetailLoading: boolean;
  detailError: string;
}) {
  const status = getStatus(order.currentStatus);

  return (
    <article className="py-4">
      <div className="flex flex-wrap items-start justify-between gap-4">
        <div className="min-w-0 space-y-1.5">
          <div className="flex flex-wrap items-center gap-2">
            <h3 className="font-semibold text-slate-900">{order.orderCode}</h3>
            <span className={`rounded-full px-2.5 py-1 text-xs font-semibold ${status.className}`}>{status.label}</span>
          </div>
          <p className="text-sm text-muted">Đặt lúc {formatDate(order.createdAt)}</p>
          {order.estimatedDeliveryTime && <p className="text-xs text-muted">Dự kiến giao: {formatDate(order.estimatedDeliveryTime)}</p>}
        </div>
        <div className="flex items-center gap-4">
          <p className="text-right text-sm font-semibold text-slate-900">{formatVnd(order.totalAmount)}</p>
          <button type="button" onClick={onToggle} aria-expanded={expanded} className="inline-flex items-center gap-1 rounded-lg px-2 py-1.5 text-sm font-medium text-orange-700 transition hover:bg-orange-50">
            {expanded ? "Thu gọn" : "Chi tiết"}
            {expanded ? <ChevronUp size={15} /> : <ChevronDown size={15} />}
          </button>
        </div>
      </div>

      {expanded && (
        <div className="mt-4 border-t border-slate-100 pt-4">
          {isDetailLoading ? <p className="text-sm text-muted">Đang tải chi tiết đơn hàng...</p> : null}
          {detailError ? <p role="alert" className="text-sm text-red-600">{detailError}</p> : null}
          {detail ? <OrderDetailPanel detail={detail} /> : null}
        </div>
      )}
    </article>
  );
}

function OrderDetailPanel({ detail }: { detail: OrderDetail }) {
  return (
    <div className="space-y-4">
      <div className="grid gap-3 text-sm sm:grid-cols-2">
        <div>
          <p className="text-xs text-muted">Người nhận</p>
          <p className="mt-1 font-medium text-slate-800">{detail.receiverName} · {detail.receiverPhone}</p>
        </div>
        <div>
          <p className="text-xs text-muted">Địa chỉ giao hàng</p>
          <p className="mt-1 font-medium text-slate-800">{detail.shippingAddress}</p>
        </div>
      </div>

      <div className="divide-y divide-slate-100 border-y border-slate-100">
        {detail.items.map((item) => (
          <div key={item.orderItemId} className="flex items-center gap-3 py-3">
            {item.productImage && <img src={getImageUrl(item.productImage)} alt="" className="h-12 w-12 rounded-lg border border-slate-100 object-cover" />}
            <div className="min-w-0 flex-1">
              <p className="truncate text-sm font-medium text-slate-800">{item.productName}</p>
              <p className="mt-0.5 text-xs text-muted">{item.quantity} × {formatVnd(item.unitPrice)}</p>
              {item.isRemoved && item.removeReason && <p className="mt-1 text-xs text-amber-700">{item.removeReason}</p>}
            </div>
            <p className="shrink-0 text-sm font-semibold text-slate-800">{formatVnd(item.totalPrice)}</p>
          </div>
        ))}
      </div>

      <div className="ml-auto max-w-xs space-y-1.5 text-sm">
        <div className="flex justify-between gap-4 text-muted"><span>Phí giao hàng</span><span>{formatVnd(detail.shippingFee)}</span></div>
        <div className="flex justify-between gap-4 border-t border-slate-200 pt-2 font-semibold text-slate-900"><span>Tổng cộng</span><span>{formatVnd(detail.totalAmount)}</span></div>
      </div>
      {detail.cancelReason && <p className="text-sm text-slate-600">Lý do hủy: {detail.cancelReason}</p>}
    </div>
  );
}