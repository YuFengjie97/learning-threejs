<script lang="ts" setup>
import {
  GLSL3,
  Mesh,
  PlaneGeometry,
  RawShaderMaterial,
  ShaderMaterial,
  Vector2,

} from 'three'

import { onMounted, ref } from 'vue'
import vertexShader from './vert.vs?raw'

// import fragmentShader from './色轮.fs?raw'

// import fragmentShader from './塔罗牌.fs?raw'
// import fragmentShader from './造型函数.fs?raw'
// import fragmentShader from './雷达.fs?raw'
// import fragmentShader from './shadertoy_line.fs?raw'
// import fragmentShader from './shadertoy_色条.fs?raw'
// import fragmentShader from './shadertoy_art.fs?raw'

// import fragmentShader from './line.fs?raw'
// import fragmentShader from './polar_shape_5.fs'
// import fragmentShader from './line_noise_3.fs'
// import fragmentShader from './sdf_triangle.fs'
// import fragmentShader from './circle.fs'
// import fragmentShader from './noise.fs'
// import fragmentShader from './circle_noise.fs'
// import fragmentShader from './mandelbrot.fs'
// import fragmentShader from './loading_2.fs'
// import fragmentShader from './valuenoise.fs'
// import fragmentShader from './pattern_circle_gradientNoise.fs'
// import fragmentShader from './frog_valueNoise_fbm.fs'
// import fragmentShader from './domain_wrapping.fs'
import fragmentShader from './kunkun.fs'

// import fragmentShader from './pattern_circle_valueNoise.fs'

import { initThree } from '@/utils'

const con = ref<HTMLElement>()
onMounted(() => {
  const { canvas, scene, render } = initThree(con.value!)
  const { width, height } = canvas
  let t = 0
  const uniforms = {
    iResolution: {
      value: new Vector2(width, height),
    },
    iMouse: {
      value: new Vector2(width / 2, height / 2),
    },
    iTime: {
      value: 0,
    },
  }

  const { x, y, height: conH } = con.value!.getBoundingClientRect()
  canvas.addEventListener('mousemove', (e) => {
    uniforms.iMouse.value.set(e.clientX - x, conH - (e.clientY - y))
  })

  const shaderMaterial = new ShaderMaterial({
    // extensions: {
    //   derivatives: '#extension GL_OES_standard_derivatives : enable',
    // },
    uniforms,
    vertexShader,
    fragmentShader,
  })
  // const shaderMaterial = new RawShaderMaterial({
  //   uniforms,
  //   vertexShader,
  //   fragmentShader,
  //   glslVersion: GLSL3,
  // })
  const geo = new PlaneGeometry(1000, 1000)
  const mesh = new Mesh(geo, shaderMaterial)
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

<style>
a{
  color: rgb(253, 121, 168);
  color:rgb(232, 67, 147);
}
</style>
