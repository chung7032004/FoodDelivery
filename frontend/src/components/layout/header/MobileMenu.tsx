import { Link } from 'react-router-dom';
import { useLocation } from 'react-router-dom';
import { User, LogOut, Package } from 'lucide-react';
import { NotificationBell } from '@/components/notifications/NotificationBell';
import { NAV_LINKS, ROLE_LINKS } from './headerNavConfig';
import { useHeaderRoles } from './useHeaderRoles';

interface MobileMenuProps {
  open: boolean;
  onClose: () => void;
  onLogout: () => void;
  isAuthenticated: boolean;
}

export function MobileMenu({ open, onClose, onLogout, isAuthenticated }: MobileMenuProps) {
  const { pathname } = useLocation();
  const { user, isAdmin, canManageRestaurant, isShipper } = useHeaderRoles();

  if (!open) return null;

  const roleLinks = [
    isAdmin && ROLE_LINKS.admin,
    canManageRestaurant && ROLE_LINKS.restaurant,
    isShipper && ROLE_LINKS.shipper,
  ].filter(Boolean) as (typeof ROLE_LINKS)[keyof typeof ROLE_LINKS][];

  return (
    <div className="space-y-4 border-t border-border bg-surface px-6 py-4 lg:hidden">
      {/* Nav links */}
      <ul className="space-y-1">
        {NAV_LINKS.map((link) => {
          const isActive = pathname === link.to;
          return (
            <li key={link.to}>
              <Link
                to={link.to}
                onClick={onClose}
                className={`block w-full rounded-lg px-3 py-2 text-left text-sm font-medium transition-colors ${
                  isActive
                    ? 'bg-white/10 text-orange-500'
                    : 'text-muted hover:bg-white/5 hover:text-orange-500'
                }`}
              >
                {link.label}
              </Link>
            </li>
          );
        })}
      </ul>

      <div className="flex flex-col gap-2 border-t border-border pt-4">
        {isAuthenticated ? (
          <>
            <div className="flex items-center gap-2 px-3">
              <NotificationBell />
              <span className="text-sm font-medium text-black text-muted hover:text-orange-500">Thông báo</span>
            </div>

            <div className="flex items-center gap-2.5 px-3 py-2 text-sm font-medium text-black-500 text-muted hover:text-orange-500">
              <span className="flex h-7 w-7 items-center justify-center rounded-full bg-orange-500/20 text-xs font-bold text-orange-500">
                {user?.name?.charAt(0).toUpperCase() ?? <User size={14} />}
              </span>
              {user?.name ?? 'Tài khoản'}
            </div>

            <MobileLink to="/account" onClose={onClose} icon={<User size={16} />}>
              Xem tài khoản
            </MobileLink>
            <MobileLink to="/account?tab=orders" onClose={onClose} icon={<Package size={16} />}>
              Đơn hàng của tôi
            </MobileLink>

            {roleLinks.map(({ to, label, icon: Icon, itemClass }) => (
              <Link
                key={to}
                to={to}
                onClick={onClose}
                className={`flex w-full items-center gap-2.5 rounded-lg px-4 py-2 text-left text-sm font-semibold ${itemClass}`}
              >
                <Icon size={16} /> {label}
              </Link>
            ))}

            <button
              onClick={onLogout}
              className="flex w-full items-center gap-2.5 rounded-lg px-4 py-2 text-left text-sm font-medium text-red-500 hover:bg-red-500/10"
            >
              <LogOut size={16} /> Đăng xuất
            </button>
          </>
        ) : (
          <>
            <Link
              to="/login"
              onClick={onClose}
              className="w-full rounded-full border border-white/10 px-4 py-2 text-center text-sm font-medium text-muted transition-colors hover:text-orange-500"
            >
              Đăng nhập
            </Link>
            <Link
              to="/register"
              onClick={onClose}
              className="w-full rounded-full bg-brand px-4 py-2 text-center text-sm font-semibold text-black transition-opacity hover:text-orange-500"
            >
              Đăng ký
            </Link>
          </>
        )}
      </div>
    </div>
  );
}

function MobileLink({
  to,
  onClose,
  icon,
  children,
}: {
  to: string;
  onClose: () => void;
  icon: React.ReactNode;
  children: React.ReactNode;
}) {
  return (
    <Link
      to={to}
      onClick={onClose}
      className="flex w-full items-center gap-2.5 px-4 py-2 text-left text-sm font-medium text-muted hover:text-orange-500"
    >
      {icon} {children}
    </Link>
  );
}