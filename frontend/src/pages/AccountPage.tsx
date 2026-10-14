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
import { accountService, type AccountResponse, type UserRoleRestriction } from '@/services/account.service';
import { getImageUrl } from '@/utils/image';
import { toast } from 'sonner';
import {
  User as UserIcon,
  Package,
  MapPin,
  ShieldCheck,
  LogOut,
  MessageSquareWarning,
} from 'lucide-react';

type Tab = 'profile' | 'orders' | 'addresses' | 'security' | 'appeals';

const TABS: { key: Tab; label: string; icon: typeof UserIcon }[] = [
  { key: 'profile',   label: 'Thông tin', icon: UserIcon },
  { key: 'orders',    label: 'Đơn hàng',  icon: Package },
  { key: 'addresses', label: 'Địa chỉ',   icon: MapPin },
  { key: 'security',  label: 'Bảo mật',   icon: ShieldCheck },
  { key: 'appeals',   label: 'Khiếu nại quyền', icon: MessageSquareWarning },
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
          {activeTab === 'appeals' && <RoleRestrictionsTab />}
        </div>
      </main>

      <Footer />
    </div>
  );
}

function RoleRestrictionsTab() {
  const [restrictions, setRestrictions] = useState<UserRoleRestriction[]>([]);
  const [reasons, setReasons] = useState<Record<string, string>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [submittingId, setSubmittingId] = useState<string | null>(null);

  async function loadRestrictions() {
    setLoading(true);
    setError('');
    try {
      setRestrictions(await accountService.getMyRoleRestrictions());
    } catch {
      setError('Không tải được trạng thái thu hồi quyền.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void loadRestrictions();
  }, []);

  async function submitAppeal(event: React.FormEvent<HTMLFormElement>, restriction: UserRoleRestriction) {
    event.preventDefault();
    const reason = reasons[restriction.restrictionId]?.trim() ?? '';
    if (reason.length < 10) {
      setError('Nội dung khiếu nại cần có ít nhất 10 ký tự.');
      return;
    }
    setSubmittingId(restriction.restrictionId);
    setError('');
    try {
      await accountService.submitRoleAppeal(restriction.roleName, reason);
      setReasons((current) => ({ ...current, [restriction.restrictionId]: '' }));
      await loadRestrictions();
      toast.success('Đã gửi khiếu nại đến admin.');
    } catch {
      setError('Không gửi được khiếu nại. Vui lòng thử lại.');
    } finally {
      setSubmittingId(null);
    }
  }

  if (loading) return <div className="flex min-h-40 items-center justify-center"><Spinner size="lg" /></div>;
  if (error && restrictions.length === 0) {
    return <div role="alert" className="space-y-3 text-sm text-red-700"><p>{error}</p><button type="button" onClick={() => void loadRestrictions()} className="font-semibold underline">Thử lại</button></div>;
  }
  if (restrictions.length === 0) {
    return <div className="py-8 text-center"><ShieldCheck className="mx-auto text-emerald-600" size={32} /><p className="mt-3 font-semibold text-slate-800">Không có quyền nào đang bị thu hồi</p><p className="mt-1 text-sm text-slate-500">Nếu admin thu hồi một quyền, thông tin và biểu mẫu khiếu nại sẽ hiển thị tại đây.</p></div>;
  }

  return (
    <div className="space-y-5">
      <div>
        <h2 className="text-lg font-semibold text-slate-900">Quyền bị thu hồi</h2>
        <p className="mt-1 text-sm text-slate-600">Bạn vẫn có thể sử dụng tài khoản khách hàng. Gửi khiếu nại nếu cho rằng quyết định thu hồi có nhầm lẫn.</p>
      </div>
      {error && <p role="alert" className="text-sm text-red-700">{error}</p>}
      {restrictions.map((restriction) => {
        const pending = restriction.appealStatus === 'Pending';
        return (
          <article key={restriction.restrictionId} className="rounded-xl border border-amber-200 bg-amber-50/60 p-4 sm:p-5">
            <h3 className="font-semibold text-slate-900">Quyền {restriction.roleName === 'Shipper' ? 'Shipper' : 'chủ nhà hàng'} đang bị thu hồi</h3>
            <p className="mt-2 text-sm text-slate-700"><span className="font-medium">Lý do:</span> {restriction.reason}</p>
            {restriction.appealResponse && <p className="mt-3 border-l-2 border-red-300 pl-3 text-sm text-slate-700"><span className="font-medium">Phản hồi của admin:</span> {restriction.appealResponse}</p>}
            {pending ? (
              <p className="mt-4 rounded-lg bg-white px-3 py-2 text-sm font-medium text-amber-800">Khiếu nại đã được gửi, đang chờ admin xem xét.</p>
            ) : (
              <form onSubmit={(event) => void submitAppeal(event, restriction)} className="mt-4 space-y-3">
                {restriction.appealStatus === 'Rejected' && <p className="text-sm font-medium text-red-700">Khiếu nại trước đã bị từ chối. Bạn có thể gửi khiếu nại mới.</p>}
                <textarea
                  required
                  minLength={10}
                  maxLength={1000}
                  rows={4}
                  value={reasons[restriction.restrictionId] ?? ''}
                  onChange={(event) => setReasons((current) => ({ ...current, [restriction.restrictionId]: event.target.value }))}
                  placeholder="Trình bày lý do bạn yêu cầu khôi phục quyền (10–1000 ký tự)..."
                  className="w-full rounded-lg border border-slate-200 bg-white p-3 text-sm outline-none focus:border-orange-500"
                />
                <button type="submit" disabled={submittingId !== null || (reasons[restriction.restrictionId]?.trim().length ?? 0) < 10} className="rounded-lg bg-orange-600 px-4 py-2 text-sm font-semibold text-white hover:bg-orange-700 disabled:cursor-not-allowed disabled:opacity-50">
                  {submittingId === restriction.restrictionId ? 'Đang gửi...' : 'Gửi khiếu nại'}
                </button>
              </form>
            )}
          </article>
        );
      })}
    </div>
  );
}
