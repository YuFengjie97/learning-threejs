<script lang="ts" setup>
import { ref, onMounted } from 'vue';
import loading from '@/utils/loading'

function handleLoading() {
  const l = loading.service()

  setTimeout(() => {
    l.close()
  }, 3000);
}

</script>

<template>
  <div class='view-con'>
    <h1>loading-service测试</h1>
    <h1>loading-service测试</h1>
    <h1>loading-service测试</h1>
    <button @click="handleLoading">loading</button>
    <h1>loading-service测试</h1>
    <h1>loading-service测试</h1>
    <h1>loading-service测试</h1>
    <h1>loading-service测试</h1>
  </div>
</template>

<style lang='less' scoped>
.view-con{
  background: #fff;
  color: #000;
  line-height: 2.5rem;
  height: 100vh;
  button{
    padding: 2rem;
  }
}
</style>@/utils/loading/loading