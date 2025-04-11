<script setup lang="ts">
import { useGlobalStore } from '@/stores/globalStore';
import { ref } from 'vue'
import  * as zResolver from '@primevue/forms/resolvers/zod';
import { z } from 'zod';
import type { FormSubmitEvent } from '@primevue/forms'

const store = useGlobalStore();

const initialValues = ref({
  email: '',
  password: '',
  username: '',
});


const formSchema = z.object({
  email: z.string()
    .min(1, { message: 'Email is required' })
    .email({ message: 'Please enter a valid email address' }),
  password: z.string()
    .min(1, { message: 'Password is required' })
    .min(6, { message: 'Password must be at least 8 characters long' })
    .regex(/[A-Z]/, { message: 'Password must contain at least one uppercase letter' })
    .regex(/[a-z]/, { message: 'Password must contain at least one lowercase letter' })
    .regex(/[0-9]/, { message: 'Password must contain at least one number' })
    .regex(/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/, {
      message: 'Password must contain at least one special character'
    }),
  username: z.string()
    .min(3, { message: 'Username is required' }),
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

  store.createUser(submit.values.email, submit.values.password, submit.values.username);
};
</script>

<template>
  <Dialog modal
          v-model:visible="store.getSignupModalState.showModal"
          header="Sign up"
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
          <InputText name="username" type="text" class="w-full" />
          <Message v-if="$form.username?.invalid" severity="error" size="small" variant="simple">
            {{ $form.username.error?.message }}
          </Message>
          <label>Username</label>
        </FloatLabel>
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
        <Button type="submit" label="Sign up" :disabled="!$form.valid" />
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
