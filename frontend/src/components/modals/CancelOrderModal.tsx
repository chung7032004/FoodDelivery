import { useEffect, useState } from "react";
import { AlertTriangle, X } from "lucide-react";

interface CancelOrderModalProps {
  isOpen: boolean;
  title?: string;
  subtitle?: string;
  suggestedReasons: string[];
  error?: string;
  isSubmitting?: boolean;
  confirmText?: string;
  onConfirm: (reason: string) => void;
  onClose: () => void;
}

export function CancelOrderModal({
  isOpen,
  title = "Hủy đơn hàng",
  subtitle = "Vui lòng chọn hoặc nhập lý do để chúng tôi hỗ trợ tốt hơn.",
  suggestedReasons,
  error: submitError = "",
  isSubmitting = false,
  confirmText = "Xác nhận hủy đơn",
  onConfirm,
  onClose,
}: CancelOrderModalProps) {
  const [selectedReason, setSelectedReason] = useState<string>("");
  const [customReason, setCustomReason] = useState<string>("");
  const [error, setError] = useState<string>("");

  useEffect(() => {
    if (isOpen) {
      setSelectedReason(suggestedReasons[0] || "");
      setCustomReason("");
      setError("");
    }
  }, [isOpen, suggestedReasons]);

  useEffect(() => {
    if (!isOpen) return;
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Escape" && !isSubmitting) onClose();
    };
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [isOpen, isSubmitting, onClose]);

  if (!isOpen) return null;

  const isOther = selectedReason === "Lý do khác";
  const finalReason = isOther ? customReason.trim() : selectedReason.trim();

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!finalReason) {
      setError("Vui lòng chọn hoặc nhập lý do hủy.");
      return;
    }
    setError("");
    onConfirm(finalReason);
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
      {/* Backdrop */}
      <div
        className="fixed inset-0 bg-slate-900/50 backdrop-blur-sm transition-opacity"
        onClick={() => !isSubmitting && onClose()}
      />

      {/* Modal Dialog */}
      <div className="relative w-full max-w-lg rounded-2xl bg-white p-6 shadow-2xl transition-all">
        {/* Header */}
        <div className="flex items-start justify-between gap-4">
          <div className="flex items-center gap-3">
            <div className="flex h-11 w-11 items-center justify-center rounded-xl bg-red-100 text-red-600">
              <AlertTriangle size={22} />
            </div>
            <div>
              <h3 className="text-lg font-bold text-slate-900">{title}</h3>
              <p className="mt-0.5 text-xs text-slate-500">{subtitle}</p>
            </div>
          </div>
          <button
            type="button"
            onClick={onClose}
            disabled={isSubmitting}
            className="rounded-lg p-1.5 text-slate-400 hover:bg-slate-100 hover:text-slate-600 disabled:opacity-50"
          >
            <X size={18} />
          </button>
        </div>

        {/* Form Body */}
        <form onSubmit={handleSubmit} className="mt-5 space-y-4">
          <p className="text-xs font-semibold text-slate-700">Chọn lý do hủy:</p>

          <div className="space-y-2">
            {suggestedReasons.map((reason) => (
              <label
                key={reason}
                className={`flex cursor-pointer items-center gap-3 rounded-xl border p-3 text-sm transition-all ${
                  selectedReason === reason
                    ? "border-red-300 bg-red-50/50 text-slate-900 font-medium"
                    : "border-slate-200 text-slate-600 hover:bg-slate-50"
                }`}
              >
                <input
                  type="radio"
                  name="cancelReason"
                  checked={selectedReason === reason}
                  onChange={() => {
                    setSelectedReason(reason);
                    setError("");
                  }}
                  className="accent-red-600"
                />
                <span>{reason}</span>
              </label>
            ))}

            <label
              className={`flex cursor-pointer items-center gap-3 rounded-xl border p-3 text-sm transition-all ${
                isOther
                  ? "border-red-300 bg-red-50/50 text-slate-900 font-medium"
                  : "border-slate-200 text-slate-600 hover:bg-slate-50"
              }`}
            >
              <input
                type="radio"
                name="cancelReason"
                checked={isOther}
                onChange={() => {
                  setSelectedReason("Lý do khác");
                  setError("");
                }}
                className="accent-red-600"
              />
              <span>Lý do khác (tự nhập)</span>
            </label>
          </div>

          {/* Ô nhập lý do khác nếu chọn */}
          {isOther && (
            <div className="mt-2">
              <textarea
                value={customReason}
                onChange={(e) => {
                  setCustomReason(e.target.value);
                  setError("");
                }}
                maxLength={300}
                rows={3}
                placeholder="Nhập chi tiết lý do của bạn..."
                className="w-full rounded-xl border border-slate-200 p-3 text-sm outline-none focus:border-red-500 focus:ring-1 focus:ring-red-500"
                autoFocus
              />
            </div>
          )}

          {error && <p className="text-xs font-medium text-red-600">{error}</p>}
          {submitError && <p role="alert" className="text-xs font-medium text-red-600">{submitError}</p>}

          {/* Footer Actions */}
          <div className="mt-6 flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
            <button
              type="button"
              onClick={onClose}
              disabled={isSubmitting}
              className="rounded-xl border border-slate-200 px-4 py-2.5 text-sm font-semibold text-slate-600 hover:bg-slate-50 disabled:opacity-50"
            >
              Không hủy nữa
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="rounded-xl bg-red-600 px-5 py-2.5 text-sm font-bold text-white shadow-md shadow-red-200 transition hover:bg-red-700 disabled:opacity-50"
            >
              {isSubmitting ? "Đang xử lý..." : confirmText}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
