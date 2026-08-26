import { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import {
  LayoutDashboard,
  ShoppingBag,
  Utensils,
  Users,
  Truck,
  Tag,
  Settings,
  Bell,
  Search,
  TrendingUp,
  TrendingDown,
  Clock,
  CheckCircle2,
  XCircle,
  ChevronRight,
} from 'lucide-react';

// ---------------------------------------------------------------------------
// Mock data — swap for real API calls (TanStack Query) when wiring up
// ---------------------------------------------------------------------------

const STATS = [
  { label: 'Doanh thu hôm nay', value: '12.480.000₫', delta: '+8.2%', trend: 'up' as const, icon: TrendingUp },
  { label: 'Đơn hàng mới', value: '186', delta: '+12.4%', trend: 'up' as const, icon: ShoppingBag },
  { label: 'Nhà hàng hoạt động', value: '54', delta: '-1.8%', trend: 'down' as const, icon: Utensils },
  { label: 'Khách hàng mới', value: '39', delta: '+4.1%', trend: 'up' as const, icon: Users },
];

type OrderStatus = 'pending' | 'preparing' | 'delivering' | 'completed' | 'cancelled';

const STATUS_MAP: Record<OrderStatus, { label: string; className: string }> = {
  pending: { label: 'Chờ xác nhận', className: 'text-amber-400 bg-amber-400/10' },
  preparing: { label: 'Đang chuẩn bị', className: 'text-blue-400 bg-blue-400/10' },
  delivering: { label: 'Đang giao', className: 'text-orange-500 bg-orange-500/10' },
  completed: { label: 'Hoàn thành', className: 'text-emerald-400 bg-emerald-400/10' },
  cancelled: { label: 'Đã huỷ', className: 'text-red-400 bg-red-400/10' },
};

const RECENT_ORDERS = [
  { id: '#ER-10482', customer: 'Nguyễn Văn An', restaurant: 'Cơm Tấm Sài Gòn', total: '128.000₫', status: 'delivering' as OrderStatus, time: '2 phút trước' },
  { id: '#ER-10481', customer: 'Trần Thị Bích', restaurant: 'Trà Sữa Gong Cha', total: '65.000₫', status: 'pending' as OrderStatus, time: '5 phút trước' },
  { id: '#ER-10480', customer: 'Lê Hoàng Nam', restaurant: 'Pizza 4P\'s', total: '312.000₫', status: 'preparing' as OrderStatus, time: '9 phút trước' },
  { id: '#ER-10479', customer: 'Phạm Thu Hà', restaurant: 'Bún Bò Huế O Xuân', total: '89.000₫', status: 'completed' as OrderStatus, time: '18 phút trước' },
  { id: '#ER-10478', customer: 'Vũ Minh Quân', restaurant: 'KFC Nguyễn Trãi', total: '156.000₫', status: 'cancelled' as OrderStatus, time: '24 phút trước' },
  { id: '#ER-10477', customer: 'Đặng Bảo Ngọc', restaurant: 'Highlands Coffee', total: '78.000₫', status: 'completed' as OrderStatus, time: '31 phút trước' },
];

const PENDING_APPROVALS = [
  { label: 'Nhà hàng chờ duyệt', count: 4 },
  { label: 'Shipper chờ duyệt', count: 7 },
  { label: 'Khiếu nại chưa xử lý', count: 2 },
];

// ---------------------------------------------------------------------------
// Sidebar
// ---------------------------------------------------------------------------

const NAV_SECTIONS = [
  {
    items: [{ label: 'Tổng quan', to: '/admin', icon: LayoutDashboard }],
  },
  {
    heading: 'Vận hành',
    items: [
      { label: 'Đơn hàng', to: '/admin/orders', icon: ShoppingBag },
      { label: 'Nhà hàng', to: '/admin/restaurants', icon: Utensils },
      { label: 'Shipper', to: '/admin/shippers', icon: Truck },
    ],
  },
  {
    heading: 'Quản lý',
    items: [
      { label: 'Khách hàng', to: '/admin/customers', icon: Users },
      { label: 'Khuyến mãi', to: '/admin/promotions', icon: Tag },
      { label: 'Cài đặt', to: '/admin/settings', icon: Settings },
    ],
  },
];

function AdminSidebar() {
  const location = useLocation();

  return (
    <aside className="hidden lg:flex flex-col w-64 shrink-0 border-r border-border bg-surface h-screen sticky top-0">
      <Link to="/admin" className="flex items-center gap-2.5 px-6 h-16 border-b border-border shrink-0">
        <div className="w-8 h-8 bg-brand rounded-xl flex items-center justify-center shadow-[0_0_16px_rgba(255,69,0,0.4)]">
          <span className="text-orange-600 font-display font-bold text-2xl">E</span>
        </div>
        <span className="font-display text-xl font-bold tracking-tight">
          <span className="text-orange-500">Eat</span>
          <span className="text-gray-400">Rush</span>
        </span>
        <span className="ml-1 text-[10px] font-bold uppercase tracking-wider text-muted border border-border rounded-full px-2 py-0.5">
          Admin
        </span>
      </Link>

      <nav className="flex-1 overflow-y-auto px-3 py-4 space-y-6">
        {NAV_SECTIONS.map((section, i) => (
          <div key={i}>
            {section.heading && (
              <p className="px-3 mb-2 text-[11px] font-bold uppercase tracking-wider text-muted/60">
                {section.heading}
              </p>
            )}
            <ul className="space-y-1">
              {section.items.map((item) => {
                const isActive = location.pathname === item.to;
                const Icon = item.icon;
                return (
                  <li key={item.to}>
                    <Link
                      to={item.to}
                      className={`
                        flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-semibold transition-colors
                        ${isActive
                          ? 'text-orange-500 bg-orange-500/10'
                          : 'text-muted hover:text-orange-500 hover:bg-white/5'
                        }
                      `}
                    >
                      <Icon size={18} />
                      {item.label}
                    </Link>
                  </li>
                );
              })}
            </ul>
          </div>
        ))}
      </nav>

      <div className="p-3 border-t border-border">
        <div className="flex items-center gap-2.5 px-3 py-2.5 rounded-lg bg-white/5">
          <span className="w-8 h-8 rounded-full bg-orange-500/20 text-orange-500 flex items-center justify-center text-xs font-bold shrink-0">
            AD
          </span>
          <div className="min-w-0">
            <p className="text-sm font-semibold text-black truncate">Admin User</p>
            <p className="text-xs text-muted truncate">admin@eatrush.vn</p>
          </div>
        </div>
      </div>
    </aside>
  );
}

// ---------------------------------------------------------------------------
// Topbar
// ---------------------------------------------------------------------------

function AdminTopbar() {
  return (
    <header className="sticky top-0 z-40 h-16 flex items-center justify-between gap-4 px-6 border-b border-border bg-surface/90 backdrop-blur-xl">
      <div>
        <h1 className="font-display text-lg font-bold text-black leading-tight">Tổng quan</h1>
        <p className="text-xs text-muted">Chào mừng trở lại, đây là tình hình hôm nay</p>
      </div>

      <div className="flex items-center gap-3">
        <div className="hidden md:flex items-center gap-2.5 bg-white/5 hover:bg-white/10 border border-white/10 rounded-full px-4 py-2 text-sm text-muted transition-colors cursor-pointer w-64">
          <Search size={16} />
          <span>Tìm đơn, nhà hàng, khách hàng...</span>
        </div>

        <button
          className="relative w-9 h-9 flex items-center justify-center rounded-full text-muted hover:text-orange-500 hover:bg-white/5 transition-colors"
          aria-label="Thông báo"
        >
          <Bell size={18} />
          <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-orange-500 rounded-full ring-2 ring-surface" />
        </button>

        <span className="w-9 h-9 rounded-full bg-orange-500/20 text-orange-500 flex items-center justify-center text-xs font-bold">
          AD
        </span>
      </div>
    </header>
  );
}

// ---------------------------------------------------------------------------
// Stat cards
// ---------------------------------------------------------------------------

function StatCard({ label, value, delta, trend, icon: Icon }: (typeof STATS)[number]) {
  const isUp = trend === 'up';
  return (
    <div className="bg-surface border border-border rounded-2xl p-5 hover:border-orange-500/30 transition-colors">
      <div className="flex items-start justify-between mb-4">
        <span className="p-2.5 rounded-xl bg-orange-500/10 text-orange-500">
          <Icon size={20} />
        </span>
        <span
          className={`
            flex items-center gap-1 text-xs font-bold px-2 py-1 rounded-full
            ${isUp ? 'text-emerald-400 bg-emerald-400/10' : 'text-red-400 bg-red-400/10'}
          `}
        >
          {isUp ? <TrendingUp size={12} /> : <TrendingDown size={12} />}
          {delta}
        </span>
      </div>
      <p className="text-2xl font-bold text-black font-display tracking-tight">{value}</p>
      <p className="text-sm text-muted mt-1">{label}</p>
    </div>
  );
}

// ---------------------------------------------------------------------------
// Recent orders table
// ---------------------------------------------------------------------------

function RecentOrdersCard() {
  return (
    <div className="bg-surface border border-border rounded-2xl overflow-hidden">
      <div className="flex items-center justify-between px-5 py-4 border-b border-border">
        <div>
          <h2 className="font-display font-bold text-black">Đơn hàng gần đây</h2>
          <p className="text-xs text-muted mt-0.5">{RECENT_ORDERS.length} đơn mới nhất</p>
        </div>
        <Link
          to="/admin/orders"
          className="flex items-center gap-1 text-sm font-semibold text-orange-500 hover:text-orange-400 transition-colors"
        >
          Xem tất cả
          <ChevronRight size={16} />
        </Link>
      </div>

      <div className="overflow-x-auto">
        <table className="w-full text-sm">
          <thead>
            <tr className="text-left text-muted text-xs uppercase tracking-wider">
              <th className="px-5 py-3 font-semibold">Mã đơn</th>
              <th className="px-5 py-3 font-semibold">Khách hàng</th>
              <th className="px-5 py-3 font-semibold hidden md:table-cell">Nhà hàng</th>
              <th className="px-5 py-3 font-semibold">Tổng tiền</th>
              <th className="px-5 py-3 font-semibold">Trạng thái</th>
              <th className="px-5 py-3 font-semibold hidden sm:table-cell">Thời gian</th>
            </tr>
          </thead>
          <tbody>
            {RECENT_ORDERS.map((order) => {
              const status = STATUS_MAP[order.status];
              return (
                <tr key={order.id} className="border-t border-border hover:bg-white/[0.03] transition-colors">
                  <td className="px-5 py-3.5 font-mono text-xs text-muted">{order.id}</td>
                  <td className="px-5 py-3.5 font-semibold text-black">{order.customer}</td>
                  <td className="px-5 py-3.5 text-muted hidden md:table-cell">{order.restaurant}</td>
                  <td className="px-5 py-3.5 font-semibold text-black">{order.total}</td>
                  <td className="px-5 py-3.5">
                    <span className={`inline-flex px-2.5 py-1 rounded-full text-xs font-bold ${status.className}`}>
                      {status.label}
                    </span>
                  </td>
                  <td className="px-5 py-3.5 text-muted text-xs hidden sm:table-cell">{order.time}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
}

// ---------------------------------------------------------------------------
// Pending approvals card
// ---------------------------------------------------------------------------

function PendingApprovalsCard() {
  return (
    <div className="bg-surface border border-border rounded-2xl p-5">
      <h2 className="font-display font-bold text-black mb-4">Cần xử lý</h2>
      <ul className="space-y-3">
        {PENDING_APPROVALS.map((item) => (
          <li key={item.label} className="flex items-center justify-between">
            <div className="flex items-center gap-2.5">
              <span className="w-1.5 h-1.5 rounded-full bg-orange-500" />
              <span className="text-sm text-muted">{item.label}</span>
            </div>
            <span className="text-sm font-bold text-black bg-white/5 rounded-full w-6 h-6 flex items-center justify-center">
              {item.count}
            </span>
          </li>
        ))}
      </ul>

      <div className="mt-5 pt-4 border-t border-border space-y-2">
        <div className="flex items-center gap-2 text-xs text-muted">
          <CheckCircle2 size={14} className="text-emerald-400" />
          Tỉ lệ hoàn thành đơn hôm nay: <span className="font-bold text-black">94.2%</span>
        </div>
        <div className="flex items-center gap-2 text-xs text-muted">
          <XCircle size={14} className="text-red-400" />
          Tỉ lệ huỷ đơn: <span className="font-bold text-black">2.1%</span>
        </div>
        <div className="flex items-center gap-2 text-xs text-muted">
          <Clock size={14} className="text-amber-400" />
          Thời gian giao trung bình: <span className="font-bold text-black">27 phút</span>
        </div>
      </div>
    </div>
  );
}

// ---------------------------------------------------------------------------
// Page
// ---------------------------------------------------------------------------

export default function AdminHomePage() {
  const [, setMounted] = useState(true);
  void setMounted;

  return (
    <div className="flex bg-background min-h-screen text-black">
      <AdminSidebar />

      <div className="flex-1 min-w-0">
        <AdminTopbar />

        <main className="p-6 space-y-6 max-w-[1600px]">
          <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-4">
            {STATS.map((stat) => (
              <StatCard key={stat.label} {...stat} />
            ))}
          </div>

          <div className="grid grid-cols-1 xl:grid-cols-3 gap-6">
            <div className="xl:col-span-2">
              <RecentOrdersCard />
            </div>
            <PendingApprovalsCard />
          </div>
        </main>
      </div>
    </div>
  );
}