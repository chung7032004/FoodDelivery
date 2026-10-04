import { ReactNode, useEffect } from 'react';
import { AlertTriangle } from 'lucide-react';
import Button from '@/components/ui/Button';

interface ConfirmationModalProps {
  open: boolean;
  title: string;
  description: string;
  confirmLabel: string;
  cancelLabel?: string;
  isLoading?: boolean;
  tone?: 'danger' | 'warning' | 'primary';
  icon?: ReactNode;
  onConfirm: () => void | Promise<void>;
  onCancel: () => void;
}

const toneClasses = {
  danger: 'bg-red-50 text-red-600',
  warning: 'bg-amber-50 text-amber-700',
  primary: 'bg-orange-50 text-orange-600',
};

export function ConfirmationModal({
  open,
  title,
  description,
  confirmLabel,
  cancelLabel = 'Hủy',
  isLoading = false,
  tone = 'primary',
  icon,
  onConfirm,
  onCancel,
}: ConfirmationModalProps) {
  useEffect(() => {
    if (!open) return;
    const previousOverflow = document.body.style.overflow;
    document.body.style.overflow = 'hidden';
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape' && !isLoading) onCancel();
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => {
      document.body.style.overflow = previousOverflow;
      window.removeEventListener('keydown', handleKeyDown);
    };
  }, [isLoading, onCancel, open]);

  if (!open) return null;
  const ModalIcon = icon ? null : AlertTriangle;

  return (
    <div className="fixed inset-0 z-[120] flex items-center justify-center p-4" role="presentation">
      <button
        type="button"
        aria-label="Đóng hộp thoại xác nhận"
        disabled={isLoading}
        className="absolute inset-0 cursor-default bg-slate-950/55 backdrop-blur-[2px] disabled:cursor-wait"
        onClick={onCancel}
      />
      <section
        role="alertdialog"
        aria-modal="true"
        aria-labelledby="confirmation-modal-title"
        aria-describedby="confirmation-modal-description"
        className="relative z-10 w-full max-w-md rounded-xl border border-slate-200 bg-white p-6 shadow-2xl"
      >
        <div className="flex items-start gap-4">
          <div className={`flex h-10 w-10 shrink-0 items-center justify-center rounded-lg ${toneClasses[tone]}`}>
            {icon ?? (ModalIcon && <ModalIcon size={19} />)}
          </div>
          <div className="min-w-0 pt-0.5">
            <h2 id="confirmation-modal-title" className="text-base font-semibold text-slate-900">{title}</h2>
            <p id="confirmation-modal-description" className="mt-1.5 text-sm leading-6 text-slate-600">{description}</p>
          </div>
        </div>
        <div className="mt-6 flex justify-end gap-2">
          <button
            type="button"
            onClick={onCancel}
            disabled={isLoading}
            className="h-10 rounded-lg border border-slate-200 px-4 text-sm font-medium text-slate-700 transition hover:bg-slate-50 disabled:opacity-50"
          >
            {cancelLabel}
          </button>
          <Button
            type="button"
            variant={tone === 'danger' ? 'danger' : 'primary'}
            size="md"
            isLoading={isLoading}
            onClick={() => void onConfirm()}
          >
            {confirmLabel}
          </Button>
        </div>
      </section>
    </div>
  );
}