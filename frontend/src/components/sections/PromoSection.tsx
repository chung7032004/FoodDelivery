import { Link } from 'react-router-dom';
import { cn } from '@/utils/cn';

// ─── PromoSection ─────────────────────────────────────────────────
export function PromoSection() {
  return (
    <section className="mx-auto max-w-7xl px-4 pb-12 sm:px-6 lg:px-8">
      <div className="grid grid-cols-1 gap-3 sm:gap-4 lg:grid-cols-[2fr_1fr]">

        {/* Big promo banner */}
        <div className="relative flex items-center justify-between overflow-hidden rounded-2xl bg-[#1A1A1A] p-5 sm:p-6 lg:p-8">
          {/* Glow blob */}
          <div className="pointer-events-none absolute bottom-0 right-0 h-40 w-40 translate-x-1/3 translate-y-1/3 rounded-full bg-brand/20 blur-3xl sm:h-56 sm:w-56" />

          <div className="relative z-10">
            <p className="mb-2 text-[10px] font-semibold uppercase tracking-widest text-orange-400 sm:text-xs">
              Ưu đãi đặc biệt
            </p>
            <h3 className="mb-4 font-display text-xl font-bold leading-snug text-white sm:text-2xl sm:mb-5">
              Miễn phí giao hàng
              <br />
              <span className="text-orange-400">đơn đầu tiên</span>
            </h3>
            <Link
              to="/promotions"
              className="inline-block rounded-full bg-brand px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-orange-600 sm:px-5 sm:py-2.5"
            >
              Dùng ngay
            </Link>
          </div>

          <span className="relative z-10 ml-4 text-[56px] leading-none opacity-90 sm:text-[80px]">
            🎁
          </span>
        </div>

        {/* Small promos stack */}
        <div className="flex flex-col gap-3">
          <SmallPromoCard
            title="Combo Trưa tiết kiệm"
            subtitle="Từ 45.000đ · Giao nhanh"
            btnLabel="Xem combo →"
            bgColor="bg-brand"
            to="/promotions"
          />
          <SmallPromoCard
            title="Tích điểm · Đổi quà"
            subtitle="Mỗi đơn tích 1 điểm thưởng"
            btnLabel="Tham gia →"
            bgColor="bg-green-600"
            to="/promotions"
          />
        </div>
      </div>
    </section>
  );
}

// ─── SmallPromoCard ───────────────────────────────────────────────
interface SmallPromoCardProps {
  title: string;
  subtitle: string;
  btnLabel: string;
  /** Class Tailwind nền — phải là full class (vd 'bg-brand'), không dynamic. */
  bgColor: string;
  to: string;
}

function SmallPromoCard({ title, subtitle, btnLabel, bgColor, to }: SmallPromoCardProps) {
  return (
    <div className={cn('flex flex-1 flex-col justify-between rounded-2xl p-5 text-white', bgColor)}>
      <div>
        <h4 className="font-display text-sm font-bold leading-snug">{title}</h4>
        <p className="mt-1 mb-4 text-xs opacity-75">{subtitle}</p>
      </div>
      <Link
        to={to}
        className="w-fit rounded-xl bg-white/20 px-4 py-2 text-xs font-medium text-white transition-colors hover:bg-white/30"
      >
        {btnLabel}
      </Link>
    </div>
  );
}