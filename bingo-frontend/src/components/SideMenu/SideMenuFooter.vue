<script setup lang="ts">
import { computed } from 'vue';
import md5 from 'md5';

const props = defineProps({
  email: {
    type: String,
    required: true
  },
  username: {
    type: String,
    required: true
  },
});

const initials = computed(() => {
  if (!props.username) return '?';

  const nameParts = props.username.trim().split(' ').filter(Boolean);
  if (nameParts.length === 0) return '?';

  if (nameParts.length === 1) {
    const name = nameParts[0];
    return name.length > 1 ? name.substring(0, 2).toUpperCase() : name.toUpperCase();
  }

  return (nameParts[0].charAt(0) + nameParts[nameParts.length - 1].charAt(0)).toUpperCase();
});


const gravatarUrl = computed(() => {
  const hash = md5(props.email.trim().toLowerCase());
  return `https://www.gravatar.com/avatar/${hash}`;
});

function handleImageError(event: Event) {
  const target = event.target as HTMLImageElement;

  target.style.display = 'none';
  (target.nextElementSibling as HTMLElement).style.display = 'flex';
}
</script>

<template>
  <div class="drawer-footer">
    <a href="#" class="user-profile">
      <div class="avatar">
        <img
          :src="gravatarUrl"
          :alt="props.username"
          @error="handleImageError"
          class="gravatar-img"
        />
        <span class="initials" style="display: none;">{{ initials }}</span>
      </div>
      <span class="user-name">{{ props.username }}</span>
    </a>
  </div>
</template>

<style scoped>
.drawer-footer {
  padding: 16px;
  margin-top: auto;
  display: flex;
  flex-direction: column;
}

.user-profile {
  font-size: 1.25em;
  color: yellow;
  display: flex;
  align-items: center;
  text-decoration: none;
  padding: 8px;
  border-radius: 4px;
}

.user-profile:hover {
  background-color: #090909;
}

.avatar {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  background-color: #222;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-right: 12px;
  position: relative;
  overflow: hidden;
}

.gravatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
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
}
</style>
