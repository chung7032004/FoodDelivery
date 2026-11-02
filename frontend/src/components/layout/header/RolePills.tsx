import { Link } from 'react-router-dom';
import { ROLE_LINKS } from './headerNavConfig';
import { useHeaderRoles } from './useHeaderRoles';

export function RolePills() {
  const { isAdmin, canManageRestaurant, isShipper } = useHeaderRoles();

  const visible = [
    isAdmin && ROLE_LINKS.admin,
    canManageRestaurant && ROLE_LINKS.restaurant,
    isShipper && ROLE_LINKS.shipper,
  ].filter(Boolean) as (typeof ROLE_LINKS)[keyof typeof ROLE_LINKS][];

  if (!visible.length) return null;

  return (
    <>
      {visible.map(({ to, label, title, icon: Icon, pillClass }) => (
        <Link
          key={to}
          to={to}
          title={title}
          className={`hidden items-center gap-2 rounded-full border px-3 py-2 text-sm font-semibold transition lg:flex ${pillClass}`}
        >
          <Icon size={16} />
          <span>{label}</span>
        </Link>
      ))}
    </>
  );
}