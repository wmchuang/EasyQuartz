<template>

  <Content :style="{ padding: '24px 0', minHeight: '280px' }">
    <Table border :columns="columns12" :data="items">
      <template slot-scope="{ row }" slot="jobKey">
        {{ row.jobKey }}
      </template>
      <template slot-scope="{ row, index }" slot="action">
        <Button type="primary" size="small" style="margin-right: 5px" @click="show(index)">执行记录</Button>
        <Button type="warning" size="small" @click="runNow(index)">立即执行</Button>
      </template>
    </Table>

    <Modal v-model="modal" width="900" title="执行记录">
      <Content :style="{ padding: '10px 0', minHeight: '280px' }">
        <Table :data="logItems" :columns="columnsLog" stripe></Table>
        <div style="margin: 10px;overflow: hidden">
          <div style="float: right;">
            <Page :total="totals" :current="formData.currentPage" show-total @on-change="pageSizeChange"></Page>
          </div>
        </div>
      </Content>

    </Modal>
  </Content>
</template>
<script>
import axios from "axios";
import {
  BIconInfoCircleFill,
  BIconArrowRepeat,
  BIconSearch
} from 'bootstrap-vue';

const formDataTpl = {
  currentPage: 2,
  perPage: 10,
  jobKey: ""
};
export default {
  components: {
    BIconInfoCircleFill,
    BIconArrowRepeat,
    BIconSearch
  },
  props: {
    status: {}
  },
  data() {
    return {
      modal: false,
      selectedItems: [],
      isBusy: false,
      tableValues: [],
      isSelectedAll: false,
      formData: { ...formDataTpl },
      totals: 10,
      items: [],
      logItems: [],
      columns12: [
        {
          title: this.$t("JobKey"),
          slot: 'jobKey',
          width: 200,
          render: (h, params) => {
            return h('div', params.row.jobKey.substring(params.row.jobKey.indexOf('.') + 1, params.row.jobKey.length))
          }
        },
        {
          title: this.$t("JobDesc"),
          key: 'jobDesc'
        },
        {
          title: this.$t("Cron"),
          key: 'cron'
        },
        {
          title: this.$t("LastFireTime"),
          key: 'lastFireTime'
        },
        {
          title: this.$t("NextFireTime"),
          key: 'nextFireTime'
        },
        {
          title: this.$t("Action"),
          slot: 'action',
          width: 200,
          align: 'center'
        }
      ],
      columnsLog: [
        {
          title: this.$t("FireTime"),
          key: 'fireTime'
        },
        {
          title: this.$t("RunTime"),
          key: 'runTime'
        }
      ]
    };
  },
  computed: {
    onMetric() {
      return this.$store.getters.getMetric;
    }
  },
  mounted() {
    this.fetchData();
    window.abc = this;
  },
  watch: {
    status: function () {
      this.fetchData();
    },
    "formData.currentPage": function () {
      this.fetchData();
    },
  },
  methods: {
    show(index) {
      var jobKey = this.items[index].jobKey;
      this.formData.jobKey = jobKey;
      this.formData.currentPage = 1;
      this.logItems = [];
      this.fetchLogData();
      this.modal = true;
    },
    runNow(index) {
      var jobKey = this.items[index].jobKey;
      
      this.runNowRequest(jobKey);
    },
    fetchData() {
      this.isBusy = true;
      axios.get(`/jobs`, {
      }).then(res => {
        this.items = res.data.items;
      }).finally(() => {
        this.isBusy = false;
      });
    },
    fetchLogData() {
      this.isBusy = true;
      axios.get(`/logs`, {
        params: this.formData
      }).then(res => {
        this.logItems = res.data.items;
        this.totals = res.data.totals;
      }).finally(() => {
        this.isBusy = false;
      });
    },
    runNowRequest(jobKey) {
      this.isBusy = true;
      axios.get(`/run?jobkey=` + jobKey, {
      }).then(res => {
        this.$Message.success(res.data);
      }).finally(() => {
      });
    },
    pageSizeChange: function (page) {
      this.formData.currentPage = page;
      this.fetchLogData();
    }
  },
};
</script>

<style scoped>
.pagination {
  flex: 1;
  justify-content: flex-end;
  align-items: center;
}

.capPagination::v-deep .page-link {
  color: #6c757d;
  box-shadow: none;
  border-color: #6c757d;
}

.capPagination::v-deep .page-link:hover {
  color: #fff;
  background-color: #6c757d;
  border-color: #6c757d;
}

.capPagination::v-deep .active .page-link {
  color: white;
  background-color: black;
}

.my-align-middle {
  vertical-align: middle;
}
</style>