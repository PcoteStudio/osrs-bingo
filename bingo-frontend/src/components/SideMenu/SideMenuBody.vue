<script setup lang="ts">
import { ref } from 'vue';
import type { MenuSectionType } from '@/types/MenuTypes.ts';
import MenuSection from '@/components/Menu/MenuSection.vue';

const menuSections = ref<MenuSectionType[]>([
  {
    id: 'events',
    title: 'Events',
    expanded: true,
    items: [
      {
        id: 'bingo',
        label: 'Bingo',
        action: 'openBingo',
        active: true,
        isSubmenu: true,
        expanded: true,
        items: [
          {
            id: 'bingo-board',
            label: 'Board',
            icon: 'faChessBoard',
            action: 'openBingoBoard'
          },
          {
            id: 'bingo-leaderboard',
            label: 'Leaderboard',
            icon: 'faTrophy',
            action: 'openBingoLeaderboard'
          },
          {
            id: 'bingo-teams',
            label: 'Teams',
            icon: 'faUsers',
            action: 'openBingoTeams'
          }
        ]
      },
      {
        id: 'dashboard',
        label: 'Snake and Ladder',
        action: 'openSnake'
      },
      {
        id: 'game-of-life',
        label: 'Game of Life',
        action: 'openGOL'
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
        action: 'openPlayers'
      },
      {
        id: 'settings',
        label: 'Settings',
        icon: 'faCog',
        action: 'openSettings'
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

const handleAction = (action: string): void => {
  console.log(`Action triggered: ${action}`);
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
      @action="handleAction"
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
