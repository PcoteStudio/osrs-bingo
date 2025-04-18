import './assets/main.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import { library } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { fab } from '@fortawesome/free-brands-svg-icons';
import PrimeVue from 'primevue/config';
import Aura from '@primeuix/themes/aura';
import Tooltip from 'primevue/tooltip';
import ToastService from 'primevue/toastservice';

import App from './App.vue';
import router from './router';
import Ripple from 'primevue/ripple';
import { AuthenticationService } from '@/services/authenticationService.ts';
import { HttpClient } from '@/clients/httpClient.ts';
import { AuthenticationClient } from '@/clients/authenticationClient.ts';
import { initializeTheme, ThemeType } from '@/utils/themeUtils.ts';

library.add(fas, far, fab);

const app = createApp(App);

const authenticationClient = AuthenticationClient.create();
app.provide(AuthenticationClient.injectionKey, authenticationClient);

const authenticationService = new AuthenticationService(authenticationClient);
app.provide(AuthenticationService.injectionKey, authenticationService);

const httpClient = HttpClient.create(authenticationService);
app.provide(HttpClient.injectionKey, httpClient);

// Store
app.use(createPinia());

// Router
app.use(router);

// PrimeVue
app.use(ToastService);
app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      prefix: 'p',
      darkModeSelector: '.' + ThemeType.dark,
      ripple: true,
      cssLayer: {
        name: 'primevue',
        order: 'tailwind-base, primevue, tailwind-utilities'
      }
    }
  }
});
app.directive('tooltip', Tooltip);
app.directive('ripple', Ripple);

initializeTheme();

// FontAwesome
app.component('FontAwesomeIcon', FontAwesomeIcon);

app.mount('#app');
