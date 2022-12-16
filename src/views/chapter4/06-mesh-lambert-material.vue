<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * lambert材质的颜色是由自身color和emissive共同决定的，
 * wrapRGB属性在ts中没有找到，估计是被弃用了
 * wireframeLinewidth 由于OpenGL Core Profile与 大多数平台上WebGL渲染器的限制，无论如何设置该值，线宽始终为1。
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
let cube: THREE.Mesh
let sphere: THREE.Mesh
let plane: THREE.Mesh
let meshMaterial: THREE.MeshLambertMaterial = new THREE.MeshLambertMaterial({color: 0xe84393})

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  initLight()
  render()
})

const mesh = {
  selectMesh: 'cube',
  color: 0xe84393,
  emissive: 0xffffff,
  transparent: false,
  opacity: 1,
  side: 'front',
  wireframe: false,
  wireframeLinewidth: 1,
  wireframeLinejoin: 'round',
  wireframeLinecap: 'round'
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  const meshGui = gui.addFolder('Mesh')
  meshGui.open()
  meshGui
    .add(mesh, 'selectMesh', ['cube', 'sphere', 'plane'])
    .onChange((val) => {
      scene.remove(cube)
      scene.remove(sphere)
      scene.remove(plane)
      switch (val) {
        case 'cube': {
          scene.add(cube)
          break
        }
        case 'sphere': {
          scene.add(sphere)
          break
        }
        case 'plane': {
          scene.add(plane)
          break
        }
      }
    })
  meshGui.addColor(mesh, 'color').onChange((val) => {
    meshMaterial.color.setHex(val)
  })
  meshGui.add(mesh, 'side', ['front', 'back', 'doublue']).onChange((val) => {
    meshMaterial.side =
      val === 'front'
        ? THREE.FrontSide
        : val === 'back'
        ? THREE.BackSide
        : THREE.DoubleSide
  })
  meshGui.addColor(mesh, 'emissive').onChange(val => {
    meshMaterial.emissive.setHex(val)
  })
  const transparentGui = meshGui.addFolder('transparent')
  transparentGui.open()
  transparentGui.add(mesh, 'transparent').onChange((val) => {
    meshMaterial.transparent = val
    meshMaterial.needsUpdate = true
  })
  transparentGui.add(mesh, 'opacity', 0, 1, 0.1).onChange((val) => {
    meshMaterial.opacity = val
    meshMaterial.needsUpdate = true
  })
  const wireframeGui = meshGui.addFolder('wireframe')
  wireframeGui.add(mesh, 'wireframe').onChange(val => {
    meshMaterial.wireframe = val
  })
  wireframeGui.add(mesh, 'wireframeLinewidth',0,10,0.1).name('lineWidth').onChange(val=>{
    meshMaterial.wireframeLinewidth = val
    meshMaterial.needsUpdate = true
  })
  wireframeGui.add(mesh, 'wireframeLinejoin',['round', 'bevel', 'miter']).name('lineJoin').onChange(val=>{
    meshMaterial.wireframeLinejoin = val
    meshMaterial.needsUpdate = true
  })
  wireframeGui.add(mesh, 'wireframeLinecap',['butt','round' ,'square']).name('lineCap').onChange(val=>{
    meshMaterial.wireframeLinecap = val
    meshMaterial.needsUpdate = true
  })

}

let step = 0.02
function animate() {
  cube.rotation.y += step
  sphere.rotation.y += step
  plane.rotation.y += step
}
function initMesh() {
  const groundGeo = new THREE.PlaneGeometry(100, 100, 4, 4)
  const groundMat = new THREE.MeshBasicMaterial({ color: 0x444444 })
  ground = new THREE.Mesh(groundGeo, groundMat)
  ground.rotation.x = -0.5 * PI
  scene.add(ground)

  const cubeGeo = new THREE.BoxGeometry(14, 14, 14)
  const sphereGeo = new THREE.SphereGeometry(7)
  const planeGeo = new THREE.PlaneGeometry(14, 14)
  cube = new THREE.Mesh(cubeGeo, meshMaterial)
  sphere = new THREE.Mesh(sphereGeo, meshMaterial)
  plane = new THREE.Mesh(planeGeo, meshMaterial)
  cube.position.set(0, 10, 0)
  sphere.position.set(0, 10, 0)
  plane.position.set(0, 10, 0)

  scene.add(cube)
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
