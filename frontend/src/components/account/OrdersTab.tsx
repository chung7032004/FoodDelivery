import { useCallback, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { ChevronDown, ChevronLeft, ChevronRight, ChevronUp, Package, RefreshCw } from "lucide-react";
import { OrderDeliveryHistoryModal } from "@/components/modals/OrderDeliveryHistoryModal";
import { OutOfStockProposalModal } from "@/components/modals/OutOfStockProposalModal";
import { CancelOrderModal } from "@/components/modals/CancelOrderModal";
import { getImageUrl } from "@/utils/image";
import { orderService, type OrderDetail, type OrderHistoryItem } from "@/services/order.service";

const PAGE_SIZE = 5;
const CUSTOMER_CANCEL_SUGGESTIONS = [
  "Tôi muốn đổi địa chỉ nhận hàng",
  "Tôi muốn thay đổi món / thêm bớt món",
  "Thời gian chuẩn bị / giao hàng quá lâu",
  "Tôi đặt nhầm đơn hàng",
  "Tìm thấy món khác / quán khác phù hợp hơn",
];

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
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const [expandedOrderId, setExpandedOrderId] = useState<string | null>(null);
  const [proposalOrderId, setProposalOrderId] = useState<string | null>(null);
  const [cancellingOrder, setCancellingOrder] = useState<{ id: string; code: string } | null>(null);

  const cancelMutation = useMutation({
    mutationFn: ({ orderId, reason }: { orderId: string; reason: string }) =>
      orderService.cancelOrder(orderId, reason),
    onSuccess: async () => {
      setCancellingOrder(null);
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: ["my-order-history"] }),
      ]);
    },
  });
  const historyQuery = useQuery({
    queryKey: ["my-order-history", page],
    queryFn: () => orderService.getMyHistory(page, PAGE_SIZE),
  });
  const detailQuery = useQuery({
    queryKey: ["my-order-detail", expandedOrderId],
    queryFn: async () => {
      const result = await orderService.getDetail(expandedOrderId!);
      if (!result.isSuccess || !result.data) {
        throw new Error(result.message ?? "Không tải được chi tiết đơn hàng.");
      }
      return result;
    },
    enabled: Boolean(expandedOrderId),
  });

  const orders = historyQuery.data?.data?.data ?? [];
  const pagination = historyQuery.data?.data?.meta;
  const totalPages = Math.max(1, pagination?.totalPages ?? 1);
  const detail = detailQuery.data?.data ?? null;
  const proposalOrder = orders.find((order) => order.orderId === proposalOrderId);
  const proposalMutation = useMutation({
    mutationFn: ({ orderId, isAccepted }: { orderId: string; isAccepted: boolean }) =>
      orderService.respondToProposal(orderId, isAccepted),
    onSuccess: async (_result, variables) => {
      setProposalOrderId(null);
      setExpandedOrderId(null);
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: ["my-order-history"] }),
        queryClient.invalidateQueries({ queryKey: ["my-order-detail", variables.orderId] }),
      ]);
    },
  });
  const proposalError = proposalMutation.isError
    ? proposalMutation.error instanceof Error
      ? proposalMutation.error.message
      : "Không thể gửi phản hồi. Vui lòng thử lại."
    : "";
  const { isPending: isProposalPending, reset: resetProposalMutation } = proposalMutation;
  const closeProposalModal = useCallback(() => {
    if (isProposalPending) return;
    setProposalOrderId(null);
    resetProposalMutation();
  }, [isProposalPending, resetProposalMutation]);

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
                onRespondToProposal={(orderId) => {
                  setExpandedOrderId(orderId);
                  setProposalOrderId(orderId);
                  proposalMutation.reset();
                }}
                onCancel={(orderId, orderCode) => {
                  cancelMutation.reset();
                  setCancellingOrder({ id: orderId, code: orderCode });
                }}
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
      <OutOfStockProposalModal
        isOpen={Boolean(proposalOrderId)}
        orderCode={proposalOrder?.orderCode ?? ""}
        detail={proposalOrderId && expandedOrderId === proposalOrderId && !detailQuery.isFetching ? detail : null}
        isLoading={Boolean(proposalOrderId) && expandedOrderId === proposalOrderId && detailQuery.isFetching}
        loadError={
          proposalOrderId && expandedOrderId === proposalOrderId && detailQuery.isError
            ? "Không thể tải thông tin đề xuất."
            : ""
        }
        isSubmitting={proposalMutation.isPending}
        error={proposalError}
        onRetry={() => void detailQuery.refetch()}
        onAccept={() => {
          if (proposalOrderId) proposalMutation.mutate({ orderId: proposalOrderId, isAccepted: true });
        }}
        onReject={() => {
          if (proposalOrderId) proposalMutation.mutate({ orderId: proposalOrderId, isAccepted: false });
        }}
        onClose={closeProposalModal}
      />
      <CancelOrderModal
        isOpen={Boolean(cancellingOrder)}
        title={`Hủy đơn hàng${cancellingOrder ? ` #${cancellingOrder.code}` : ""}`}
        suggestedReasons={CUSTOMER_CANCEL_SUGGESTIONS}
        isSubmitting={cancelMutation.isPending}
        error={cancelMutation.isError
          ? cancelMutation.error instanceof Error
            ? cancelMutation.error.message
            : "Không thể hủy đơn hàng. Vui lòng thử lại."
          : ""}
        onConfirm={(reason) => {
          if (cancellingOrder) cancelMutation.mutate({ orderId: cancellingOrder.id, reason });
        }}
        onClose={() => {
          if (cancelMutation.isPending) return;
          setCancellingOrder(null);
          cancelMutation.reset();
        }}
      />
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
  onRespondToProposal,
  onCancel,
}: {
  order: OrderHistoryItem;
  expanded: boolean;
  onToggle: () => void;
  detail: OrderDetail | null;
  isDetailLoading: boolean;
  detailError: string;
  onRespondToProposal: (orderId: string) => void;
  onCancel: (orderId: string, orderCode: string) => void;
}) {
  const status = getStatus(order.currentStatus);
  const canCancel = ["1", "2", "3", "Pending", "WaitingCustomerDecision", "Confirmed"]
    .includes(String(order.currentStatus));

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
          {String(order.currentStatus) === "2" || String(order.currentStatus) === "WaitingCustomerDecision" ? (
            <button
              type="button"
              onClick={() => onRespondToProposal(order.orderId)}
              className="mt-2 rounded-lg bg-amber-50 px-3 py-1.5 text-sm font-semibold text-amber-800 transition hover:bg-amber-100"
            >
              Phản hồi đề xuất của nhà hàng
            </button>
          ) : null}
          {canCancel && (
            <button
              type="button"
              onClick={() => onCancel(order.orderId, order.orderCode)}
              className="mt-2 rounded-lg border border-red-200 px-3 py-1.5 text-sm font-semibold text-red-700 transition hover:bg-red-50"
            >
              Hủy đơn hàng
            </button>
          )}
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
  const [isHistoryOpen, setIsHistoryOpen] = useState(false);

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

      <div className="ml-auto max-w-sm space-y-1.5 text-sm">
        <div className="flex justify-between gap-4 text-muted"><span>Phí giao hàng</span><span>{formatVnd(detail.shippingFee)}</span></div>
        <div className="flex flex-wrap items-center justify-between gap-2 border-t border-slate-200 pt-2 font-semibold text-slate-900">
          <span>Tổng cộng</span>
          <div className="flex items-center gap-3">
            <span>{formatVnd(detail.totalAmount)}</span>
            <button
              type="button"
              onClick={() => setIsHistoryOpen(true)}
              className="rounded-lg border border-orange-200 px-3 py-1.5 text-xs font-semibold text-orange-700 transition hover:bg-orange-50"
            >
              Lịch sử giao hàng
            </button>
          </div>
        </div>
      </div>
      {detail.cancelReason && <p className="text-sm text-slate-600">Lý do hủy: {detail.cancelReason}</p>}
      <OrderDeliveryHistoryModal
        isOpen={isHistoryOpen}
        onClose={() => setIsHistoryOpen(false)}
        orderCode={detail.orderCode}
        statusHistories={detail.statusHistories ?? []}
      />
    </div>
  );
}