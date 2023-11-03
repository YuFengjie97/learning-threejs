<script lang="ts" setup>
import * as THREE from 'three'

import { onMounted, ref } from 'vue'
import vertexShader from './vert.vs?raw'

// import fragmentShader from './色轮.fs?raw'

// import fragmentShader from './塔罗牌.fs?raw'
// import fragmentShader from './造型函数.fs?raw'
// import fragmentShader from './雷达.fs?raw'
// import fragmentShader from './shadertoy_line.fs?raw'
// import fragmentShader from './shadertoy_色条.fs?raw'
import fragmentShader from './shadertoy_art.fs?raw'

import { initThree } from '@/utils'

const con = ref<HTMLElement>()
onMounted(() => {
  const { canvas, scene, render } = initThree(con.value!)
  const { width, height } = canvas
  let t = 0
  const uniforms = {
    iResolution: {
      value: new THREE.Vector2(width, height),
    },
    iMouse: {
      value: new THREE.Vector2(width / 2, height / 2),
    },
    iTime: {
      value: 0,
    },
  }

  canvas.addEventListener('mousemove', (e) => {
    const { x, y } = con.value!.getBoundingClientRect()
    uniforms.iMouse.value.set(e.clientX - x, e.clientY - y)
  })

  const shaderMaterial = new THREE.ShaderMaterial({
    uniforms,
    vertexShader,
    fragmentShader,
  })
  const geo = new THREE.PlaneGeometry(1000, 1000)
  const mesh = new THREE.Mesh(geo, shaderMaterial)
  mesh.position.set(10, 10, 10)
  scene.add(mesh)

  render(() => {
    t = performance.now() / 1000
    uniforms.iTime.value = t
  })
})
</script>

<template>
  <div ref="con" class="h-full w-full" />
</template>
