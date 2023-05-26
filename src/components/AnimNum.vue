<script lang="ts" setup>
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'

const {abs, floor} = Math

const props = withDefaults(
  defineProps<{
    num: number
    duration?: number
    ms?: number
    isLocal?: boolean
    isUnit?: boolean
  }>(),
  {
    num: 0,
    duration: 1500,
    ms: 10,
    isLocal: true,
    isUnit: false,
  }
)

const n = ref<number>(0)
const nStr = ref<string>('0')

let timer: null | NodeJS.Timer = null

function formatNum(val: number, isLocal: boolean, isUnit: boolean): string {
  let num: number | string = val
  let unit = ''
  if (isUnit) {
    if (abs(val) > 1000000) {
      num = floor(val / 1000000)
      unit = 'M'
    } else if (abs(val) > 1000) {
      num = floor(val / 1000)
      unit = 'K'
    } else {
      num = val
    }
  }
  if (isLocal) {
    num = num.toLocaleString()
  }
  return `${num}${unit}`
}

function handleAnim() {
  if (timer) {
    clearInterval(timer)
  }

  const { num, duration, ms, isLocal, isUnit } = props
  const inc = floor((num - n.value) / (duration / ms))

  timer = setInterval(() => {
    n.value += inc

    if (inc > 0) {
      if (n.value > num) n.value = num
    } else {
      if (n.value < num) n.value = num
    }

    if (n.value === num) clearInterval(timer!)

    nStr.value = formatNum(n.value, isLocal, isUnit)
  }, ms)
}

watch(
  () => props.num,
  () => {
    nextTick(() => {
      handleAnim()
    })
  },
  {
    immediate: true,
  }
)

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
