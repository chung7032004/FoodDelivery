import { useEffect, useState } from 'react';
import { Check, Clock3, X } from 'lucide-react';
import { AdminLayout } from './AdminLayout';
import { adminService, type AdminRoleAppeal } from '@/services/admin.service';
import Button from '@/components/ui/Button';

export default function AdminRoleAppealsPage() {
  const [appeals, setAppeals] = useState<AdminRoleAppeal[]>([]);
  const [responses, setResponses] = useState<Record<string, string>>({});
  const [loading, setLoading] = useState(true);
  const [processingId, setProcessingId] = useState<string | null>(null);
  const [error, setError] = useState('');

  async function loadAppeals() {
    setLoading(true);
    setError('');
    try {
      setAppeals(await adminService.getRoleAppeals());
    } catch {
      setError('Không thể tải danh sách khiếu nại.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void loadAppeals();
  }, []);

  async function review(appeal: AdminRoleAppeal, isApproved: boolean) {
    const response = responses[appeal.appealId]?.trim();
    if (!isApproved && (!response || response.length < 5)) {
      setError('Vui lòng nhập lý do từ chối ít nhất 5 ký tự.');
      return;
    }
    setProcessingId(appeal.appealId);
    setError('');
    try {
      await adminService.reviewRoleAppeal(appeal.appealId, isApproved, response);
      await loadAppeals();
    } catch {
      setError('Không thể xử lý khiếu nại. Hãy tải lại danh sách và thử lại.');
    } finally {
      setProcessingId(null);
    }
  }

  return (
    <AdminLayout>
      <div className="mb-6">
        <h1 className="font-display text-3xl font-bold text-foreground">Khiếu nại thu hồi quyền</h1>
        <p className="mt-1 text-sm text-muted">Xem xét yêu cầu khôi phục quyền Shipper hoặc chủ nhà hàng.</p>
      </div>
      {error && <p role="alert" className="mb-4 border-y border-red-200 py-3 text-sm text-red-700">{error}</p>}
      <div className="space-y-4">
        {loading ? (
          <p className="border-y border-border py-12 text-center text-sm text-muted">Đang tải khiếu nại...</p>
        ) : appeals.length === 0 ? (
          <p className="border-y border-border py-12 text-center text-sm text-muted">Hiện không có khiếu nại nào đang chờ xử lý.</p>
        ) : appeals.map((appeal) => (
          <article key={appeal.appealId} className="rounded-xl border border-border bg-surface p-5">
            <div className="flex flex-wrap items-start justify-between gap-3">
              <div>
                <h2 className="font-semibold text-foreground">{appeal.userName}</h2>
                <p className="mt-1 text-sm text-muted">{appeal.userEmail} · {appeal.roleName === 'Shipper' ? 'Shipper' : 'Chủ nhà hàng'}</p>
              </div>
              <span className="inline-flex items-center gap-1.5 rounded-full bg-amber-100 px-2.5 py-1 text-xs font-semibold text-amber-800"><Clock3 size={13} />Chờ xử lý</span>
            </div>
            <div className="mt-4 grid gap-4 sm:grid-cols-2">
              <div className="border-l-2 border-red-300 pl-3">
                <p className="text-xs font-semibold text-slate-500">Lý do thu hồi quyền</p>
                <p className="mt-1 whitespace-pre-wrap text-sm text-slate-800">{appeal.restrictionReason}</p>
              </div>
              <div className="border-l-2 border-orange-300 pl-3">
                <p className="text-xs font-semibold text-slate-500">Nội dung khiếu nại</p>
                <p className="mt-1 whitespace-pre-wrap text-sm text-slate-800">{appeal.appealReason}</p>
              </div>
            </div>
            <p className="mt-3 text-xs text-muted">Gửi lúc {new Date(appeal.submittedAt).toLocaleString('vi-VN')}</p>
            <textarea
              aria-label={`Phản hồi cho ${appeal.userName}`}
              maxLength={500}
              rows={2}
              value={responses[appeal.appealId] ?? ''}
              onChange={(event) => setResponses((current) => ({ ...current, [appeal.appealId]: event.target.value }))}
              placeholder="Phản hồi khi từ chối khiếu nại (bắt buộc)..."
              className="mt-4 w-full rounded-lg border border-slate-200 bg-white p-3 text-sm outline-none focus:border-orange-500"
            />
            <div className="mt-3 flex flex-wrap justify-end gap-2">
              <Button type="button" variant="danger" isLoading={processingId === appeal.appealId} leftIcon={<X size={16} />} disabled={(responses[appeal.appealId]?.trim().length ?? 0) < 5} onClick={() => void review(appeal, false)}>Từ chối</Button>
              <Button type="button" isLoading={processingId === appeal.appealId} leftIcon={<Check size={16} />} disabled={processingId !== null} onClick={() => void review(appeal, true)}>Khôi phục quyền</Button>
            </div>
          </article>
        ))}
      </div>
    </AdminLayout>
  );
}
