import { defineStore } from 'pinia';

export const useGlobalStore = defineStore('globalStore', {
  state: () => {
    const notifications: Notification[] = [];

    return {
      notifications: notifications
    };
  },
  getters: {
  },
  actions: {
    addNotification(notification: Notification) {
      this.notifications.push(notification);
    },
    clearNotifications() {
      if (this.notifications.length > 0)
        this.notifications = [];
    },
  },
});
