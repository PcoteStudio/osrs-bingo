<script setup lang="ts">

import { useGlobalStore } from '@/stores/globalStore.ts';
import { nextTick, onMounted, ref } from 'vue';
import { FilterMatchMode } from '@primevue/core/api';
import type { PlayerResponse } from '@/clients/responses/playerResponse.ts';
import { useGetAllPlayers } from '@/queries/playerQueries.ts';

const globalStore = useGlobalStore();
const playersState = ref(globalStore.getPlayersState);
const { players } = useGetAllPlayers();

const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS }
});

const editingRows = ref([]);
const onRowEditSave = (event) => {
  const { newData, index } = event;
  console.log('Save player', newData, index);
};

const removePlayer = (data: PlayerResponse) => {
  console.log('remove', data);
};

const removeTeam = (playerId: number, teamId: number) => {
  console.log('Remove team ' + teamId + ' from player ' + playerId);
};

const addPlayer = () => {
  console.log('Add new player');
  globalStore.addPlayer({});

  nextTick(() => {
    if (playersState.value.players.length > 0) {
      editingRows.value = [playersState.value.players[0]];
    }
  });
};
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
          <InputText v-model="filters['global'].value" placeholder="Search" />
        </IconField>
        <Button @click="addPlayer()"
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
      <DataTable :value="playersState.players"
                 class="datatable"
                 scrollable
                 scrollHeight="35rem"
                 dataKey="id"
                 v-model:filters="filters"
                 :globalFilterFields="['id', 'name']"
                 v-model:editingRows="editingRows"
                 editMode="row"
                 @row-edit-save="onRowEditSave"
      >
        <template #empty> No players found. </template>
        <template #loading> Loading players data. Please wait. </template>
        <Column field="id" sortable header="Id" style="width: 10%"></Column>
        <Column field="name" sortable header="Name" style="width: 30%">
          <template #editor="{ data, field }">
            <InputText v-model="data[field]" />
          </template>
        </Column>
        <Column field="teams" header="Teams" style="width: 40%">
          <template #body="{ data }">
            <div class="teams">
              <div v-for="team in data.teams" :key="team.id">
                <Chip class="py-0 pl-0 pr-4"
                      v-tooltip.top="team.event.name"
                      :label="team.name"
                      :image="team.event.avatar"
                />
              </div>
            </div>
          </template>
          <template #editor="{ data, field }">
            <div class="teams">
              <div v-for="team in data[field]" :key="team.id">
                <Chip class="py-0 pl-0 pr-4"
                      v-tooltip.top="team.event.name"
                      removable
                      :label="team.name"
                      :image="team.event.avatar"
                      @remove="removeTeam(data.id, team.id)"
                />
              </div>
              <Button @click="addTeam()"
                      icon="fas fa-plus"
                      size="small"
                      severity="success"
                      variant="outlined"
                      rounded
              />
            </div>
          </template>
        </Column>
        <Column field="actions" header="Actions" :rowEditor="true" style="width: 15%">
          <template #body="{ editorInitCallback }">
            <div class="actions">
              <Button @click="editorInitCallback"
                      icon="fas fa-pen"
                      size="small"
                      variant="text"
                      severity="secondary"
                      rounded
              />
            </div>
          </template>
          <template #editor="{ editorCancelCallback, editorSaveCallback }">
            <div class="actions">
              <Button @click="editorCancelCallback"
                      icon="fas fa-x"
                      size="small"
                      variant="text"
                      severity="secondary"
                      rounded
              />
              <Button @click="editorSaveCallback"
                      icon="fas fa-check"
                      size="small"
                      variant="text"
                      severity="success"
                      rounded
              />
              <Button @click="removePlayer"
                      icon="fas fa-trash"
                      size="small"
                      variant="text"
                      severity="danger"
                      rounded
              />
            </div>
          </template>
        </Column>
      </DataTable>
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

.teams {
  display: flex;
  gap: 0.2em;
  flex-wrap: wrap;
}

.search-bar {
  display: flex;
  gap: 1em;
  align-items: center;
}

.actions {
  display: flex;
  gap: 0.2em;
  flex-wrap: wrap;
  justify-content: center;
}

.title {
  font-weight: 600;
  font-size: 1.25rem;
}
</style>
