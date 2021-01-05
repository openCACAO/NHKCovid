using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CrowlFunc
{
    public static class WorldCovid
    {
        [FunctionName("WorldCovid")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("called WorldCovid");
            var url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
            // CSV形式で取得
            try
            {
                var cl = new HttpClient();
                // 1行ずつ読み込み JSON 形式に変換
                var res = await cl.GetAsync(url);
                var dates = new List<DateTime>();
                var data = new List<Covid>();


                using (var st = new StreamReader(await res.Content.ReadAsStreamAsync()))
                {
                    // 1行目から日付を取得
                    //  5カラム目から日付
                    // 多分、抜けはないが念のため
                    string line = st.ReadLine();
                    var items = line.Split(",");
                    var culture = new CultureInfo("en-US");
                    for (int i = 4; i < items.Length; i++)
                    {
                        dates.Add(DateTime.Parse(items[i], culture));
                    }

                    // 2行目から国名と陽性者数
                    //  1カラム目が空白, 2カラム目に国名, 5カラム目から陽性者数
                    while (true)
                    {
                        line = st.ReadLine();
                        if (string.IsNullOrEmpty(line)) break;
                        // 国名を変換しておく。カンマ抜きにする
                        // "Korea, South"
                        // "Bonaire, Sint Eustatius and Saba"
                        line = line.Replace("\"Korea, South\"", "Korea South");
                        line = line.Replace("\"Bonaire, Sint Eustatius and Saba\"", "Bonaire Sint Eustatius and Saba");
                        items = line.Split(",");
                        if (items.Length >= dates.Count + 4)
                        {
                            if (items[0] != "") continue;
                            int pre = 0;
                            string location = items[1];

                            for ( int i=5; i<items.Length; i++ )
                            {
                                try
                                {
                                    var date = dates[i - 4];
                                    int cases = int.Parse(items[i]) - pre;
                                    pre = int.Parse(items[i]);
                                    var it = new Covid()
                                    {
                                        Date = date,
                                        Location = location,
                                        Cases = cases,
                                    };
                                    data.Add(it);
                                } 
                                catch ( Exception ex )
                                {
                                    log.LogError(ex.Message);
                                }
                            }
                        }
                    }
                    // ソートしておく
                    data = data.OrderBy(t => t.Location).ThenBy(t => t.Date).ToList();
                    // 週平均を計算
                    NHKCovid.calcCasesAve(data);
                    // 週単位Rt値を計算
                    NHKCovid.calcCasesRt(data);
                    // 週単位Rt平均値を計算
                    NHKCovid.calcCasesRtAve(data);

                    return new OkObjectResult(new { result = data });
                }
            } 
            catch ( Exception ex )
            {
                log.LogError( ex.Message);
                return new NotFoundResult();
            }
        }

        [FunctionName("WorldCovidTimerOne")]
        public static async Task<IActionResult> RunTimerOne(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Blob("covid/world.json", FileAccess.Write)] Stream jsonfile,
            ILogger log)
        {
            await WorldCovid.RunTimer(null, jsonfile, log);
            return new OkObjectResult("world.json " + DateTime.Now.ToString());
        }

        [FunctionName("WorldCovidTimer")]
        public static async Task RunTimer([TimerTrigger("0 5 * * * *")] TimerInfo myTimer,
            [Blob("covid/world.json", FileAccess.Write)] Stream jsonfile,
            ILogger log)

        {
            log.LogInformation("called WorldCovidTimer");
            var url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

            var cl = new HttpClient();
            // 1行ずつ読み込み JSON 形式に変換
            var res = await cl.GetAsync(url);
            var dates = new List<DateTime>();
            var data = new List<Covid>();


            using (var st = new StreamReader(await res.Content.ReadAsStreamAsync()))
            {
                // 1行目から日付を取得
                //  5カラム目から日付
                // 多分、抜けはないが念のため
                string line = st.ReadLine();
                var items = line.Split(",");
                var culture = new CultureInfo("en-US");
                for (int i = 4; i < items.Length; i++)
                {
                    dates.Add(DateTime.Parse(items[i], culture));
                }

                // 2行目から国名と陽性者数
                //  1カラム目が空白, 2カラム目に国名, 5カラム目から陽性者数
                while (true)
                {
                    line = st.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;
                    // 国名を変換しておく。カンマ抜きにする
                    // "Korea, South"
                    // "Bonaire, Sint Eustatius and Saba"
                    line = line.Replace("\"Korea, South\"", "Korea South");
                    line = line.Replace("\"Bonaire, Sint Eustatius and Saba\"", "Bonaire Sint Eustatius and Saba");
                    items = line.Split(",");
                    if (items.Length >= dates.Count + 4)
                    {
                        if (items[0] != "") continue;
                        int pre = 0;
                        string location = items[1];

                        for (int i = 5; i < items.Length; i++)
                        {
                            try
                            {
                                var date = dates[i - 4];
                                int cases = int.Parse(items[i]) - pre;
                                pre = int.Parse(items[i]);
                                var it = new Covid()
                                {
                                    Date = date,
                                    Location = location,
                                    Cases = cases,
                                };
                                data.Add(it);
                            }
                            catch (Exception ex)
                            {
                                log.LogError(ex.Message);
                            }
                        }
                    }
                }
                // ソートしておく
                data = data.OrderBy(t => t.Location).ThenBy(t => t.Date).ToList();
                // 週平均を計算
                NHKCovid.calcCasesAve(data);
                // 週単位Rt値を計算
                NHKCovid.calcCasesRt(data);
                // 週単位Rt平均値を計算
                NHKCovid.calcCasesRtAve(data);
            }
            var json = JsonConvert.SerializeObject(new { result = data });
            var writer = new StreamWriter(jsonfile);
            writer.Write(json);
            writer.Close();
            // return new OkObjectResult("save json " + DateTime.Now.ToString());
        }

    }
}
