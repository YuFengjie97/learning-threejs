<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 对之前版本的重写，上个版本在gui修改参数后，是直接将原有的mesh删除，然后重写定义geo与mat创建新的mesh
 * 这个版本是动态修改基准点，且只原有mesh的geo进行处理
 * 将lathe的半横截面基准点使用闭包保存
 * updateSphere会根据gui参数与当前sphereMesh数量情况进行判断补全处理
 * height & pointNum的修改会导致新的基准点生成
 * gui所有参数的修改会导致latheGeo重新定义
 * updateLathe会在原有mesh（group）上对geo进行丢弃，和重新赋值
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
let mesh: THREE.Group // sphere与lathe添加于此
let spheres: Array<THREE.Mesh> = [] //点
let lathe: THREE.Group // lathe

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

  gui.add(params, 'height', 1, 20, 1).onChange(() => {
    updateSpheres()
    updateLathe()
  })
  gui.add(params, 'pointNum', 1, 50, 1).onChange(() => {
    updateSpheres()
    updateLathe()
  })
  gui.add(params, 'segments', 1, 20, 1).onChange((val) => updateLathe())
  gui.add(params, 'phiStart', 0, PI * 2, 0.01).onChange(() => updateLathe())
  gui.add(params, 'phiLength', 0, PI * 2, 0.01).onChange(() => updateLathe())
}

let step = 0
function animate() {
  mesh.rotation.x = step += 0.005
  mesh.rotation.y = step += 0.005
}
function initMesh() {
  mesh = new THREE.Group()
  scene.add(mesh)

  // 点
  let points = getLathePoints()()
  points.forEach((p) => {
    initSphere(p)
  })

  // lathe
  const latheGeo = getLatheGeo()
  lathe = createMesh(latheGeo)
  mesh.add(lathe)
}

// 初始化点sphere
function initSphere(p: THREE.Vector2) {
  const sphereGeo = new THREE.SphereGeometry(0.4)
  const sphereMat = new THREE.MeshLambertMaterial({ color: 0xff0000 })
  const sphere = new THREE.Mesh(sphereGeo, sphereMat)
  sphere.position.set(p.x, p.y, 0)
  spheres.push(sphere)
  mesh.add(sphere)
}
// lathe基准点计算，根据height与pointNum获取半横截面点位置
function getLathePoints() {
  // 使用闭包将points位置信息存储在内存
  let points: Array<THREE.Vector2> = []
  return function () {
    points.length = 0
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
}
// 根据gui参数初始化latheGeo
function getLatheGeo() {
  const points = getLathePoints()()
  const { segments, phiStart, phiLength } = params
  return new THREE.LatheGeometry(points, segments, phiStart, phiLength)
}

/** 根据points来更新sphere位置与是否显示
 * sphere在gui控制中，数量有一次达到最大值，在scene中就会一直存在最大数量的sphere
 * 当gui控制数量小于sphere数量时，多余的sphere就会隐藏
 */
function updateSpheres() {
  let points = getLathePoints()()
  points.forEach((p, i) => {
    if (i < spheres.length) {
      // 对已存在的sphere更新位置
      spheres[i].position.set(p.x, p.y, 0)
      spheres[i].visible = true
    } else {
      // 对不存在的sphere进行初始化，并设置位置
      initSphere(p)
    }
  })
  // 对用不到的sphere隐藏处理
  for (let i = points.length, len = spheres.length; i < len; i++) {
    spheres[i].visible = false
  }
}

// latheMesh，弃用原来的geo，并使用新的geo
function updateLathe(){
  let geoNew = getLatheGeo()
  // 因为lathe是使用createMesh创造的双材质mesh（group）
  ;(lathe.children[0] as THREE.Mesh).geometry.dispose()
  ;(lathe.children[1] as THREE.Mesh).geometry.dispose()
  ;(lathe.children[0] as THREE.Mesh).geometry = geoNew
  ;(lathe.children[1] as THREE.Mesh).geometry = geoNew
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
