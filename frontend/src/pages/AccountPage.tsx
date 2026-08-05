// src/pages/AccountPage.tsx
import { useState } from 'react';
import { HeaderNav } from '../components/layout/HeaderNav';
import { Footer } from '../components/layout/Footer';
import { ButtonPrimary } from '../components/ui';

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

const STATUS_CONFIG: Record<Order['status'], { label: string; className: string }> = {
  delivered:  { label: 'Đã giao',    className: 'bg-emerald-500/10 text-emerald-400' },
  delivering: { label: 'Đang giao',  className: 'bg-brand/10 text-brand' },
  cancelled:  { label: 'Đã huỷ',     className: 'bg-white/10 text-muted' },
};

const TABS: { key: Tab; label: string }[] = [
  { key: 'profile',   label: 'Thông tin' },
  { key: 'orders',    label: 'Đơn hàng' },
  { key: 'addresses', label: 'Địa chỉ' },
  { key: 'security',  label: 'Bảo mật' },
];

export function AccountPage() {
  const [activeTab, setActiveTab] = useState<Tab>('profile');

  // TODO: thay bằng useAuth() thật khi có AuthContext
  const user = {
    name: 'Nguyễn Văn A',
    email: 'nguyenvana@gmail.com',
    phone: '0912 345 678',
  };

  const handleLogout = () => {
    // TODO: gọi authService.logout()
    console.log('Đăng xuất');
  };

  return (
    <div className="min-h-screen bg-surface font-sans">
      <HeaderNav cartCount={0} isAuthenticated userName={user.name} onLogout={handleLogout} />

      <main className="mx-auto max-w-6xl px-6 py-10 lg:px-8">
        {/* Header trang */}
        <div className="mb-8 flex items-center gap-4">
          <div className="flex h-16 w-16 items-center justify-center rounded-full bg-brand/20 text-2xl font-bold text-brand">
            {user.name.charAt(0)}
          </div>
          <div>
            <h1 className="font-display text-2xl font-bold text-white">{user.name}</h1>
            <p className="text-sm text-muted">{user.email}</p>
          </div>
        </div>

        <div className="grid gap-8 lg:grid-cols-[220px_1fr]">
          {/* Sidebar tabs */}
          <nav className="flex gap-2 overflow-x-auto lg:flex-col lg:overflow-visible">
            {TABS.map((tab) => (
              <button
                key={tab.key}
                onClick={() => setActiveTab(tab.key)}
                className={`
                  shrink-0 rounded-xl px-4 py-2.5 text-left text-sm font-medium transition-colors
                  ${activeTab === tab.key
                    ? 'bg-white/10 text-white'
                    : 'text-muted hover:bg-white/5 hover:text-white'
                  }
                `}
              >
                {tab.label}
              </button>
            ))}

            <button
              onClick={handleLogout}
              className="mt-0 shrink-0 rounded-xl px-4 py-2.5 text-left text-sm font-medium text-red-400 hover:bg-red-500/10 lg:mt-4"
            >
              Đăng xuất
            </button>
          </nav>

          {/* Nội dung tab */}
          <div className="rounded-2xl border border-border bg-white/[0.02] p-6">
            {activeTab === 'profile' && <ProfileTab user={user} />}
            {activeTab === 'orders' && <OrdersTab orders={MOCK_ORDERS} />}
            {activeTab === 'addresses' && <AddressesTab />}
            {activeTab === 'security' && <SecurityTab />}
          </div>
        </div>
      </main>

      <Footer />
    </div>
  );
}

/* ---------- Tab: Thông tin cá nhân ---------- */
function ProfileTab({ user }: { user: { name: string; email: string; phone: string } }) {
  return (
    <div className="space-y-6">
      <h2 className="font-display text-lg font-bold text-white">Thông tin cá nhân</h2>

      <div className="grid gap-4 sm:grid-cols-2">
        <Field label="Họ và tên" value={user.name} />
        <Field label="Email" value={user.email} />
        <Field label="Số điện thoại" value={user.phone} />
        <Field label="Ngày tham gia" value="15/03/2025" />
      </div>

      <ButtonPrimary size="sm">Cập nhật thông tin</ButtonPrimary>
    </div>
  );
}

function Field({ label, value }: { label: string; value: string }) {
  return (
    <div className="space-y-1.5">
      <label className="text-xs font-medium text-muted">{label}</label>
      <div className="rounded-xl border border-white/10 bg-white/5 px-4 py-2.5 text-sm text-white">
        {value}
      </div>
    </div>
  );
}

/* ---------- Tab: Lịch sử đơn hàng ---------- */
function OrdersTab({ orders }: { orders: Order[] }) {
  if (orders.length === 0) {
    return (
      <div className="py-12 text-center">
        <p className="text-muted">Bạn chưa có đơn hàng nào.</p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <h2 className="font-display text-lg font-bold text-white">Lịch sử đơn hàng</h2>

      <div className="space-y-3">
        {orders.map((order) => {
          const status = STATUS_CONFIG[order.status];
          return (
            <div
              key={order.id}
              className="flex flex-col gap-3 rounded-xl border border-white/10 bg-white/5 p-4 sm:flex-row sm:items-center sm:justify-between"
            >
              <div className="space-y-1">
                <div className="flex items-center gap-2">
                  <span className="font-semibold text-white">{order.id}</span>
                  <span className={`rounded-full px-2.5 py-0.5 text-xs font-semibold ${status.className}`}>
                    {status.label}
                  </span>
                </div>
                <p className="text-sm text-muted">{order.items}</p>
                <p className="text-xs text-muted">{order.date}</p>
              </div>

              <div className="flex items-center gap-3 sm:flex-col sm:items-end">
                <span className="font-semibold text-white">{order.total}</span>
                <button className="text-sm font-medium text-brand hover:underline">Xem chi tiết</button>
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
        <h2 className="font-display text-lg font-bold text-white">Địa chỉ giao hàng</h2>
        <ButtonPrimary size="sm">+ Thêm địa chỉ</ButtonPrimary>
      </div>

      <div className="rounded-xl border border-white/10 bg-white/5 p-4">
        <div className="flex items-start justify-between">
          <div>
            <div className="mb-1 flex items-center gap-2">
              <span className="font-semibold text-white">Nhà riêng</span>
              <span className="rounded-full bg-brand/10 px-2 py-0.5 text-xs font-semibold text-brand">
                Mặc định
              </span>
            </div>
            <p className="text-sm text-muted">
              123 Đường Lê Lợi, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh
            </p>
          </div>
          <button className="text-sm font-medium text-muted hover:text-white">Sửa</button>
        </div>
      </div>
    </div>
  );
}

/* ---------- Tab: Bảo mật ---------- */
function SecurityTab() {
  return (
    <div className="space-y-6">
      <h2 className="font-display text-lg font-bold text-white">Đổi mật khẩu</h2>

      <div className="max-w-sm space-y-4">
        <Field label="Mật khẩu hiện tại" value="••••••••" />
        <Field label="Mật khẩu mới" value="" />
        <Field label="Nhập lại mật khẩu mới" value="" />
      </div>

      <ButtonPrimary size="sm">Cập nhật mật khẩu</ButtonPrimary>
    </div>
  );
}