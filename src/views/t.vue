<script lang="ts" setup>
import { ref } from 'vue'
import { gsap } from 'gsap'

const list = ref<Array<number>>([])
function addList() {
  for (let i = 0; i < 4; i++) {
    list.value.push(i)
  }
}

addList()

function removeAll() {
  list.value.length = 0
}

function onEnter(el: any, done) {
  console.log(done);
  
  gsap.to(el, {
    opacity: 1,
    duration: 0.3,
    delay: el.dataset.index * 0.15,
    onComplete: done
  })
}

function onLeave(el: any, done) {
  console.log(done);
  
  gsap.to(el, {
    // opacity: 0,
    x: '100%',
    // duration: 0.3,
    delay: el.dataset.index * 0.15,
    onComplete: done
  })
}

</script>

<template>
  <div class="view-con">
    <button @click="addList">add</button>
    <button @click="removeAll">remove All</button>
    <TransitionGroup tag="div" :css="false" @enter="onEnter" @leave="onLeave">
      <div class="item" v-for="(item, i) in list" :key="i" :data-index="i">
        {{ item }}
        <div class="info">hello</div>
      </div>
    </TransitionGroup>
  </div>
</template>

<style lang="less" scoped>
.view-con {
  display: flex;
  flex-flow: column nowrap;
  .item {
    line-height: 40px;
    background-color: rgb(47, 146, 123);
    border: 4px solid rgb(45, 85, 54);
  }

  // .list-move,
  // .list-enter-active,
  // .list-leave-active {
  //   transition: all 0.5s ease;
  // }
  // .list-enter-from,
  // .list-leave-to {
  //   opacity: 0;
  //   transform: translateX(30px);
  // }
}
</style>
