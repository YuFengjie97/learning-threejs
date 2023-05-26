<script lang="ts" setup>
import { ref, onMounted, onUnmounted, watch, nextTick, computed } from 'vue'

const { abs, floor } = Math

const props = withDefaults(
  defineProps<{
    num: number | string // 传入的值
    duration?: number // 动画总时长
    ms?: number // interval定时器多久触发一次，
    isLocal?: boolean // 是否本地化
    isUnit?: boolean // 是否加单位K，M
    startZero?: boolean // 变化动画从0开始，还是从上次状态开始
  }>(),
  {
    num: 0,
    duration: 1500,
    ms: 10,
    isLocal: true,
    isUnit: false,
    startZero: false
  }
)

const n = ref<number>(0)
const nStr = ref<string>('0')

let timer: any = null

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
  if (props.startZero) {
    n.value = 0
  }

  const { duration, ms, isLocal, isUnit } = props
  const num = Number(props.num)
  const inc = floor((num - n.value) / (duration / ms))

  timer = setInterval(() => {
    n.value += inc

    if (inc > 0 && n.value > num) {
      n.value = num
      clearInterval(timer!)
    } else if (inc < 0 && n.value < num) {
      n.value = num
      clearInterval(timer!)
    }

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
    immediate: true
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
