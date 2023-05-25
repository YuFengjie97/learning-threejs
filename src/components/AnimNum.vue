<script lang="ts" setup>
import { ref, onMounted, onUnmounted } from 'vue'
const props = withDefaults(
  defineProps<{
    num: number
    duration: number
    ms: number
    isLocal: boolean
    isUnit: boolean
  }>(),
  {
    num: 0,
    duration: 3000,
    ms: 10,
    isLocal: true,
    isUnit: false
  }
)

const n = ref<number>(0)
const nStr = ref<string>('0')

let timer: null | NodeJS.Timer = null

function formatUnit(val: number): string {
  if (val > 1000000) {
    return `${Math.floor(val / 1000000)}M`
  } else if (val > 1000) {
    return `${Math.floor(val / 1000)}K`
  } else {
    return `${val}`
  }
}

onMounted(() => {
  const { num, duration, ms, isLocal, isUnit } = props

  let inc = Math.floor(num / (duration / ms))

  timer = setInterval(() => {
    if (n.value < num) {
      n.value += inc
    } else {
      n.value = num
    }

    if (isUnit) {
      nStr.value = formatUnit(n.value)
    } else {
      if(isLocal) {
        nStr.value =  n.value.toLocaleString()
      } else {
        nStr.value = `${n}`
      }
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
  <div class="anim-num">{{ nStr }}</div>
</template>

<style lang="less" scoped>
.anim-num {
}
</style>
