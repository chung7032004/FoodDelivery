import React, { useState } from "react";
import {
  Star,
  Minus,
  Plus,
  ShoppingCart,
  Store,
  Tag,
  Check,
  ChevronRight,
} from "lucide-react";
import { HeaderNav } from "@/components/layout/HeaderNav";
import { Footer } from "@/components/layout/Footer";
import { useQuery } from "@tanstack/react-query";
import { productService } from "@/services/product.service";
import { ProductDetailResponseDto } from "@/types/product";
import { ApiResult } from "@/types";
import { useParams } from "react-router-dom";
import { getImageUrl } from "@/utils/image";

// ---------------------------------------------------------------------------
// MOCK DATA — shaped like ProductDetailResponseDto + a paginated review slice
// ---------------------------------------------------------------------------
const PRODUCT = {
  id: "e3b1c9a0-14f2-4b7a-9e2d-8f6a1c0d9b21",
  name: "Lẩu Thái Hải Sản Đặc Biệt",
  description:
    "Nước lẩu chua cay đậm đà ninh từ xương ống 6 tiếng, kết hợp sả, riềng, lá chanh và me Thái. Ăn kèm tôm sú, mực ống, chả cá thác lác và nấm kim châm tươi. Phần ăn dành cho 2-3 người, đi kèm rau nhúng theo mùa.",
  price: 249000,
  isAvailable: true,
  isFeatured: true,
  categoryName: "Món Lẩu",
  restaurantName: "Nhà Hàng Sông Trăng",
  averageRating: 4.6,
  reviewCount: 128,
  images: [
    { id: "1", imageUrl: "https://picsum.photos/seed/laut-hero/900/900", isMain: true },
    { id: "2", imageUrl: "https://picsum.photos/seed/laut-side1/900/900", isMain: false },
    { id: "3", imageUrl: "https://picsum.photos/seed/laut-side2/900/900", isMain: false },
    { id: "4", imageUrl: "https://picsum.photos/seed/laut-side3/900/900", isMain: false },
  ],
};

const RATING_BREAKDOWN = [
  { stars: 5, count: 96 },
  { stars: 4, count: 21 },
  { stars: 3, count: 7 },
  { stars: 2, count: 3 },
  { stars: 1, count: 1 },
];

const REVIEWS = [
  {
    id: "r1",
    customerName: "Minh T.",
    rating: 5,
    comment:
      "Nước lẩu chua cay vừa miệng, hải sản tươi. Giao đúng giờ, đóng gói cẩn thận nên nước không bị đổ.",
    createdAt: "2 ngày trước",
  },
  {
    id: "r2",
    customerName: "Hồng Nhung",
    rating: 4,
    comment: "Ngon nhưng phần rau hơi ít so với giá. Chả cá thác lác rất được.",
    createdAt: "5 ngày trước",
  },
  {
    id: "r3",
    customerName: "Quốc Bảo",
    rating: 5,
    comment: "Ăn cho 2 người là vừa đủ no. Sẽ đặt lại vào cuối tuần.",
    createdAt: "1 tuần trước",
  },
];

// ---------------------------------------------------------------------------

function formatVnd(value: number) {
  return value.toLocaleString("vi-VN") + "đ";
}

function StarRow({ rating = 0, size = 14 }) {
  return (
    <div className="flex items-center gap-0.5">
      {[1, 2, 3, 4, 5].map((i) => (
        <Star
          key={i}
          size={size}
          className={
            i <= Math.round(rating)
              ? "fill-[#ffb648] text-[#ffb648]"
              : "fill-transparent text-slate-300"
          }
        />
      ))}
    </div>
  );
}

function RatingDial({ rating = 0, reviewCount = 0 }) {
  const r = 46;
  const circumference = 2 * Math.PI * r;
  const pct = Math.max(0, Math.min(1, rating / 5));
  const offset = circumference * (1 - pct);

  return (
    <div className="flex items-center gap-4">
      <div className="relative shrink-0 w-[108px] h-[108px]">
        <svg width="108" height="108" viewBox="0 0 108 108" className="-rotate-90">
          <circle
            cx="54"
            cy="54"
            r={r}
            fill="none"
            stroke="#e2e8f0"
            strokeWidth="9"
          />
          <circle
            cx="54"
            cy="54"
            r={r}
            fill="none"
            stroke="#fb6900"
            strokeWidth="9"
            strokeLinecap="round"
            strokeDasharray={circumference}
            strokeDashoffset={offset}
            style={{ transition: "stroke-dashoffset 700ms ease" }}
          />
        </svg>
        <div className="absolute inset-0 flex flex-col items-center justify-center">
          <span className="font-serif text-[26px] leading-none text-slate-900">
            {rating.toFixed(1)}
          </span>
          <span className="mt-1 text-[10px] uppercase tracking-wider text-slate-500">
            trên 5
          </span>
        </div>
      </div>
      <div>
        <StarRow rating={rating} size={16} />
        <p className="mt-1.5 text-sm text-slate-500">
          {reviewCount} lượt đánh giá
        </p>
      </div>
    </div>
  );
}

export default function ProductDetailPage() {
  const [selectedImage, setSelectedImage] = useState(0);
  const [quantity, setQuantity] = useState(1);
  const [activeTab, setActiveTab] = useState("desc");
  const { productId } = useParams<{ productId: string }>();
  const maxCount = RATING_BREAKDOWN[0].count;
  const { data: productDetail, isLoading: isProductDetailLoading } = useQuery({
      queryKey: ['product-detail', productId],
      queryFn: async (): Promise<ApiResult<ProductDetailResponseDto>> => {
        const res = await productService.getDetailById(productId!); // hoặc productId ?? ""
        return res;
      },
      enabled: !!productId,       //không chạy query khi chưa có productId
      staleTime: 5 * 60 * 1000,
  });

  return (
    <div className="min-h-screen bg-white font-sans text-slate-900">
      <HeaderNav />
      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Fraunces:opsz,wght@9..144,500;9..144,600&family=Inter:wght@400;500;600;700&display=swap');
        .font-serif { font-family: 'Fraunces', serif; }
        .font-sans { font-family: 'Inter', sans-serif; }
      `}</style>

      {/* Top bar */}
      <div className="max-w-6xl mx-auto px-5 lg:px-8 h-10 flex items-center justify-between">
        <div className="flex items-center gap-1.5 text-l font-medium text-[#fb6900]">
          <Store size={20} className="text-[#fb6900]" />
          {productDetail?.data?.restaurantName}
          <ChevronRight size={16} className="text-[#fb6900]" />
        </div>
      </div>

      <section className="max-w-6xl mx-auto px-5 lg:px-8 py-8 lg:py-12">
        <div className="grid grid-cols-1 lg:grid-cols-[1.1fr_0.9fr] gap-10">
          {/* ---------------- Gallery ---------------- */}
          <div>
            <div className="relative rounded-2xl overflow-hidden bg-slate-100 aspect-square">
              <img
                src={getImageUrl(productDetail?.data?.images[selectedImage]?.imageUrl)}
                alt={productDetail?.data?.name}
                className="w-full h-full object-cover"
              />
              {productDetail?.data?.isFeatured && (
                <span className="absolute top-4 left-4 inline-flex items-center gap-1 rounded-full bg-[#fb6900] px-3 py-1 text-xs font-semibold text-white">
                  Nổi bật
                </span>
              )}
            </div>
            <div className="mt-3 grid grid-cols-4 gap-3">
              {productDetail?.data?.images.map((img, i) => (
                <button
                  key={img.id}
                  onClick={() => setSelectedImage(i)}
                  className={`rounded-xl overflow-hidden aspect-square border-2 transition-colors ${
                    i === selectedImage
                      ? "border-[#fb6900]"
                      : "border-transparent hover:border-slate-300"
                  }`}
                >
                  <img
                    src={getImageUrl(img.imageUrl)}
                    alt=""
                    className="w-full h-full object-cover"
                  />
                </button>
              ))}
            </div>
          </div>

          {/* ---------------- Info panel ---------------- */}
          <div className="lg:sticky lg:top-24 self-start">
            <div className="flex items-center gap-2 text-xs">
              <span className="inline-flex items-center gap-1 rounded-full bg-[#fb6900]/10 text-[#fb6900] px-2.5 py-1 font-medium">
                <Tag size={12} />
                {productDetail?.data?.categoryName}
              </span>
              {productDetail?.data?.isAvailable ? (
                <span className="inline-flex items-center gap-1 rounded-full bg-emerald-50 text-emerald-600 px-2.5 py-1 font-medium">
                  <Check size={12} />
                  Còn hàng
                </span>
              ) : (
                <span className="rounded-full bg-slate-100 text-slate-500 px-2.5 py-1 font-medium">
                  Hết hàng
                </span>
              )}
            </div>

            <h1 className="font-serif text-3xl mt-4 leading-tight text-slate-900">
              {productDetail?.data?.name}
            </h1>

            <div className="mt-3 flex items-center gap-2">
              <StarRow rating={productDetail?.data?.averageRating} />
              <span className="text-sm text-slate-500">
                {productDetail?.data?.averageRating?.toFixed(1)} · {productDetail?.data?.reviewCount} đánh giá
              </span>
            </div>

            <p className="font-serif text-3xl text-[#fb6900] mt-5">
              {formatVnd(productDetail?.data?.price ?? 0)}
            </p>

            <p className="mt-4 text-sm leading-relaxed text-slate-600">
              {productDetail?.data?.description}
            </p>

            <div className="mt-6 flex items-center gap-4">
              <div className="flex items-center border border-slate-300 rounded-xl overflow-hidden">
                <button
                  onClick={() => setQuantity((q) => Math.max(1, q - 1))}
                  className="w-10 h-11 flex items-center justify-center text-slate-500 hover:text-slate-900 hover:bg-slate-100 transition-colors"
                >
                  <Minus size={16} />
                </button>
                <span className="w-10 text-center text-sm font-medium text-slate-900">
                  {quantity}
                </span>
                <button
                  onClick={() => setQuantity((q) => Math.min(20, q + 1))}
                  className="w-10 h-11 flex items-center justify-center text-slate-500 hover:text-slate-900 hover:bg-slate-100 transition-colors"
                >
                  <Plus size={16} />
                </button>
              </div>

              <button className="flex-1 h-11 rounded-xl bg-[#fb6900] text-white font-semibold text-sm flex items-center justify-center gap-2 hover:bg-[#e35f00] transition-colors">
                <ShoppingCart size={17} />
                Thêm vào giỏ · {formatVnd((productDetail?.data?.price ?? 0) * quantity)}
              </button>
            </div>
          </div>
        </div>

        {/* ---------------- Tabs ---------------- */}
        <div className="mt-12 border-b border-slate-200 flex gap-6">
          {[
            { key: "desc", label: "Mô tả" },
            { key: "reviews", label: `Đánh giá (${productDetail?.data?.reviewCount})` },
          ].map((tab) => (
            <button
              key={tab.key}
              onClick={() => setActiveTab(tab.key)}
              className={`pb-3 text-sm font-medium border-b-2 transition-colors ${
                activeTab === tab.key
                  ? "border-[#fb6900] text-slate-900"
                  : "border-transparent text-slate-500 hover:text-slate-900"
              }`}
            >
              {tab.label}
            </button>
          ))}
        </div>

        {activeTab === "desc" ? (
          <div className="mt-6 max-w-2xl text-sm leading-relaxed text-slate-600">
            <p>{productDetail?.data?.description}</p>
            <p className="mt-4">
              Phục vụ kèm bếp cồn mini và nước chấm hải sản pha sẵn. Có thể yêu
              cầu điều chỉnh độ cay khi đặt hàng ở phần ghi chú.
            </p>
          </div>
        ) : (
          <div className="mt-6 grid grid-cols-1 lg:grid-cols-[0.8fr_1.2fr] gap-10">
            {/* Breakdown */}
            <div>
              <RatingDial
                rating={PRODUCT.averageRating}
                reviewCount={PRODUCT.reviewCount}
              />
              <div className="mt-6 space-y-2 max-w-xs">
                {RATING_BREAKDOWN.map((row) => (
                  <div key={row.stars} className="flex items-center gap-2.5">
                    <span className="text-xs text-slate-500 w-8">
                      {row.stars} sao
                    </span>
                    <div className="flex-1 h-1.5 rounded-full bg-slate-200 overflow-hidden">
                      <div
                        className="h-full rounded-full bg-[#fb6900]"
                        style={{ width: `${(row.count / maxCount) * 100}%` }}
                      />
                    </div>
                    <span className="text-xs text-slate-500 w-6 text-right">
                      {row.count}
                    </span>
                  </div>
                ))}
              </div>
            </div>

            {/* Review list */}
            <div className="space-y-5">
              {REVIEWS.map((r) => (
                <div
                  key={r.id}
                  className="rounded-xl border border-slate-200 p-4"
                >
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2.5">
                      <div className="w-8 h-8 rounded-full bg-[#fb6900]/15 text-[#fb6900] text-xs font-semibold flex items-center justify-center">
                        {r.customerName.charAt(0)}
                      </div>
                      <span className="text-sm font-medium text-slate-900">
                        {r.customerName}
                      </span>
                    </div>
                    <span className="text-xs text-slate-500">
                      {r.createdAt}
                    </span>
                  </div>
                  <div className="mt-2.5">
                    <StarRow rating={r.rating} size={13} />
                  </div>
                  <p className="mt-2 text-sm leading-relaxed text-slate-600">
                    {r.comment}
                  </p>
                </div>
              ))}

              <button className="w-full h-10 rounded-xl border border-slate-300 text-sm text-slate-500 hover:text-slate-900 hover:border-slate-400 transition-colors">
                Xem thêm đánh giá
              </button>
            </div>
          </div>
        )}
      </section>
      <Footer />
    </div>
  );
}