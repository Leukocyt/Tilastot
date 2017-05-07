using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using Newtonsoft;
using stateMonitor.Models;

namespace stateMonitor.Controllers
{

    public class ListsController : Controller
    {
        // GET: Lists
        //string url = "http://otus.dy.fi:6420/data/memo?name=talvip%C3%A4iv%C3%A4t";
        public ActionResult Index(string url = "")
        {
            if( url == "")
            {
                using(var db = new internetEntities1())
                {
                    url = db.memos.OrderByDescending(x => x.rowid).First().url;
                }
            }
            var json = "";
            using ( WebClient wc = new WebClient())
            {
                json = wc.DownloadString(url);
            }
            RootObject test = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
            return View(test.memo);
        }
    }
}