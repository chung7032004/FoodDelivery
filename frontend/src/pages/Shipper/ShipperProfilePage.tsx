import { useEffect, useState } from 'react';
import type { ReactNode } from 'react';
import { MapPin, UserRound } from 'lucide-react';
import { AddressesTab } from '@/components/account/AddressesTab';
import { ProfileTab } from '@/components/account/ProfileTab';
import { PageHeader, Panel } from '@/components/admin/AdminUI';
import { accountService, type AccountResponse } from '@/services/account.service';
import { ShipperLayout } from './ShipperLayout';

type ProfileTabKey = 'account' | 'addresses';

export default function ShipperProfilePage() {
  const [activeTab, setActiveTab] = useState<ProfileTabKey>('account');
  const [account, setAccount] = useState<AccountResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    let active = true;
    accountService.getMe()
      .then((result) => {
        if (!active) return;
        if (result.isSuccess && result.data) setAccount(result.data);
        else setError(result.message ?? 'Không thể tải thông tin tài khoản.');
      })
      .catch(() => {
        if (active) setError('Không thể tải thông tin tài khoản.');
      })
      .finally(() => {
        if (active) setLoading(false);
      });

    return () => { active = false; };
  }, []);

  return (
    <ShipperLayout>
      <PageHeader
        eyebrow="Thiết lập tài khoản"
        title="Hồ sơ shipper"
        description="Cập nhật thông tin liên hệ và địa chỉ của bạn."
      />
      {error && (
        <p role="alert" className="mb-4 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {error}
        </p>
      )}
      <Panel title="Thông tin tài khoản" subtitle="Thông tin này được dùng để liên hệ khi giao hàng.">
        <div className="flex gap-2 border-b border-slate-200 px-4 pt-3">
          <Tab active={activeTab === 'account'} onClick={() => setActiveTab('account')}>
            <UserRound size={16} />Thông tin cá nhân
          </Tab>
          <Tab active={activeTab === 'addresses'} onClick={() => setActiveTab('addresses')}>
            <MapPin size={16} />Địa chỉ
          </Tab>
        </div>
        <div className="p-5 sm:p-6">
          {loading ? (
            <p className="py-8 text-center text-sm text-slate-500">Đang tải hồ sơ...</p>
          ) : activeTab === 'account' ? (
            account ? (
              <ProfileTab user={account} onUpdated={setAccount} />
            ) : (
              <p className="py-8 text-center text-sm text-slate-500">Không có thông tin tài khoản.</p>
            )
          ) : (
            <>
              <p className="mb-5 rounded-xl border border-sky-200 bg-sky-50 px-4 py-3 text-sm text-sky-900">
                Quản lý địa chỉ cá nhân đã lưu tại đây. Hiện hệ thống chưa giới hạn đơn theo khoảng cách hoặc khu vực của shipper.
              </p>
              <AddressesTab />
            </>
          )}
        </div>
      </Panel>
    </ShipperLayout>
  );
}

function Tab({ active, onClick, children }: { active: boolean; onClick: () => void; children: ReactNode }) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={`flex items-center gap-2 whitespace-nowrap border-b-2 px-3 py-3 text-sm font-bold ${
        active ? 'border-emerald-600 text-emerald-700' : 'border-transparent text-slate-500 hover:text-slate-700'
      }`}
    >
      {children}
    </button>
  );
}
