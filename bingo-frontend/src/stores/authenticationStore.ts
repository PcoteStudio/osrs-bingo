import { defineStore } from 'pinia';
import { AuthenticationService } from '@/services/authenticationService.ts';
import { inject } from 'vue';
import { useCurrentUser } from '@/queries/user.ts';
import type { UserResponse } from '@/clients/responses/userResponse.ts';
import { useNotificationStore } from '@/stores/notificationStore.ts';

export const useAuthenticationStore = defineStore('authenticationStore', {
  state: () => {
    const notificationStore = useNotificationStore();
    const authenticationService = inject(AuthenticationService.injectionKey)!;
    const user: UserResponse | undefined = undefined;

    return {
      notificationStore: notificationStore,
      authenticationService: authenticationService,
      user: user as  UserResponse | undefined,
    };
  },
  getters: {
    getCurrentUser: (state) => {
      return state.user;
    },
  },
  actions: {
    async authenticate(username: string, password: string) {
      if(await this.authenticationService.authenticate(username, password)) {
        await this.getUser();
        this.notificationStore.addNotification({
          logLevel: 'success',
          message: 'Connected : ' + this.user?.username,
          life: 5000
        });
        return true;
      }

      this.notificationStore.addNotification({
        logLevel: 'error',
        message: 'An error occurred while connecting to user : ' + username
      });
      return false;
    },
    async disconnect() {
      if(await this.authenticationService.disconnect()) {
        this.notificationStore.addNotification({
          logLevel: 'info',
          message: 'Disconnected from user : ' + this.user?.username,
          life: 5000
        });
        this.user = undefined;
        return;
      }

      this.notificationStore.addNotification({
        logLevel: 'error',
        message: 'An error occurred while disconnecting from user : ' + this.user?.username
      });
    },
    async getUser() {
      const query = useCurrentUser();
      await query.refresh();
      this.user = query.user.value;
    },
    async createUser(username: string, password: string, email: string,) {
      if(await this.authenticationService.createAccount(username, password, email)) {
        this.notificationStore.addNotification({
          logLevel: 'success',
          message: 'Account created : ' + email,
          life: 5000
        });
        return true;
      }

      this.notificationStore.addNotification({
        logLevel: 'error',
        message: 'An error occurred while creating a new user : ' + email
      });
      return false;
    },
  }
});
