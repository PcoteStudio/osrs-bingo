<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { getIcon } from '@/utils/iconUtils';
import type { MenuItemType } from '@/types/MenuTypes.ts'

const props = defineProps<{
  item: MenuItemType;
}>();

const emit = defineEmits<{
  (e: 'action', action: string): void;
}>();

const handleClick = () => {
  emit('action', props.item.action);
};
</script>

<template>
  <button @click="handleClick" class="menu-button">
    <FontAwesomeIcon v-if="props.item.icon" :icon="getIcon(props.item.icon)" class="menu-icon" />
    <span class="menu-text" :class="[props.item.active ? 'highlight' : '']">{{ props.item.label }}</span>
  </button>
</template>

<style scoped>
.menu-button {
  display: flex;
  align-items: center;
  padding: 12px 16px;
  text-decoration: none;
  transition: background-color 0.2s;
  position: relative;
  width: 100%;
  text-align: left;
  background: none;
  border: none;
  color: inherit;
  font: inherit;
  cursor: pointer;
}

.menu-button:hover {
  background-color: #090909;
}

.menu-icon {
  font-size: 18px;
  flex-shrink: 0;
  width: 20px;
  text-align: center;
  margin-right: 12px;
}
</style>
