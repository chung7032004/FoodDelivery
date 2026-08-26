import { ButtonPrimary, ButtonGhost } from '../ui';
import type { HeroStat } from '../../types/index';
import { HERO_STATS } from '../../constants/mockData.ts';

export function HeroSection() {
  return (
    <section className="grid grid-cols-2 min-h-[420px] overflow-hidden">

      {/* ── Left: Dark content ── */}
      <div className="bg-[#1A1A1A] px-12 py-14 flex flex-col justify-center gap-5">

        {/* Live badge */}
        <div className="flex items-center gap-2 w-fit bg-brand/20 border border-brand/40 text-orange-300 text-xs font-medium px-4 py-1.5 rounded-full">
          <span className="w-1.5 h-1.5 rounded-full bg-orange-400 animate-pulse" />
          Giao hàng trong 30 phút
        </div>

        {/* Headline */}
        <h1 className="font-display text-5xl font-extrabold text-white leading-[1.1] tracking-tight">
          Đói bụng?<br />
          Gọi món<br />
          <span className="text-orange-400">ngay thôi</span>
        </h1>

        {/* Desc */}
        <p className="text-gray-400 text-sm leading-relaxed max-w-sm">
          Hơn 500 nhà hàng, 2000+ món ngon được giao tận cửa. Nhanh, tươi, đúng giờ.
        </p>

        {/* CTAs */}
        <div className="flex items-center gap-3 mt-2">
          <ButtonPrimary size="lg">Đặt ngay</ButtonPrimary>
          <ButtonGhost>Xem nhà hàng</ButtonGhost>
        </div>

        {/* Stats */}
        <div className="flex gap-8 mt-4 pt-6 border-t border-white/10">
          {HERO_STATS.map(({ value, label }: HeroStat) => (
            <div key={label}>
              <div className="font-display text-2xl font-bold text-white">{value}</div>
              <div className="text-xs text-gray-500 mt-0.5">{label}</div>
            </div>
          ))}
        </div>

      </div>

      {/* ── Right: Visual ── */}
      <div className="relative bg-[#F0EAE0] flex items-center justify-center overflow-hidden">

        {/* Glow */}
        <div className="absolute inset-0 bg-[radial-gradient(circle_at_60%_40%,rgba(232,68,26,0.18),transparent_60%)]" />

        {/* Main emoji */}
        <span className="text-[120px] leading-none animate-[float_3s_ease-in-out_infinite] relative z-10">
          🍜
        </span>

        {/* Hot badge */}
        <div className="absolute top-8 left-6 bg-brand text-white text-xs font-semibold px-3 py-2 rounded-xl flex items-center gap-1.5 shadow-md">
          🔥 Hôm nay hot
        </div>

        {/* Speed badge */}
        <div className="absolute bottom-10 right-8 bg-white rounded-2xl px-4 py-3 flex items-center gap-3 shadow-xl">
          <span className="text-3xl">⚡</span>
          <div>
            <div className="text-sm font-medium text-gray-900">Giao siêu tốc</div>
            <div className="text-xs text-gray-400 mt-0.5">Trung bình 25 phút</div>
          </div>
        </div>

      </div>

    </section>
  );
}