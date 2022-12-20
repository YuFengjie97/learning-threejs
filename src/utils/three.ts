import * as THREE from 'three'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'
const { random, PI, floor, ceil, min, max, sin, cos } = Math

// 使用多材质和传入geo创建mesh（group）
export function createMesh(geo: THREE.BufferGeometry): THREE.Group{
  const mat1 = new THREE.MeshNormalMaterial({side: THREE.DoubleSide})
  const mat2 = new THREE.MeshBasicMaterial({
    wireframe: true
  })

  const mesh = createMultiMaterialObject(geo, [mat1, mat2])
  return mesh
}

// 设置物体中心点为标准中心点
export function setCenter(obj: THREE.Object3D){
  const box = new THREE.Box3().setFromObject(obj)
  box.getCenter(obj.position)
  obj.position.multiplyScalar(-1)
}

export function getRandomVec3(min: number, max: number){
  const p = new THREE.Vector3()

  p.x = random() * (max - min) + min
  p.y = random() * (max - min) + min
  p.z = random() * (max - min) + min
  return p
}

export function getRandomVec2(min: number, max: number){
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
      (2 * p1 - 2 * p2 + v0 + v1) * t3 +
      (-3 * p1 + 3 * p2 - 2 * v0 - v1) * t2 +
      v0 * t +
      p1
    )
  }
  getPoint(t: number, optionalTarget = new THREE.Vector3()) {
    const points = this.points
    const point = (points.length - 1) * t
    const intPoint = Math.floor(point)
    const weight = point - intPoint
    const point0 = points[intPoint == 0 ? intPoint : intPoint - 1]
    const point1 = points[intPoint]
    const point2 =
      points[intPoint > points.length - 2 ? points.length - 1 : intPoint + 1]
    const point3 =
      points[intPoint > points.length - 3 ? points.length - 1 : intPoint + 2]
    const vector = optionalTarget
    vector.x = this.interpolate(point0.x, point1.x, point2.x, point3.x, weight)
    vector.y = this.interpolate(point0.y, point1.y, point2.y, point3.y, weight)
    vector.z = this.interpolate(point0.z, point1.z, point2.z, point3.z, weight)
    return vector
  }
}

