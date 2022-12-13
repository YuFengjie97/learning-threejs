<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * scene中所有mesh的材质将以overrideMaterial属性指定的材质重新渲染展示，
 * 可以发现outputObjects中输出的mesh材质都是原来定义的MeshLambertMaterial，
 * 但是实际展示的是overrideMaterial定义的材质
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

let planeMesh: THREE.Mesh

onMounted(() => {
  initTHREE()
  initMesh()
  initLight()

  controls.numberOfObjects = scene.children.length
  initGUI()
  render()
})

const controls = {
  rotationSpeed: 0.02,
  numberOfObjects: 0, // 初始化为0，具体数目要通过scene.child来确定
  removeCube() {
    const allChild = scene.children
    const lastChild = allChild[allChild.length-1]
    if(lastChild instanceof THREE.Mesh){
      scene.remove(lastChild)
      this.numberOfObjects --
    }
  },
  addCube() {
    const cubeSize = random() * 3
    const cubeGeo = new THREE.BoxGeometry(cubeSize, cubeSize, cubeSize)
    const cubeMat = new THREE.MeshLambertMaterial({
      color: random() * 0xffffff,
    })
    const cube = new THREE.Mesh(cubeGeo, cubeMat)
    cube.castShadow = true
    cube.name = 'cube-' + scene.children.length

    // prettier-ignore
    const {width: planeWidth, height: planeHeight} = (planeMesh.geometry as THREE.PlaneGeometry).parameters
    cube.position.x = -planeWidth / 2 + random() * planeWidth
    cube.position.y = floor(random() * 3) + 3
    cube.position.z = -planeHeight / 2 + random() * planeHeight

    scene.add(cube)
    this.numberOfObjects ++
  },
  outputObjects() {
    console.log(scene.children);
  },
}

function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
  gui.add(controls, 'rotationSpeed', 0, 0.5)
  gui.add(controls, 'addCube')
  gui.add(controls, 'removeCube')
  gui.add(controls, 'outputObjects')
  gui.add(controls, 'numberOfObjects').listen() // 如果不listen，该值改变后，dom中的内容就不会改变，类似响应式
}

function initMesh() {
  const planeGeo = new THREE.PlaneGeometry(60, 20, 1, 1)
  const planeMat = new THREE.MeshLambertMaterial({ color: 0xffffff })
  planeMesh = new THREE.Mesh(planeGeo, planeMat)
  planeMesh.receiveShadow = true
  planeMesh.rotation.x = -0.5 * PI
  planeMesh.position.set(0, 0, 0)
  scene.add(planeMesh)
}

function initLight() {
  const spotLight = new THREE.SpotLight(0xffffff)
  spotLight.position.set(-20 ,20 , 0)
  spotLight.castShadow = true
  const helper = new THREE.SpotLightHelper(spotLight)
  scene.add(helper)
  scene.add(spotLight)
}

function animate() {
  scene.traverse(function (e) {
    if (e instanceof THREE.Mesh && e !== planeMesh) {
      e.rotation.x += controls.rotationSpeed
      e.rotation.y += controls.rotationSpeed
      e.rotation.z += controls.rotationSpeed
    }
  })
}
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
  scene.overrideMaterial = new THREE.MeshLambertMaterial({color: 0xffffff})
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
