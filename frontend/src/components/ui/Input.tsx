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
  md: 'h-11 text-sm',
  lg: 'h-[52px] text-base',
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
          <label htmlFor={inputId} className="text-sm font-semibold text-slate-800">
            {label}
            {required && <span className="ml-0.5 text-red-500">*</span>}
          </label>
        )}

        <div
          className={cn(
            'group flex items-center gap-2 rounded-xl border bg-slate-50/70 shadow-sm transition-all duration-150',
            'border-slate-200 hover:border-slate-300',
            'focus-within:border-orange-500 focus-within:bg-white focus-within:shadow-[0_0_0_4px_rgba(255,107,53,0.12)]',
            hasError &&
              'border-red-400 bg-red-50/40 focus-within:border-red-500 focus-within:shadow-[0_0_0_4px_rgba(239,68,68,0.12)]',
            disabled && 'cursor-not-allowed bg-slate-100 opacity-70 shadow-none',
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
              'h-full w-full flex-1 px-2 bg-transparent text-slate-900 outline-none placeholder:text-slate-400',
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