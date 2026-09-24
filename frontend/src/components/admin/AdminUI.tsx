import { ReactNode } from 'react';
import { ArrowUpRight, ChevronRight } from 'lucide-react';
import type { LucideIcon } from 'lucide-react';
import { Link } from 'react-router-dom';

export function PageHeader({ eyebrow, title, description, action }: { eyebrow?: string; title: string; description?: string; action?: ReactNode }) {
  return <div className="mb-7 flex flex-wrap items-end justify-between gap-4"><div><p className="mb-2 text-xs font-bold uppercase tracking-[0.18em] text-orange-600">{eyebrow || 'EatRush / Admin'}</p><h1 className="font-display text-3xl font-bold tracking-tight sm:text-4xl">{title}</h1>{description && <p className="mt-2 max-w-2xl text-sm leading-6 text-muted">{description}</p>}</div>{action}</div>;
}

export function Panel({ title, subtitle, action, children, className = '' }: { title?: string; subtitle?: string; action?: ReactNode; children: ReactNode; className?: string }) {
  return <section className={`border border-border bg-surface ${className}`}><div className="flex items-center justify-between gap-4 border-b border-border px-5 py-4">{title ? <div><h2 className="font-display text-lg font-bold">{title}</h2>{subtitle && <p className="mt-0.5 text-xs text-muted">{subtitle}</p>}</div> : <span />}{action}</div>{children}</section>;
}

export function StatCard({ label, value, detail, icon: Icon, tone = 'orange', href }: { label: string; value: string | number; detail: string; icon: LucideIcon; tone?: 'orange' | 'blue' | 'green' | 'purple'; href?: string }) {
  const tones = { orange: 'bg-orange-500/10 text-orange-600', blue: 'bg-sky-500/10 text-sky-600', green: 'bg-emerald-500/10 text-emerald-600', purple: 'bg-violet-500/10 text-violet-600' };
  const content = <div className="group border border-border bg-surface p-5 transition hover:-translate-y-0.5 hover:border-orange-500/50 hover:shadow-[0_12px_30px_rgba(30,20,10,0.08)]"><div className="mb-5 flex items-start justify-between"><span className={`rounded-xl p-3 ${tones[tone]}`}><Icon size={21} strokeWidth={1.8} /></span>{href && <ArrowUpRight size={19} className="text-muted transition group-hover:-translate-y-0.5 group-hover:translate-x-0.5 group-hover:text-orange-500" />}</div><p className="font-display text-3xl font-bold tracking-tight">{value}</p><p className="mt-1 text-sm font-semibold">{label}</p><p className="mt-2 text-xs text-muted">{detail}</p></div>;
  return href ? <Link to={href}>{content}</Link> : content;
}

export function ProgressBar({ label, value, color = 'bg-orange-500' }: { label: string; value: number; color?: string }) { return <div><div className="mb-2 flex justify-between text-xs font-semibold"><span>{label}</span><span className="text-muted">{value}%</span></div><div className="h-2 bg-black/5"><div className={`h-full ${color}`} style={{ width: `${value}%` }} /></div></div>; }
export function PanelLink({ to, children }: { to: string; children: ReactNode }) { return <Link to={to} className="flex items-center gap-1 text-xs font-bold text-orange-600 hover:text-orange-700">{children}<ChevronRight size={15} /></Link>; }
export function LoadingState() { return <div className="px-5 py-10 text-center text-sm text-muted">Đang tải dữ liệu...</div>; }
