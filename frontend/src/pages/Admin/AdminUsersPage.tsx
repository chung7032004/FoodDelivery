import { useEffect, useState } from 'react';
import { CalendarDays, Eye, Lock, Mail, Pencil, Phone, Search, ShieldOff, Trash2, Unlock, X } from 'lucide-react';
import { AdminLayout } from './AdminLayout';
import { AdminModal } from '@/components/admin/AdminModal';
import { ConfirmationModal } from '@/components/ui/ConfirmationModal';
import Input from '@/components/ui/Input';
import Button from '@/components/ui/Button';
import { AdminUser, AdminUserDetail, adminService, PagedResult } from '@/services/admin.service';
import { getImageUrl } from '@/utils/image';

const money = new Intl.NumberFormat('vi-VN', {
  style: 'currency',
  currency: 'VND',
  maximumFractionDigits: 0,
});

export default function AdminUsersPage() {
  const [result, setResult] = useState<PagedResult<AdminUser> | null>(null);
  const [search, setSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState<'all' | 'active' | 'blocked'>('all');
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [processingId, setProcessingId] = useState<string | null>(null);
  const [confirmingUser, setConfirmingUser] = useState<AdminUser | null>(null);
  const [deletingUser, setDeletingUser] = useState<AdminUser | null>(null);
  const [error, setError] = useState('');
  const [selectedUserId, setSelectedUserId] = useState<string | null>(null);
  const [selectedUser, setSelectedUser] = useState<AdminUserDetail | null>(null);
  const [detailLoading, setDetailLoading] = useState(false);
  const [detailError, setDetailError] = useState('');
  const [isEditing, setIsEditing] = useState(false);
  const [editFullName, setEditFullName] = useState('');
  const [editPhone, setEditPhone] = useState('');
  const [avatarFile, setAvatarFile] = useState<File | null>(null);
  const [isSaving, setIsSaving] = useState(false);
  const [restrictingRole, setRestrictingRole] = useState<string | null>(null);
  const [restrictionReason, setRestrictionReason] = useState('');
  const [isRestrictingRole, setIsRestrictingRole] = useState(false);

  // Tải danh sách người dùng
  const loadUsers = async (isMounted = true) => {
    setLoading(true);
    setError('');
    try {
      const data = await adminService.getUsers({
        searchQuery: search.trim() || undefined,
        isActive: statusFilter === 'all' ? undefined : statusFilter === 'active',
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
  }, [search, statusFilter, page]);

  useEffect(() => {
    if (!selectedUserId) {
      setSelectedUser(null);
      return;
    }
    let isSubscribed = true;
    setDetailLoading(true);
    setDetailError('');
    adminService.getUserDetail(selectedUserId)
      .then((user) => {
        if (!isSubscribed || !user) return;
        setSelectedUser(user);
        setEditFullName(user.fullName);
        setEditPhone(user.phone);
      })
      .catch(() => {
        if (isSubscribed) setDetailError('Không tải được thông tin tài khoản.');
      })
      .finally(() => {
        if (isSubscribed) setDetailLoading(false);
      });
    return () => { isSubscribed = false; };
  }, [selectedUserId]);

  // Xử lý Khóa / Mở khóa tài khoản
  async function handleToggleUser(user: AdminUser) {
    setProcessingId(user.id);
    setError('');

    try {
      if (user.isActive) {
        await adminService.blockUser(user.id);
      } else {
        await adminService.unblockUser(user.id);
      }
      setConfirmingUser(null);
      await loadUsers();
    } catch {
      setError('Không thể cập nhật trạng thái tài khoản.');
    } finally {
      setProcessingId(null);
    }
  }

  async function handleDeleteUser(user: AdminUser) {
    setProcessingId(user.id);
    setError('');
    try {
      await adminService.deleteUser(user.id);
      setDeletingUser(null);
      if (selectedUserId === user.id) setSelectedUserId(null);
      await loadUsers();
    } catch {
      setError('Không thể xóa tài khoản.');
    } finally {
      setProcessingId(null);
    }
  }

  async function handleSaveUser(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!selectedUser) return;
    setIsSaving(true);
    setDetailError('');
    const formData = new FormData();
    formData.append('FullName', editFullName.trim());
    formData.append('Phone', editPhone.trim());
    formData.append('Email', selectedUser.email);
    if (avatarFile) formData.append('Avatar', avatarFile);
    try {
      await adminService.updateUser(selectedUser.id, formData);
      const updatedUser = await adminService.getUserDetail(selectedUser.id);
      if (updatedUser) setSelectedUser(updatedUser);
      setAvatarFile(null);
      setIsEditing(false);
      await loadUsers();
    } catch {
      setDetailError('Không thể cập nhật tài khoản. Hãy kiểm tra email, số điện thoại và ảnh đại diện.');
    } finally {
      setIsSaving(false);
    }
  }

  async function handleSuspendRole(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!selectedUser || !restrictingRole) return;
    setIsRestrictingRole(true);
    setDetailError('');
    try {
      await adminService.suspendUserRole(selectedUser.id, restrictingRole, restrictionReason.trim());
      const updatedUser = await adminService.getUserDetail(selectedUser.id);
      if (updatedUser) setSelectedUser(updatedUser);
      setRestrictingRole(null);
      setRestrictionReason('');
      await loadUsers();
    } catch {
      setDetailError('Không thể thu hồi quyền. Hãy kiểm tra lý do và thử lại.');
    } finally {
      setIsRestrictingRole(false);
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

      <div className="mb-4 flex flex-wrap items-center gap-2" role="group" aria-label="Lọc trạng thái tài khoản">
        {([
          ['all', 'Tất cả'],
          ['active', 'Đang hoạt động'],
          ['blocked', 'Đã khóa'],
        ] as const).map(([value, label]) => (
          <button
            key={value}
            type="button"
            aria-pressed={statusFilter === value}
            onClick={() => { setStatusFilter(value); setPage(1); }}
            className={`rounded-lg border px-3 py-2 text-sm font-medium transition ${statusFilter === value ? 'border-orange-500 bg-orange-50 text-orange-700' : 'border-border bg-surface text-muted hover:bg-black/5'}`}
          >
            {label}
          </button>
        ))}
        <span className="ml-auto text-sm text-muted">{result?.meta.totalCount ?? 0} tài khoản</span>
      </div>

      {error && <ErrorMessage text={error} />}

      {/* Bảng danh sách người dùng */}
      <div className="overflow-x-auto rounded-2xl border border-border bg-surface shadow-sm">
        <table className="w-full min-w-[860px] text-left text-sm">
          <thead>
            <tr className="border-b border-border text-xs uppercase tracking-wider text-muted">
              <th className="px-5 py-4">Người dùng</th>
              <th className="px-5 py-4">Liên hệ</th>
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
                <td colSpan={7} className="px-5 py-10 text-center text-muted">
                  Đang tải dữ liệu...
                </td>
              </tr>
            ) : result?.data?.length === 0 ? (
              <tr>
                <td colSpan={7} className="px-5 py-10 text-center text-muted">
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
                      <div className="flex min-w-0 items-center gap-3">
                        {user.avatarUrl ? (
                          <img src={getImageUrl(user.avatarUrl)} alt="" className="h-10 w-10 shrink-0 rounded-full border border-border object-cover" />
                        ) : (
                          <span aria-hidden="true" className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-orange-100 text-sm font-semibold text-orange-700">
                            {user.fullName.trim().charAt(0).toUpperCase() || '?'}
                          </span>
                        )}
                        <div className="min-w-0">
                          <p className="truncate font-semibold text-foreground">{user.fullName}</p>
                          <p className="mt-0.5 truncate text-xs text-muted">{user.email}</p>
                        </div>
                      </div>
                    </td>

                    <td className="px-5 py-4 text-sm text-muted">{user.phone || 'Chưa cập nhật'}</td>

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
                        type="button"
                        disabled={isProcessing}
                        onClick={() => { setSelectedUserId(user.id); setIsEditing(false); setAvatarFile(null); }}
                        title="Xem chi tiết tài khoản"
                        aria-label={`Xem tài khoản ${user.fullName}`}
                        className="rounded-lg p-2 text-muted transition hover:bg-sky-50 hover:text-sky-700 disabled:opacity-40"
                      >
                        <Eye size={17} />
                      </button>
                      <button
                        type="button"
                        disabled={isProcessing}
                        onClick={() => setConfirmingUser(user)}
                        title={user.isActive ? 'Khóa tài khoản' : 'Mở khóa tài khoản'}
                        className={`rounded-xl p-2 transition disabled:opacity-40 ${
                          user.isActive
                            ? 'text-muted hover:bg-red-50 hover:text-red-500'
                            : 'text-amber-600 hover:bg-amber-50 hover:text-amber-700'
                        }`}
                      >
                        {user.isActive ? <Lock size={17} /> : <Unlock size={17} />}
                      </button>
                      <button
                        type="button"
                        disabled={isProcessing}
                        onClick={() => setDeletingUser(user)}
                        title="Xóa tài khoản"
                        aria-label={`Xóa tài khoản ${user.fullName}`}
                        className="rounded-lg p-2 text-muted transition hover:bg-red-50 hover:text-red-600 disabled:opacity-40"
                      >
                        <Trash2 size={17} />
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

      <AdminModal
        open={Boolean(selectedUserId)}
        onClose={() => { setSelectedUserId(null); setIsEditing(false); setAvatarFile(null); }}
        title={isEditing ? 'Chỉnh sửa tài khoản' : 'Chi tiết tài khoản'}
        description={selectedUser?.email ?? 'Thông tin và hoạt động của tài khoản'}
        size="lg"
      >
        {detailLoading ? (
          <p className="py-8 text-center text-sm text-slate-500">Đang tải thông tin...</p>
        ) : selectedUser ? (
          isEditing ? (
            <form onSubmit={handleSaveUser} className="space-y-4">
              <Input label="Họ và tên" required minLength={2} maxLength={100} value={editFullName} onChange={(event) => setEditFullName(event.target.value)} fullWidth />
              <Input label="Email" type="email" required maxLength={254} value={selectedUser.email} disabled helperText="Email đăng nhập không thể thay đổi tại đây." fullWidth />
              <Input label="Số điện thoại" type="tel" required maxLength={15} value={editPhone} onChange={(event) => setEditPhone(event.target.value)} fullWidth />
              <label className="block text-sm font-medium text-slate-700">
                Ảnh đại diện
                <input type="file" accept="image/*" onChange={(event) => setAvatarFile(event.target.files?.[0] ?? null)} className="mt-1 block w-full rounded-lg border border-slate-200 bg-white p-2 text-sm text-slate-600 file:mr-3 file:rounded-md file:border-0 file:bg-orange-50 file:px-3 file:py-1.5 file:font-semibold file:text-orange-700" />
              </label>
              {detailError && <p role="alert" className="text-sm text-red-600">{detailError}</p>}
              <div className="flex justify-end gap-2 pt-2">
                <button type="button" onClick={() => { setIsEditing(false); setAvatarFile(null); }} disabled={isSaving} className="inline-flex items-center gap-1.5 rounded-lg border border-slate-200 px-4 py-2 text-sm font-medium text-slate-600 hover:bg-slate-50 disabled:opacity-50"><X size={15} /> Hủy</button>
                <Button type="submit" size="md" isLoading={isSaving}>Lưu thay đổi</Button>
              </div>
            </form>
          ) : (
            <div className="space-y-6">
              {detailError && <p role="alert" className="text-sm text-red-600">{detailError}</p>}
              <div className="flex flex-wrap items-center gap-4">
                <img src={getImageUrl(selectedUser.avatarUrl)} alt="" className="h-16 w-16 rounded-full border border-slate-200 object-cover" />
                <div>
                  <h3 className="text-lg font-semibold text-slate-900">{selectedUser.fullName}</h3>
                  <div className="mt-1 flex flex-wrap gap-1.5">
                    {selectedUser.roles.map((role) => <span key={role} className="rounded-full bg-orange-50 px-2.5 py-1 text-xs font-semibold text-orange-700">{role}</span>)}
                  </div>
                </div>
                <button type="button" onClick={() => setIsEditing(true)} title="Chỉnh sửa tài khoản" aria-label="Chỉnh sửa tài khoản" className="ml-auto rounded-lg border border-slate-200 p-2 text-slate-600 hover:bg-slate-50"><Pencil size={16} /></button>
              </div>
              {selectedUser.roles.some((role) => role === 'Shipper' || role === 'RestaurantOwner') && (
                <div className="border-y border-slate-100 py-4">
                  <h4 className="text-sm font-semibold text-slate-800">Quyền kinh doanh</h4>
                  <p className="mt-1 text-xs text-slate-500">Thu hồi quyền này không khóa tài khoản; người dùng vẫn có thể sử dụng quyền Khách hàng.</p>
                  <div className="mt-3 flex flex-wrap gap-2">
                    {(['Shipper', 'RestaurantOwner'] as const).filter((role) => selectedUser.roles.includes(role)).map((role) => (
                      <button key={role} type="button" onClick={() => { setRestrictingRole(role); setRestrictionReason(''); }} className="inline-flex items-center gap-1.5 rounded-lg border border-red-200 px-3 py-2 text-xs font-semibold text-red-700 hover:bg-red-50">
                        <ShieldOff size={14} /> Thu hồi {role === 'Shipper' ? 'Shipper' : 'chủ nhà hàng'}
                      </button>
                    ))}
                  </div>
                </div>
              )}
              <div className="grid gap-4 sm:grid-cols-2">
                <DetailField icon={<Mail size={15} />} label="Email" value={selectedUser.email} />
                <DetailField icon={<Phone size={15} />} label="Điện thoại" value={selectedUser.phone || 'Chưa cập nhật'} />
                <DetailField icon={<CalendarDays size={15} />} label="Ngày tạo" value={formatDate(selectedUser.createdAt)} />
                <DetailField icon={<CalendarDays size={15} />} label="Đăng nhập gần nhất" value={formatDate(selectedUser.lastLogin)} />
                <DetailField label="Đơn hàng" value={String(selectedUser.totalOrders)} />
                <DetailField label="Tổng chi tiêu" value={money.format(selectedUser.totalSpent)} />
              </div>
              <div>
                <h4 className="mb-2 text-sm font-semibold text-slate-800">Đơn hàng gần đây</h4>
                {selectedUser.recentOrders.length ? (
                  <div className="divide-y divide-slate-100 border-y border-slate-100">
                    {selectedUser.recentOrders.map((order) => (
                      <div key={order.orderId} className="flex flex-wrap items-center justify-between gap-2 py-2.5 text-sm">
                        <span className="font-medium text-slate-800">{order.orderCode}</span>
                        <span className="text-slate-600">{money.format(order.amount)}</span>
                        <span className="text-xs text-slate-500">{formatDate(order.createdAt)}</span>
                      </div>
                    ))}
                  </div>
                ) : <p className="text-sm text-slate-500">Chưa có đơn hàng.</p>}
              </div>
            </div>
          )
        ) : detailError ? (
          <p role="alert" className="py-6 text-sm text-red-600">{detailError}</p>
        ) : null}
      </AdminModal>
      <AdminModal
        open={Boolean(restrictingRole)}
        onClose={() => { if (!isRestrictingRole) setRestrictingRole(null); }}
        title="Thu hồi quyền truy cập?"
        description={selectedUser?.fullName}
      >
        <form onSubmit={handleSuspendRole} className="space-y-4">
          <p className="text-sm text-slate-600">
            Người dùng sẽ mất quyền {restrictingRole === 'Shipper' ? 'Shipper' : 'chủ nhà hàng'} và chỉ còn quyền khách hàng. Họ có thể gửi khiếu nại để admin xem xét.
          </p>
          <label className="block text-sm font-medium text-slate-700">
            Lý do thu hồi quyền
            <textarea required minLength={5} maxLength={500} value={restrictionReason} onChange={(event) => setRestrictionReason(event.target.value)} rows={4} className="mt-1 w-full rounded-lg border border-slate-200 p-3 text-sm outline-none focus:border-orange-500" />
          </label>
          {detailError && <p role="alert" className="text-sm text-red-600">{detailError}</p>}
          <div className="flex justify-end gap-2">
            <button type="button" disabled={isRestrictingRole} onClick={() => setRestrictingRole(null)} className="rounded-lg border border-slate-200 px-4 py-2 text-sm font-medium text-slate-600 disabled:opacity-50">Hủy</button>
            <Button type="submit" variant="danger" isLoading={isRestrictingRole} disabled={restrictionReason.trim().length < 5}>Thu hồi quyền</Button>
          </div>
        </form>
      </AdminModal>
      <ConfirmationModal
        open={Boolean(confirmingUser)}
        title={confirmingUser?.isActive ? 'Khóa tài khoản?' : 'Mở khóa tài khoản?'}
        description={confirmingUser?.isActive
          ? `Tài khoản ${confirmingUser.fullName} sẽ không thể đăng nhập cho đến khi được mở khóa.`
          : `Cho phép ${confirmingUser?.fullName} tiếp tục đăng nhập và sử dụng tài khoản?`}
        confirmLabel={confirmingUser?.isActive ? 'Khóa tài khoản' : 'Mở khóa'}
        tone={confirmingUser?.isActive ? 'danger' : 'primary'}
        isLoading={Boolean(confirmingUser && processingId === confirmingUser.id)}
        onCancel={() => setConfirmingUser(null)}
        onConfirm={() => {
          if (confirmingUser) return handleToggleUser(confirmingUser);
        }}
      />
      <ConfirmationModal
        open={Boolean(deletingUser)}
        title="Xóa tài khoản?"
        description={`Tài khoản ${deletingUser?.fullName ?? ''} sẽ bị vô hiệu hóa và không thể đăng nhập.`}
        confirmLabel="Xóa tài khoản"
        tone="danger"
        isLoading={Boolean(deletingUser && processingId === deletingUser.id)}
        onCancel={() => setDeletingUser(null)}
        onConfirm={() => {
          if (deletingUser) return handleDeleteUser(deletingUser);
        }}
      />
    </AdminLayout>
  );
}

function formatDate(value: string | null) {
  return value ? new Intl.DateTimeFormat('vi-VN', { dateStyle: 'medium' }).format(new Date(value)) : 'Chưa đăng nhập';
}

function DetailField({ icon, label, value }: { icon?: React.ReactNode; label: string; value: string }) {
  return (
    <div className="min-w-0">
      <p className="flex items-center gap-1.5 text-xs text-slate-500">{icon}{label}</p>
      <p className="mt-1 break-words text-sm font-medium text-slate-800">{value}</p>
    </div>
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
    <div className="mb-4 max-w-md">
      <Input
        value={value}
        onChange={(event) => onChange(event.target.value)}
        placeholder="Tìm theo tên, email, số điện thoại..."
        leftIcon={<Search size={16} />}
        size="sm"
        fullWidth
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