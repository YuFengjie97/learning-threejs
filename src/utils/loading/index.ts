import {
  defineComponent,
  createApp,
  h,
  ref,
  type ComponentPublicInstance,
} from 'vue'

import MyLoading from './Loading.vue'

function createLoadingComponent() {
  const show = ref(true)

  // 组件
  const loadingComponent = defineComponent({
    name: 'MyLoading',
    setup() {
      return () => {
        const c = h(
          'div',
          { style: { display: show ? 'block' : 'none' } },
          h(MyLoading)
        )
        console.log(c);
        
        return c
      }
      // jsx语法，vite开箱即用，需要将文件夹ts改为tsx
      // return () => {
      //   return <>{show.value ? <MyLoading /> : ''}</>
      // }
    },
  })
  // 应用实例
  const loadingInstance = createApp(loadingComponent)
  // vue实例
  const vm = loadingInstance.mount(document.createElement('div'))

  // dom添加
  const body = document.querySelector('body')
  body?.appendChild(vm.$el)

  function close() {
    show.value = false

    vm.$el.parentNode.removeChild(vm.$el)
    loadingInstance.unmount()
  }

  return {
    close
  }
}

const loading = {
  service: createLoadingComponent,
}

export default loading