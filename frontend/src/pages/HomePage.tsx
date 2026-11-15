import { useCallback, useEffect, useMemo, useState } from 'react';
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { ClipboardList, CreditCard, Bike } from 'lucide-react';
import { toast } from 'sonner';
import { HeaderNav } from '../components/layout/header/HeaderNav';
import { Footer } from '../components/layout/Footer';
import { HeroSection } from '../components/sections/HeroSection';
import { CategoryList } from '../components/sections/CategoryList';
import { DishGrid } from '../components/sections/DishCard';
import Skeleton from '../components/ui/Skeleton';
import { categoryService } from '../services/category.service';
import { productService } from '../services/product.service';
import type { Category } from '@/types/category';
import type { Dish } from '@/types';
import type { ProductResponseDto } from '@/types/product';
import type { PagedResponse } from '@/types/common';
import { useAuth } from '@/contexts/AuthContext';
import { cartQueryKey, cartService } from '@/services/cart.service';
import { mapProductToDish } from '@/utils/mappers';

const HOME_PAGE_SIZE = 12;

export function HomePage() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const { isAuthenticated } = useAuth();
  const [selectedCategoryId, setSelectedCategoryId] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [debouncedSearch, setDebouncedSearch] = useState('');

  useEffect(() => {
    const timer = window.setTimeout(() => setDebouncedSearch(searchTerm.trim()), 300);
    return () => window.clearTimeout(timer);
  }, [searchTerm]);

  const addToCartMutation = useMutation({
    mutationFn: (dish: Dish) => cartService.addItem(dish.id, 1),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: cartQueryKey });
      toast.success('Đã thêm món vào giỏ hàng.');
    },
  });

  const {
    data: categories,
    isLoading: isCategoriesLoading,
    isError: isCategoriesError,
    refetch: refetchCategories,
  } = useQuery({
    queryKey: ['home-categories'],
    queryFn: async (): Promise<Category[]> => {
      const response = await categoryService.getAll({ page: 1, pageSize: 100 });
      if (!response.isSuccess || !response.data) {
        throw new Error(response.message ?? 'Không thể tải danh mục món ăn.');
      }
      return response.data.data
        .sort((left, right) => left.displayOrder - right.displayOrder)
        .map((category) => ({
          id: category.id,
          name: category.name,
          description: category.description ?? '',
          displayOrder: category.displayOrder,
          imageUrl: category.imageUrl,
        }));
    },
    staleTime: 5 * 60 * 1000,
  });

  const {
    data: productPages,
    isLoading: isProductsLoading,
    isError: isProductsError,
    isFetchingNextPage,
    hasNextPage,
    fetchNextPage,
    refetch: refetchProducts,
  } = useInfiniteQuery({
    queryKey: ['home-products', selectedCategoryId, debouncedSearch],
    initialPageParam: 1,
    queryFn: async ({ pageParam }): Promise<PagedResponse<ProductResponseDto>> => {
      const response = await productService.getAll({
        page: pageParam,
        pageSize: HOME_PAGE_SIZE,
        categoryId: selectedCategoryId ?? undefined,
        searchQuery: debouncedSearch || undefined,
      });
      if (!response.isSuccess || !response.data) {
        throw new Error(response.message ?? 'Không thể tải danh sách món ăn.');
      }
      return response.data;
    },
    getNextPageParam: (lastPage) =>
      lastPage.meta.page < lastPage.meta.totalPages ? lastPage.meta.page + 1 : undefined,
  });

  const dishes = useMemo(
    () => productPages?.pages.flatMap((page) => page.data.map(mapProductToDish)) ?? [],
    [productPages]
  );

  const handleAddToCart = (dish: Dish) => {
    if (!isAuthenticated) {
      navigate('/login');
      return;
    }
    addToCartMutation.mutate(dish);
  };

  const handleLoadMore = useCallback(() => {
    if (hasNextPage && !isFetchingNextPage) {
      void fetchNextPage();
    }
  }, [fetchNextPage, hasNextPage, isFetchingNextPage]);

  const handleSearch = () => {
    document.getElementById('mon-an')?.scrollIntoView({ behavior: 'smooth', block: 'start' });
  };

  return (
    <div className="min-h-screen bg-[#fffdfa] font-sans text-slate-900">
      <HeaderNav />

      <main>
        <HeroSection
          searchTerm={searchTerm}
          onSearchChange={setSearchTerm}
          onSearch={handleSearch}
        />

        {isCategoriesLoading ? (
          <CategoryListSkeleton />
        ) : isCategoriesError ? (
          <div className="mx-auto max-w-7xl px-4 pb-6 text-sm text-slate-600 sm:px-6 lg:px-8">
            Không tải được danh mục.{' '}
            <button type="button" onClick={() => void refetchCategories()} className="font-semibold text-orange-600 hover:underline">
              Thử lại
            </button>
          </div>
        ) : (
          <CategoryList
            categories={categories ?? []}
            selectedCategoryId={selectedCategoryId}
            onSelectCategory={setSelectedCategoryId}
          />
        )}

        <DishGrid
          dishes={dishes}
          isLoading={isProductsLoading}
          isFetchingNextPage={isFetchingNextPage}
          hasNextPage={Boolean(hasNextPage)}
          error={isProductsError}
          onAddToCart={handleAddToCart}
          onLoadMore={handleLoadMore}
          onRetry={() => {
            if (dishes.length > 0) handleLoadMore();
            else void refetchProducts();
          }}
        />

        <HomeOrderingSteps />
      </main>

      <Footer />
    </div>
  );
}

function CategoryListSkeleton() {
  return (
    <section aria-label="Đang tải danh mục" className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
      <div className="mb-5">
        <Skeleton shape="text" width={180} height={28} />
      </div>
      <div className="flex gap-3 overflow-hidden">
        {Array.from({ length: 7 }).map((_, index) => (
          <Skeleton key={index} shape="rect" width={92} height={94} className="shrink-0 rounded-2xl" />
        ))}
      </div>
    </section>
  );
}

function HomeOrderingSteps() {
  const steps = [
    { icon: ClipboardList, title: 'Chọn món yêu thích', description: 'Khám phá thực đơn và thêm món vào giỏ hàng.' },
    { icon: CreditCard, title: 'Xác nhận đơn hàng', description: 'Kiểm tra giỏ hàng, địa chỉ và phương thức thanh toán.' },
    { icon: Bike, title: 'Theo dõi đơn', description: 'Cập nhật trạng thái đơn hàng trong tài khoản của bạn.' },
  ];

  return (
    <section className="mx-auto max-w-7xl px-4 pb-14 sm:px-6 lg:px-8">
      <div className="rounded-3xl border border-orange-100 bg-gradient-to-br from-orange-50 to-white p-6 sm:p-8">
        <div className="mb-6">
          <p className="text-xs font-bold uppercase tracking-[0.16em] text-orange-600">Bắt đầu thật dễ dàng</p>
          <h2 className="mt-1 font-display text-xl font-bold text-slate-950 sm:text-2xl">Đặt món trong vài bước</h2>
        </div>
        <div className="grid gap-5 md:grid-cols-3 md:gap-8">
          {steps.map(({ icon: Icon, title, description }, index) => (
            <div key={title} className="flex items-start gap-4">
              <span className="flex h-11 w-11 shrink-0 items-center justify-center rounded-2xl bg-white text-orange-600 shadow-sm ring-1 ring-orange-100">
                <Icon size={20} />
              </span>
              <div>
                <p className="text-xs font-bold uppercase tracking-wider text-orange-500">Bước {index + 1}</p>
                <h3 className="mt-1 text-sm font-bold text-slate-900">{title}</h3>
                <p className="mt-1 text-sm leading-6 text-slate-600">{description}</p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
