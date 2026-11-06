import { useEffect } from "react";
import { X } from "lucide-react";
import type { OrderDetail } from "@/services/order.service";

type OrderStatusHistory = OrderDetail["statusHistories"][number];

interface OrderDeliveryHistoryModalProps {
  isOpen: boolean;
  onClose: () => void;
  orderCode: string;
  statusHistories: OrderStatusHistory[];
}

const STATUS_LABELS: Record<string, string> = {
  "1": "Chờ xác nhận",
  "2": "Chờ bạn phản hồi",
  "3": "Đã xác nhận",
  "4": "Đang chuẩn bị",
  "5": "Chờ lấy hàng",
  "6": "Đang giao",
  "7": "Đã giao",
  "8": "Đã hủy",
  Pending: "Chờ xác nhận",
  WaitingCustomerDecision: "Chờ bạn phản hồi",
  Confirmed: "Đã xác nhận",
  Preparing: "Đang chuẩn bị",
  ReadyForPickup: "Chờ lấy hàng",
  Shipping: "Đang giao",
  Completed: "Đã giao",
  Cancelled: "Đã hủy",
};

function formatDate(value: string) {
  return new Intl.DateTimeFormat("vi-VN", {
    dateStyle: "medium",
    timeStyle: "short",
  }).format(new Date(value));
}

export function OrderDeliveryHistoryModal({
  isOpen,
  onClose,
  orderCode,
  statusHistories,
}: OrderDeliveryHistoryModalProps) {
  useEffect(() => {
    if (!isOpen) return;

    const previousOverflow = document.body.style.overflow;
    document.body.style.overflow = "hidden";
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Escape") onClose();
    };
    window.addEventListener("keydown", handleKeyDown);

    return () => {
      document.body.style.overflow = previousOverflow;
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  const sortedHistories = [...statusHistories].sort(
    (first, second) => new Date(first.changedAt).getTime() - new Date(second.changedAt).getTime(),
  );

  return (
    <div className="fixed inset-0 z-[120] flex items-center justify-center p-4" role="presentation">
      <button
        type="button"
        aria-label="Đóng lịch sử giao hàng"
        className="absolute inset-0 bg-slate-950/60 backdrop-blur-sm"
        onClick={onClose}
      />
      <section
        role="dialog"
        aria-modal="true"
        aria-labelledby="order-delivery-history-title"
        className="relative z-10 max-h-[85vh] w-full max-w-lg overflow-y-auto rounded-2xl bg-white shadow-2xl"
      >
        <header className="flex items-start justify-between gap-4 border-b border-slate-100 px-5 py-4">
          <div>
            <h2 id="order-delivery-history-title" className="text-lg font-bold text-slate-900">
              Lịch sử giao hàng
            </h2>
            <p className="mt-1 text-sm text-slate-500">Đơn hàng {orderCode}</p>
          </div>
          <button
            type="button"
            aria-label="Đóng"
            onClick={onClose}
            className="rounded-lg p-2 text-slate-500 transition hover:bg-slate-100"
          >
            <X size={18} />
          </button>
        </header>

        <div className="p-5">
          {sortedHistories.length === 0 ? (
            <p className="py-8 text-center text-sm text-slate-500">
              Chưa có cập nhật về quá trình đơn hàng.
            </p>
          ) : (
            <ol>
              {sortedHistories.map((history, index) => {
                const isCurrent = index === sortedHistories.length - 1;
                const status = String(history.status);
                return (
                  <li
                    key={`${status}-${history.changedAt}-${index}`}
                    className="relative flex gap-3 pb-6 last:pb-0"
                  >
                    {index < sortedHistories.length - 1 && (
                      <span
                        aria-hidden="true"
                        className="absolute left-[7px] top-4 h-[calc(100%-8px)] w-px bg-slate-200"
                      />
                    )}
                    <span
                      aria-hidden="true"
                      className={`relative mt-1 h-4 w-4 shrink-0 rounded-full border-[3px] bg-white ${
                        isCurrent ? "border-orange-500" : "border-emerald-500"
                      }`}
                    />
                    <div className="min-w-0 flex-1">
                      <div className="flex flex-wrap items-center justify-between gap-x-3 gap-y-1">
                        <p className="text-sm font-semibold text-slate-800">
                          {STATUS_LABELS[status] ?? status}
                        </p>
                        <time className="text-xs text-slate-500">
                          {formatDate(history.changedAt)}
                        </time>
                      </div>
                      {history.note && <p className="mt-1 text-sm text-slate-600">{history.note}</p>}
                      {history.changedBy && (
                        <p className="mt-1 text-xs text-slate-500">
                          Cập nhật bởi {history.changedBy}
                        </p>
                      )}
                    </div>
                  </li>
                );
              })}
            </ol>
          )}
        </div>
      </section>
    </div>
  );
}
