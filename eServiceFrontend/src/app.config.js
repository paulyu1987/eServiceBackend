const baseUri = 'http://localhost:63618/api/'
const title = 'Vue.js'

export default {
  app: {
    title: title,
    name: 'WebApi & Vue.js',
    description: 'A small project with Vue.js and ASP.Net (WebApi).',
    author: 'Cendyn',
  },
  api: {
    hotels: {
      endpoint: baseUri + 'values/'
    },
  },
  token:{
    authTokenValue: 'basic dGVzdDoxMjM='
  },
  
}