<template>
  <div>

   <div class="img-panel">
     <img :src="hotelPic" class="img-avatar" @click="sethotelPic">
     <input type="file" name="avatar" id="uppic" accept="image/gif,image/jpeg,image/jpg,image/png" @change="changeImage($event)" ref="avatarInput" class="uppic" style="display:none" >
   </div>

   <button type="button" @click="edit"> {{btnUpload}}</button>

    <div class="returnmessage">
      <span>{{returnmessage}}  </span>
    </div>
    
  </div>
</template>

<script>
import axios from 'axios'
import store from '@/store'

import Vue from 'vue'
import VuejsDialog from "vuejs-dialog"

Vue.use(VuejsDialog)

export default {
  name: 'UploadImage',
  components: {
    store
  },
  data () {
    return {
      btnUpload:'Upload',
      hotelPic: require('../assets/TextUploadPic.jpg'),
      formData: new FormData(),

      returnmessage:'',
    }
  },

   methods: {
   changeImage(e) {
       var file = e.target.files[0]
       this.formData.append('file', file)

       var reader = new FileReader()
       var that = this
       reader.readAsDataURL(file)
       reader.onload = function(e) {
       that.hotelPic = this.result
       }
     },

  sethotelPic() {
      document.getElementById('uppic').click()
    },

     edit() {
        // if (document.getElementById('uppic').files.length !== 0) {
          store.uploadImage(this.formData).then(
            result => {this.returnmessage = result;
          }).catch((err) => { 
          console.log(err)
          });
      // }
    },
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
  .img-avatar {
     height: 15rem;
     width: 20rem;
  }
</style>
