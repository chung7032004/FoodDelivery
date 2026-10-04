import { useState, useRef, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { User, LogOut, Package, ChevronDown, Settings, Truck, Store } from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';

export function AccountDropdown() {
  const { user, logout } = useAuth();
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);
  const navigate = useNavigate();

  useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
      }
    }
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const handleLogout = async () => {
    await logout();
    setOpen(false);
    navigate('/login');
  };

  const initial = user?.name?.charAt(0).toUpperCase();

  return (
    <div className="relative" ref={ref}>
      <button
        onClick={() => setOpen((prev) => !prev)}
        className={`
          flex items-center gap-2.5 pl-2 pr-3 py-1.5 rounded-full text-sm font-medium
          border transition-all duration-200
          ${open
            ? 'bg-orange-500/10 border-orange-500/40 text-white'
            : 'bg-white/5 border-white/10 text-muted hover:bg-white/10 hover:border-white/20 hover:text-white'
          }
        `}
      >
        <span className="w-7 h-7 rounded-full bg-gradient-to-br from-orange-400 to-orange-600 text-white flex items-center justify-center text-xs font-bold shadow-[0_0_10px_rgba(251,105,0,0.4)]">
          {initial ?? <User size={14} />}
        </span>
        <span className="max-w-[100px] truncate text-orange-500">{user?.name ?? 'Tài khoản'}</span>
        <ChevronDown
          size={14}
          className={`text-orange-400 transition-transform duration-200 ${open ? 'rotate-180' : ''}`}
        />
      </button>

      <div
        className={`
          absolute right-0 top-full mt-2 w-56 origin-top-right
          bg-surface border border-white/10 rounded-2xl shadow-[0_12px_40px_rgba(0,0,0,0.55)]
          overflow-hidden transition-all duration-200 z-50
          ${open ? 'opacity-100 scale-100 translate-y-0 pointer-events-auto' : 'opacity-0 scale-95 -translate-y-1 pointer-events-none'}
        `}
      >
        {/* Header */}
        <div className="flex items-center gap-3 px-4 py-3.5 bg-gradient-to-br from-orange-500/15 to-transparent border-b border-white/10">
          <span className="w-9 h-9 shrink-0 rounded-full bg-gradient-to-br from-orange-400 to-orange-600 text-white flex items-center justify-center text-sm font-bold shadow-[0_0_10px_rgba(251,105,0,0.4)]">
            {initial ?? <User size={16} />}
          </span>
          <div className="min-w-0">
            <p className="text-sm font-semibold text-orange-500 truncate">{user?.name ?? 'Tài khoản'}</p>
            {user?.email && (
              <p className="text-xs text-muted truncate">{user.email}</p>
            )}
          </div>
        </div>

        {/* Menu items */}
        <div className="py-1.5">
          <Link
            to="/account"
            onClick={() => setOpen(false)}
            className="flex items-center gap-2.5 px-4 py-2.5 text-sm text-muted hover:text-orange-400 hover:bg-orange-500/5 transition-colors"
          >
            <User size={16} /> Xem tài khoản
          </Link>
          <Link
            to="/orders"
            onClick={() => setOpen(false)}
            className="flex items-center gap-2.5 px-4 py-2.5 text-sm text-muted hover:text-orange-400 hover:bg-orange-500/5 transition-colors"
          >
            <Package size={16} /> Đơn hàng của tôi
          </Link>
          {!user?.roles.includes('Shipper') && (
            <Link
              to="/shipper/apply"
              onClick={() => setOpen(false)}
              className="flex items-center gap-2.5 px-4 py-2.5 text-sm text-muted hover:text-orange-400 hover:bg-orange-500/5 transition-colors"
            >
              <Truck size={16} /> Đăng ký làm shipper
            </Link>
          )}
          {!user?.roles.includes('RestaurantOwner') && !user?.roles.includes('RestaurantStaff') && (
            <Link
              to="/restaurant/apply"
              onClick={() => setOpen(false)}
              className="flex items-center gap-2.5 px-4 py-2.5 text-sm text-muted hover:text-orange-400 hover:bg-orange-500/5 transition-colors"
            >
              <Store size={16} /> Đăng ký kinh doanh nhà hàng
            </Link>
          )}
          <Link
            to="/settings"
            onClick={() => setOpen(false)}
            className="flex items-center gap-2.5 px-4 py-2.5 text-sm text-muted hover:text-orange-400 hover:bg-orange-500/5 transition-colors"
          >
            <Settings size={16} /> Cài đặt
          </Link>
        </div>

        <div className="border-t border-white/10" />

        <button
          onClick={handleLogout}
          className="w-full flex items-center gap-2.5 px-4 py-3 text-sm font-medium text-red-400 hover:bg-red-500/10 transition-colors"
        >
          <LogOut size={16} /> Đăng xuất
        </button>
      </div>
    </div>
  );
}