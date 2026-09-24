import { FormEvent, useEffect, useState } from 'react';
import { Pencil, Plus, Search, Trash2 } from 'lucide-react';
import { RestaurantLayout } from '@/pages/Restaurant/RestaurantLayout';
import { AdminModal } from '@/components/admin/AdminModal';
import { categoryService, CategoryResponse } from '@/services/category.service';
import { productService } from '@/services/product.service';
import { ProductResponseDto } from '@/types/product';
import { AdminLayout } from './AdminLayout';

type ProductForm = {
  restaurantId: string;
  categoryId: string;
  name: string;
  description: string;
  price: string;
  isAvailable: boolean;
  isFeatured: boolean;
  displayOrder: string;
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
  function openForm(product?: ProductResponseDto) {
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
          }
        : emptyForm
    );
    setShowForm(true);
  }

  // Submit Form
  async function submit(event: FormEvent) {
    event.preventDefault();
    setSubmitting(true);
    setError('');
    try {
      if (editing) {
        await productService.update(editing.id, {
          name: form.name,
          price: Number(form.price),
          isAvailable: form.isAvailable,
          isFeatured: form.isFeatured,
          displayOrder: Number(form.displayOrder),
        });
      } else {
        await productService.create({
          restaurantId: form.restaurantId,
          categoryId: form.categoryId,
          name: form.name,
          description: form.description,
          price: Number(form.price),
          isAvailable: form.isAvailable,
          isFeatured: form.isFeatured,
          displayOrder: Number(form.displayOrder),
        });
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
        <label className="flex items-center gap-2 rounded-xl border border-border bg-surface px-3.5 py-2 text-sm text-muted shadow-sm focus-within:border-orange-500">
          <Search size={17} />
          <input
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Tìm sản phẩm..."
            className="bg-transparent text-foreground outline-none placeholder:text-muted/60"
          />
        </label>
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

          {!editing && (
            <Field
              label="Mô tả"
              value={form.description}
              onChange={(description) => setForm({ ...form, description })}
            />
          )}

          <Field
            label="Thứ tự hiển thị"
            type="number"
            value={form.displayOrder}
            onChange={(displayOrder) => setForm({ ...form, displayOrder })}
          />

          <label className="mb-3 flex items-center gap-2 text-sm font-semibold text-slate-700">
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
  type?: string;
  required?: boolean;
}) {
  return (
    <label className="mb-4 block text-sm font-semibold text-slate-700">
      {label}
      <input
        required={required}
        type={type}
        value={value}
        onChange={(event) => onChange(event.target.value)}
        className="mt-1 w-full rounded-lg border border-slate-200 bg-white px-3 py-2 font-normal outline-none focus:border-orange-500"
      />
    </label>
  );
}