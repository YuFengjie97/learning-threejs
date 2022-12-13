<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 原先教程里的很多api已经被弃用了。
 * 这个例子基本上算是重写了。
 * 首先定义顶点数组，vertex，一共8个，
 * 然后是由顶点构成的面，这里使用getVertices去转换，并且声明为Float32Array。因为bufferGeomotry需要用这种格式来定义顶点坐标。(very fast)
 * 面为三角形，一共前后左右上下，6个面，分为三角形，为12个面，每个三角形面，分别对应3组顶点坐标，共36组
 * gui的对顶点的控制：通过改变vertex来重新生成每个三角面的顶点坐标
 * 
 * 重要：
 * e.geometry.setAttribute('position', new THREE.BufferAttribute(getVertices(), 3))
 * e.geometry.attributes.position.needsUpdate = true
 * 
 * 关于相机移动到面的另一侧，面会消失掉：
 * 将面的材质属性side设置为THREE.DoubleSide
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'

import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'

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
let customGeo: THREE.BufferGeometry
let customMesh: THREE.Group

onMounted(() => {
  initTHREE()
  initMesh()
  initLight()

  initGUI()
  render()
})

const s = 5
// prettier-ignore
const vertex = [
  [-s, 2*s,  s],
  [-s, 0,    s],
  [s,  0,    s],
  [s,  2*s,  s],
  [s,  2*s, -s],
  [s,  0,   -s],
  [-s, 2*s, -s],
  [-s, 0,   -s]
]

function getVertices() {
  // prettier-ignore
  return new Float32Array([
  // 前
  ...vertex[0],
  ...vertex[1],
  ...vertex[2],
  ...vertex[2],
  ...vertex[3],
  ...vertex[0],
   // 右
  ...vertex[3],
  ...vertex[4],
  ...vertex[2],
  ...vertex[4],
  ...vertex[5],
  ...vertex[2],
   // 后
  ...vertex[6],
  ...vertex[5],
  ...vertex[4],
  ...vertex[6],
  ...vertex[7],
  ...vertex[5],
   //左
  ...vertex[0],
  ...vertex[6],
  ...vertex[1],
  ...vertex[6],
  ...vertex[7],
  ...vertex[1],
    //上
  ...vertex[6],
  ...vertex[0],
  ...vertex[3],
  ...vertex[3],
  ...vertex[4],
  ...vertex[6],
   //下
  ...vertex[1],
  ...vertex[2],
  ...vertex[5],
  ...vertex[5],
  ...vertex[7],
  ...vertex[1]
  ])
}

function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  for (let i = 0; i < 8; i++) {
    const folder = gui.addFolder(`vertex${i}`)
    for (let j = 0; j < 3; j++) {
      // prettier-ignore
      folder.add(vertex[i], `${j}`,-20,20,1).name(j===0?'x':j===1?'y':'z').onChange(val=>{
        customMesh.children.forEach(e=>{
          if(e instanceof THREE.Mesh){
            e.geometry.setAttribute('position', new THREE.BufferAttribute(getVertices(), 3))
            e.geometry.attributes.position.needsUpdate = true
          }
        })
      })
    }
  }
}

function initMesh() {
  const planeGeo = new THREE.PlaneGeometry(60, 40, 1, 1)
  const planeMat = new THREE.MeshLambertMaterial({ color: 0xffffff, side: THREE.DoubleSide })
  planeMesh = new THREE.Mesh(planeGeo, planeMat)
  planeMesh.receiveShadow = true
  planeMesh.rotation.x = -0.5 * PI
  planeMesh.position.set(0, 0, 0)
  scene.add(planeMesh)

  // 定义自定义物体
  customGeo = new THREE.BufferGeometry()
  customGeo.setAttribute(
    'position',
    new THREE.BufferAttribute(getVertices(), 3)
  )
  const materials = [
    // 面材质
    new THREE.MeshLambertMaterial({
      opacity: 0.6,
      color: 0x6c5ce7,
      transparent: true,
      side: THREE.DoubleSide
    }),
    // 线材质
    new THREE.MeshBasicMaterial({ color: 0xe84393, wireframe: true }),
  ]
  customMesh = createMultiMaterialObject(customGeo, materials)
  customMesh.children.forEach(function (e) {
    e.castShadow = true
  })
  // mesh.translateZ(s / 2)
  // mesh.translateY(s / 2)
  // mesh.translateX(-s / 2)
  scene.add(customMesh)
}

function initLight() {
  initSpotLight(-20,20,-0)
}
function initSpotLight(x: number,y: number,z: number){
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
