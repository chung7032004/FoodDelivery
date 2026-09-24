import { useEffect, useState } from 'react';
import { Bike, CheckCircle2, Power } from 'lucide-react';
import { ShipperLayout } from './ShipperLayout';
import { PageHeader, Panel } from '@/components/admin/AdminUI';
import { ShipperProfile, shipperService } from '@/services/shipper.service';

export default function ShipperProfilePage() {
  const [profile, setProfile] = useState<ShipperProfile | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  async function load() { try { setProfile(await shipperService.getProfile()); } catch { setError('Không thể tải hồ sơ tài xế.'); } finally { setLoading(false); } }
  useEffect(() => { void load(); }, []);
  async function toggleAvailability() { if (!profile) return; try { await shipperService.updateAvailability(!profile.isAvailable); await load(); } catch { setError('Không thể cập nhật trạng thái.'); } }
  return <ShipperLayout><PageHeader eyebrow="Tài khoản tài xế" title="Hồ sơ tài xế" description="Kiểm tra trạng thái hoạt động và thông tin giao hàng của bạn." />{error && <p className="mb-4 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">{error}</p>}{loading ? <Panel><div className="p-10 text-center text-sm text-slate-500">Đang tải hồ sơ...</div></Panel> : profile && <div className="max-w-xl"><Panel title="Trạng thái tài khoản"><div className="space-y-5 p-5"><div className="flex items-center gap-4"><span className="flex h-14 w-14 items-center justify-center rounded-2xl bg-emerald-100 text-emerald-700"><Bike size={28} /></span><div><p className="font-display text-xl font-bold">{profile.isActive ? 'Tài khoản đang hoạt động' : 'Tài khoản bị tạm dừng'}</p><p className="mt-1 text-sm text-slate-500">Mã tài xế: {profile.shipperId}</p></div></div><div className="grid grid-cols-2 gap-3"><div className="rounded-xl bg-slate-50 p-4"><p className="text-xs text-slate-500">Đã giao thành công</p><strong className="mt-1 block text-xl">{profile.totalDeliveredOrders}</strong></div><div className="rounded-xl bg-slate-50 p-4"><p className="text-xs text-slate-500">Trạng thái nhận đơn</p><strong className="mt-1 block text-xl">{profile.isAvailable ? 'Bật' : 'Tắt'}</strong></div></div><button onClick={() => void toggleAvailability()} className={`flex w-full items-center justify-center gap-2 rounded-xl px-4 py-3 text-sm font-bold text-white ${profile.isAvailable ? 'bg-slate-700' : 'bg-emerald-600'}`}><Power size={17} />{profile.isAvailable ? 'Tắt nhận đơn' : 'Bật nhận đơn'}</button><p className="flex items-center gap-2 text-xs text-slate-500"><CheckCircle2 size={14} className="text-emerald-600" />Chỉ shipper đã được admin duyệt mới nhận được đơn.</p></div></Panel></div>}</ShipperLayout>;
}
