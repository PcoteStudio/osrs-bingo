<script setup lang="ts">
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { faChevronDown, faChevronRight } from '@fortawesome/free-solid-svg-icons';
import { getIcon } from '@/utils/iconUtils';

const props = defineProps<{
  item: {
    id: string;
    label: string;
    icon: string;
    action: string;
    expanded?: boolean;
    items?: Array<{
      id: string;
      label: string;
      icon: string;
      action: string;
    }>;
  };
}>();

const emit = defineEmits<{
  (e: 'toggle'): void;
  (e: 'action', action: string): void;
}>();

const handleToggle = () => {
  emit('toggle');
};

const handleAction = (action: string) => {
  emit('action', action);
};
</script>

<template>
  <div>
    <button
      class="menu-button submenu-button"
      @click="handleToggle"
    >
      <FontAwesomeIcon :icon="getIcon(props.item.icon)" class="menu-icon" />
      <span class="menu-text">{{ props.item.label }}</span>
      <FontAwesomeIcon
        :icon="props.item.expanded ? faChevronDown : faChevronRight"
        class="submenu-indicator"
      />
    </button>

    <!-- Submenu items -->
    <ul class="submenu" v-show="props.item.expanded">
      <li v-for="subItem in props.item.items" :key="subItem.id" class="menu-item">
        <button @click="handleAction(subItem.action)" class="menu-button">
          <FontAwesomeIcon :icon="getIcon(subItem.icon)" class="menu-icon" />
          <span class="menu-text">{{ subItem.label }}</span>
        </button>
      </li>
    </ul>
  </div>
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

.submenu-button {
  justify-content: space-between;
}

.menu-icon {
  font-size: 18px;
  flex-shrink: 0;
  width: 20px;
  text-align: center;
  margin-right: 12px;
}

.submenu-indicator {
  margin-left: auto;
}

.submenu {
  list-style: none;
  padding-left: 20px;
  margin: 0;
}

.submenu .menu-button {
  padding: 10px 16px;
}

.menu-item {
  padding: 0;
  margin: 0;
}
</style>
