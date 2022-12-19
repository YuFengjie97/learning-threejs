import * as THREE from 'three'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'

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