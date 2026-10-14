import { useEffect, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { ArrowLeft, CheckCircle2, Clock3, Store, XCircle } from 'lucide-react';
import { Link } from 'react-router-dom';
import { AddressMapPicker } from '@/components/account/AddressMapPicker';
import { Footer } from '@/components/layout/Footer';
import { HeaderNav } from '@/components/layout/HeaderNav';
import Button from '@/components/ui/Button';
import Input from '@/components/ui/Input';
import { accountService } from '@/services/account.service';
import { restaurantService, type RestaurantApplication, type RestaurantRegistrationRequest } from '@/services/restaurant.service';
import { toast } from 'sonner';

function statusKind(status: number | string) {
  if (status === 1 || status === '1' || status === 'Approved') return 'approved';
  if (status === 2 || status === '2' || status === 'Rejected') return 'rejected';
  return 'pending';
}

function displayTime(value: string) {
  return value.slice(0, 5);
}

export default function RestaurantApplyPage() {
  const queryClient = useQueryClient();
  const accountQuery = useQuery({ queryKey: ['my-account'], queryFn: accountService.getMe });
  const applicationQuery = useQuery({ queryKey: ['my-restaurant-application'], queryFn: restaurantService.getMyApplication });
  const application = applicationQuery.data?.data?.application ?? null;
  const applicationState = application ? statusKind(application.applicationStatus) : null;
  const [name, setName] = useState('');
  const [address, setAddress] = useState('');
  const [phone, setPhone] = useState('');
  const [latitude, setLatitude] = useState<number | null>(null);
  const [longitude, setLongitude] = useState<number | null>(null);
  const [openTime, setOpenTime] = useState('08:00');
  const [closeTime, setCloseTime] = useState('22:00');
  const [error, setError] = useState('');

  useEffect(() => {
    if (!application || applicationState !== 'rejected') return;
    setName(application.name);
    setAddress(application.address);
    setPhone(application.phone);
    setLatitude(application.latitude);
    setLongitude(application.longitude);
    setOpenTime(displayTime(application.openTime));
    setCloseTime(displayTime(application.closeTime));
  }, [application, applicationState]);

  const registerMutation = useMutation({
    mutationFn: async (request: RestaurantRegistrationRequest) => {
      const result = await restaurantService.register(request);
      if (!result.isSuccess) throw new Error(result.message ?? 'Không thể gửi hồ sơ nhà hàng.');
      return result;
    },
    onSuccess: async (result) => {
      await queryClient.invalidateQueries({ queryKey: ['my-restaurant-application'] });
      setError('');
      toast.success(result.message ?? 'Đã gửi hồ sơ, vui lòng chờ admin duyệt.');
    },
    onError: (requestError) => setError(requestError instanceof Error ? requestError.message : 'Không thể gửi hồ sơ nhà hàng.'),
  });

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError('');
    if (!/^0\d{9}$/.test(phone.trim())) {
      setError('Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0.');
      return;
    }
    if (latitude === null || longitude === null) {
      setError('Hãy chọn vị trí nhà hàng trên bản đồ.');
      return;
    }
    if (openTime === closeTime) {
      setError('Giờ mở cửa và đóng cửa không được giống nhau.');
      return;
    }
    registerMutation.mutate({
      name: name.trim(),
      address: address.trim(),
      phone: phone.trim(),
      latitude,
      longitude,
      openTime: `${openTime}:00`,
      closeTime: `${closeTime}:00`,
    });
  }

  const hasPendingOrApproved = application && applicationState !== 'rejected';

  return (
    <div className="min-h-screen bg-surface font-sans">
      <HeaderNav />
      <main className="mx-auto min-h-[65vh] max-w-5xl px-4 py-10 sm:px-6 lg:px-8">
        <Link to="/account" className="mb-6 inline-flex items-center gap-2 text-sm font-medium text-slate-600 transition hover:text-orange-600">
          <ArrowLeft size={16} /> Về tài khoản
        </Link>
        <div className="mb-7 flex items-start gap-4">
          <span className="flex h-12 w-12 shrink-0 items-center justify-center rounded-xl bg-orange-100 text-orange-700"><Store size={23} /></span>
          <div>
            <h1 className="font-display text-2xl font-bold text-slate-950">Đăng ký kinh doanh nhà hàng</h1>
            <p className="mt-1 text-sm text-slate-600">Gửi thông tin nhà hàng để admin kiểm tra và phê duyệt.</p>
          </div>
        </div>

        {applicationQuery.isLoading ? (
          <div className="border-y border-slate-200 py-12 text-center text-sm text-slate-500">Đang kiểm tra hồ sơ...</div>
        ) : applicationQuery.isError || !applicationQuery.data?.isSuccess ? (
          <div role="alert" className="border-y border-red-200 py-5 text-sm text-red-700">
            <p>{applicationQuery.data?.message ?? 'Không kiểm tra được trạng thái hồ sơ.'}</p>
            <button type="button" onClick={() => void applicationQuery.refetch()} className="mt-2 font-semibold underline">Thử lại</button>
          </div>
        ) : hasPendingOrApproved ? (
          <ApplicationStatus application={application} state={applicationState} />
        ) : (
          <div className="grid gap-8 lg:grid-cols-[minmax(0,1fr)_260px]">
            <form onSubmit={handleSubmit} className="space-y-5 border-y border-slate-200 py-6">
              {applicationState === 'rejected' && application && (
                <div className="border-l-2 border-red-400 bg-red-50 px-4 py-3">
                  <p className="text-sm font-semibold text-red-800">Hồ sơ chưa được duyệt</p>
                  <p className="mt-1 text-sm text-red-700">{application.rejectionReason || 'Vui lòng kiểm tra và cập nhật lại thông tin.'}</p>
                </div>
              )}

              <div className="grid gap-4 sm:grid-cols-2">
                <Input label="Tên nhà hàng" required minLength={2} maxLength={120} value={name} onChange={(event) => setName(event.target.value)} placeholder="Tên hiển thị với khách hàng" fullWidth />
                <Input label="Số điện thoại nhà hàng" required type="tel" maxLength={10} value={phone} onChange={(event) => setPhone(event.target.value.replace(/\D/g, '').slice(0, 10))} placeholder="0901234567" fullWidth />
              </div>
              <div>
                <label className="mb-2 block text-sm font-medium text-slate-700">Địa chỉ và vị trí nhà hàng <span className="text-red-500">*</span></label>
                <AddressMapPicker
                  address={address}
                  position={latitude !== null && longitude !== null ? { lat: latitude, lng: longitude } : null}
                  onAddressInput={(value) => { setAddress(value); setLatitude(null); setLongitude(null); }}
                  onAddressSelected={(value, coordinates) => {
                    setAddress(value);
                    setLatitude(coordinates.lat);
                    setLongitude(coordinates.lng);
                  }}
                  onPositionSelected={(coordinates) => { setLatitude(coordinates.lat); setLongitude(coordinates.lng); }}
                />
              </div>
              <div className="grid gap-4 sm:grid-cols-2">
                <Input label="Giờ mở cửa" required type="time" value={openTime} onChange={(event) => setOpenTime(event.target.value)} fullWidth />
                <Input label="Giờ đóng cửa" required type="time" value={closeTime} onChange={(event) => setCloseTime(event.target.value)} fullWidth />
              </div>
              <div className="grid gap-4 sm:grid-cols-2">
                <Input label="Chủ nhà hàng" value={accountQuery.data?.data?.fullName ?? ''} disabled helperText="Lấy từ tài khoản đang đăng nhập" fullWidth />
                <Input label="Email liên hệ" value={accountQuery.data?.data?.email ?? ''} disabled helperText="Lấy từ tài khoản đang đăng nhập" fullWidth />
              </div>
              {error && <p role="alert" className="text-sm text-red-600">{error}</p>}
              <Button type="submit" size="lg" isLoading={registerMutation.isPending} leftIcon={<Store size={17} />}>Gửi hồ sơ đăng ký</Button>
            </form>

            <aside className="self-start border-t border-slate-200 py-5 lg:border-t-0 lg:border-l lg:pl-6">
              <h2 className="font-semibold text-slate-900">Hồ sơ cần có</h2>
              <ul className="mt-3 space-y-3 text-sm leading-6 text-slate-600">
                <li>Tên, địa chỉ và số điện thoại hoạt động của nhà hàng.</li>
                <li>Vị trí chính xác trên bản đồ để tính khoảng cách giao hàng.</li>
                <li>Giờ mở cửa và đóng cửa mỗi ngày.</li>
              </ul>
              <p className="mt-4 flex items-start gap-2 text-xs leading-5 text-slate-500"><Clock3 size={15} className="mt-0.5 shrink-0" />Nhà hàng chỉ được quản lý sản phẩm và nhận đơn sau khi admin duyệt.</p>
            </aside>
          </div>
        )}
      </main>
      <Footer />
    </div>
  );
}

function ApplicationStatus({
  application,
  state,
}: {
  application: RestaurantApplication | null;
  state: 'pending' | 'approved' | 'rejected' | null;
}) {
  if (!application) return null;
  const approved = state === 'approved';
  const rejected = state === 'rejected';
  const Icon = approved ? CheckCircle2 : rejected ? XCircle : Clock3;
  const colors = approved ? 'border-emerald-200 bg-emerald-50 text-emerald-900' : rejected ? 'border-red-200 bg-red-50 text-red-900' : 'border-amber-200 bg-amber-50 text-amber-900';

  return (
    <section className={`border p-5 sm:p-7 ${colors}`}>
      <div className="flex items-start gap-3">
        <Icon size={22} className="mt-0.5 shrink-0" />
        <div>
          <h2 className="font-semibold">{approved ? 'Nhà hàng đã được duyệt' : rejected ? 'Hồ sơ cần cập nhật' : 'Hồ sơ đang chờ admin duyệt'}</h2>
          <p className="mt-1 text-sm">{approved ? 'Bạn đã được cấp quyền quản lý nhà hàng. Đăng nhập lại để làm mới quyền truy cập nếu chưa vào được khu vực nhà hàng.' : rejected ? application.rejectionReason || 'Vui lòng gửi lại hồ sơ sau khi cập nhật thông tin.' : 'Admin sẽ kiểm tra thông tin nhà hàng trước khi cấp quyền kinh doanh.'}</p>
        </div>
      </div>
      <dl className="mt-5 grid gap-4 border-t border-current/10 pt-4 text-sm sm:grid-cols-2">
        <div><dt className="text-xs opacity-70">Nhà hàng</dt><dd className="mt-1 font-medium">{application.name}</dd></div>
        <div><dt className="text-xs opacity-70">Điện thoại</dt><dd className="mt-1 font-medium">{application.phone}</dd></div>
        <div><dt className="text-xs opacity-70">Địa chỉ</dt><dd className="mt-1 font-medium">{application.address}</dd></div>
        <div><dt className="text-xs opacity-70">Giờ hoạt động</dt><dd className="mt-1 font-medium">{displayTime(application.openTime)} - {displayTime(application.closeTime)}</dd></div>
      </dl>
      {rejected && <Link to="/restaurant/apply" className="mt-5 inline-flex text-sm font-semibold underline">Cập nhật và gửi lại hồ sơ</Link>}
    </section>
  );
}