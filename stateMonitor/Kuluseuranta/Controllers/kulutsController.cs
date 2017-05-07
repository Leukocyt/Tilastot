using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kuluseuranta.Models;

namespace Kuluseuranta.Controllers
{
    public class kulutsController : Controller
    {
        private kulutEntities db = new kulutEntities();

        // GET: kuluts
        public ActionResult Index(DateTime? start, DateTime? end, string[] sList = null, string plainList = "")
        {

            if (plainList != "")
                sList = plainList.Split(',');
            KulutViewModel m = new KulutViewModel();
            if (start == null && end == null)
            {
                m.periodStart = DateTime.Now.AddDays(-14);
                m.periodEnd = DateTime.Now;
            } else
            {
                m.periodStart = start.Value;
                m.periodEnd = end.Value;
            }            
            using (var db = new kulutEntities())
            {
                m.tyypit = db.kulutyypit.ToList();
                List<int?> types = null;
                if( sList == null)
                {
                    types = (m.tyypit.Select(x => (int?)x.id ).ToList());
                } else
                {
                    types =  commonFunctions.formIntList(sList);
                }
                m.kulut = db.kulut.Include("paikat").Include("kulutyypit").Where(x => x.timestamp >= m.periodStart && x.timestamp <= m.periodEnd && types.Contains(x.tyyppi) ).OrderByDescending(x => x.timestamp).ToList();                
                m.typeList = commonFunctions.filterList(m.tyypit, sList);
            }
            ////var result = persons.Where(p => p.Locations.Any(l => searachIds.Contains(l.Id)));
            m.kulujenSumma = m.kulut.Sum(x => x.maara.Value);
            ViewBag.start = m.periodStart;
            ViewBag.end = m.periodEnd;
            return View(m);
        }


        public ActionResult IndexWeekNumber(int WeekNumber, string sList = null, int year = -1)
        {
            if (year == -1) year = DateTime.Now.Year;
            DateTime start = commonFunctions.FirstDateOfWeekISO8601(year, WeekNumber);
            DateTime end = start.AddDays(7).AddMilliseconds(-1);
            return RedirectToAction("Index", new { start = start, end = end, plainList = sList});
        }

        // GET: kuluts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kulut kulut = db.kulut.Find(id);
            if (kulut == null)
            {
                return HttpNotFound();
            }
            return View(kulut);
        }

        // GET: kuluts/Create
        public ActionResult Create()
        {
            //Tyypit
            List<kulutyypit> tyypit = db.kulutyypit.ToList();
            tyypit.Add(new kulutyypit { id = 0, kuvaus = "Lisää uusi!" });
            //Paikat
            List<paikat> paikat = db.paikat.ToList();
            paikat.Add(new paikat { rowid = 0, selite = "Lisää uusi!" });
            ViewBag.tyyppi = new SelectList(tyypit , "id", "kuvaus");
            ViewBag.paikkaID = new SelectList(paikat, "rowid", "selite");
            return View();
        }

        // POST: kuluts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rowid,timestamp,tyyppi,paikkaID,maara,selite")] kulut kulut, string uusiTyyppi = "", string uusiPaikka = "")
        {
            if (ModelState.IsValid)
            {

                if( kulut.tyyppi== 0) //Uusi tyyppi
                {
                    db.kulutyypit.Add(new kulutyypit { kuvaus = uusiTyyppi });
                    db.SaveChanges();
                    kulut.tyyppi = db.kulutyypit.Max(x => x.id);
                }
                if( kulut.paikkaID == 0)
                {
                    db.paikat.Add(new paikat { selite = uusiPaikka, kaupunki = "" });
                    db.SaveChanges();
                    kulut.paikkaID = db.paikat.Max(x => x.rowid);
                }
                db.kulut.Add(kulut);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tyyppi = new SelectList(db.kulutyypit, "id", "kuvaus", kulut.tyyppi);
            ViewBag.paikkaID = new SelectList(db.paikat, "rowid", "selite", kulut.paikkaID);
            
            return View(kulut);
        }

        // GET: kuluts/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kulut kulut = db.kulut.Find(id);
            if (kulut == null)
            {
                return HttpNotFound();
            }
            //Tyypit
            List<kulutyypit> tyypit = db.kulutyypit.ToList();
            tyypit.Add(new kulutyypit { id = 0, kuvaus = "Lisää uusi!" });
            //Paikat
            List<paikat> paikat = db.paikat.ToList();
            paikat.Add(new paikat { rowid = 0, selite = "Lisää uusi!" });

            ViewBag.tyyppi = new SelectList(tyypit, "id", "kuvaus", kulut.tyyppi);
            ViewBag.paikkaID = new SelectList(paikat, "rowid", "selite", kulut.paikkaID);
            return View(kulut);
        }

        // POST: kuluts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "rowid,timestamp,tyyppi,paikkaID,maara,selite")] kulut kulut, string uusiTyyppi = "", string uusiPaikka = "")
        {
            if (ModelState.IsValid)
            {
                if (kulut.tyyppi == 0) //Uusi tyyppi
                {
                    db.kulutyypit.Add(new kulutyypit { kuvaus = uusiTyyppi });
                    db.SaveChanges();
                    kulut.tyyppi = db.kulutyypit.Max(x => x.id);
                }
                if (kulut.paikkaID == 0)
                {
                    db.paikat.Add(new paikat { selite = uusiPaikka, kaupunki = "" });
                    db.SaveChanges();
                    kulut.paikkaID = db.paikat.Max(x => x.rowid);
                }
                db.Entry(kulut).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tyyppi = new SelectList(db.kulutyypit, "id", "kuvaus", kulut.tyyppi);
            ViewBag.paikkaID = new SelectList(db.paikat, "rowid", "selite", kulut.paikkaID);
            return View(kulut);
        }

        // GET: kuluts/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kulut kulut = db.kulut.Find(id);
            if (kulut == null)
            {
                return HttpNotFound();
            }
            return View(kulut);
        }

        // POST: kuluts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            kulut kulut = db.kulut.Find(id);
            db.kulut.Remove(kulut);
            db.SaveChanges();
            return RedirectToAction("Index");
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
