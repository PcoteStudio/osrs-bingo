import { defineStore } from 'pinia';
import type { Notification } from '@/types/NotificationType.ts';

export const useNotificationStore = defineStore('notificationStore', {
  state: () => {
    const notifications: Notification[] = [];

    return {
      notifications: notifications,
    };
  },
  getters: {},
  actions: {
    addNotification(notification: Notification) {
      this.notifications.push(notification);
    },
    clearNotifications() {
      if (this.notifications.length > 0)
        this.notifications = [];
    },
  }
});
