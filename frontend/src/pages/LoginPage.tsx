// src/pages/LoginPage.tsx
import { useState, useCallback } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { AxiosError } from 'axios';
import { Mail, Lock, Eye, EyeOff } from 'lucide-react';
import { AuthLayout } from '../components/layout/AuthLayout';
import Input from '@/components/ui/Input';
import Button from '@/components/ui/Button';
import { authService } from '@/services/auth.service';
import { ApiErrorResponse } from '@/services/api';

interface LoginFormData {
  email: string;
  password: string;
}

interface LoginErrors {
  email?: string;
  password?: string;
  form?: string;
}


function validateLogin(data: LoginFormData): LoginErrors {
  const errors: LoginErrors = {};
  if (!data.email.trim()) {
    errors.email = 'Vui lòng nhập email';
  } else if (!/^\S+@\S+\.\S+$/.test(data.email)) {
    errors.email = 'Email không hợp lệ';
  }
  if (!data.password) {
    errors.password = 'Vui lòng nhập mật khẩu';
  } else if (data.password.length < 6) {
    errors.password = 'Mật khẩu tối thiểu 6 ký tự';
  }
  return errors;
}

// Map errorCode backend -> field lỗi tương ứng trên form
function mapErrorToFormErrors(errorCode: string | null, message: string | null): LoginErrors {
  const fallbackMessage = message || 'Đã xảy ra lỗi, vui lòng thử lại';
  switch (errorCode) {
    case 'INVALID_INPUT':
      return { form: fallbackMessage };
    case 'USER_NOT_FOUND':
      return { email: fallbackMessage };
    case 'USER_INACTIVE':
      return { form: fallbackMessage };
    case 'INVALID_PASSWORD':
      // Không tiết lộ email tồn tại hay không -> hiện lỗi chung ở form, không gắn riêng field password
      return { form: fallbackMessage };
    default:
      return { form: fallbackMessage };
  }
}

export function LoginPage() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<LoginFormData>({ email: '', password: '' });
  const [errors, setErrors] = useState<LoginErrors>({});
  const [showPassword, setShowPassword] = useState(false);
  const [rememberMe, setRememberMe] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const handleChange = useCallback(
    (field: keyof LoginFormData) => (e: React.ChangeEvent<HTMLInputElement>) => {
      setFormData((prev) => ({ ...prev, [field]: e.target.value }));
      setErrors((prev) => ({ ...prev, [field]: undefined, form: undefined }));
    },
    []
  );

  const handleSubmit = useCallback(
    async (e: React.FormEvent) => {
      e.preventDefault();
      if (isLoading) return; // chặn double submit

      const validationErrors = validateLogin(formData);
      if (Object.keys(validationErrors).length > 0) {
        setErrors(validationErrors);
        return;
      }

      setIsLoading(true);
      setErrors({});
      try {
        const {user} = await authService.login({
          email: formData.email.trim(),
          password: formData.password,
        });
         if (user?.roles.includes('Admin')) {
          navigate('/admin', { replace: true });
        } else {
          navigate('/', { replace: true });
        }
      } catch (err) {
        const axiosError = err as AxiosError<ApiErrorResponse>;

        // Không có response -> lỗi network/timeout/server down, không phải lỗi sai mật khẩu
        if (!axiosError.response) {
          setErrors({ form: 'Không thể kết nối tới máy chủ. Vui lòng kiểm tra mạng và thử lại.' });
          setFormData((prev) => ({ ...prev, password: '' }));
          return;
        }

        const { errorCode, message } = axiosError.response.data ?? {
          errorCode: null,
          message: null,
        };

        setErrors(mapErrorToFormErrors(errorCode, message));

        // Xóa password sau khi login sai để tránh lộ trên màn hình
        if (errorCode === 'INVALID_PASSWORD' || errorCode === 'USER_NOT_FOUND') {
          setFormData((prev) => ({ ...prev, password: '' }));
        }
      } finally {
        setIsLoading(false);
      }
    },
    [formData, navigate, isLoading]
  );

  return (
    <AuthLayout title="Đăng nhập" subtitle="Chào mừng quay lại! Đăng nhập để tiếp tục đặt món.">
      <form onSubmit={handleSubmit} className="space-y-4" noValidate>
        {errors.form && (
          <div
            role="alert"
            className="rounded-xl border border-red-100 bg-red-50 px-3.5 py-2.5 text-sm text-red-600"
          >
            {errors.form}
          </div>
        )}

        <Input
          label="Email"
          type="email"
          placeholder="you@example.com"
          value={formData.email}
          onChange={handleChange('email')}
          error={errors.email}
          leftIcon={<Mail size={16} />}
          fullWidth
          autoComplete="email"
          disabled={isLoading}
        />

        <Input
          label="Mật khẩu"
          type={showPassword ? 'text' : 'password'}
          placeholder="••••••••"
          value={formData.password}
          onChange={handleChange('password')}
          error={errors.password}
          leftIcon={<Lock size={16} />}
          rightIcon={showPassword ? <EyeOff size={16} /> : <Eye size={16} />}
          onRightIconClick={() => setShowPassword((prev) => !prev)}
          fullWidth
          autoComplete="current-password"
          disabled={isLoading}
        />

        <div className="flex items-center justify-between pt-0.5">
          <label className="flex cursor-pointer items-center gap-2 text-sm text-slate-600">
            <input
              type="checkbox"
              checked={rememberMe}
              onChange={(e) => setRememberMe(e.target.checked)}
              className="h-4 w-4 rounded border-slate-300 text-orange-500 focus:ring-orange-500/30"
              disabled={isLoading}
            />
            Ghi nhớ đăng nhập
          </label>
          <Link to="/send-otp" className="text-sm font-medium text-orange-500 hover:text-orange-600">
            Quên mật khẩu?
          </Link>
        </div>

        <Button type="submit" variant="primary" size="lg" fullWidth isLoading={isLoading}>
          Đăng nhập
        </Button>

        <p className="pt-1 text-center text-sm text-slate-500">
          Chưa có tài khoản?{' '}
          <Link to="/register" className="font-semibold text-orange-500 hover:text-orange-600">
            Đăng ký ngay
          </Link>
        </p>
      </form>
    </AuthLayout>
  );
}