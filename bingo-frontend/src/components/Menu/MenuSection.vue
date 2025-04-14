<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { faChevronDown, faChevronRight } from '@fortawesome/free-solid-svg-icons';
import MenuItem from './MenuItem.vue';
import SubMenuItem from './SubMenuItem.vue';
import type { MenuSectionType } from '@/types/MenuTypes.ts';
import { computed } from 'vue'

const props = defineProps<{
  section: MenuSectionType;
  sectionIndex: number;
}>();

const emit = defineEmits<{
  (e: 'toggleSection', index: number): void;
  (e: 'toggleSubmenu', sectionIndex: number, itemIndex: number): void;
}>();

const show = computed(() => {
  return props.section.expanded || props.section.hideSection;
});

const handleToggleSection = () => {
  emit('toggleSection', props.sectionIndex);
};

const handleToggleSubmenu = (itemIndex: number) => {
  emit('toggleSubmenu', props.sectionIndex, itemIndex);
};
</script>

<template>
  <div class="menu-section" :class="[props.section.bottom ? 'mt-auto' : '']">
    <div
      class="menu-title"
      @click="handleToggleSection"
      style="cursor: pointer;"
      v-show="!props.section.hideSection"
    >
      <span class="menu-text">{{ props.section.title }}</span>
      <FontAwesomeIcon
        :icon="props.section.expanded ? faChevronDown : faChevronRight"
        class="section-indicator"
      />
    </div>

    <ul class="menu-list" v-show="show">
      <li v-for="(item, itemIndex) in props.section.items" :key="item.id" class="menu-item">
        <MenuItem
          v-if="!item.isSubmenu"
          :item="item"
          @action="item.action"
        />
        <SubMenuItem
          v-else
          :item="item"
          @toggle="handleToggleSubmenu(itemIndex)"
          @action="item.action"
        />
      </li>
    </ul>
  </div>
</template>

<style scoped>
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
