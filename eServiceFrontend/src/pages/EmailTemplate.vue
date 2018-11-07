<template>
  <div>
    <HotelList></HotelList>
  </div>
</template>

<script>

import HotelList from '@/components/HotelList'

export default {
  name: 'EmailTemplate',

  components: {
  	HotelList
  },

  data () {
    return {
      msg: 'This is the message from email tempate',
      items:[{text:'A',value:'a'},{text:'B',value:'b'},{text:'C',value:'c'}],
      selected:''
    }
  }
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
