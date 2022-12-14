<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 添加一个小球作为相机焦点，相机焦点随小球运动
 * 这里没有直接使用camera，而是使用轨道控制器。
 * 在修改轨道控制器焦点（也就是相机焦点）后，需要重新调用update方法来更新
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
let camera: THREE.PerspectiveCamera | THREE.OrthographicCamera
let renderer: THREE.WebGLRenderer

let planeMesh: THREE.Mesh
let lookAtMesh: THREE.Mesh

onMounted(() => {
  initTHREE()
  initMesh()
  initLight()

  initGUI()
  render()
})

const controls = {
  switchCamera: ()=>{},
  perspective: 'Perspective',
}

function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(controls, 'switchCamera').onChange(()=>{
    if (camera instanceof THREE.PerspectiveCamera) {
      camera = new THREE.OrthographicCamera(
        window.innerWidth / -16,
        window.innerWidth / 16,
        window.innerHeight / 16,
        window.innerHeight / -16,
        -200,
        500
      )
      initOrbitControls()
      controls.perspective = 'Orthographic'
    } else {
      camera = new THREE.PerspectiveCamera(45, width / height, 1, 1000)
      initOrbitControls()
      controls.perspective = 'Perspective'
    }
    gui.__controllers.forEach(c=>c.updateDisplay())
  })
  gui.add(controls, 'perspective')
}

function initMesh() {
  const planeGeo = new THREE.PlaneGeometry(180, 180, 1, 1)
  const planeMat = new THREE.MeshLambertMaterial({
    color: 0xffffff,
    side: THREE.DoubleSide,
  })
  planeMesh = new THREE.Mesh(planeGeo, planeMat)
  planeMesh.receiveShadow = true
  planeMesh.rotation.x = -0.5 * PI
  planeMesh.position.set(0, 0, 0)
  scene.add(planeMesh)

  const geo = new THREE.BoxGeometry(4, 4, 4)
  const { width: planeW, height: planeH } = planeGeo.parameters
  for (let i = 0; i < planeW / 5; i++) {
    for (let j = 0; j < planeH / 5; j++) {
      const rnd = Math.random() * 0.75 + 0.25
      const color = new THREE.Color(rnd, 0, 0)
      const mat = new THREE.MeshLambertMaterial({ color })
      const mesh = new THREE.Mesh(geo, mat)
      mesh.position.set(i * 5 - planeW / 2 + 2, 2, j * 5 - planeH / 2 + 2)
      scene.add(mesh)
    }
  }
  const lookAtGeo = new THREE.SphereGeometry(2)
  lookAtMesh = new THREE.Mesh(lookAtGeo,new THREE.MeshLambertMaterial({color:0xfdcb6e}))
  lookAtMesh.position.set(0, 10, 0)
  scene.add(lookAtMesh)
}

function initLight() {
  initSpotLight(-120, 50, -0)
}
function initSpotLight(x: number, y: number, z: number) {
  const spotLight = new THREE.SpotLight(0xffffff)
  spotLight.position.set(x, y, z)
  spotLight.castShadow = true
  const helper = new THREE.SpotLightHelper(spotLight)
  scene.add(helper)
  scene.add(spotLight)
}

let step = 0
function animate() {
  step += 0.02
  let x = 10 + 100 * sin(step)
  lookAtMesh.position.x = x
  orbitControls.target = lookAtMesh.position
  orbitControls.update()
}
// 绘制
function render() {
  requestAnimationFrame(render)
  stats.update()
  animate()
  renderer.render(scene, camera)
}
function initOrbitControls() {
  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(-150, 50, 150)
  // camera.lookAt(new THREE.Vector3(0, 0, 0))
  // camera.position.set(-50, 20, 20)
  orbitControls.update()
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

  initOrbitControls()
}
// 自适应
function onWindowResize() {
  width = window.innerWidth
  height = window.innerHeight
  if (camera instanceof THREE.PerspectiveCamera) {
    camera.aspect = width / height
    camera.updateProjectionMatrix()
  }
  renderer.setSize(width, height)
}
</script>
