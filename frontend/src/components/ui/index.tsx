// ─── Badge ───────────────────────────────────────────────────────
interface BadgeProps {
  label: string;
  color?: string;
}

export function Badge({ label, color = '#E8441A' }: BadgeProps) {
  return (
    <div
      className="absolute top-3 left-3 px-3 py-1 rounded-full text-white text-xs font-bold"
      style={{ backgroundColor: color }}
    >
      {label}
    </div>
  );
}

// ─── StarRating ──────────────────────────────────────────────────
interface StarRatingProps {
  rating: number;
  reviewCount?: number;
  size?: 'sm' | 'md' | 'lg';
}

export function StarRating({ rating, reviewCount, size = 'md' }: StarRatingProps) {
  const sizeClass = size === 'sm' ? 'text-xs' : size === 'lg' ? 'text-lg' : 'text-sm';
  return (
    <div className={`flex items-center gap-1 ${sizeClass}`}>
      <span>⭐ {rating}</span>
      {reviewCount && <span className="text-gray-400">({reviewCount})</span>}
    </div>
  );
}

// ─── ButtonPrimary ───────────────────────────────────────────────
interface ButtonPrimaryProps {
  children: React.ReactNode;
  onClick?: () => void;
  size?: 'sm' | 'md' | 'lg';
  className?: string;
}

export function ButtonPrimary({ children, onClick, size = 'md', className = '' }: ButtonPrimaryProps) {
  const sizeClass =
    size === 'sm' ? 'px-4 py-2 text-sm' : size === 'lg' ? 'px-6 py-3 text-base' : 'px-5 py-2.5 text-sm';

  return (
    <button
      onClick={onClick}
      className={`bg-brand text-white rounded-full font-medium hover:bg-brand-dark transition-colors cursor-pointer ${sizeClass} ${className}`}
    >
      {children}
    </button>
  );
}

// ─── ButtonGhost ─────────────────────────────────────────────────
interface ButtonGhostProps {
  children: React.ReactNode;
  onClick?: () => void;
  size?: 'sm' | 'md' | 'lg';
  className?: string;
}

export function ButtonGhost({ children, onClick, size = 'md', className = '' }: ButtonGhostProps) {
  const sizeClass =
    size === 'sm' ? 'px-4 py-2 text-sm' : size === 'lg' ? 'px-6 py-3 text-base' : 'px-5 py-2.5 text-sm';

  return (
    <button
      onClick={onClick}
      className={`border border-brand text-brand rounded-full font-medium hover:bg-brand/10 transition-colors cursor-pointer ${sizeClass} ${className}`}
    >
      {children}
    </button>
  );
}

// ─── PromoSection ─────────────────────────────────────────────────
export function PromoSection() {
  return (
    <section className="px-8 pb-12 max-w-7xl mx-auto">
      <div className="grid grid-cols-[2fr_1fr] gap-4">

        {/* Big promo banner */}
        <div className="relative bg-[#1A1A1A] rounded-2xl p-8 flex items-center justify-between overflow-hidden">
          {/* Glow blob */} 
          <div className="absolute right-0 bottom-0 w-56 h-56 bg-brand/20 rounded-full blur-3xl translate-x-1/3 translate-y-1/3 pointer-events-none" />

          <div className="relative z-10">
            <p className="text-xs font-semibold text-orange-400 uppercase tracking-widest mb-2">
              Ưu đãi đặc biệt
            </p>
            <h3 className="font-display text-2xl font-bold text-white leading-snug mb-5">
              Miễn phí giao hàng<br />
              <span className="text-orange-400">đơn đầu tiên</span>
            </h3>
            <button className="bg-brand text-white px-5 py-2.5 rounded-full text-sm font-medium hover:bg-brand-dark transition-colors cursor-pointer">
              Dùng ngay
            </button>
          </div>

          <span className="text-[80px] leading-none opacity-90 relative z-10">🎁</span>
        </div>

        {/* Small promos stack */}
        <div className="flex flex-col gap-3">

          <SmallPromoCard
            title="Combo Trưa tiết kiệm"
            subtitle="Từ 45.000đ · Giao nhanh"
            btnLabel="Xem combo →"
            bgColor="bg-brand"
          />

          <SmallPromoCard
            title="Tích điểm · Đổi quà"
            subtitle="Mỗi đơn tích 1 điểm thưởng"
            btnLabel="Tham gia →"
            bgColor="bg-green-600"
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
  bgColor: string;
}

function SmallPromoCard({ title, subtitle, btnLabel, bgColor }: SmallPromoCardProps) {
  return (
    <div className={`${bgColor} rounded-2xl p-5 flex flex-col justify-between flex-1 text-white`}>
      <div>
        <h4 className="font-display font-bold text-sm leading-snug">{title}</h4>
        <p className="text-xs opacity-75 mt-1 mb-4">{subtitle}</p>
      </div>
      <button className="bg-white/20 hover:bg-white/30 text-white text-xs font-medium px-4 py-2 rounded-xl w-fit cursor-pointer transition-colors">
        {btnLabel}
      </button>
    </div>
  );
}