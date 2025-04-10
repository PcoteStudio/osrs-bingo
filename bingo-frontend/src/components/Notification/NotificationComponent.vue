<script setup lang="ts">

import { watch } from 'vue'
import { useToast } from 'primevue/usetoast'
import { useGlobalStore } from '@/stores/globalStore'

const toast = useToast();
const state = useGlobalStore();

watch(() => state.notifications,
  (notifications) => {
    for (const notification of notifications) {
      let life;
      if (notification.life === undefined) {
        life = 1500;
      }
      else if (notification.life === -1) {
        life = undefined;
      }
      else {
        life = notification.life;
      }

      toast.add({
        severity: notification.logLevel,
        detail: notification.message,
        life: life
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
