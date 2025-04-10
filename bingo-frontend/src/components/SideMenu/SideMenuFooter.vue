<script setup lang="ts">
import { computed } from 'vue';
import { useGlobalStore } from '@/stores/globalStore.ts'

const store = useGlobalStore();

const currentUser = computed(() => store.getCurrentUser);

const initials = computed(() => {
  if (!currentUser?.value.name) return '?';

  const nameParts = currentUser?.value.name.trim().split(' ').filter(Boolean);
  if (nameParts.length === 0) return '?';

  if (nameParts.length === 1) {
    const name = nameParts[0];
    return name.length > 1 ? name.substring(0, 2).toUpperCase() : name.toUpperCase();
  }

  return (nameParts[0].charAt(0) + nameParts[nameParts.length - 1].charAt(0)).toUpperCase();
});
</script>

<template>
  <div class="drawer-footer">
    <div v-if="currentUser.isAuthenticated" class="user-profile">
      <div class="avatar">
        <span class="initials">{{ initials }}</span>
      </div>
      <span class="user-name">{{ currentUser.name }}</span>
      <Button
        @click="store.disconnect"
        icon="fa-solid fa-right-from-bracket"
        type="button"
        variant="outlined"
        rounded
        size="small"
        class="ml-auto"
      />
    </div>
    <div v-else>
      <Button
        @click="store.toggleLoginModal"
        icon="fa-solid fa-right-to-bracket"
        type="button"
        variant="outlined"
        rounded
        size="small"
        class="float-right"
      />
    </div>
  </div>
</template>

<style scoped>
.drawer-footer {
  height: 72px;
  padding: 16px;
  margin-top: auto;
  display: flex;
  flex-direction: column;
}

.user-profile {
  font-size: 1em;
  color: yellow;
  display: flex;
  align-items: center;
  text-decoration: none;
  border-radius: 4px;
}

.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background-color: #222;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 12px;
  position: relative;
  overflow: hidden;
}

.initials {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.user-name {
  font-weight: bold;
  max-width: 50%;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
