<script setup lang="ts">
import { ref } from 'vue';
import type { MenuSectionType } from '@/types/MenuTypes.ts'
import MenuSection from '@/components/Menu/MenuSection.vue'

const menuSections = ref<MenuSectionType[]>([
  {
    id: 'favorites',
    title: 'FAVORITES',
    expanded: true,
    items: [
      {
        id: 'dashboard',
        label: 'Dashboard',
        icon: 'faHome',
        action: 'openDashboard'
      },
      {
        id: 'bookmarks',
        label: 'Bookmarks',
        icon: 'faBookmark',
        action: 'openBookmarks'
      },
      {
        id: 'reports',
        label: 'Reports',
        icon: 'faChartLine',
        action: 'toggleReports',
        isSubmenu: true,
        expanded: false,
        items: [
          {
            id: 'revenue',
            label: 'Revenue',
            icon: 'faChartPie',
            action: 'openRevenue'
          },
          {
            id: 'expenses',
            label: 'Expenses',
            icon: 'faChartBar',
            action: 'openExpenses'
          }
        ]
      },
      {
        id: 'team',
        label: 'Team',
        icon: 'faUsers',
        action: 'openTeam'
      },
      {
        id: 'messages',
        label: 'Messages',
        icon: 'faComments',
        action: 'openMessages'
      },
      {
        id: 'calendar',
        label: 'Calendar',
        icon: 'faCalendar',
        action: 'openCalendar'
      },
      {
        id: 'settings',
        label: 'Settings',
        icon: 'faCog',
        action: 'openSettings'
      }
    ]
  },
  {
    id: 'application',
    title: 'APPLICATION',
    expanded: true,
    items: [
      {
        id: 'projects',
        label: 'Projects',
        icon: 'faFolder',
        action: 'openProjects'
      },
      {
        id: 'performance',
        label: 'Performance',
        icon: 'faChartBar',
        action: 'openPerformance'
      },
      {
        id: 'appSettings',
        label: 'Settings',
        icon: 'faCog',
        action: 'openAppSettings'
      }
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
  flex: 1;
  overflow-y: auto;
  padding: 12px 0;
}
</style>
