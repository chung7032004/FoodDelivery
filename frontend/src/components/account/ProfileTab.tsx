import { useEffect, useState } from "react";
import { Camera, Pencil, X } from "lucide-react";
import { toast } from "sonner";
import Button from "@/components/ui/Button";
import Input from "@/components/ui/Input";
import { getImageUrl } from "@/utils/image";
import { accountService, type AccountResponse } from "@/services/account.service";

export function ProfileTab({ user, onUpdated }: { user: AccountResponse; onUpdated: (user: AccountResponse) => void }) {
  const [isEditing, setIsEditing] = useState(false);
  const [fullName, setFullName] = useState(user.fullName ?? "");
  const [phone, setPhone] = useState(user.phone ?? "");
  const [avatarFile, setAvatarFile] = useState<File | null>(null);
  const [avatarPreview, setAvatarPreview] = useState<string | null>(null);
  const [error, setError] = useState("");
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    setFullName(user.fullName ?? "");
    setPhone(user.phone ?? "");
  }, [user.fullName, user.phone]);

  useEffect(() => {
    if (!avatarFile) {
      setAvatarPreview(null);
      return;
    }
    const previewUrl = URL.createObjectURL(avatarFile);
    setAvatarPreview(previewUrl);
    return () => URL.revokeObjectURL(previewUrl);
  }, [avatarFile]);

  function cancelEditing() {
    setFullName(user.fullName ?? "");
    setPhone(user.phone ?? "");
    setAvatarFile(null);
    setError("");
    setIsEditing(false);
  }

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setIsSaving(true);
    const formData = new FormData();
    formData.append("FullName", fullName.trim());
    formData.append("Phone", phone.trim());
    if (avatarFile) formData.append("Avatar", avatarFile);

    try {
      const result = await accountService.updateMe(formData);
      if (!result.isSuccess) throw new Error(result.message ?? "Không thể cập nhật hồ sơ.");
      const refreshed = await accountService.getMe();
      if (!refreshed.isSuccess || !refreshed.data) throw new Error(refreshed.message ?? "Không thể tải hồ sơ vừa cập nhật.");
      onUpdated(refreshed.data);
      setAvatarFile(null);
      setIsEditing(false);
      toast.success("Đã cập nhật hồ sơ.");
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : "Không thể cập nhật hồ sơ.");
    } finally {
      setIsSaving(false);
    }
  }

  const avatarUrl = avatarPreview ?? getImageUrl(user.avatarUrl);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h2 className="font-display text-lg font-bold text-black">Thông tin cá nhân</h2>
        {!isEditing && (
          <button type="button" onClick={() => setIsEditing(true)} className="flex items-center gap-1.5 text-sm font-medium text-brand hover:underline">
            <Pencil size={14} /> Chỉnh sửa
          </button>
        )}
      </div>

      {isEditing ? (
        <form onSubmit={handleSubmit} className="space-y-5">
          <div className="flex items-center gap-4">
            <img src={avatarUrl} alt="Ảnh đại diện" className="h-16 w-16 rounded-full border border-slate-200 object-cover" />
            <label className="inline-flex cursor-pointer items-center gap-2 rounded-lg border border-slate-200 px-3 py-2 text-sm font-medium text-slate-700 transition hover:bg-slate-50">
              <Camera size={16} /> Chọn ảnh đại diện
              <input
                type="file"
                accept="image/*"
                className="sr-only"
                onChange={(event) => {
                  const file = event.target.files?.[0] ?? null;
                  if (!file) return;
                  if (!file.type.startsWith("image/")) {
                    setError("Vui lòng chọn tệp hình ảnh.");
                    return;
                  }
                  if (file.size > 5 * 1024 * 1024) {
                    setError("Ảnh đại diện không được vượt quá 5 MB.");
                    return;
                  }
                  setError("");
                  setAvatarFile(file);
                }}
              />
            </label>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <Input label="Họ và tên" required minLength={2} maxLength={100} value={fullName} onChange={(event) => setFullName(event.target.value)} autoComplete="name" fullWidth />
            <Input label="Số điện thoại" required type="tel" maxLength={15} value={phone} onChange={(event) => setPhone(event.target.value)} autoComplete="tel" fullWidth />
            <Input label="Email" type="email" value={user.email ?? ""} disabled helperText="Email hiện chưa thể thay đổi." fullWidth />
          </div>
          {error && <p role="alert" className="text-sm text-red-600">{error}</p>}
          <div className="flex flex-wrap gap-2">
            <Button type="submit" variant="primary" size="md" isLoading={isSaving}>Lưu thay đổi</Button>
            <button type="button" onClick={cancelEditing} disabled={isSaving} className="inline-flex items-center gap-1.5 rounded-lg border border-slate-200 px-4 py-2 text-sm font-medium text-slate-600 transition hover:bg-slate-50 disabled:opacity-50">
              <X size={15} /> Hủy
            </button>
          </div>
        </form>
      ) : (
        <>
          <div className="grid gap-4 sm:grid-cols-2">
            <Field label="Họ và tên" value={user.fullName ?? "Chưa cập nhật"} />
            <Field label="Email" value={user.email ?? "Chưa cập nhật"} />
            <Field label="Số điện thoại" value={user.phone ?? "Chưa cập nhật"} />
            <Field label="Vai trò" value={user.roles.length > 0 ? user.roles.join(", ") : "Khách hàng"} />
          </div>
          <Button type="button" variant="primary" size="sm" onClick={() => setIsEditing(true)}>Cập nhật thông tin</Button>
        </>
      )}
    </div>
  );
}

function Field({ label, value }: { label: string; value: string }) {
  return (
    <div className="space-y-1.5">
      <p className="text-xs font-medium text-muted">{label}</p>
      <div className="rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm font-medium text-slate-800">
        {value}
      </div>
    </div>
  );
}