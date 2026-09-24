import { useEffect, useState } from 'react';
import { Search } from 'lucide-react';
import { AdminLayout } from './AdminLayout';
import { AdminOrder, adminService, PagedResult } from '@/services/admin.service';

const money = new Intl.NumberFormat('vi-VN', {
  style: 'currency',
  currency: 'VND',
  maximumFractionDigits: 0,
});

export default function AdminOrdersPage() {
  const [result, setResult] = useState<PagedResult<AdminOrder> | null>(null);
  const [code, setCode] = useState('');
  const [status, setStatus] = useState('');
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  // Tải danh sách đơn hàng
  useEffect(() => {
    let isSubscribed = true;

    async function fetchOrders() {
      setLoading(true);
      setError('');
      try {
        const data = await adminService.getOrders({
          searchCode: code.trim() || undefined,
          status: status || undefined,
          page,
          pageSize: 10,
        });
        if (isSubscribed) setResult(data);
      } catch (err) {
        if (isSubscribed) setError('Không thể tải danh sách đơn hàng.');
      } finally {
        if (isSubscribed) setLoading(false);
      }
    }

    void fetchOrders();

    return () => {
      isSubscribed = false;
    };
  }, [code, status, page]);

  return (
    <AdminLayout>
      {/* Header */}
      <div className="mb-6">
        <h2 className="font-display text-3xl font-bold">Đơn hàng</h2>
        <p className="mt-1 text-sm text-muted">Theo dõi đơn hàng trên toàn hệ thống.</p>
      </div>

      {/* Thanh bộ lọc */}
      <div className="mb-4 flex flex-wrap gap-3">
        <label className="flex items-center gap-2 rounded-xl border border-border bg-surface px-3.5 py-2 text-sm text-muted shadow-sm transition-focus focus-within:border-orange-500">
          <Search size={17} />
          <input
            value={code}
            onChange={(e) => {
              setCode(e.target.value);
              setPage(1);
            }}
            placeholder="Mã đơn hàng..."
            className="bg-transparent text-foreground outline-none placeholder:text-muted/60"
          />
        </label>

        <select
          value={status}
          onChange={(e) => {
            setStatus(e.target.value);
            setPage(1);
          }}
          className="rounded-xl border border-border bg-surface px-3.5 py-2 text-sm text-foreground shadow-sm outline-none transition focus:border-orange-500"
        >
          <option value="">Tất cả trạng thái</option>
          <option value="Pending">Chờ xác nhận</option>
          <option value="Preparing">Đang chuẩn bị</option>
          <option value="Delivering">Đang giao</option>
          <option value="Completed">Hoàn thành</option>
          <option value="Cancelled">Đã hủy</option>
        </select>
      </div>

      {/* Thông báo lỗi */}
      {error && (
        <p className="mb-4 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">
          {error}
        </p>
      )}

      {/* Bảng danh sách đơn hàng */}
      <div className="overflow-x-auto rounded-2xl border border-border bg-surface shadow-sm">
        <table className="w-full min-w-[720px] text-left text-sm">
          <thead>
            <tr className="border-b border-border text-xs uppercase tracking-wider text-muted">
              <th className="px-5 py-4">Mã đơn</th>
              <th className="px-5 py-4">Khách hàng</th>
              <th className="px-5 py-4">Tổng tiền</th>
              <th className="px-5 py-4">Trạng thái</th>
              <th className="px-5 py-4">Thời gian</th>
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
                  Không tìm thấy đơn hàng nào.
                </td>
              </tr>
            ) : (
              result?.data.map((order) => (
                <tr
                  key={order.id}
                  className="border-b border-border transition-colors last:border-0 hover:bg-orange-500/[.02]"
                >
                  <td className="px-5 py-4 font-mono text-xs font-semibold text-orange-600">
                    #{order.orderCode}
                  </td>
                  <td className="px-5 py-4 font-semibold">{order.customerName}</td>
                  <td className="px-5 py-4 font-medium">{money.format(order.totalAmount)}</td>
                  <td className="px-5 py-4">
                    <OrderStatusBadge status={String(order.status)} />
                  </td>
                  <td className="px-5 py-4 text-xs text-muted">
                    {new Date(order.createdAt).toLocaleString('vi-VN')}
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Phân trang */}
      <div className="flex items-center justify-between py-4 text-sm text-muted">
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

/* Badge hiển thị màu sắc theo trạng thái đơn hàng */
function OrderStatusBadge({ status }: { status: string }) {
  const configs: Record<string, { label: string; className: string }> = {
    Pending: { label: 'Chờ xác nhận', className: 'bg-amber-500/10 text-amber-700' },
    Preparing: { label: 'Đang chuẩn bị', className: 'bg-blue-500/10 text-blue-700' },
    Delivering: { label: 'Đang giao', className: 'bg-purple-500/10 text-purple-700' },
    Completed: { label: 'Hoàn thành', className: 'bg-emerald-500/10 text-emerald-700' },
    Cancelled: { label: 'Đã hủy', className: 'bg-red-500/10 text-red-700' },
  };

  const config = configs[status] || {
    label: status,
    className: 'bg-gray-500/10 text-gray-700',
  };

  return (
    <span className={`inline-block rounded-full px-3 py-1 text-xs font-bold ${config.className}`}>
      {config.label}
    </span>
  );
}