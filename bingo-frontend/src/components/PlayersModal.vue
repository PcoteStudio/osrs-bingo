<script setup lang="ts">
import { useGlobalStore } from '@/stores/globalStore.ts';
import { computed, ref, watch } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import fuzzySort from 'fuzzysort';
import _ from 'lodash';

const globalStore = useGlobalStore();
const showModal = computed(() => globalStore.getPlayersState.showModal);
const players = computed(() => globalStore.getPlayersState.players);


const filter = ref();
const fuzzySearchKeys = [
  'id',
  'name'
];
const filteredPlayers = computed(() =>
  fuzzySort.go(
    filter.value,
    players.value,
    { keys: fuzzySearchKeys, all: true }
  ));

const highlight = (key: string, id?: number) => {
  if (!id)
    return;

  const results = filteredPlayers.value.find(r => r.obj.id === id);
  if (!results)
    return;

  const elements = results[fuzzySearchKeys.indexOf(key)]
    .highlight((m: string, i: number) =>
      `<span class="highlight-match" key=${i}>${m}</span>`);

  if (elements && elements.length > 0) {
    return elements.join('');
  }

  return _.get(results.obj, key);
};

const editRow = (value) => {
  console.log('editRow', value);
};

const cancelEditRow = (value) => {
  console.log('cancelEditRow', value);
};

const saveEditedRow = (value) => {
  console.log('saveEditedRow', value);
};

const removeRow = (value) => {
  console.log('removeRow', value);
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
        Players {{ players.length }}
      </span>
      <div class="search-bar">
        <IconField>
          <InputIcon>
            <FontAwesomeIcon icon="fas fa-search" />
          </InputIcon>
          <InputText v-model="filter" placeholder="Search" />
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
        <tr v-for="player in filteredPlayers" :key="player.obj.id">
          <td><span v-html="highlight('id', player.obj.id)" /></td>
          <td><span v-html="highlight('name', player.obj.id)" /></td>
          <td>
            <div v-for="team in player.obj.teams" :key="team.id">
              <div class="team-badge">
                  <span class="team-icon">
                    {{ team.id }}
                  </span>
                <span class="team-name">
                    {{ team.name }}
                  </span>
                <button class="team-remove-button">
                  <FontAwesomeIcon icon="fas fa-x"/>
                </button>
              </div>
            </div>
            <div class="team-badge">
              <button class="team-add-button">
                <FontAwesomeIcon icon="fas fa-plus"/>
              </button>
            </div>
          </td>
          <td>
            <div class="actions">
              <Button @click="editRow(player.obj.id)"
                      icon="fas fa-edit"
                      size="small"
                      variant="text"
                      severity="primary"
                      rounded
              />
              <Button @click="cancelEditRow(player.obj.id)"
                      icon="fas fa-x"
                      size="small"
                      variant="text"
                      severity="secondary"
                      rounded
              />
              <Button @click="saveEditedRow(player.obj.id)"
                      icon="fas fa-check"
                      size="small"
                      variant="text"
                      severity="success"
                      rounded
              />
              <Button @click="removeRow(player.obj.id)"
                      icon="fas fa-trash"
                      size="small"
                      variant="text"
                      severity="danger"
                      rounded
              />
            </div>
          </td>
        </tr>
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

.actions {
  display: flex;
  flex-direction: row;
  gap: 0.2em;
  justify-content: center;
}

.team-badge {
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 0.5em;

  width: fit-content;
  background-color: #222;
  border-radius: 15px;
  padding: 0.2em;

  .team-name {
    line-height: 1.5em;
  }

  .team-icon {
    position: absolute;
    left: 0;
    top: 0;
    background-color: rgba(0, 191, 255, 0.49);
    aspect-ratio: 1/1;
    height: 2em;
    border-radius: 15px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
  }

  .team-name {
    padding-left: 2em;
  }

  .team-remove-button, .team-add-button {
    background-color: rgba(0, 0, 0, 0.5);
    border: none;
    border-radius: 50%;
    height: 1.75em;
    width: 1.75em;
    display: flex;
    justify-content: center;
    align-items: center;
    cursor: pointer;


    &:hover {
      background-color: rgba(0, 0, 0, 1);
    }

    font-size: 0.7em;
  }
  .team-add-button {
    background-color: rgb(35, 156, 69);
  }
}

table {
  border-radius: 12px;
  border: 1px solid #343434;
  background-color: #111111;
  height: 500px;
  width: 100%;
  display: grid;
  grid-template-rows: auto 1fr;

  th, td {
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

  .actions {
    display: flex;
    gap: 0.5rem;
    justify-content: flex-start;
  }
}
</style>
