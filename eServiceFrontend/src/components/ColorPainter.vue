<template>
  <div class="color-painter">

        <!-- <button type="button" @click="GetHtml" >aa</button> -->
  <span>{{returnmessage}}  </span>
<div class="settingsTitle">
  <strong>Settings</strong>
</div>
<div class="color-block">
  From Name  * 
  <select class="dropdownlist" v-model="selectedFromName">
        <option v-for="item in fromNames" v-bind:value="item.key">{{item.value}}</option>
  </select>
</div>

<div class="color-block">
  Subject  *
  <select class="dropdownlist" v-model="selectedSubject">
        <option v-for="item in fromSubjects" v-bind:value="item.key">{{item.value}}</option>
  </select>
</div>

<div class="color-block">
  From Email
  <input class="dropdownlist">
</div>

<div class="settingsTitle">
  <strong>Design</strong>
</div>

   <div class="textcolor color-block">
    {{ textcolor }}
     <color-picker v-model="colortext" v-on:change="headleChangeTextColor"></color-picker>
   </div>

  <div class="linkscolor color-block">
    {{ linkscolor }}
     <color-picker v-model="colorlinks" v-on:change="headleChangeLinksColor"></color-picker>
   </div>

  <div class="headertextcolor color-block">
    {{ headertextcolor }}
     <color-picker v-model="colorheadertext" v-on:change="headleChangeHeaderTextColor"></color-picker>
   </div>

   <div class="headerbackgroundcolor color-block">
    {{ headerbackgroundcolor }}
     <color-picker v-model="colorheaderbackground" v-on:change="headleChangeHeaderBackgroundColor"></color-picker>
   </div>

   <div class="footertextcolor color-block">
    {{ footertextcolor }}
     <color-picker v-model="colorfootertext" v-on:change="headleChangeFooterTextColor"></color-picker>
   </div>

   <div class="footerbackgroundcolor color-block">
    {{ footerbackgroundcolor }}
     <color-picker v-model="colorfooterbackground" v-on:change="headleChangeFooterBackgroundColor"></color-picker>
   </div>

  </div>
</template>

<script>

import ColorPicker from '@/components/color-picker'


import axios from 'axios'
import store from '@/store'

export default {
  name: 'ColorPainter',
  components: {
    ColorPicker,
    store,
  },
  data () {
    return {
      returnmessage:'',

      textcolor:"Text",
      colortext:'#ff0000',

      linkscolor:"Links",
      colorlinks:'#ff0000',

      headertextcolor:"Header Text",
      colorheadertext:'#ff0000',

      headerbackgroundcolor:"Header Background",
      colorheaderbackground:'#000000',

      footertextcolor:"Footer Text",
      colorfootertext:'#ff0000',

      footerbackgroundcolor:"Footer Background",
      colorfooterbackground:'#ff0000',

      selectedFromName:"-1",
      fromNames: [
          {key: -1, value: "Upgrades From"},
          {key: 0, value: "Hotel Name"},
          {key: 1, value: "Hotel Email"},
          {key: 2, value: "Hotel Phone"},
          {key: 3, value: "Hotel Address"},
          {key: 4, value: "Guest Name"},
          {key: 5, value: "Year"},
        ],

      selectedSubject:"0",
      fromSubjects: [
          {key: 0, value: "Your |HOTELNAME| Upgrades"},
        ],
    }
  },

  methods: {
              headleChangeTextColor (colortext) {
                // this.$store.state.color = this.color
                this.$store.state.TextColor = this.colortext
                // console.log(`颜色值改变事件：${color}`)
            },
             headleChangeLinksColor (colorlinks) {
                this.$store.state.LinksColor = this.colorlinks
            },

             headleChangeHeaderTextColor (colorheadertext) {
                this.$store.state.HeaderTextColor = this.colorheadertext
            },

            headleChangeHeaderBackgroundColor(colorheaderbackground) {
                this.$store.state.HeaderBackgroundColor = this.colorheaderbackground
            },

            headleChangeFooterTextColor(colorfootertext) {
                this.$store.state.FooterTextColor = this.colorfootertext
            },

            headleChangeFooterBackgroundColor(colorfooterbackground) {
                this.$store.state.FooterBackgroundColor = this.colorfooterbackground
            },


                GetHtml() {
                  var asdasd=document.getElementById('ExportEmailTemplate').innerHTML; 
                  console.log(asdasd); 

                        var EmailTemplate = {
                            Template : asdasd,
                          };

                      store.saveEmailTemplate(EmailTemplate).then(result => {
                        this.returnmessage = result;
                        }).catch((err) => { 
                        console.log(err)
                        });
            },
          }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

  .settingsTitle{
    background:#0003;
    Height:45px;
    padding-left: 10px;
    padding-top: 8px;
    font-size: 19px;
  }

  .color-painter{
    display: inline-block;
    width: 300px;
    text-align: left;
    border: 1px solid #0003;
    margin: 50px;
  }

  .color-block{
    margin: 10px 10px 10px 10px;
    padding-bottom: 15px;
  }

  .dropdownlist{
    Height: 30px;
    width: 100%;
  }

</style>
