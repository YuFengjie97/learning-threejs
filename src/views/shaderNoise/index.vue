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
import fragmentShader from './frag1.fs?raw'

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
  }
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

function animate() {
  t = performance.now() / 1000

  uniforms.u_time.value = t
}

function onMouseMove(e: MouseEvent) {
  uniforms.u_mouse.value.set(e.clientX, e.clientY)
}
</script>
