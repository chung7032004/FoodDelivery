import { InputHTMLAttributes, ReactNode, forwardRef, useId } from 'react';
import { cn } from '@/utils/cn';

export type InputSize = 'sm' | 'md' | 'lg';

export interface InputProps extends Omit<InputHTMLAttributes<HTMLInputElement>, 'size'> {
  label?: string;
  required?: boolean;
  error?: string;
  helperText?: string;
  leftIcon?: ReactNode;
  rightIcon?: ReactNode;
  onRightIconClick?: () => void;
  size?: InputSize;
  fullWidth?: boolean;
  containerClassName?: string;
}

const sizeClasses: Record<InputSize, string> = {
  sm: 'h-9 text-sm',
  md: 'h-10 text-sm',
  lg: 'h-12 text-base',
};

const Input = forwardRef<HTMLInputElement, InputProps>(
  (
    {
      label,
      required,
      error,
      helperText,
      leftIcon,
      rightIcon,
      onRightIconClick,
      size = 'md',
      fullWidth = false,
      disabled,
      className,
      containerClassName,
      id,
      ...rest
    },
    ref
  ) => {
    const generatedId = useId();
    const inputId = id ?? generatedId;
    const hasError = Boolean(error);

    return (
      <div className={cn('flex flex-col gap-1.5', fullWidth && 'w-full', containerClassName)}>
        {label && (
          <label htmlFor={inputId} className="text-sm font-medium text-slate-700">
            {label}
            {required && <span className="ml-0.5 text-red-500">*</span>}
          </label>
        )}

        <div
          className={cn(
            'group flex items-center gap-2 rounded-lg border bg-white transition-colors duration-150',
            'border-slate-200 hover:border-slate-300',
            'focus-within:border-orange-500 focus-within:ring-2 focus-within:ring-orange-500/15',
            hasError &&
              'border-red-400 bg-red-50/40 focus-within:border-red-500 focus-within:ring-red-500/15',
            disabled && 'cursor-not-allowed bg-slate-100 opacity-70',
            sizeClasses[size]
          )}
        >
          {leftIcon && (
            <span
              className={cn(
                'flex items-center px-2 text-slate-400 transition-colors',
                'group-focus-within:text-orange-500',
                hasError && 'group-focus-within:text-red-500'
              )}
            >
              {leftIcon}
            </span>
          )}

          <input
            ref={ref}
            id={inputId}
            disabled={disabled}
            aria-invalid={hasError}
            className={cn(
              'h-full w-full flex-1 bg-transparent px-3 text-slate-900 outline-none placeholder:text-slate-400',
              'disabled:cursor-not-allowed disabled:text-slate-400',
              className
            )}
            {...rest}
          />

          {rightIcon && (
            <span
              onClick={onRightIconClick}
              role={onRightIconClick ? 'button' : undefined}
              tabIndex={onRightIconClick ? 0 : undefined}
              className={cn(
                'flex items-center pr-2 text-slate-400 transition-colors',
                onRightIconClick && 'cursor-pointer hover:text-slate-700'
              )}
            >
              {rightIcon}
            </span>
          )}
        </div>

        {error ? (
          <span className="flex items-center gap-1 text-xs font-medium text-red-500">{error}</span>
        ) : helperText ? (
          <span className="text-xs text-slate-500">{helperText}</span>
        ) : null}
      </div>
    );
  }
);

Input.displayName = 'Input';
export default Input;