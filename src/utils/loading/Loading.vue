<script lang="ts" setup>
</script>

<template>
  <div class='my-loading-con'>
    <div class="mask"></div>
    <div class="text-loading">loading....</div>
  </div>
</template>

<style lang='less' scoped>

.my-loading-con {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: grid;
  place-items: center;
  .mask {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.505);
    z-index: 1;
  }
  .text-loading {
    position: relative;
    font-size: 2rem;
    font-weight: 900;
    color: #fff;
    z-index: 2;
  }
}

</style>