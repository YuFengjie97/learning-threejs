<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import * as echarts from 'echarts'
import { read, utils, writeFile } from 'xlsx'
import dataFull from '@/assets/xlsx/stats3.xlsx?url'
import dataPart from '@/assets/xlsx/stats2.xlsx?url'
import { Key } from 'readline'

const echartCon = ref()

onMounted(async() => {
  const f = await(await fetch(dataFull)).arrayBuffer()
  const wb = read(f)
  const data = utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]).reverse()
  console.log(data)

  const keyMap = {
    注册: [],
    注册充值: [],
    注册充值金额: [],
    日活: [],
    日活充值: [],
    日活首充: [],
    日活充值金额: [],
    Rich用户: [],
    Rich用户活跃: [],
    Rich用户充值: [],
    Rich用户充值额: [],
    最高同时在线: [],
    开播: [],
    进房: [],
    进Party房: [],
    上麦: [],
    上Party: [],
    关注: [],
    加粉丝团: [],
    送礼用户: [],
    钻石支出: [],
    金币收入: [],
    送礼: [],
    收礼: [],
    下注: [],
    返奖: [],
    下注超10000: [],
    新用户下注: [],
    新用户返奖: [],
    任务奖励: [],
    活动奖励: [],
    邀请奖励: [],
    提现: [],
  }

  type Key = keyof typeof keyMap

  const legendData = Object.keys(keyMap)
  const legendSelected = {
    注册: false,
    注册充值: false,
    注册充值金额: false,
    日活: false,
    日活充值: false,
    日活首充: false,
    日活充值金额: false,
    Rich用户: false,
    Rich用户活跃: false,
    Rich用户充值: false,
    Rich用户充值额: false,
    最高同时在线: false,
    开播: false,
    进房: false,
    进Party房: false,
    上麦: false,
    上Party: false,
    关注: false,
    加粉丝团: false,
    送礼用户: false,
    钻石支出: false,
    金币收入: false,
    送礼: false,
    收礼: false,
    下注: false,
    返奖: false,
    下注超10000: false,
    新用户下注: false,
    新用户返奖: false,
    任务奖励: false,
    活动奖励: false,
    邀请奖励: false,
    提现: false,
  }

  const xAxisData = data.map((item) => {
    return item['日期']
  })

  data.forEach((d: any) => {
    Object.entries(d).forEach(([k, v]) => {
      if (k === '日期') return
      keyMap[k].push(v)
    })
  })
  console.log(keyMap)

  const option = {
    title: {
      text: '',
    },
    tooltip: {
      trigger: 'axis',
    },
    legend: {
      selected: legendSelected,
      top: '2%',
      left: '10%',
      right: '10%',
      data: legendData,
    },
    grid: {
      left: '10%',
      right: '10%',
      bottom: '10%',
      top: '20%',
    },
    toolbox: {
      feature: {
        saveAsImage: {},
      },
    },
    xAxis: {
      type: 'category',
      boundaryGap: false,
      data: xAxisData,
    },
    yAxis: {
      type: 'value',
    },
    series: (() => {
      return Object.entries(keyMap).map(([k, v]) => {
        const item = {
          name: k,
          type: 'line',
          data: v,
        }
        return item
      })
    })(),
  }

  const myChart = echarts.init(echartCon.value)
  myChart.setOption(option)
})
</script>

<template>
  <div class="view-con">
    <div class="echart-con" ref="echartCon"></div>
  </div>
</template>

<style lang="less" scoped>
.view-con {
  background: #fff;
  .echart-con {
    width: 100vw;
    height: 100vh;
  }
}
</style>
