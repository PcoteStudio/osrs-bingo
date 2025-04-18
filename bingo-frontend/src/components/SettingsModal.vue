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
        <Button
          @click="httpClient.seedDatabase()"
          label="Seed"
        />
      </div>
            <div class="flex flex-col gap-1 w-fit">
        <label>Drop Database</label>
        <Button
          @click="httpClient.dropDatabase()"
          label="Drop"
        />
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
</style>
