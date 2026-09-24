import { useEffect, useState } from 'react';
import { Bike, Check, Clock3, MapPin, Navigation, PackageCheck, Phone, Power, Wallet } from 'lucide-react';
import { ShipperLayout } from './ShipperLayout';
import { PageHeader, Panel, StatCard } from '@/components/admin/AdminUI';
import { ShipperOrder, ShipperProfile, ShipperStats, shipperService } from '@/services/shipper.service';

const money = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', maximumFractionDigits: 0 });
const statusName: Record<string, string> = { ReadyForPickup: 'Chờ lấy hàng', Shipping: 'Đang giao', Completed: 'Hoàn tất', Cancelled: 'Đã hủy' };
const statusColor: Record<string, string> = { ReadyForPickup: 'bg-amber-50 text-amber-700', Shipping: 'bg-sky-50 text-sky-700', Completed: 'bg-emerald-50 text-emerald-700', Cancelled: 'bg-red-50 text-red-700' };
type View = 'available' | 'assigned' | 'completed';

export default function ShipperDashboardPage({ view = 'available' }: { view?: View }) {
  const [profile, setProfile] = useState<ShipperProfile | null>(null);
  const [stats, setStats] = useState<ShipperStats | null>(null);
  const [orders, setOrders] = useState<ShipperOrder[]>([]);
  const [activeView, setActiveView] = useState(view);
  const [loading, setLoading] = useState(true);
  const [busy, setBusy] = useState('');
  const [error, setError] = useState('');

  async function load() {
    setLoading(true);
    try {
      const [profileData, statsData] = await Promise.all([shipperService.getProfile(), shipperService.getStats()]);
      setProfile(profileData); setStats(statsData);
      const list = activeView === 'available' ? await shipperService.getAvailableOrders() : activeView === 'assigned' ? await shipperService.getAssignedOrders() : await shipperService.getCompletedOrders();
      setOrders(list);
    } catch { setError('Không thể tải dữ liệu giao hàng.'); } finally { setLoading(false); }
  }
  useEffect(() => { void load(); }, [activeView]);

  async function availability() {
    if (!profile) return; setBusy('availability');
    try { await shipperService.updateAvailability(!profile.isAvailable); await load(); } catch { setError('Không thể cập nhật trạng thái sẵn sàng.'); } finally { setBusy(''); }
  }
  async function orderAction(order: ShipperOrder, action: 'accept' | 'pickup' | 'complete' | 'fail') {
    setBusy(order.id);
    try { if (action === 'accept') await shipperService.acceptOrder(order.id); if (action === 'pickup') await shipperService.confirmPickup(order.id); if (action === 'complete') await shipperService.complete(order.id); if (action === 'fail') await shipperService.fail(order.id, 'Không thể giao đơn'); await load(); } catch { setError('Không thể cập nhật đơn hàng.'); } finally { setBusy(''); }
  }

  return <ShipperLayout>
    <PageHeader eyebrow="Bảng điều hành tài xế" title="Hôm nay giao gì?" description="Nhận đơn, mở đường đi và cập nhật trạng thái ngay trên điện thoại." action={<button onClick={() => void availability()} disabled={!profile || busy === 'availability'} className={`flex items-center gap-2 rounded-xl px-4 py-2.5 text-sm font-bold text-white shadow-sm ${profile?.isAvailable ? 'bg-emerald-600 hover:bg-emerald-700' : 'bg-slate-700 hover:bg-slate-800'}`}><Power size={17} />{profile?.isAvailable ? 'Đang sẵn sàng' : 'Đang tắt'}</button>} />
    {error && <p className="mb-4 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">{error}</p>}
    <div className="mb-6 grid grid-cols-2 gap-3 lg:grid-cols-4"><StatCard label="Đơn đã nhận" value={stats?.totalAssignedOrders ?? '-'} detail="Tổng số đơn được phân công" icon={PackageCheck} tone="blue" /><StatCard label="Đã giao" value={stats?.totalCompletedOrders ?? '-'} detail="Đơn giao thành công" icon={Check} tone="green" /><StatCard label="Thu nhập" value={stats ? money.format(stats.totalEarnings) : '-'} detail="Tổng thu nhập hiện tại" icon={Wallet} tone="orange" /><StatCard label="Đánh giá" value={stats?.averageRating || '-'} detail="Điểm đánh giá trung bình" icon={Bike} tone="purple" /></div>
    <Panel title="Đơn giao hàng" subtitle="Cập nhật theo thời gian thực"><div className="flex gap-1 overflow-x-auto border-b border-slate-200 px-4 pt-3"><Tab active={activeView === 'available'} onClick={() => setActiveView('available')}>Có thể nhận</Tab><Tab active={activeView === 'assigned'} onClick={() => setActiveView('assigned')}>Đang giao</Tab><Tab active={activeView === 'completed'} onClick={() => setActiveView('completed')}>Lịch sử</Tab></div>{loading ? <div className="p-10 text-center text-sm text-slate-500">Đang tải đơn hàng...</div> : orders.length === 0 ? <div className="p-12 text-center text-sm text-slate-500">Chưa có đơn hàng trong danh sách này.</div> : <div className="grid gap-3 p-4 lg:grid-cols-2">{orders.map((order) => <OrderCard key={order.id} order={order} busy={busy === order.id} view={activeView} onAction={orderAction} />)}</div>}</Panel>
  </ShipperLayout>;
}
function Tab({ active, onClick, children }: { active: boolean; onClick: () => void; children: string }) { return <button onClick={onClick} className={`whitespace-nowrap border-b-2 px-3 py-3 text-sm font-bold ${active ? 'border-emerald-600 text-emerald-700' : 'border-transparent text-slate-500'}`}>{children}</button>; }
function OrderCard({ order, busy, view, onAction }: { order: ShipperOrder; busy: boolean; view: View; onAction: (order: ShipperOrder, action: 'accept' | 'pickup' | 'complete' | 'fail') => void }) { const next = view === 'available' ? ['accept', 'Nhận đơn'] as const : order.status === 'ReadyForPickup' ? ['pickup', 'Đã lấy hàng'] as const : order.status === 'Shipping' ? ['complete', 'Giao thành công'] as const : null; const mapUrl = `https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(order.shippingAddress || '')}`; return <article className="rounded-2xl border border-slate-200 bg-white p-4 shadow-sm"><div className="flex items-start justify-between gap-3"><div><p className="font-mono text-xs font-bold text-emerald-700">#{order.orderCode}</p><h3 className="mt-1 font-display text-lg font-bold">{order.customerName}</h3><p className="mt-1 flex items-center gap-1 text-xs text-slate-500"><Clock3 size={13} />{new Date(order.createdAt).toLocaleString('vi-VN')}</p></div><span className={`rounded-full px-2 py-1 text-[11px] font-bold ${statusColor[order.status] || 'bg-slate-100 text-slate-600'}`}>{statusName[order.status] || order.status}</span></div><div className="my-4 flex items-center justify-between border-y border-slate-100 py-3"><span className="text-sm text-slate-500">Giá trị đơn</span><strong className="text-base">{money.format(order.totalAmount)}</strong></div><p className="mb-4 flex items-start gap-2 text-xs text-slate-500"><MapPin size={14} className="mt-0.5 shrink-0 text-sky-600" />{order.shippingAddress || 'Chưa có địa chỉ giao hàng'}</p><div className="flex gap-2">{next && <button disabled={busy} onClick={() => onAction(order, next[0])} className="flex flex-1 items-center justify-center gap-2 rounded-xl bg-emerald-600 px-3 py-3 text-xs font-bold text-white hover:bg-emerald-700 disabled:opacity-50">{next[0] === 'accept' ? <Check size={16} /> : <PackageCheck size={16} />}{busy ? 'Đang xử lý...' : next[1]}</button>}{view === 'assigned' && order.status === 'Shipping' && <button disabled={busy} onClick={() => onAction(order, 'fail')} className="rounded-xl border border-red-200 px-3 py-3 text-xs font-bold text-red-600 hover:bg-red-50">Báo lỗi</button>}<a href={mapUrl} target="_blank" rel="noreferrer" title="Mở Google Maps" className="flex items-center justify-center rounded-xl border border-slate-200 px-3 text-sky-700 hover:bg-sky-50"><Navigation size={17} /></a></div>{order.status === 'Shipping' && <a href="tel:" className="mt-3 flex items-center gap-2 text-xs font-bold text-sky-700"><Phone size={14} />Gọi người nhận</a>}</article>; }
