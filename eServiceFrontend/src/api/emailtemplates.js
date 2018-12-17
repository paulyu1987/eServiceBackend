import axios from 'axios'
import config from '@/app.config'

export default {
  save(emailtemplate) {
    return axios.post(config.api.emailtemplates.endpoint,
                {
                  headers:{
                              "Authorization":config.token.authTokenValue
                  },
                  Template: emailtemplate.Template,
                })
                .then((response) => Promise.resolve(response.data))
                .catch((error) => Promise.reject(error))
  },

}