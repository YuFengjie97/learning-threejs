<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { gsap } from 'gsap'

interface Pos {
  x: number
  y: number
}

interface Range {
  l: number
  r: number
  t: number
  b: number
}

const { random, floor } = Math

const boxSize = 80
const boxConSize = 400

const balls = ref<Array<HTMLElement>>([])

class Ball {
  private range: Range
  private el: HTMLElement
  private tl: gsap.core.Timeline
  private duration: number

  constructor(el: HTMLElement, range: Range) {
    this.el = el
    this.range = range
    this.duration = random() * 0.4 + 0.2
    // const pos = getRandomPos(range)
    // this.el.style.translate = `${pos.x}px ${pos.y}px`
    this.tl = gsap.timeline({
      onComplete: () => {
        this.goNext()
      },
    })
    gsap.to(this.el, {
      rotate: random() > 0.5 ? 360 : -360,
      repeat: -1,
      duration: 1,
      ease: "linear"
    })
  }

  goNext() {
    const nextPos = getRandomPos(this.range)
    this.tl.to(this.el, { ...nextPos, duration: this.duration})
  }
}

function getRandomPos(range: Range): Pos {
  const { l, r, t, b } = range
  return { x: random() * (r - l) + l, y: random() * (b - t) + t }
}

function go() {
  balls.value.forEach((b) => {
    const ball = new Ball(b, { l: 0, r: boxConSize - boxSize, t: 0, b: boxConSize - boxSize })
  })
}

onMounted(() => {
  go()
})
</script>

<template>
  <div class="view-con">
    <button @click="go">Go</button>
    <div class="ball-con">
      <div class="ball" v-for="i in 20" :key="i" ref="balls"></div>
    </div>
  </div>
</template>

<style lang="less" scoped>
.view-con {
  height: 100vh;
  button {
    padding: 10px 20px;
  }
  .ball-con {
    position: relative;
    width: 400px;
    height: 400px;
    border-radius: 50%;
    border: 2px solid red;
    overflow: hidden;
  }
  .ball {
    position: absolute;
    width: 80px;
    height: 80px;
    border-radius: 16px;
    background: green;
    border: 2px solid yellow;
  }
}
</style>
