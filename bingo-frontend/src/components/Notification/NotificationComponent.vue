<script setup lang="ts">

import { watch } from 'vue';
import { useToast } from 'primevue/usetoast';
import { useNotificationStore } from '@/stores/notificationStore.ts';

const toast = useToast();
const notificationStore = useNotificationStore();

watch(() => notificationStore.notifications,
  (notifications) => {
    for (const notification of notifications) {
      let life;
      if (notification.life === undefined) {
        life = 4000;
      }
      else if (notification.life === -1) {
        life = undefined;
      }
      else {
        life = notification.life;
      }

      toast.add({
        severity: notification.logLevel,
        summary: notification.message,
        life: life
      });
    }
    notificationStore.clearNotifications();
  }, { deep: true }
);

notificationStore.$onAction(
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
