import { ButtonPrimary, ButtonGhost } from '../ui';
import type { HeroStat } from '../../types';
import { HERO_STATS } from '../../constants/mockData';

export function HeroSection() {
  return (
    <section className="grid min-h-[420px] grid-cols-1 overflow-hidden lg:grid-cols-2">

      {/* ── Left: Dark content ── */}
      <div className="flex flex-col justify-center gap-4 bg-[#1A1A1A] px-5 py-10 sm:gap-5 sm:px-8 sm:py-12 lg:px-12 lg:py-14">

        {/* Live badge */}
        <div className="flex w-fit items-center gap-2 rounded-full border border-brand/40 bg-brand/20 px-3 py-1.5 text-xs font-medium text-orange-300 sm:px-4">
          <span className="h-1.5 w-1.5 animate-pulse rounded-full bg-orange-400" />
          Giao hàng trong 30 phút
        </div>

        {/* Headline */}
        <h1 className="font-display text-3xl font-extrabold leading-[1.1] tracking-tight text-white sm:text-4xl lg:text-5xl">
          Đói bụng?<br />
          Gọi món<br />
          <span className="text-orange-400">ngay thôi</span>
        </h1>

        {/* Desc */}
        <p className="max-w-sm text-sm leading-relaxed text-gray-400">
          Hơn 500 nhà hàng, 2000+ món ngon được giao tận cửa. Nhanh, tươi, đúng giờ.
        </p>

        {/* CTAs */}
        <div className="mt-2 flex flex-wrap items-center gap-3">
          <ButtonPrimary size="lg">Đặt ngay</ButtonPrimary>
          <ButtonGhost>Xem nhà hàng</ButtonGhost>
        </div>

        {/* Stats */}
        <div className="mt-4 flex flex-wrap gap-6 border-t border-white/10 pt-6 sm:gap-8">
          {HERO_STATS.map(({ value, label }: HeroStat) => (
            <div key={label}>
              <div className="font-display text-xl font-bold text-white sm:text-2xl">
                {value}
              </div>
              <div className="mt-0.5 text-xs text-gray-500">{label}</div>
            </div>
          ))}
        </div>

      </div>

      {/* ── Right: Visual ── */}
      <div className="relative flex min-h-[280px] items-center justify-center overflow-hidden bg-[#F0EAE0] sm:min-h-[320px] lg:min-h-0">

        {/* Glow */}
        <div className="absolute inset-0 bg-[radial-gradient(circle_at_60%_40%,rgba(232,68,26,0.18),transparent_60%)]" />

        {/* Main emoji */}
        <span className="relative z-10 animate-[float_3s_ease-in-out_infinite] text-[80px] leading-none sm:text-[100px] lg:text-[120px]">
          🍜
        </span>

        {/* Hot badge */}
        <div className="absolute left-4 top-4 z-20 flex items-center gap-1.5 rounded-xl bg-brand px-2.5 py-1.5 text-xs font-semibold text-orange-500 shadow-md sm:left-6 sm:top-8 sm:px-3 sm:py-2">
          🔥 Hôm nay hot
        </div>

        {/* Speed badge */}
        <div className="absolute bottom-4 right-4 z-20 flex items-center gap-2 rounded-2xl bg-white px-3 py-2 shadow-xl sm:bottom-10 sm:right-8 sm:gap-3 sm:px-4 sm:py-3">
          <span className="text-2xl sm:text-3xl">⚡</span>
          <div>
            <div className="text-xs font-medium text-gray-900 sm:text-sm">Giao siêu tốc</div>
            <div className="mt-0.5 text-[10px] text-gray-400 sm:text-xs">Trung bình 25 phút</div>
          </div>
        </div>

      </div>

    </section>
  );
}