<template>
    <div>
        <v-select
            :items="locations"
            item-text="name"
            item-value="name"
            v-model="value"
            label="都道府県"
            dense
            multiple
            @input="selectLocation"
            ></v-select>
        <h3>陽性者数</h3>      
        <line-chart :chart-data="datax" :options="options"/>
        <h3>陽性者数(100万人あたり)</h3>      
        <line-chart :chart-data="datax2" :options="options"/>
        <h3>実効再生産数 Rt</h3>      
        <line-chart :chart-data="datart" :options="options"/>
        <h3>陽性者数予測</h3>      
        <line-chart :chart-data="datafu" :options="optionsfu"/>
        <div style="font-size: 0.8em">
            陽性者数予測値は、過去7日間の陽性者数と実効再生産数Rtから計算しています。
            予測値は、最終のRtをそのまま適用しています。
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import LineChart from './LineChart.vue';

export default {
  name: '',
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
      datax2: {
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
      { code: 1, name: "北海道", population: 5381733 },
      { code: 2, name: "青森県", population: 1308265 },
      { code: 3, name: "岩手県", population: 1279594 },
      { code: 4, name: "宮城県", population: 2333899 },
      { code: 5, name: "秋田県", population: 1023119 },
      { code: 6, name: "山形県", population: 1123891 },
      { code: 7, name: "福島県", population: 1914039 },
      { code: 8, name: "茨城県", population: 2916976 },
      { code: 9, name: "栃木県", population: 1974255 },
      { code: 10, name: "群馬県", population: 1973115 },
      { code: 11, name: "埼玉県", population: 7266534 },
      { code: 12, name: "千葉県", population: 6222666 },
      { code: 13, name: "東京都", population: 13515271 },
      { code: 14, name: "神奈川県", population: 9126214 },
      { code: 15, name: "新潟県", population: 2304264 },
      { code: 16, name: "富山県", population: 1066328 },
      { code: 17, name: "石川県", population: 1154008 },
      { code: 18, name: "福井県", population: 786740 },
      { code: 19, name: "山梨県", population: 834930 },
      { code: 20, name: "長野県", population: 2098804 },
      { code: 21, name: "岐阜県", population: 2031903 },
      { code: 22, name: "静岡県", population: 3700305 },
      { code: 23, name: "愛知県", population: 7483128 },
      { code: 24, name: "三重県", population: 1815865 },
      { code: 25, name: "滋賀県", population: 1412916 },
      { code: 26, name: "京都府", population: 2610353 },
      { code: 27, name: "大阪府", population: 8839469 },
      { code: 28, name: "兵庫県", population: 5534800 },
      { code: 29, name: "奈良県", population: 1364316 },
      { code: 30, name: "和歌山県", population: 963579 },
      { code: 31, name: "鳥取県", population: 573441 },
      { code: 32, name: "島根県", population: 694352 },
      { code: 33, name: "岡山県", population: 1921525 },
      { code: 34, name: "広島県", population: 2843990 },
      { code: 35, name: "山口県", population: 1404729 },
      { code: 36, name: "徳島県", population: 755733 },
      { code: 37, name: "香川県", population: 976263 },
      { code: 38, name: "愛媛県", population: 1385262 },
      { code: 39, name: "高知県", population: 728276 },
      { code: 40, name: "福岡県", population: 5101556 },
      { code: 41, name: "佐賀県", population: 832832 },
      { code: 42, name: "長崎県", population: 1377187 },
      { code: 43, name: "熊本県", population: 1786170 },
      { code: 44, name: "大分県", population: 1166338 },
      { code: 45, name: "宮崎県", population: 1104069 },
      { code: 46, name: "鹿児島県", population: 1648177 },
      { code: 47, name: "沖縄県", population: 1433566 },
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
     * 感染者数のグラフ(100万人あたり)を作成
     */
    makeCases2(res,start_date,end_date,locations) {

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

              var lo = this.locations.find( it => it.name == location )
              data.push( el.cases /(lo.population/1000000))
              data2.push( el.casesAverage/(lo.population/1000000) )
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
            label: location + "週平均Rt",
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

      var data = this.makeCases2( res, "2020-10-01", "2021-12-31", this.value )
      this.datax2.labels = data.labels ;
      this.datax2.datasets = data.datasets ;


      // 再描画の代わり
      this.datax = JSON.parse(JSON.stringify(this.datax));
      this.datax2 = JSON.parse(JSON.stringify(this.datax2));
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
}
</script>

<style scoped>
h3 {
    margin-top: 1.5em;
}
</style>