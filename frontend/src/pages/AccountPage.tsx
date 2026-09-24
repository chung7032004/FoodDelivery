// src/pages/AccountPage.tsx
import { useEffect, useState } from 'react';
import { HeaderNav } from '../components/layout/HeaderNav';
import { Footer } from '../components/layout/Footer';
import { ButtonPrimary } from '../components/ui';
import Spinner from '@/components/ui/Spinner';
import { useAuth } from '@/contexts/AuthContext';
import { accountService, AccountResponse } from '@/services/account.service';
import { getImageUrl } from '@/utils/image';
import {
  User as UserIcon,
  Package,
  MapPin,
  ShieldCheck,
  LogOut,
  Pencil,
  Plus,
  ChevronRight,
} from 'lucide-react';
import { authService } from '@/services/auth.service';

type Tab = 'profile' | 'orders' | 'addresses' | 'security';

interface Order {
  id: string;
  date: string;
  items: string;
  total: string;
  status: 'delivered' | 'delivering' | 'cancelled';
}

const MOCK_ORDERS: Order[] = [
  { id: '#EF10234', date: '02/07/2026', items: 'Bún chả Hà Nội, Trà đá x2', total: '85.000đ', status: 'delivered' },
  { id: '#EF10221', date: '28/06/2026', items: 'Pizza hải sản size L', total: '229.000đ', status: 'delivered' },
  { id: '#EF10198', date: '25/06/2026', items: 'Gà rán 6 miếng, Coca 1.5L', total: '159.000đ', status: 'cancelled' },
];

const STATUS_CONFIG: Record<Order['status'], { label: string; className: string; barClassName: string }> = {
  delivered:  { label: 'Đã giao',   className: 'bg-emerald-500/10 text-emerald-400', barClassName: 'bg-emerald-400' },
  delivering: { label: 'Đang giao', className: 'bg-brand/10 text-brand',             barClassName: 'bg-brand' },
  cancelled:  { label: 'Đã huỷ',    className: 'bg-white/10 text-muted',             barClassName: 'bg-white/20' },
};

const TABS: { key: Tab; label: string; icon: typeof UserIcon }[] = [
  { key: 'profile',   label: 'Thông tin', icon: UserIcon },
  { key: 'orders',    label: 'Đơn hàng',  icon: Package },
  { key: 'addresses', label: 'Địa chỉ',   icon: MapPin },
  { key: 'security',  label: 'Bảo mật',   icon: ShieldCheck },
];

export function AccountPage() {
  const [activeTab, setActiveTab] = useState<Tab>('profile');
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

  const displayName = user?.fullName ?? authUser?.name ?? 'Tài khoản';
  const displayEmail = user?.email ?? authUser?.email ?? '';
  const displayRole = user && user.roles.length > 0 ? user.roles.join(', ') : 'Khách hàng';

  return (
    <div className="min-h-screen bg-surface font-sans">
      <HeaderNav cartCount={0} />

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
                onClick={() => setActiveTab(tab.key)}
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
          {!isLoading && !error && user && activeTab === 'profile' && <ProfileTab user={user} />}
          {activeTab === 'orders' && <OrdersTab orders={MOCK_ORDERS} />}
          {activeTab === 'addresses' && <AddressesTab />}
          {activeTab === 'security' && <SecurityTab />}
        </div>
      </main>

      <Footer />
    </div>
  );
}

/* ---------- Tab: Thông tin cá nhân ---------- */
function ProfileTab({ user }: { user: AccountResponse }) {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h2 className="font-display text-lg font-bold text-black">Thông tin cá nhân</h2>
        <button className="flex items-center gap-1.5 text-sm font-medium text-brand hover:underline">
          <Pencil size={14} />
          Chỉnh sửa
        </button>
      </div>

      <div className="grid gap-4 sm:grid-cols-2">
        <Field label="Họ và tên" value={user.fullName ?? 'Chưa cập nhật'} />
        <Field label="Email" value={user.email ?? 'Chưa cập nhật'} />
        <Field label="Số điện thoại" value={user.phone ?? 'Chưa cập nhật'} />
        <Field label="Vai trò" value={user.roles.length > 0 ? user.roles.join(', ') : 'Khách hàng'} />
      </div>

      <ButtonPrimary size="sm">Cập nhật thông tin</ButtonPrimary>
    </div>
  );
}

function Field({ label, value }: { label: string; value: string }) {
  return (
    <div className="space-y-1.5">
      <label className="text-xs font-medium text-muted">{label}</label>
      <div className="rounded-xl border border-white/10 bg-white/5 px-4 py-2.5 text-sm text-orange-500 font-medium">
        {value}
      </div>
    </div>
  );
}

/* ---------- Tab: Lịch sử đơn hàng ---------- */
function OrdersTab({ orders }: { orders: Order[] }) {
  if (orders.length === 0) {
    return (
      <div className="py-16 text-center">
        <Package size={32} className="mx-auto mb-3 text-muted" />
        <p className="text-muted">Bạn chưa có đơn hàng nào.</p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <h2 className="font-display text-lg font-bold text-black">Lịch sử đơn hàng</h2>

      <div className="space-y-3">
        {orders.map((order) => {
          const status = STATUS_CONFIG[order.status];
          return (
            <div
              key={order.id}
              className="flex items-stretch gap-4 overflow-hidden rounded-xl border border-white/10 bg-white/5 transition-colors hover:bg-white/[0.07]"
            >
              <span className={`w-1 shrink-0 ${status.barClassName}`} />

              <div className="flex flex-1 flex-col gap-3 py-4 pr-4 sm:flex-row sm:items-center sm:justify-between">
                <div className="space-y-1">
                  <div className="flex items-center gap-2">
                    <span className="font-semibold text-black">{order.id}</span>
                    <span className={`rounded-full px-2.5 py-0.5 text-xs font-semibold ${status.className}`}>
                      {status.label}
                    </span>
                  </div>
                  <p className="text-sm text-muted">{order.items}</p>
                  <p className="text-xs text-muted">{order.date}</p>
                </div>

                <div className="flex items-center gap-4 sm:flex-col sm:items-end sm:gap-1.5">
                  <span className="font-semibold text-black">{order.total}</span>
                  <button className="flex items-center gap-0.5 text-sm font-medium text-brand hover:underline">
                    Xem chi tiết
                    <ChevronRight size={14} />
                  </button>
                </div>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}

/* ---------- Tab: Địa chỉ ---------- */
function AddressesTab() {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h2 className="font-display text-lg font-bold text-black">Địa chỉ giao hàng</h2>
        <ButtonPrimary size="sm">
          <span className="flex items-center gap-1.5">
            <Plus size={15} />
            Thêm địa chỉ
          </span>
        </ButtonPrimary>
      </div>

      <div className="flex items-start gap-3 rounded-xl border border-white/10 bg-white/5 p-4">
        <span className="mt-0.5 flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-brand/10 text-brand">
          <MapPin size={16} />
        </span>
        <div className="flex flex-1 items-start justify-between gap-3">
          <div>
            <div className="mb-1 flex items-center gap-2">
              <span className="font-semibold text-black">Nhà riêng</span>
              <span className="rounded-full bg-brand/10 px-2 py-0.5 text-xs font-semibold text-brand">
                Mặc định
              </span>
            </div>
            <p className="text-sm text-muted">
              123 Đường Lê Lợi, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh
            </p>
          </div>
          <button className="shrink-0 text-sm font-medium text-muted hover:text-black">Sửa</button>
        </div>
      </div>
    </div>
  );
}

/* ---------- Tab: Bảo mật ---------- */
function SecurityTab() {
  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmNewPassword, setConfirmNewPassword] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess(false);

    // Validation
    if (!oldPassword || !newPassword || !confirmNewPassword) {
      setError('Vui lòng điền đầy đủ các trường');
      return;
    }

    if (newPassword.length < 8) {
      setError('Mật khẩu mới phải có ít nhất 8 ký tự');
      return;
    }

    if (newPassword !== confirmNewPassword) {
      setError('Mật khẩu mới và xác nhận không khớp');
      return;
    }

    if (oldPassword === newPassword) {
      setError('Mật khẩu mới phải khác mật khẩu hiện tại');
      return;
    }

    setIsLoading(true);

    try {
      await authService.changePassword({
        oldPassword,
        newPassword,
        confirmNewPassword
      });
      await new Promise(resolve => setTimeout(resolve, 1500));
      
      setSuccess(true);
      setOldPassword('');
      setNewPassword('');
      setConfirmNewPassword('');
    } catch (err) {
      setError('Có lỗi xảy ra, vui lòng thử lại');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      <h2 className="font-display text-lg font-bold text-black">Đổi mật khẩu</h2>

      <div className="max-w-sm space-y-4">
        <PasswordInput
          label="Mật khẩu hiện tại"
          value={oldPassword}
          onChange={(e) => setOldPassword(e.target.value)}
          disabled={isLoading}
        />
        <PasswordInput
          label="Mật khẩu mới"
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
          disabled={isLoading}
        />
        <PasswordInput
          label="Nhập lại mật khẩu mới"
          value={confirmNewPassword}
          onChange={(e) => setConfirmNewPassword(e.target.value)}
          disabled={isLoading}
        />

        {error && (
          <div className="text-sm text-red-500 bg-red-50/10 px-3 py-2 rounded-lg">
            {error}
          </div>
        )}

        {success && (
          <div className="text-sm text-green-600 bg-green-50/10 px-3 py-2 rounded-lg">
            Cập nhật mật khẩu thành công!
          </div>
        )}
      </div>

      <button
        type="submit"
        className="inline-flex items-center justify-center px-4 py-2 text-sm font-medium rounded-xl bg-brand text-orange-500 hover:bg-brand-dark disabled:opacity-50 disabled:cursor-not-allowed"
        disabled={isLoading}
      >
        {isLoading ? 'Đang xử lý...' : 'Cập nhật mật khẩu'}
      </button>
    </form>
  );
}

// Updated PasswordInput with proper props
function PasswordInput({ 
  label, 
  value, 
  onChange, 
  disabled = false 
}: { 
  label: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  disabled?: boolean;
}) {
  return (
    <div className="space-y-1.5">
      <label className="text-xs font-medium text-muted">{label}</label>
      <input
        type="password"
        value={value}
        onChange={onChange}
        disabled={disabled}
        className="w-full rounded-xl border border-white/10 bg-white/5 px-4 py-2.5 text-sm text-black outline-none transition-colors placeholder:text-muted focus:border-brand/50 disabled:opacity-50 disabled:cursor-not-allowed"
        placeholder="••••••••"
      />
    </div>
  );
}