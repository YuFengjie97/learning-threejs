<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<style lang="less" scoped>
.viewCon {
  .canvasCon {
    position: relative;
  }
}
</style>

<script lang="ts" setup>
/**
 * PDB文件载入后，会得到一个有两个属性（geometryAtoms,geometryBonds）的对象,
 * 这两个属性分别记录了原子的顶点位置颜色与原子之间的链接
 * 需要自己手动解析配置材质
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { PDBLoader } from 'three/examples/jsm/loaders/PDBLoader'
import { PDB } from 'three/examples/jsm/loaders/PDBLoader'
import { CSS2DObject } from 'three/examples/jsm/renderers/CSS2DRenderer'
import { CSS2DRenderer } from 'three/examples/jsm/renderers/CSS2DRenderer'

import modelUrl from '@/assets/models/caffeine.pdb?url'
import { send } from 'vite'

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
let labelRenderer: CSS2DRenderer

let model: THREE.Group = new THREE.Group()

onMounted(async () => {
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

const loader = new PDBLoader()
function modelLoad(modelUrl: string): Promise<PDB> {
  return new Promise((resolve, reject) => {
    loader.load(modelUrl, resolve)
  })
}

async function initMesh() {
  const posScl = 50
  const sizeScl = 10
  scene.add(model)

  const { geometryAtoms, geometryBonds, json } = await modelLoad(modelUrl)
  let positions = geometryAtoms.getAttribute('position')
  const colors = geometryAtoms.getAttribute('color')

  const pos = new THREE.Vector3()
  const col = new THREE.Color()
  const sphereGeometry = new THREE.IcosahedronGeometry(1, 3) //原子使用的几何体
  const boxGeometry = new THREE.BoxGeometry(1, 1, 1) // 原子链接使用的几何体

  for (let i = 0; i < positions.count; i++) {
    // 设置原子的位置颜色大小
    // 看上去bufferAttribute会自动帮你区分xyz，不用自己手动计算索引
    pos.x = positions.getX(i)
    pos.y = positions.getY(i)
    pos.z = positions.getZ(i)
    col.r = colors.getX(i)
    col.g = colors.getY(i)
    col.b = colors.getZ(i)

    const mat = new THREE.MeshPhongMaterial({ color: col })
    const mesh = new THREE.Mesh(sphereGeometry, mat)
    mesh.position.copy(pos)
    mesh.position.multiplyScalar(posScl)
    mesh.scale.multiplyScalar(sizeScl)
    model.add(mesh)

    // 使用css2d添加原子文本标识
    const atom = json.atoms[i]
    const text = document.createElement('div')
    text.style.cssText = `
      color: rgb(${atom[3][0]},${atom[3][1]},${atom[3][2]});
      text-shadow: -1px 1px 1px rgb(0,0,0);
      margin-left: 25px;
      font-size: 20px;`
    text.textContent = atom[4]
    const label = new CSS2DObject(text)
    label.position.copy(mesh.position)
    model.add(label)
  }

  // 初始化原子之间的链接
  positions = geometryBonds.getAttribute('position')
  const start = new THREE.Vector3()
  const end = new THREE.Vector3()
  for (let i = 0; i < positions.count; i += 2) {
    start.x = positions.getX(i)
    start.y = positions.getY(i)
    start.z = positions.getZ(i)

    end.x = positions.getX(i + 1)
    end.y = positions.getY(i + 1)
    end.z = positions.getZ(i + 1)

    start.multiplyScalar(posScl)
    end.multiplyScalar(posScl)

    const link = new THREE.Mesh(
      boxGeometry,
      new THREE.MeshPhongMaterial({ color: 0xffffff })
    )
    link.position.copy(start)
    link.position.lerp(end, 0.5)
    link.scale.set(5, 5, start.distanceTo(end))
    link.lookAt(end)
    model.add(link)
  }
}
function initLight() {
  const hemiLight = new THREE.HemisphereLight(0xffffff, 0x000000, 1)
  scene.add(hemiLight)
  const light = new THREE.PointLight(0xff0000, 2)
  light.position.set(50, 50, 50)
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
  renderer.shadowMap.enabled = true
  renderer.setPixelRatio(window.devicePixelRatio)

  // 原子的文本标记需要这个renderer，有点类似一个新图层的感觉
  labelRenderer = new CSS2DRenderer()
  labelRenderer.setSize(window.innerWidth, window.innerHeight)
  labelRenderer.domElement.style.position = 'absolute'
  labelRenderer.domElement.style.top = '0px'
  labelRenderer.domElement.style.pointerEvents = 'none'
  canvasCon.value?.append(labelRenderer.domElement)

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
  orbitControls.object.position.set(0, 0, 400)
  orbitControls.update()
}
// 绘制
function render() {
  requestAnimationFrame(render)
  stats.update()
  animate()
  renderer.render(scene, camera)
  labelRenderer.render(scene, camera)
}
// 自适应
window.addEventListener('resize', onWindowResize)
function onWindowResize() {
  width = window.innerWidth
  height = window.innerHeight
  camera.aspect = width / height
  camera.updateProjectionMatrix()
  renderer.setSize(width, height)
  labelRenderer.setSize(width, height)
}
</script>
