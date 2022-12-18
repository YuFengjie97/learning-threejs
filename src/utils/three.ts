import * as THREE from 'three'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'

export function createMesh(geo: THREE.BufferGeometry){
  const mat1 = new THREE.MeshNormalMaterial({side: THREE.DoubleSide})
  const mat2 = new THREE.MeshBasicMaterial({
    wireframe: true
  })

  const mesh = createMultiMaterialObject(geo, [mat1, mat2])
  return mesh
}