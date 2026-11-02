// src/components/layout/AccountDropdown.tsx
import { useState, useRef, useEffect } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { User, LogOut, Package, ChevronDown, Settings, Truck, Store } from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';
import { cn } from '@/utils/cn';

export function AccountDropdown() {
  const { user, logout } = useAuth();
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);
  const navigate = useNavigate();
  const { pathname, search } = useLocation();

  // Đóng khi click ra ngoài — dùng 'click' (không phải 'mousedown') để không nuốt click của Link
  useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
      }
    }
    document.addEventListener('click', handleClickOutside);
    return () => document.removeEventListener('click', handleClickOutside);
  }, []);

  // Đóng mỗi khi route đổi
  useEffect(() => {
    setOpen(false);
  }, [pathname, search]);

  const handleLogout = async () => {
    await logout();
    setOpen(false);
    navigate('/login');
  };

  const initial = user?.name?.charAt(0).toUpperCase();
  const roles = user?.roles ?? [];

  return (
    <div className="relative" ref={ref}>
      {/* ===== Nút trigger ===== */}
      <button
        onClick={() => setOpen((prev) => !prev)}
        className={cn(
          'flex items-center gap-2.5 rounded-full border py-1.5 pl-2 pr-3 text-sm font-medium',
          'transition-all duration-200',
          open
            ? 'border-orange-300 bg-orange-50 text-gray-900'
            : 'border-gray-200 bg-white text-gray-700 hover:border-gray-300 hover:bg-gray-50'
        )}
      >
        <span className="flex h-7 w-7 items-center justify-center rounded-full bg-gradient-to-br from-orange-400 to-orange-600 text-xs font-bold text-white shadow-sm">
          {initial ?? <User size={14} />}
        </span>
        <span className="max-w-[100px] truncate font-semibold text-orange-600">
          {user?.name ?? 'Tài khoản'}
        </span>
        <ChevronDown
          size={14}
          className={cn(
            'text-orange-500 transition-transform duration-200',
            open && 'rotate-180'
          )}
        />
      </button>

      {/* ===== Dropdown ===== */}
      <div
        className={cn(
          'absolute right-0 top-full z-50 mt-2 w-60 origin-top-right overflow-hidden',
          'rounded-2xl border border-gray-200 bg-white',
          'shadow-lg shadow-gray-200/60',
          'transition-all duration-200',
          open
            ? 'pointer-events-auto translate-y-0 scale-100 opacity-100'
            : 'pointer-events-none -translate-y-1 scale-95 opacity-0'
        )}
      >
        {/* Header user */}
        <div className="flex items-center gap-3 border-b border-gray-100 bg-gradient-to-br from-orange-50 to-transparent px-4 py-3.5">
          <span className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-gradient-to-br from-orange-400 to-orange-600 text-sm font-bold text-white shadow-sm">
            {initial ?? <User size={16} />}
          </span>
          <div className="min-w-0">
            <p className="truncate text-sm font-semibold text-gray-900">
              {user?.name ?? 'Tài khoản'}
            </p>
            {user?.email && (
              <p className="truncate text-xs text-gray-500">{user.email}</p>
            )}
          </div>
        </div>

        {/* Menu items */}
        <div className="py-1.5">
          <MenuItem to="/account" icon={User}>
            Xem tài khoản
          </MenuItem>
          <MenuItem to="/account?tab=orders" icon={Package}>
            Đơn hàng của tôi
          </MenuItem>

          {!roles.includes('Shipper') && (
            <MenuItem to="/shipper/apply" icon={Truck}>
              Đăng ký làm shipper
            </MenuItem>
          )}

          {!roles.includes('RestaurantOwner') && !roles.includes('RestaurantStaff') && (
            <MenuItem to="/restaurant/apply" icon={Store}>
              Đăng ký kinh doanh nhà hàng
            </MenuItem>
          )}

          <MenuItem to="/settings" icon={Settings}>
            Cài đặt
          </MenuItem>
        </div>

        {/* Divider */}
        <div className="border-t border-gray-100" />

        {/* Logout */}
        <button
          onClick={handleLogout}
          className="flex w-full items-center gap-2.5 px-4 py-3 text-sm font-medium text-red-500 transition-colors hover:bg-red-50 hover:text-red-600"
        >
          <LogOut size={16} />
          Đăng xuất
        </button>
      </div>
    </div>
  );
}

/* ================= Sub-component ================= */

function MenuItem({
  to,
  icon: Icon,
  children,
}: {
  to: string;
  icon: typeof User;
  children: React.ReactNode;
}) {
  return (
    <Link
      to={to}
      className="flex items-center gap-2.5 px-4 py-2.5 text-sm text-gray-600 transition-colors hover:bg-orange-50 hover:text-orange-600"
    >
      <Icon size={16} />
      {children}
    </Link>
  );
}