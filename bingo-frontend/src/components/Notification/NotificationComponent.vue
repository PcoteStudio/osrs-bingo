<script setup lang="ts">

import { watch } from 'vue'
import { useToast } from 'primevue/usetoast'
import { useGlobalStore } from '@/stores/globalStore'

const toast = useToast();
const state = useGlobalStore();

watch(() => state.notifications,
  (notifications) => {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    for (const notification of notifications) {
      const details = getMessageDetails();

      toast.add({
        severity: details.logLevel,
        detail: details.message,
        life: 1500
      });
    }
    state.clearNotifications();
  }, { deep: true }
);

state.$onAction(
  ({ onError }) => {
    onError((error) => {
      console.error(error);
      const details = getErrorMessageDetails(error);

      toast.add({
        severity: details.logLevel,
        detail: details.message,
      });
    });
  }
);

function getMessageDetails(error?: unknown) {
  return { message: error, logLevel: 'info' };
}

function getErrorMessageDetails(error: unknown) {
  const details = { message: '', logLevel: 'error' };

  details.message = 'An error occurred: ' + error;

  return details;
}

</script>

<template>
  <Toast />
</template>

<style scoped>

</style>
