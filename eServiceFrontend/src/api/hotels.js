import axios from 'axios'
import config from '@/app.config'

export default {
  list() {
    return axios.get(config.api.hotels.endpoint, 
                {
                  headers:{
                              "Authorization":config.token.authTokenValue
                  },
                })
                .then((response) => Promise.resolve(response.data))
                .catch((error) => Promise.reject(error))
  },
  
  get(id) {
    return axios.get(config.api.hotels.endpoint + id,
                {
                  headers:{
                              "Authorization":config.token.authTokenValue
                  },
                })
                .then((response) => Promise.resolve(response.data))
                .catch((error) => Promise.reject(error))
  },

  save(hotel) {
    return axios.post(config.api.hotels.endpoint,
                {
                  headers:{
                              "Authorization":config.token.authTokenValue
                  },
                  id: hotel.id,
                  RateTypeCode: hotel.RateTypeCode
                })
                .then((response) => Promise.resolve(response.data))
                .catch((error) => Promise.reject(error))
  },
}