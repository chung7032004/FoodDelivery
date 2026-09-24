import { useEffect, useState } from 'react';
import { Boxes, MessageSquare, ShoppingBag, Truck, Users } from 'lucide-react';
import type { LucideIcon } from 'lucide-react';
import { AdminLayout } from './AdminLayout';
import { adminService } from '@/services/admin.service';
import { categoryService } from '@/services/category.service';
import { productService } from '@/services/product.service';
import {
  LoadingState,
  PageHeader,
  Panel,
  PanelLink,
  ProgressBar,
  StatCard,
} from '@/components/admin/AdminUI';

interface DashboardStats {
  users: number;
  orders: number;
  categories: number;
  products: number;
  pendingShippers: number;
}

export default function AdminDashboardPage() {
  const [stats, setStats] = useState<DashboardStats>({
    users: 0,
    orders: 0,
    categories: 0,
    products: 0,
    pendingShippers: 0,
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    async function fetchDashboardData() {
      setLoading(true);
      setError('');
      try {
        const [users, orders, categories, products, shippers] = await Promise.all([
          adminService.getUsers({ page: 1, pageSize: 1 }),
          adminService.getOrders({ page: 1, pageSize: 1 }),
          categoryService.getAll({ page: 1, pageSize: 1 }),
          productService.getAll({ page: 1, pageSize: 1 }),
          adminService.getPendingShippers(1, 1),
        ]);

        setStats({
          users: users?.meta?.totalCount || 0,
          orders: orders?.meta?.totalCount || 0,
          categories: categories?.data?.meta?.totalCount || 0,
          products: products?.data?.meta?.totalCount || 0,
          pendingShippers: shippers?.meta?.totalCount || 0,
        });
      } catch (err) {
        console.error('Lỗi tải dữ liệu dashboard:', err);
        setError('Không thể tải dữ liệu tổng quan. Vui lòng thử lại sau.');
      } finally {
        setLoading(false);
      }
    }

    void fetchDashboardData();
  }, []);

  return (
    <AdminLayout>
      <PageHeader
        eyebrow="Tổng quan hệ thống"
        title="Bảng điều hành EatRush"
        description="Theo dõi sức khỏe nền tảng, nội dung thực đơn và các hoạt động cần xử lý trong ngày."
        action={
          <span className="rounded-lg border border-border bg-surface px-3 py-2 text-xs font-semibold text-muted shadow-sm">
            Cập nhật trực tiếp
          </span>
        }
      />

      {error && (
        <p className="mb-6 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">
          {error}
        </p>
      )}

      {loading ? (
        <Panel>
          <LoadingState />
        </Panel>
      ) : (
        <>
          {/* Stat Cards Grid */}
          <div className="mb-6 grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
            <StatCard
              label="Tài khoản"
              value={stats.users}
              detail="Tổng người dùng trong hệ thống"
              icon={Users}
              tone="blue"
              href="/admin/users"
            />
            <StatCard
              label="Đơn hàng"
              value={stats.orders}
              detail="Tổng đơn hàng đã ghi nhận"
              icon={ShoppingBag}
              tone="orange"
              href="/admin/orders"
            />
            <StatCard
              label="Danh mục"
              value={stats.categories}
              detail="Nhóm món đang được quản lý"
              icon={Boxes}
              tone="purple"
              href="/admin/categories"
            />
            <StatCard
              label="Sản phẩm"
              value={stats.products}
              detail="Món ăn trong toàn hệ thống"
              icon={ShoppingBag}
              tone="green"
              href="/admin/products"
            />
          </div>

          {/* Middle Layout: Work Center & Operational Status */}
          <div className="grid gap-6 xl:grid-cols-[1.4fr_0.9fr]">
            <Panel
              title="Trung tâm công việc"
              subtitle="Các khu vực quản trị chính"
              action={<PanelLink to="/admin/users">Mở quản lý</PanelLink>}
            >
              <div className="grid gap-3 p-5 sm:grid-cols-2">
                <WorkItem
                  icon={Users}
                  title="Tài khoản & phân quyền"
                  value={`${stats.users} tài khoản`}
                  to="/admin/users"
                  color="text-sky-600 bg-sky-500/10"
                />
                <WorkItem
                  icon={Boxes}
                  title="Danh mục món ăn"
                  value={`${stats.categories} danh mục`}
                  to="/admin/categories"
                  color="text-violet-600 bg-violet-500/10"
                />
                <WorkItem
                  icon={ShoppingBag}
                  title="Sản phẩm & thực đơn"
                  value={`${stats.products} sản phẩm`}
                  to="/admin/products"
                  color="text-emerald-600 bg-emerald-500/10"
                />
                <WorkItem
                  icon={Truck}
                  title="Hồ sơ shipper"
                  value={`${stats.pendingShippers} hồ sơ chờ duyệt`}
                  to="/admin/shippers"
                  color="text-orange-600 bg-orange-500/10"
                />
              </div>
            </Panel>

            <Panel title="Tình trạng vận hành" subtitle="Chỉ số cần theo dõi">
              <div className="space-y-6 p-5">
                <ProgressBar label="Tài khoản đang hoạt động" value={94} color="bg-sky-500" />
                <ProgressBar label="Đơn hàng hoàn tất" value={86} color="bg-emerald-500" />
                <ProgressBar label="Sản phẩm đang bán" value={78} color="bg-orange-500" />
                <div className="border-t border-border pt-4 text-xs text-muted">
                  <span className="font-bold text-foreground">{stats.pendingShippers}</span> hồ sơ
                  shipper đang chờ admin xử lý.
                </div>
              </div>
            </Panel>
          </div>

          {/* Bottom Layout: Action Queue & Quick Links */}
          <div className="mt-6 grid gap-6 lg:grid-cols-2">
            <Panel title="Cần xử lý" subtitle="Các hàng đợi đang mở">
              <QueueRow
                icon={Truck}
                label="Hồ sơ shipper chờ duyệt"
                count={stats.pendingShippers}
                to="/admin/shippers"
              />
              <QueueRow
                icon={MessageSquare}
                label="Đánh giá cần kiểm duyệt"
                count="Xem danh sách"
                to="/admin/reviews"
              />
              <QueueRow
                icon={ShoppingBag}
                label="Đơn hàng toàn hệ thống"
                count={stats.orders}
                to="/admin/orders"
              />
            </Panel>

            <Panel title="Truy cập nhanh" subtitle="Thao tác thường dùng">
              <div className="grid grid-cols-2 gap-3 p-5">
                <QuickLink title="Thêm danh mục" to="/admin/categories" />
                <QuickLink title="Thêm sản phẩm" to="/admin/products" />
                <QuickLink title="Duyệt shipper" to="/admin/shippers" />
                <QuickLink title="Kiểm tra đánh giá" to="/admin/reviews" />
              </div>
            </Panel>
          </div>
        </>
      )}
    </AdminLayout>
  );
}

/* --- Sub-components --- */

interface WorkItemProps {
  icon: LucideIcon;
  title: string;
  value: string;
  to: string;
  color: string;
}

function WorkItem({ icon: Icon, title, value, to, color }: WorkItemProps) {
  return (
    <a
      href={to}
      className="flex items-center gap-3 rounded-xl border border-border p-4 transition-all duration-200 hover:border-orange-500/40 hover:bg-orange-500/[.02] hover:shadow-sm"
    >
      <span className={`rounded-lg p-2.5 ${color}`}>
        <Icon size={19} />
      </span>
      <span>
        <span className="block text-sm font-bold">{title}</span>
        <span className="mt-1 block text-xs text-muted">{value}</span>
      </span>
    </a>
  );
}

interface QueueRowProps {
  icon: LucideIcon;
  label: string;
  count: string | number;
  to: string;
}

function QueueRow({ icon: Icon, label, count, to }: QueueRowProps) {
  return (
    <a
      href={to}
      className="flex items-center justify-between border-b border-border px-5 py-4 transition-colors last:border-0 hover:bg-black/[.02]"
    >
      <span className="flex items-center gap-3 text-sm font-semibold">
        <Icon size={18} className="text-orange-500" />
        {label}
      </span>
      <span className="rounded-md bg-orange-50 px-2.5 py-1 text-xs font-bold text-orange-600">
        {count}
      </span>
    </a>
  );
}

interface QuickLinkProps {
  title: string;
  to: string;
}

function QuickLink({ title, to }: QuickLinkProps) {
  return (
    <a
      href={to}
      className="rounded-xl border border-border p-4 text-sm font-bold transition-all duration-200 hover:border-orange-500/50 hover:text-orange-600 hover:shadow-sm"
    >
      {title}
      <span className="mt-2 block text-xs font-normal text-muted">Mở ngay →</span>
    </a>
  );
}