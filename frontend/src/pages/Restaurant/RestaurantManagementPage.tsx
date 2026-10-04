import { FormEvent, useEffect, useState } from 'react';
import { Clock3, MapPin, Pencil, Phone, Power, Store } from 'lucide-react';
import { AdminModal } from '@/components/admin/AdminModal';
import { RestaurantLayout } from './RestaurantLayout';
import { PageHeader, Panel } from '@/components/admin/AdminUI';
import Input, { type InputProps } from '@/components/ui/Input';
import { RestaurantProfile, restaurantService } from '@/services/restaurant.service';

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

export default function RestaurantManagementPage() {
  const [profile, setProfile] = useState<RestaurantProfile | null>(null);
  const [form, setForm] = useState(emptyProfile);
  const [closeMessage, setCloseMessage] = useState('');
  const [editOpen, setEditOpen] = useState(false);
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
      if (!result.data) throw new Error();

      if (isMounted) {
        setProfile(result.data);
        setForm({
          ...result.data,
          openTime: timeValue(result.data.openTime),
          closeTime: timeValue(result.data.closeTime),
        });
      }
    } catch {
      if (isMounted) {
        setError('Không thể tải thông tin nhà hàng. Bạn có thể chưa được gán vào nhà hàng nào.');
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
    setSaving(true);
    try {
      await restaurantService.updateProfile(form);
      setEditOpen(false);
      await loadProfile();
    } catch {
      setError('Không thể cập nhật thông tin nhà hàng.');
    } finally {
      setSaving(false);
    }
  }

  // Đổi trạng thái Đóng / Mở cửa
  async function handleChangeStatus() {
    setSaving(true);
    try {
      if (profile?.isOpen) {
        await restaurantService.close(closeMessage);
        setCloseOpen(false);
      } else {
        await restaurantService.open();
      }
      await loadProfile();
    } catch {
      setError('Không thể cập nhật trạng thái nhà hàng.');
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
            onClick={() => setEditOpen(true)}
            disabled={!profile || loading}
            className="flex items-center gap-2 rounded-xl bg-orange-500 px-4 py-2.5 text-sm font-bold text-white shadow-sm transition hover:-translate-y-0.5 hover:bg-orange-600 disabled:opacity-50"
          >
            <Pencil size={17} />
            Chỉnh sửa
          </button>
        }
      />

      {error && (
        <div className="mb-5 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600 shadow-sm">
          {error}
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
      >
        <form onSubmit={handleUpdateProfile} className="space-y-4">
          <Field
            label="Tên nhà hàng"
            value={form.name}
            onChange={(name) => setForm({ ...form, name })}
            required
          />
          <Field
            label="Địa chỉ"
            value={form.address}
            onChange={(address) => setForm({ ...form, address })}
            required
          />
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

          <div className="grid gap-4 sm:grid-cols-2">
            <Field
              label="Vĩ độ"
              type="number"
              value={String(form.latitude)}
              onChange={(latitude) => setForm({ ...form, latitude: Number(latitude) })}
            />
            <Field
              label="Kinh độ"
              type="number"
              value={String(form.longitude)}
              onChange={(longitude) => setForm({ ...form, longitude: Number(longitude) })}
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