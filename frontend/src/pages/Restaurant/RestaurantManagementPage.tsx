import { FormEvent, useEffect, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { BarChart3, Clock3, MapPin, Pencil, Phone, Power, RefreshCw, Store, TrendingUp } from 'lucide-react';
import { Link } from 'react-router-dom';
import { AdminModal } from '@/components/admin/AdminModal';
import { AddressMapPicker } from '@/components/account/AddressMapPicker';
import { RestaurantLayout } from './RestaurantLayout';
import { PageHeader, Panel } from '@/components/admin/AdminUI';
import Input, { type InputProps } from '@/components/ui/Input';
import { RestaurantProfile, restaurantService } from '@/services/restaurant.service';
import { staffService, type RestaurantAnalytics } from '@/services/staff.service';

const emptyProfile: Omit<RestaurantProfile, 'id' | 'isOpen' | 'closingMessage'> = {
  name: '',
  address: '',
  phone: '',
  openTime: '08:00',
  closeTime: '22:00',
  latitude: 0,
  longitude: 0,
};

const timeValue = (value: string) => value.slice(0, 5);
const timeSpanValue = (value: string) => `${timeValue(value)}:00`;
const money = new Intl.NumberFormat('vi-VN', {
  style: 'currency',
  currency: 'VND',
  maximumFractionDigits: 0,
});

function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  if (typeof error === 'object' && error !== null && 'message' in error && typeof error.message === 'string') {
    return error.message;
  }
  return 'Không thể cập nhật thông tin nhà hàng.';
}

export default function RestaurantManagementPage() {
  const [profile, setProfile] = useState<RestaurantProfile | null>(null);
  const [form, setForm] = useState(emptyProfile);
  const [closeMessage, setCloseMessage] = useState('');
  const [editOpen, setEditOpen] = useState(false);
  const [editError, setEditError] = useState('');
  const [closeOpen, setCloseOpen] = useState(false);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');

  // Tải thông tin nhà hàng
  async function loadProfile(isMounted = true) {
    setLoading(true);
    setError('');
    try {
      const result = await restaurantService.getProfile();
      if (!result.isSuccess || !result.data) {
        throw new Error(result.message ?? 'Không thể tải thông tin nhà hàng.');
      }

      if (isMounted) {
        setProfile(result.data);
        setForm({
          ...result.data,
          openTime: timeValue(result.data.openTime),
          closeTime: timeValue(result.data.closeTime),
        });
      }
    } catch (loadError) {
      if (isMounted) {
        setError(getErrorMessage(loadError));
      }
    } finally {
      if (isMounted) setLoading(false);
    }
  }

  useEffect(() => {
    let isSubscribed = true;
    void loadProfile(isSubscribed);
    return () => {
      isSubscribed = false;
    };
  }, []);

  // Cập nhật thông tin hồ sơ nhà hàng
  async function handleUpdateProfile(event: FormEvent) {
    event.preventDefault();
    if (!form.name.trim() || !form.address.trim()) {
      setEditError('Vui lòng nhập tên và địa chỉ nhà hàng.');
      return;
    }
    if (!/^0\d{9}$/.test(form.phone.trim())) {
      setEditError('Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0.');
      return;
    }
    if (
      !Number.isFinite(form.latitude)
      || !Number.isFinite(form.longitude)
      || form.latitude < -90
      || form.latitude > 90
      || form.longitude < -180
      || form.longitude > 180
    ) {
      setEditError('Hãy chọn vị trí nhà hàng hợp lệ trên bản đồ trước khi lưu.');
      return;
    }
    if (form.openTime === form.closeTime) {
      setEditError('Giờ mở cửa và giờ đóng cửa không được trùng nhau.');
      return;
    }

    setEditError('');
    setSaving(true);
    try {
      const result = await restaurantService.updateProfile({
        ...form,
        name: form.name.trim(),
        address: form.address.trim(),
        phone: form.phone.trim(),
        openTime: timeSpanValue(form.openTime),
        closeTime: timeSpanValue(form.closeTime),
      });
      if (result.data && typeof result.data === 'object' && 'isSuccess' in result.data && !result.data.isSuccess) {
        throw new Error('Không thể cập nhật thông tin nhà hàng.');
      }
      setEditOpen(false);
      await loadProfile();
    } catch (updateError) {
      setEditError(getErrorMessage(updateError));
    } finally {
      setSaving(false);
    }
  }

  // Đổi trạng thái Đóng / Mở cửa
  async function handleChangeStatus() {
    setSaving(true);
    setError('');
    try {
      if (profile?.isOpen) {
        await restaurantService.close(closeMessage);
        setCloseOpen(false);
      } else {
        await restaurantService.open();
      }
      await loadProfile();
    } catch (statusError) {
      setError(getErrorMessage(statusError));
    } finally {
      setSaving(false);
    }
  }

  return (
    <RestaurantLayout>
      <PageHeader
        eyebrow="Không gian nhà hàng"
        title="Quản lý nhà hàng"
        description="Cập nhật thông tin, giờ hoạt động và trạng thái nhận đơn của nhà hàng."
        action={
          <button
            onClick={() => {
              setEditError('');
              setEditOpen(true);
            }}
            disabled={!profile || loading}
            className="flex items-center gap-2 rounded-xl bg-orange-500 px-4 py-2.5 text-sm font-bold text-white shadow-sm transition hover:-translate-y-0.5 hover:bg-orange-600 disabled:opacity-50"
          >
            <Pencil size={17} />
            Chỉnh sửa
          </button>
        }
      />

      {error && (
        <div role="alert" className="mb-5 flex flex-wrap items-center justify-between gap-3 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700 shadow-sm">
          <span>{error}</span>
          <button type="button" onClick={() => void loadProfile()} disabled={loading} className="inline-flex items-center gap-2 font-semibold hover:text-red-900 disabled:opacity-50">
            <RefreshCw size={15} className={loading ? 'animate-spin' : ''} />
            Tải lại
          </button>
        </div>
      )}

      {loading ? (
        <Panel>
          <div className="p-10 text-center text-sm text-muted">
            Đang tải thông tin nhà hàng...
          </div>
        </Panel>
      ) : (
        profile && (
          <>
            {/* Các thẻ thông tin chính */}
            <div className="mb-6 grid gap-4 md:grid-cols-3">
              <InfoCard icon={Store} label="Nhà hàng" value={profile.name} />
              <InfoCard icon={MapPin} label="Địa chỉ" value={profile.address} />
              <InfoCard icon={Phone} label="Số điện thoại" value={profile.phone} />
            </div>

            <RestaurantOverview />

            <div className="mb-6 grid gap-3 sm:grid-cols-2">
              <Link to="/restaurant/menu" className="rounded-2xl border border-sky-200 bg-white p-5 transition hover:border-sky-400 hover:shadow-sm">
                <p className="font-semibold text-slate-900">Quản lý thực đơn</p>
                <p className="mt-1 text-sm text-slate-500">Thêm, sửa, ẩn hoặc xóa món của nhà hàng.</p>
              </Link>
              <Link to="/restaurant/orders" className="rounded-2xl border border-sky-200 bg-white p-5 transition hover:border-sky-400 hover:shadow-sm">
                <p className="font-semibold text-slate-900">Quản lý đơn hàng</p>
                <p className="mt-1 text-sm text-slate-500">Theo dõi và xử lý đơn khách đặt từ nhà hàng.</p>
              </Link>
            </div>

            {/* Trạng thái & Giờ hoạt động */}
            <div className="grid gap-6 lg:grid-cols-[1.1fr_0.9fr]">
              <Panel
                title="Trạng thái nhận đơn"
                subtitle="Thay đổi trạng thái hiển thị của nhà hàng"
              >
                <div className="p-5">
                  <div
                    className={`mb-5 flex items-center gap-4 rounded-2xl p-5 ${
                      profile.isOpen ? 'bg-emerald-50/70' : 'bg-slate-100'
                    }`}
                  >
                    <span
                      className={`flex h-12 w-12 shrink-0 items-center justify-center rounded-full shadow-sm ${
                        profile.isOpen ? 'bg-emerald-500 text-white' : 'bg-slate-400 text-white'
                      }`}
                    >
                      <Power size={22} />
                    </span>
                    <div>
                      <p
                        className={`font-display text-xl font-bold ${
                          profile.isOpen ? 'text-emerald-700' : 'text-slate-700'
                        }`}
                      >
                        {profile.isOpen ? 'Đang mở cửa' : 'Đang đóng cửa'}
                      </p>
                      <p className="mt-1 text-sm text-muted">
                        {profile.closingMessage ||
                          (profile.isOpen
                            ? 'Nhà hàng đang nhận đơn.'
                            : 'Nhà hàng tạm thời không nhận đơn.')}
                      </p>
                    </div>
                  </div>

                  <button
                    onClick={() => (profile.isOpen ? setCloseOpen(true) : void handleChangeStatus())}
                    disabled={saving}
                    className={`w-full rounded-xl px-4 py-3 text-sm font-bold text-white transition shadow-sm disabled:opacity-50 ${
                      profile.isOpen
                        ? 'bg-slate-800 hover:bg-slate-950'
                        : 'bg-emerald-600 hover:bg-emerald-700'
                    }`}
                  >
                    {saving
                      ? 'Đang cập nhật...'
                      : profile.isOpen
                      ? 'Đóng nhận đơn'
                      : 'Mở nhận đơn'}
                  </button>
                </div>
              </Panel>

              <Panel title="Giờ hoạt động" subtitle="Khung giờ hiển thị cho khách">
                <div className="space-y-4 p-5">
                  <div className="flex items-center justify-between rounded-xl bg-orange-50/80 p-4">
                    <span className="flex items-center gap-3 text-sm font-semibold text-slate-700">
                      <Clock3 size={19} className="text-orange-600" />
                      Giờ mở cửa
                    </span>
                    <strong className="font-display text-base text-slate-900">
                      {timeValue(profile.openTime)}
                    </strong>
                  </div>
                  <div className="flex items-center justify-between rounded-xl bg-sky-50/80 p-4">
                    <span className="flex items-center gap-3 text-sm font-semibold text-slate-700">
                      <Clock3 size={19} className="text-sky-600" />
                      Giờ đóng cửa
                    </span>
                    <strong className="font-display text-base text-slate-900">
                      {timeValue(profile.closeTime)}
                    </strong>
                  </div>
                </div>
              </Panel>
            </div>
          </>
        )
      )}

      {/* Modal Chỉnh sửa nhà hàng */}
      <AdminModal
        open={editOpen}
        onClose={() => setEditOpen(false)}
        title="Chỉnh sửa nhà hàng"
        description="Thông tin này được dùng trên trang khách hàng."
        size="lg"
      >
        <form onSubmit={handleUpdateProfile} className="space-y-4">
          {editError && (
            <div role="alert" className="rounded-lg border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
              {editError}
            </div>
          )}
          <Field
            label="Tên nhà hàng"
            value={form.name}
            onChange={(name) => setForm({ ...form, name })}
            required
          />
          <div>
            <label className="mb-2 block text-sm font-medium text-slate-700">
              Địa chỉ và vị trí nhà hàng <span className="text-red-500">*</span>
            </label>
            <AddressMapPicker
              address={form.address}
              position={Number.isFinite(form.latitude) && Number.isFinite(form.longitude)
                ? { lat: form.latitude, lng: form.longitude }
                : null}
              onAddressInput={(address) => setForm((current) => ({
                ...current,
                address,
                latitude: Number.NaN,
                longitude: Number.NaN,
              }))}
              onAddressSelected={(address, coordinates) => setForm((current) => ({
                ...current,
                address,
                latitude: coordinates.lat,
                longitude: coordinates.lng,
              }))}
              onPositionSelected={(coordinates) => setForm((current) => ({
                ...current,
                latitude: coordinates.lat,
                longitude: coordinates.lng,
              }))}
            />
          </div>
          <Field
            label="Số điện thoại"
            value={form.phone}
            onChange={(phone) => setForm({ ...form, phone })}
            required
          />

          <div className="grid gap-4 sm:grid-cols-2">
            <Field
              label="Giờ mở cửa"
              type="time"
              value={form.openTime}
              onChange={(openTime) => setForm({ ...form, openTime })}
              required
            />
            <Field
              label="Giờ đóng cửa"
              type="time"
              value={form.closeTime}
              onChange={(closeTime) => setForm({ ...form, closeTime })}
              required
            />
          </div>

          <ModalActions onCancel={() => setEditOpen(false)} saving={saving} />
        </form>
      </AdminModal>

      {/* Modal Đóng nhận đơn */}
      <AdminModal
        open={closeOpen}
        onClose={() => setCloseOpen(false)}
        title="Đóng nhận đơn"
        description="Khách hàng sẽ thấy nhà hàng đang tạm đóng cửa."
      >
        <div className="space-y-4">
          <Field
            label="Lý do đóng cửa"
            value={closeMessage}
            onChange={setCloseMessage}
          />
          <ModalActions
            onCancel={() => setCloseOpen(false)}
            saving={saving}
            submitLabel="Xác nhận đóng"
            onSubmit={handleChangeStatus}
          />
        </div>
      </AdminModal>
    </RestaurantLayout>
  );
}

function RestaurantOverview() {
  const [period, setPeriod] = useState<RestaurantAnalytics['period']>('daily');
  const analyticsQuery = useQuery({
    queryKey: ['restaurant-analytics', period],
    queryFn: () => staffService.getAnalytics(period),
  });
  const statsQuery = useQuery({
    queryKey: ['restaurant-order-stats'],
    queryFn: staffService.getStats,
  });
  const analytics = analyticsQuery.data?.data;
  const stats = statsQuery.data?.data;
  const points = analytics?.points ?? [];
  const maxRevenue = Math.max(1, ...points.map((point) => point.revenue));
  const pointCoordinates = (value: number, index: number) => {
    const x = points.length < 2 ? 380 : 30 + (index * 700) / (points.length - 1);
    const y = 205 - (value / maxRevenue) * 170;
    return `${x},${y}`;
  };
  const revenueLine = points.map((point, index) => pointCoordinates(point.revenue, index)).join(' ');
  const hasCompleteCostData = points.every((point) => point.grossProfit !== null);
  const profitLine = hasCompleteCostData
    ? points.map((point, index) => pointCoordinates(point.grossProfit ?? 0, index)).join(' ')
    : '';
  const formatPeriod = (value: string) => {
    const date = new Date(value);
    if (period === 'daily') return date.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit' });
    if (period === 'monthly') return date.toLocaleDateString('vi-VN', { month: '2-digit', year: '2-digit' });
    return String(date.getFullYear());
  };
  const activeStatuses = [
    { label: 'Đơn mới', count: stats?.pendingCount ?? 0, tone: 'text-amber-700 bg-amber-50' },
    { label: 'Chờ khách phản hồi', count: stats?.waitingCustomerDecisionCount ?? 0, tone: 'text-violet-700 bg-violet-50' },
    { label: 'Đã xác nhận', count: stats?.confirmedCount ?? 0, tone: 'text-sky-700 bg-sky-50' },
    { label: 'Đang chuẩn bị', count: stats?.preparingCount ?? 0, tone: 'text-orange-700 bg-orange-50' },
    { label: 'Chờ lấy hàng', count: stats?.readyCount ?? 0, tone: 'text-emerald-700 bg-emerald-50' },
    { label: 'Đang giao', count: stats?.shippingCount ?? 0, tone: 'text-blue-700 bg-blue-50' },
  ];

  return (
    <div className="mb-7 space-y-6">
      <Panel title="Trạng thái đơn hôm nay" subtitle={`${stats?.totalOrdersToday ?? 0} đơn được tạo hôm nay`}>
        {statsQuery.isPending ? (
          <div className="p-5 text-sm text-muted">Đang tải trạng thái đơn hàng...</div>
        ) : statsQuery.isError || !statsQuery.data?.isSuccess ? (
          <div role="alert" className="p-5 text-sm text-red-600">
            {statsQuery.data?.message ?? 'Không tải được thống kê trạng thái đơn.'}
          </div>
        ) : (
          <div className="grid gap-3 p-5 sm:grid-cols-2 lg:grid-cols-3">
            {activeStatuses.map((status) => (
              <Link
                key={status.label}
                to="/restaurant/orders"
                className={`flex items-center justify-between gap-3 rounded-xl px-4 py-3 ${status.tone}`}
              >
                <span className="text-sm font-semibold">{status.label}</span>
                <span className="font-display text-xl font-bold">{status.count}</span>
              </Link>
            ))}
          </div>
        )}
      </Panel>

      <Panel
        title="Doanh thu & lợi nhuận"
        subtitle="Chỉ tính đơn đã giao thành công; doanh thu là tiền món, không gồm phí giao hàng."
        action={
          <div className="flex flex-wrap gap-1">
            {([
              ['daily', 'Ngày'],
              ['monthly', 'Tháng'],
              ['yearly', 'Năm'],
            ] as const).map(([value, label]) => (
              <button
                key={value}
                type="button"
                onClick={() => setPeriod(value)}
                className={`rounded-lg px-3 py-1.5 text-xs font-semibold ${
                  period === value ? 'bg-sky-600 text-white' : 'text-slate-600 hover:bg-slate-100'
                }`}
              >
                {label}
              </button>
            ))}
          </div>
        }
      >
        {analyticsQuery.isPending ? (
          <div className="p-8 text-center text-sm text-muted">Đang tải báo cáo...</div>
        ) : analyticsQuery.isError || !analyticsQuery.data?.isSuccess || !analytics ? (
          <div role="alert" className="p-5 text-sm text-red-600">
            {analyticsQuery.data?.message ?? 'Không tải được báo cáo doanh thu.'}
          </div>
        ) : (
          <div className="p-5">
            <div className="grid gap-3 sm:grid-cols-3">
              <MetricCard icon={BarChart3} label="Doanh thu" value={money.format(analytics.revenue)} detail="Tiền món của đơn đã giao" />
              <MetricCard
                icon={TrendingUp}
                label="Lãi gộp"
                value={analytics.grossProfit === null ? 'Chưa đủ dữ liệu' : money.format(analytics.grossProfit)}
                detail="Doanh thu trừ giá vốn món"
                tone="green"
              />
              <MetricCard icon={Store} label="Đơn hoàn tất" value={analytics.completedOrders} detail="Đơn giao thành công trong kỳ" tone="blue" />
            </div>

            {analytics.ordersMissingCost > 0 && (
              <div className="mt-4 rounded-lg border border-amber-200 bg-amber-50 px-4 py-3 text-sm text-amber-900">
                Chưa thể tính lãi chính xác cho {analytics.ordersMissingCost} đơn vì món chưa có giá vốn tại thời điểm đặt.
                {' '}<Link to="/restaurant/menu" className="font-semibold underline">Cập nhật giá vốn trong thực đơn</Link>.
              </div>
            )}

            <div className="mt-6">
              <div className="mb-3 flex flex-wrap items-center gap-x-5 gap-y-2 text-xs font-semibold text-slate-600">
                <span className="flex items-center gap-2"><i className="h-2.5 w-2.5 rounded-full bg-sky-500" />Doanh thu</span>
                {hasCompleteCostData && <span className="flex items-center gap-2"><i className="h-2.5 w-2.5 rounded-full bg-emerald-500" />Lãi gộp</span>}
              </div>
              <div className="overflow-x-auto">
                <svg viewBox="0 0 760 250" role="img" aria-label="Biểu đồ doanh thu và lợi nhuận" className="h-64 min-w-[680px] w-full">
                  {[35, 90, 145, 205].map((y) => (
                    <line key={y} x1="25" x2="735" y1={y} y2={y} stroke="#e2e8f0" strokeDasharray="4 5" />
                  ))}
                  {revenueLine && <polyline points={revenueLine} fill="none" stroke="#0ea5e9" strokeWidth="3" strokeLinejoin="round" strokeLinecap="round" />}
                  {profitLine && <polyline points={profitLine} fill="none" stroke="#10b981" strokeWidth="3" strokeLinejoin="round" strokeLinecap="round" />}
                  {points.map((point, index) => {
                    const [x, y] = pointCoordinates(point.revenue, index).split(',');
                    const showLabel = index === 0 || index === points.length - 1 || index % Math.ceil(points.length / 6) === 0;
                    return (
                      <g key={point.periodStart}>
                        <circle cx={x} cy={y} r="4" fill="#0ea5e9" />
                        {showLabel && <text x={x} y="235" textAnchor="middle" fill="#64748b" fontSize="10">{formatPeriod(point.periodStart)}</text>}
                      </g>
                    );
                  })}
                </svg>
              </div>
              <p className="mt-1 flex items-center gap-1 text-xs text-muted">
                <TrendingUp size={13} /> Lãi gộp chưa trừ chi phí vận hành, thuế và hoa hồng nền tảng.
              </p>
            </div>
          </div>
        )}
      </Panel>
    </div>
  );
}

function MetricCard({
  icon: Icon,
  label,
  value,
  detail,
  tone = 'orange',
}: {
  icon: typeof Store;
  label: string;
  value: string | number;
  detail: string;
  tone?: 'orange' | 'green' | 'blue';
}) {
  const styles = {
    orange: 'bg-orange-50 text-orange-700',
    green: 'bg-emerald-50 text-emerald-700',
    blue: 'bg-sky-50 text-sky-700',
  };
  return (
    <div className={`rounded-xl p-4 ${styles[tone]}`}>
      <div className="flex items-center gap-2 text-xs font-semibold"><Icon size={16} />{label}</div>
      <p className="mt-3 break-words font-display text-xl font-bold">{value}</p>
      <p className="mt-1 text-xs opacity-80">{detail}</p>
    </div>
  );
}

/* Thẻ thông tin nhanh */
function InfoCard({
  icon: Icon,
  label,
  value,
}: {
  icon: typeof Store;
  label: string;
  value: string;
}) {
  return (
    <div className="rounded-2xl border border-border bg-surface p-5 shadow-sm transition hover:shadow-md">
      <Icon size={20} className="mb-3 text-orange-600" />
      <p className="text-xs font-semibold uppercase tracking-wider text-muted">{label}</p>
      <p className="mt-1 truncate font-display text-lg font-bold text-foreground">{value}</p>
    </div>
  );
}

/* Trường nhập liệu */
function Field({
  label,
  value,
  onChange,
  type = 'text',
  required = false,
}: {
  label: string;
  value: string;
  onChange: (value: string) => void;
  type?: InputProps['type'];
  required?: boolean;
}) {
  return (
    <Input
        label={label}
        required={required}
        type={type}
        value={value}
        onChange={(event) => onChange(event.target.value)}
        fullWidth
      />
  );
}

/* Nút hành động cho Modal */
function ModalActions({
  onCancel,
  saving,
  submitLabel = 'Lưu thay đổi',
  onSubmit,
}: {
  onCancel: () => void;
  saving: boolean;
  submitLabel?: string;
  onSubmit?: () => void;
}) {
  return (
    <div className="mt-6 flex justify-end gap-2">
      <button
        type="button"
        onClick={onCancel}
        className="rounded-xl border border-slate-200 px-4 py-2 text-sm font-semibold text-slate-600 transition hover:bg-slate-100"
      >
        Hủy
      </button>
      <button
        type={onSubmit ? 'button' : 'submit'}
        onClick={onSubmit}
        disabled={saving}
        className="rounded-xl bg-orange-500 px-4 py-2 text-sm font-bold text-white transition hover:bg-orange-600 disabled:opacity-50"
      >
        {saving ? 'Đang lưu...' : submitLabel}
      </button>
    </div>
  );
}