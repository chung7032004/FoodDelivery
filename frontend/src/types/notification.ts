export interface NotificationItem {
  id: string;
  title: string;
  message: string;
  type: string;
  isRead: boolean;
  createdAt: string;
  link: string | null;
}

export interface NotificationSummary {
  unreadCount: number;
  notifications: NotificationItem[];
}

export interface NotificationMessage {
  title: string;
  message: string;
  link?: string | null;
}

