// src/components/Skeleton.tsx
import { CSSProperties } from 'react';
import { cn } from '@/utils/cn';

export type SkeletonShape = 'text' | 'circle' | 'rect';

interface SkeletonProps {
  shape?: SkeletonShape;
  width?: number | string;
  height?: number | string;
  className?: string;
  style?: CSSProperties;
}

const shapeClasses: Record<SkeletonShape, string> = {
  text: 'rounded-md',
  circle: 'rounded-full',
  rect: 'rounded-xl',
};

export default function Skeleton({ shape = 'text', width, height, className, style }: SkeletonProps) {
  const defaultHeight = shape === 'text' ? 14 : shape === 'circle' ? 40 : 80;
  const defaultWidth = shape === 'circle' ? defaultHeight : '100%';

  return (
    <span
      className={cn(
        'block animate-pulse bg-gray-200',
        shapeClasses[shape],
        className
      )}
      style={{ width: width ?? defaultWidth, height: height ?? defaultHeight, ...style }}
    />
  );
}