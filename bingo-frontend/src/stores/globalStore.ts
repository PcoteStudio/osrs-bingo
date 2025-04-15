import { defineStore } from 'pinia';
import { useAuthenticationStore } from '@/stores/authenticationStore';
import type { PlayerResponse } from '@/clients/responses/playerResponse.ts';

export const useGlobalStore = defineStore('globalStore', {
  state: () => {
    const authenticationStore = useAuthenticationStore();

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
    fetchPlayers() {
      this.playersState.players = [
        {
          id: 1,
          name: "Michael Johnson",
          teams: [
            {
              id: 1,
              name: "Barboteuse",
              avatar: "",
              event: {
                id: 1,
                name: "Bingo",
                avatar: ""
              }
            },
            {
              id: 2,
              name: "Wololo",
              avatar: "",
              event: {
                id: 1,
                name: "Bingo",
                avatar: ""
              }
            },
            {
              id: 3,
              name: "Team 3",
              avatar: "",
              event: {
                id: 1,
                name: "Bingo",
                avatar: ""
              }
            },
            {
              id: 4,
              name: "Team 4",
              avatar: "",
              event: {
                id: 1,
                name: "Bingo",
                avatar: ""
              }
            }
          ]
        },
        {
          id: 2,
          name: "Sarah Wilson"
        },
        {
          id: 3,
          name: "David Thompson"
        },
        {
          id: 4,
          name: "Emma Rodriguez"
        },
        {
          id: 5,
          name: "James Davis"
        },
        {
          id: 6,
          name: "Olivia Martinez"
        },
        {
          id: 7,
          name: "Daniel Brown"
        },
        {
          id: 8,
          name: "Sophia Clark"
        },
        {
          id: 9,
          name: "Ethan Lewis"
        },
        {
          id: 10,
          name: "Isabella Walker"
        },
        {
          id: 11,
          name: "Alexander Turner"
        },
        {
          id: 12,
          name: "Mia Phillips"
        },
        {
          id: 13,
          name: "William King"
        },
        {
          id: 14,
          name: "Charlotte Wright"
        },
        {
          id: 15,
          name: "Benjamin Harris"
        },
        {
          id: 16,
          name: "Amelia Young"
        },
        {
          id: 17,
          name: "Lucas Scott"
        },
        {
          id: 18,
          name: "Harper Green"
        },
        {
          id: 19,
          name: "Matthew Allen"
        },
        {
          id: 20,
          name: "Evelyn Baker"
        }
      ];
    },
    addPlayer(player: PlayerResponse) {
      this.playersState.players.unshift(player);
      console.log('player added', this.playersState.players.length);
    }
  }
});
