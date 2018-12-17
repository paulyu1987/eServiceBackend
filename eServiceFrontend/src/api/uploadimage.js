import axios from 'axios'
import config from '@/app.config'

export default {
  save(formData) {
    return axios.post(config.api.uploadimage.endpoint, formData,
                {
                  headers:{
                              "Authorization":config.token.authTokenValue
                  },
                })
                .then((response) => Promise.resolve(response.data))
                .catch((error) => Promise.reject(error))
  },

}