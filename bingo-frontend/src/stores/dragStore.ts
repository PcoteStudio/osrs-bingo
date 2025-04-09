import { defineStore } from 'pinia';

export const useDragStore = defineStore('dragStore', {
  state: () => {
    const isDragging = false;
    const dragFrom = undefined as string | undefined;
    const dragTarget = undefined as string | undefined;
    const targetLocation = undefined as string | undefined;

    return {
      isDragging,
      dragFrom,
      dragTarget,
      targetLocation
    };
  }
});