import * as THREE from 'three'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'
import { GUI } from 'dat.gui'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'

const { random, PI, floor, ceil, min, max, sin, cos } = Math

// 使用多材质和传入geo创建mesh（group）
export function createMesh(geo: THREE.BufferGeometry): THREE.Group {
  const mat1 = new THREE.MeshNormalMaterial({ side: THREE.DoubleSide })
  const mat2 = new THREE.MeshBasicMaterial({
    wireframe: true,
  })

  const mesh = createMultiMaterialObject(geo, [mat1, mat2])
  return mesh
}

// 设置物体中心点为标准中心点
export function setCenter(obj: THREE.Object3D) {
  const box = new THREE.Box3().setFromObject(obj)
  box.getCenter(obj.position)
  obj.position.multiplyScalar(-1)
}

export function getRandomVec3(min: number, max: number) {
  const p = new THREE.Vector3()

  p.x = random() * (max - min) + min
  p.y = random() * (max - min) + min
  p.z = random() * (max - min) + min
  return p
}

export function getRandomVec2(min: number, max: number) {
  const p = new THREE.Vector2()

  p.x = random() * (max - min) + min
  p.y = random() * (max - min) + min
  return p
}

export class CustomCurve extends THREE.Curve<THREE.Vector3> {
  points: THREE.Vector3[]
  constructor(points: THREE.Vector3[]) {
    super()
    this.points = points
  }

  interpolate(p0: number, p1: number, p2: number, p3: number, t: number) {
    const v0 = (p2 - p0) * 0.5
    const v1 = (p3 - p1) * 0.5
    const t2 = t * t
    const t3 = t * t2
    return (
      (2 * p1 - 2 * p2 + v0 + v1) * t3
      + (-3 * p1 + 3 * p2 - 2 * v0 - v1) * t2
      + v0 * t
      + p1
    )
  }

  getPoint(t: number, optionalTarget = new THREE.Vector3()) {
    const points = this.points
    const point = (points.length - 1) * t
    const intPoint = Math.floor(point)
    const weight = point - intPoint
    const point0 = points[intPoint == 0 ? intPoint : intPoint - 1]
    const point1 = points[intPoint]
    const point2
      = points[intPoint > points.length - 2 ? points.length - 1 : intPoint + 1]
    const point3
      = points[intPoint > points.length - 3 ? points.length - 1 : intPoint + 2]
    const vector = optionalTarget
    vector.x = this.interpolate(point0.x, point1.x, point2.x, point3.x, weight)
    vector.y = this.interpolate(point0.y, point1.y, point2.y, point3.y, weight)
    vector.z = this.interpolate(point0.z, point1.z, point2.z, point3.z, weight)
    return vector
  }
}

export function initThree(con: HTMLElement) {
  let { width, height } = con.getBoundingClientRect()
  const canvas = document.createElement('canvas')
  canvas.width = width
  canvas.height = height
  canvas.style.cssText = `display: block;width: ${width}px;height: ${height}px;`
  con.append(canvas)
  const scene = new THREE.Scene()
  scene.background = new THREE.Color(0x444444)
  const camera = new THREE.PerspectiveCamera(75, width / height, 1, 1000)
  const renderer = new THREE.WebGLRenderer({
    canvas,
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
  const stats = Stats()
  con.style.cssText = `position: relative`
  stats.dom.style.cssText = `position:absolute;top:0;left:0`
  con.append(stats.dom)

  // 轨道控制器
  const orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(0, 0, 100)
  orbitControls.update()

  function render(animate: () => void) {
    requestAnimationFrame(() => render(animate))
    stats.update()
    animate()
    renderer!.render(scene!, camera!)
  }

  function onWindowResize() {
    const { width: w, height: h } = con.getBoundingClientRect()
    width = w
    height = h
    camera.aspect = width / height
    camera.updateProjectionMatrix()
    renderer.setSize(width, height)
  }

  return { canvas, scene, camera, orbitControls, render }
}
