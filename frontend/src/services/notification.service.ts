import { api } from './api';
import type { ApiResult, NotificationItem, NotificationSummary } from '@/types';

export type { NotificationItem, NotificationSummary };

export const notificationQueryKey = (userId: string) => ['notifications', userId] as const;

export const notificationService = {
  getNotifications: async (): Promise<ApiResult<NotificationSummary>> => {
    const { data } = await api.get<ApiResult<NotificationSummary>>('/notification');
    return data;
  },

  markAsRead: async (notificationId: string): Promise<ApiResult<NotificationItem>> => {
    const { data } = await api.put<ApiResult<NotificationItem>>(`/notification/${notificationId}/read`);
    return data;
  },

  markAllAsRead: async (): Promise<ApiResult<number>> => {
    const { data } = await api.put<ApiResult<number>>('/notification/mark-all-read');
    return data;
  },
};
