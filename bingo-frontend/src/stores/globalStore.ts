import { defineStore } from 'pinia';

export const useGlobalStore = defineStore('globalStore', {
  state: () => {
    const notifications: Notification[] = [];

    const loginModalState = {
      showModal: true,
    };

    return {
      notifications: notifications,
      loginModalState: loginModalState,
    };
  },
  getters: {
    getLoginModalState: (state) => {
      return state.loginModalState;
    },
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
