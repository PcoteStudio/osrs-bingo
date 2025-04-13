<script setup lang="ts">
import { useNotificationStore } from '@/stores/notificationStore.ts';
import { ref } from 'vue';

const notificationStore = useNotificationStore();

const counter = ref(0);
const add = () => {
  notificationStore.addNotification({
    key: 'test',
    logLevel: 'info',
    message: 'Test notification' + counter.value++,
    life: -1
  });
};

</script>

<template>
  <div class="board">
    <Button @click="add">Add notification {{ counter }}</Button>
<!--    <img src="https://cdn.discordapp.com/attachments/1356383638698328065/1359015142318145661/H8lCf7D.png?ex=67f93cbb&is=67f7eb3b&hm=faa938adbc4030ad57f7d3c9ce9ae59286b318d5c10da727b4246d81017f1b2a" />-->
  </div>
</template>

<style scoped>
.board {
  width: 100%;
  height: 100%;
  overflow-y: auto;
  overflow-x: hidden;

  img {
    width: 100%;
    height: auto;
    image-rendering: optimizeQuality;
  }
}
</style>
