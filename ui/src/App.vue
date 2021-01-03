<template>
  <v-app>
    <v-main>
      <v-container>
      <h2>Covid19 感染者状況</h2>
      <div>
      from <a href="https://www3.nhk.or.jp/news/special/coronavirus/data-widget/">情報提供:NHK</a>
      </div>
      <div style="margin-top: 50px">
        <v-select
            :items="locations"
            v-model="value"
            label="都道府県"
            dense
            multiple
            @input="selectLocation"
          ></v-select>
        <h3>陽性者数</h3>      
        <line-chart :chart-data="datax" :options="options"/>
        <h3>実効再生産数 Rt</h3>      
        <line-chart :chart-data="datart" :options="options"/>
        <h3>陽性者数予測</h3>      
        <line-chart :chart-data="datafu" :options="optionsfu"/>
        <div style="font-size: 0.8em">
          陽性者数予測値は、過去7日間の陽性者数と実効再生産数Rtから計算しています。
          予測値は、最終のRtをそのまま適用しています。
        </div>
      </div>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import axios from 'axios'
import LineChart from './components/LineChart.vue';
// import Vue from 'vue'

export default {
  name: 'App',
  components: {
    LineChart,
  },

  data() {
    return {
      datax: {
        labels: ["1", "2", "3", "4", "5"],
        datasets: [
          {
            label: '東京都',
            fill: false,
            borderColor: "rgba(0,0,200,0.5)",
            data: [10, 10, 10, 10, 10, 10]
          }
        ]
      },
      datart: {
        labels: ["1", "2", "3", "4", "5"],
        datasets: [
          {
            label: '東京都',
            fill: false,
            borderColor: "rgba(0,0,200,0.5)",
            data: [10, 10, 10, 10, 10, 10]
          }
        ]
      },
      datafu: {
        labels: ["1", "2", "3", "4", "5"],
        datasets: [
          {
            label: '東京都',
            fill: false,
            borderColor: "rgba(0,0,200,0.5)",
            data: [10, 10, 10, 10, 10, 10]
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false
      },
      optionsfu: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            yAxes: [{
                id: "y-axis-1",   // Y軸のID
                type: "linear",   // linear固定 
                position: "left", // どちら側に表示される軸か？
            }, 
            {
                id: "y-axis-2",
                type: "linear", 
                position: "right",
            }],
        }
      },      
      value: ["東京都"],
      locations: [
        "北海道",
        "青森県",
        "岩手県",
        "宮城県",
        "秋田県",
        "山形県",
        "福島県",
        "茨城県",
        "栃木県",
        "群馬県",
        "埼玉県",
        "千葉県",
        "東京都",
        "神奈川県",
        "新潟県",
        "富山県",
        "石川県",
        "福井県",
        "山梨県",
        "長野県",
        "岐阜県",
        "静岡県",
        "愛知県",
        "三重県",
        "滋賀県",
        "京都府",
        "大阪府",
        "兵庫県",
        "奈良県",
        "和歌山県",
        "鳥取県",
        "島根県",
        "岡山県",
        "広島県",
        "山口県",
        "徳島県",
        "香川県",
        "愛媛県",
        "高知県",
        "福岡県",
        "佐賀県",
        "長崎県",
        "熊本県",
        "大分県",
        "宮崎県",
        "鹿児島県",
        "沖縄県",
      ],
      colors: [
        { n: "rgba(0,0,200,0.5)", ave: "rgba(0,0,100,0.5)" },
        { n: "rgba(200,0,0,0.5)", ave: "rgba(200,0,0,0.5)" },
        { n: "rgba(0,200,0,0.5)",ave: "rgba(0,200,0,0.5)"},
        { n: "rgba(200,0,200,0.5)",ave: "rgba(200,0,200,0.5)"},
        { n: "rgba(200,200,0,0.5)",ave: "rgba(200,200,0,0.5)"},
        { n: "rgba(0,200,200,0.5)",ave: "rgba(0,200,200,0.5)"},
      ],
    }
  },
  mounted() {
    this.getData() 
  },
  methods: {
    /**
     * 感染者数のグラフを作成
     */
    makeCases(res,start_date,end_date,locations) {

      var sdate = Date.parse(start_date)
      var edate = Date.parse(end_date)
      var datasets = []
      var labels = []

      var i = 0;
      locations.forEach(location => {
        var data = [];
        var data2 = [];
        labels = [];
        res.data.result.forEach(el => {
          if ( el.location == location ) {
            var dt = Date.parse( el.date )
            if ( sdate <= dt && dt <= edate ) {
              dt = new Date(dt)
              dt = dt.getFullYear() + "/" + (dt.getMonth()+1) + "/" + dt.getDate() 
              labels.push( dt )
              data.push( el.cases )
              data2.push( el.casesAverage )
            }
          }
        });
        var dataset = 
          {
            label: location,
            fill: false,
            borderColor: i >= this.colors.length? "rgba(200,200,200,0.5)": this.colors[i].n,
            data: data
          }
        var dataset2 = 
          {
            label: location + "(週平均)",
            fill: false,
            borderColor: i >= this.colors.length? "rgba(100,100,100,0.5)": this.colors[i].ave,
            data: data2
          }
        datasets.push( dataset )
        datasets.push( dataset2 )
        i++;
      })
      return { labels, datasets };
    },

    /**
     * 実効再生産数のグラフを作成
     */
    makeCasesRt(res,start_date,end_date,locations) {

      var sdate = Date.parse(start_date)
      var edate = Date.parse(end_date)
      var datasets = []
      var labels = []

      var i = 0;
      locations.forEach(location => {
        var data = [];
        var data2 = [];
        labels = [];
        res.data.result.forEach(el => {
          if ( el.location == location ) {
            var dt = Date.parse( el.date )
            if ( sdate <= dt && dt <= edate ) {
              dt = new Date(dt)
              dt = dt.getFullYear() + "/" + (dt.getMonth()+1) + "/" + dt.getDate() 
              labels.push( dt )
              if ( el.casesRt > 0.0 ) {
                data.push( el.casesRt )
                data2.push( el.casesRtAverage )
              }
            }
          }
        });
        // this.datart.labels = labels 
        var dataset = 
          {
            label: location,
            fill: false,
            borderColor: i >= this.colors.length? "rgba(200,200,200,0.5)": this.colors[i].n,
            data: data
          }
        var dataset2 = 
          {
            label: location + "(週平均)",
            fill: false,
            borderColor: i >= this.colors.length? "rgba(100,100,100,0.5)": this.colors[i].ave,
            data: data2
          }
        datasets.push( dataset )
        datasets.push( dataset2 )
        i++;
      })
      return { labels2: labels, datasets2: datasets };
    },

    /**
     * 最新の実効再生産数から今後1か月の感染者数を予測
     */
    makeCasesFuture(res,start_date,end_date,locations) {

      var sdate = Date.parse(start_date)
      var edate = Date.parse(end_date)
      var datasets = []
      var labels = []
      var i = 0;

      console.log( edate )

      locations.forEach(location => {
        // 最終日を取得
        var lastdate = null 
        var last = null
        res.data.result.forEach(el => {
          if ( el.location == location ) {
            var dt = Date.parse( el.date )
            if ( el.casesRt > 0.0 ) {
              if ( lastdate < dt ) {
                lastdate = dt 
                last = el
              }
              if ( sdate <= dt ) {
                dt = new Date(dt)
                dt = dt.getFullYear() + "/" + (dt.getMonth()+1) + "/" + dt.getDate() 
                if ( i == 0 ) labels.push( dt )
              }
            }
          }
        })
        console.log( last );

        var data = [];
        var data2 = [];
        // 実測値を集計
        res.data.result.forEach(el => {
          if ( el.location == location ) {
            var dt = Date.parse( el.date )
            if ( el.casesRt > 0.0 ) {
              if ( sdate <= dt ) {
                data.push( el.cases )
                data2.push( el.casesRtAverage )
              }
            }
          }
        })
        // 予測値を計算
        var rt = last.casesRtAverage ;
        for ( var j=1; j<=40; j++ ) {
          // 過去7日間の cases と Rt から予測 cases を計算する
          var len = data.length ;
          var cases = (
          data[ len-7 ] * data2[ len-7 ] + 
          data[ len-6 ] * data2[ len-6 ] +
          data[ len-5 ] * data2[ len-5 ] + 
          data[ len-4 ] * data2[ len-4 ] + 
          data[ len-3 ] * data2[ len-3 ] + 
          data[ len-2 ] * data2[ len-2 ] + 
          data[ len-1 ] * data2[ len-1 ] ) / 7.0 ;
          data.push( Math.floor(cases))
          data2.push( rt );

          var dt = new Date(Date.parse( last.date ))
          dt.setDate(dt.getDate() + j);
          dt = dt.getFullYear() + "/" + (dt.getMonth()+1) + "/" + dt.getDate() 
          if ( i == 0 ) labels.push( dt )
        }

        var dataset = 
          {
            label: location,
            fill: false,
            borderColor: i >= this.colors.length? "rgba(200,200,200,0.5)": this.colors[i].n,
            data: data,
            yAxisID: "y-axis-1", 
          }
        var dataset2 = 
          {
            label: location + "Rt",
            fill: false,
            borderColor: i >= this.colors.length? "rgba(100,100,100,0.5)": this.colors[i].ave,
            data: data2,
            yAxisID: "y-axis-2", 
          }
        datasets.push( dataset )
        datasets.push( dataset2 )
        i++;
      })
      return { labels3: labels, datasets3: datasets };
    },


    async getData() {
      var url = "https://sample-moonmile.azurewebsites.net/api/NHKCovid"
      // var url = "http://localhost:7071/api/NHKCovid"
      var res = await axios.get(url);

      var { labels, datasets } = this.makeCases( res, "2020-10-01", "2021-12-31", this.value )
      var { labels2, datasets2 } = this.makeCasesRt( res, "2020-10-01", "2021-12-31", this.value )
      this.datax.labels = labels ;
      this.datax.datasets = datasets ;
      this.datart.labels = labels2 ;
      this.datart.datasets = datasets2 ;

      var { labels3, datasets3 } = this.makeCasesFuture( res, "2020-12-01", "2021-12-31", this.value )
      this.datafu.labels = labels3 ;
      this.datafu.datasets = datasets3 ;

      // 再描画の代わり
      this.datax = JSON.parse(JSON.stringify(this.datax));
      this.datart = JSON.parse(JSON.stringify(this.datart));
      this.datafu = JSON.parse(JSON.stringify(this.datafu));
      // this.datax = Vue.util.extend({}, this.datax);
      // console.log( this.datax )
    },
    /// 都道府県の選択時
    selectLocation() {
      this.getData()
    }
  }
};
</script>

