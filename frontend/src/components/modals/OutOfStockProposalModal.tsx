import { useEffect } from "react";
import { AlertTriangle, X } from "lucide-react";
import type { OrderDetail } from "@/services/order.service";

interface OutOfStockProposalModalProps {
  isOpen: boolean;
  orderCode: string;
  detail: OrderDetail | null;
  isLoading: boolean;
  loadError: string;
  isSubmitting: boolean;
  error: string;
  onRetry: () => void;
  onAccept: () => void;
  onReject: () => void;
  onClose: () => void;
}

function isWaitingDecision(status: number | string) {
  return String(status) === "2" || String(status) === "WaitingCustomerDecision";
}

export function OutOfStockProposalModal({
  isOpen,
  orderCode,
  detail,
  isLoading,
  loadError,
  isSubmitting,
  error,
  onRetry,
  onAccept,
  onReject,
  onClose,
}: OutOfStockProposalModalProps) {
  useEffect(() => {
    if (!isOpen) return;

    const previousOverflow = document.body.style.overflow;
    document.body.style.overflow = "hidden";
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Escape" && !isSubmitting) onClose();
    };
    window.addEventListener("keydown", handleKeyDown);

    return () => {
      document.body.style.overflow = previousOverflow;
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, [isOpen, isSubmitting, onClose]);

  if (!isOpen) return null;

  const removedItems = detail?.items.filter((item) => item.isRemoved) ?? [];
  const canRespond = detail ? isWaitingDecision(detail.currentStatus) : false;
  const proposalNote = [...(detail?.statusHistories ?? [])]
    .filter((history) => isWaitingDecision(history.status))
    .sort((first, second) => new Date(second.changedAt).getTime() - new Date(first.changedAt).getTime())[0]?.note;

  return (
    <div className="fixed inset-0 z-[130] flex items-center justify-center p-4" role="presentation">
      <button
        type="button"
        aria-label="Đóng đề xuất của nhà hàng"
        disabled={isSubmitting}
        className="absolute inset-0 bg-slate-950/60 backdrop-blur-sm disabled:cursor-wait"
        onClick={onClose}
      />
      <section
        role="dialog"
        aria-modal="true"
        aria-labelledby="out-of-stock-proposal-title"
        className="relative z-10 max-h-[85vh] w-full max-w-lg overflow-y-auto rounded-2xl bg-white shadow-2xl"
      >
        <header className="flex items-start justify-between gap-4 border-b border-slate-100 px-5 py-4">
          <div className="flex items-start gap-3">
            <span className="mt-0.5 rounded-lg bg-amber-50 p-2 text-amber-600">
              <AlertTriangle size={18} />
            </span>
            <div>
              <h2 id="out-of-stock-proposal-title" className="text-lg font-bold text-slate-900">
                Nhà hàng báo món hết
              </h2>
              <p className="mt-1 text-sm text-slate-500">Đơn hàng {orderCode}</p>
            </div>
          </div>
          <button
            type="button"
            aria-label="Đóng"
            disabled={isSubmitting}
            onClick={onClose}
            className="rounded-lg p-2 text-slate-500 transition hover:bg-slate-100 disabled:opacity-50"
          >
            <X size={18} />
          </button>
        </header>

        <div className="space-y-4 p-5">
          <p className="text-sm leading-6 text-slate-600">
            Nhà hàng không thể chuẩn bị một số món trong đơn. Bạn có thể tiếp tục với các món còn lại hoặc hủy đơn hàng.
          </p>

          {isLoading ? (
            <p className="py-6 text-center text-sm text-slate-500">Đang tải thông tin đề xuất...</p>
          ) : loadError ? (
            <div role="alert" className="rounded-lg bg-red-50 px-3 py-3 text-sm text-red-700">
              <p>{loadError}</p>
              <button type="button" onClick={onRetry} className="mt-2 font-semibold underline">
                Thử tải lại
              </button>
            </div>
          ) : detail ? (
            <>
              {!canRespond && (
                <p role="status" className="rounded-lg bg-slate-100 px-3 py-2 text-sm text-slate-700">
                  Đơn hàng đã được cập nhật và không còn chờ phản hồi.
                </p>
              )}
              {removedItems.length > 0 && (
                <div className="rounded-xl border border-amber-200 bg-amber-50/70 p-4">
                  <h3 className="text-sm font-semibold text-amber-900">Món không thể chuẩn bị</h3>
                  <ul className="mt-2 space-y-2">
                    {removedItems.map((item) => (
                      <li key={item.orderItemId} className="flex items-start justify-between gap-3 text-sm">
                        <span className="font-medium text-slate-800">
                          {item.quantity} × {item.productName}
                        </span>
                        <span className="shrink-0 text-slate-600">
                          {(item.totalPrice).toLocaleString("vi-VN")}đ
                        </span>
                      </li>
                    ))}
                  </ul>
                </div>
              )}
              {proposalNote && <p className="text-sm text-slate-600">Ghi chú: {proposalNote}</p>}
            </>
          ) : null}

          {error && <p role="alert" className="rounded-lg bg-red-50 px-3 py-2 text-sm text-red-700">{error}</p>}

          <div className="flex flex-col-reverse gap-2 border-t border-slate-100 pt-4 sm:flex-row sm:justify-end">
            <button
              type="button"
              onClick={onReject}
              disabled={isLoading || !canRespond || isSubmitting}
              className="rounded-xl border border-red-200 px-4 py-2.5 text-sm font-semibold text-red-700 transition hover:bg-red-50 disabled:opacity-50"
            >
              {isSubmitting ? "Đang xử lý..." : "Hủy đơn hàng"}
            </button>
            <button
              type="button"
              onClick={onAccept}
              disabled={isLoading || !canRespond || isSubmitting || removedItems.length === detail?.items.length}
              className="rounded-xl bg-orange-600 px-4 py-2.5 text-sm font-semibold text-white transition hover:bg-orange-700 disabled:opacity-50"
            >
              {isSubmitting ? "Đang xử lý..." : "Đồng ý, tiếp tục"}
            </button>
          </div>
          {detail && removedItems.length === detail.items.length && (
            <p className="text-xs text-slate-500">
              Đơn không còn món nào khả dụng nên chỉ có thể hủy.
            </p>
          )}
        </div>
      </section>
    </div>
  );
}
