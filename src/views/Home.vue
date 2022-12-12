<template>
  <div class='home'>
    <h1>Chapter 1</h1>
    <RouterLink to="/chapter1/01-basic-skeleton">01-basic-skeleton</RouterLink>
    <RouterLink to="/chapter1/02-first-scene">02-first-scene</RouterLink>
    <RouterLink to="/chapter1/03-materials-light">03-materials-light</RouterLink>
    
    <RouterLink to="/chapter1/04-materials-light-animation">04-materials-light-animation</RouterLink>
  </div>
</template>

<script lang="ts" setup>
import { ref} from 'vue';

</script>

<style lang='less' scoped>
.home{}
</style>