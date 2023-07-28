<script lang="ts" setup>
import { ref, computed } from 'vue';
import { useTransition, TransitionPresets } from '@vueuse/core'

const source = ref(0)

function addSource() {
  source.value += 1000
}

const sourceFormat = computed(() => {
  
})

const output = useTransition(source, {
  duration: 1000,
  transition: TransitionPresets.linear
})


</script>

<template>
  <div class='view-con'>
    <button @click="addSource">adddd</button>
    <h1>{{ output.toFixed(0) }}</h1>
  </div>
</template>

<style lang='less' scoped>
.view-con{}
</style>