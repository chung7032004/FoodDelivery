import { ReactNode, useEffect, useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import {
  Bell,
  Boxes,
  ChevronRight,
  Home,
  LayoutDashboard,
  LogOut,
  Menu,
  MessageSquare,
  PackageOpen,
  ShoppingBag,
  Store,
  Truck,
  Users,
  X,
} from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';

const navItems = [
  { label: 'Tổng quan', to: '/admin', icon: LayoutDashboard },
  { label: 'Tài khoản', to: '/admin/users', icon: Users },
  { label: 'Đơn hàng', to: '/admin/orders', icon: ShoppingBag },
  { label: 'Shipper', to: '/admin/shippers', icon: Truck },
  { label: 'Nhà hàng', to: '/admin/restaurants', icon: Store },
  { label: 'Đánh giá', to: '/admin/reviews', icon: MessageSquare },
  { label: 'Danh mục', to: '/admin/categories', icon: Boxes },
  { label: 'Sản phẩm', to: '/admin/products', icon: PackageOpen },
];

export function AdminLayout({ children }: { children: ReactNode }) {
  const location = useLocation();
  const { user, logout } = useAuth();
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  // Tự động đóng menu mobile khi đổi trang
  useEffect(() => {
    setMobileMenuOpen(false);
  }, [location.pathname]);

  // Khóa scroll body khi mở menu mobile & lắng nghe phím Escape
  useEffect(() => {
    if (mobileMenuOpen) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = '';
    }

    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') setMobileMenuOpen(false);
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => {
      document.body.style.overflow = '';
      window.removeEventListener('keydown', handleKeyDown);
    };
  }, [mobileMenuOpen]);

  const renderNavigation = (isMobile = false) => (
    <nav className={`${isMobile ? 'space-y-1' : 'min-h-0 flex-1 space-y-1 overflow-y-auto'} p-4`}>
      <p className="mb-3 px-3 text-[11px] font-bold uppercase tracking-[0.16em] text-muted/60">
        Không gian quản trị
      </p>
      {navItems.map(({ label, to, icon: Icon }) => {
        const active = location.pathname === to;
        return (
          <Link
            key={to}
            to={to}
            className={`flex items-center justify-between rounded-xl px-3 py-3 text-sm font-semibold transition-all duration-200 ${
              active
                ? 'bg-orange-500 text-white shadow-lg shadow-orange-500/20'
                : 'text-muted hover:bg-orange-500/10 hover:text-orange-600'
            }`}
          >
            <span className="flex items-center gap-3">
              <Icon size={18} strokeWidth={active ? 2.4 : 1.8} />
              {label}
            </span>
            {active && <ChevronRight size={16} />}
          </Link>
        );
      })}

      <div className="my-4 border-t border-border" />

      <p className="mb-2 px-3 text-[11px] font-bold uppercase tracking-[0.16em] text-muted/60">
        Chuyển không gian
      </p>
      <Link
        to="/"
        className="flex items-center gap-3 rounded-xl px-3 py-3 text-sm font-semibold text-muted transition hover:bg-sky-500/10 hover:text-sky-700"
      >
        <Home size={18} />
        Trang người dùng
      </Link>
    </nav>
  );

  return (
    <div className="flex h-screen overflow-hidden bg-background text-foreground">
      {/* Sidebar Desktop */}
      <aside className="hidden h-screen w-72 shrink-0 flex-col border-r border-border bg-surface lg:flex">
        <Brand />
        {renderNavigation()}
        <AccountFooter userName={user?.name} email={user?.email} logout={logout} />
      </aside>

      {/* Overlay Mobile */}
      {mobileMenuOpen && (
        <div
          className="fixed inset-0 z-40 bg-slate-950/45 transition-opacity lg:hidden"
          onClick={() => setMobileMenuOpen(false)}
          aria-hidden="true"
        />
      )}

      {/* Sidebar Mobile */}
      <aside
        className={`fixed inset-y-0 left-0 z-50 flex w-[min(86vw,20rem)] flex-col bg-surface shadow-2xl transition-transform duration-300 ease-in-out lg:hidden ${
          mobileMenuOpen ? 'translate-x-0' : '-translate-x-full'
        }`}
      >
        <div className="flex items-center justify-between border-b border-border">
          <Brand />
          <button
            onClick={() => setMobileMenuOpen(false)}
            aria-label="Đóng menu"
            className="mr-3 rounded-xl p-2 text-muted transition hover:bg-black/5"
          >
            <X size={20} />
          </button>
        </div>
        <div className="min-h-0 flex-1 overflow-y-auto">{renderNavigation(true)}</div>
        <AccountFooter userName={user?.name} email={user?.email} logout={logout} />
      </aside>

      {/* Main Content Area */}
      <div className="flex min-w-0 flex-1 flex-col">
        {/* Top Header */}
        <header className="z-20 flex h-16 shrink-0 items-center justify-between border-b border-border bg-surface px-4 sm:px-6">
          <div className="flex items-center gap-3">
            <button
              onClick={() => setMobileMenuOpen(true)}
              aria-label="Mở menu quản trị"
              className="rounded-xl p-2 text-orange-600 transition hover:bg-orange-500/10 lg:hidden"
            >
              <Menu size={21} />
            </button>
            <div>
              <p className="hidden text-[11px] font-bold uppercase tracking-[0.16em] text-orange-600 sm:block">
                EatRush Control Center
              </p>
              <h1 className="font-display text-base font-bold sm:text-lg">Quản trị hệ thống</h1>
            </div>
          </div>

          <div className="flex items-center gap-2 sm:gap-4">
            <Link
              to="/"
              title="Sang trang người dùng"
              className="hidden items-center gap-2 rounded-xl border border-border px-3 py-2 text-xs font-bold text-muted transition hover:border-sky-300 hover:text-sky-700 sm:flex"
            >
              <Home size={16} />
              Trang người dùng
            </Link>
            <button
              aria-label="Thông báo"
              className="relative rounded-xl p-2 text-muted transition hover:bg-black/5 hover:text-orange-500"
            >
              <Bell size={19} />
              <span className="absolute right-1.5 top-1.5 h-2 w-2 rounded-full bg-orange-500" />
            </button>
            <span className="flex h-9 w-9 items-center justify-center rounded-full bg-orange-500/15 text-xs font-bold text-orange-600">
              {user?.name?.slice(0, 2).toUpperCase() || 'AD'}
            </span>
          </div>
        </header>

        {/* Dynamic Page Content */}
        <main className="min-h-0 flex-1 overflow-y-auto">
          <div className="mx-auto max-w-[1600px] p-4 sm:p-6">{children}</div>
        </main>
      </div>
    </div>
  );
}

/* --- Sub-components --- */

function Brand() {
  return (
    <Link to="/admin" className="flex h-16 shrink-0 items-center gap-3 border-b border-border px-5">
      <span className="flex h-9 w-9 items-center justify-center rounded-xl bg-orange-500 text-xl font-bold text-white shadow-lg shadow-orange-500/20">
        E
      </span>
      <span className="font-display text-xl font-bold">
        <span className="text-orange-500">Eat</span>
        <span className="text-gray-400">Rush</span>
      </span>
      <span className="rounded-full border border-orange-200 bg-orange-50 px-2 py-0.5 text-[9px] font-bold uppercase tracking-wider text-orange-600">
        Admin
      </span>
    </Link>
  );
}

interface AccountFooterProps {
  userName?: string;
  email?: string;
  logout: () => Promise<void>;
}

function AccountFooter({ userName, email, logout }: AccountFooterProps) {
  return (
    <div className="shrink-0 border-t border-border p-4">
      <div className="mb-3 flex items-center gap-3 px-2">
        <span className="flex h-9 w-9 items-center justify-center rounded-full bg-orange-500/15 text-xs font-bold text-orange-600">
          {userName?.slice(0, 2).toUpperCase() || 'AD'}
        </span>
        <div className="min-w-0">
          <p className="truncate text-sm font-semibold">{userName || 'Admin'}</p>
          <p className="truncate text-xs text-muted">{email || ''}</p>
        </div>
      </div>
      <button
        onClick={() => void logout()}
        className="flex w-full items-center gap-3 rounded-xl px-3 py-2.5 text-sm font-semibold text-muted transition hover:bg-red-50 hover:text-red-500"
      >
        <LogOut size={17} />
        Đăng xuất
      </button>
    </div>
  );
}