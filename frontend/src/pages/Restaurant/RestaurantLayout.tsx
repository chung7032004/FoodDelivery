import { ReactNode, useEffect, useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import {
  Bell,
  ClipboardList,
  Home,
  LayoutDashboard,
  LogOut,
  Menu,
  PackageOpen,
  Store,
  X,
} from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';

interface NavigationItem {
  label: string;
  to: string;
  icon: React.ElementType;
}

const items: NavigationItem[] = [
  { label: 'Tổng quan quán', to: '/restaurant', icon: LayoutDashboard },
  { label: 'Quản lý đơn hàng', to: '/restaurant/orders', icon: ClipboardList },
  { label: 'Thực đơn', to: '/restaurant/menu', icon: PackageOpen },
  { label: 'Thông tin nhà hàng', to: '/restaurant/settings', icon: Store },
];

export function RestaurantLayout({ children }: { children: ReactNode }) {
  const location = useLocation();
  const { user, logout } = useAuth();
  const [open, setOpen] = useState(false);

  // Tự động đóng Mobile Menu khi chuyển trang
  useEffect(() => {
    setOpen(false);
  }, [location.pathname]);

  // Khóa scroll body khi mở Mobile Menu
  useEffect(() => {
    document.body.style.overflow = open ? 'hidden' : '';
    return () => {
      document.body.style.overflow = '';
    };
  }, [open]);

  return (
    <div className="flex h-screen overflow-hidden bg-slate-50 text-slate-900">
      {/* Sidebar Desktop */}
      <aside className="hidden h-screen w-72 shrink-0 flex-col border-r border-slate-200 bg-white lg:flex">
        <Brand />
        <div className="min-h-0 flex-1 overflow-y-auto">
          <NavigationMenu currentPath={location.pathname} />
        </div>
        <UserFooter user={user} onLogout={() => void logout()} />
      </aside>

      {/* Overlay Mobile */}
      {open && (
        <div
          className="fixed inset-0 z-40 bg-slate-950/40 backdrop-blur-sm lg:hidden"
          onClick={() => setOpen(false)}
        />
      )}

      {/* Sidebar Mobile Drawer */}
      <aside
        className={`fixed inset-y-0 left-0 z-50 flex w-[min(86vw,20rem)] flex-col bg-white shadow-2xl transition-transform duration-300 ease-in-out lg:hidden ${
          open ? 'translate-x-0' : '-translate-x-full'
        }`}
      >
        <div className="flex items-center justify-between border-b border-slate-200">
          <Brand />
          <button
            onClick={() => setOpen(false)}
            aria-label="Đóng menu"
            className="mr-3 rounded-xl p-2 text-slate-500 transition hover:bg-slate-100"
          >
            <X size={20} />
          </button>
        </div>
        <div className="min-h-0 flex-1 overflow-y-auto">
          <NavigationMenu currentPath={location.pathname} />
        </div>
        <UserFooter user={user} onLogout={() => void logout()} />
      </aside>

      {/* Area Nội dung chính */}
      <div className="flex min-w-0 flex-1 flex-col">
        {/* Header trên */}
        <header className="flex h-16 shrink-0 items-center justify-between border-b border-slate-200 bg-white px-4 sm:px-6">
          <div className="flex items-center gap-3">
            <button
              onClick={() => setOpen(true)}
              aria-label="Mở menu nhà hàng"
              className="rounded-xl p-2 text-sky-700 transition hover:bg-sky-50 lg:hidden"
            >
              <Menu size={21} />
            </button>
            <div>
              <p className="hidden text-[11px] font-bold uppercase tracking-[0.16em] text-sky-700 sm:block">
                Restaurant workspace
              </p>
              <h1 className="font-display text-base font-bold sm:text-lg">
                Không gian nhà hàng
              </h1>
            </div>
          </div>

          <div className="flex items-center gap-2">
            <Link
              to="/"
              className="hidden rounded-xl border border-slate-200 px-3.5 py-2 text-xs font-bold text-slate-600 transition hover:border-sky-500 hover:text-sky-700 sm:block"
            >
              Trang người dùng
            </Link>
            <button
              aria-label="Thông báo"
              className="rounded-xl p-2 text-slate-500 transition hover:bg-slate-100"
            >
              <Bell size={19} />
            </button>
          </div>
        </header>

        {/* Nội dung trang */}
        <main className="min-h-0 flex-1 overflow-y-auto">
          <div className="mx-auto max-w-[1500px] p-4 sm:p-6">{children}</div>
        </main>
      </div>
    </div>
  );
}

/* Component Logo thương hiệu */
function Brand() {
  return (
    <Link
      to="/restaurant"
      className="flex h-16 shrink-0 items-center gap-3 border-b border-slate-200 px-5"
    >
      <span className="flex h-9 w-9 items-center justify-center rounded-xl bg-sky-600 text-xl font-bold text-white shadow-sm">
        E
      </span>
      <span className="font-display text-xl font-bold">
        <span className="text-orange-500">Eat</span>
        <span className="text-slate-500">Rush</span>
      </span>
      <span className="rounded-full bg-sky-50 px-2 py-0.5 text-[9px] font-bold uppercase tracking-wider text-sky-700">
        Quán
      </span>
    </Link>
  );
}

/* Component Danh sách Menu điều hướng */
function NavigationMenu({ currentPath }: { currentPath: string }) {
  return (
    <nav className="space-y-1 p-4">
      {items.map(({ label, to, icon: Icon }) => {
        const active = currentPath === to;

        return (
          <Link
            key={label}
            to={to}
            className={`flex items-center justify-between rounded-xl px-3.5 py-3 text-sm font-semibold transition ${
              active
                ? 'bg-sky-600 text-white shadow-lg shadow-sky-600/20'
                : 'text-slate-600 hover:bg-sky-50 hover:text-sky-700'
            }`}
          >
            <span className="flex items-center gap-3">
              <Icon size={18} />
              {label}
            </span>
            {active && <span className="h-1.5 w-1.5 rounded-full bg-white" />}
          </Link>
        );
      })}

      <div className="my-4 border-t border-slate-200" />

      <Link
        to="/"
        className="flex items-center gap-3 rounded-xl px-3.5 py-3 text-sm font-semibold text-slate-600 transition hover:bg-slate-100"
      >
        <Home size={18} />
        Trang người dùng
      </Link>
    </nav>
  );
}

/* Component Footer hiển thị User & Đăng xuất */
function UserFooter({
  user,
  onLogout,
}: {
  user: { name?: string; email?: string } | null;
  onLogout: () => void;
}) {
  const avatarText = user?.name ? user.name.slice(0, 2).toUpperCase() : 'QU';

  return (
    <div className="border-t border-slate-200 p-4">
      <div className="mb-3 flex items-center gap-3 px-2">
        <span className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-sky-100 text-xs font-bold text-sky-700">
          {avatarText}
        </span>
        <div className="min-w-0">
          <p className="truncate text-sm font-semibold text-slate-800">
            {user?.name || 'Quản lý quán'}
          </p>
          <p className="truncate text-xs text-slate-500">{user?.email || ''}</p>
        </div>
      </div>
      <button
        onClick={onLogout}
        className="flex w-full items-center gap-3 rounded-xl px-3 py-2.5 text-sm font-semibold text-slate-500 transition hover:bg-red-50 hover:text-red-600"
      >
        <LogOut size={17} />
        Đăng xuất
      </button>
    </div>
  );
}