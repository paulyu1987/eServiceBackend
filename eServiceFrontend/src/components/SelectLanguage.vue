<template>
  <div>

     {{selectLanguage}}
    <select v-model="selectedLanguage" @change="onSelectedLanguage()">
        <option v-for="item in items" v-bind:value="item.Value">{{item.DisplayName}}</option>
    </select>
    </br></br>

    {{ dateformat }}


    <div class="returnmessage">
      <span>{{returnmessage}}  </span>
    </div>

  </div>
</template>

<script>
import axios from 'axios'
import store from '@/store'

export default {
  name: 'selectLanguage',
  components: {
    store
  },
  data () {
    return {
      selectLanguage : 'Select Language',
      selectedLanguage:'',
      items:[],
      dateformat:'TextDateformat',

      returnmessage:'',
    }
  },

  methods: {
    loadLanguages() {
      store.fetchLanguages().then(result => {
      this.items = result;
      this.selectedLanguage = this.items[0].Value;
      }).catch((err) => { 
      console.log(err)
      })
    },

    onSelectedLanguage() {
      store.fetchLanguageByCutureId(this.selectedLanguage).then(
        result => {this.returnmessage = result;
       
       this.dateformat = result.DateFormat;

      }).catch((err) => { 
      console.log(err)
      });

    },

  },
  mounted() {
    this.loadLanguages()
  },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1, h2 {
  font-weight: normal;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
.returnmessage{
  padding-top:100px;
  color:red;
}
</style>
