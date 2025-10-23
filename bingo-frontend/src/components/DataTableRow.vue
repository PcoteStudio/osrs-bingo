<script setup lang="ts">

import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import KeysResult = Fuzzysort.KeysResult;
import type { PlayerResponse } from '@/clients/responses/playerResponse.ts';
import { computed, onMounted, ref } from 'vue'
import _ from 'lodash'

const props = defineProps<{
  highlight: (key: string, data: KeysResult<PlayerResponse>) => void;
  data: KeysResult<PlayerResponse>;
}>();

const emit = defineEmits<{
  save: [id: number, data: any]
  delete: [id: number]
}>();

const isEditing = ref(false);
const isDeleting = ref(false);
const tmpName = ref();
const tmpTeams = ref();

onMounted(() => {
  reset();
});

const canSave = computed(() =>
  isDeleting.value
  || tmpName.value !== props.data.obj.name
  || !_.isEqual(tmpTeams.value, props.data.obj.teams)

);

function reset() {
  tmpName.value = props.data.obj.name;
  tmpTeams.value = _.clone(props.data.obj.teams);
  isDeleting.value = false;
}

const saveEditedRow = () => {
  if (isDeleting.value) {
    emit('delete', props.data.obj.id);
  }
  else {
    let dataToUpdate = {
      name: tmpName.value,
      teams: tmpTeams.value,
    };
    emit('save', props.data.obj.id, dataToUpdate);
  }

  isEditing.value = false;
};

const editRow = () => {
  isEditing.value = true;
};

const cancelEditRow = () => {
  reset();
  isEditing.value = false;
};

const removeRow = () => {
  isDeleting.value = !isDeleting.value;
};

const removeTeam = (teamId: number) => {
  tmpTeams.value = tmpTeams.value.filter(t => t.id !== teamId);
};

const addTeam = () => {
  tmpTeams.value.push({})
};
</script>

<template>
  <tr :class="{ 'is-deleting': isDeleting }">
    <td><span v-html="highlight('id', data)" /></td>
    <td>
      <InputText v-if="isEditing" type="text" v-model="tmpName" @keydown.enter="saveEditedRow()" @keydown.esc="cancelEditRow()" />
      <span v-else v-html="highlight('name', data)" />
    </td>
    <td class="teams-container">
      <template v-if="isEditing">
        <div v-for="team in tmpTeams" :key="team.id">
          <div class="team-badge">
            <span class="team-icon">{{ team.id }}</span>
            <span class="team-name">{{ team.name }}</span>
            <button v-if="isEditing" class="team-remove-button" type="button" @click="removeTeam(team.id)">
              <FontAwesomeIcon icon="fas fa-x"/>
            </button>
          </div>
        </div>
        <div v-if="isEditing" class="team-badge" type="button" @click="addTeam()">
          <button class="team-add-button">
            <FontAwesomeIcon icon="fas fa-plus"/>
          </button>
        </div>
      </template>
      <template v-else>
        <div v-for="team in data.obj.teams" :key="team.id">
          <div class="team-badge">
            <span class="team-icon">{{ team.id }}</span>
            <span class="team-name">{{ team.name }}</span>
          </div>
        </div>
      </template>
    </td>
    <td>
      <div class="actions">
        <Button v-if="!isEditing"
                @click="editRow()"
                icon="fas fa-edit"
                size="small"
                variant="text"
                severity="primary"
                rounded
        />
        <template v-else>
          <Button @click="cancelEditRow()"
                  icon="fas fa-edit"
                  size="small"
                  variant=""
                  severity="primary"
                  rounded
          />
          <Button @click="removeRow()"
                  icon="fas fa-trash"
                  size="small"
                  :variant="isDeleting ? '' : 'text'"
                  severity="danger"
                  rounded
          />
          <Button @click="saveEditedRow()"
                  icon="fas fa-save"
                  size="small"
                  variant="text"
                  :severity="canSave ? 'success' : 'secondary'"
                  :disabled="!canSave"
                  rounded
          />
        </template>
      </div>
    </td>
  </tr>
</template>

<style scoped>
td {
  padding: 0.25em 0.75em;
  text-align: left;
}

.is-deleting {
  background-color: #333;
}

.actions {
  display: flex;
  flex-direction: row;
  gap: 0.2em;
  justify-content: center;
  background-color: #222;
  border-radius: 15px;
  padding: 0.2em;
  width: fit-content;
}

.teams-container {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5em;
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
    padding-right: 0.2em;
  }

  .team-icon {
    position: absolute;
    left: 0;
    top: 0;
    background-color: rgba(0, 191, 255, 0.49);
    aspect-ratio: 1/1;
    height: 100%;
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
</style>
