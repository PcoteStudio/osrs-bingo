<script setup lang="ts">
import SideMenuHeader from './SideMenuHeader.vue';
import SideMenuBody from './SideMenuBody.vue';
import SideMenuFooter from './SideMenuFooter.vue';

</script>

<template>
  <div class="custom-drawer-wrapper">
    <div class="background-element"></div>
    <div class="custom-drawer">
      <div class="drawer-content">
        <SideMenuHeader />
        <SideMenuBody />
        <SideMenuFooter />
      </div>
    </div>
  </div>
</template>

<style scoped>
.custom-drawer-wrapper {
  width: min-content;
  position: relative;
  height: 100%;
  overflow: visible;
  z-index: 1;
}

.custom-drawer {
  height: 100%;
  width: 250px;
  max-width: 250px;
  position: relative;
  color: white;
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  overflow: visible;
}

.background-element {
  position: absolute;
  top: -50px;
  left: -50px;
  right: -50px;
  bottom: -50px;
  background-blend-mode: multiply;
  background: url("@/assets/Nieve.png") no-repeat left;
  transform: scaleX(-1);
  z-index: -1;
  pointer-events: none;
}

.drawer-content {
  position: relative;
  display: flex;
  flex-direction: column;
  height: 100%;
  width: 100%;
  overflow-y: auto;
  z-index: 2;
  background-color: rgba(17, 17, 17, 0.95);
}

hr {
  color: #4b4b4b;
}
</style>
