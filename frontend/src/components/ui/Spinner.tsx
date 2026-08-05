// src/components/Spinner.tsx
import { cn } from '@/utils/cn';

export type SpinnerSize = 'sm' | 'md' | 'lg';

interface SpinnerProps {
  size?: SpinnerSize;
  inheritColor?: boolean;
  className?: string;
}

const sizeMap: Record<SpinnerSize, string> = {
  sm: 'h-3.5 w-3.5 border-2',
  md: 'h-5 w-5 border-[2.5px]',
  lg: 'h-8 w-8 border-[3px]',
};

export default function Spinner({ size = 'md', inheritColor = false, className }: SpinnerProps) {
  return (
    <span
      role="status"
      aria-label="Đang tải..."
      className={cn(
        'inline-block animate-spin rounded-full border-current border-r-transparent',
        inheritColor ? 'text-inherit' : 'text-orange-500',
        sizeMap[size],
        className
      )}
    />
  );
}