import { useState } from "react";
import { authService } from "@/services/auth.service";
import Input from "@/components/ui/Input";

export function SecurityTab() {
  const [oldPassword, setOldPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmNewPassword, setConfirmNewPassword] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setError("");
    setSuccess(false);

    if (!oldPassword || !newPassword || !confirmNewPassword) {
      setError("Vui lòng điền đầy đủ các trường");
      return;
    }
    if (newPassword.length < 8) {
      setError("Mật khẩu mới phải có ít nhất 8 ký tự");
      return;
    }
    if (newPassword !== confirmNewPassword) {
      setError("Mật khẩu mới và xác nhận không khớp");
      return;
    }
    if (oldPassword === newPassword) {
      setError("Mật khẩu mới phải khác mật khẩu hiện tại");
      return;
    }

    setIsLoading(true);
    try {
      await authService.changePassword({ oldPassword, newPassword, confirmNewPassword });
      setSuccess(true);
      setOldPassword("");
      setNewPassword("");
      setConfirmNewPassword("");
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : "Có lỗi xảy ra, vui lòng thử lại");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      <h2 className="font-display text-lg font-bold text-black">Đổi mật khẩu</h2>
      <div className="max-w-sm space-y-4">
        <PasswordInput label="Mật khẩu hiện tại" value={oldPassword} onChange={setOldPassword} disabled={isLoading} />
        <PasswordInput label="Mật khẩu mới" value={newPassword} onChange={setNewPassword} disabled={isLoading} />
        <PasswordInput label="Nhập lại mật khẩu mới" value={confirmNewPassword} onChange={setConfirmNewPassword} disabled={isLoading} />
        {error && <div className="rounded-lg bg-red-50/10 px-3 py-2 text-sm text-red-500">{error}</div>}
        {success && <div className="rounded-lg bg-green-50/10 px-3 py-2 text-sm text-green-600">Cập nhật mật khẩu thành công!</div>}
      </div>
      <button
        type="submit"
        disabled={isLoading}
        className="inline-flex items-center justify-center rounded-xl bg-brand px-4 py-2 text-sm font-medium text-orange-500 hover:bg-brand-dark disabled:cursor-not-allowed disabled:opacity-50"
      >
        {isLoading ? "Đang xử lý..." : "Cập nhật mật khẩu"}
      </button>
    </form>
  );
}

function PasswordInput({
  label,
  value,
  onChange,
  disabled,
}: {
  label: string;
  value: string;
  onChange: (value: string) => void;
  disabled: boolean;
}) {
  return (
    <Input
        label={label}
        type="password"
        value={value}
        onChange={(event) => onChange(event.target.value)}
        disabled={disabled}
        required
        minLength={8}
        placeholder="••••••••"
        fullWidth
      />
  );
}