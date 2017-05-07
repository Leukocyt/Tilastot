using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using stateMonitor.Models;
using System.Web.UI.DataVisualization.Charting;


namespace stateMonitor.Controllers
{
    public class F_ResultsController : Controller
    {
        private internetEntities1 db = new internetEntities1();
        public ActionResult Index(DateTime ?start, DateTime ?end, int? year, string channel = "#G6")
        {
            DateTime qStart, qEnd;
            qEnd = DateTime.Now.AddDays(1);
            qStart = DateTime.Now.AddDays(-180);
            if( start != null && end != null)
            {
                qStart = start.Value;
                qEnd = end.Value.AddDays(1);
            }
            else if( year != null) {
                qStart = new DateTime(year.Value, 1, 1);
                qEnd = new DateTime(year.Value + 1, 1, 1);
            }
            else
            {
                qStart = db.irkki.Where( x => x.kanava.ToLower() == channel.ToLower()).Min(x => x.aika).Value;
            }
            ViewBag.start = qStart;
            ViewBag.end = qEnd.AddDays(-1);
            List<F_ROWS_Result> l = db.F_ROWS(qStart, qEnd, channel).OrderByDescending(x => x.Count).Take(10).ToList();
            FResultsClass retModel = new FResultsClass();
            retModel.channels = db.irkki.Select(x => x.kanava).Distinct().ToList();
            retModel.fList = l; //Nickilista
            ViewBag.selectedChannel = channel;
            //Tuntitilasto
            var query = from irkki in db.irkki
                        where
                            irkki.aika >= qStart && irkki.aika <= qEnd && irkki.kanava.ToLower() == channel.ToLower() && irkki.old != 1
                        group irkki by irkki.aika.Value.Hour into tulos
                        select new hourGroup
                        {
                            hour = tulos.Key,
                            count = tulos.Count()
                        };
            List<hourGroup> hList = query.OrderBy(x => x.hour).ToList();            
            for ( int i = 0; i < 24; i++)
            {
                if( hList.Where(x => x.hour == i).Count() < 1)
                {
                    hList.Add(new hourGroup { hour = i, count = 0 });
                }
            }
            hList = hList.OrderBy(x => x.hour).ToList();
            retModel.hList = hList;
            retModel.fillFlotArray();
            retModel.fillFromHourList();
            retModel.resultLists.Add(new graphItem { title = "Kuukausitilastot", resultList = getMonthStats(qStart, qEnd, channel) });
            //Täytetään Flot-taulu.
            return View(retModel);
            //return View(db.F_ROWS(DateTime.Now.AddDays(-180), DateTime.Now, "#G6").OrderByDescending(x => x.Count).ToList());
        }
        //Rivikuvaaja.
        public ActionResult Graph(DateTime ?start, DateTime ?end, int? width, int? height)
        {
            int h, w = 0;
            if (width != null)
                w = width.Value;
            else
                w = 400;
            if (height != null)
                h = height.Value;
            else
                h = 300;
            DateTime qStart, qEnd;
            qEnd = DateTime.Now;
            qStart = DateTime.Now.AddDays(-180);
            if (start != null && end != null)
            {
                qStart = start.Value;
                qEnd = end.Value.AddDays(1);
            }
            List<F_ROWS_Result> res = db.F_ROWS(qStart, qEnd, "#G6").OrderByDescending(x => x.Count).Take(10).ToList();
            List<string> nicks = res.Select(x => x.nick).ToList<string>();
            List<int> counts = res.Select(x => x.Count.Value).ToList();
            return File(createGraph(nicks,counts, "Jebuli", w, h), "image/png");
        }
        //Hour Chart
        public ActionResult GraphH(DateTime? start, DateTime? end, string nick = "", string channel = "")
        {
            DateTime qStart, qEnd;
            qEnd = DateTime.Now;
            qStart = DateTime.Now.AddDays(-180);
            if (start != null && end != null)
            {
                qStart = start.Value;
                qEnd = end.Value.AddDays(1);
            }
            List<int> hHours = new List<int>();
            List<long> hHoursCount = new List<long>();
            for(int i = 0; i < 24; i++)
            {
                hHours.Add(i);
                hHoursCount.Add(0);            
            }      
            var query = from irkki in db.irkki
                            where 
                                (nick == "" || irkki.nick == nick) && (channel == "" || irkki.kanava.ToLower() == channel.ToLower()) && irkki.aika >= qStart && irkki.aika <= qEnd && irkki.old != 1
                            group irkki by irkki.aika.Value.Hour into tulos
                            select new hourGroup
                            {
                                hour = tulos.Key,
                                count = tulos.Count()
                            };
            List<hourGroup> hList = query.OrderBy(x => x.hour).ToList();
            int c = hList.Count();
            List<string> nicks = new List<string>();
            for ( int i = 0; i < 24; i++)
            {
                hHoursCount[i] = 0;
                nicks.Add(i.ToString());
            }
            for(int i = 0; i < c; i++)
            {
                hHoursCount[hList[i].hour] = hList[i].count;
            }
            string title = "";
            if (nick == "")
                title = "All the nicks ";
            else
                title = nick + " ";
            title += qStart.ToString() + " -------> " + qEnd.ToString();
            return File(createGraph(nicks, hHoursCount, title), "image/png");
        }

        private byte[] createGraph(System.Collections.IEnumerable xValues, System.Collections.IEnumerable yValues, string title = "", int width = 1800, int height = 800)
        {
            string temp = @"<Chart>
                      <ChartAreas>
                        <ChartArea Name=""Default"" _Template_=""All"">
                          <AxisY>
                            <LabelStyle Font=""Verdana, 12px"" />
                          </AxisY>
                          <AxisX LineColor=""64, 64, 64, 64"" Interval=""1"">
                            <LabelStyle Font=""Verdana, 12px"" />
                          </AxisX>
                        </ChartArea>
                      </ChartAreas>
                    </Chart>";
            //here the chart is going on
            var bytes = new System.Web.Helpers.Chart(width: width , height: height, theme: temp)
            .AddSeries(
            chartType: "Column", legend: "Mindcracker Current Month Runner up",
             xValue: xValues,
             yValues: yValues).AddTitle(title)
            .GetBytes("png");
            return bytes;
        }

        public ActionResult overviewStats(string channel ="#G6")
        {
            DateTime qStart = db.irkki.Where(x => x.kanava.ToLower() == channel.ToLower()).Min(x => x.aika).Value;
            DateTime qEnd = DateTime.Now;
            var query = from irkki in db.irkki
                        where
                            irkki.kanava.ToLower() == channel.ToLower() && irkki.old != 1
                        group irkki by irkki.aika.Value.Year into tulos
                        select new hourGroup
                        {
                            hour = tulos.Key,
                            count = tulos.Count()
                        };
            List<hourGroup> yearList = query.ToList();
            FResultsClass retModel = new FResultsClass();
            List<F_ROWS_Result> l = db.F_ROWS(qStart, qEnd, channel).OrderByDescending(x => x.Count).Take(10).ToList();
            retModel.channels = db.irkki.Select(x => x.kanava).Distinct().ToList();
            retModel.fList = l;
            retModel.hList = yearList;
            //Fill flot lists.
            retModel.fillFlotArray();
            retModel.fillFromHourList();
            ViewBag.selectedChannel = channel;
            ViewBag.selectedChannelURL = channel.Replace("#", "%23");
            return View(retModel);
        }

        #region Fetcher functions
        private List<object[]> getMonthStats(DateTime start, DateTime end, string channel)
        {
            var query = (from i in db.irkki
                         where i.aika >= start && i.aika <= end && i.kanava.ToLower() == channel.ToLower() && i.old != 1
                         group i by new { i.aika.Value.Year, i.aika.Value.Month }   into g
                         ///select new dateTimeSeries  { time = new DateTime(g.Key.Year,g.Key.Month,1), value =  g.Count() });
                         select new monthRes { year = g.Key.Year, month = g.Key.Month, count = g.Count()});
            query = query.OrderBy(x => x.year).ThenBy(x => x.month);
            List<monthRes> r = query.OrderBy(x => x.year).ThenBy(x => x.month).ToList();
            //r = r.OrderBy(x => (int)x[0]).ThenBy(x => (int) x[1]).ToList();
            List<object[]> retList = (from i in r
                                      select new object[] { i.month + "." + i.year.ToString().Substring(2), i.count }).ToList();
            return retList;
        }
        #endregion
    }
}