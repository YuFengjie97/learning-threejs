<script lang="ts" setup>
import { ref } from 'vue'
import { read, readFile, utils } from 'xlsx'

// async function readExcelFile() {
//   const fileReader = new FileReader()

//   const workbook = await readFile(dataFile, { type: 'file' })
//   const sheetName = workbook.SheetNames[0]
//   const worksheet = workbook.Sheets[sheetName]
//   const jsonData = utils.sheet_to_json(worksheet, { header: 'A' })
//   return jsonData
// }

// ;(async () => {
//   const d = await readExcelFile()
//   console.log(d)
// })()

const analyseExcelToJson = (file) => {
  return new Promise((resolve, reject) => {
    if (file instanceof File) {
      const reader = new FileReader()
      reader.onloadend = (progressEvent) => {
        const arrayBuffer = reader.result
        const workbook = read(arrayBuffer, { type: 'array' })
        const sheetName = workbook.SheetNames
        const sheet = workbook.Sheets[sheetName]
        resolve(sheet)
      }
      reader.readAsArrayBuffer(file)
    } else {
      reject(new Error('入参不是 File 类型'))
    }
  })
}

function handleInputFile(e: Event){
  const target = e.target as HTMLInputElement
  if(target.files && target.files[0]) {
    const file = target.files[0]
    console.log('e----',e);
    console.log('file----',file);

    const reader = new FileReader()
    reader.readAsText(file, 'binary')
    reader.onload = () => {
      console.log('reader-res----',reader.result);
      const data = reader.reader.result
    }
    reader.onerror = () => {
      console.log('reader-error----',reader.error);
      
    }
  }
}
</script>

<template>
  <div class="view-con">
    <h1>excel解析</h1>
    <input type="file" @change="handleInputFile">
  </div>
</template>

<style lang="less" scoped>
.view-con {
}
</style>
