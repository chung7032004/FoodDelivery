// src/components/Badge.tsx
import { HTMLAttributes, ReactNode } from 'react';
import { cn } from '@/utils/cn';

export type BadgeVariant = 'neutral' | 'success' | 'warning' | 'error' | 'info' | 'primary';

interface BadgeProps extends HTMLAttributes<HTMLSpanElement> {
  variant?: BadgeVariant;
  dot?: boolean;
  children: ReactNode;
}

const variantClasses: Record<BadgeVariant, string> = {
  neutral: 'bg-gray-100 text-gray-600',
  success: 'bg-emerald-50 text-emerald-600',
  warning: 'bg-amber-50 text-amber-700',
  error: 'bg-red-50 text-red-600',
  info: 'bg-blue-50 text-blue-600',
  primary: 'bg-orange-50 text-orange-600',
};

export default function Badge({ variant = 'neutral', dot = false, children, className, ...rest }: BadgeProps) {
  return (
    <span
      className={cn(
        'inline-flex items-center gap-1.5 rounded-full px-2.5 py-0.5 text-xs font-semibold whitespace-nowrap',
        variantClasses[variant],
        className
      )}
      {...rest}
    >
      {dot && <span className="h-1.5 w-1.5 rounded-full bg-current" />}
      {children}
    </span>
  );
}