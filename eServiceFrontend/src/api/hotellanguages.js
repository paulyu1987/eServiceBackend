import axios from 'axios'
import config from '@/app.config'

export default {
  list() {
    return axios.get(config.api.hotellanguages.endpoint, 
                {
                  headers:{
                              "Authorization":config.token.authTokenValue
                  },
                })
                .then((response) => Promise.resolve(response.data))
                .catch((error) => Promise.reject(error))
  },

    
  get(cultureID) {
    return axios.get(config.api.hotellanguages.endpoint + cultureID,
                {
                  headers:{
                              "Authorization":config.token.authTokenValue
                  },
                })
                .then((response) => Promise.resolve(response.data))
                .catch((error) => Promise.reject(error))
  },

}