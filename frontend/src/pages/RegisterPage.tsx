import { useState, useCallback } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Mail, Lock, User, Phone, Eye, EyeOff } from 'lucide-react';
import { AuthLayout } from '../components/layout/AuthLayout';
import  Input  from '../components/ui/Input';
import  Button  from '../components/ui/Button';
import { authService } from '@/services/auth.service';
import axios from 'axios';

interface RegisterFormData {
  fullName: string;
  email: string;
  phone: string;
  password: string;
  confirmPassword: string;
}
interface ApiResult<T = null> {
  isSuccess: boolean;
  errorCode: string | null;
  message: string | null;
  statusCode: number;
  data: T;
} 
type RegisterErrors = Partial<Record<keyof RegisterFormData, string>> & { form?: string };

function validateRegister(data: RegisterFormData): RegisterErrors {
  const errors: RegisterErrors = {};

  if (!data.fullName.trim()) errors.fullName = 'Vui lòng nhập họ tên';

  if (!data.email.trim()) {
    errors.email = 'Vui lòng nhập email';
  } else if (!/^\S+@\S+\.\S+$/.test(data.email)) {
    errors.email = 'Email không hợp lệ';
  }

  if (!data.phone.trim()) {
    errors.phone = 'Vui lòng nhập số điện thoại';
  } else if (!/^(0|\+84)\d{9,10}$/.test(data.phone.replace(/\s/g, ''))) {
    errors.phone = 'Số điện thoại không hợp lệ';
  }

  if (!data.password) {
    errors.password = 'Vui lòng nhập mật khẩu';
  } else if (data.password.length < 6) {
    errors.password = 'Mật khẩu tối thiểu 6 ký tự';
  }

  if (data.confirmPassword !== data.password) {
    errors.confirmPassword = 'Mật khẩu nhập lại không khớp';
  }

  return errors;
}

const initialFormData: RegisterFormData = {
  fullName: '',
  email: '',
  phone: '',
  password: '',
  confirmPassword: '',
};

export function RegisterPage() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<RegisterFormData>(initialFormData);
  const [errors, setErrors] = useState<RegisterErrors>({});
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const handleChange = useCallback(
    (field: keyof RegisterFormData) => (e: React.ChangeEvent<HTMLInputElement>) => {
      setFormData((prev) => ({ ...prev, [field]: e.target.value }));
      setErrors((prev) => ({ ...prev, [field]: undefined, form: undefined }));
    },
    []
  );

  const handleSubmit = useCallback(
    async (e: React.FormEvent) => {
      e.preventDefault();
      const validationErrors = validateRegister(formData);
      if (Object.keys(validationErrors).length > 0) {
        setErrors(validationErrors);
        return;
      }

      setIsLoading(true);
      try {
        await authService.register(formData);
        navigate('/login');
      } catch (err) {
        const message = axios.isAxiosError(err)
        ? (err.response?.data as ApiResult | undefined)?.message
        : undefined;
        setErrors({ form: message ?? 'Đăng ký thất bại, vui lòng thử lại' });
      } finally {
        setIsLoading(false);
      }
    },
    [formData, navigate]
  );

  return (
    <AuthLayout title="Tạo tài khoản" subtitle="Đăng ký để bắt đầu đặt món yêu thích ngay hôm nay.">
      <form onSubmit={handleSubmit} className="space-y-4" noValidate>
        {errors.form && (
          <div className="rounded-lg bg-red-50 px-3 py-2 text-sm text-red-600">{errors.form}</div>
        )}

        <Input
          label="Họ và tên"
          placeholder="Nguyễn Văn A"
          value={formData.fullName}
          onChange={handleChange('fullName')}
          error={errors.fullName}
          leftIcon={<User size={16} />}
          fullWidth
          autoComplete="name"
        />

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
        />

        <Input
          label="Số điện thoại"
          placeholder="0912 345 678"
          value={formData.phone}
          onChange={handleChange('phone')}
          error={errors.phone}
          leftIcon={<Phone size={16} />}
          fullWidth
          autoComplete="tel"
        />

        <Input
          label="Mật khẩu"
          type={showPassword ? 'text' : 'password'}
          placeholder="Tối thiểu 6 ký tự"
          value={formData.password}
          onChange={handleChange('password')}
          error={errors.password}
          leftIcon={<Lock size={16} />}
          rightIcon={showPassword ? <EyeOff size={16} /> : <Eye size={16} />}
          onRightIconClick={() => setShowPassword((prev) => !prev)}
          fullWidth
          autoComplete="new-password"
        />

        <Input
          label="Nhập lại mật khẩu"
          type={showPassword ? 'text' : 'password'}
          placeholder="Nhập lại mật khẩu"
          value={formData.confirmPassword}
          onChange={handleChange('confirmPassword')}
          error={errors.confirmPassword}
          leftIcon={<Lock size={16} />}
          fullWidth
          autoComplete="new-password"
        />

        <Button type="submit" variant="primary" size="lg" fullWidth isLoading={isLoading}>
          Đăng ký
        </Button>

        <p className="text-center text-sm text-slate-500">
          Đã có tài khoản?{' '}
          <Link to="/login" className="font-semibold text-orange-500 hover:text-orange-600">
            Đăng nhập
          </Link>
        </p>
      </form>
    </AuthLayout>
  );
}