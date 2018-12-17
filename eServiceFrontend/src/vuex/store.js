import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

const store = new Vuex.Store({
  state: {
    author: 'Wise Wrong',
    color: '#ff0000',
    TextColor:'#ff0000',
    LinksColor:'#ff0000',
    HeaderTextColor:'#ff0000',
    HeaderBackgroundColor:'#ff0000',
    FooterTextColor:'#ff0000',
    FooterBackgroundColor:'#ff0000',
  }
})

export default store