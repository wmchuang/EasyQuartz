<template>
  <div id="app" class="layout">
    <layout>
      <Navigation />
      <b-container class="xl">
        <router-view />
      </b-container>
    </layout>

  </div>
</template>

<script>
import Navigation from "@/components/Navigation.vue";
import Footer from "@/components/Footer.vue";

export default {
  name: "App",
  components: {
    Navigation,
    Footer
  },
  data() {
    return { timer: '' }
  },
  methods: {
    getData() {
      if (window.pollingInterval == "%(pollingInterval)") {
        window.pollingInterval = 2000;
      }

      // axios.get('/stats').then(response => {
      //   this.$store.dispatch("pollingMertic", response.data);
      //   setTimeout(() => {
      //     this.getData()
      //   }, window.pollingInterval);
      // });
    }
  },
  mounted() {
    this.getData();
  },
  beforeDestroy() {
    clearInterval(this.timer);
  }
};

Date.prototype.format = function (fmt) {
  var o = {
    "M+": this.getMonth() + 1,
    "d+": this.getDate(),
    "h+": this.getHours(),
    "m+": this.getMinutes(),
    "s+": this.getSeconds(),
    "q+": Math.floor((this.getMonth() + 3) / 3),
    S: this.getMilliseconds(),
  };
  if (/(y+)/.test(fmt)) {
    fmt = fmt.replace(
      RegExp.$1,
      (this.getFullYear() + "").substr(4 - RegExp.$1.length)
    );
  }
  for (var k in o) {
    if (new RegExp("(" + k + ")").test(fmt)) {
      fmt = fmt.replace(
        RegExp.$1,
        RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length)
      );
    }
  }
  return fmt;
};
</script>
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  padding-bottom: 50px;
}

.page-line {
  text-align: left;
  line-height: 38px;
  padding-bottom: 9px;
  border-bottom: 1px solid #eee;
}