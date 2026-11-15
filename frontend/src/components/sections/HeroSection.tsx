import { FormEvent } from 'react';
import { ArrowDown, ArrowRight, Clock3, Search, ShieldCheck, Sparkles } from 'lucide-react';
import { Link } from 'react-router-dom';

interface HeroSectionProps {
  searchTerm: string;
  onSearchChange: (value: string) => void;
  onSearch: () => void;
}

export function HeroSection({ searchTerm, onSearchChange, onSearch }: HeroSectionProps) {
  function submitSearch(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    onSearch();
  }

  return (
    <section className="relative isolate overflow-hidden bg-[#fff8f2]">
      <div className="pointer-events-none absolute -right-32 -top-40 -z-10 h-[34rem] w-[34rem] rounded-full bg-orange-200/50 blur-3xl" />
      <div className="pointer-events-none absolute -bottom-48 left-1/4 -z-10 h-96 w-96 rounded-full bg-amber-100/70 blur-3xl" />

      <div className="mx-auto grid max-w-7xl items-center gap-8 px-4 py-12 sm:px-6 sm:py-16 lg:grid-cols-[1.05fr_.95fr] lg:gap-12 lg:px-8 lg:py-20">
        <div className="relative z-10">
          <div className="mb-5 inline-flex items-center gap-2 rounded-full border border-orange-200 bg-white/80 px-3.5 py-2 text-xs font-semibold text-orange-700 shadow-sm">
            <Sparkles size={15} />
            Món ngon giao đến tận cửa
          </div>

          <h1 className="max-w-2xl font-display text-4xl font-extrabold leading-[1.08] tracking-tight text-slate-950 sm:text-5xl lg:text-[3.75rem]">
            Hôm nay ăn gì?
            <span className="mt-1 block text-[#e8441a]">Để EatRush tìm giúp bạn.</span>
          </h1>
          <p className="mt-5 max-w-xl text-base leading-7 text-slate-600 sm:text-lg">
            Khám phá thực đơn quanh bạn, chọn món yêu thích và đặt hàng chỉ với vài thao tác.
          </p>

          <form
            onSubmit={submitSearch}
            className="mt-8 flex max-w-xl flex-col gap-2 rounded-2xl border border-orange-100 bg-white p-2 shadow-[0_16px_50px_rgba(114,61,24,0.10)] sm:flex-row sm:items-center"
          >
            <label htmlFor="home-product-search" className="sr-only">Tìm món ăn</label>
            <div className="flex min-w-0 flex-1 items-center gap-3 px-3">
              <Search size={20} className="shrink-0 text-orange-500" />
              <input
                id="home-product-search"
                type="search"
                value={searchTerm}
                onChange={(event) => onSearchChange(event.target.value)}
                placeholder="Bạn muốn ăn món gì?"
                className="h-11 w-full min-w-0 bg-transparent text-sm text-slate-800 outline-none placeholder:text-slate-400"
              />
            </div>
            <button
              type="submit"
              className="inline-flex h-11 items-center justify-center gap-2 rounded-xl bg-[#e8441a] px-5 text-sm font-bold text-white transition hover:bg-[#cc3a15] focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-orange-500 focus-visible:ring-offset-2"
            >
              Tìm món <ArrowRight size={17} />
            </button>
          </form>

          <div className="mt-7 flex flex-wrap gap-x-6 gap-y-3 text-sm text-slate-600">
            <span className="inline-flex items-center gap-2">
              <Clock3 size={17} className="text-orange-500" />
              Theo dõi trạng thái đơn
            </span>
            <span className="inline-flex items-center gap-2">
              <ShieldCheck size={17} className="text-orange-500" />
              Thanh toán khi nhận hoặc VNPay
            </span>
          </div>
        </div>

        <div className="relative mx-auto flex min-h-[300px] w-full max-w-xl items-center justify-center sm:min-h-[380px]">
          <div className="absolute h-[75%] w-[75%] rounded-full bg-gradient-to-br from-orange-200 via-orange-100 to-amber-50 shadow-[0_30px_90px_rgba(232,68,26,0.15)]" />
          <div className="absolute h-[62%] w-[62%] rounded-full border border-white/80" />
          <div className="relative z-10 flex h-48 w-48 items-center justify-center rounded-full border-[10px] border-white bg-gradient-to-br from-amber-50 to-orange-100 text-[6rem] shadow-[0_24px_70px_rgba(81,43,20,0.18)] sm:h-60 sm:w-60 sm:text-[7.5rem]">
            🍜
          </div>

          <div className="absolute left-0 top-[15%] z-20 rounded-2xl border border-white/80 bg-white/95 px-4 py-3 shadow-xl sm:left-4">
            <p className="text-[11px] font-medium text-slate-500">Gợi ý hôm nay</p>
            <p className="mt-0.5 text-sm font-bold text-slate-900">Món ngon đang chờ bạn</p>
          </div>
          <div className="absolute bottom-[13%] right-0 z-20 flex items-center gap-3 rounded-2xl border border-white/80 bg-white/95 px-4 py-3 shadow-xl sm:right-2">
            <span className="flex h-10 w-10 items-center justify-center rounded-xl bg-orange-50 text-xl">🛵</span>
            <div>
              <p className="text-sm font-bold text-slate-900">Đặt món dễ dàng</p>
              <p className="mt-0.5 text-xs text-slate-500">Chọn món, theo dõi đơn</p>
            </div>
          </div>
          <Link
            to="/products"
            className="absolute bottom-0 left-1/2 z-20 inline-flex -translate-x-1/2 items-center gap-2 rounded-full border border-white bg-white/90 px-4 py-2 text-xs font-semibold text-slate-700 shadow-md transition hover:-translate-y-0.5 hover:text-orange-600"
          >
            Khám phá thực đơn <ArrowDown size={14} />
          </Link>
        </div>
      </div>
    </section>
  );
}
