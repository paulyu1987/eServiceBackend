<template>
  <div>

   <p>
      Copyright&nbsp;&copy;&nbsp;{{author}} - 2016 All rights reserved

      <button class="btn" :style="{'background-color': color  }" ><i class="fa fa-check"></i> bbb</button>
    </p>

  </div>
</template>

<script>

import ColorPicker from '@/components/color-picker'

export default {
  name: 'TestVuex',
  components: {

  },
  data () {
    return {
     
    }
  },
   computed: {
      author () {
        return this.$store.state.author
      },

      color () {
        return this.$store.state.color
      }
    },

  methods: {
            
          },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>


</style>
