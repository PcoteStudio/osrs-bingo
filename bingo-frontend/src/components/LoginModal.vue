<script setup lang="ts">
import { useGlobalStore } from '@/stores/globalStore';
import { ref } from 'vue'
import { useToast } from 'primevue/usetoast';
import  * as zResolver from '@primevue/forms/resolvers/zod';
import { z } from 'zod';
import type { FormSubmitEvent } from '@primevue/forms'

const store = useGlobalStore();

const initialValues = ref({
  email: '',
  password: ''
});


const formSchema = z.object({
  email: z.string()
    .min(1, { message: 'Email is required' })
    .email({ message: 'Please enter a valid email address' }),
  password: z.string()
    .min(1, { message: 'Password is required' })
});
const resolver = ref(zResolver.zodResolver(formSchema));

const onFormSubmit = (submit: FormSubmitEvent) => {
  if (!submit.valid) {
    store.addNotification({
      logLevel: 'warn',
      message: 'Form is not valid',
      life: 5000
    })
    return;
  }

  store.authenticate(submit.values.email, submit.values.password);
};
</script>

<template>
  <Dialog modal
          v-model:visible="store.loginModalState.showModal"
          header="Login"
          :style="{ width: '25rem' }"
  >
    <div class="content">
      <Form v-slot="$form"
            :initialValues="initialValues"
            :resolver="resolver"
            :validateOnValueUpdate="false"
            :validateOnBlur="true"
            @submit="onFormSubmit"
      >
        <FloatLabel class="w-full">
          <InputText name="email" type="email" class="w-full" />
          <Message v-if="$form.email?.invalid" severity="error" size="small" variant="simple">
            {{ $form.email.error?.message }}
          </Message>
          <label>E-mail</label>
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
}
</style>
