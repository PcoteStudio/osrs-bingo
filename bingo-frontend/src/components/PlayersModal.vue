<script setup lang="ts">
import { useGlobalStore } from '@/stores/globalStore.ts';
import { computed, ref, watch } from 'vue'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import fuzzySort from 'fuzzysort';
import _ from 'lodash';
import { UseVirtualList } from '@vueuse/components';
import DataTableRow from '@/components/DataTableRow.vue'
import type { PlayerResponse } from '@/clients/responses/playerResponse.ts'
import KeysResult = Fuzzysort.KeysResult;

const globalStore = useGlobalStore();
const showModal = computed(() => globalStore.getPlayersState.showModal);
const players = computed(() => globalStore.getPlayersState.players || []);

const filter = ref();

const fuzzySearchKeys = [
  'id',
  'name'
];

const filteredPlayers = computed(() =>
  [...fuzzySort.go(
    filter.value,
    players.value,
    { keys: fuzzySearchKeys, all: true }
  )]);

const highlight = (key: string, data: KeysResult<PlayerResponse>) => {
  const elements = data[fuzzySearchKeys.indexOf(key)]
    .highlight((m: string, i: number) =>
      `<span class="highlight-match" key=${i}>${m}</span>`);

  if (elements && elements.length > 0) {
    return elements.join('');
  }

  return _.get(data.obj, key);
};

const addPlayer = () => {
  console.log('Add new player');
  // players.value.unshift({});
};

const saveRow = (id: number, data: any) => {
  console.log('Datatable saveEditedRow', id, data);
};

const deleteRow = (id: number) => {
  console.log('Datatable removeRow', id);
};

watch(showModal, () => {
  globalStore.fetchPlayers();
});
</script>

<template>
  <Dialog
    modal
    v-model:visible="globalStore.playersState.showModal"
    :style="{ width: '50rem' }"
  >
    <template #header>
      <span class="title">
        Players ({{ players.length }})
      </span>
      <div class="search-bar">
        <IconField>
          <InputIcon>
            <FontAwesomeIcon icon="fas fa-search" />
          </InputIcon>
          <InputText v-model="filter" placeholder="Search" />
        </IconField>
        <Button @click="addPlayer"
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
      <table>
        <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th>Teams</th>
          <th>Actions</th>
        </tr>
        </thead>
        <tbody>
        <UseVirtualList :list="filteredPlayers" :options="{ itemHeight: 65, overscan: 20 }" height="500px">
          <template #default="{ data }">
            <DataTableRow :data="data"
                          :highlight="highlight"
                          @save="saveRow"
                          @delete="deleteRow"
            />
          </template>
        </UseVirtualList>
        </tbody>
      </table>
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

table {
  border-radius: 12px;
  border: 1px solid #343434;
  background-color: #111111;
  width: 100%;
  display: grid;
  grid-template-rows: auto 1fr;

  th {
    padding: 0.25em 0.75em;
    text-align: left;
  }

  thead {
    background-color: #060606;
    display: grid;
    grid-template-rows: auto;
    border-top-left-radius: 12px;
    border-top-right-radius: 12px;
    padding: 0.5em;
    border-bottom: 1px solid #343434;
  }

  tbody {
    display: grid;
    grid-auto-rows: auto;
    overflow-y: auto;
    border-bottom-left-radius: 12px;
    border-bottom-right-radius: 12px;
    margin-bottom: 0.5em;

    tr {
      border-bottom: 1px solid #343434;
    }
  }

  tr {
    display: grid;
    grid-template-columns: 80px 1fr 1fr 150px;
    align-items: center;
  }

  td div {
    overflow: hidden;
    text-overflow: ellipsis;
  }
}
</style>
