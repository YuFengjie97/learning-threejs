<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" @mousemove="onMouseMove" />
    </div>
  </div>
</template>

<script lang="ts" setup>
import * as THREE from 'three'

import vertexShader from './vert.vs?raw'
import fragmentShader from './frag3.fs?raw'

import { useTHREE } from '@/hooks/three'

const { random, PI, floor, ceil, min, max, sin, cos, tan, abs } = Math

const { canvasDom, canvasCon, width, height } = useTHREE(initMesh, animate)

const uniforms = {
  u_resolution: {
    value: new THREE.Vector2(width, height),
  },
  u_mouse: {
    value: new THREE.Vector2(width / 2, height / 2),
  },
  u_time: {
    value: 0,
  },
  u_color: {
    value: new THREE.Vector3(1, 1, 1),
  },
  u_r1: {
    value: 0.1,
  },
  u_r2: {
    value: 0.3,
  },
  u_r3: {
    value: 0.5,
  },
  u_r4: {
    value: 0.09,
  },
  u_yis: {
    value: [true], // 挂的三爻变
  },
}

function initYis() {
  uniforms.u_yis.value.length = 0
  for (let i = 0; i < 100; i++) {
    uniforms.u_yis.value.push(random() > 0.5, random() > 0.5, random() > 0.5)
  }
}
initYis()

function updateYis() {
  uniforms.u_yis.value.forEach((yi, i) => {
    uniforms.u_yis.value[i] = random() > 0.5
  })
}

let t

function initMesh(): Array<THREE.Mesh> {
  const shaderMaterial = new THREE.ShaderMaterial({
    uniforms,
    vertexShader,
    fragmentShader,
  })

  const geo = new THREE.PlaneGeometry(1000, 1000)

  const mesh = new THREE.Mesh(geo, shaderMaterial)

  mesh.position.set(10, 10, 10)

  return [mesh]
}

let lastTime = 0
let updateGap = 1 // 秒
function animate() {
  t = performance.now() / 1000

  uniforms.u_time.value = t
  // const r = sin(t)
  // const g = sin(t + 0.4)
  // const b = sin(t + 0.2)

  uniforms.u_r1.value = abs(sin(t) * 0.1)
  uniforms.u_r2.value = abs(cos(t) * 0.1)
  uniforms.u_r3.value = random() * 0.1

  if (t % PI > 3.1) {
    uniforms.u_color.value = new THREE.Vector3(random(), random(), random())
  }
  
  const gap = t - lastTime
  if (gap > updateGap) {
    updateYis()
    lastTime = t
  }
}

function onMouseMove(e: MouseEvent) {
  uniforms.u_mouse.value.set(e.clientX, e.clientY)
}
</script>
