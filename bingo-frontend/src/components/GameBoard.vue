<script setup lang="ts">

</script>

<template>
  <div class="container">
    <div class="banner">
      <span class="title">Bingo</span>
    </div>
    <div class="board">
      <div v-for="row in 10" class="row" :key="row">
        <div v-for="column in 10" class="tile" :key="column">
          <span>
<!--             <img src="@/assets/logo.png"  alt="Nieve’s Orders logo"/>-->
            {{ row }} / {{ column }}
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.container {
  display: flex;
  flex-direction: column;


}
  .banner {
    display: flex;
    justify-content: space-around;

    .title {
      font-size: 3em;
    }
  }
.board {
  display: flex;
  flex-direction: column;
  gap: 5px;

  width: 100%;
  height: 100%;
  overflow-y: auto;
  overflow-x: hidden;

  .row {
    height: 100%;
    display: flex;
    flex-direction: row;
    gap: 5px;

    .tile {
      display: flex;
      padding-right: 5px;
      justify-content: right;
      width: 100%;
      background: rgba(0, 0, 0, 0.5);
    }
  }
}
</style>
