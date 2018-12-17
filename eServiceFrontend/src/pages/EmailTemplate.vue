<template>
  <div>

<b-tabs>
  <!-- <b-tab title="first" active>
    <SelectLanguage></SelectLanguage>
  </b-tab>

  <b-tab title="second" >
    <UploadImage></UploadImage>
  </b-tab>

  <b-tab title="three" >  
   <button class="btn" :style="{'background-color': color  }" ><i class="fa fa-check"></i> bbb</button>
   
    <color-picker v-model="color" v-on:change="headleChangeColor"></color-picker>
  </b-tab>

  <b-tab title="fourth" >  
     <ColorPainter></ColorPainter>
     <TestVuex></TestVuex>
  </b-tab> -->


  <b-tab title="Email" active> 
  <div> 
    <SelectLanguage></SelectLanguage>
     <ColorPainter></ColorPainter>

     <Email></Email>
     </div>
  </b-tab>

</b-tabs>

  </div>
</template>

<script>

import 'bootstrap/dist/css/bootstrap.css' 
import 'bootstrap-vue/dist/bootstrap-vue.css'

import SelectLanguage from '@/components/SelectLanguage'
import HotelList from '@/components/HotelList'
import UploadImage from '@/components/UploadImage'

import Vue from 'vue'
import bTabs from 'bootstrap-vue/es/components/tabs/tabs';
import bTab from 'bootstrap-vue/es/components/tabs/tab';
import bCard from 'bootstrap-vue/es/components/card/card';

Vue.component('b-tabs', bTabs);
Vue.component('b-tab', bTab);
Vue.component('b-card', bCard);

import ColorPicker from '@/components/color-picker'
import Email from '@/components/Email'
import ColorPainter from '@/components/ColorPainter'
import TestVuex from '@/components/TestVuex'


export default {
  name: 'EmailTemplate',

  components: {
  	HotelList,
    SelectLanguage,
    UploadImage,
    ColorPicker,
    Email,
    ColorPainter,
    TestVuex,
  },

  data () {
    return {
      msg: 'This is the message from email tempate',
      items:[{text:'A',value:'a'},{text:'B',value:'b'},{text:'C',value:'c'}],
      selected:'',
      color: '#ff0000',

      textarea:"asdhaskdaslkd",
    }
  },

       methods: {
              headleChangeColor (color) {
                // console.log(`颜色值改变事件：${color}`)
            }
          }

 
}
</script>


