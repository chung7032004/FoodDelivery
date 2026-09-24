import { useEffect, useState } from 'react';
import { Check, X } from 'lucide-react';
import { AdminLayout } from './AdminLayout';
import { adminService, PagedResult, ShipperApplication } from '@/services/admin.service';

export default function AdminShippersPage() {
  const [result, setResult] = useState<PagedResult<ShipperApplication> | null>(null);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [processingId, setProcessingId] = useState<string | null>(null);
  const [error, setError] = useState('');

  // Tải danh sách hồ sơ Shipper chờ duyệt
  const loadShippers = async (currentPage: number, isMounted = true) => {
    setLoading(true);
    setError('');
    try {
      const data = await adminService.getPendingShippers(currentPage, 10);
      if (isMounted) setResult(data);
    } catch {
      if (isMounted) setError('Không thể tải danh sách hồ sơ shipper.');
    } finally {
      if (isMounted) setLoading(false);
    }
  };

  useEffect(() => {
    let isSubscribed = true;
    void loadShippers(page, isSubscribed);
    return () => {
      isSubscribed = false;
    };
  }, [page]);

  // Xử lý Phê duyệt / Từ chối hồ sơ
  async function handleReview(shipper: ShipperApplication, approved: boolean) {
    let reason: string | undefined = undefined;

    if (approved) {
      if (!window.confirm(`Bạn có chắc chắn muốn PHÊ DUYỆT hồ sơ của ${shipper.fullName}?`)) {
        return;
      }
    } else {
      const inputReason = window.prompt(
        `Nhập lý do từ chối hồ sơ của ${shipper.fullName}:`,
        'Hồ sơ chưa đáp ứng yêu cầu'
      );
      if (inputReason === null) return; // Người dùng bấm Hủy prompt
      reason = inputReason.trim() || 'Hồ sơ chưa đáp ứng yêu cầu';
    }

    setProcessingId(shipper.shipperId);
    setError('');

    try {
      await adminService.reviewShipper(shipper.shipperId, approved, reason);
      await loadShippers(page);
    } catch {
      setError('Không thể cập nhật trạng thái hồ sơ shipper.');
    } finally {
      setProcessingId(null);
    }
  }

  return (
    <AdminLayout>
      {/* Header */}
      <div className="mb-6">
        <h2 className="font-display text-3xl font-bold">Shipper chờ duyệt</h2>
        <p className="mt-1 text-sm text-muted">
          Kiểm tra thông tin phương tiện và phê duyệt hồ sơ đăng ký tài xế.
        </p>
      </div>

      {/* Thông báo lỗi */}
      {error && (
        <p className="mb-4 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">
          {error}
        </p>
      )}

      {/* Bảng danh sách hồ sơ */}
      <div className="overflow-x-auto rounded-2xl border border-border bg-surface shadow-sm">
        <table className="w-full min-w-[760px] text-left text-sm">
          <thead>
            <tr className="border-b border-border text-xs uppercase tracking-wider text-muted">
              <th className="px-5 py-4">Ứng viên</th>
              <th className="px-5 py-4">Phương tiện</th>
              <th className="px-5 py-4">Biển số</th>
              <th className="px-5 py-4">Ngày đăng ký</th>
              <th className="px-5 py-4 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={5} className="px-5 py-10 text-center text-muted">
                  Đang tải dữ liệu...
                </td>
              </tr>
            ) : result?.data?.length === 0 ? (
              <tr>
                <td colSpan={5} className="px-5 py-10 text-center text-muted">
                  Hiện không có hồ sơ shipper nào đang chờ duyệt.
                </td>
              </tr>
            ) : (
              result?.data.map((shipper) => {
                const isProcessing = processingId === shipper.shipperId;
                return (
                  <tr
                    key={shipper.shipperId}
                    className="border-b border-border transition-colors last:border-0 hover:bg-orange-500/[.02]"
                  >
                    {/* Tên & Thông tin liên hệ */}
                    <td className="px-5 py-4">
                      <p className="font-semibold text-foreground">{shipper.fullName}</p>
                      <p className="mt-0.5 text-xs text-muted">
                        {shipper.email} • {shipper.phone}
                      </p>
                    </td>

                    {/* Loại phương tiện */}
                    <td className="px-5 py-4 font-medium text-slate-700">
                      {shipper.vehicleType}
                    </td>

                    {/* Biển số xe */}
                    <td className="px-5 py-4">
                      <span className="inline-block rounded-md bg-slate-100 px-2.5 py-1 font-mono text-xs font-semibold text-slate-800">
                        {shipper.vehiclePlate}
                      </span>
                    </td>

                    {/* Ngày gửi đơn */}
                    <td className="px-5 py-4 text-xs text-muted">
                      {new Date(shipper.appliedAt).toLocaleDateString('vi-VN')}
                    </td>

                    {/* Nút Phê duyệt / Từ chối */}
                    <td className="px-5 py-4 text-right">
                      <div className="flex justify-end gap-1.5">
                        <button
                          disabled={isProcessing}
                          onClick={() => void handleReview(shipper, true)}
                          title="Duyệt hồ sơ"
                          className="rounded-xl p-2 text-emerald-600 transition hover:bg-emerald-50 disabled:opacity-40"
                        >
                          <Check size={18} />
                        </button>
                        <button
                          disabled={isProcessing}
                          onClick={() => void handleReview(shipper, false)}
                          title="Từ chối hồ sơ"
                          className="rounded-xl p-2 text-red-500 transition hover:bg-red-50 disabled:opacity-40"
                        >
                          <X size={18} />
                        </button>
                      </div>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>

      {/* Phân trang */}
      <div className="mt-4 flex items-center justify-between text-sm text-muted">
        <span>
          Trang <strong className="text-foreground">{page}</strong> / {result?.meta?.totalPages || 1}
        </span>
        <div className="flex gap-2">
          <button
            disabled={page <= 1}
            onClick={() => setPage((p) => p - 1)}
            className="rounded-xl border border-border px-4 py-2 text-xs font-semibold transition hover:bg-black/5 disabled:opacity-40"
          >
            Trước
          </button>
          <button
            disabled={page >= (result?.meta?.totalPages || 1)}
            onClick={() => setPage((p) => p + 1)}
            className="rounded-xl border border-border px-4 py-2 text-xs font-semibold transition hover:bg-black/5 disabled:opacity-40"
          >
            Sau
          </button>
        </div>
      </div>
    </AdminLayout>
  );
}