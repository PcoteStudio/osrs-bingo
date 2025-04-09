<script setup lang="ts">
import SideMenuHeader from './SideMenuHeader.vue';
import SideMenuBody from './SideMenuBody.vue';
import SideMenuFooter from './SideMenuFooter.vue';

</script>

<template>
  <div class="custom-drawer-wrapper">
    <div class="custom-drawer">
      <div class="drawer-content">
        <SideMenuHeader />
        <hr>
        <SideMenuBody />
        <hr>
        <SideMenuFooter email="n.signo@hotmail.com" username="Woofi" />
      </div>
    </div>
  </div>
</template>

<style scoped>
.custom-drawer-wrapper {
  position: relative;
  height: 100%;
}

.custom-drawer {
  position: fixed;
  top: 0;
  left: 0;
  height: 100%;
  width: 250px;
  max-width: 250px;
  background-color: #111111;
  color: white;
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  overflow-y: auto;
}

.drawer-content {
  display: flex;
  flex-direction: column;
  height: 100%;
}

hr {
  color: #4b4b4b;
}
</style>
