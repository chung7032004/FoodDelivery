import { AuthLayout } from "@/components/layout/AuthLayout";
import Button from "@/components/ui/Button";
import Input from "@/components/ui/Input";
import { authService } from "@/services/auth.service";
import { Mail } from "lucide-react";
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useMutation } from "@tanstack/react-query";

function validateEmail(email: string): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

export function SendOtpPage() {
  const [email, setEmail] = useState('');
  const [fieldError, setFieldError] = useState(''); // chỉ dùng cho lỗi validate client, không dùng cho lỗi server
  const navigate = useNavigate();

  const sendOtpMutation = useMutation({
    mutationFn: authService.sendOtp,
    onSuccess: () => {
      navigate('/reset-password', { state: { email } });
    },
    // Không cần onError ở đây — lỗi server sẽ tự rơi vào
    // MutationCache.onError (global) đã cấu hình ở queryClient.ts, tự hiện toast.
  });

  const handleSubmit = (e: React.FormEvent) => {
      e.preventDefault();
      if (!validateEmail(email)) {
        setFieldError('Vui lòng nhập địa chỉ email hợp lệ.');
        return;
      }
      setFieldError('');
      sendOtpMutation.mutate(email);
    }

  return (
    <AuthLayout title="Đặt lại mật khẩu" subtitle="Nhập email tài khoản của bạn.">
      <form onSubmit={handleSubmit} className="space-y-4">
        <Input
          label="Email"
          type="email"
          placeholder="you@example.com"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          error={fieldError}
          fullWidth
          autoComplete="email"
          leftIcon={<Mail size={16} />}
        />
        <Button type="submit" variant="primary" size="lg" fullWidth isLoading={sendOtpMutation.isPending}>
          Gửi OTP
        </Button>
        <Link to="/login" className="text-sm font-medium text-orange-500 hover:text-orange-600">
          Quay lại đăng nhập
        </Link>
      </form>
    </AuthLayout>
  );
}