import { useCallback, useEffect, useState } from 'react';
import { Bike, Check, Clock3, MapPin, Navigation, PackageCheck, Power, RefreshCw, Wallet } from 'lucide-react';
import { Link } from 'react-router-dom';
import { ShipperLayout } from './ShipperLayout';
import { PageHeader, Panel, StatCard } from '@/components/admin/AdminUI';
import { ShipperOrder, ShipperProfile, ShipperStats, shipperService } from '@/services/shipper.service';

const money = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 });
const statusName: Record<string, string> = { Confirmed: 'Đã xác nhận', Preparing: 'Đang chuẩn bị', ReadyForPickup: 'Món đã sẵn sàng', Shipping: 'Đang giao', Completed: 'Hoàn tất', Cancelled: 'Đã hủy' };
const statusColor: Record<string, string> = { Confirmed: 'bg-indigo-50 text-indigo-700', Preparing: 'bg-sky-50 text-sky-700', ReadyForPickup: 'bg-amber-50 text-amber-700', Shipping: 'bg-sky-50 text-sky-700', Completed: 'bg-emerald-50 text-emerald-700', Cancelled: 'bg-red-50 text-red-700' };
type View = 'available' | 'assigned' | 'completed';

export default function ShipperDashboardPage({ view = 'available' }: { view?: View }) {
  const [profile, setProfile] = useState<ShipperProfile | null>(null);
  const [stats, setStats] = useState<ShipperStats | null>(null);
  const [orders, setOrders] = useState<ShipperOrder[]>([]);
  const [hasActiveOrder, setHasActiveOrder] = useState(false);
  const [activeView, setActiveView] = useState(view);
  const [loading, setLoading] = useState(true);
  const [busy, setBusy] = useState('');
  const [error, setError] = useState('');
  const [lastUpdated, setLastUpdated] = useState<Date | null>(null);
  const isApproved = profile?.status === 'Approved' || profile?.status === 1;
  const canGoOnline = Boolean(profile?.isActive && isApproved);

  const load = useCallback(async (showLoading = true) => {
    if (showLoading) setLoading(true);
    setError('');
    try {
      const [profileData, statsData, assignedOrders] = await Promise.all([
        shipperService.getProfile(),
        shipperService.getStats(),
        shipperService.getAssignedOrders(),
      ]);
      setProfile(profileData); setStats(statsData);
      setHasActiveOrder(assignedOrders.length > 0);
      const list = activeView === 'available'
        ? await shipperService.getAvailableOrders()
        : activeView === 'assigned'
          ? assignedOrders
          : await shipperService.getCompletedOrders();
      setOrders(list);
      setLastUpdated(new Date());
    } catch { setError('Không thể tải dữ liệu giao hàng. Vui lòng thử tải lại.'); } finally { if (showLoading) setLoading(false); }
  }, [activeView]);
  useEffect(() => {
    void load();
    const refreshInterval = window.setInterval(() => void load(false), 15000);
    return () => window.clearInterval(refreshInterval);
  }, [load]);

  async function availability() {
    if (!profile) return; setBusy('availability');
    try { await shipperService.updateAvailability(!profile.isAvailable); await load(); } catch { setError('Không thể cập nhật trạng thái sẵn sàng.'); } finally { setBusy(''); }
  }
  async function orderAction(order: ShipperOrder, action: 'accept' | 'pickup' | 'complete' | 'fail' | 'release') {
    setBusy(order.id);
    try { if (action === 'accept') await shipperService.acceptOrder(order.id); if (action === 'pickup') await shipperService.confirmPickup(order.id); if (action === 'complete') await shipperService.complete(order.id); if (action === 'fail') await shipperService.fail(order.id, 'Không thể giao đơn'); if (action === 'release') await shipperService.releaseOrder(order.id, 'Shipper không thể tiếp tục nhận đơn trước khi lấy hàng'); await load(); } catch { setError('Không thể cập nhật đơn hàng.'); } finally { setBusy(''); }
  }

  return <ShipperLayout>
    <PageHeader
      eyebrow="Bảng điều hành tài xế"
      title="Hôm nay giao gì?"
      description="Theo dõi đơn, tiền công và cập nhật từng bước giao hàng."
      action={
        <div className="flex flex-wrap gap-2">
          <Link
            to="/shipper/profile"
            className="flex items-center gap-2 rounded-xl border border-slate-200 bg-white px-4 py-2.5 text-sm font-bold text-slate-700 hover:bg-slate-50"
          >
            Thiết lập hồ sơ
          </Link>
          <button
            onClick={() => void load()}
            disabled={loading}
            className="flex items-center gap-2 rounded-xl border border-slate-200 bg-white px-4 py-2.5 text-sm font-bold text-slate-700 hover:bg-slate-50 disabled:opacity-50"
          >
            <RefreshCw size={16} className={loading ? 'animate-spin' : ''} />
            Tải lại
          </button>
          <button
            onClick={() => void availability()}
            disabled={!canGoOnline || busy === 'availability'}
            aria-pressed={profile?.isAvailable ?? false}
            className={`flex items-center gap-2 rounded-xl px-4 py-2.5 text-sm font-bold text-white shadow-sm ${profile?.isAvailable ? 'bg-emerald-600 hover:bg-emerald-700' : 'bg-slate-700 hover:bg-slate-800'}`}
          >
            <Power size={17} />
            {profile?.isAvailable ? 'Đang nhận đơn' : 'Bật nhận đơn'}
          </button>
        </div>
      }
    />
    {error && (
      <div role="alert" className="mb-4 flex flex-wrap items-center justify-between gap-3 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
        <span>{error}</span>
        <button onClick={() => void load()} className="font-bold underline">Thử lại</button>
      </div>
    )}
    {!loading && profile && !canGoOnline && (
      <p role="status" className="mb-4 rounded-xl border border-amber-200 bg-amber-50 px-4 py-3 text-sm text-amber-900">
        {!isApproved
          ? 'Hồ sơ shipper chưa được duyệt nên chưa thể nhận đơn.'
          : 'Tài khoản shipper đang tạm dừng. Vui lòng liên hệ quản trị viên để mở lại.'}
      </p>
    )}
    <div className="mb-6 grid grid-cols-2 gap-3 lg:grid-cols-4">
      <StatCard label="Đơn đã nhận" value={stats?.totalAssignedOrders ?? '-'} detail="Tổng số đơn được phân công" icon={PackageCheck} tone="blue" />
      <StatCard label="Đã giao" value={stats?.totalCompletedOrders ?? '-'} detail="Đơn giao thành công" icon={Check} tone="green" />
      <StatCard label="Thu nhập" value={stats ? money.format(stats.totalEarnings) : '-'} detail="Tiền công từ đơn hoàn tất" icon={Wallet} tone="orange" />
      <StatCard label="Đánh giá" value={stats?.averageRating || '-'} detail="Điểm đánh giá trung bình" icon={Bike} tone="purple" />
    </div>
    <Panel
      title={activeView === 'available' ? 'Đơn đang chờ shipper' : activeView === 'assigned' ? 'Đơn của tôi' : 'Lịch sử giao hàng'}
      subtitle={lastUpdated ? `Tự cập nhật mỗi 15 giây · Lần cuối ${lastUpdated.toLocaleTimeString('vi-VN')}` : 'Đang đồng bộ đơn hàng'}
    >
      <div className="flex gap-1 overflow-x-auto border-b border-slate-200 px-4 pt-3">
        <Tab active={activeView === 'available'} onClick={() => setActiveView('available')}>Đơn chờ nhận</Tab>
        <Tab active={activeView === 'assigned'} onClick={() => setActiveView('assigned')}>Đơn của tôi</Tab>
        <Tab active={activeView === 'completed'} onClick={() => setActiveView('completed')}>Lịch sử</Tab>
      </div>
      {canGoOnline && !profile?.isAvailable && activeView === 'available' && (
        <div className="mx-4 mt-4 rounded-xl border border-amber-200 bg-amber-50 px-4 py-3 text-sm text-amber-900">
          Bạn đang tắt nhận đơn. Có thể xem đơn đang chờ, nhưng hãy bật <strong>Nhận đơn</strong> để nhận giao.
        </div>
      )}
      {hasActiveOrder && activeView === 'available' && (
        <div role="status" className="mx-4 mt-4 rounded-xl border border-sky-200 bg-sky-50 px-4 py-3 text-sm text-sky-900">
          Bạn đang có đơn cần xử lý. Hoàn tất hoặc trả đơn hiện tại trước khi nhận đơn mới; hệ thống sẽ tạm ngừng gửi thông báo đơn mới cho bạn.
        </div>
      )}
      {loading ? (
        <div className="p-10 text-center text-sm text-slate-500">Đang tải đơn hàng...</div>
      ) : orders.length === 0 ? (
        <div className="p-10 text-center">
          <PackageCheck className="mx-auto mb-3 text-slate-300" size={32} />
          <p className="font-semibold text-slate-700">
            {activeView === 'available'
              ? hasActiveOrder ? 'Bạn đã có đơn đang xử lý' : 'Chưa có đơn chờ shipper nhận'
              : activeView === 'assigned' ? 'Bạn chưa có đơn được phân công' : 'Chưa có đơn hoàn tất'}
          </p>
          <p className="mt-1 text-sm text-slate-500">
            {activeView === 'available'
              ? hasActiveOrder
                ? 'Danh sách nhận đơn sẽ được mở lại sau khi bạn hoàn tất hoặc trả đơn đang xử lý.'
                : 'Đơn sẽ xuất hiện ngay khi nhà hàng xác nhận, kể cả khi món còn đang chuẩn bị.'
              : 'Danh sách sẽ tự cập nhật khi có thay đổi.'}
          </p>
        </div>
      ) : (
        <div className="grid gap-3 p-4 lg:grid-cols-2">
          {orders.map((order) => (
            <OrderCard
              key={order.id}
              order={order}
              busy={busy === order.id}
              view={activeView}
              canAccept={Boolean(profile?.isAvailable && !hasActiveOrder)}
              onAction={orderAction}
            />
          ))}
        </div>
      )}
    </Panel>
  </ShipperLayout>;
}
function Tab({ active, onClick, children }: { active: boolean; onClick: () => void; children: string }) { return <button onClick={onClick} className={`whitespace-nowrap border-b-2 px-3 py-3 text-sm font-bold ${active ? 'border-emerald-600 text-emerald-700' : 'border-transparent text-slate-500'}`}>{children}</button>; }
function OrderCard({ order, busy, view, canAccept, onAction }: { order: ShipperOrder; busy: boolean; view: View; canAccept: boolean; onAction: (order: ShipperOrder, action: 'accept' | 'pickup' | 'complete' | 'fail' | 'release') => void }) {
  const next = view === 'available'
    ? ['accept', 'Nhận đơn'] as const
    : order.status === 'ReadyForPickup'
      ? ['pickup', 'Đã lấy hàng'] as const
      : order.status === 'Shipping'
        ? ['complete', 'Giao thành công'] as const
        : null;
  const mapUrl = `https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(order.shippingAddress || '')}`;

  return (
    <article className="rounded-2xl border border-slate-200 bg-white p-4 shadow-sm">
      <div className="flex items-start justify-between gap-3">
        <div>
          <p className="font-mono text-xs font-bold text-emerald-700">#{order.orderCode}</p>
          <h3 className="mt-1 font-display text-lg font-bold">{order.customerName}</h3>
          <p className="mt-1 flex items-center gap-1 text-xs text-slate-500">
            <Clock3 size={13} />{new Date(order.createdAt).toLocaleString('vi-VN')}
          </p>
        </div>
        <span className={`rounded-full px-2 py-1 text-[11px] font-bold ${statusColor[order.status] || 'bg-slate-100 text-slate-600'}`}>
          {statusName[order.status] || order.status}
        </span>
      </div>
      <div className="my-4 divide-y divide-slate-100 border-y border-slate-100">
        <div className="flex items-center justify-between py-3">
          <span className="text-sm text-slate-500">Giá trị đơn</span>
          <strong className="text-base">{money.format(order.totalAmount)}</strong>
        </div>
        <div className="flex items-center justify-between py-3">
          <span className="text-sm text-slate-500">
            {view === 'completed' ? 'Tiền công đã nhận' : 'Tiền công dự kiến'}
          </span>
          <strong className="text-base text-emerald-700">{money.format(order.shippingFee)}</strong>
        </div>
      </div>
      <p className="mb-4 flex items-start gap-2 text-xs text-slate-500">
        <MapPin size={14} className="mt-0.5 shrink-0 text-sky-600" />
        {order.shippingAddress || 'Chưa có địa chỉ giao hàng'}
      </p>
      {order.prepMinutes != null && (
        <p className="mb-4 flex items-center gap-2 text-xs font-semibold text-slate-600">
          <Clock3 size={14} className="text-amber-600" />
          Thời gian chuẩn bị dự kiến: {order.prepMinutes} phút
        </p>
      )}
      <div className="flex gap-2">
        {next && (
          <button disabled={busy || (next[0] === 'accept' && !canAccept)} onClick={() => onAction(order, next[0])} className="flex flex-1 items-center justify-center gap-2 rounded-xl bg-emerald-600 px-3 py-3 text-xs font-bold text-white hover:bg-emerald-700 disabled:opacity-50">
            {next[0] === 'accept' ? <Check size={16} /> : <PackageCheck size={16} />}
            {busy ? 'Đang xử lý...' : next[1]}
          </button>
        )}
        {view === 'assigned' && order.status === 'Shipping' && (
          <button disabled={busy} onClick={() => onAction(order, 'fail')} className="rounded-xl border border-red-200 px-3 py-3 text-xs font-bold text-red-600 hover:bg-red-50">
            Báo lỗi
          </button>
        )}
        {view === 'assigned' && ['Confirmed', 'Preparing', 'ReadyForPickup'].includes(order.status) && (
          <button disabled={busy} onClick={() => onAction(order, 'release')} className="rounded-xl border border-red-200 px-3 py-3 text-xs font-bold text-red-600 hover:bg-red-50">
            Trả đơn
          </button>
        )}
        <a href={mapUrl} target="_blank" rel="noreferrer" title="Mở Google Maps" className="flex items-center justify-center rounded-xl border border-slate-200 px-3 text-sky-700 hover:bg-sky-50">
          <Navigation size={17} />
        </a>
      </div>
      {order.status === 'Shipping' && (
        <span className="mt-3 block text-xs font-bold text-sky-700">
          Đang giao đơn hàng
        </span>
      )}
      {view === 'assigned' && ['Confirmed', 'Preparing'].includes(order.status) && (
        <span className="mt-3 block text-xs font-bold text-sky-700">
          Đang chờ nhà hàng chuẩn bị món
        </span>
      )}
    </article>
  );
}
