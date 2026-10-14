import { useEffect } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { toast } from 'sonner';
import { ArrowUpRight, Bell, X } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/contexts/AuthContext';
import { tokenStore } from '@/services/api';
import { notificationQueryKey } from '@/services/notification.service';

interface NotificationMessage {
  title: string;
  message: string;
  link?: string | null;
}

function getNotificationHubUrl() {
  const apiUrl = new URL((import.meta.env.VITE_API_URL || '/api').trim(), window.location.origin);
  const apiPath = apiUrl.pathname.replace(/\/api\/?$/, '');
  apiUrl.pathname = `${apiPath}/notificationHub`;
  apiUrl.search = '';
  apiUrl.hash = '';
  return apiUrl.toString();
}

export function NotificationRealtime() {
  const { user, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  useEffect(() => {
    if (!isAuthenticated || !user?.id) return;

    let disposed = false;
    let retryTimer: ReturnType<typeof setTimeout> | undefined;
    let retryDelayMs = 1000;
    let startInProgress = false;
    const connection = new HubConnectionBuilder()
      .withUrl(getNotificationHubUrl(), {
        accessTokenFactory: () => tokenStore.get() ?? '',
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .configureLogging(LogLevel.Error)
      .build();

    connection.on('ReceiveNotification', (notification: NotificationMessage) => {
      void queryClient.invalidateQueries({ queryKey: notificationQueryKey(user.id) });
      const link = notification.link;
      toast.custom((toastId) => (
        <article
          role="status"
          style={{
            width: 'min(400px, calc(100vw - 32px))',
            overflow: 'hidden',
            border: '1px solid #475569',
            borderRadius: 16,
            background: '#111827',
            color: '#f8fafc',
            boxShadow: '0 18px 48px rgba(0, 0, 0, 0.48)',
          }}
        >
          <div style={{ display: 'flex', gap: 12, padding: 16 }}>
            <span
              aria-hidden="true"
              style={{
                display: 'grid',
                width: 38,
                height: 38,
                flex: '0 0 38px',
                placeItems: 'center',
                borderRadius: 12,
                background: '#9a3412',
                color: '#fff7ed',
              }}
            >
              <Bell size={19} />
            </span>
            <div style={{ minWidth: 0, flex: 1 }}>
              <div style={{ display: 'flex', alignItems: 'flex-start', gap: 8 }}>
                <h3 style={{ flex: 1, margin: 0, color: '#fff', fontSize: 14, fontWeight: 700, lineHeight: 1.45 }}>
                  {notification.title}
                </h3>
                <button
                  type="button"
                  aria-label="Đóng thông báo"
                  onClick={() => toast.dismiss(toastId)}
                  style={{
                    display: 'grid',
                    width: 26,
                    height: 26,
                    flex: '0 0 26px',
                    placeItems: 'center',
                    border: 0,
                    borderRadius: 7,
                    background: 'transparent',
                    color: '#cbd5e1',
                    cursor: 'pointer',
                  }}
                >
                  <X size={16} />
                </button>
              </div>
              <p style={{ margin: '5px 0 0', color: '#cbd5e1', fontSize: 13, lineHeight: 1.55, overflowWrap: 'anywhere' }}>
                {notification.message}
              </p>
              {link && (
                <button
                  type="button"
                  onClick={() => {
                    toast.dismiss(toastId);
                    navigate(link);
                  }}
                  style={{
                    display: 'inline-flex',
                    alignItems: 'center',
                    gap: 5,
                    marginTop: 10,
                    padding: '6px 10px',
                    border: '1px solid #fb923c',
                    borderRadius: 8,
                    background: '#c2410c',
                    color: '#fff',
                    fontSize: 12,
                    fontWeight: 700,
                    cursor: 'pointer',
                  }}
                >
                  Xem thông báo <ArrowUpRight size={14} />
                </button>
              )}
            </div>
          </div>
          <div style={{ height: 3, background: '#c2410c' }} />
        </article>
      ), {
        duration: 8000,
        position: 'top-right',
      });
    });

    const scheduleRetry = () => {
      if (disposed || retryTimer) return;
      retryTimer = setTimeout(() => {
        retryTimer = undefined;
        void startConnection();
      }, retryDelayMs);
      retryDelayMs = Math.min(retryDelayMs * 2, 30_000);
    };

    const startConnection = async () => {
      if (disposed || startInProgress || connection.state !== 'Disconnected') return;
      startInProgress = true;
      try {
        await connection.start();
        retryDelayMs = 1000;
      } catch (error) {
        console.error('Could not connect to the notification service:', error);
        scheduleRetry();
      } finally {
        startInProgress = false;
      }
    };

    connection.onclose((error) => {
      if (error) console.error('Notification connection closed:', error);
      scheduleRetry();
    });

    void startConnection();

    return () => {
      disposed = true;
      if (retryTimer) clearTimeout(retryTimer);
      connection.stop().catch((error: unknown) => {
        console.error('Could not stop the notification connection:', error);
      });
    };
  }, [isAuthenticated, navigate, queryClient, user?.id]);

  return null;
}
