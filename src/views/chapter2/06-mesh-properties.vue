<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
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

let planeMesh: THREE.Mesh
let cubeMesh: THREE.Mesh

onMounted(() => {
  initTHREE()
  initMesh()
  initLight()

  initGUI()
  render()
})

const controls = {
  scaleX: 1,
  scaleY: 1,
  scaleZ: 1,
  positionX: 0,
  positionY: 4,
  positionZ: 0,
  rotationX: 0,
  rotationY: 0,
  rotationZ: 0,
  translateX: 0,
  translateY: 0,
  translateZ: 0,
  handleTranslate: () => {},
  visible: true,
}

function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
  gui.add(controls, 'visible').onChange((val) => {
    cubeMesh.visible = val
  })
  const folder1 = gui.addFolder('scale')
  folder1.open()
  folder1.add(controls, 'scaleX', 0, 5, 0.1).onChange((val) => {
    cubeMesh.scale.x = val
  })
  folder1.add(controls, 'scaleY', 0, 5, 0.1).onChange((val) => {
    cubeMesh.scale.y = val
  })
  folder1.add(controls, 'scaleZ', 0, 5, 0.1).onChange((val) => {
    cubeMesh.scale.z = val
  })
  const folder2 = gui.addFolder('position')
  folder2.open()
  folder2.add(controls, 'positionX', -10, 10, 0.1).onChange((val) => {
    cubeMesh.position.x = val
  })
  folder2.add(controls, 'positionY', -10, 10, 0.1).onChange((val) => {
    cubeMesh.position.y = val
  })
  folder2.add(controls, 'positionZ', -10, 10, 0.1).onChange((val) => {
    cubeMesh.position.z = val
  })
  const folder3 = gui.addFolder('rotate')
  folder3.open()
  folder3.add(controls, 'rotationX', 0, 5, 0.1).onChange((val) => {
    cubeMesh.rotation.x = val
  })
  folder3.add(controls, 'rotationY', 0, 5, 0.1).onChange((val) => {
    cubeMesh.rotation.y = val
  })
  folder3.add(controls, 'rotationZ', 0, 5, 0.1).onChange((val) => {
    cubeMesh.rotation.z = val
  })
  const folder4 = gui.addFolder('translate')
  folder4.open()
  folder4.add(controls, 'translateX', -10, 10, 0.1)
  folder4.add(controls, 'translateY', -10, 10, 0.1)
  folder4.add(controls, 'translateZ', -10, 10, 0.1)
  folder4.add(controls, 'handleTranslate').onChange((val) => {
    /**
     *这里为了在translate后更新datgui中dom显示的position的信息，
     *使用onChange,而不是listen().
     */
    const { translateX, translateY, translateZ } = controls
    cubeMesh.translateX(translateX)
    cubeMesh.translateY(translateY)
    cubeMesh.translateZ(translateZ)
    controls.positionX = cubeMesh.position.x
    controls.positionY = cubeMesh.position.y
    controls.positionZ = cubeMesh.position.z
    folder2.__controllers.forEach((c) => c.updateDisplay())
  })
}

function initMesh() {
  const planeGeo = new THREE.PlaneGeometry(60, 40, 1, 1)
  const planeMat = new THREE.MeshLambertMaterial({
    color: 0xffffff,
    side: THREE.DoubleSide,
  })
  planeMesh = new THREE.Mesh(planeGeo, planeMat)
  planeMesh.receiveShadow = true
  planeMesh.rotation.x = -0.5 * PI
  planeMesh.position.set(0, 0, 0)
  scene.add(planeMesh)

  const cubeGeo = new THREE.BoxGeometry(5, 8, 3)
  const cubeMaterial = new THREE.MeshLambertMaterial({ color: 0x0984e3 })
  cubeMesh = new THREE.Mesh(cubeGeo, cubeMaterial)
  cubeMesh.position.set(0, 4, 0)
  scene.add(cubeMesh)
}

function initLight() {
  initSpotLight(-20, 20, -0)
}
function initSpotLight(x: number, y: number, z: number) {
  const spotLight = new THREE.SpotLight(0xffffff)
  spotLight.position.set(x, y, z)
  spotLight.castShadow = true
  const helper = new THREE.SpotLightHelper(spotLight)
  scene.add(helper)
  scene.add(spotLight)
}

function animate() {}
// 绘制
function render() {
  requestAnimationFrame(render)
  stats.update()
  animate()
  renderer.render(scene, camera)
}
// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  // scene.background = new THREE.Color(0x444444)
  // scene.fog = new THREE.Fog(0xffffff, 1, 100);

  camera = new THREE.PerspectiveCamera(45, width / height, 1, 1000)
  renderer = new THREE.WebGLRenderer({
    canvas: canvasDom.value,
    antialias: true,
  })
  renderer.setClearColor(0x444444, 1.0)
  renderer.setSize(width, height)
  renderer.shadowMap.enabled = true
  renderer.setPixelRatio(window.devicePixelRatio) // 不推荐
  window.addEventListener('resize', onWindowResize)

  // 坐标轴
  scene.add(new THREE.AxesHelper(1000))

  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)

  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(-50, 20, 20)
  // camera.lookAt(new THREE.Vector3(0, 0, 0))
  // camera.position.set(-50, 20, 20)
  orbitControls.update()
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
