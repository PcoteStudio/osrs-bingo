<script setup lang="ts">
import { useGlobalStore } from '@/stores/globalStore';
import { ref } from 'vue';
import * as zResolver from '@primevue/forms/resolvers/zod';
import { z } from 'zod';
import type { FormSubmitEvent } from '@primevue/forms';
import { useAuthenticationStore } from '@/stores/authenticationStore.ts';
import { useNotificationStore } from '@/stores/notificationStore.ts';

const globalStore = useGlobalStore();
const notificationStore = useNotificationStore();
const authenticationStore = useAuthenticationStore();

const initialValues = ref({
  username: '',
  password: ''
});

const formSchema = z.object({
  username: z.string()
    .min(1, { message: 'Username is required' }),
  password: z.string()
    .min(1, { message: 'Password is required' })
});
const resolver = ref(zResolver.zodResolver(formSchema));

const onFormSubmit = async (submit: FormSubmitEvent) => {
  if (!submit.valid) {
    notificationStore.addNotification({
      logLevel: 'warn',
      message: 'Form is not valid',
      life: 5000
    });
    return;
  }

  if (await authenticationStore.authenticate(submit.values.username, submit.values.password)) {
    globalStore.toggleLoginModal();
  }
};
</script>

<template>
  <Dialog modal
          v-model:visible="globalStore.loginModalState.showModal"
          header="Login"
          :style="{ width: '25rem' }"
  >
    <div class="content">
      <div class="signup">
        <span>
          No account yet? 😢
        </span>
        <Button>Sign up</Button>
      </div>
      <Form v-slot="$form"
            :initialValues="initialValues"
            :resolver="resolver"
            :validateOnValueUpdate="false"
            :validateOnBlur="true"
            @submit="onFormSubmit"
      >
        <FloatLabel class="w-full">
          <InputText name="username" type="text" class="w-full" />
          <Message v-if="$form.username?.invalid" severity="error" size="small" variant="simple">
            {{ $form.username.error?.message }}
          </Message>
          <label>Username</label>
        </FloatLabel>
        <FloatLabel class="w-full">
          <Password name="password" :feedback="false" toggleMask fluid class="w-full" />
          <Message v-if="$form.password?.invalid" severity="error" size="small" variant="simple">
            {{ $form.password.error?.message }}
          </Message>
          <label>Password</label>
        </FloatLabel>
        <Button type="submit" label="Login" :disabled="!$form.valid" />
      </Form>
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

  .signup {
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;

    * {
      width: fit-content;
    }
  }
}
</style>
