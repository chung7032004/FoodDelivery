import { FormEvent, useEffect, useState } from 'react';
import { Pencil, Plus, Trash2, ImageIcon, ChartBarStacked } from 'lucide-react';
import { AdminLayout } from './AdminLayout';
import { AdminModal } from '@/components/admin/AdminModal';
import Input, { type InputProps } from '@/components/ui/Input';
import { categoryService, CategoryResponse } from '@/services/category.service';
import { getImageUrl } from '@/utils/image';

type CategoryForm = {
  name: string;
  description: string;
  displayOrder: number;
  imageFile: File | null;
  imagePreview: string | null;
};

const emptyForm: CategoryForm = {
  name: '',
  description: '',
  displayOrder: 0,
  imageFile: null,
  imagePreview: null,
};

export default function AdminCategoriesPage() {
  const [items, setItems] = useState<CategoryResponse[]>([]);
  const [form, setForm] = useState<CategoryForm>(emptyForm);
  const [editing, setEditing] = useState<CategoryResponse | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  async function load() {
    setLoading(true);
    setError('');
    try {
      const result = await categoryService.getAll({ page: 1, pageSize: 100 });
      setItems(result.data?.data || []);
    } catch {
      setError('Không thể tải danh sách danh mục.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void load();
  }, []);

  function openForm(category?: CategoryResponse) {
    setEditing(category || null);
    setForm(
      category
        ? {
            name: category.name,
            description: category.description || '',
            displayOrder: category.displayOrder,
            imageFile: null,
            imagePreview: category.imageUrl ? getImageUrl(category.imageUrl) : null,
          }
        : emptyForm
    );
    setShowForm(true);
  }

  async function submit(event: FormEvent) {
    event.preventDefault();
    setSubmitting(true);
    setError('');
    try {
      const formData = new FormData();
      formData.append('name', form.name.trim());
      formData.append('description', form.description.trim());
      formData.append('displayOrder', String(form.displayOrder));

      if (form.imageFile) {
        formData.append('imageFile', form.imageFile);
      }

      if (editing) {
        await categoryService.update(editing.id, formData);
      } else {
        await categoryService.create(formData);
      }
      setShowForm(false);
      await load();
    } catch {
      setError('Không thể lưu danh mục.');
    } finally {
      setSubmitting(false);
    }
  }

  async function remove(id: string) {
    if (!window.confirm('Xóa danh mục này?')) return;
    try {
      await categoryService.remove(id);
      await load();
    } catch {
      setError('Không thể xóa danh mục.');
    }
  }

  return (
    <AdminLayout>
      {/* Header */}
      <div className="mb-6 flex items-end justify-between gap-4">
        <div>
          <h2 className="font-display text-3xl font-bold">Danh mục</h2>
          <p className="mt-1 text-sm text-muted">Sắp xếp và quản lý nhóm món ăn.</p>
        </div>
        <button
          onClick={() => openForm()}
          className="flex items-center gap-2 rounded-xl bg-orange-500 px-4 py-2.5 text-sm font-bold text-white shadow-sm transition hover:-translate-y-0.5 hover:bg-orange-600"
        >
          <Plus size={17} />
          Thêm danh mục
        </button>
      </div>

      {/* Thông báo lỗi */}
      {error && (
        <p className="mb-4 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">
          {error}
        </p>
      )}

      {/* Bảng danh sách */}
      <div className="overflow-x-auto rounded-2xl border border-border bg-surface shadow-sm">
        <table className="w-full min-w-[650px] text-left text-sm">
          <thead>
            <tr className="border-b border-border text-xs uppercase tracking-wider text-muted">
              <th className="px-5 py-4">Ảnh</th>
              <th className="px-5 py-4">Tên danh mục</th>
              <th className="px-5 py-4">Mô tả</th>
              <th className="px-5 py-4">Thứ tự</th>
              <th className="px-5 py-4" />
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={4} className="px-5 py-10 text-center text-muted">
                  Đang tải dữ liệu...
                </td>
              </tr>
            ) : items.length === 0 ? (
              <tr>
                <td colSpan={4} className="px-5 py-10 text-center text-muted">
                  Chưa có danh mục nào.
                </td>
              </tr>
            ) : (
              items.map((category) => (
                <tr
                  key={category.id}
                  className="border-b border-border transition-colors last:border-0 hover:bg-orange-500/[.02]"
                >
                  <td className="px-5 py-4">
                    <div className="flex h-11 w-11 items-center justify-center overflow-hidden rounded-xl border border-slate-200 bg-slate-50 text-slate-400">
                      {category.imageUrl ? (
                        <img
                          src={getImageUrl(category.imageUrl)}
                          alt={category.name}
                          className="h-full w-full object-cover"
                        />
                      ) : (
                        <ChartBarStacked size={18} />
                      )}
                    </div>
                  </td>
                  <td className="px-5 py-4 font-semibold">{category.name}</td>
                  <td className="px-5 py-4 text-muted">
                    {category.description || 'Chưa có mô tả'}
                  </td>
                  <td className="px-5 py-4">{category.displayOrder}</td>
                  <td className="px-5 py-4 text-right">
                    <button
                      onClick={() => openForm(category)}
                      title="Sửa"
                      className="mr-1 rounded-lg p-2 text-muted transition hover:bg-black/5 hover:text-orange-500"
                    >
                      <Pencil size={17} />
                    </button>
                    <button
                      onClick={() => void remove(category.id)}
                      title="Xóa"
                      className="rounded-lg p-2 text-muted transition hover:bg-red-50 hover:text-red-500"
                    >
                      <Trash2 size={17} />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Modal Thêm / Sửa */}
      <AdminModal
        open={showForm}
        onClose={() => setShowForm(false)}
        title={editing ? 'Sửa danh mục' : 'Thêm danh mục'}
        description="Thông tin hiển thị của nhóm món ăn."
      >
        <form onSubmit={submit}>
          <Field
            label="Tên danh mục"
            value={form.name}
            onChange={(name) => setForm({ ...form, name })}
            required
          />
          <Field
            label="Mô tả"
            value={form.description}
            onChange={(description) => setForm({ ...form, description })}
          />
          <Field
            label="Thứ tự hiển thị"
            type="number"
            value={String(form.displayOrder)}
            onChange={(displayOrder) =>
              setForm({ ...form, displayOrder: Number(displayOrder) })
            }
          />

          <div className="mt-5">
            <label className="mb-2 block text-sm font-medium text-slate-700">
              Ảnh danh mục
            </label>

            <div className="flex items-center gap-4 rounded-xl border border-dashed border-slate-300 bg-slate-50 p-3">
              <div className="flex h-20 w-20 items-center justify-center overflow-hidden rounded-xl border border-slate-200 bg-white text-slate-400">
                {form.imagePreview ? (
                  <img src={form.imagePreview} alt="Preview" className="h-full w-full object-cover" />
                ) : (
                  <ImageIcon size={24} />
                )}
              </div>

              <input
                type="file"
                accept="image/*"
                onChange={(e) => {
                  const file = e.target.files?.[0] || null;
                  setForm((prev) => ({
                    ...prev,
                    imageFile: file,
                    imagePreview: file ? URL.createObjectURL(file) : prev.imagePreview,
                  }));
                }}
                className="block w-full text-sm text-slate-600 file:mr-4 file:rounded-lg file:border-0 file:bg-orange-500 file:px-4 file:py-2 file:text-sm file:font-semibold file:text-white hover:file:bg-orange-600"
              />
            </div>
          </div>

          <div className="mt-6 flex justify-end gap-2">
            <button
              type="button"
              onClick={() => setShowForm(false)}
              className="rounded-lg border border-slate-200 px-4 py-2 text-sm font-semibold text-slate-600 transition hover:bg-slate-50"
            >
              Hủy
            </button>
            <button
              type="submit"
              disabled={submitting}
              className="rounded-lg bg-orange-500 px-4 py-2 text-sm font-bold text-white transition hover:bg-orange-600 disabled:opacity-50"
            >
              {submitting ? 'Đang lưu...' : 'Lưu'}
            </button>
          </div>
        </form>
      </AdminModal>
    </AdminLayout>
  );
}

function Field({
  label,
  value,
  onChange,
  type = 'text',
  required = false,
}: {
  label: string;
  value: string;
  onChange: (value: string) => void;
  type?: InputProps['type'];
  required?: boolean;
}) {
  return (
    <Input
        label={label}
        required={required}
        type={type}
        value={value}
        onChange={(event) => onChange(event.target.value)}
        fullWidth
        containerClassName="mb-4"
      />
  );
}