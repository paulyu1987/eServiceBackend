import api from '@/api'
import events from '@/events'

const store = {}

/* Fetch brands */
 store.fetchHotels = () => {
  return new Promise((resolve, reject) => {
    api.hotels.list().then(hotels => {
      resolve(hotels)
    }).catch(err => {
      events.$emit('error', err)
      reject(err)
    })
  })
}

 store.fetchLanguages = () => {
  return new Promise((resolve, reject) => {
    api.hotellanguages.list().then(hotellanguages => {
      resolve(hotellanguages)
    }).catch(err => {
      events.$emit('error', err)
      reject(err)
    })
  })
}


/* Fetch brands by id */
 store.fetchHotelById = (id) => {
  return new Promise((resolve, reject) => {
    api.hotels.get(id).then(hotel => {
      resolve(hotel)
    }).catch(err => {
      events.$emit('error', err)
      reject(err)
    })
  })
}

 store.fetchLanguageByCutureId = (cultureID) => {
  return new Promise((resolve, reject) => {
    api.hotellanguages.get(cultureID).then(laguange => {
      resolve(laguange)
    }).catch(err => {
      events.$emit('error', err)
      reject(err)
    })
  })
}

/* Save brand */
 store.saveHotel = (hotel) => {
  return new Promise((resolve, reject) => {
    api.hotels.save(hotel).then(message => {
      resolve(message)}).catch(err => {
        events.$emit('error', err)
        reject(err)
      })
  })
}

/* Upload Image */
 store.uploadImage = (formData) => {
  return new Promise((resolve, reject) => {
    api.uploadimage.save(formData).then(message => {
      resolve(message)}).catch(err => {
        events.$emit('error', err)
        reject(err)
      })
  })
}

 store.saveEmailTemplate = (emailtemplate) => {
  return new Promise((resolve, reject) => {
    api.emailtemplates.save(emailtemplate).then(message => {
      resolve(message)}).catch(err => {
        events.$emit('error', err)
        reject(err)
      })
  })
}


export default store