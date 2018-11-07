<template>
  <div>
    {{hotelName}}
    <select v-model="selectedHotel">
        <option v-for="item in items" v-bind:value="item.ID">{{item.Name}}</option>
    </select>
  </div>
</template>

<script>
import axios from 'axios'
import store from '@/store'

export default {
  name: 'HotelList',
  components: {
    store
  },
  data () {
    return {
      hotelName : 'Hotel Name :',
      items:[],
      selectedHotel:''
    }
  },
  methods: {
    loadHotels() {
      store.fetchHotels().then(result => {
      this.items = result;
      }).catch((err) => { 
      console.log(err)
      })
    },


  },
  mounted() {
    this.loadHotels()
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
</style>
