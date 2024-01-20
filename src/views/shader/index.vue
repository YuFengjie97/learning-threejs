<script setup lang="ts"></script>

<template>
  <div class="h-100vh w-full flex flex-nowrap">
    <div class="link-wrap">
      <RouterLink to="/shader/normal">
        normal
      </RouterLink>
      <RouterLink to="/shader/matrices">
        matrices
      </RouterLink>
      <RouterLink to="/shader/patterns">
        patterns
      </RouterLink>
      <RouterLink to="/shader/shape">
        shape
      </RouterLink>
    </div>
    <div class="flex-1">
      <RouterView />
    </div>
  </div>
</template>

<style lang="less" scoped>
.page-wrap {
  .view-wrap {
    flex: 1;
  }
}
.link-wrap {
  padding: 1rem;
  display: flex;
  display: none;
  flex-flow: column nowrap;
  @media screen and (max-width: 576px) {
    padding: 1rem;
  }
}
h1{
    padding: 2rem 0rem;
  }
a{
  padding: 0.3rem 0;
  padding-left: 2rem;
  display: inline-block;
  font-size: 1.2rem;
  color: rgb(132, 132, 132);
  transition: .3s;
  &:link{}
  &:hover{
    color: #fff;
  }
  &:visited{}
  &:active{}
}
</style>
