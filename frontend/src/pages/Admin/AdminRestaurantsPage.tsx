import { useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { Check, Clock3, Eye, MapPin, Phone} from 'lucide-react';
import { AdminLayout } from './AdminLayout';
import { AdminModal } from '@/components/admin/AdminModal';
import Button from '@/components/ui/Button';
import Input from '@/components/ui/Input';
import { restaurantService, type RestaurantApplication, type RestaurantApplicationStatus } from '@/services/restaurant.service';
import { toast } from 'sonner';

type StatusFilter = 'all' | RestaurantApplicationStatus;

const statusOptions: { value: StatusFilter; label: string }[] = [
  { value: 'all', label: 'Tất cả' },
  { value: 0, label: 'Chờ duyệt' },
  { value: 1, label: 'Đã duyệt' },
  { value: 2, label: 'Từ chối' },
];

function statusDetails(status: RestaurantApplicationStatus) {
  if (status === 1) return { label: 'Đã duyệt', style: 'bg-emerald-100 text-emerald-800' };
  if (status === 2) return { label: 'Từ chối', style: 'bg-red-100 text-red-800' };
  return { label: 'Chờ duyệt', style: 'bg-amber-100 text-amber-800' };
}

function formatDate(value: string) {
  return new Intl.DateTimeFormat('vi-VN', { dateStyle: 'medium' }).format(new Date(value));
}

function formatTime(value: string) {
  return value.slice(0, 5);
}

export default function AdminRestaurantsPage() {
  const queryClient = useQueryClient();
  const [statusFilter, setStatusFilter] = useState<StatusFilter>('all');
  const [page, setPage] = useState(1);
  const [selectedApplication, setSelectedApplication] = useState<RestaurantApplication | null>(null);
  const [rejectReason, setRejectReason] = useState('');
  const [isReviewing, setIsReviewing] = useState(false);
  const [error, setError] = useState('');
  const query = useQuery({
    queryKey: ['admin-restaurants', statusFilter, page],
    queryFn: () => restaurantService.getAdminRestaurants(page, 10, statusFilter === 'all' ? undefined : statusFilter),
  });

  const result = query.data;
  const restaurants = result?.data ?? [];
  const pendingCount = restaurants.filter((restaurant) => restaurant.applicationStatus === 0).length;

  async function handleReview(application: RestaurantApplication, isApproved: boolean, reason?: string) {
    setIsReviewing(true);
    setError('');
    try {
      await restaurantService.reviewApplication(application.restaurantId, isApproved, reason);
      setSelectedApplication(null);
      setRejectReason('');
      await queryClient.invalidateQueries({ queryKey: ['admin-restaurants'] });
      toast.success(isApproved ? 'Đã duyệt nhà hàng.' : 'Đã từ chối hồ sơ nhà hàng.');
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : 'Không thể cập nhật hồ sơ nhà hàng.');
    } finally {
      setIsReviewing(false);
    }
  }

  return (
    <AdminLayout>
      <div className="mb-6 flex flex-wrap items-end justify-between gap-3">
        <div>
          <h1 className="font-display text-3xl font-bold text-foreground">Nhà hàng</h1>
          <p className="mt-1 text-sm text-muted">Duyệt hồ sơ đăng ký và theo dõi nhà hàng trong hệ thống.</p>
        </div>
        <div className="flex items-center gap-2 rounded-lg border border-amber-200 bg-amber-50 px-3 py-2 text-sm font-semibold text-amber-800">
          <Clock3 size={16} /> {query.isLoading ? '...' : `${pendingCount} chờ duyệt trên trang`}
        </div>
      </div>

      <div className="mb-4 flex flex-wrap items-center gap-2" role="group" aria-label="Lọc trạng thái nhà hàng">
        {statusOptions.map((option) => (
          <button
            key={String(option.value)}
            type="button"
            aria-pressed={statusFilter === option.value}
            onClick={() => { setStatusFilter(option.value); setPage(1); }}
            className={`rounded-lg border px-3 py-2 text-sm font-medium transition ${statusFilter === option.value ? 'border-orange-500 bg-orange-50 text-orange-700' : 'border-border bg-surface text-muted hover:bg-black/5'}`}
          >
            {option.label}
          </button>
        ))}
        <span className="ml-auto text-sm text-muted">{result?.meta.totalCount ?? 0} nhà hàng</span>
      </div>

      {error && <p role="alert" className="mb-4 border-y border-red-200 py-3 text-sm text-red-700">{error}</p>}
      {query.isError && <p role="alert" className="mb-4 border-y border-red-200 py-3 text-sm text-red-700">Không tải được danh sách nhà hàng.</p>}

      <div className="overflow-x-auto rounded-xl border border-border bg-surface">
        <table className="w-full min-w-[1000px] text-left text-sm">
          <thead>
            <tr className="border-b border-border text-xs uppercase text-muted">
              <th className="px-4 py-3">Nhà hàng</th>
              <th className="px-4 py-3">Chủ nhà hàng</th>
              <th className="px-4 py-3">Địa chỉ</th>
              <th className="px-4 py-3">Giờ hoạt động</th>
              <th className="px-4 py-3">Trạng thái</th>
              <th className="px-4 py-3 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {query.isLoading ? (
              <tr><td colSpan={6} className="px-4 py-12 text-center text-muted">Đang tải nhà hàng...</td></tr>
            ) : restaurants.length === 0 ? (
              <tr><td colSpan={6} className="px-4 py-12 text-center text-muted">Không có hồ sơ nhà hàng.</td></tr>
            ) : restaurants.map((restaurant) => {
              const badge = statusDetails(restaurant.applicationStatus);
              return (
                <tr key={restaurant.restaurantId} className="border-b border-border last:border-0 hover:bg-orange-500/[.02]">
                  <td className="px-4 py-4">
                    <p className="font-semibold text-foreground">{restaurant.name}</p>
                    <p className="mt-1 flex items-center gap-1.5 text-xs text-muted"><Phone size={12} />{restaurant.phone}</p>
                  </td>
                  <td className="px-4 py-4">
                    <p className="font-medium text-foreground">{restaurant.ownerName}</p>
                    <p className="mt-1 text-xs text-muted">{restaurant.ownerEmail}</p>
                    <p className="mt-1 text-xs text-muted">{restaurant.ownerPhone}</p>
                  </td>
                  <td className="max-w-64 px-4 py-4 text-muted"><span className="flex gap-1.5"><MapPin size={14} className="mt-0.5 shrink-0" /><span>{restaurant.address}</span></span></td>
                  <td className="px-4 py-4 text-muted">{formatTime(restaurant.openTime)} - {formatTime(restaurant.closeTime)}</td>
                  <td className="px-4 py-4">
                    <span className={`rounded-full px-2.5 py-1 text-xs font-semibold ${badge.style}`}>{badge.label}</span>
                    <p className="mt-1 text-xs text-muted">{restaurant.isOpen ? 'Đang mở cửa' : 'Đang đóng cửa'}</p>
                  </td>
                  <td className="px-4 py-4 text-right">
                    <div className="flex justify-end gap-1.5">
                      <button type="button" onClick={() => { setSelectedApplication(restaurant); setRejectReason(''); }} title="Xem và xét duyệt hồ sơ" aria-label={`Xem ${restaurant.name}`} className="rounded-lg p-2 text-slate-600 hover:bg-slate-100"><Eye size={17} /></button>
                      {restaurant.applicationStatus === 0 && (
                        <span className="sr-only">Đang chờ xét duyệt</span>
                      )}
                    </div>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      <div className="mt-4 flex items-center justify-between text-sm text-muted">
        <span>Trang {page} / {Math.max(1, result?.meta.totalPages ?? 1)}</span>
        <div className="flex gap-2">
          <button type="button" disabled={page <= 1} onClick={() => setPage((value) => value - 1)} className="rounded-lg border border-border px-3 py-2 disabled:opacity-40">Trước</button>
          <button type="button" disabled={page >= (result?.meta.totalPages ?? 1)} onClick={() => setPage((value) => value + 1)} className="rounded-lg border border-border px-3 py-2 disabled:opacity-40">Sau</button>
        </div>
      </div>

      <AdminModal
        open={Boolean(selectedApplication)}
        onClose={() => { if (!isReviewing) setSelectedApplication(null); }}
        title={selectedApplication?.applicationStatus === 0 ? 'Xét duyệt nhà hàng' : 'Chi tiết nhà hàng'}
        description={selectedApplication?.ownerName}
        size="lg"
      >
        {selectedApplication && (
          <div className="space-y-5">
            <div className="grid gap-4 sm:grid-cols-2">
              <Detail label="Tên nhà hàng" value={selectedApplication.name} />
              <Detail label="Điện thoại nhà hàng" value={selectedApplication.phone} />
              <Detail label="Chủ sở hữu" value={`${selectedApplication.ownerName} · ${selectedApplication.ownerEmail}`} />
              <Detail label="Số điện thoại chủ" value={selectedApplication.ownerPhone} />
              <Detail label="Địa chỉ" value={selectedApplication.address} />
              <Detail label="Tọa độ" value={`${selectedApplication.latitude.toFixed(5)}, ${selectedApplication.longitude.toFixed(5)}`} />
              <Detail label="Giờ hoạt động" value={`${formatTime(selectedApplication.openTime)} - ${formatTime(selectedApplication.closeTime)}`} />
              <Detail label="Ngày đăng ký" value={formatDate(selectedApplication.createdAt)} />
            </div>
            {selectedApplication.rejectionReason && <p className="border-l-2 border-red-400 bg-red-50 px-3 py-2 text-sm text-red-700">Lý do từ chối: {selectedApplication.rejectionReason}</p>}
            {selectedApplication.applicationStatus === 0 && (
              <div className="border-t border-slate-200 pt-4">
                <Input label="Lý do từ chối (nếu từ chối)" maxLength={500} value={rejectReason} onChange={(event) => setRejectReason(event.target.value)} placeholder="Nêu thông tin cần bổ sung..." fullWidth />
                <div className="mt-4 flex flex-wrap justify-end gap-2">
                  <Button type="button" variant="danger" isLoading={isReviewing} disabled={rejectReason.trim().length < 5} onClick={() => void handleReview(selectedApplication, false, rejectReason.trim())}>Từ chối</Button>
                  <Button type="button" isLoading={isReviewing} leftIcon={<Check size={16} />} onClick={() => void handleReview(selectedApplication, true)}>Duyệt nhà hàng</Button>
                </div>
              </div>
            )}
          </div>
        )}
      </AdminModal>
    </AdminLayout>
  );
}

function Detail({ label, value }: { label: string; value: string }) {
  return <div className="min-w-0"><p className="text-xs text-slate-500">{label}</p><p className="mt-1 break-words text-sm font-medium text-slate-800">{value}</p></div>;
}