<script setup lang="ts">
import { ref } from 'vue';
import type { MenuSectionType } from '@/types/MenuTypes.ts';
import MenuSection from '@/components/Menu/MenuSection.vue';
import { useGlobalStore } from '@/stores/globalStore.ts';

const globalState = useGlobalStore();

const menuSections = ref<MenuSectionType[]>([
  {
    id: 'events',
    title: 'Events',
    expanded: true,
    items: [
      {
        id: 'bingo',
        label: 'Bingo',
        action: () => console.log('openBingo'),
        active: true,
        isSubmenu: true,
        expanded: true,
        items: [
          {
            id: 'bingo-board',
            label: 'Board',
            icon: 'faChessBoard',
            action: () => console.log('openBingoBoard'),
          },
          {
            id: 'bingo-leaderboard',
            label: 'Leaderboard',
            icon: 'faTrophy',
            action: () => console.log('openBingoLeaderboard'),
          },
          {
            id: 'bingo-teams',
            label: 'Teams',
            icon: 'faUsers',
            action: () => console.log('openBingoTeams'),
          }
        ]
      },
      {
        id: 'dashboard',
        label: 'Snake and Ladder',
        action: () => console.log('openSnake'),
      },
      {
        id: 'game-of-life',
        label: 'Game of Life',
        action: () => console.log('openGOL'),
      },
    ]
  },
  {
    id: 'more',
    title: 'More',
    hideSection: true,
    bottom: true,
    items: [
      {
        id: 'players',
        label: 'Players',
        icon: 'faUsers',
        action: () => console.log('openPlayers'),
      },
      {
        id: 'settings',
        label: 'Settings',
        icon: 'faCog',
        action: globalState.toggleSettingsModal,
      },
    ]
  }
]);

const toggleSection = (sectionIndex: number): void => {
  menuSections.value[sectionIndex].expanded = !menuSections.value[sectionIndex].expanded;
};

const toggleSubmenu = (sectionIndex: number, itemIndex: number): void => {
  const item = menuSections.value[sectionIndex].items[itemIndex];
  if (item.isSubmenu) {
    item.expanded = !item.expanded;
  }
};
</script>

<template>
  <div class="drawer-body">
    <MenuSection
      v-for="(section, sectionIndex) in menuSections"
      :key="section.id"
      :section="section"
      :section-index="sectionIndex"
      @toggle-section="toggleSection"
      @toggle-submenu="toggleSubmenu"
    />
  </div>
</template>

<style scoped>
.drawer-body {
  display: flex;
  flex-direction: column;
  flex: 1;
  overflow-y: auto;
}
</style>
