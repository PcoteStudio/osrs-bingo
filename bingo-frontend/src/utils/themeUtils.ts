import { usePreset } from '@primeuix/themes';
import Aura from '@primeuix/themes/aura';

export enum ThemeType {
  dark = 'dark-theme',
  light = 'light-theme',
}

export const applyTheme = (theme: string) => {
  document.documentElement.classList.remove(
    ThemeType.dark,
    ThemeType.light,
  );

  switch (theme) {
    case ThemeType.light:
      document.documentElement.classList.add(ThemeType.light);
      usePreset(Aura);
      break;
    case ThemeType.dark:
    default:
      document.documentElement.classList.add(ThemeType.dark);
      usePreset(Aura);
      break;
  }

  localStorage.setItem('theme-preference', theme);
};

export const getPreferredTheme = (): ThemeType => {
  const savedTheme = localStorage.getItem('theme-preference') as ThemeType;

  if (savedTheme && Object.values(ThemeType).includes(savedTheme)) {
    return savedTheme;
  }

  if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
    return ThemeType.dark;
  }

  return ThemeType.light;
};

export const initializeTheme = () => {
  const preferredTheme = getPreferredTheme();
  applyTheme(preferredTheme);

  if (window.matchMedia) {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
      if (!localStorage.getItem('theme-preference')) {
        applyTheme(e.matches ? ThemeType.dark : ThemeType.light);
      }
    });
  }
};
