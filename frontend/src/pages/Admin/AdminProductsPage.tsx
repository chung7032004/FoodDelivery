import { FormEvent, useEffect, useState } from 'react';
import { Pencil, Plus, Search, Trash2, ChartBarStacked } from 'lucide-react';
import { RestaurantLayout } from '@/pages/Restaurant/RestaurantLayout';
import { AdminModal } from '@/components/admin/AdminModal';
import { categoryService, CategoryResponse } from '@/services/category.service';
import { productService } from '@/services/product.service';
import { ProductResponseDto } from '@/types/product';
import { getImageUrl } from '@/utils/image';
import { AdminLayout } from './AdminLayout';
import Input, { type InputProps } from '@/components/ui/Input';

type ProductForm = {
  restaurantId: string;
  categoryId: string;
  name: string;
  description: string;
  price: string;
  isAvailable: boolean;
  isFeatured: boolean;
  displayOrder: string;
  imageFiles: File[];
  previews: string[];
  mainImageIndex: number;
};

const emptyForm: ProductForm = {
  restaurantId: '',
  categoryId: '',
  name: '',
  description: '',
  price: '',
  isAvailable: true,
  isFeatured: false,
  displayOrder: '0',
  imageFiles: [],
  previews: [],
  mainImageIndex: 0,
};

const money = new Intl.NumberFormat('vi-VN', {
  style: 'currency',
  currency: 'VND',
  maximumFractionDigits: 0,
});

interface AdminProductsPageProps {
  workspace?: 'admin' | 'restaurant';
}

export default function AdminProductsPage({ workspace = 'admin' }: AdminProductsPageProps) {
  const [items, setItems] = useState<ProductResponseDto[]>([]);
  const [categories, setCategories] = useState<CategoryResponse[]>([]);
  const [form, setForm] = useState<ProductForm>(emptyForm);
  const [editing, setEditing] = useState<ProductResponseDto | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [search, setSearch] = useState('');
  const [categoryId, setCategoryId] = useState('');
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  // Tải danh sách sản phẩm và danh mục
  async function load() {
    setLoading(true);
    setError('');
    try {
      const [products, categoryResult] = await Promise.all([
        productService.getAll({
          searchQuery: search.trim() || undefined,
          categoryId: categoryId || undefined,
          page: 1,
          pageSize: 100,
        }),
        categoryService.getAll({ page: 1, pageSize: 100 }),
      ]);
      setItems(products.data?.data || []);
      setCategories(categoryResult.data?.data || []);
    } catch {
      setError('Không thể tải danh sách sản phẩm.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void load();
  }, [search, categoryId]);

  // Mở modal thêm mới hoặc sửa
  async function openForm(product?: ProductResponseDto) {
    if (product) {
      try {
        const detailRes = await productService.getDetailById(product.id);
        const detail = detailRes.data;
        const images = detail?.images ?? [];
        const previews = images.map((img) => getImageUrl(img.imageUrl));
        setEditing(product);
        setForm({
          restaurantId: product.restaurantId || '',
          categoryId: product.categoryId || '',
          name: product.name || '',
          description: detail?.description ?? product.description ?? '',
          price: String(product.price || 0),
          isAvailable: product.isAvailable ?? true,
          isFeatured: product.isFeatured ?? false,
          displayOrder: String(product.displayOrder || 0),
          imageFiles: [],
          previews,
          mainImageIndex: images.findIndex((img) => img.isMain) >= 0 ? images.findIndex((img) => img.isMain) : 0,
        });
        setShowForm(true);
        return;
      } catch {
        // fallback to basic product values below
      }
    }

    setEditing(product || null);
    setForm(
      product
        ? {
            restaurantId: product.restaurantId || '',
            categoryId: product.categoryId || '',
            name: product.name || '',
            description: product.description || '',
            price: String(product.price || 0),
            isAvailable: product.isAvailable ?? true,
            isFeatured: product.isFeatured ?? false,
            displayOrder: String(product.displayOrder || 0),
            imageFiles: [],
            previews: product.mainImageUrl ? [getImageUrl(product.mainImageUrl)] : [],
            mainImageIndex: 0,
          }
        : emptyForm
    );
    setShowForm(true);
  }

  function handleImageFilesChange(fileList: FileList | null) {
    if (!fileList || fileList.length === 0) {
      setForm((prev) => ({ ...prev, imageFiles: [], previews: [], mainImageIndex: 0 }));
      return;
    }

    const newFiles = Array.from(fileList);
    const newPreviews = newFiles.map((file) => URL.createObjectURL(file));

    setForm((prev) => {
      const mergedFiles = [...(prev.imageFiles ?? []), ...newFiles];
      const mergedPreviews = [...(prev.previews ?? []), ...newPreviews];

      return {
        ...prev,
        imageFiles: mergedFiles,
        previews: mergedPreviews,
        mainImageIndex:
          prev.mainImageIndex >= mergedFiles.length ? 0 : Math.min(prev.mainImageIndex, mergedFiles.length - 1),
      };
    });
  }

  // Submit Form
  async function submit(event: FormEvent) {
    event.preventDefault();
    setSubmitting(true);
    setError('');
    try {
      const formData = new FormData();
      formData.append('restaurantId', form.restaurantId);
      formData.append('categoryId', form.categoryId);
      formData.append('name', form.name.trim());
      formData.append('description', form.description.trim());
      formData.append('price', String(Number(form.price || 0)));
      formData.append('isAvailable', String(form.isAvailable));
      formData.append('isFeatured', String(form.isFeatured));
      formData.append('displayOrder', String(Number(form.displayOrder || 0)));

      if (form.imageFiles.length > 0) {
        form.imageFiles.forEach((file) => {
          formData.append('imageFiles', file);
        });
        formData.append('mainImageIndex', String(form.mainImageIndex));
      }

      if (editing) {
        await productService.update(editing.id, formData);
      } else {
        await productService.create(formData);
      }
      setShowForm(false);
      await load();
    } catch {
      setError('Không thể lưu thông tin sản phẩm.');
    } finally {
      setSubmitting(false);
    }
  }

  // Đổi trạng thái ẩn/hiện sản phẩm
  async function toggle(product: ProductResponseDto) {
    try {
      await productService.toggle(product.id);
      await load();
    } catch {
      setError('Không thể đổi trạng thái sản phẩm.');
    }
  }

  // Xóa sản phẩm
  async function remove(id: string) {
    if (!window.confirm('Xóa sản phẩm này?')) return;
    try {
      await productService.remove(id);
      await load();
    } catch {
      setError('Không thể xóa sản phẩm.');
    }
  }

  const Layout = workspace === 'restaurant' ? RestaurantLayout : AdminLayout;

  return (
    <Layout>
      {/* Header */}
      <div className="mb-6 flex items-end justify-between gap-4">
        <div>
          <h2 className="font-display text-3xl font-bold">Sản phẩm</h2>
          <p className="mt-1 text-sm text-muted">Quản lý món ăn, giá bán và trạng thái hiển thị.</p>
        </div>
        <button
          onClick={() => openForm()}
          className="flex items-center gap-2 rounded-xl bg-orange-500 px-4 py-2.5 text-sm font-bold text-white shadow-sm transition hover:-translate-y-0.5 hover:bg-orange-600"
        >
          <Plus size={17} />
          Thêm sản phẩm
        </button>
      </div>

      {/* Thanh bộ lọc và tìm kiếm */}
      <div className="mb-4 flex flex-wrap gap-3">
        <Input
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Tìm sản phẩm..."
            leftIcon={<Search size={16} />}
            size="sm"
            containerClassName="w-full sm:w-56"
            fullWidth
          />
        <select
          value={categoryId}
          onChange={(e) => setCategoryId(e.target.value)}
          className="rounded-xl border border-border bg-surface px-3.5 py-2 text-sm text-foreground shadow-sm outline-none focus:border-orange-500"
        >
          <option value="">Tất cả danh mục</option>
          {categories.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
      </div>

      {/* Thông báo lỗi */}
      {error && (
        <p className="mb-4 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">
          {error}
        </p>
      )}

      {/* Bảng dữ liệu sản phẩm */}
      <div className="overflow-x-auto rounded-2xl border border-border bg-surface shadow-sm">
        <table className="w-full min-w-[850px] text-left text-sm">
          <thead>
            <tr className="border-b border-border text-xs uppercase tracking-wider text-muted">
              <th className="px-5 py-4">Ảnh</th>
              <th className="px-5 py-4">Sản phẩm</th>
              <th className="px-5 py-4">Danh mục</th>
              <th className="px-5 py-4">Giá</th>
              <th className="px-5 py-4">Trạng thái</th>
              <th className="px-5 py-4 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={5} className="px-5 py-10 text-center text-muted">
                  Đang tải dữ liệu...
                </td>
              </tr>
            ) : items.length === 0 ? (
              <tr>
                <td colSpan={5} className="px-5 py-10 text-center text-muted">
                  Chưa có sản phẩm nào.
                </td>
              </tr>
            ) : (
              items.map((product) => (
                <tr
                  key={product.id}
                  className="border-b border-border transition-colors last:border-0 hover:bg-orange-500/[.02]"
                >
                  <td className="px-5 py-4">
                    <div className="flex h-11 w-11 items-center justify-center overflow-hidden rounded-xl border border-slate-200 bg-slate-50">
                      {product.mainImageUrl ? (
                        <img
                          src={getImageUrl(product.mainImageUrl)}
                          alt={product.name}
                          className="h-full w-full object-cover"
                        />
                      ) : (
                        <ChartBarStacked size={18} className="text-slate-400" />
                      )}
                    </div>
                  </td>
                  <td className="px-5 py-4">
                    <p className="font-semibold text-foreground">{product.name}</p>
                    <p className="mt-0.5 text-xs text-muted">
                      {product.isFeatured ? (
                        <span className="font-medium text-amber-600">★ Sản phẩm nổi bật</span>
                      ) : (
                        'Sản phẩm thường'
                      )}
                    </p>
                  </td>
                  <td className="px-5 py-4 text-muted">{product.categoryName || 'N/A'}</td>
                  <td className="px-5 py-4 font-semibold text-foreground">
                    {money.format(product.price)}
                  </td>
                  <td className="px-5 py-4">
                    <button
                      onClick={() => void toggle(product)}
                      className={`rounded-full px-3 py-1 text-xs font-bold transition ${
                        product.isAvailable
                          ? 'bg-emerald-50 text-emerald-600 hover:bg-emerald-100'
                          : 'bg-slate-100 text-slate-500 hover:bg-slate-200'
                      }`}
                    >
                      {product.isAvailable ? 'Đang bán' : 'Tạm ẩn'}
                    </button>
                  </td>
                  <td className="px-5 py-4 text-right">
                    <button
                      onClick={() => openForm(product)}
                      title="Sửa"
                      className="mr-1 rounded-lg p-2 text-muted transition hover:bg-black/5 hover:text-orange-500"
                    >
                      <Pencil size={17} />
                    </button>
                    <button
                      onClick={() => void remove(product.id)}
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

      {/* Modal Thêm/Sửa */}
      <AdminModal
        open={showForm}
        onClose={() => setShowForm(false)}
        title={editing ? 'Sửa sản phẩm' : 'Thêm sản phẩm'}
        description="Thông tin món ăn và trạng thái hiển thị."
      >
        <form onSubmit={submit}>
          {!editing && (
            <Field
              label="Restaurant ID"
              value={form.restaurantId}
              onChange={(restaurantId) => setForm({ ...form, restaurantId })}
              required
            />
          )}

          <Field
            label="Tên sản phẩm"
            value={form.name}
            onChange={(name) => setForm({ ...form, name })}
            required
          />

          <label className="mb-4 block text-sm font-semibold text-slate-700">
            Danh mục
            <select
              required
              value={form.categoryId}
              onChange={(e) => setForm({ ...form, categoryId: e.target.value })}
              className="mt-1 w-full rounded-lg border border-slate-200 bg-white px-3 py-2 font-normal outline-none focus:border-orange-500"
            >
              <option value="">Chọn danh mục</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </select>
          </label>

          <Field
            label="Giá"
            type="number"
            value={form.price}
            onChange={(price) => setForm({ ...form, price })}
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
            value={form.displayOrder}
            onChange={(displayOrder) => setForm({ ...form, displayOrder })}
          />

          <div className="mt-4">
            <label className="mb-2 block text-sm font-semibold text-slate-700">
              Ảnh sản phẩm
            </label>

            <div className="rounded-xl border border-dashed border-slate-300 bg-slate-50 p-3">
              <input
                type="file"
                accept="image/*"
                multiple
                onChange={(e) => handleImageFilesChange(e.target.files)}
                className="block w-full text-sm text-slate-600 file:mr-4 file:rounded-lg file:border-0 file:bg-orange-500 file:px-4 file:py-2 file:text-sm file:font-semibold file:text-white hover:file:bg-orange-600"
              />

              {form.previews.length > 0 && (
                <div className="mt-3 flex flex-wrap gap-3">
                  {form.previews.map((preview, index) => (
                    <label
                      key={`${preview}-${index}`}
                      className="flex cursor-pointer flex-col items-center gap-1 text-xs"
                    >
                      <div className="relative h-16 w-16 overflow-hidden rounded-lg border border-slate-200 bg-white">
                        <img src={preview} alt={`preview-${index}`} className="h-full w-full object-cover" />
                        {index === form.mainImageIndex && (
                          <span className="absolute bottom-1 left-1 rounded bg-orange-500 px-1.5 py-0.5 text-[10px] font-bold text-white">
                            Main
                          </span>
                        )}
                      </div>
                      <input
                        type="radio"
                        name="mainImage"
                        checked={index === form.mainImageIndex}
                        onChange={() => setForm((prev) => ({ ...prev, mainImageIndex: index }))}
                        className="h-4 w-4 accent-orange-500"
                      />
                    </label>
                  ))}
                </div>
              )}
            </div>
          </div>

          <label className="mb-3 mt-4 flex items-center gap-2 text-sm font-semibold text-slate-700">
            <input
              type="checkbox"
              checked={form.isAvailable}
              onChange={(e) => setForm({ ...form, isAvailable: e.target.checked })}
              className="rounded border-slate-300 text-orange-500 focus:ring-orange-500"
            />
            Đang bán
          </label>

          <label className="flex items-center gap-2 text-sm font-semibold text-slate-700">
            <input
              type="checkbox"
              checked={form.isFeatured}
              onChange={(e) => setForm({ ...form, isFeatured: e.target.checked })}
              className="rounded border-slate-300 text-orange-500 focus:ring-orange-500"
            />
            Sản phẩm nổi bật
          </label>

          <div className="mt-6 flex justify-end gap-2">
            <button
              type="button"
              onClick={() => setShowForm(false)}
              className="rounded-lg border border-slate-200 px-4 py-2 text-sm font-semibold text-slate-600 hover:bg-slate-50"
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
    </Layout>
  );
}

/* Component Field tái sử dụng */
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