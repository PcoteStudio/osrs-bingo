import { defineStore } from 'pinia';
import axios from 'axios';
import type { Notification } from '@/types/NotificationType.ts';
import type { User } from '@/types/UserType.ts';

export const useGlobalStore = defineStore('globalStore', {
  state: () => {
    const notifications: Notification[] = [];

    const loginModalState = {
      showModal: false,
    };

    const signupModalState = {
      showModal: true,
    };

    const user: User = {
      isAuthenticated: false,
      authToken: null,
    };

    return {
      notifications: notifications,
      loginModalState: loginModalState,
      signupModalState: signupModalState,
      user: user,
    };
  },
  getters: {
    getLoginModalState: (state) => {
      return state.loginModalState;
    },
    getSignupModalState: (state) => {
      return state.signupModalState;
    },
    getCurrentUser: (state) => {
      return state.user;
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
    async authenticate(email: string, password: string) {
      try {
        const response = await axios.post("http://localhost:5257/api/auth/login",
          {
            username: email,
            password: password,
          },
          {
            headers: {
              'Content-Type': 'application/json'
            }
          });

        const { accessToken } = response.data;
        this.user.authToken = accessToken;

        await this.fetchCurrentUserInformation();

        this.addNotification({
          message: 'Authenticated as ' + this.user.email,
          logLevel: 'success'
        });

        this.toggleLoginModal();
        this.user.isAuthenticated = true;
        return true;
      } catch (error) {
        this.user.isAuthenticated = false;
        this.user.authToken = null;

        let message = "";
        if (axios.isAxiosError(error)) {
          message = error.response?.data?.message || 'Authentication failed';
        } else {
          message = 'An unexpected error occurred';
        }
        this.addNotification({
          message: message,
          logLevel: 'error',
          life: -1
        });
      }
    },
    async disconnect() {
      try {
        const response = await axios.post("http://localhost:5257/api/auth/revoke",
          {},
          {
            headers: {
              'Content-Type': 'application/json',
              'Authorization':  `Bearer ${this.user.authToken}`
            }
          });

        this.user = {
          isAuthenticated: false,
          authToken: null,
          name: undefined,
          email: undefined,
        };

        this.addNotification({
          message: 'Disconnected',
          logLevel: 'success'
        });
      } catch (error) {
        let message = "";
        if (axios.isAxiosError(error)) {
          message = error.response?.data?.message || 'Disconnecting failed';
        } else {
          message = 'An unexpected error occurred';
        }
        this.addNotification({
          message: message,
          logLevel: 'error',
          life: -1
        });
      }
    },
    async fetchCurrentUserInformation() {
      try {
        const response = await axios.post("http://localhost:5257/api/users/me",
          {},
          {
            headers: {
              'Content-Type': 'application/json',
              'Authorization':  `Bearer ${this.user.authToken}`
            }
          });
        const { name, email } = response.data;

        this.user.email = email;
        this.user.name = name;
      } catch (error) {
        let message = "";
        if (axios.isAxiosError(error)) {
          message = error.response?.data?.message || 'Fetching user information failed';
        } else {
          message = 'An unexpected error occurred';
        }
        this.addNotification({
          message: message,
          logLevel: 'error',
          life: -1
        });
      }
    },
    toggleLoginModal() {
      if (this.user.isAuthenticated) {
        console.log("User already connected");
        return;
      }
      this.loginModalState.showModal = !this.loginModalState.showModal;
    },
    toggleSignupModal() {
      this.signupModalState.showModal = !this.signupModalState.showModal;
    },
    async createUser(email: string, password: string, username: string) {
      try {
        const response = await axios.post("http://localhost:5257/api/auth/signup",
          {
            name: username,
            email: email,
            password: password,
          },
          {
            headers: {
              'Content-Type': 'application/json'
            }
          });

        const newEmail = response.data.email;

        this.addNotification({
          message: 'Account created with email : ' + newEmail,
          logLevel: 'success'
        });

        this.toggleSignupModal();
        return true;
      } catch (error) {
        let message = "";
        if (axios.isAxiosError(error)) {
          message = error.response?.data?.message || 'Creating user failed';
        } else {
          message = 'An unexpected error occurred';
        }
        this.addNotification({
          message: message,
          logLevel: 'error',
          life: -1
        });
      }
    },
  }
});
