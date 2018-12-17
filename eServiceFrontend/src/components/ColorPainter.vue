<template>
  <div>

        <button type="button" @click="GetHtml" >aa</button>
  <span>{{returnmessage}}  </span>

   <div class="textcolor">
    {{ textcolor }}
     <color-picker v-model="colortext" v-on:change="headleChangeTextColor"></color-picker>
   </div>

  <div class="linkscolor">
    {{ linkscolor }}
     <color-picker v-model="colorlinks" v-on:change="headleChangeLinksColor"></color-picker>
   </div>

  <div class="headertextcolor">
    {{ headertextcolor }}
     <color-picker v-model="colorheadertext" v-on:change="headleChangeHeaderTextColor"></color-picker>
   </div>

   <div class="headerbackgroundcolor">
    {{ headerbackgroundcolor }}
     <color-picker v-model="colorheaderbackground" v-on:change="headleChangeHeaderBackgroundColor"></color-picker>
   </div>

   <div class="footertextcolor">
    {{ footertextcolor }}
     <color-picker v-model="colorfootertext" v-on:change="headleChangeFooterTextColor"></color-picker>
   </div>

   <div class="footerbackgroundcolor">
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


</style>
