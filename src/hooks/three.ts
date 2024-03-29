import { onMounted, ref } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'

export function useTHREE(initMesh: () => Array<THREE.Mesh>, animate: () => void) {
  const canvasDom = ref<HTMLElement>()
  const canvasCon = ref<HTMLElement>()
  let width = window.innerWidth
  let height = window.innerHeight
  let stats: Stats
  let orbitControls: OrbitControls | undefined
  let scene: THREE.Scene | undefined
  let camera: THREE.PerspectiveCamera | undefined
  let renderer: THREE.WebGLRenderer | undefined
  let gui: GUI | undefined

  function initGUI() {
    gui = new GUI({
      autoPlace: false,
      width: 300,
    })
    gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
    canvasCon.value?.appendChild(gui.domElement)
  }

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
    orbitControls.target = new THREE.Vector3(0, 0, 0)
    orbitControls.object.position.set(0, 0, 100)
    orbitControls.update()
  }

  // 绘制
  function render() {
    requestAnimationFrame(render)
    stats.update()
    animate()
    renderer!.render(scene!, camera!)
  }
  // 自适应
  function onWindowResize() {
    width = window.innerWidth
    height = window.innerHeight
    camera!.aspect = width / height
    camera!.updateProjectionMatrix()
    renderer!.setSize(width, height)
  }

  function addMesh() {
    const meshs = initMesh()
    meshs.forEach((m) => {
      scene?.add(m)
    })
  }

  onMounted(() => {
    initGUI()
    initTHREE()
    addMesh()
    render()
  })

  return {
    canvasDom,
    canvasCon,
    width,
    height,
    orbitControls,
    scene,
    camera,
    renderer,
    gui,
  }
}
