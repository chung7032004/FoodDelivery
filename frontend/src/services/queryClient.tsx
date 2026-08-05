// src/api/queryClient.ts
import { QueryClient, QueryCache, MutationCache } from "@tanstack/react-query";
import type { ApiError } from "./api";
import { toast } from "sonner";
import { AlertTriangle, X } from "lucide-react";

const TOAST_DURATION = 4000;

function handleApiError(error: ApiError) {
  const message = error.message ?? "Không thể kết nối tới máy chủ. Vui lòng thử lại.";

  toast.custom(
    (id) => (
      <div
        className="group relative w-full max-w-sm overflow-hidden rounded-2xl bg-[#18181b] border border-white/[0.06] shadow-[0_8px_24px_rgba(0,0,0,0.45),0_0_0_1px_rgba(226,75,74,0.12)] px-4 pt-4 pb-[18px]"
        onMouseEnter={(e) => {
          const bar = e.currentTarget.querySelector<HTMLDivElement>("[data-bar]");
          if (bar) bar.style.animationPlayState = "paused";
        }}
        onMouseLeave={(e) => {
          const bar = e.currentTarget.querySelector<HTMLDivElement>("[data-bar]");
          if (bar) bar.style.animationPlayState = "running";
        }}
      >
        <div className="flex items-start gap-3">
          <div className="flex-shrink-0 w-[34px] h-[34px] rounded-full bg-red-500/10 flex items-center justify-center">
            <AlertTriangle className="w-[18px] h-[18px] text-red-300" />
          </div>
          <div className="flex-1 min-w-0 pt-px">
            <p className="text-sm font-medium text-white tracking-tight">Đã có lỗi xảy ra</p>
            <p className="mt-[5px] text-[13px] text-zinc-400 leading-snug">{message}</p>
          </div>
          <button
            onClick={() => toast.dismiss(id)}
            aria-label="Đóng"
            className="flex-shrink-0 -mr-1 -mt-0.5 p-1 rounded-md text-zinc-500 hover:text-white hover:bg-white/[0.06] transition-colors"
          >
            <X className="w-[15px] h-[15px]" />
          </button>
        </div>
        <div className="absolute left-0 bottom-0 h-[2.5px] w-full bg-white/[0.06]">
          <div
            data-bar
            className="h-full origin-left bg-gradient-to-r from-red-500 to-red-300"
            style={{ animation: `toast-shrink ${TOAST_DURATION}ms linear forwards` }}
          />
        </div>
      </div>
    ),
    { duration: TOAST_DURATION }
  );
}

export const queryClient = new QueryClient({
  queryCache: new QueryCache({
    onError: (error) => handleApiError(error as unknown as ApiError),
  }),
  mutationCache: new MutationCache({
    onError: (error) => handleApiError(error as unknown as ApiError),
  }),
  defaultOptions: {
    queries: {
      retry: 1,
    },
  },
});