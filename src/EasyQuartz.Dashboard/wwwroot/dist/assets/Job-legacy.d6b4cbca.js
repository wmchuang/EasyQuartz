!function(){function t(t,e){var n=Object.keys(t);if(Object.getOwnPropertySymbols){var i=Object.getOwnPropertySymbols(t);e&&(i=i.filter((function(e){return Object.getOwnPropertyDescriptor(t,e).enumerable}))),n.push.apply(n,i)}return n}function e(e){for(var i=1;i<arguments.length;i++){var o=null!=arguments[i]?arguments[i]:{};i%2?t(Object(o),!0).forEach((function(t){n(e,t,o[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(o)):t(Object(o)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(o,t))}))}return e}function n(t,e,n){return e in t?Object.defineProperty(t,e,{value:n,enumerable:!0,configurable:!0,writable:!0}):t[e]=n,t}System.register(["./index-legacy.4f48d52b.js"],(function(t,n){"use strict";var i,o,a,r,s,c=document.createElement("style");return c.textContent=".pagination[data-v-b008a26b]{flex:1;justify-content:flex-end;align-items:center}.capPagination[data-v-b008a26b] .page-link{color:#6c757d;box-shadow:none;border-color:#6c757d}.capPagination[data-v-b008a26b] .page-link:hover{color:#fff;background-color:#6c757d;border-color:#6c757d}.capPagination[data-v-b008a26b] .active .page-link{color:#fff;background-color:#000}.my-align-middle[data-v-b008a26b]{vertical-align:middle}\n",document.head.appendChild(c),{setters:[function(t){i=t.n,o=t.B,a=t.a,r=t.b,s=t.c}],execute:function(){var n={currentPage:2,perPage:10,jobKey:""};t("default",i({components:{BIconInfoCircleFill:o,BIconArrowRepeat:a,BIconSearch:r},props:{status:{}},data:function(){return{modal:!1,selectedItems:[],isBusy:!1,tableValues:[],isSelectedAll:!1,formData:e({},n),totals:10,items:[],logItems:[],columns12:[{title:this.$t("JobKey"),slot:"jobKey",width:200,render:function(t,e){return t("div",e.row.jobKey.substring(e.row.jobKey.indexOf(".")+1,e.row.jobKey.length))}},{title:this.$t("JobDesc"),key:"jobDesc"},{title:this.$t("Cron"),key:"cron"},{title:this.$t("LastFireTime"),key:"lastFireTime"},{title:this.$t("NextFireTime"),key:"nextFireTime"},{title:this.$t("Action"),slot:"action",width:200,align:"center"}],columnsLog:[{title:this.$t("FireTime"),key:"fireTime"},{title:this.$t("RunTime"),key:"runTime"}]}},computed:{onMetric:function(){return this.$store.getters.getMetric}},mounted:function(){this.fetchData(),window.abc=this},watch:{status:function(){this.fetchData()},"formData.currentPage":function(){this.fetchData()}},methods:{show:function(t){var e=this.items[t].jobKey;this.formData.jobKey=e,this.formData.currentPage=1,this.logItems=[],this.fetchLogData(),this.modal=!0},runNow:function(t){var e=this.items[t].jobKey;this.runNowRequest(e)},fetchData:function(){var t=this;this.isBusy=!0,s.get("/jobs",{}).then((function(e){t.items=e.data.items})).finally((function(){t.isBusy=!1}))},fetchLogData:function(){var t=this;this.isBusy=!0,s.get("/logs",{params:this.formData}).then((function(e){t.logItems=e.data.items,t.totals=e.data.totals})).finally((function(){t.isBusy=!1}))},runNowRequest:function(t){var e=this;this.isBusy=!0,s.get("/run?jobkey="+t,{}).then((function(t){e.$Message.success(t.data)})).finally((function(){}))},pageSizeChange:function(t){this.formData.currentPage=t,this.fetchLogData()}}},(function(){var t=this,e=t._self._c;return e("Content",{style:{padding:"24px 0",minHeight:"280px"}},[e("Table",{attrs:{border:"",columns:t.columns12,data:t.items},scopedSlots:t._u([{key:"jobKey",fn:function(e){var n=e.row;return[t._v(" "+t._s(n.jobKey)+" ")]}},{key:"action",fn:function(n){n.row;var i=n.index;return[e("Button",{staticStyle:{"margin-right":"5px"},attrs:{type:"primary",size:"small"},on:{click:function(e){return t.show(i)}}},[t._v("执行记录")]),e("Button",{attrs:{type:"warning",size:"small"},on:{click:function(e){return t.runNow(i)}}},[t._v("立即执行")])]}}])}),e("Modal",{attrs:{width:"900",title:"执行记录"},model:{value:t.modal,callback:function(e){t.modal=e},expression:"modal"}},[e("Content",{style:{padding:"10px 0",minHeight:"280px"}},[e("Table",{attrs:{data:t.logItems,columns:t.columnsLog,stripe:""}}),e("div",{staticStyle:{margin:"10px",overflow:"hidden"}},[e("div",{staticStyle:{float:"right"}},[e("Page",{attrs:{total:t.totals,current:t.formData.currentPage,"show-total":""},on:{"on-change":t.pageSizeChange}})],1)])],1)],1)],1)}),[],!1,null,"b008a26b",null,null).exports)}}}))}();
