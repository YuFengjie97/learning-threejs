<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" @pointermove="pointerMove" />
    </div>
  </div>
</template>

<script lang="ts" setup>
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

let cube: THREE.Mesh
let sphere: THREE.Mesh
let bar: THREE.Mesh
const meshGroup = new THREE.Group() // 射线检测仅针对该group

const raycaster = new THREE.Raycaster()
const rayVertexes = new Float32Array(6) //起点终点3元组
let rayGeo: THREE.BufferGeometry
let rayMesh: THREE.Line

onMounted(() => {
  initGUI()
  initTHREE()
  initLight()
  initMesh()
  render()
})

function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
}

let rotateSpeed = 0.01
let bouncingStep = 0
let bouncingSpeed = 0.05
let scalingStep = 0
let scalingSpeed = 0.1
function animate() {
  cube.rotation.x += rotateSpeed
  cube.rotation.y += rotateSpeed

  bouncingStep += bouncingSpeed
  sphere.position.x = 40 + 10 * Math.cos(bouncingStep)
  sphere.position.y = 2 + 10 * Math.abs(Math.sin(bouncingStep))

  scalingStep += scalingSpeed
  const scaleX = Math.abs(Math.sin(scalingStep / 4))
  const scaleY = Math.abs(Math.cos(scalingStep / 5))
  const scaleZ = Math.abs(Math.sin(scalingStep / 7))
  bar.scale.set(scaleX, scaleY, scaleZ)

  updateRay()
}
function initMesh() {
  scene.add(meshGroup)
  const plane = new THREE.Mesh(
    new THREE.PlaneGeometry(100, 100),
    new THREE.MeshLambertMaterial({
      color: 0x444444,
    })
  )
  plane.rotation.x = -PI / 2
  plane.receiveShadow = true
  meshGroup.add(plane)

  cube = new THREE.Mesh(
    new THREE.BoxGeometry(10, 10, 10),
    new THREE.MeshLambertMaterial({
      color: 0xff0000,
    })
  )
  cube.castShadow = true
  cube.position.set(-40, 5, 0)
  meshGroup.add(cube)

  sphere = new THREE.Mesh(
    new THREE.SphereGeometry(5),
    new THREE.MeshLambertMaterial({
      color: 0x00ff00,
    })
  )
  sphere.position.set(40, 5, 0)
  sphere.castShadow = true
  meshGroup.add(sphere)

  bar = new THREE.Mesh(
    new THREE.CylinderGeometry(2, 2, 20),
    new THREE.MeshLambertMaterial({
      color: 0x0000ff,
    })
  )
  bar.position.set(0, 10, 0)
  bar.castShadow = true
  meshGroup.add(bar)

  rayGeo = new THREE.BufferGeometry()
  rayGeo.setAttribute('position', new THREE.BufferAttribute(rayVertexes, 3))
  rayMesh = new THREE.Line(
    rayGeo,
    new THREE.MeshBasicMaterial({ color: 0xff0000 })
  )
  scene.add(rayMesh)
}

const rayPointer = new THREE.Vector2()
// 对鼠标位置进行规格化
function pointerMove(e: PointerEvent) {
  rayPointer.x = (e.clientX / window.innerWidth) * 2 - 1
  rayPointer.y = -(e.clientY / window.innerHeight) * 2 + 1
}

let detectObj: THREE.Mesh<THREE.BufferGeometry, THREE.MeshLambertMaterial> // 射线选中的obj
const choseColor = new THREE.Color(0xffeaa7) //选中时的颜色
const saveColor = new THREE.Color() // 保存选中前的颜色
function updateRay() {
  raycaster.setFromCamera(rayPointer, camera)
  
  rayVertexes[3] = camera.position.x
  rayVertexes[4] = camera.position.y-1
  rayVertexes[5] = camera.position.z
  rayGeo.attributes.position.needsUpdate = true

  const intersects = raycaster.intersectObjects(meshGroup.children, false) // 射线检测只针对meshgroup中的物体，并且不会检查物体的后代
  if (intersects.length > 0) {
    // prettier-ignore
    const currentObj = intersects[0].object as THREE.Mesh<THREE.BufferGeometry,THREE.MeshLambertMaterial>
    const rayPos = intersects[0].point
    rayVertexes[0] = rayPos.x
    rayVertexes[1] = rayPos.y
    rayVertexes[2] = rayPos.z
    
    if (detectObj !== currentObj) {
      //检测到，并且和之前的不是同一个
      if (detectObj) detectObj.material.color.set(saveColor) // 之前的还原
      // 同步当前选中的mesh
      detectObj = currentObj
      saveColor.set(detectObj.material.color)
    } 
      detectObj.material.color.set(choseColor)
  } else {
    // 没检测到，还原之前的mesh
    if (detectObj) {
      detectObj.material.color.set(saveColor)
    }
  }
}
function initLight() {
  let light = new THREE.SpotLight(0xffffff, 2)
  light.position.set(-40, 40, 40)
  light.castShadow = true
  light.lookAt(new THREE.Vector3(0, 0, 0))
  let lightHelper = new THREE.SpotLightHelper(light)

  scene.add(light)
  scene.add(lightHelper)
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
  renderer.shadowMap.enabled = true
  // renderer.setPixelRatio(window.devicePixelRatio) // 不推荐
  window.addEventListener('resize', onWindowResize)

  // 坐标轴
  scene.add(new THREE.AxesHelper(1000))

  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)

  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(50, 50, 50)
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
