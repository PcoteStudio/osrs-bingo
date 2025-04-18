<script setup lang="ts">
import { useGlobalStore } from '@/stores/globalStore';
import { inject, ref, watch } from 'vue';
import { useNotificationStore } from '@/stores/notificationStore.ts';
import { applyTheme, getPreferredTheme, ThemeType } from '@/utils/themeUtils.ts';
import { HttpClient } from '@/clients/httpClient.ts';

const globalStore = useGlobalStore();
const notificationStore = useNotificationStore();

const httpClient = inject(HttpClient.injectionKey)!;

const selectedTheme = ref(getPreferredTheme());
const options = ref([
  { name: 'Dark', value: ThemeType.dark },
  { name: 'Light', value: ThemeType.light },
]);

watch(selectedTheme, (newTheme) => {
  applyTheme(newTheme);

  notificationStore.addNotification({
    logLevel: 'success',
    message: `Theme changed to ${newTheme.replace('-theme', '')}`,
    life: 5000
  });
});

const loadingSeedDatabase = ref(false);
const loadingSeedState = ref('secondary');
const loadingSeedMessage = ref('');
const seedDatabase = () => {
  loadingSeedDatabase.value = true;

  httpClient.seedDatabase().then(() => {
    loadingSeedState.value = 'success';
    loadingSeedMessage.value = 'Database seeded successfully 👌';
  }).catch((error) => {
    console.log(error);
    loadingSeedState.value = 'danger';
    loadingSeedMessage.value = 'Something went wrong 😢, blame the backend guy!';
  }).finally(() => {
    loadingSeedDatabase.value = false;
  });
};

const loadingDropDatabase = ref(false);
const loadingDropState = ref('secondary');
const loadingDropMessage = ref('');
const dropDatabase = () => {
  loadingDropDatabase.value = true;

  httpClient.seedDatabase().then(() => {
    loadingDropState.value = 'success';
    loadingDropMessage.value = 'Database deleted successfully 👀';
  }).catch((error) => {
    console.log(error);
    loadingDropState.value = 'danger';
    loadingDropMessage.value = 'Something went wrong 😢, blame the backend guy!';
  }).finally(() => {
    loadingDropDatabase.value = false;
  });
};
</script>

<template>
  <Dialog
    modal
    v-model:visible="globalStore.settingsState.showModal"
    header="Settings"
    :style="{ width: '25rem' }"
  >
    <div class="content">
      <div class="flex flex-col gap-1">
        <label>Theme</label>
        <SelectButton
          v-model="selectedTheme"
          :options="options"
          optionLabel="name"
          optionValue="value"
        />
      </div>
      <div class="flex flex-col gap-1 w-fit">
        <label>Seed Database</label>
        <div class="message-group">
          <Button @click="seedDatabase"
            :disabled="loadingSeedDatabase"
            :icon="loadingSeedDatabase ? 'fas fa-spinner' : ''"
            :severity="loadingSeedState"
            label="Seed"
          />
          <span v-if="loadingSeedMessage">{{ loadingSeedMessage }}</span>
        </div>
      </div>
      <div class="flex flex-col gap-1 w-fit">
        <label>Drop Database</label>
        <div class="message-group">
          <Button @click="dropDatabase"
            :disabled="loadingDropDatabase"
            :icon="loadingDropDatabase ? 'fas fa-spinner' : ''"
            :severity="loadingDropState"
            label="Drop"
          />
          <span v-if="loadingDropMessage">{{ loadingDropMessage }}</span>
        </div>
      </div>
    </div>
  </Dialog>
</template>

<style scoped>
.content {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.message-group {
  display: flex;
  align-items: center;
  gap: 0.5em;
}
</style>
