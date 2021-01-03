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
        [FunctionName("NHKCovid")]
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

                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    // T•½‹Ï‚ğŒvZ
                    calcCasesAve(data);
                    // T’PˆÊRt’l‚ğŒvZ
                    calcCasesRt(data);
                    // T’PˆÊRt•½‹Ï’l‚ğŒvZ
                    calcCasesRtAve(data);

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    log.LogInformation("time " + ts.ToString());

                    return new OkObjectResult(new { result = data });
                }
            } 
            catch
            {
                return new NotFoundResult();
            }
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

        public class Covid
        {
            public DateTime Date { get; set; }
            public int LocationId { get; set; }
            public string Location { get; set; }
            public int Cases { get; set; }
            public int CasesTotal { get; set; }
            public int Deaths { get; set; }
            public int DeathsTotal { get; set; }

            public float CasesAverage { get; set; }   // TˆÚ“®•½‹Ï
            public float CasesRt { get; set; }        // T’PˆÊRt’l = ‘±‚­1TŠÔ‚ÌŠ´õÒ”•½‹Ï / “–“úŠ´õÒ”  
            public float CasesRtAverage { get; set; }     // Rt’l‚ÌTˆÚ“®•½‹Ï

        }
    }
}
