import { AuthLayout } from "@/components/layout/AuthLayout"
import Button from "@/components/ui/Button"
import Input from "@/components/ui/Input"
import { authService } from "@/services/auth.service"
import { useMutation } from "@tanstack/react-query"
import { Eye, EyeOff, Lock, Mail, ShieldCheck } from "lucide-react"
import React, { useCallback, useState } from "react"
import { Link, useNavigate, useLocation } from "react-router-dom";

interface ResetPasswordFormData {
    email? : string
    otp? : string 
    password? : string
    confirmPassword? : string  
}
interface ResetPasswordErrors {
    email? : string
    otp? : string 
    newPassword? : string
    confirmNewPassword? : string
    form? : string
}
function validateResetPassword(data: ResetPasswordFormData): ResetPasswordErrors {
    const errors: ResetPasswordErrors = {};
    if (!data.email?.trim()) {
        errors.email = 'Vui lòng nhập email';
    } else if (!/^\S+@\S+\.\S+$/.test(data.email)) {
        errors.email = 'Email không hợp lệ';
    }
    if (!data.otp?.trim()) {
        errors.otp = 'Vui lòng nhập mã OTP';
    }
    if (!data.password?.trim()) {
        errors.newPassword = 'Vui lòng nhập mật khẩu mới';
    } else if (data.password.length < 6) {
        errors.newPassword = 'Mật khẩu mới tối thiểu 6 ký tự';
    }
    if (data.password !== data.confirmPassword) {
        errors.confirmNewPassword = 'Mật khẩu xác nhận không khớp';
    }
    return errors;
}
export const ResetPasswordPage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const initialEmail = location.state?.email || '';
    const [formData, setFormData] = useState<ResetPasswordFormData>({ email: initialEmail, otp: '', password: '', confirmPassword: '' });
    const [errors, setErrors] = useState<ResetPasswordErrors>({});
    const [showPassword, setShowPassword] = useState(false)

    const handleChange = useCallback(
        (field: keyof ResetPasswordFormData) => (e: React.ChangeEvent<HTMLInputElement>) => {
            setFormData((prev)=> ({ ...prev, [field]: e.target.value }))
            setErrors((prev) => ({ ...prev, [field]: undefined, form: undefined }))
        },[]
    );
    const resetPasswordMutation = useMutation({
        mutationFn: (variables: { email: string; otp: string; password: string }) => 
            authService.resetPassword(variables.email, variables.otp, variables.password),
        onSuccess: () => {
            navigate('/login', { state: { email: formData.email } });
        },
    })
    const handleSubmit = useCallback((e: React.FormEvent) => 
        {
            e.preventDefault(); 
            const validationErrors = validateResetPassword(formData);
            if (Object.keys(validationErrors).length > 0) {
                setErrors(validationErrors);
                return;
            }
           setErrors({});

           resetPasswordMutation.mutate({
                email: formData.email || '',
                otp: formData.otp || '',
                password: formData.password || ''
           });

        },[formData,navigate]
    )
    return (
        <AuthLayout title="Đặt lại mật khẩu" subtitle="Nhập email, mã OTP và mật khẩu mới của bạn.">
            <form className="space-y-4" noValidate onSubmit={handleSubmit}>
                {errors.form && (
                    <div 
                        role= "alert"
                        className="rounded-xl border border-red-100 bg-red-50 px-3.5 py-2.5 text-sm text-red-600"
                        >
                        {errors.form}
                    </div>
                )}
                <Input 
                    label="Email"
                    type="email"
                    placeholder="Nhập email của bạn"
                    value={formData.email}
                    onChange={handleChange('email')}
                    error={errors.email}
                    leftIcon={<Mail size={16} />}
                    fullWidth
                />
                <Input 
                    label="Mã OTP"
                    type="text"
                    placeholder="Nhập mã OTP"
                    value={formData.otp}
                    onChange={handleChange('otp')}
                    error={errors.otp}
                    fullWidth
                    leftIcon={<ShieldCheck size={16} />}
                />
                <Input 
                    label="Mật khẩu mới"
                    type={showPassword ? "text" : "password"}
                    placeholder="Nhập mật khẩu mới"
                    value={formData.password}
                    onChange={handleChange('password')}
                    error={errors.newPassword}
                    leftIcon={<Lock size={16} />}
                    rightIcon={showPassword ? <EyeOff size={16} /> : <Eye size={16} />}
                    onRightIconClick={() => setShowPassword((prev) => !prev)}
                />
                <Input 
                    label="Xác nhận mật khẩu mới"
                    type={showPassword ? "text" : "password"}
                    placeholder="Xác nhận mật khẩu mới"
                    value={formData.confirmPassword}
                    onChange={handleChange('confirmPassword')}
                    error={errors.confirmNewPassword}
                    leftIcon={<Lock size={16} />}
                    rightIcon={showPassword ? <EyeOff size={16} /> : <Eye size={16} />}
                    onRightIconClick={() => setShowPassword((prev) => !prev)}
                />
                <Button type="submit" variant="primary" size="lg" fullWidth>
                    Đặt lại mật khẩu
                </Button>
                <Link to="/login" className="font-semibold text-orange-500 hover:text-orange-600">
                    Quay lại đăng nhập
                </Link>
            </form>
        </AuthLayout>
    )
}