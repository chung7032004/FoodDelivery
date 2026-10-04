import { useEffect, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { ArrowLeft, CheckCircle2, Clock3, ShieldCheck, Truck } from 'lucide-react';
import { Link } from 'react-router-dom';
import { Footer } from '@/components/layout/Footer';
import { HeaderNav } from '@/components/layout/HeaderNav';
import Button from '@/components/ui/Button';
import Input from '@/components/ui/Input';
import { accountService } from '@/services/account.service';
import { shipperService, type ApplyShipperRequest, type ShipperApplicationStatus } from '@/services/shipper.service';
import { toast } from 'sonner';

const VEHICLE_TYPES = ['Xe máy', 'Ô tô', 'Xe đạp điện', 'Khác'];

function applicationStatus(status: number | string | null | undefined) {
  const value = String(status);
  if (value === '1' || value === 'Approved') return 'approved';
  if (value === '2' || value === 'Rejected') return 'rejected';
  return 'pending';
}

function formatDate(value: string | null | undefined) {
  return value
    ? new Intl.DateTimeFormat('vi-VN', { dateStyle: 'medium', timeStyle: 'short' }).format(new Date(value))
    : 'Chưa cập nhật';
}

export default function ShipperApplyPage() {
  const queryClient = useQueryClient();
  const [identityCard, setIdentityCard] = useState('');
  const [vehicleType, setVehicleType] = useState(VEHICLE_TYPES[0]);
  const [customVehicleType, setCustomVehicleType] = useState('');
  const [vehiclePlate, setVehiclePlate] = useState('');
  const [acceptedAccuracy, setAcceptedAccuracy] = useState(false);
  const [formError, setFormError] = useState('');

  const profileQuery = useQuery({ queryKey: ['my-account'], queryFn: accountService.getMe });
  const applicationQuery = useQuery({ queryKey: ['my-shipper-application'], queryFn: shipperService.getMyApplication });
  const application = applicationQuery.data?.data;
  const status = applicationStatus(application?.status);
  const canSubmit = !application?.hasApplication || status === 'rejected';

  useEffect(() => {
    if (!application || status !== 'rejected') return;
    setIdentityCard(application.identityCard ?? '');
    setVehiclePlate(application.vehiclePlate ?? '');
    if (application.vehicleType && VEHICLE_TYPES.includes(application.vehicleType)) {
      setVehicleType(application.vehicleType);
      setCustomVehicleType('');
    } else if (application.vehicleType) {
      setVehicleType('Khác');
      setCustomVehicleType(application.vehicleType);
    }
  }, [application, status]);

  const applyMutation = useMutation({
    mutationFn: async (request: ApplyShipperRequest) => {
      const result = await shipperService.apply(request);
      if (!result.isSuccess) throw new Error(result.message ?? 'Không thể gửi hồ sơ.');
      return result;
    },
    onSuccess: async (result) => {
      setFormError('');
      setAcceptedAccuracy(false);
      await queryClient.invalidateQueries({ queryKey: ['my-shipper-application'] });
      toast.success(result.message ?? 'Đã gửi hồ sơ đăng ký.');
    },
    onError: (error) => setFormError(error instanceof Error ? error.message : 'Không thể gửi hồ sơ.'),
  });

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setFormError('');
    const normalizedIdentityCard = identityCard.trim();
    const normalizedPlate = vehiclePlate.trim().toUpperCase();
    const normalizedVehicleType = vehicleType === 'Khác' ? customVehicleType.trim() : vehicleType;

    if (!/^(\d{9}|\d{12})$/.test(normalizedIdentityCard)) {
      setFormError('Số CCCD/CMND phải gồm đúng 9 hoặc 12 chữ số.');
      return;
    }
    if (normalizedVehicleType.length < 2 || normalizedVehicleType.length > 50) {
      setFormError('Vui lòng nhập loại phương tiện hợp lệ.');
      return;
    }
    if (!/^[0-9A-Z .-]{5,15}$/.test(normalizedPlate)) {
      setFormError('Biển số xe phải từ 5 đến 15 ký tự chữ hoặc số.');
      return;
    }
    if (!acceptedAccuracy) {
      setFormError('Vui lòng xác nhận thông tin đăng ký là chính xác.');
      return;
    }

    applyMutation.mutate({
      identityCard: normalizedIdentityCard,
      vehicleType: normalizedVehicleType,
      vehiclePlate: normalizedPlate,
    });
  }

  return (
    <div className="min-h-screen bg-surface font-sans">
      <HeaderNav />
      <main className="mx-auto min-h-[65vh] max-w-4xl px-4 py-10 sm:px-6 lg:px-8">
        <Link to="/account" className="mb-6 inline-flex items-center gap-2 text-sm font-medium text-slate-600 transition hover:text-orange-600">
          <ArrowLeft size={16} /> Về tài khoản
        </Link>

        <div className="mb-7 flex items-start gap-4">
          <span className="flex h-12 w-12 shrink-0 items-center justify-center rounded-xl bg-orange-100 text-orange-700"><Truck size={23} /></span>
          <div>
            <h1 className="font-display text-2xl font-bold text-slate-950">Đăng ký làm shipper</h1>
            <p className="mt-1 text-sm text-slate-600">Gửi thông tin giấy tờ và phương tiện để admin kiểm tra hồ sơ.</p>
          </div>
        </div>

        {applicationQuery.isLoading ? (
          <div className="border-y border-slate-200 py-12 text-center text-sm text-slate-500">Đang kiểm tra hồ sơ...</div>
        ) : applicationQuery.isError || !applicationQuery.data?.isSuccess ? (
          <div role="alert" className="border-y border-red-200 py-5 text-sm text-red-700">
            <p>{applicationQuery.data?.message ?? 'Không kiểm tra được trạng thái hồ sơ.'}</p>
            <button type="button" onClick={() => void applicationQuery.refetch()} className="mt-2 font-semibold underline">Thử lại</button>
          </div>
        ) : application?.hasApplication && !canSubmit ? (
          <ApplicationStatus application={application} status={status} />
        ) : (
          <div className="grid gap-8 lg:grid-cols-[minmax(0,1fr)_280px]">
            <form onSubmit={handleSubmit} className="space-y-5 border-y border-slate-200 py-6">
              {application?.hasApplication && status === 'rejected' && (
                <div className="border-l-2 border-red-400 bg-red-50 px-4 py-3">
                  <p className="text-sm font-semibold text-red-800">Hồ sơ trước chưa được duyệt</p>
                  <p className="mt-1 text-sm text-red-700">{application.rejectReason || 'Vui lòng kiểm tra và cập nhật lại thông tin.'}</p>
                </div>
              )}

              <div className="grid gap-4 sm:grid-cols-2">
                <Input label="Họ và tên" value={profileQuery.data?.data?.fullName ?? ''} disabled helperText="Lấy từ hồ sơ tài khoản" fullWidth />
                <Input label="Số điện thoại" value={profileQuery.data?.data?.phone ?? ''} disabled helperText="Lấy từ hồ sơ tài khoản" fullWidth />
              </div>
              <Input
                label="Số CCCD/CMND"
                required
                value={identityCard}
                onChange={(event) => setIdentityCard(event.target.value.replace(/\D/g, '').slice(0, 12))}
                inputMode="numeric"
                autoComplete="off"
                placeholder="Nhập 9 hoặc 12 chữ số"
                helperText="Số giấy tờ phải khớp với thông tin thật để admin đối chiếu."
                fullWidth
              />

              <div className="grid gap-4 sm:grid-cols-2">
                <label className="flex flex-col gap-1.5 text-sm font-medium text-slate-700">
                  Loại phương tiện <span className="text-red-500">*</span>
                  <select
                    required
                    value={vehicleType}
                    onChange={(event) => setVehicleType(event.target.value)}
                    className="h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-900 outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/15"
                  >
                    {VEHICLE_TYPES.map((type) => <option key={type} value={type}>{type}</option>)}
                  </select>
                </label>
                {vehicleType === 'Khác' ? (
                  <Input label="Tên phương tiện" required maxLength={50} value={customVehicleType} onChange={(event) => setCustomVehicleType(event.target.value)} placeholder="Nhập loại phương tiện" fullWidth />
                ) : (
                  <Input label="Biển số xe" required maxLength={15} value={vehiclePlate} onChange={(event) => setVehiclePlate(event.target.value.toUpperCase())} placeholder="Ví dụ: 59A1-123.45" autoComplete="off" fullWidth />
                )}
              </div>
              {vehicleType === 'Khác' && <Input label="Biển số xe" required maxLength={15} value={vehiclePlate} onChange={(event) => setVehiclePlate(event.target.value.toUpperCase())} placeholder="Ví dụ: 59A1-123.45" autoComplete="off" fullWidth />}

              <label className="flex cursor-pointer items-start gap-2.5 text-sm text-slate-700">
                <input type="checkbox" checked={acceptedAccuracy} onChange={(event) => setAcceptedAccuracy(event.target.checked)} className="mt-0.5 h-4 w-4 rounded border-slate-300 accent-orange-500" />
                <span>Tôi xác nhận thông tin cung cấp là chính xác và đồng ý để admin kiểm tra hồ sơ.</span>
              </label>
              {formError && <p role="alert" className="text-sm text-red-600">{formError}</p>}
              <Button type="submit" size="lg" isLoading={applyMutation.isPending} leftIcon={<ShieldCheck size={17} />}>
                Gửi hồ sơ đăng ký
              </Button>
            </form>

            <aside className="self-start border-t border-slate-200 py-5 lg:border-t-0 lg:border-l lg:pl-6">
              <h2 className="font-semibold text-slate-900">Thông tin cần chuẩn bị</h2>
              <ul className="mt-3 space-y-3 text-sm leading-6 text-slate-600">
                <li>CCCD/CMND hợp lệ gồm 9 hoặc 12 chữ số.</li>
                <li>Biển số xe đang sử dụng để giao hàng.</li>
                <li>Admin sẽ kiểm tra thông tin trước khi cấp quyền shipper.</li>
              </ul>
              <p className="mt-4 flex items-start gap-2 text-xs leading-5 text-slate-500"><Clock3 size={15} className="mt-0.5 shrink-0" />Trong thời gian chờ, bạn có thể quay lại trang này để xem trạng thái hồ sơ.</p>
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
  status,
}: {
  application: ShipperApplicationStatus;
  status: 'pending' | 'approved' | 'rejected';
}) {
  const isApproved = status === 'approved';
  const Icon = isApproved ? CheckCircle2 : Clock3;
  const color = isApproved ? 'border-emerald-200 bg-emerald-50 text-emerald-800' : 'border-amber-200 bg-amber-50 text-amber-900';

  return (
    <section className={`border p-5 sm:p-7 ${color}`}>
      <div className="flex items-start gap-3">
        <Icon size={22} className="mt-0.5 shrink-0" />
        <div>
          <h2 className="font-semibold">{isApproved ? 'Hồ sơ đã được duyệt' : 'Hồ sơ đang chờ admin duyệt'}</h2>
          <p className="mt-1 text-sm">{isApproved ? 'Bạn đã được cấp quyền shipper. Đăng nhập lại để làm mới quyền truy cập nếu chưa thấy khu vực shipper.' : 'Admin sẽ kiểm tra giấy tờ và thông tin phương tiện bạn đã gửi.'}</p>
        </div>
      </div>
      <dl className="mt-5 grid gap-4 border-t border-current/10 pt-4 text-sm sm:grid-cols-2">
        <div><dt className="text-xs opacity-70">CCCD/CMND</dt><dd className="mt-1 font-medium">{application.identityCard}</dd></div>
        <div><dt className="text-xs opacity-70">Phương tiện</dt><dd className="mt-1 font-medium">{application.vehicleType} · {application.vehiclePlate}</dd></div>
        <div><dt className="text-xs opacity-70">Ngày gửi</dt><dd className="mt-1 font-medium">{formatDate(application.appliedAt)}</dd></div>
        {application.reviewedAt && <div><dt className="text-xs opacity-70">Ngày duyệt</dt><dd className="mt-1 font-medium">{formatDate(application.reviewedAt)}</dd></div>}
      </dl>
      <Link to="/account" className="mt-5 inline-flex items-center gap-2 text-sm font-semibold underline"><ArrowLeft size={15} />Về tài khoản</Link>
    </section>
  );
}