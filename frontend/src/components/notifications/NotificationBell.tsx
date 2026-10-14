import { useEffect, useRef, useState } from 'react';
import { createPortal } from 'react-dom';
import { Bell, CheckCheck } from 'lucide-react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '@/contexts/AuthContext';
import {
  notificationQueryKey,
  notificationService,
  type NotificationItem,
} from '@/services/notification.service';

export function NotificationBell({ appearance = 'dark' }: { appearance?: 'dark' | 'light' }) {
  const { user, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const containerRef = useRef<HTMLDivElement>(null);
  const panelRef = useRef<HTMLElement>(null);
  const [open, setOpen] = useState(false);
  const [panelPosition, setPanelPosition] = useState<{ left: number; top: number; maxHeight: number } | null>(null);
  const queryKey = notificationQueryKey(user?.id ?? '');
  const { data, isPending, isError } = useQuery({
    queryKey,
    queryFn: notificationService.getNotifications,
    enabled: isAuthenticated && !!user?.id,
    refetchInterval: 60_000,
  });
  const summary = data?.data;

  const refreshNotifications = () =>
    queryClient.invalidateQueries({ queryKey });

  const markReadMutation = useMutation({
    mutationFn: notificationService.markAsRead,
    onSuccess: refreshNotifications,
  });

  const markAllReadMutation = useMutation({
    mutationFn: notificationService.markAllAsRead,
    onSuccess: refreshNotifications,
  });

  useEffect(() => {
    if (!open) return;

    const updatePosition = () => {
      const trigger = containerRef.current?.getBoundingClientRect();
      if (!trigger) return;

      const width = Math.min(384, window.innerWidth - 24);
      const left = Math.max(12, Math.min(trigger.right - width, window.innerWidth - width - 12));
      const spaceBelow = window.innerHeight - trigger.bottom - 16;
      const top = spaceBelow >= 260
        ? trigger.bottom + 8
        : Math.max(12, trigger.top - Math.min(420, trigger.top - 20));
      setPanelPosition({
        left,
        top,
        maxHeight: Math.max(180, window.innerHeight - top - 16),
      });
    };

    updatePosition();
    window.addEventListener('resize', updatePosition);
    window.addEventListener('scroll', updatePosition, true);
    return () => {
      window.removeEventListener('resize', updatePosition);
      window.removeEventListener('scroll', updatePosition, true);
    };
  }, [open]);

  useEffect(() => {
    const handlePointerDown = (event: MouseEvent) => {
      const target = event.target as Node;
      if (!containerRef.current?.contains(target) && !panelRef.current?.contains(target)) {
        setOpen(false);
      }
    };
    document.addEventListener('mousedown', handlePointerDown);
    return () => document.removeEventListener('mousedown', handlePointerDown);
  }, []);

  if (!isAuthenticated) return null;

  const openNotification = (notification: NotificationItem) => {
    if (!notification.isRead) markReadMutation.mutate(notification.id);
    setOpen(false);
    if (notification.link) navigate(notification.link);
  };

  const unreadCount = summary?.unreadCount ?? 0;
  const isLight = appearance === 'light';

  return (
    <div className="relative" ref={containerRef}>
      <button
        type="button"
        aria-label={`Thông báo${unreadCount ? `, ${unreadCount} chưa đọc` : ''}`}
        aria-expanded={open}
        onClick={() => setOpen((previous) => !previous)}
        className={`relative flex h-10 w-10 items-center justify-center rounded-xl border transition ${
          isLight
            ? 'border-slate-200 bg-white text-slate-500 hover:border-orange-300 hover:bg-orange-50 hover:text-orange-600'
            : 'border-white/10 bg-white/5 text-muted hover:border-orange-500/30 hover:bg-orange-500/10 hover:text-orange-400'
        }`}
      >
        <Bell size={19} />
        {unreadCount > 0 && (
          <span className="absolute -right-1 -top-1 flex h-[19px] min-w-[19px] items-center justify-center rounded-full bg-orange-500 px-1 text-[10px] font-bold text-white ring-2 ring-surface">
            {unreadCount > 99 ? '99+' : unreadCount}
          </span>
        )}
      </button>

      {open && (
        createPortal(
        <section
          ref={panelRef}
          aria-label="Thông báo của bạn"
          className={`fixed z-[10000] overflow-hidden rounded-2xl border shadow-[0_18px_56px_rgba(0,0,0,0.42)] ${
            isLight ? 'border-slate-200 bg-white text-slate-900' : 'border-slate-700 bg-slate-900 text-white'
          }`}
          style={{
            left: panelPosition?.left ?? 12,
            top: panelPosition?.top ?? 72,
            width: `min(384px, calc(100vw - 24px))`,
            maxHeight: panelPosition?.maxHeight ?? 'calc(100vh - 88px)',
            visibility: panelPosition ? 'visible' : 'hidden',
          }}
        >
          <div className={`flex items-center justify-between border-b px-4 py-3 ${isLight ? 'border-slate-200 bg-slate-50' : 'border-slate-700 bg-slate-800'}`}>
            <div>
              <h2 className="text-sm font-semibold">Thông báo</h2>
              <p className={`mt-0.5 text-xs ${isLight ? 'text-slate-500' : 'text-slate-400'}`}>{unreadCount} chưa đọc</p>
            </div>
            {unreadCount > 0 && (
              <button
                type="button"
                disabled={markAllReadMutation.isPending}
                onClick={() => markAllReadMutation.mutate()}
                className={`inline-flex items-center gap-1.5 rounded-lg px-2 py-1.5 text-xs font-semibold transition disabled:opacity-50 ${
                  isLight ? 'text-orange-700 hover:bg-orange-100' : 'text-orange-300 hover:bg-orange-500/15'
                }`}
              >
                <CheckCheck size={14} />
                Đọc tất cả
              </button>
            )}
          </div>

          <div
            className="overflow-y-auto"
            style={{ maxHeight: Math.max(120, (panelPosition?.maxHeight ?? 480) - 60) }}
          >
            {isPending ? (
              <p className={`px-4 py-8 text-center text-sm ${isLight ? 'text-slate-500' : 'text-slate-300'}`}>Đang tải thông báo...</p>
            ) : isError ? (
              <div className="px-4 py-8 text-center">
                <p className={`text-sm ${isLight ? 'text-red-700' : 'text-red-300'}`}>Không tải được thông báo.</p>
                <button
                  type="button"
                  onClick={() => void refreshNotifications()}
                  className={`mt-2 text-xs font-semibold ${isLight ? 'text-orange-700 hover:text-orange-800' : 'text-orange-300 hover:text-orange-200'}`}
                >
                  Thử lại
                </button>
              </div>
            ) : summary?.notifications.length ? (
              summary.notifications.map((notification) => (
                <button
                  key={notification.id}
                  type="button"
                  onClick={() => openNotification(notification)}
                  className={`block w-full border-b px-4 py-3 text-left transition ${
                    isLight
                      ? `border-slate-200 hover:bg-slate-50 ${notification.isRead ? 'bg-white' : 'bg-orange-50'}`
                      : `border-slate-700 hover:bg-slate-800 ${notification.isRead ? 'bg-slate-900' : 'bg-orange-950/50'}`
                  }`}
                >
                  <span className="flex items-start gap-2.5">
                    {!notification.isRead && (
                      <span className="mt-1.5 h-2 w-2 shrink-0 rounded-full bg-orange-500" />
                    )}
                    <span className="min-w-0 flex-1">
                      <span className={`block text-sm font-semibold ${isLight ? 'text-slate-900' : 'text-slate-50'}`}>{notification.title}</span>
                      <span className={`mt-1 block text-xs leading-relaxed ${isLight ? 'text-slate-700' : 'text-slate-300'}`}>{notification.message}</span>
                      <time className={`mt-1.5 block text-[11px] ${isLight ? 'text-slate-500' : 'text-slate-400'}`}>
                        {new Date(notification.createdAt).toLocaleString()}
                      </time>
                    </span>
                  </span>
                </button>
              ))
            ) : (
              <p className={`px-4 py-8 text-center text-sm ${isLight ? 'text-slate-500' : 'text-slate-300'}`}>Bạn chưa có thông báo nào.</p>
            )}
          </div>
        </section>,
        document.body
        )
      )}
    </div>
  );
}
