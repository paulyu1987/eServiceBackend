<template>
  <div>
    {{hotelName}}
    <select v-model="selectedHotel">
        <option v-for="item in items" v-bind:value="item.Value">{{item.DisplayName}}</option>
    </select>
    </br></br>
    {{add}}
    <input v-model="addHotelName" placeholder="">
    <input type="submit" @click="addHotel" value="Save" class="btn btn-default" />
    </br></br>
    {{edit}}
    <input v-model="editHotelName" placeholder="">
    <input type="submit" @click="saveHotel" value="Save" class="btn btn-default" />
    </br></br>
    {{delete1}}
    <input type="submit" @click="deleteHotel" value="Delete" class="btn btn-default" />
    <div class="returnmessage">
      <span>{{returnmessage}}  </span>
    </div>
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
      hotelName : 'RateType Name :',
      add : 'Add RateType :',
      edit : 'Update RateType :',
      delete1 : 'Delete RateType :',
      items:[],
      selectedHotel:'',
      editHotelName:'',
      addHotelName:'',
      returnmessage:''
    }
  },
  methods: {
    addHotel() {
      var hotel = {
        id : 0,
        RateTypeCode : this.addHotelName
      };
      store.saveHotel(hotel).then(result => {
      this.returnmessage = result;
      }).catch((err) => { 
      console.log(err)
      });
    },
    deleteHotel() {
      var hotel = {
        id : this.selectedHotel,
        RateTypeCode : 'delete'
      };
      store.saveHotel(hotel).then(
        result => {this.returnmessage = result;
        this.loadHotels();
      }).catch((err) => { 
      console.log(err)
      });
    },
    saveHotel() {

      var hotel = {
        id : this.selectedHotel,
        RateTypeCode : this.editHotelName
      };
      store.saveHotel(hotel).then(result => {
      this.returnmessage = result;
      }).catch((err) => { 
      console.log(err)
      });
    },

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
.returnmessage{
  padding-top:100px;
  color:red;
}
</style>
