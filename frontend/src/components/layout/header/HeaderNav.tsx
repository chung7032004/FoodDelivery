import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ShoppingCart, Search } from 'lucide-react';
import { ButtonPrimary } from '../../ui';
import { useAuth } from '@/contexts/AuthContext';
import { AccountDropdown } from './AccountDropdown';
import { NotificationBell } from '@/components/notifications/NotificationBell';
import { useQuery } from '@tanstack/react-query';
import { cartQueryKey, cartService } from '@/services/cart.service';
import { DesktopNav } from './DesktopNav';
import { RolePills } from './RolePills';
import { MobileMenu } from './MobileMenu';

interface HeaderNavProps {
  cartCount?: number;
}

export function HeaderNav({ cartCount }: HeaderNavProps) {
  const navigate = useNavigate();
  const { isAuthenticated, logout } = useAuth();
  const [scrolled, setScrolled] = useState(false);
  const [mobileOpen, setMobileOpen] = useState(false);

  const { data: cartResult } = useQuery({
    queryKey: cartQueryKey,
    queryFn: cartService.getCart,
    enabled: isAuthenticated && cartCount === undefined,
  });

  const displayedCartCount =
    cartCount ??
    cartResult?.data?.data.reduce((sum, item) => sum + item.quantity, 0) ??
    0;

  useEffect(() => {
    const fn = () => setScrolled(window.scrollY > 24);
    window.addEventListener('scroll', fn);
    return () => window.removeEventListener('scroll', fn);
  }, []);

  const handleLogout = async () => {
    await logout();
    setMobileOpen(false);
    navigate('/login');
  };

  return (
    <header
      className={`sticky top-0 z-50 border-b border-border transition-all duration-300 ${
        scrolled
          ? 'bg-surface/90 shadow-[0_4px_32px_rgba(0,0,0,0.5)] backdrop-blur-xl'
          : 'bg-surface'
      }`}
    >
      <div className="mx-auto flex h-16 max-w-7xl items-center justify-between gap-6 px-6 lg:px-8">
        {/* Logo */}
        <Link to="/" className="group flex shrink-0 items-center gap-2.5">
          <div className="flex h-8 w-8 items-center justify-center rounded-xl bg-brand shadow-[0_0_16px_rgba(255,69,0,0.4)] transition-shadow group-hover:shadow-[0_0_24px_rgba(255,69,0,0.6)]">
            <span className="font-display text-2xl font-bold text-orange-600">E</span>
          </div>
          <span className="font-display text-xl font-bold tracking-tight">
            <span className="text-orange-500">Eat</span>
            <span className="text-gray-400">Rush</span>
          </span>
        </Link>

        <DesktopNav />

        {/* Right */}
        <div className="flex items-center gap-3">
          <SearchPill />

          {/* Auth desktop */}
          <div className="hidden items-center gap-2 md:flex">
            {isAuthenticated ? (
              <AccountDropdown />
            ) : (
              <>
                <Link
                  to="/login"
                  className="inline-flex items-center justify-center px-4 py-2 text-sm font-semibold text-muted transition-all duration-200 hover:text-[#fb6900] active:scale-95"
                >
                  Đăng nhập
                </Link>
                <Link
                  to="/register"
                  className="inline-flex items-center justify-center rounded-full border border-[#fb6900]/30 bg-[#fb6900]/10 px-4 py-2 text-sm font-semibold text-[#fb6900] transition-all duration-200 hover:bg-[#fb6900] hover:text-white active:scale-95"
                >
                  Đăng ký
                </Link>
              </>
            )}
          </div>

          <RolePills />

          <CartButton count={displayedCartCount} />

          <NotificationBell />

          <button
            onClick={() => setMobileOpen((v) => !v)}
            className="flex h-9 w-9 items-center justify-center rounded-full text-orange-500 transition-colors hover:bg-white/5 lg:hidden"
            aria-label="Menu"
          >
            {mobileOpen ? '✕' : '☰'}
          </button>
        </div>
      </div>

      <MobileMenu
        open={mobileOpen}
        onClose={() => setMobileOpen(false)}
        onLogout={handleLogout}
        isAuthenticated={isAuthenticated}
      />
    </header>
  );
}

/* ---------- Sub-components nhỏ, chỉ dùng trong file này ---------- */

function SearchPill() {
  return (
    <div className="group hidden cursor-pointer items-center gap-2.5 rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-muted shadow-sm transition-all duration-200 hover:border-white/20 hover:bg-white/10 active:scale-95 md:flex">
      <span className="rounded-full bg-[#fb6900]/10 p-1 text-[#fb6900] transition-all duration-200 group-hover:scale-115 group-hover:bg-[#fb6900] group-hover:text-white">
        <Search size={16} />
      </span>
      <span className="transition-colors group-hover:text-orange-500">Tìm món...</span>
    </div>
  );
}

function CartButton({ count }: { count: number }) {
  return (
    <Link to="/cart" className="group inline-block">
      <ButtonPrimary
        size="sm"
        className="relative bg-orange-500 px-4 py-2 transition-all duration-300 hover:scale-105 hover:bg-orange-600"
      >
        <div className="flex items-center gap-2">
          <ShoppingCart size={18} className="text-white transition-transform group-hover:scale-110" />
          <span className="hidden font-medium text-white sm:inline">Giỏ hàng</span>
        </div>
        {count > 0 && (
          <span className="absolute -right-2 -top-2 flex animate-pulse items-center justify-center rounded-full bg-orange-500 px-1.5 text-[11px] font-bold text-white shadow-lg shadow-orange-500/30 ring-2 ring-white dark:ring-gray-800">
            {count > 99 ? '99+' : count}
          </span>
        )}
      </ButtonPrimary>
    </Link>
  );
}