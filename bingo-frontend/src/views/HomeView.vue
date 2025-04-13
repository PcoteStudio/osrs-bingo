<script setup lang="ts">
import NotificationComponent from '@/components/Notification/NotificationComponent.vue'
import SideMenuContainer from '@/components/SideMenu/SideMenuContainer.vue'
import GameBoard from '@/components/GameBoard.vue'
import LoginModal from '@/components/LoginModal.vue';
import SignupModal from '@/components/SignupModal.vue'
</script>

<template>
  <main>
    <!-- Special components -->
    <NotificationComponent />

    <!--  Modals  -->
    <LoginModal />
    <SignupModal />

    <!--  Regular components  -->
    <SideMenuContainer />
    <GameBoard />
  </main>
</template>

<style scoped>
main {
  display: flex;
  gap: 0.5rem;
  width: 100%;
  height: 100vh;
  overflow-y: hidden;

  @media screen and (max-width: 450px) {
    flex-wrap: wrap;
    overflow-y: auto;
    justify-content: center;
  }
}
</style>
