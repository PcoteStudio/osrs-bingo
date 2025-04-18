import { defineStore } from 'pinia';
import { useAuthenticationStore } from '@/stores/authenticationStore';
import type { PlayerResponse } from '@/clients/responses/playerResponse.ts';
import { inject } from 'vue';
import { HttpClient } from '@/clients/httpClient.ts';
export const useGlobalStore = defineStore('globalStore', {
  state: () => {
    const authenticationStore = useAuthenticationStore();
    const httpClient = inject(HttpClient.injectionKey)!;

    const loginModalState = {
      showModal: false,
    };

    const signupModalState = {
      showModal: false,
    };

    const settingsState = {
      showModal: false,
    };

    const playersState = {
      showModal: false,
      players: [] as PlayerResponse[]
    };

    return {
      httpClient: httpClient,
      authenticationStore: authenticationStore,
      loginModalState: loginModalState,
      signupModalState: signupModalState,
      settingsState: settingsState,
      playersState: playersState,
    };
  },
  getters: {
    getLoginModalState: (state) => {
      return state.loginModalState;
    },
    getSignupModalState: (state) => {
      return state.signupModalState;
    },
    getPlayersState: (state) => {
      return state.playersState;
    },
    getPlayersModalState: (state) => {
      return state.playersState.showModal;
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
    },
    toggleSettingsModal() {
      this.settingsState.showModal = !this.settingsState.showModal;
    },
    togglePlayersModal() {
      this.playersState.showModal = !this.playersState.showModal;
    },
    addPlayer(player: PlayerResponse) {
      this.playersState.players.unshift(player);
      console.log('player added', this.playersState.players.length);
    },
    fetchPlayers() {
      this.httpClient.getPlayers().then(players => this.playersState.players = players);
    }
  }
});
