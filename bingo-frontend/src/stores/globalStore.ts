import { defineStore } from 'pinia';
import { useAuthenticationStore } from '@/stores/authenticationStore';

export const useGlobalStore = defineStore('globalStore', {
  state: () => {
    const authenticationStore = useAuthenticationStore();

    const loginModalState = {
      showModal: false,
    };

    const signupModalState = {
      showModal: false,
    };

    return {
      authenticationStore: authenticationStore,
      loginModalState: loginModalState,
      signupModalState: signupModalState
    };
  },
  getters: {
    getLoginModalState: (state) => {
      return state.loginModalState;
    },
    getSignupModalState: (state) => {
      return state.signupModalState;
    },
  },
  actions: {
    toggleLoginModal() {
      this.loginModalState.showModal = !this.loginModalState.showModal;
    },
    toggleSignupModal() {
      this.signupModalState.showModal = !this.signupModalState.showModal;
    },
    openSignUpModal() {
      this.loginModalState.showModal = false;
      this.signupModalState.showModal = true;
    }
  }
});
