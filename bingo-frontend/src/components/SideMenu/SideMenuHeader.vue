<script setup lang="ts">

</script>

<template>
<div class="drawer-header">
  <div class="logo">
    <img src="@/assets/logo.png"  alt="OSRS Bingo logo"/>
    OSRS Bingo
  </div>
</div>
</template>

<style scoped>
.drawer-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px;
}

.logo {
  color: yellow;
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 0.5em;

  font-weight: bold;
  font-size: 24px;
  img {
    height: 32px;
  }
}
</style>
