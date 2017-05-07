using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kuluseuranta.Models;

namespace Kuluseuranta.Controllers
{
    public class KulutController : Controller
    {
        // GET: Kulut
        private kulutEntities db = new kulutEntities();
        public ActionResult Index(DateTime ?start, DateTime ?end)
        {
            KulutViewModel m = new KulutViewModel();
            if ( start == null && end == null)
            {
                m.periodStart = DateTime.Now.AddDays(-14);
                m.periodEnd = DateTime.Now;
            }
            using( var db = new kulutEntities())
            {
                m.kulut = db.kulut.Include("kulutyypit").Where(x => x.timestamp >= m.periodStart && x.timestamp <= m.periodEnd).OrderByDescending(x => x.timestamp).ToList();
            }
            m.kulujenSumma = m.kulut.Sum(x => x.maara.Value);
            return View(m);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.paikkaID = new SelectList(db.paikat, "rowid", "selite");
            ViewBag.tyyppiID = new SelectList(db.kulutyypit, "id", "kuvaus");
            return View();
        }
        [HttpPost]
        public ActionResult Create(kulut k)
        {
            return View();

        }

    }
}