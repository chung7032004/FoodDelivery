import { useState, useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { ButtonPrimary } from '../ui';
import { Search, ShoppingCart, User, LogOut, Package } from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';
import { AccountDropdown } from './AccountDropdown';

const NAV_LINKS = [
  { label: 'Trang chủ', to: '/' },
  { label: 'Thực đơn', to: '/list-category' },
  { label: 'Nhà hàng', to: '/restaurants' },
  { label: 'Khuyến mãi', to: '/promotions' },
];

interface HeaderNavProps {
  cartCount?: number;
}

export function HeaderNav({ cartCount = 3 }: HeaderNavProps) {
  const location = useLocation();
  const navigate = useNavigate();
  const { user, isAuthenticated, logout } = useAuth();
  const [scrolled, setScrolled] = useState(false);
  const [mobileOpen, setMobileOpen] = useState(false);

  useEffect(() => {
    const fn = () => setScrolled(window.scrollY > 24);
    window.addEventListener('scroll', fn);
    return () => window.removeEventListener('scroll', fn);
  }, []);

  const handleLogoutClick = async () => {
    await logout();
    setMobileOpen(false);
    navigate('/login');
  };

  return (
    <header className={`
      sticky top-0 z-50 transition-all duration-300
      ${scrolled
        ? 'bg-surface/90 backdrop-blur-xl border-b border-border shadow-[0_4px_32px_rgba(0,0,0,0.5)]'
        : 'bg-surface border-b border-border'
      }
    `}>
      <div className="max-w-7xl mx-auto px-6 lg:px-8 h-16 flex items-center justify-between gap-6">

        {/* Logo */}
        <Link to="/" className="flex items-center gap-2.5 shrink-0 group">
          <div className="w-8 h-8 bg-brand rounded-xl flex items-center justify-center shadow-[0_0_16px_rgba(255,69,0,0.4)] group-hover:shadow-[0_0_24px_rgba(255,69,0,0.6)] transition-shadow">
            <span className="text-orange-600 font-display font-bold text-2xl">E</span>
          </div>
          <span className="font-display text-xl font-bold tracking-tight">
            <span className="text-orange-500">Eat</span>
            <span className="text-gray-400">Rush</span>
          </span>
        </Link>

        {/* Nav */}
        <nav className="hidden lg:block">
          <ul className="flex gap-1">
            {NAV_LINKS.map((link) => {
              const isActive = location.pathname === link.to;
              return (
                <li key={link.to}>
                  <Link
                    to={link.to}
                    className={`
                      relative px-5 py-2 text-sm font-bold rounded-full transition-all duration-200 inline-block
                      ${isActive
                        ? 'text-orange-500 bg-orange/10'
                        : 'text-muted hover:text-orange-500 hover:bg-orange/5'
                      }
                    `}
                  >
                    {link.label}
                    {isActive && (
                      <span className="absolute bottom-1 left-1/2 -translate-x-1/2 w-4 h-0.5 bg-brand rounded-full" />
                    )}
                  </Link>
                </li>
              );
            })}
          </ul>
        </nav>

        {/* Right */}
        <div className="flex items-center gap-3">
          {/* Search pill */}
          <div className="hidden md:flex items-center gap-2.5 bg-white/5 hover:bg-white/10 border border-white/10 hover:border-white/20 rounded-full px-4 py-2 text-sm text-muted transition-all duration-200 cursor-pointer group shadow-sm active:scale-95">
            <span className="p-1 rounded-full bg-[#fb6900]/10 text-[#fb6900] group-hover:bg-[#fb6900] group-hover:text-white transition-all duration-200 group-hover:scale-115">
              <Search size={16} />
            </span>
            <span className="group-hover:text-orange-500 transition-colors">Tìm món...</span>
          </div>

          {/* Auth — desktop */}
          <div className="hidden md:flex items-center gap-2">
            {isAuthenticated ? (
              <AccountDropdown />
            ) : (
              <div className="flex items-center gap-2">
                <Link
                  to="/login"
                  className="px-4 py-2 text-sm font-semibold text-muted hover:text-[#fb6900] transition-all duration-200 active:scale-95 inline-flex items-center justify-center"
                >
                  Đăng nhập
                </Link>
                <Link
                  to="/register"
                  className="px-4 py-2 text-sm font-semibold text-[#fb6900] border border-[#fb6900]/30 bg-[#fb6900]/10 hover:bg-[#fb6900] hover:text-white rounded-full transition-all duration-200 active:scale-95 inline-flex items-center justify-center"
                >
                  Đăng ký
                </Link>
              </div>
            )}
          </div>

          <Link to="/cart" className="inline-block group">
            <ButtonPrimary size="sm" className="relative px-4 py-2 transition-all duration-300 hover:scale-105 bg-orange-500 hover:bg-orange-600">
              <div className="flex items-center gap-2">
                <ShoppingCart size={18} className="text-white transition-transform group-hover:scale-110" />
                <span className="hidden sm:inline font-medium text-white">Giỏ hàng</span>
              </div>

              {cartCount > 0 && (
                <span className="absolute -top-2 -right-2 min-w-[22px] h-[22px] px-1.5 bg-orange-500 text-white text-[11px] font-bold rounded-full flex items-center justify-center shadow-lg shadow-orange-500/30 ring-2 ring-white dark:ring-gray-800 animate-pulse">
                  {cartCount > 99 ? '99+' : cartCount}
                </span>
              )}
            </ButtonPrimary>
          </Link>

          {/* Hamburger — mobile */}
          <button
            onClick={() => setMobileOpen((prev) => !prev)}
            className="lg:hidden w-9 h-9 flex items-center justify-center rounded-full text-orange-500 hover:bg-white/5 transition-colors"
            aria-label="Menu"
          >
            {mobileOpen ? '✕' : '☰'}
          </button>
        </div>

      </div>

      {/* Mobile menu */}
      {mobileOpen && (
        <div className="lg:hidden border-t border-border bg-surface px-6 py-4 space-y-4">
          <ul className="space-y-1">
            {NAV_LINKS.map((link) => {
              const isActive = location.pathname === link.to;
              return (
                <li key={link.to}>
                  <Link
                    to={link.to}
                    onClick={() => setMobileOpen(false)}
                    className={`block w-full text-left px-3 py-2 rounded-lg text-sm font-medium transition-colors
                      ${isActive ? 'text-orange-500 bg-white/10' : 'text-muted hover:text-orange-500 hover:bg-white/5'}
                    `}
                  >
                    {link.label}
                  </Link>
                </li>
              );
            })}
          </ul>

          <div className="border-t border-border pt-4 flex flex-col gap-2">
            {isAuthenticated ? (
              <>
                <div className="flex items-center gap-2.5 px-3 py-2 text-sm font-medium text-white">
                  <span className="w-7 h-7 rounded-full bg-orange-500/20 text-orange-500 flex items-center justify-center text-xs font-bold">
                    {user?.name?.charAt(0).toUpperCase() ?? <User size={14} />}
                  </span>
                  {user?.name ?? 'Tài khoản'}
                </div>
                <Link
                  to="/account"
                  onClick={() => setMobileOpen(false)}
                  className="flex items-center gap-2.5 w-full px-4 py-2 text-sm font-medium text-muted hover:text-orange-500 text-left"
                >
                  <User size={16} /> Xem tài khoản
                </Link>
                <Link
                  to="/orders"
                  onClick={() => setMobileOpen(false)}
                  className="flex items-center gap-2.5 w-full px-4 py-2 text-sm font-medium text-muted hover:text-orange-500 text-left"
                >
                  <Package size={16} /> Đơn hàng của tôi
                </Link>
                <button
                  onClick={handleLogoutClick}
                  className="flex items-center gap-2.5 w-full px-4 py-2 text-sm font-medium text-red-500 hover:bg-red-500/10 rounded-lg text-left"
                >
                  <LogOut size={16} /> Đăng xuất
                </button>
              </>
            ) : (
              <>
                <Link
                  to="/login"
                  onClick={() => setMobileOpen(false)}
                  className="w-full px-4 py-2 text-sm font-medium text-center text-muted hover:text-orange-500 border border-white/10 rounded-full transition-colors"
                >
                  Đăng nhập
                </Link>
                <Link
                  to="/register"
                  onClick={() => setMobileOpen(false)}
                  className="w-full px-4 py-2 text-sm font-semibold text-center text-black bg-brand rounded-full hover:text-orange-500 transition-opacity"
                >
                  Đăng ký
                </Link>
              </>
            )}
          </div>
        </div>
      )}
    </header>
  );
}