<script setup lang="ts">
import { useGlobalStore } from '@/stores/globalStore';
import { ref } from 'vue';
import { useToast } from 'primevue/usetoast';

const toast = useToast();
const store = useGlobalStore();

const email = ref();
const password = ref();

const login = () => {
  if (!email.value || !password.value) {
    toast.add({
      severity: 'warn',
      detail: 'Please provide import data',
      life: 5000,
    });
  }

  toast.add({
    severity: 'info',
    detail: 'Login in progress . . .',
    life: 5000,
  });

  console.log('Login', email, password);
};
</script>

<template>
  <Dialog modal
          v-model:visible="store.loginModalState.showModal"
          header="Login"
          :style="{ width: '25rem' }"
  >
    <div class="content">
      <FloatLabel class="w-full">
        <InputText v-model="email" class="w-full" />
        <label>E-mail</label>
      </FloatLabel>
      <FloatLabel class="w-full">
        <InputText v-model="password" class="w-full" />
        <label>Password</label>
      </FloatLabel>
      <Button label="Login" icon="pi pi-file-import" :disabled="email || password" @click="login()" />
    </div>
  </Dialog>
</template>

<style scoped>
.content {
  display: flex;
  flex-direction: column;
  gap: 1rem;

  * {
    width: 100%;
  }
}
</style>
