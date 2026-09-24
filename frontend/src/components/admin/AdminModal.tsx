import { ReactNode, useEffect } from 'react';
import { X } from 'lucide-react';

interface AdminModalProps {
  open: boolean;
  title: string;
  description?: string;
  children: ReactNode;
  onClose: () => void;
  size?: 'sm' | 'md' | 'lg';
}

export function AdminModal({ open, title, description, children, onClose, size = 'md' }: AdminModalProps) {
  useEffect(() => {
    if (!open) return;
    const previousOverflow = document.body.style.overflow;
    document.body.style.overflow = 'hidden';
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') onClose();
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => {
      document.body.style.overflow = previousOverflow;
      window.removeEventListener('keydown', handleKeyDown);
    };
  }, [open, onClose]);

  if (!open) return null;

  const sizes = { sm: 'max-w-md', md: 'max-w-xl', lg: 'max-w-3xl' };
  return <div className="fixed inset-0 z-[100] flex items-center justify-center p-4" role="presentation">
    <button aria-label="Đóng hộp thoại" className="absolute inset-0 cursor-default bg-slate-950/60 backdrop-blur-[2px]" onClick={onClose} />
    <div role="dialog" aria-modal="true" aria-labelledby="admin-modal-title" className={`relative z-10 flex max-h-[calc(100vh-2rem)] w-full ${sizes[size]} flex-col overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-2xl shadow-slate-950/25`}>
      <div className="flex shrink-0 items-start justify-between gap-4 border-b border-slate-200 bg-white px-6 py-5">
        <div><h2 id="admin-modal-title" className="font-display text-xl font-bold text-slate-950">{title}</h2>{description && <p className="mt-1 text-sm text-slate-500">{description}</p>}</div>
        <button type="button" onClick={onClose} aria-label="Đóng" className="rounded-lg p-2 text-slate-400 transition hover:bg-slate-100 hover:text-slate-950"><X size={19} /></button>
      </div>
      <div className="min-h-0 overflow-y-auto bg-white px-6 py-5">{children}</div>
    </div>
  </div>;
}
