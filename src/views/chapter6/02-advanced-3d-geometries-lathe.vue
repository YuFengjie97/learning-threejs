<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 更改参数都是在scene中remove掉原有的mesh，然后重新根据参数初始化mesh
 * 如果是动态修改geo呢？我只记得bufferGeo可以动态初始化，用needUpdate=true来通知渲染器更新mesh
 * 这两种哪种性能更好呢？
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { createMesh } from '@/utils'

const { random, PI, floor, ceil, min, max, sin, cos } = Math

const canvasDom = ref<HTMLElement>()
const canvasCon = ref<HTMLElement>()
let width = window.innerWidth
let height = window.innerHeight
let stats: Stats
let orbitControls: OrbitControls
let scene: THREE.Scene
let camera: THREE.PerspectiveCamera
let renderer: THREE.WebGLRenderer
let mesh: THREE.Group

onMounted(() => {
  initTHREE()
  initMesh()
  initGUI()
  render()
})

const params = {
  height: 5,
  pointNum: 30,
  segments: 12,
  phiStart: 0,
  phiLength: PI * 2,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params,'height',1,20,1).onChange(()=>initMesh())
  gui.add(params,'pointNum',1,50,1).onChange(()=>initMesh())
  gui.add(params, 'segments', 1, 20, 1).onChange(()=>initMesh())
  gui.add(params, 'phiStart', 0, PI * 2, 0.01).onChange(()=>initMesh())
  gui.add(params, 'phiLength', 0, PI * 2, 0.01).onChange(()=>initMesh())
}

let step = 0
function animate() {
  mesh.rotation.x = step += 0.005
  mesh.rotation.y = step += 0.005
}
function initMesh() {
  if(mesh) scene.remove(mesh)
  mesh = new THREE.Group()
  scene.add(mesh)

  // 点
  let points = getLathePoints()
  points.forEach((p) => {
    const sphereGeo = new THREE.SphereGeometry(0.4)
    const sphereMat = new THREE.MeshLambertMaterial({ color: 0xff0000 })
    const sphere = new THREE.Mesh(sphereGeo, sphereMat)
    sphere.position.set(p.x, p.y, 0)
    mesh.add(sphere)
  })

  // lathe
  const { segments, phiStart, phiLength } = params
  const latheGeo = new THREE.LatheGeometry(
    points,
    segments,
    phiStart,
    phiLength
  )
  const lathe = createMesh(latheGeo)
  mesh.add(lathe)
}
function getLathePoints() {
  let points: Array<THREE.Vector2> = []
  const { height, pointNum } = params
  for (let i = 0; i < pointNum; i++) {
    points.push(
      new THREE.Vector2(
        (Math.sin(i * 0.2) + Math.cos(i * 0.3)) * height + 12,
        i - pointNum + pointNum / 2
      )
    )
  }
  return points
}

// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0xffffff)
  camera = new THREE.PerspectiveCamera(75, width / height, 1, 1000)
  renderer = new THREE.WebGLRenderer({
    canvas: canvasDom.value,
    antialias: true,
  })
  // renderer.shadowMap.enabled = true
  renderer.setSize(width, height)
  // renderer.setPixelRatio(window.devicePixelRatio) // 不推荐
  window.addEventListener('resize', onWindowResize)

  // 坐标轴
  scene.add(new THREE.AxesHelper(1000))

  // 环境光
  scene.add(new THREE.AmbientLight(0xffffff))

  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)

  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(0, 30, 50)
  orbitControls.update()
}
// 绘制
function render() {
  requestAnimationFrame(render)
  stats.update()
  animate()
  renderer.render(scene, camera)
}
// 自适应
function onWindowResize() {
  width = window.innerWidth
  height = window.innerHeight
  camera.aspect = width / height
  camera.updateProjectionMatrix()
  renderer.setSize(width, height)
}
</script>
