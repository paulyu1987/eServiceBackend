<template>
  <div id="app">
    <EmailTemplate></EmailTemplate>
  </div>
</template>

<script>

import EmailTemplate from './pages/EmailTemplate'

export default {
  name: 'App',

  components: {
    EmailTemplate
  },
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
