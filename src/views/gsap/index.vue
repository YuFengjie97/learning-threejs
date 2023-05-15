<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import gsap from 'gsap'

onMounted(() => {
  gsap.to('.ball', {
    x: 200,
  })
})
</script>

<template>
  <div class="view-con">
    <div class="ball-con">
      <div class="ball" v-for="i in 10" :key="i"></div>
    </div>
  </div>
</template>

<style lang="less" scoped>
.view-con {
  height: 100vh;
  display: grid;
  place-items: center;
  .ball-con {
    position: relative;
    width: 400px;
    height: 400px;
    border-radius: 50%;
    border: 2px solid red;
    overflow: hidden;
  }
  .ball {
    width: 30px;
    height: 30px;
    border-radius: 50%;
    background: green;
  }
}
</style>
