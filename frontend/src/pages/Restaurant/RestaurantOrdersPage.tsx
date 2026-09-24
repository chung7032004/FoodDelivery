import { useEffect, useState } from 'react';
import { AlertTriangle, Check, ChefHat, Clock3, PackageCheck, Phone } from 'lucide-react';
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

const money = new Intl.NumberFormat('vi-VN', {
  style: 'currency',
  currency: 'VND',
  maximumFractionDigits: 0,
});

type Tab = 'active' | 'completed';

export default function RestaurantOrdersPage() {
  const [orders, setOrders] = useState<RestaurantOrder[]>([]);
  const [tab, setTab] = useState<Tab>('active');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [busy, setBusy] = useState('');

  // Tải danh sách đơn hàng
  const loadOrders = async (isMounted = true) => {
    setLoading(true);
    setError('');
    try {
      const result =
        tab === 'active'
          ? await staffService.getOrders()
          : await staffService.getCompletedOrders();
      if (isMounted) setOrders(result.data || []);
    } catch {
      if (isMounted) setError('Không thể tải danh sách đơn hàng.');
    } finally {
      if (isMounted) setLoading(false);
    }
  };

  useEffect(() => {
    let isSubscribed = true;
    void loadOrders(isSubscribed);
    return () => {
      isSubscribed = false;
    };
  }, [tab]);

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
          Đang xử lý
        </button>
        <button
          onClick={() => setTab('completed')}
          className={`border-b-2 px-4 py-3 text-sm font-bold transition ${
            tab === 'completed'
              ? 'border-sky-600 text-sky-700'
              : 'border-transparent text-slate-500 hover:text-slate-700'
          }`}
        >
          Lịch sử 30 ngày
        </button>
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
      ) : orders.length === 0 ? (
        <Panel>
          <div className="p-12 text-center text-slate-500">
            Chưa có đơn hàng nào trong danh sách này.
          </div>
        </Panel>
      ) : (
        <div className="grid gap-5 xl:grid-cols-2">
          {orders.map((order) => (
            <OrderCard
              key={order.id}
              order={order}
              busy={busy === order.id}
              onAction={handleAction}
            />
          ))}
        </div>
      )}
    </RestaurantLayout>
  );
}

/* Thẻ thông tin Đơn hàng */
function OrderCard({
  order,
  busy,
  onAction,
}: {
  order: RestaurantOrder;
  busy: boolean;
  onAction: (
    order: RestaurantOrder,
    next: 'confirm' | 'prepare' | 'ready'
  ) => void;
}) {
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

        {/* Báo sự cố */}
        {order.status <= 5 && (
          <button className="mt-4 flex items-center gap-1.5 text-xs font-semibold text-red-600 transition hover:text-red-700">
            <AlertTriangle size={14} />
            Báo sự cố đơn hàng
          </button>
        )}
      </div>
    </article>
  );
}