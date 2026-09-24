import { useEffect, useState } from 'react';
import { Lock, Search, Unlock } from 'lucide-react';
import { AdminLayout } from './AdminLayout';
import { AdminUser, adminService, PagedResult } from '@/services/admin.service';

const money = new Intl.NumberFormat('vi-VN', {
  style: 'currency',
  currency: 'VND',
  maximumFractionDigits: 0,
});

export default function AdminUsersPage() {
  const [result, setResult] = useState<PagedResult<AdminUser> | null>(null);
  const [search, setSearch] = useState('');
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [processingId, setProcessingId] = useState<string | null>(null);
  const [error, setError] = useState('');

  // Tải danh sách người dùng
  const loadUsers = async (isMounted = true) => {
    setLoading(true);
    setError('');
    try {
      const data = await adminService.getUsers({
        searchQuery: search.trim() || undefined,
        page,
        pageSize: 10,
      });
      if (isMounted) setResult(data);
    } catch {
      if (isMounted) setError('Không thể tải danh sách tài khoản.');
    } finally {
      if (isMounted) setLoading(false);
    }
  };

  useEffect(() => {
    let isSubscribed = true;
    void loadUsers(isSubscribed);
    return () => {
      isSubscribed = false;
    };
  }, [search, page]);

  // Xử lý Khóa / Mở khóa tài khoản
  async function handleToggleUser(user: AdminUser) {
    const actionText = user.isActive ? 'KHÓA' : 'MỞ KHÓA';
    if (!window.confirm(`Bạn có chắc chắn muốn ${actionText} tài khoản của ${user.fullName}?`)) {
      return;
    }

    setProcessingId(user.id);
    setError('');

    try {
      if (user.isActive) {
        await adminService.blockUser(user.id);
      } else {
        await adminService.unblockUser(user.id);
      }
      await loadUsers();
    } catch {
      setError('Không thể cập nhật trạng thái tài khoản.');
    } finally {
      setProcessingId(null);
    }
  }

  return (
    <AdminLayout>
      <PageHeading
        title="Tài khoản"
        subtitle="Quản lý người dùng, trạng thái hoạt động và các vai trò được gán."
      />

      <Toolbar
        value={search}
        onChange={(value) => {
          setSearch(value);
          setPage(1);
        }}
      />

      {error && <ErrorMessage text={error} />}

      {/* Bảng danh sách người dùng */}
      <div className="overflow-x-auto rounded-2xl border border-border bg-surface shadow-sm">
        <table className="w-full min-w-[760px] text-left text-sm">
          <thead>
            <tr className="border-b border-border text-xs uppercase tracking-wider text-muted">
              <th className="px-5 py-4">Người dùng</th>
              <th className="px-5 py-4">Vai trò</th>
              <th className="px-5 py-4">Đơn hàng</th>
              <th className="px-5 py-4">Đã chi</th>
              <th className="px-5 py-4">Trạng thái</th>
              <th className="px-5 py-4 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={6} className="px-5 py-10 text-center text-muted">
                  Đang tải dữ liệu...
                </td>
              </tr>
            ) : result?.data?.length === 0 ? (
              <tr>
                <td colSpan={6} className="px-5 py-10 text-center text-muted">
                  Không tìm thấy tài khoản nào.
                </td>
              </tr>
            ) : (
              result?.data.map((user) => {
                const isProcessing = processingId === user.id;
                return (
                  <tr
                    key={user.id}
                    className="border-b border-border transition-colors last:border-0 hover:bg-orange-500/[.02]"
                  >
                    {/* Tên & Email */}
                    <td className="px-5 py-4">
                      <p className="font-semibold text-foreground">{user.fullName}</p>
                      <p className="mt-0.5 text-xs text-muted">{user.email}</p>
                    </td>

                    {/* Vai trò */}
                    <td className="px-5 py-4">
                      <div className="flex flex-wrap gap-1">
                        {user.roles?.map((role) => (
                          <span
                            key={role}
                            className="rounded-full bg-orange-500/10 px-2.5 py-0.5 text-xs font-semibold text-orange-600"
                          >
                            {role}
                          </span>
                        ))}
                      </div>
                    </td>

                    {/* Đơn hàng */}
                    <td className="px-5 py-4 font-medium text-foreground">
                      {user.totalOrders}
                    </td>

                    {/* Đã chi */}
                    <td className="px-5 py-4 font-semibold text-foreground">
                      {money.format(user.totalSpent)}
                    </td>

                    {/* Trạng thái */}
                    <td className="px-5 py-4">
                      <span
                        className={`inline-block rounded-full px-2.5 py-0.5 text-xs font-bold ${
                          user.isActive
                            ? 'bg-emerald-500/10 text-emerald-600'
                            : 'bg-red-500/10 text-red-600'
                        }`}
                      >
                        {user.isActive ? 'Đang hoạt động' : 'Đã khóa'}
                      </span>
                    </td>

                    {/* Thao tác */}
                    <td className="px-5 py-4 text-right">
                      <button
                        disabled={isProcessing}
                        onClick={() => void handleToggleUser(user)}
                        title={user.isActive ? 'Khóa tài khoản' : 'Mở khóa tài khoản'}
                        className={`rounded-xl p-2 transition disabled:opacity-40 ${
                          user.isActive
                            ? 'text-muted hover:bg-red-50 hover:text-red-500'
                            : 'text-amber-600 hover:bg-amber-50 hover:text-amber-700'
                        }`}
                      >
                        {user.isActive ? <Lock size={17} /> : <Unlock size={17} />}
                      </button>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>

      <Pagination
        page={page}
        totalPages={result?.meta?.totalPages || 1}
        onChange={setPage}
      />
    </AdminLayout>
  );
}

/* Header trang */
function PageHeading({ title, subtitle }: { title: string; subtitle: string }) {
  return (
    <div className="mb-6">
      <h2 className="font-display text-3xl font-bold">{title}</h2>
      <p className="mt-1 text-sm text-muted">{subtitle}</p>
    </div>
  );
}

/* Thanh công cụ / Tìm kiếm */
function Toolbar({ value, onChange }: { value: string; onChange: (value: string) => void }) {
  return (
    <div className="mb-4 flex max-w-md items-center gap-2 rounded-xl border border-border bg-surface px-3.5 py-2 text-sm text-muted shadow-sm transition focus-within:border-orange-500">
      <Search size={17} />
      <input
        value={value}
        onChange={(event) => onChange(event.target.value)}
        placeholder="Tìm theo tên, email, số điện thoại..."
        className="w-full bg-transparent text-foreground outline-none placeholder:text-muted/60"
      />
    </div>
  );
}

/* Thông báo lỗi */
function ErrorMessage({ text }: { text: string }) {
  return (
    <p className="mb-4 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">
      {text}
    </p>
  );
}

/* Phân trang */
function Pagination({
  page,
  totalPages,
  onChange,
}: {
  page: number;
  totalPages: number;
  onChange: (page: number) => void;
}) {
  return (
    <div className="mt-4 flex items-center justify-between text-sm text-muted">
      <span>
        Trang <strong className="text-foreground">{page}</strong> / {totalPages}
      </span>
      <div className="flex gap-2">
        <button
          disabled={page <= 1}
          onClick={() => onChange(page - 1)}
          className="rounded-xl border border-border px-4 py-2 text-xs font-semibold transition hover:bg-black/5 disabled:opacity-40"
        >
          Trước
        </button>
        <button
          disabled={page >= totalPages}
          onClick={() => onChange(page + 1)}
          className="rounded-xl border border-border px-4 py-2 text-xs font-semibold transition hover:bg-black/5 disabled:opacity-40"
        >
          Sau
        </button>
      </div>
    </div>
  );
}