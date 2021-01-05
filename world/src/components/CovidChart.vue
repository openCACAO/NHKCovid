<template>
    <div>
        <v-select
            :items="locations"
            item-text="name"
            item-value="name"
            v-model="value"
            label="国名"
            dense
            multiple
            @input="selectLocation"
            ></v-select>
        <h3>陽性者数</h3>      
        <line-chart :chart-data="datax" :options="options"/>
        <h3>陽性者数(100万人あたり)</h3>      
        <line-chart :chart-data="datax2" :options="options"/>
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

      options: {
        responsive: true,
        maintainAspectRatio: false
      },

      value: ["Japan"],
      locations: [

{ code: 0, name: "Afghanistan",	population: 38041800 },
{ code: 0, name: "Albania",	population: 2880900 },
{ code: 0, name: "Algeria",	population: 43053100 },
// { code: 0, name: "American Samoa",	population: 55300 },
{ code: 0, name: "Andorra",	population: 77100 },
{ code: 0, name: "Angola",	population: 31825300 },
//{ code: 0, name: "Anguilla",	population: 14900 },
//{ code: 0, name: "Antigua and Barbuda",	population: 97100 },
{ code: 0, name: "Argentina",	population: 44780700 },
{ code: 0, name: "Armenia",	population: 2957700 },
// { code: 0, name: "Aruba",	population: 106300 },
// { code: 0, name: "Australia",	population: 25203200 },
{ code: 0, name: "Austria",	population: 8955100 },
{ code: 0, name: "Azerbaijan",	population: 10047700 },
{ code: 0, name: "Bahamas",	population: 389500 },
{ code: 0, name: "Bahrain",	population: 1641200 },
{ code: 0, name: "Bangladesh",	population: 163046200 },
{ code: 0, name: "Barbados",	population: 287000 },
{ code: 0, name: "Belarus",	population: 9452400 },
{ code: 0, name: "Belgium",	population: 11539300 },
{ code: 0, name: "Belize",	population: 390400 },
{ code: 0, name: "Benin",	population: 11801200 },
//{ code: 0, name: "Bermuda",	population: 62500 },
{ code: 0, name: "Bhutan",	population: 763100 },
{ code: 0, name: "Bolivia",	population: 11513100 },
{ code: 0, name: "Bosnia and Herzegovina",	population: 3301000 },
{ code: 0, name: "Botswana",	population: 2303700 },
{ code: 0, name: "Brazil",	population: 211049500 },
//{ code: 0, name: "British Virgin Islands",	population: 30000 },
{ code: 0, name: "Brunei",	population: 433300 },
{ code: 0, name: "Bulgaria",	population: 7000100 },
{ code: 0, name: "Burkina Faso",	population: 20321400 },
{ code: 0, name: "Burundi",	population: 11530600 },
{ code: 0, name: "Cabo Verde",	population: 549900 },
{ code: 0, name: "Cambodia",	population: 16486500 },
{ code: 0, name: "Cameroon",	population: 25876400 },
//{ code: 0, name: "Canada",	population: 37411000 },
//{ code: 0, name: "Cayman Islands",	population: 64900 },
{ code: 0, name: "Central African Republic",	population: 4745200 },
{ code: 0, name: "Chad",	population: 15946900 },
{ code: 0, name: "Chile",	population: 18952000 },
//{ code: 0, name: "China",	population: 1433783700 },
//{ code: 0, name: "China, Hong Kong SAR",	population: 7436200 },
//{ code: 0, name: "China, Macao SAR",	population: 640400 },
{ code: 0, name: "Colombia",	population: 50339400 },
{ code: 0, name: "Comoros",	population: 850900 },
{ code: 0, name: "Congo (Brazzaville)",	population: 5380500 },
{ code: 0, name: "Congo (Kinshasa)",	population: 17500 },
{ code: 0, name: "Costa Rica",	population: 5047600 },
{ code: 0, name: "Cote d'Ivoire",	population: 25716500 },
{ code: 0, name: "Croatia",	population: 4130300 },
{ code: 0, name: "Cuba",	population: 11333500 },
//{ code: 0, name: "Curacao",	population: 163400 },
{ code: 0, name: "Cyprus",	population: 1198600 },
{ code: 0, name: "Czechia",	population: 10689200 },
//{ code: 0, name: "Dem. People's Rep. Korea",	population: 25666200 },
//{ code: 0, name: "Dem. Rep. of the Congo",	population: 86790600 },
{ code: 0, name: "Denmark",	population: 5771900 },
{ code: 0, name: "Djibouti",	population: 973600 },
{ code: 0, name: "Dominica",	population: 71800 },
{ code: 0, name: "Dominican Republic",	population: 10739000 },
{ code: 0, name: "Ecuador",	population: 17373700 },
{ code: 0, name: "Egypt",	population: 100388100 },
{ code: 0, name: "El Salvador",	population: 6453600 },
{ code: 0, name: "Equatorial Guinea",	population: 1356000 },
{ code: 0, name: "Eritrea",	population: 3497100 },
{ code: 0, name: "Estonia",	population: 1325600 },
{ code: 0, name: "Eswatini",	population: 1148100 },
{ code: 0, name: "Ethiopia",	population: 112078700 },
//{ code: 0, name: "Falkland Islands Malvinas",	population: 3400 },
//{ code: 0, name: "Faroe Islands",	population: 48700 },
{ code: 0, name: "Fiji",	population: 890000 },
{ code: 0, name: "Finland",	population: 5532200 },
{ code: 0, name: "France",	population: 65129700 },
//{ code: 0, name: "French Guiana",	population: 290800 },
//{ code: 0, name: "French Polynesia",	population: 279300 },
{ code: 0, name: "Gabon",	population: 2172600 },
{ code: 0, name: "Gambia",	population: 2347700 },
{ code: 0, name: "Georgia",	population: 3996800 },
{ code: 0, name: "Germany",	population: 83517000 },
{ code: 0, name: "Ghana",	population: 30417900 },
//{ code: 0, name: "Gibraltar",	population: 33700 },
{ code: 0, name: "Greece",	population: 10473500 },
//{ code: 0, name: "Greenland",	population: 56700 },
{ code: 0, name: "Grenada",	population: 112000 },
//{ code: 0, name: "Guadeloupe",	population: 400100 },
//{ code: 0, name: "Guam",	population: 167300 },
{ code: 0, name: "Guatemala",	population: 17581500 },
{ code: 0, name: "Guinea",	population: 12771200 },
{ code: 0, name: "Guinea-Bissau",	population: 1920900 },
{ code: 0, name: "Guyana",	population: 782800 },
{ code: 0, name: "Haiti",	population: 11263100 },
{ code: 0, name: "Holy See",	population: 800 },
{ code: 0, name: "Honduras",	population: 9746100 },
{ code: 0, name: "Hungary",	population: 9684700 },
{ code: 0, name: "Iceland",	population: 339000 },
{ code: 0, name: "India",	population: 1366417800 },
{ code: 0, name: "Indonesia",	population: 270625600 },
{ code: 0, name: "Iran",	population: 82913900 },
{ code: 0, name: "Iraq",	population: 39309800 },
{ code: 0, name: "Ireland",	population: 4882500 },
//{ code: 0, name: "Isle of Man",	population: 84600 },
{ code: 0, name: "Israel",	population: 8519400 },
{ code: 0, name: "Italy",	population: 60550100 },
{ code: 0, name: "Jamaica",	population: 2948300 },
{ code: 0, name: "Japan",	population: 126860300 },
{ code: 0, name: "Jordan",	population: 10101700 },
{ code: 0, name: "Kazakhstan",	population: 18551400 },
{ code: 0, name: "Kenya",	population: 52574000 },
//{ code: 0, name: "Kiribati",	population: 117600 },
{ code: 0, name: "Korea South",	population: 51225300 },
{ code: 0, name: "Kuwait",	population: 4207100 },
{ code: 0, name: "Kyrgyzstan",	population: 6415900 },
{ code: 0, name: "Lao",	population: 7169500 },
{ code: 0, name: "Latvia",	population: 1906700 },
{ code: 0, name: "Lebanon",	population: 6855700 },
{ code: 0, name: "Lesotho",	population: 2125300 },
{ code: 0, name: "Liberia",	population: 4937400 },
{ code: 0, name: "Libya",	population: 6777500 },
{ code: 0, name: "Liechtenstein",	population: 38000 },
{ code: 0, name: "Lithuania",	population: 2759600 },
{ code: 0, name: "Luxembourg",	population: 615700 },
{ code: 0, name: "Madagascar",	population: 26969300 },
{ code: 0, name: "Malawi",	population: 18628700 },
{ code: 0, name: "Malaysia",	population: 31949800 },
{ code: 0, name: "Maldives",	population: 531000 },
{ code: 0, name: "Mali",	population: 19658000 },
{ code: 0, name: "Malta",	population: 440400 },
{ code: 0, name: "Marshall Islands",	population: 58800 },
//{ code: 0, name: "Martinique",	population: 375600 },
{ code: 0, name: "Mauritania",	population: 4525700 },
{ code: 0, name: "Mauritius",	population: 1269700 },
//{ code: 0, name: "Mayotte",	population: 266200 },
{ code: 0, name: "Mexico",	population: 127575500 },
//{ code: 0, name: "Micronesia Fed. States of",	population: 113800 },
{ code: 0, name: "Monaco",	population: 39000 },
{ code: 0, name: "Mongolia",	population: 3225200 },
{ code: 0, name: "Montenegro",	population: 628000 },
//{ code: 0, name: "Montserrat",	population: 5000 },
{ code: 0, name: "Morocco",	population: 36471800 },
{ code: 0, name: "Mozambique",	population: 30366000 },
//{ code: 0, name: "Myanmar",	population: 54045400 },
{ code: 0, name: "Namibia",	population: 2494500 },
//{ code: 0, name: "Nauru",	population: 10800 },
{ code: 0, name: "Nepal",	population: 28608700 },
{ code: 0, name: "Netherlands",	population: 17097100 },
//{ code: 0, name: "New Caledonia",	population: 282800 },
{ code: 0, name: "New Zealand",	population: 4783100 },
{ code: 0, name: "Nicaragua",	population: 6545500 },
{ code: 0, name: "Niger",	population: 23310700 },
{ code: 0, name: "Nigeria",	population: 200963600 },
//{ code: 0, name: "Niue",	population: 1600 },
{ code: 0, name: "North Macedonia",	population: 2083500 },
//{ code: 0, name: "Northern Mariana Islands",	population: 57200 },
{ code: 0, name: "Norway",	population: 5378900 },
{ code: 0, name: "Oman",	population: 4975000 },
{ code: 0, name: "Pakistan",	population: 216565300 },
//{ code: 0, name: "Palau",	population: 18000 },
{ code: 0, name: "Panama",	population: 4246400 },
{ code: 0, name: "Papua New Guinea",	population: 8776100 },
{ code: 0, name: "Paraguay",	population: 7044600 },
{ code: 0, name: "Peru",	population: 32510500 },
{ code: 0, name: "Philippines",	population: 108116600 },
{ code: 0, name: "Poland",	population: 37887800 },
{ code: 0, name: "Portugal",	population: 10226200 },
//{ code: 0, name: "Puerto Rico",	population: 2933400 },
{ code: 0, name: "Qatar",	population: 2832100 },
//{ code: 0, name: "Republic of Moldova",	population: 4043300 },
//{ code: 0, name: "Reunion",	population: 888900 },
{ code: 0, name: "Romania",	population: 19364600 },
{ code: 0, name: "Russian",	population: 145872300 },
{ code: 0, name: "Rwanda",	population: 12627000 },
//{ code: 0, name: "Saint Barthelemy",	population: 9800 },
//{ code: 0, name: "Saint Helena",	population: 6100 },
{ code: 0, name: "Saint Kitts and Nevis",	population: 52800 },
{ code: 0, name: "Saint Lucia",	population: 182800 },
//{ code: 0, name: "Saint Martin French part",	population: 38000 },
//{ code: 0, name: "Saint Pierre and Miquelon",	population: 5800 },
{ code: 0, name: "Saint Vincent and the Grenadines",	population: 110600 },
{ code: 0, name: "Samoa",	population: 197100 },
{ code: 0, name: "San Marino",	population: 33900 },
{ code: 0, name: "Sao Tome and Principe",	population: 215100 },
{ code: 0, name: "Saudi Arabia",	population: 34268500 },
{ code: 0, name: "Senegal",	population: 16296400 },
{ code: 0, name: "Serbia",	population: 8772200 },
{ code: 0, name: "Seychelles",	population: 97700 },
{ code: 0, name: "Sierra Leone",	population: 7813200 },
{ code: 0, name: "Singapore",	population: 5804300 },
//{ code: 0, name: "Sint Maarten Dutch part",	population: 42400 },
{ code: 0, name: "Slovakia",	population: 5457000 },
{ code: 0, name: "Slovenia",	population: 2078700 },
{ code: 0, name: "Solomon Islands",	population: 669800 },
{ code: 0, name: "Somalia",	population: 15442900 },
{ code: 0, name: "South Africa",	population: 58558300 },
{ code: 0, name: "South Sudan",	population: 11062100 },
{ code: 0, name: "Spain",	population: 46736800 },
{ code: 0, name: "Sri Lanka",	population: 21323700 },
//{ code: 0, name: "State of Palestine",	population: 4981400 },
{ code: 0, name: "Sudan",	population: 42813200 },
{ code: 0, name: "Suriname",	population: 581400 },
{ code: 0, name: "Sweden",	population: 10036400 },
{ code: 0, name: "Switzerland",	population: 8591400 },
{ code: 0, name: "Syrian Arab Republic",	population: 17070100 },
{ code: 0, name: "Taiwan*", population: 23570000 },
{ code: 0, name: "Tajikistan",	population: 9321000 },
{ code: 0, name: "Thailand",	population: 69625600 },
{ code: 0, name: "Timor-Leste",	population: 1293100 },
{ code: 0, name: "Togo",	population: 8082400 },
//{ code: 0, name: "Tokelau",	population: 1300 },
//{ code: 0, name: "Tonga",	population: 104500 },
{ code: 0, name: "Trinidad and Tobago",	population: 1395000 },
{ code: 0, name: "Tunisia",	population: 11694700 },
{ code: 0, name: "Turkey",	population: 83429600 },
//{ code: 0, name: "Turkmenistan",	population: 5942100 },
//{ code: 0, name: "Turks and Caicos Islands",	population: 38200 },
//{ code: 0, name: "Tuvalu",	population: 11600 },
{ code: 0, name: "Uganda",	population: 44269600 },
{ code: 0, name: "Ukraine",	population: 43993600 },
{ code: 0, name: "United Arab Emirates",	population: 9770500 },
{ code: 0, name: "United Kingdom",	population: 67530200 },
//{ code: 0, name: "United Rep. of Tanzania",	population: 58005500 },
{ code: 0, name: "US",	population: 329064900 },
//{ code: 0, name: "United States Virgin Islands",	population: 104600 },
{ code: 0, name: "Uruguay",	population: 3461700 },
{ code: 0, name: "Uzbekistan",	population: 32981700 },
{ code: 0, name: "Vanuatu",	population: 299900 },
{ code: 0, name: "Venezuela",	population: 28515800 },
{ code: 0, name: "Vietnam", poplulation: 95540000 },
//{ code: 0, name: "Viet Nam",	population: 96462100 },
//{ code: 0, name: "Wallis and Futuna Islands",	population: 11400 },
//{ code: 0, name: "Western Sahara",	population: 582500 },
{ code: 0, name: "Yemen",	population: 29161900 },
{ code: 0, name: "Zambia",	population: 17861000 },
{ code: 0, name: "Zimbabwe",	population: 14645500 }, 


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

    async getData() {
      var url = process.env.VUE_APP_WORLD_COVID_API_URL 

      console.log( url )
      var res = await axios.get(url);

      var data = this.makeCases( res, "2020-10-01", "2021-12-31", this.value )
      this.datax.labels = data.labels ;
      this.datax.datasets = data.datasets ;

      data = this.makeCases2( res, "2020-10-01", "2021-12-31", this.value )
      this.datax2.labels = data.labels ;
      this.datax2.datasets = data.datasets ;
      console.log( this.datax2 )

      // 再描画の代わり
      this.datax = JSON.parse(JSON.stringify(this.datax));
      this.datax2 = JSON.parse(JSON.stringify(this.datax2));
    },
    /// 国の選択時
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