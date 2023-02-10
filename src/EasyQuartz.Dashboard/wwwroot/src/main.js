import Vue from 'vue'
import App from './App.vue'
import router from './router'
import BootstrapVue from 'bootstrap-vue'
import VueJsonPretty from 'vue-json-pretty';
import 'vue-json-pretty/lib/styles.css';
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import store from '../src/store/store.js'
import axios from "axios";
import VueI18n from 'vue-i18n'
import * as zh from './assets/language/zh-cn'
import * as en from './assets/language/en-us'
import ViewUI from 'view-design';
import 'view-design/dist/styles/iview.css';


//

let baseURL = "";
switch (import.meta.env.MODE) {
  case 'development':
      baseURL = "/easyjob/api";
      break
  default:
      baseURL = window.serverUrl;
      break
}

axios.defaults.baseURL = baseURL;
axios.defaults.withCredentials = true
axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.interceptors.request.use(
  config => {
    let accessToken = localStorage.getItem('token');
    if (accessToken) {
      config.headers = Object.assign({
        Authorization: `Bearer ${accessToken}`
      }, config.headers);
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

Vue.config.productionTip = false

Vue.use(BootstrapVue)
Vue.component("vue-json-pretty", VueJsonPretty)
Vue.use(VueI18n)
Vue.use(ViewUI);

const i18n = new VueI18n({
  locale: (function () {
    if (localStorage.getItem('lang')) {
      return localStorage.getItem('lang')
    }
    return 'en-us'
  }()),
  messages: {
    'en-us': en.default,
    'zh-cn': zh.default,
  }
})

new Vue({
  router,
  store,
  i18n,
  render: h => h(App)
}).$mount('#app')