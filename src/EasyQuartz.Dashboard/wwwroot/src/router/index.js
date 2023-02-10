import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
    {
        path: '/',
        name: 'Job',
        component: () => import('../pages/Job.vue')
    },
    {
        path: '/job',
        name: 'Job',
        component: () => import('../pages/Job.vue')
    }
]

const router = new VueRouter({
    routes
})

export default router;