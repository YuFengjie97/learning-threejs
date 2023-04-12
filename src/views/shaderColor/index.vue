<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" @mousemove="onMouseMove" />
    </div>
  </div>
</template>

<script lang="ts" setup>
import * as THREE from 'three'

import vertexShader from './vertex.glsl?raw'
import fragmentShader from './fragment1.glsl?raw'

import { useTHREE } from '@/hooks/three'

const {
  canvasDom,
  canvasCon,
  width,
  height,
} = useTHREE(initMesh, animate)

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
}

function initMesh(): Array<THREE.Mesh> {
  const shaderMaterial = new THREE.ShaderMaterial({
    uniforms,
    vertexShader,
    fragmentShader,
  })

  const geo = new THREE.PlaneGeometry(2, 2)

  const mesh = new THREE.Mesh(geo, shaderMaterial)

  mesh.position.set(10, 10, 10)
  
  return [mesh]
}

function animate() {
  const t = performance.now() / 1000
  uniforms.u_time.value = t
}

function onMouseMove(e: MouseEvent) {
  uniforms.u_mouse.value.set(e.clientX, e.clientY)
}
</script>
