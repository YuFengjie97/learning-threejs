<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 原来的方法已经弃用，现在可以new THREE.Mesh(geo,mat)中的mat可以是Array<mat>
 * 原来的mats是12个材质，分别对应6个面，一个面又有两个三角形面，所以是12个
 * 只用6个就可以
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'

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

let ground: THREE.Mesh
let cubes: THREE.Group

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  initLight()
  render()
})

const params = {
  rotateSpeed: 0.02
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params,'rotateSpeed',0,2,0.001)
}

function animate() {
  cubes.rotation.y += params.rotateSpeed
}
function initMesh() {
  const groundGeo = new THREE.PlaneGeometry(100, 100, 4, 4)
  const groundMat = new THREE.MeshBasicMaterial({ color: 0x444444 })
  ground = new THREE.Mesh(groundGeo, groundMat)
  ground.rotation.x = -0.5 * PI
  scene.add(ground)

  cubes = new THREE.Group()
  scene.add(cubes)
  const mats = []
  mats.push(new THREE.MeshBasicMaterial({ color: 0x00b894 }))
  mats.push(new THREE.MeshBasicMaterial({ color: 0x00cec9 }))
  mats.push(new THREE.MeshBasicMaterial({ color: 0x0984e3 }))
  mats.push(new THREE.MeshBasicMaterial({ color: 0x6c5ce7 }))
  mats.push(new THREE.MeshBasicMaterial({ color: 0xfdcb6e }))
  mats.push(new THREE.MeshBasicMaterial({ color: 0xe17055 }))

  let size = 3
  let cubeSize = 2.9

  for (let x = 0; x < 3; x++) {
    for (let y = 0; y < 3; y++) {
      for (let z = 0; z < 3; z++) {
        const geo = new THREE.BoxGeometry(cubeSize, cubeSize, cubeSize)
        const cube = new THREE.Mesh(geo, mats)
        cube.position.set(
          x * size - size,
          y * size + size / 2,
          z * size - size
        )
        cubes.add(cube)
      }
    }
  }
}
function initLight() {
  const spotLight = new THREE.SpotLight(0xffffff, 1)
  spotLight.target = ground
  spotLight.position.set(-50, 50, 50)
  const spotLightHelper = new THREE.SpotLightHelper(spotLight)
  scene.add(spotLight)
  scene.add(spotLightHelper)
}
// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  // scene.background = new THREE.Color(0x444444)
  camera = new THREE.PerspectiveCamera(75, width / height, 1, 1000)
  renderer = new THREE.WebGLRenderer({
    canvas: canvasDom.value,
    antialias: true,
  })
  renderer.setSize(width, height)
  // renderer.setPixelRatio(window.devicePixelRatio) // 不推荐
  window.addEventListener('resize', onWindowResize)

  // 坐标轴
  scene.add(new THREE.AxesHelper(1000))

  // 环境光
  scene.add(new THREE.AmbientLight(0x404040))

  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)

  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(40, 40, 40)
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
