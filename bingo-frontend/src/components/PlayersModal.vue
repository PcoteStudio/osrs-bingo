<script setup lang="ts">

import { useGlobalStore } from '@/stores/globalStore.ts';
import { computed, ref, watch } from 'vue';

const globalStore = useGlobalStore();
const showModal = computed(() => globalStore.getPlayersState.showModal);
const players = computed(() => globalStore.getPlayersState.players);

const filterValue = ref('');

watch(showModal, () => {
  globalStore.fetchPlayers();
});

const rowData = ref([
  { make: "Tesla", model: "Model Y", price: 64950, electric: true },
  { make: "Ford", model: "F-Series", price: 33850, electric: false },
  { make: "Toyota", model: "Corolla", price: 29600, electric: false },
]);

// Column Definitions: Defines the columns to be displayed.
const colDefs = ref([
  { field: "make" },
  { field: "model" },
  { field: "price" },
  { field: "electric" }
]);



</script>

<template>
  <Dialog
    modal
    v-model:visible="globalStore.playersState.showModal"
    :style="{ width: '50rem' }"
  >
    <template #header>
      <span class="title">
        Players {{ players.length }}
      </span>
      <div class="search-bar">
        <IconField>
          <InputIcon>
            <i class="pi pi-search" />
          </InputIcon>
          <InputText v-model="filterValue" placeholder="Search" />
        </IconField>
        <Button @click="console.log('add player')"
                icon="fas fa-plus"
                size="small"
                severity="success"
                variant="outlined"
                label="Add Player"
                rounded
        />
      </div>
    </template>
    <div class="content">
<!--      <div v-for="player in players">-->
<!--        {{ player.name }}-->
<!--        <div v-for="team in player.teams">-->
<!--          {{ team }}-->
<!--        </div>-->
      <ag-grid-vue
        style="height: 500px; width: 100%;"
        :columnDefs="colDefs"
        :rowData="rowData">
      </ag-grid-vue>
    </div>
  </Dialog>
</template>

<style scoped>
.content {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  max-height: 35rem;
}

.search-bar {
  display: flex;
  gap: 1em;
  align-items: center;
}

.title {
  font-weight: 600;
  font-size: 1.25rem;
}
</style>
