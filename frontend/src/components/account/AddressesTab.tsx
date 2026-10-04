import { useEffect, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { AddressMapPicker, AddressOverviewMap } from "@/components/account/AddressMapPicker";
import Input from "@/components/ui/Input";
import { MapPin, Pencil, Plus, Star, Trash2, X } from "lucide-react";
import { toast } from "sonner";
import { addressService, type AddressRequest, type CustomerAddress } from "@/services/address.service";

interface AddressFormState {
  fullAddress: string;
  label: string;
  receiverName: string;
  phoneNumber: string;
  latitude: string;
  longitude: string;
  isDefault: boolean;
}

interface Coordinates {
  lat: number;
  lng: number;
}

const EMPTY_FORM: AddressFormState = {
  fullAddress: "",
  label: "Nhà riêng",
  receiverName: "",
  phoneNumber: "",
  latitude: "",
  longitude: "",
  isDefault: false,
};

export function AddressesTab() {
  const queryClient = useQueryClient();
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);
  const [form, setForm] = useState<AddressFormState>(EMPTY_FORM);
  const [formError, setFormError] = useState("");
  const addressesQuery = useQuery({
    queryKey: ["my-addresses"],
    queryFn: addressService.getMine,
  });
  const editAddressQuery = useQuery({
    queryKey: ["address", editingId],
    queryFn: () => addressService.getById(editingId!),
    enabled: !!editingId,
  });
  const refreshAddresses = () => queryClient.invalidateQueries({ queryKey: ["my-addresses"] });
  const createMutation = useMutation({
    mutationFn: (request: AddressRequest) => addressService.create(request),
    onSuccess: async () => {
      await refreshAddresses();
      closeForm();
      toast.success("Đã thêm địa chỉ.");
    },
  });
  const updateMutation = useMutation({
    mutationFn: ({ id, request }: { id: string; request: AddressRequest }) => addressService.update(id, request),
    onSuccess: async () => {
      await refreshAddresses();
      closeForm();
      toast.success("Đã cập nhật địa chỉ.");
    },
  });
  const deleteMutation = useMutation({
    mutationFn: addressService.remove,
    onSuccess: async () => {
      await refreshAddresses();
      toast.success("Đã xóa địa chỉ.");
    },
  });
  const defaultMutation = useMutation({
    mutationFn: addressService.setDefault,
    onSuccess: async () => {
      await refreshAddresses();
      toast.success("Đã cập nhật địa chỉ mặc định.");
    },
  });

  const addresses = addressesQuery.data?.data ?? [];
  const isSaving = createMutation.isPending || updateMutation.isPending;

  useEffect(() => {
    const address = editAddressQuery.data?.data;
    if (!address) return;
    setForm(toFormState(address));
  }, [editAddressQuery.data]);

  function closeForm() {
    setIsFormOpen(false);
    setEditingId(null);
    setForm(EMPTY_FORM);
    setFormError("");
  }

  function startCreate() {
    setEditingId(null);
    setForm(EMPTY_FORM);
    setFormError("");
    setIsFormOpen(true);
  }

  function startEdit(addressId: string) {
    setForm(EMPTY_FORM);
    setFormError("");
    setEditingId(addressId);
    setIsFormOpen(true);
  }

  function setCoordinates(coordinates: Coordinates) {
    setForm((current) => ({
      ...current,
      latitude: String(coordinates.lat),
      longitude: String(coordinates.lng),
    }));
  }

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setFormError("");
    const latitude = Number(form.latitude);
    const longitude = Number(form.longitude);
    if (!Number.isFinite(latitude) || latitude < -90 || latitude > 90) {
      setFormError("Hãy chọn vị trí giao hàng trên bản đồ.");
      return;
    }
    if (!Number.isFinite(longitude) || longitude < -180 || longitude > 180) {
      setFormError("Hãy chọn vị trí giao hàng trên bản đồ.");
      return;
    }

    const request: AddressRequest = {
      fullAddress: form.fullAddress.trim(),
      label: form.label.trim(),
      receiverName: form.receiverName.trim(),
      phoneNumber: form.phoneNumber.trim(),
      latitude,
      longitude,
      isDefault: form.isDefault,
    };
    if (editingId) updateMutation.mutate({ id: editingId, request });
    else createMutation.mutate(request);
  }

  function handleDelete(address: CustomerAddress) {
    if (window.confirm(`Xóa địa chỉ "${address.label || address.fullAddress}"?`)) {
      deleteMutation.mutate(address.id);
    }
  }

  return (
    <section className="space-y-6">
      <div className="flex flex-wrap items-center justify-between gap-3">
        <div>
          <h2 className="font-display text-lg font-bold text-black">Địa chỉ giao hàng</h2>
          <p className="mt-1 text-sm text-muted">Quản lý địa chỉ nhận hàng và vị trí giao.</p>
        </div>
        {!isFormOpen && (
          <button onClick={startCreate} className="inline-flex items-center gap-2 rounded-lg bg-brand px-4 py-2 text-sm font-semibold text-black hover:bg-brand-dark">
            <Plus size={16} /> Thêm địa chỉ
          </button>
        )}
      </div>

      {!isFormOpen && <AddressOverviewMap addresses={addresses} />}

      {isFormOpen && (
        <form onSubmit={handleSubmit} className="space-y-4 border-y border-border py-5">
          <div className="flex items-center justify-between gap-3">
            <h3 className="font-semibold text-black">{editingId ? "Sửa địa chỉ" : "Địa chỉ mới"}</h3>
            <button type="button" onClick={closeForm} aria-label="Đóng biểu mẫu" className="p-2 text-muted hover:text-black"><X size={17} /></button>
          </div>

          {editingId && editAddressQuery.isLoading ? (
            <p className="text-sm text-muted">Đang tải địa chỉ...</p>
          ) : (
            <>
              <div className="grid gap-4 sm:grid-cols-2">
                <Input label="Tên gợi nhớ" required maxLength={50} value={form.label} onChange={(event) => setForm({ ...form, label: event.target.value })} placeholder="Nhà riêng, Công ty..." fullWidth />
                <Input label="Người nhận" required maxLength={100} value={form.receiverName} onChange={(event) => setForm({ ...form, receiverName: event.target.value })} fullWidth />
                <Input label="Số điện thoại" required type="tel" maxLength={20} value={form.phoneNumber} onChange={(event) => setForm({ ...form, phoneNumber: event.target.value })} fullWidth />
              </div>

              <div className="space-y-2">
                <label className="block text-xs font-medium text-muted">Tìm địa chỉ giao hàng</label>
                <>
                  <AddressMapPicker
                    address={form.fullAddress}
                    position={toCoordinates(form)}
                    onAddressInput={(fullAddress) => setForm((current) => ({ ...current, fullAddress, latitude: "", longitude: "" }))}
                    onAddressSelected={(fullAddress, coordinates) => setForm((current) => ({
                      ...current,
                      fullAddress,
                      latitude: String(coordinates.lat),
                      longitude: String(coordinates.lng),
                    }))}
                    onPositionSelected={setCoordinates}
                  />
                </>
              </div>

              <label className="inline-flex items-center gap-2 text-sm text-black">
                <input type="checkbox" checked={form.isDefault} onChange={(event) => setForm({ ...form, isDefault: event.target.checked })} className="accent-brand" />
                Đặt làm địa chỉ mặc định
              </label>
              {formError && <p role="alert" className="text-sm text-red-500">{formError}</p>}
              {editAddressQuery.isError && <p role="alert" className="text-sm text-red-500">Không tải được địa chỉ cần sửa.</p>}
              <div className="flex gap-2">
                <button type="submit" disabled={isSaving || (Boolean(editingId) && editAddressQuery.isLoading)} className="rounded-lg bg-brand px-4 py-2 text-sm font-semibold text-black hover:bg-brand-dark disabled:opacity-50">
                  {isSaving ? "Đang lưu..." : "Lưu địa chỉ"}
                </button>
                <button type="button" onClick={closeForm} className="rounded-lg border border-border px-4 py-2 text-sm font-medium text-muted hover:text-black">Hủy</button>
              </div>
            </>
          )}
        </form>
      )}

      {addressesQuery.isLoading ? (
        <p className="py-8 text-center text-sm text-muted">Đang tải địa chỉ...</p>
      ) : addressesQuery.isError ? (
        <div className="flex items-center justify-between gap-3 border-y border-red-400/20 py-4 text-sm text-red-400">
          <span>Không tải được địa chỉ.</span>
          <button onClick={() => void addressesQuery.refetch()} className="underline">Thử lại</button>
        </div>
      ) : addresses.length === 0 ? (
        <div className="border-y border-border py-12 text-center">
          <MapPin size={30} className="mx-auto text-muted" />
          <p className="mt-3 text-sm text-muted">Bạn chưa lưu địa chỉ giao hàng nào.</p>
        </div>
      ) : (
        <div className="divide-y divide-border border-y border-border">
          {addresses.map((address) => (
            <article key={address.id} className="flex flex-wrap items-start justify-between gap-4 py-5">
              <div className="flex min-w-0 items-start gap-3">
                <MapPin size={18} className="mt-0.5 shrink-0 text-brand" />
                <div className="min-w-0">
                  <div className="flex flex-wrap items-center gap-2">
                    <h3 className="font-semibold text-black">{address.label || "Địa chỉ"}</h3>
                    {address.isDefault && <span className="text-xs font-semibold text-brand">Mặc định</span>}
                  </div>
                  <p className="mt-1 text-sm text-muted">{address.fullAddress}</p>
                  <p className="mt-1 text-sm text-muted">{address.receiverName} · {address.phoneNumber}</p>
                </div>
              </div>
              <div className="flex items-center gap-1">
                {!address.isDefault && (
                  <button title="Đặt làm mặc định" aria-label={`Đặt ${address.label} làm mặc định`} disabled={defaultMutation.isPending} onClick={() => defaultMutation.mutate(address.id)} className="p-2 text-muted hover:text-brand disabled:opacity-50"><Star size={16} /></button>
                )}
                <button title="Sửa địa chỉ" aria-label={`Sửa ${address.label}`} onClick={() => startEdit(address.id)} className="p-2 text-muted hover:text-brand"><Pencil size={16} /></button>
                <button title="Xóa địa chỉ" aria-label={`Xóa ${address.label}`} disabled={deleteMutation.isPending} onClick={() => handleDelete(address)} className="p-2 text-muted hover:text-red-500 disabled:opacity-50"><Trash2 size={16} /></button>
              </div>
            </article>
          ))}
        </div>
      )}
    </section>
  );
}

function toFormState(address: CustomerAddress): AddressFormState {
  return {
    fullAddress: address.fullAddress ?? "",
    label: address.label ?? "",
    receiverName: address.receiverName ?? "",
    phoneNumber: address.phoneNumber ?? "",
    latitude: String(address.latitude),
    longitude: String(address.longitude),
    isDefault: address.isDefault,
  };
}

function toCoordinates(form: AddressFormState): Coordinates | null {
  const lat = Number(form.latitude);
  const lng = Number(form.longitude);
  if (!form.latitude || !form.longitude || !Number.isFinite(lat) || !Number.isFinite(lng)) return null;
  return { lat, lng };
}

