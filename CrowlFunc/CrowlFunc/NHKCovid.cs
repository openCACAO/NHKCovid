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
using System.Linq;
using System.Diagnostics;

namespace CrowlFunc
{
    public static class NHKCovid
    {
        [FunctionName("NHKCovidRead")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("called NHKCovid");

            var url = "https://www3.nhk.or.jp/n-data/opendata/coronavirus/nhk_news_covid19_prefectures_daily_data.csv";
            // CSVŒ`®‚Åæ“¾
            try
            {
                var cl = new HttpClient();
                // 1s‚¸‚Â“Ç‚İ‚İ JSON Œ`®‚É•ÏŠ·
                var res = await cl.GetAsync(url);
                using (var st = new StreamReader(await res.Content.ReadAsStreamAsync()))
                {
                    // ƒ^ƒCƒgƒ‹‚Í“Ç‚İ”ò‚Î‚µ
                    st.ReadLine();
                    var data = new List<Covid>();
                    while (true)
                    {
                        string line = st.ReadLine();
                        if (string.IsNullOrEmpty(line)) break;
                        var items = line.Split(",");
                        if (items.Length >= 7)
                        {
                            var it = new Covid()
                            {
                                Date = DateTime.Parse(items[0]),
                                LocationId = int.Parse(items[1]),
                                Location = items[2],
                                Cases = int.Parse(items[3]),
                                CasesTotal = int.Parse(items[3]),
                                Deaths = int.Parse(items[3]),
                                DeathsTotal = int.Parse(items[3]),
                            };
                            data.Add(it);
                        }
                    }
                    // ƒ\[ƒg‚µ‚Ä‚¨‚­
                    data = data.OrderBy(t => t.LocationId).ThenBy(t => t.Date).ToList();
                    // T•½‹Ï‚ğŒvZ
                    calcCasesAve(data);
                    // T’PˆÊRt’l‚ğŒvZ
                    calcCasesRt(data);
                    // T’PˆÊRt•½‹Ï’l‚ğŒvZ
                    calcCasesRtAve(data);

                    return new OkObjectResult(new { result = data });
                }
            } 
            catch
            {
                return new NotFoundResult();
            }
        }

        [FunctionName("NHKCovid")]
        public static async Task<IActionResult> RunRead(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Blob("covid/japan.json", FileAccess.Read)] Stream jsonfile,
            ILogger log)
        {
            log.LogInformation("called NHKCovid");
            var sr = new StreamReader(jsonfile);
            var json = await sr.ReadToEndAsync();
            return new OkObjectResult(json);
        }

        [FunctionName("NHKCovidTimerOne")]
        public static async Task<IActionResult> RunTimerOne(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Blob("covid/japan.json", FileAccess.Write)] Stream jsonfile,
            ILogger log)
        {
            await NHKCovid.RunTimer(null, jsonfile, log);
            return new OkObjectResult("japan.json " + DateTime.Now.ToString());
        }

        [FunctionName("NHKCovidTimer")]
        public static async Task RunTimer([TimerTrigger("0 5 * * * *")] TimerInfo myTimer,
            [Blob("covid/japan.json", FileAccess.Write)] Stream jsonfile,
            ILogger log)

        {
            log.LogInformation("called NHKCovidTimer");
            var url = "https://www3.nhk.or.jp/n-data/opendata/coronavirus/nhk_news_covid19_prefectures_daily_data.csv";
            var cl = new HttpClient();
            // 1s‚¸‚Â“Ç‚İ‚İ JSON Œ`®‚É•ÏŠ·
            var res = await cl.GetAsync(url);
            var data = new List<Covid>();
            using (var st = new StreamReader(await res.Content.ReadAsStreamAsync()))
            {
                // ƒ^ƒCƒgƒ‹‚Í“Ç‚İ”ò‚Î‚µ
                st.ReadLine();
                while (true)
                {
                    string line = st.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;
                    var items = line.Split(",");
                    if (items.Length >= 7)
                    {
                        var it = new Covid()
                        {
                            Date = DateTime.Parse(items[0]),
                            LocationId = int.Parse(items[1]),
                            Location = items[2],
                            Cases = int.Parse(items[3]),
                            CasesTotal = int.Parse(items[3]),
                            Deaths = int.Parse(items[3]),
                            DeathsTotal = int.Parse(items[3]),
                        };
                        data.Add(it);
                    }
                }
                // ƒ\[ƒg‚µ‚Ä‚¨‚­
                data = data.OrderBy(t => t.LocationId).ThenBy(t => t.Date).ToList();
                // T•½‹Ï‚ğŒvZ
                calcCasesAve(data);
                // T’PˆÊRt’l‚ğŒvZ
                calcCasesRt(data);
                // T’PˆÊRt•½‹Ï’l‚ğŒvZ
                calcCasesRtAve(data);
            }
            var json = JsonConvert.SerializeObject(new { result = data });
            var writer = new StreamWriter(jsonfile);
            writer.Write(json);
            writer.Close();
            // return new OkObjectResult("save json " + DateTime.Now.ToString());
        }


        /// <summary>
        /// T•½‹Ï‚ğŒvZ
        /// </summary>
        /// <param name="items"></param>
        public static void calcCasesAve( List<Covid> data )
        {
#if false
            foreach ( var it in data )
            {
                int ave = data.Where(t => t.Location == it.Location)
                    .Where(t => it.Date.AddDays(-7) < t.Date && t.Date <= it.Date)
                    .Sum(t => t.Cases) / 7;
                it.CasesAverage = ave;
            }
#else
            // ‚‘¬‰»‚µ‚Ä‚¨‚­
            for (int i = 0; i < data.Count(); i++ )
            {
                if (i < 7) continue;
                // 1TŠÔ•ª‘k‚é
                if (data[i - 1].Location != data[i].Location) continue;
                if (data[i - 2].Location != data[i].Location) continue;
                if (data[i - 3].Location != data[i].Location) continue;
                if (data[i - 4].Location != data[i].Location) continue;
                if (data[i - 5].Location != data[i].Location) continue;
                if (data[i - 6].Location != data[i].Location) continue;
                data[i].CasesAverage =
                    (data[i].Cases +
                    data[i - 1].Cases +
                    data[i - 2].Cases +
                    data[i - 3].Cases +
                    data[i - 4].Cases +
                    data[i - 5].Cases +
                    data[i - 6].Cases) / 7;
            }
#endif

        }
        /// <summary>
        /// TRt‚ğŒvZ
        /// </summary>
        /// <param name="items"></param>
        public static void calcCasesRt(List<Covid> data)
        {
#if false
            var locations = data.Select(t => t.Location).Distinct();
            foreach (var lo in locations)
            {
                var items = data.Where(t => t.Location == lo).OrderBy(t => t.Date);
                var lastdate = items.LastOrDefault();
                foreach (var it in items)
                {
                    if (it.Date <= lastdate.Date.AddDays(-7))
                    {
                        var dt = it.Date;
                        if (it.Cases > 0 ) {
                            it.CasesRt = ((float)items.Where(t => dt <= t.Date && t.Date < dt.AddDays(7)).Sum(t => t.Cases) / 7.0f) / (float)it.Cases;
                        }
                    }
                }
            }
#else
            // ‚‘¬‰»‚µ‚Ä‚¨‚­
            for (int i = 0; i < data.Count()-7; i++)
            {
                var item0 = data[i];
                // 1TŠÔŒã‚Ü‚Å
                if (data[i + 1].Location != data[i].Location) continue;
                if (data[i + 2].Location != data[i].Location) continue;
                if (data[i + 3].Location != data[i].Location) continue;
                if (data[i + 4].Location != data[i].Location) continue;
                if (data[i + 5].Location != data[i].Location) continue;
                if (data[i + 6].Location != data[i].Location) continue;
                if (data[i + 7].Location != data[i].Location) continue;
                if (data[i].Cases == 0) 
                    data[i].CasesRt = 0.0f;
                else 
                    data[i].CasesRt =
                        (float)(data[i+1].Cases +
                        data[i + 2].Cases +
                        data[i + 3].Cases +
                        data[i + 4].Cases +
                        data[i + 5].Cases +
                        data[i + 6].Cases +
                        data[i + 7].Cases) / 7.0f / (float)data[i].Cases;
            }

#endif
        }
        /// <summary>
        /// RtT•½‹Ï‚ğŒvZ
        /// </summary>
        /// <param name="items"></param>
        public static void calcCasesRtAve(List<Covid> data)
        {
#if false
            var locations = data.Select(t => t.Location).Distinct();
            foreach (var lo in locations)
            {
                var items = data.Where(t => t.Location == lo).OrderBy(t => t.Date);
                var lastdate = items.LastOrDefault();
                foreach (var it in items)
                {
                    if ( it.Date <= lastdate.Date.AddDays(-14))
                    {
                        var dt = it.Date;
                        it.CasesRtAverage = items.Where(t => dt.AddDays(-7) < t.Date && t.Date <= dt).Sum(t => t.CasesRt) / 7.0f;
                    }
                }
            }
#else
            // ‚‘¬‰»‚µ‚Ä‚¨‚­
            var lastdate = data.Max(t => t.Date);

            for (int i = 0; i < data.Count() - 7; i++)
            {
                if (i < 7) continue;
                if (data[i].Date > lastdate.AddDays(-7)) continue;
                // 1TŠÔ‘k‚é
                if (data[i - 1].Location != data[i].Location) continue;
                if (data[i - 2].Location != data[i].Location) continue;
                if (data[i - 3].Location != data[i].Location) continue;
                if (data[i - 4].Location != data[i].Location) continue;
                if (data[i - 5].Location != data[i].Location) continue;
                if (data[i - 6].Location != data[i].Location) continue;
                data[i].CasesRtAverage =
                    (data[i].CasesRt +
                    data[i - 1].CasesRt +
                    data[i - 2].CasesRt +
                    data[i - 3].CasesRt +
                    data[i - 4].CasesRt +
                    data[i - 5].CasesRt +
                    data[i - 6].CasesRt) / 7.0f;
            }
#endif
        }

    }
    public class Covid
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("locationId")]
        public int LocationId { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("cases")]
        public int Cases { get; set; }
        [JsonProperty("casesTotal")]
        public int CasesTotal { get; set; }
        [JsonProperty("deaths")]
        public int Deaths { get; set; }
        [JsonProperty("deathsTotal")]
        public int DeathsTotal { get; set; }

        [JsonProperty("casesAverage")]
        public float CasesAverage { get; set; }   // TˆÚ“®•½‹Ï
        [JsonProperty("casesRt")]
        public float CasesRt { get; set; }        // T’PˆÊRt’l = ‘±‚­1TŠÔ‚ÌŠ´õÒ”•½‹Ï / “–“úŠ´õÒ”  
        [JsonProperty("casesRtAverage")]
        public float CasesRtAverage { get; set; }     // Rt’l‚ÌTˆÚ“®•½‹Ï
    }
}
