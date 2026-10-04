// src/pages/AccountPage.tsx
import { useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { HeaderNav } from '../components/layout/HeaderNav';
import { Footer } from '../components/layout/Footer';
import { AddressesTab as AddressesTabView } from '@/components/account/AddressesTab';
import { OrdersTab as OrdersTabView } from '@/components/account/OrdersTab';
import { ProfileTab as ProfileTabView } from '@/components/account/ProfileTab';
import { SecurityTab as SecurityTabView } from '@/components/account/SecurityTab';
import Spinner from '@/components/ui/Spinner';
import { useAuth } from '@/contexts/AuthContext';
import { accountService, type AccountResponse } from '@/services/account.service';
import { getImageUrl } from '@/utils/image';
import {
  User as UserIcon,
  Package,
  MapPin,
  ShieldCheck,
  LogOut,
} from 'lucide-react';

type Tab = 'profile' | 'orders' | 'addresses' | 'security';

const TABS: { key: Tab; label: string; icon: typeof UserIcon }[] = [
  { key: 'profile',   label: 'Thông tin', icon: UserIcon },
  { key: 'orders',    label: 'Đơn hàng',  icon: Package },
  { key: 'addresses', label: 'Địa chỉ',   icon: MapPin },
  { key: 'security',  label: 'Bảo mật',   icon: ShieldCheck },
];

export function AccountPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const [activeTab, setActiveTab] = useState<Tab>(() => {
    const requestedTab = searchParams.get('tab');
    return TABS.some((tab) => tab.key === requestedTab) ? requestedTab as Tab : 'profile';
  });
  const { user: authUser, logout } = useAuth();
  const [user, setUser] = useState<AccountResponse | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let isMounted = true;

    accountService.getMe()
      .then((response) => {
        if (!isMounted) return;
        if (response.isSuccess && response.data) {
          setUser(response.data);
        } else {
          setError(response.message ?? 'Không thể tải thông tin tài khoản.');
        }
      })
      .catch((requestError: { message?: string }) => {
        if (isMounted) setError(requestError.message ?? 'Không thể tải thông tin tài khoản.');
      })
      .finally(() => {
        if (isMounted) setIsLoading(false);
      });

    return () => { isMounted = false; };
  }, []);

  const handleLogout = async () => {
    await logout();
  };

  const handleTabChange = (tab: Tab) => {
    setActiveTab(tab);
    const nextParams = new URLSearchParams(searchParams);
    if (tab === 'profile') nextParams.delete('tab');
    else nextParams.set('tab', tab);
    setSearchParams(nextParams, { replace: true });
  };

  const displayName = user?.fullName ?? authUser?.name ?? 'Tài khoản';
  const displayEmail = user?.email ?? authUser?.email ?? '';
  const displayRole = user && user.roles.length > 0 ? user.roles.join(', ') : 'Khách hàng';

  return (
    <div className="min-h-screen bg-surface font-sans">
      <HeaderNav />

      <main className="mx-auto max-w-5xl px-6 pb-16 lg:px-8">
        {/* ---------- Profile banner ---------- */}
        <div className="relative mt-8 overflow-hidden rounded-3xl border border-border">
          <div className="h-28 bg-gradient-to-br from-brand/40 via-orange-600/20 to-surface sm:h-32" />

          <div className="relative bg-white/[0.02] px-6 pb-6 sm:px-8">
            <div className="flex flex-col gap-4 sm:flex-row sm:items-end sm:justify-between">
              <div className="flex items-end gap-4 -mt-10 sm:-mt-12">
                {user?.avatarUrl ? (
                  <img
                    src={getImageUrl(user.avatarUrl)}
                    alt={displayName}
                    className="h-20 w-20 shrink-0 rounded-2xl object-cover ring-4 ring-surface sm:h-24 sm:w-24"
                  />
                ) : (
                  <div className="flex h-20 w-20 shrink-0 items-center justify-center rounded-2xl bg-brand text-3xl font-bold text-black ring-4 ring-surface sm:h-24 sm:w-24">
                    {displayName.charAt(0).toUpperCase()}
                  </div>
                )}
                <div className="pb-1">
                  <h1 className="font-display text-xl font-bold text-black sm:text-2xl">{displayName}</h1>
                  <p className="text-sm text-muted">{displayEmail}</p>
                  <span className="mt-1.5 inline-block rounded-full bg-brand/10 px-2.5 py-0.5 text-xs font-semibold text-brand">
                    {displayRole}
                  </span>
                </div>
              </div>

              <button
                onClick={handleLogout}
                className="flex shrink-0 items-center justify-center gap-1.5 self-start rounded-xl border border-white/10 px-4 py-2 text-sm font-medium text-red-400 transition-colors hover:bg-red-500/10 sm:self-auto"
              >
                <LogOut size={15} />
                Đăng xuất
              </button>
            </div>
          </div>
        </div>

        {/* ---------- Segmented tab bar ---------- */}
        <nav className="mt-6 flex gap-1 overflow-x-auto rounded-2xl border border-border bg-white/[0.02] p-1.5">
          {TABS.map((tab) => {
            const Icon = tab.icon;
            const isActive = activeTab === tab.key;
            return (
              <button
                key={tab.key}
                onClick={() => handleTabChange(tab.key)}
                className={`
                  flex shrink-0 items-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold transition-colors
                  ${isActive
                    ? 'bg-brand text-black shadow-[0_4px_16px_rgba(255,69,0,0.35)]'
                    : 'text-muted hover:bg-white/5 hover:text-black'
                  }
                `}
              >
                <Icon size={16} />
                {tab.label}
              </button>
            );
          })}
        </nav>

        {/* ---------- Tab content ---------- */}
        <div className="mt-6 rounded-2xl border border-border bg-white/[0.02] p-6 sm:p-8">
          {isLoading && (
            <div className="flex min-h-48 items-center justify-center"><Spinner size="lg" /></div>
          )}
          {!isLoading && error && (
            <div className="rounded-xl border border-red-400/20 bg-red-400/10 p-4 text-sm text-red-300">
              {error}
            </div>
          )}
          {!isLoading && !error && user && activeTab === 'profile' && <ProfileTabView user={user} onUpdated={setUser} />}
          {activeTab === 'orders' && <OrdersTabView />}
          {activeTab === 'addresses' && <AddressesTabView />}
          {activeTab === 'security' && <SecurityTabView />}
        </div>
      </main>

      <Footer />
    </div>
  );
}

