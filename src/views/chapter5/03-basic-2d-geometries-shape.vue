<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 跟原来的例子不同
 * 我是，在一开始就在场景里添加了shapeMesh和shapeLine
 * shapeMesh使用createMultiMaterialObject使用多个材质展示，返回的是group
 * 
 * shapeLine是THREE.line,首先通过shape.extractPoints获取该shape的形状边缘和空洞边缘的所有点
 * shapeLine的geo使用的是bufferGeo，对应的数据数组是Float32Array格式，使用了缓存机制来更新点坐标
 * 
 * const geo = new THREE.BufferGeometry().setAttribute(
 *   'position',
 *  new THREE.BufferAttribute(points, 2)
 * )通过这种形式，为geo设置position属性，并以二元组的形式来解析points
 * 
 * 我是预先计算了笑脸形状在精度为20（gui控制里的最大精度）时的点的数量，因为Float32Array在初始化时，需要确定长度
 * updatePoints重新以精度来获取点坐标更新points，如果新的点长度比原来短，要把points用不到的点坐标置为0
 * 然后通过shapeLine.geometry.attributes.position.needsUpdate = true告知渲染器，该mesh的几何属性position需要更新
 * 
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

let smileyShape: THREE.Shape
let shapeMesh: THREE.Group
let shapeLine: THREE.Line
const points = new Float32Array(408)
let shapeGroup: THREE.Group

onMounted(() => {
  initTHREE()
  initMesh()
  initGUI()
  render()
})

const params = {
  rotateSpeed: 0.01,
  showShape: true,
  divisions: 10, //通过shape取点的精度
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params, 'rotateSpeed', 0, 2, 0.01)
  gui.add(params, 'showShape').onChange((val) => {
    shapeMesh.visible = val
    shapeLine.visible = !val
  })
  // 笑脸shape最大精度20，会有204个点，对应float32Array长度应该是408
  gui.add(params, 'divisions', 1, 20, 1).onChange((val) => {
    updatePoints(smileyShape, val)
  })
}

let step = 0
function animate() {
  shapeGroup.rotation.y = step+=params.rotateSpeed
  // shapeLine.rotation.y = step+=params.rotateSpeed
}
function initMesh() {
  shapeGroup = new THREE.Group()
  shapeGroup.rotation.z = PI
  scene.add(shapeGroup)

  smileyShape = initSmileyShape()
  shapeMesh = initShpaeMesh(smileyShape)
  shapeLine = initShapeLine(smileyShape)
}

let scale = 0.1
function initShpaeMesh(s: THREE.Shape) {
  const geo = new THREE.ShapeGeometry(smileyShape)
  const matNormal = new THREE.MeshNormalMaterial({ side: THREE.DoubleSide })
  const matBasic = new THREE.MeshBasicMaterial({
    wireframe: true,
  })
  const mesh = createMultiMaterialObject(geo, [matBasic, matNormal])
  mesh.scale.multiplyScalar(scale)
  
  setCenter(mesh)
  shapeGroup.add(mesh)

  return mesh
}
function initShapeLine(s: THREE.Shape) {
  updatePoints(s, params.divisions)

  const geo = new THREE.BufferGeometry().setAttribute(
    'position',
    new THREE.BufferAttribute(points, 2)
  )
  const lineMat = new THREE.LineBasicMaterial({ color: 0xff3333 })
  const line = new THREE.Line(geo, lineMat)
  line.scale.multiplyScalar(scale)

  setCenter(line)
  shapeGroup.add(line)

  line.visible = false
  return line
}

// 设置mesh几何中心点
function setCenter(obj: THREE.Object3D){
  const box = new THREE.Box3().setFromObject(obj)
  box.getCenter(obj.position)
  obj.position.multiplyScalar(-1)
}

// 获取一个shape上的形状边缘与孔洞的所有点,并将更新points
function updatePoints(s: THREE.Shape, divisions: number) {
  let shapePoints = []
  const { shape, holes } = s.extractPoints(divisions)
  shapePoints.push(...shape)
  holes.forEach((h) => {
    shapePoints.push(...h)
  })
  shapePoints.forEach((p, i) => {
    points[i * 2] = p.x
    points[i * 2 + 1] = p.y
  })

  for (let i = shapePoints.length * 2; i < 408; i++) {
    points[i] = 0 // 用不到的点置空
  }
  if (shapeLine) shapeLine.geometry.attributes.position.needsUpdate = true

}
function initSmileyShape() {
  const smileyShape = new THREE.Shape()
    .moveTo(80, 40)
    .absarc(40, 40, 40, 0, Math.PI * 2, false)

  const smileyEye1Path = new THREE.Path()
    .moveTo(35, 20)
    .absellipse(25, 20, 10, 10, 0, Math.PI * 2, true, 0)

  const smileyEye2Path = new THREE.Path()
    .moveTo(65, 20)
    .absarc(55, 20, 10, 0, Math.PI * 2, true)

  const smileyMouthPath = new THREE.Path()
    .moveTo(20, 40)
    .quadraticCurveTo(40, 60, 60, 40)
    .bezierCurveTo(70, 45, 70, 50, 60, 60)
    .quadraticCurveTo(40, 80, 20, 60)
    .quadraticCurveTo(5, 50, 20, 40)

  smileyShape.holes.push(smileyEye1Path)
  smileyShape.holes.push(smileyEye2Path)
  smileyShape.holes.push(smileyMouthPath)
  return smileyShape
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
  // scene.add(new THREE.AmbientLight(0xffffff))

  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)

  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(0, 3, 20)
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
