import { Link, useLocation } from 'react-router-dom';
import { NAV_LINKS } from './headerNavConfig';

export function DesktopNav() {
  const { pathname } = useLocation();

  return (
    <nav className="hidden lg:block">
      <ul className="flex gap-1">
        {NAV_LINKS.map((link) => {
          const isActive = pathname === link.to;
          return (
            <li key={link.to}>
              <Link
                to={link.to}
                className={`relative inline-block rounded-full px-5 py-2 text-sm font-bold transition-all duration-200 ${
                  isActive
                    ? 'bg-orange/10 text-orange-500'
                    : 'text-muted hover:bg-orange/5 hover:text-orange-500'
                }`}
              >
                {link.label}
                {isActive && (
                  <span className="absolute bottom-1 left-1/2 h-0.5 w-4 -translate-x-1/2 rounded-full bg-brand" />
                )}
              </Link>
            </li>
          );
        })}
      </ul>
    </nav>
  );
}