// src/components/layout/AuthLayout.tsx
import { ReactNode } from 'react';
import { Truck, Clock, ShieldCheck, Sparkles } from 'lucide-react';

interface AuthLayoutProps {
  title: string;
  subtitle: string;
  children: ReactNode;
}

export function AuthLayout({ title, subtitle, children }: AuthLayoutProps) {
  return (
    <div className="grid min-h-screen bg-white font-sans lg:grid-cols-2">
      {/* Cột form */}
      <div className="flex items-center justify-center px-6 py-12 sm:px-10">
        <div className="w-full max-w-sm">
          <div className="mb-8 flex items-center gap-2">
            <div className="flex h-9 w-9 items-center justify-center rounded-xl bg-orange-500 text-white shadow-md shadow-orange-500/25">
              <Truck size={18} />
            </div>
            <span className="text-lg font-bold text-slate-900">FastFood</span>
          </div>

          <h1 className="text-[26px] font-bold leading-tight text-slate-900">{title}</h1>
          <p className="mt-1.5 text-[15px] text-slate-500">{subtitle}</p>

          <div className="mt-8">{children}</div>
        </div>
      </div>

      {/* Cột thương hiệu */}
      <div className="relative hidden overflow-hidden bg-gradient-to-br from-orange-400 via-orange-500 to-orange-600 lg:flex">
        {/* Blob trang trí */}
        <div className="absolute -right-24 -top-24 h-72 w-72 rounded-full bg-white/10 blur-[2px]" />
        <div className="absolute -bottom-16 -left-10 h-56 w-56 rounded-full bg-white/10" />
        <div className="absolute right-1/4 top-1/3 h-24 w-24 rounded-full bg-white/5" />

        <div className="relative z-10 flex w-full flex-col justify-center px-14 py-16 text-white xl:px-16">
          <div className="mb-6 inline-flex w-fit items-center gap-1.5 rounded-full bg-white/15 px-3.5 py-1.5 text-xs font-semibold backdrop-blur-sm">
            <Sparkles size={13} />
            Giao siêu tốc trong nội thành
          </div>

          <h2 className="max-w-md text-[34px] font-bold leading-[1.25] xl:text-4xl">
            Đói bụng? Đặt món trong 30 giây, giao tận nơi trong 20 phút.
          </h2>

          <div className="mt-9 space-y-4">
            <Feature icon={<Clock size={17} />} text="Giao siêu tốc, theo dõi realtime" />
            <Feature icon={<ShieldCheck size={17} />} text="Thanh toán an toàn, hoàn tiền nếu trễ" />
            <Feature icon={<Truck size={17} />} text="Hàng ngàn quán ăn đối tác trên toàn quốc" />
          </div>
        </div>

        <p className="absolute bottom-6 left-14 z-10 text-xs text-white/60">
          © {new Date().getFullYear()} FastFood. All rights reserved.
        </p>
      </div>
    </div>
  );
}

function Feature({ icon, text }: { icon: ReactNode; text: string }) {
  return (
    <div className="flex items-center gap-3">
      <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl bg-white/15 backdrop-blur-sm">
        {icon}
      </div>
      <span className="text-[15px] text-white/90">{text}</span>
    </div>
  );
}