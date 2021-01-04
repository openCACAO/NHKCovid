<template>
  <v-app>
    <v-main>
      <v-container>
        <h2>Covid19 感染者状況</h2>
        <div>
        from <a href="https://www3.nhk.or.jp/news/special/coronavirus/data-widget/">情報提供:NHK</a>
        </div>
        <div style="margin-bottom: 2em;" />
        <covid-chart></covid-chart>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import CovidChart from './components/CovidChart.vue';

export default {
  name: 'App',
  components: {
    CovidChart,
  },
  data() {
    return {}
  },

};
</script>

