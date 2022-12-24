<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * mtl文件加载
 * obj文件是模型，也就是Object3D,如果模型比较简单，可能只是个mesh，复杂点就是个Group
 * mtl文件是材质，有些在加载后重新设置材质属性，不然在场景中不可见
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import butterflyObj from '@/assets/models/butterfly/butterfly.obj?url'
import butterflyMtl from '@/assets/models/butterfly/butterfly.mtl?url'

import { OBJLoader } from 'three/examples/jsm/loaders/OBJLoader'
import { MTLLoader } from 'three/examples/jsm/loaders/MTLLoader'

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

let model: THREE.Object3D

onMounted(async() => {
  initGUI()
  initTHREE()
  initLight()
  await initMesh()
  render()
})

const params = {}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
}

function animate() {
  model.rotation.y += 0.01
}

function modelLoad(mtlUrl: string, objUrl: string): Promise<THREE.Object3D>{
  const mtlLoader = new MTLLoader()
  const objLoader = new OBJLoader()

  return new Promise((resolve,reject)=>{
    mtlLoader.load(mtlUrl, function(mats){
      objLoader.setMaterials(mats).load(objUrl, function(object){
            const wing1 = object.children[4];
            const wing2 = object.children[5];

            const mat1 = (wing1 as THREE.Mesh).material as THREE.MeshPhongMaterial
            const mat2 = (wing2 as THREE.Mesh).material as THREE.MeshPhongMaterial

            mat1.opacity = 0.6;
            mat1.transparent = true;
            mat1.depthTest = false;
            mat1.side = THREE.DoubleSide;
            mat2.opacity = 0.6;
            mat2.depthTest = false;
            mat2.transparent = true;
            mat2.side = THREE.DoubleSide;

            object.rotation.x = 0.2;
            object.rotation.y = -1.3;

            resolve(object)
      })
    })
  })
  
}

async function initMesh(){
  model = await modelLoad(butterflyMtl, butterflyObj)
  model.position.set(0,40,0)
  model.scale.multiplyScalar(100)
  scene.add(model)

}
function initLight() {
  const light = new THREE.PointLight(0xffffff, 3)
  light.position.set(-10,50,0)
  scene.add(light)
}
// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0x444444)
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
  orbitControls.target = new THREE.Vector3(0, 40, 0)
  orbitControls.object.position.set(0, 40, 60)
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
