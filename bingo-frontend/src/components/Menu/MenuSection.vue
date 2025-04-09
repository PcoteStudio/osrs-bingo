<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { faChevronDown, faChevronRight } from '@fortawesome/free-solid-svg-icons';
import MenuItem from './MenuItem.vue';
import SubMenuItem from './SubMenuItem.vue';
import type { MenuSectionType } from '@/types/MenuTypes.ts';

const props = defineProps<{
  section: MenuSectionType;
  sectionIndex: number;
}>();

const emit = defineEmits<{
  (e: 'toggleSection', index: number): void;
  (e: 'toggleSubmenu', sectionIndex: number, itemIndex: number): void;
  (e: 'action', action: string): void;
}>();

const handleToggleSection = () => {
  emit('toggleSection', props.sectionIndex);
};

const handleToggleSubmenu = (itemIndex: number) => {
  emit('toggleSubmenu', props.sectionIndex, itemIndex);
};

const handleAction = (action: string) => {
  emit('action', action);
};
</script>

<template>
  <div class="menu-section">
    <div
      class="menu-title"
      @click="handleToggleSection"
      style="cursor: pointer;"
    >
      <span class="menu-text">{{ props.section.title }}</span>
      <FontAwesomeIcon
        :icon="props.section.expanded ? faChevronDown : faChevronRight"
        class="section-indicator"
      />
    </div>

    <ul class="menu-list" v-show="props.section.expanded">
      <li v-for="(item, itemIndex) in props.section.items" :key="item.id" class="menu-item">
        <MenuItem
          v-if="!item.isSubmenu"
          :item="item"
          @action="handleAction"
        />
        <SubMenuItem
          v-else
          :item="item"
          @toggle="handleToggleSubmenu(itemIndex)"
          @action="handleAction"
        />
      </li>
    </ul>
  </div>
</template>

<style scoped>
.menu-section {
  margin-bottom: 20px;
}

.menu-title {
  padding: 12px 16px;
  font-size: 1.2em;
  font-weight: bold;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.section-indicator {
  display: inline-block;
}

.menu-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.menu-item {
  padding: 0;
  margin: 0;
}
</style>
