import { useEffect, useState } from 'react';
import { EyeOff, Trash2, Star } from 'lucide-react';
import { AdminLayout } from './AdminLayout';
import { AdminReview, adminService, PagedResult } from '@/services/admin.service';

export default function AdminReviewsPage() {
  const [result, setResult] = useState<PagedResult<AdminReview> | null>(null);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  // Tải danh sách đánh giá
  const loadReviews = async (currentPage: number, isMounted = true) => {
    setLoading(true);
    setError('');
    try {
      const data = await adminService.getReviews({ page: currentPage, pageSize: 10 });
      if (isMounted) setResult(data);
    } catch {
      if (isMounted) setError('Không thể tải danh sách đánh giá.');
    } finally {
      if (isMounted) setLoading(false);
    }
  };

  useEffect(() => {
    let isSubscribed = true;
    void loadReviews(page, isSubscribed);
    return () => {
      isSubscribed = false;
    };
  }, [page]);

  // Xử lý Ẩn hoặc Xóa vĩnh viễn đánh giá
  async function handleRemoveReview(review: AdminReview) {
    if (review.isHidden) {
      if (!window.confirm('Bạn có chắc chắn muốn XÓA VĨNH VIỄN đánh giá này không?')) return;
    }

    try {
      if (review.isHidden) {
        await adminService.deleteReview(review.id);
      } else {
        await adminService.hideReview(review.id);
      }
      await loadReviews(page);
    } catch {
      setError('Không thể cập nhật trạng thái đánh giá.');
    }
  }

  return (
    <AdminLayout>
      {/* Header */}
      <div className="mb-6">
        <h2 className="font-display text-3xl font-bold">Đánh giá</h2>
        <p className="mt-1 text-sm text-muted">
          Kiểm duyệt phản hồi từ khách hàng và ẩn nội dung không phù hợp.
        </p>
      </div>

      {/* Thông báo lỗi */}
      {error && (
        <p className="mb-4 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">
          {error}
        </p>
      )}

      {/* Danh sách bài đánh giá */}
      <div className="space-y-3">
        {loading ? (
          <div className="rounded-2xl border border-border bg-surface px-5 py-12 text-center text-muted shadow-sm">
            Đang tải dữ liệu...
          </div>
        ) : result?.data?.length === 0 ? (
          <div className="rounded-2xl border border-border bg-surface px-5 py-12 text-center text-muted shadow-sm">
            Chưa có đánh giá nào trên hệ thống.
          </div>
        ) : (
          result?.data.map((review) => (
            <article
              key={review.id}
              className="flex items-start justify-between gap-4 rounded-2xl border border-border bg-surface p-5 shadow-sm transition hover:border-orange-500/20"
            >
              <div className="flex-1">
                {/* Tên khách hàng, Trạng thái & Rating */}
                <div className="flex flex-wrap items-center gap-2.5">
                  <h3 className="font-semibold text-foreground">{review.customerName}</h3>

                  {/* Rating Stars */}
                  <div className="flex items-center gap-1 text-amber-400">
                    {Array.from({ length: 5 }).map((_, index) => (
                      <Star
                        key={index}
                        size={15}
                        className={
                          index < review.rating
                            ? 'fill-amber-400 text-amber-400'
                            : 'text-slate-200'
                        }
                      />
                    ))}
                    <span className="ml-1 text-xs font-bold text-slate-600">
                      {review.rating}.0
                    </span>
                  </div>

                  {/* Badge Đã ẩn */}
                  {review.isHidden && (
                    <span className="rounded-full bg-red-500/10 px-2.5 py-0.5 text-xs font-bold text-red-600">
                      Đã ẩn
                    </span>
                  )}
                </div>

                {/* Thông tin món ăn & Ngày tạo */}
                <p className="mt-1 text-xs text-muted">
                  Sản phẩm: <span className="font-medium text-foreground">{review.productName}</span>{' '}
                  • {new Date(review.createdAt).toLocaleDateString('vi-VN')}
                </p>

                {/* Nội dung comment */}
                <p className="mt-3 text-sm text-slate-700 leading-relaxed">
                  {review.comment || <em className="text-muted">Không có nội dung nhận xét.</em>}
                </p>
              </div>

              {/* Nút hành động */}
              <div className="flex shrink-0 gap-1">
                <button
                  onClick={() => void handleRemoveReview(review)}
                  title={review.isHidden ? 'Xóa vĩnh viễn' : 'Ẩn đánh giá này'}
                  className={`rounded-xl p-2.5 transition ${
                    review.isHidden
                      ? 'text-red-500 hover:bg-red-50 hover:text-red-600'
                      : 'text-muted hover:bg-black/5 hover:text-orange-500'
                  }`}
                >
                  {review.isHidden ? <Trash2 size={18} /> : <EyeOff size={18} />}
                </button>
              </div>
            </article>
          ))
        )}
      </div>

      {/* Phân trang */}
      <div className="mt-6 flex items-center justify-between text-sm text-muted">
        <span>
          Trang <strong className="text-foreground">{page}</strong> / {result?.meta?.totalPages || 1}
        </span>
        <div className="flex gap-2">
          <button
            disabled={page <= 1}
            onClick={() => setPage((p) => p - 1)}
            className="rounded-xl border border-border px-4 py-2 text-xs font-semibold transition hover:bg-black/5 disabled:opacity-40"
          >
            Trước
          </button>
          <button
            disabled={page >= (result?.meta?.totalPages || 1)}
            onClick={() => setPage((p) => p + 1)}
            className="rounded-xl border border-border px-4 py-2 text-xs font-semibold transition hover:bg-black/5 disabled:opacity-40"
          >
            Sau
          </button>
        </div>
      </div>
    </AdminLayout>
  );
}