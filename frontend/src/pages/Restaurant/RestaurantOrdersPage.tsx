import { useCallback, useEffect, useState } from 'react';
import { AlertTriangle, Check, ChefHat, Clock3, PackageCheck, Phone, RefreshCw } from 'lucide-react';
import { RestaurantLayout } from './RestaurantLayout';
import { PageHeader, Panel } from '@/components/admin/AdminUI';
import { RestaurantOrder, staffService } from '@/services/staff.service';

const statusLabels: Record<number, string> = {
  1: 'Đơn mới',
  2: 'Chờ khách phản hồi',
  3: 'Đã xác nhận',
  4: 'Đang chuẩn bị',
  5: 'Sẵn sàng giao',
  6: 'Đang giao',
  7: 'Hoàn tất',
  8: 'Đã hủy',
};

const statusStyles: Record<number, string> = {
  1: 'bg-amber-50 text-amber-700 border border-amber-200/60',
  2: 'bg-violet-50 text-violet-700 border border-violet-200/60',
  3: 'bg-sky-50 text-sky-700 border border-sky-200/60',
  4: 'bg-orange-50 text-orange-700 border border-orange-200/60',
  5: 'bg-emerald-50 text-emerald-700 border border-emerald-200/60',
  6: 'bg-blue-50 text-blue-700 border border-blue-200/60',
  7: 'bg-slate-100 text-slate-700',
  8: 'bg-red-50 text-red-700 border border-red-200/60',
};
const activeStatuses = [1, 2, 3, 4, 5, 6];
const completedStatuses = [7, 8];

const money = new Intl.NumberFormat('vi-VN', {
  style: 'currency',
  currency: 'VND',
  maximumFractionDigits: 0,
});

type Tab = 'active' | 'completed';

export default function RestaurantOrdersPage() {
  const [orders, setOrders] = useState<RestaurantOrder[]>([]);
  const [tab, setTab] = useState<Tab>('active');
  const [statusFilter, setStatusFilter] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [busy, setBusy] = useState('');

  // Tải danh sách đơn hàng
  const loadOrders = useCallback(async (isMounted: () => boolean = () => true) => {
    setLoading(true);
    setError('');
    try {
      const result =
        tab === 'active'
          ? await staffService.getOrders()
          : await staffService.getCompletedOrders();
      if (isMounted()) setOrders(result.data || []);
    } catch {
      if (isMounted()) setError('Không thể tải danh sách đơn hàng.');
    } finally {
      if (isMounted()) setLoading(false);
    }
  }, [tab]);

  useEffect(() => {
    let isSubscribed = true;
    void loadOrders(() => isSubscribed);
    return () => {
      isSubscribed = false;
    };
  }, [loadOrders]);

  useEffect(() => setStatusFilter(null), [tab]);

  // Xử lý chuyển trạng thái đơn hàng
  async function handleAction(
    order: RestaurantOrder,
    next: 'confirm' | 'prepare' | 'ready'
  ) {
    setBusy(order.id);
    setError('');
    try {
      if (next === 'confirm') await staffService.confirm(order.id);
      if (next === 'prepare') await staffService.startPreparing(order.id);
      if (next === 'ready') await staffService.markReady(order.id);
      await loadOrders();
    } catch {
      setError('Không thể cập nhật đơn hàng.');
    } finally {
      setBusy('');
    }
  }

  async function handleOutOfStock(order: RestaurantOrder, removedProductIds: string[], note: string) {
    setBusy(order.id);
    setError('');
    try {
      await staffService.reportOutOfStock(order.id, removedProductIds, note);
      await loadOrders();
    } catch {
      setError('Không thể báo món hết hàng. Vui lòng kiểm tra lại đơn và thử lại.');
    } finally {
      setBusy('');
    }
  }

  const visibleStatuses = tab === 'active' ? activeStatuses : completedStatuses;
  const visibleOrders = orders.filter((order) =>
    visibleStatuses.includes(order.status) && (statusFilter === null || order.status === statusFilter)
  );

  return (
    <RestaurantLayout>
      <PageHeader
        eyebrow="Vận hành nhà hàng"
        title="Quản lý đơn hàng"
        description="Xử lý đơn theo từng bước từ xác nhận đến bàn giao cho shipper."
      />

      {/* Điều hướng Tab */}
      <div className="mb-6 flex gap-2 border-b border-slate-200">
        <button
          onClick={() => setTab('active')}
          className={`border-b-2 px-4 py-3 text-sm font-bold transition ${
            tab === 'active'
              ? 'border-sky-600 text-sky-700'
              : 'border-transparent text-slate-500 hover:text-slate-700'
          }`}
        >
          Đang xử lý ({orders.filter((order) => activeStatuses.includes(order.status)).length})
        </button>
        <button
          onClick={() => setTab('completed')}
          className={`border-b-2 px-4 py-3 text-sm font-bold transition ${
            tab === 'completed'
              ? 'border-sky-600 text-sky-700'
              : 'border-transparent text-slate-500 hover:text-slate-700'
          }`}
        >
          Đã hoàn tất / hủy ({orders.filter((order) => completedStatuses.includes(order.status)).length})
        </button>
        <button
          type="button"
          onClick={() => void loadOrders()}
          disabled={loading}
          aria-label="Tải lại danh sách đơn"
          className="ml-auto self-center rounded-lg border border-slate-200 p-2 text-slate-600 hover:bg-slate-50 disabled:opacity-50"
        >
          <RefreshCw size={16} className={loading ? 'animate-spin' : ''} />
        </button>
      </div>

      <div className="mb-5 flex flex-wrap gap-2">
        <StatusFilter label="Tất cả" active={statusFilter === null} onClick={() => setStatusFilter(null)} />
        {visibleStatuses.map((status) => (
          <StatusFilter
            key={status}
            label={statusLabels[status]}
            count={orders.filter((order) => order.status === status).length}
            active={statusFilter === status}
            onClick={() => setStatusFilter(status)}
          />
        ))}
      </div>

      {error && (
        <p className="mb-5 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600 shadow-sm">
          {error}
        </p>
      )}

      {/* Hiển thị danh sách */}
      {loading ? (
        <Panel>
          <div className="p-10 text-center text-sm text-slate-500">
            Đang tải danh sách đơn hàng...
          </div>
        </Panel>
      ) : visibleOrders.length === 0 ? (
        <Panel>
          <div className="p-12 text-center text-slate-500">
            Chưa có đơn hàng nào trong danh sách này.
          </div>
        </Panel>
      ) : (
        <div className="grid gap-5 xl:grid-cols-2">
          {visibleOrders.map((order) => (
            <OrderCard
              key={order.id}
              order={order}
              busy={busy === order.id}
              onAction={handleAction}
              onReportOutOfStock={handleOutOfStock}
            />
          ))}
        </div>
      )}
    </RestaurantLayout>
  );
}

function StatusFilter({
  label,
  count,
  active,
  onClick,
}: {
  label: string;
  count?: number;
  active: boolean;
  onClick: () => void;
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={`rounded-full border px-3 py-1.5 text-xs font-semibold transition ${
        active
          ? 'border-sky-600 bg-sky-600 text-white'
          : 'border-slate-200 bg-white text-slate-600 hover:border-sky-300'
      }`}
    >
      {label}{count === undefined ? '' : ` · ${count}`}
    </button>
  );
}

/* Thẻ thông tin Đơn hàng */
function OrderCard({
  order,
  busy,
  onAction,
  onReportOutOfStock,
}: {
  order: RestaurantOrder;
  busy: boolean;
  onAction: (
    order: RestaurantOrder,
    next: 'confirm' | 'prepare' | 'ready'
  ) => void;
  onReportOutOfStock: (order: RestaurantOrder, removedProductIds: string[], note: string) => void;
}) {
  const [reportOpen, setReportOpen] = useState(false);
  const [removedProductIds, setRemovedProductIds] = useState<string[]>([]);
  const [reportNote, setReportNote] = useState('');
  // Xác định nút hành động tiếp theo
  const getActionConfig = () => {
    if (order.status === 1) {
      return { type: 'confirm' as const, label: 'Xác nhận đơn', Icon: Check };
    }
    if (order.status === 3) {
      return { type: 'prepare' as const, label: 'Bắt đầu chế biến', Icon: ChefHat };
    }
    if (order.status === 4) {
      return { type: 'ready' as const, label: 'Đánh dấu sẵn sàng', Icon: PackageCheck };
    }
    return null;
  };

  const actionConfig = getActionConfig();

  return (
    <article className="flex flex-col justify-between rounded-2xl border border-slate-200 bg-white p-5 shadow-sm transition hover:-translate-y-0.5 hover:shadow-md">
      <div>
        {/* Tiêu đề card */}
        <div className="flex items-start justify-between gap-4">
          <div>
            <p className="font-mono text-xs font-bold text-sky-700">
              #{order.orderNumber}
            </p>
            <h3 className="mt-1 font-display text-lg font-bold text-slate-900">
              {order.customerName}
            </h3>
            <p className="mt-1 flex items-center gap-2 text-xs text-slate-500">
              <span className="flex items-center gap-1">
                <Phone size={13} />
                {order.customerPhone}
              </span>
              <span>·</span>
              <span className="flex items-center gap-1">
                <Clock3 size={13} />
                {new Date(order.createdAt).toLocaleTimeString('vi-VN', {
                  hour: '2-digit',
                  minute: '2-digit',
                })}
              </span>
            </p>
          </div>

          <span
            className={`rounded-full px-3 py-1 text-xs font-bold ${
              statusStyles[order.status] || 'bg-slate-100 text-slate-600'
            }`}
          >
            {statusLabels[order.status] || `Trạng thái ${order.status}`}
          </span>
        </div>

        <OrderProgress status={order.status} />

        {/* Danh sách món ăn */}
        <div className="my-4 space-y-2 border-y border-slate-100 py-3">
          {order.items.map((item, index) => (
            <div
              key={`${item.productName}-${index}`}
              className="flex justify-between gap-3 text-sm"
            >
              <span className="text-slate-800">
                <strong className="text-orange-600">{item.quantity}x</strong>{' '}
                {item.productName}
              </span>
              <span className="shrink-0 font-medium text-slate-500">
                {money.format(item.price)}
              </span>
            </div>
          ))}
        </div>
      </div>

      {/* Tổng tiền & Nút Thao tác */}
      <div>
        <div className="flex items-center justify-between gap-3 pt-2">
          <span className="text-sm font-bold text-slate-900">
            Tổng món: <span className="text-base text-orange-600">{money.format(order.totalAmount)}</span>
          </span>

          {actionConfig && (
            <button
              disabled={busy}
              onClick={() => onAction(order, actionConfig.type)}
              className="flex items-center gap-2 rounded-xl bg-sky-600 px-3.5 py-2 text-xs font-bold text-white shadow-sm transition hover:bg-sky-700 disabled:opacity-50"
            >
              <actionConfig.Icon size={15} />
              {busy ? 'Đang xử lý...' : actionConfig.label}
            </button>
          )}
        </div>

        {order.status === 1 && (
          <div className="mt-4">
            <button
              type="button"
              onClick={() => setReportOpen((open) => !open)}
              className="flex items-center gap-1.5 text-xs font-semibold text-red-600 transition hover:text-red-700"
            >
              <AlertTriangle size={14} />
              Báo món hết hàng
            </button>
            {reportOpen && (
              <form
                className="mt-3 space-y-3 rounded-xl border border-red-100 bg-red-50/60 p-4"
                onSubmit={(event) => {
                  event.preventDefault();
                  onReportOutOfStock(order, removedProductIds, reportNote);
                  setReportOpen(false);
                }}
              >
                <p className="text-xs font-semibold text-slate-700">Chọn món không thể chuẩn bị:</p>
                {order.items.map((item) => (
                  <label key={item.productId} className="flex items-center gap-2 text-sm text-slate-700">
                    <input
                      type="checkbox"
                      checked={removedProductIds.includes(item.productId)}
                      onChange={(event) => setRemovedProductIds((selected) =>
                        event.target.checked
                          ? [...selected, item.productId]
                          : selected.filter((id) => id !== item.productId)
                      )}
                    />
                    {item.quantity}x {item.productName}
                  </label>
                ))}
                <textarea
                  value={reportNote}
                  onChange={(event) => setReportNote(event.target.value)}
                  maxLength={500}
                  rows={2}
                  placeholder="Ghi chú cho khách (không bắt buộc)"
                  className="w-full rounded-lg border border-slate-200 bg-white px-3 py-2 text-xs outline-none focus:border-sky-400"
                />
                <button
                  type="submit"
                  disabled={busy || removedProductIds.length === 0}
                  className="rounded-lg bg-red-600 px-3 py-2 text-xs font-bold text-white disabled:opacity-50"
                >
                  {busy ? 'Đang gửi...' : 'Gửi đề xuất cho khách'}
                </button>
              </form>
            )}
          </div>
        )}
      </div>
    </article>
  );
}

function OrderProgress({ status }: { status: number }) {
  if (status === 2 || status === 8) {
    return (
      <div className={`my-4 rounded-lg px-3 py-2 text-xs font-medium ${
        status === 2 ? 'bg-violet-50 text-violet-700' : 'bg-red-50 text-red-700'
      }`}>
        {status === 2
          ? 'Đơn đang chờ khách xác nhận thay đổi món.'
          : 'Đơn hàng đã kết thúc ở trạng thái hủy.'}
      </div>
    );
  }

  const steps = [
    { status: 1, label: 'Mới' },
    { status: 3, label: 'Đã xác nhận' },
    { status: 4, label: 'Đang làm' },
    { status: 5, label: 'Chờ lấy' },
    { status: 6, label: 'Đang giao' },
    { status: 7, label: 'Hoàn tất' },
  ];
  const activeIndex = steps.findIndex((step) => step.status === status);

  return (
    <ol aria-label="Tiến trình đơn hàng" className="my-5 grid grid-cols-6 gap-1">
      {steps.map((step, index) => (
        <li key={step.status} className="min-w-0 text-center">
          <span className={`mx-auto block h-2 rounded-full ${
            index <= activeIndex ? 'bg-sky-500' : 'bg-slate-200'
          }`} />
          <span className={`mt-1 block truncate text-[10px] ${
            index <= activeIndex ? 'font-semibold text-sky-700' : 'text-slate-400'
          }`}>
            {step.label}
          </span>
        </li>
      ))}
    </ol>
  );
}