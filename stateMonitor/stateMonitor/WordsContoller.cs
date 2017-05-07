using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using stateMonitor.Models;

namespace stateMonitor
{
    public class wordsController : Controller
    {
        private internetEntities1 db = new internetEntities1();

        // GET: irkkis
        public ActionResult Index(string word, string channel = "#g6")
        {
            //Kikka...
            if (word != null && word.Length > 1)
            {
                int c = word.Length;
                if (word[c - 1] == '/')
                {
                    word = word.Substring(0, c - 1);
                }
            } else
            {
                word = null;
            }
            var query = from irkki in db.irkki
                        where
                            irkki.kanava == channel && (irkki.viesti.Contains(word) || word == null) && irkki.old != 1                             
                        group irkki by irkki.nick into tulos
                        select new nickStats
                        {
                            nick = tulos.Key,
                            count = tulos.Count()
                        };
            ViewBag.word = @word;
            List<nickStats> stats = query.OrderByDescending(x => x.count).ToList();
            wordsViewModel wVM = new wordsViewModel();
            wVM.channels = db.irkki.Select(x => x.kanava).Distinct().ToList();
            wVM.nStats = stats.ToList();
            wVM.resultLists.Add(new graphItem { title = "Vuositilastot", resultList = getYearStats(channel, word) });
            ViewBag.selectedChannel = channel;
            return View( wVM);
        }

        public ActionResult Details(string word, string nick, string channel, string year = "")
        {
            int yearI = -1;
            if( year.Length > 0)
            {
                try
                {
                    yearI = int.Parse(year);
                } catch(Exception e)
                {
                }
            }
            //Kikka...
            if (word != null)
            {
                int c = word.Length;
                if (word[c - 1] == '/')
                {
                    word = word.Substring(0, c - 1);
                }
            }
            List<irkki> L;
            if (yearI > -1)
            {
                DateTime qStart = new DateTime(yearI, 1, 1);
                DateTime qEnd = new DateTime(yearI + 1, 1, 1);
                L = db.irkki.Where(i => i.viesti.Contains(word) && i.nick == nick && i.kanava.ToLower() == channel.ToLower() && i.aika >= qStart && i.aika <= qEnd).OrderByDescending(i => i.aika).ToList();
            }
            else
            {
                L = db.irkki.Where(i => i.viesti.Contains(word) && i.nick == nick && i.kanava.ToLower() == channel.ToLower()).OrderByDescending(i => i.aika).ToList();
            }
            return View(L.ToList());
        }
        public ActionResult statsDetails(string word, string channel, string nick)
        {
            wordsViewModel wVM = new wordsViewModel();
            List<object[]> yStats = getYearStats(channel, word, nick);
            List<nickStats> yList = (from i in yStats select new nickStats { nick = i[0].ToString(), count = (int)i[1] }).ToList();
            wVM.nStats = yList;
            wVM.resultLists.Add(new graphItem { title = "Vuosi", resultList = yStats });
            ViewBag.word = word;
            ViewBag.selectedChannel = channel;
            ViewBag.nick = nick;
            return View(wVM);
        }
        private List<object[]> getYearStats(string channel, string word, string nick = "")
        {
            var query = (from i in db.irkki
                         where i.viesti.Contains(word) && i.kanava.ToLower() == channel.ToLower() && (nick == "" || i.nick.ToLower() == nick.ToLower()) && i.old != 1
                         group i by i.aika.Value.Year into g
                         ///select new dateTimeSeries  { time = new DateTime(g.Key.Year,g.Key.Month,1), value =  g.Count() });
                         select new monthRes { year = g.Key, month = 0, count = g.Count() });
            query = query.OrderBy(x => x.year).ThenBy(x => x.month);
            List<monthRes> r = query.ToList();
            //r = r.OrderBy(x => (int)x[0]).ThenBy(x => (int) x[1]).ToList();
            List<object[]> retList = (from i in r
                                      select new object[] { i.year.ToString(), i.count }).ToList();
            return retList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
