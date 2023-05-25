<script lang="ts" setup>
import { ref, onMounted, onUnmounted } from 'vue'
const props = withDefaults(
  defineProps<{
    num: number
    duration: number
    ms: number
  }>(),
  {
    num: 0,
    duration: 3000,
    ms: 10,
  }
)

const n = ref(0)

let timer: null | NodeJS.Timer = null

onMounted(() => {
  const { num, duration, ms } = props

  let inc = Math.floor(num / (duration / ms))

  timer = setInterval(() => {
    if (n.value < num) {
      n.value += inc
    } else {
      n.value = num
    }
  }, ms)
})
onUnmounted(() => {
  if (timer) {
    clearInterval(timer)
  }
})
</script>

<template>
  <div class="anim-num">{{ n }}</div>
</template>

<style lang="less" scoped>
.anim-num {
}
</style>
