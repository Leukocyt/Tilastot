using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace stateMonitor
{
    public class irkkisController : Controller
    {
        private internetEntities1 db = new internetEntities1();

        // GET: irkkis
        public ActionResult Index(DateTime? start, DateTime? end, long rowId = -1, string channel = "#G6")
        {
            DateTime qStart, qEnd;
            string limitD = "2011-09-06 20:24:59.000";
            DateTime limit = Convert.ToDateTime(limitD);
            if (start == null || end == null)
            {
                qStart = DateTime.Now.AddDays(-1);
                qEnd = DateTime.Now.AddDays(1);
            } else
            {
                qStart = start.Value;
                qEnd = end.Value.AddDays(1);
            }
            ViewBag.start = qStart;
            ViewBag.end = qEnd.AddDays(-1);
            ViewBag.rowid = rowId;
            ViewBag.selectedChannel = channel;
            stateMonitor.Models.irkkiViewModel vModel = new Models.irkkiViewModel();

            if (qStart > limit)
            {
                vModel.messageList = (from u in db.irkki
                                      where u.aika >= qStart && u.aika <= qEnd && u.kanava.ToLower() == channel.ToLower()
                                      select new Models.ircViewModel { aika = u.aika, kanava = u.kanava, maara = u.maara, nick = u.nick, viesti = u.viesti }).OrderBy(x => x.aika).Take(5000).ToList();
            } else
            {
                vModel.messageList = (from u in db.irkki_old
                                      where u.aika >= qStart && u.aika <= qEnd && u.kanava.ToLower() == channel.ToLower()
                                      select new Models.ircViewModel { aika = u.aika, kanava = u.kanava, maara = u.maara, nick = u.nick, viesti = u.viesti }).OrderBy(x => x.aika).Take(5000).ToList();
            }
            //vModel.ircList = db.irkki.Where(x => x.aika <= qEnd && x.aika >= qStart && x.kanava.ToLower() == channel.ToLower()).OrderBy(x => x.aika).Take(5000).ToList();
            vModel.channels = db.irkki.Select(x => x.kanava).Distinct().ToList();
            //List<stateMonitor.irkki> L = db.irkki.Where(x => x.aika <= qEnd && x.aika >= qStart).OrderByDescending(x => x.aika).Take(1000).ToList
            return View(vModel);
        }

        public ActionResult Spesific(DateTime d, long rowId = -1)
        {
            return RedirectToAction("Index", new { start = d.AddDays(-1), end = d.AddDays(1), rowId = rowId });
        }
        // GET: irkkis/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            irkki irkki = db.irkki.Find(id);
            if (irkki == null)
            {
                return HttpNotFound();
            }
            return View(irkki);
        }
        // GET: irkkis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: irkkis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "viesti,nick,maara,kanava,id,aika")] irkki irkki)
        {
            if (ModelState.IsValid)
            {
                db.irkki.Add(irkki);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(irkki);
        }

        // GET: irkkis/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            irkki irkki = db.irkki.Find(id);
            if (irkki == null)
            {
                return HttpNotFound();
            }
            return View(irkki);
        }

        // POST: irkkis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "viesti,nick,maara,kanava,id,aika")] irkki irkki)
        {
            if (ModelState.IsValid)
            {
                db.Entry(irkki).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(irkki);
        }
        // GET: irkkis/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            irkki irkki = db.irkki.Find(id);
            if (irkki == null)
            {
                return HttpNotFound();
            }
            return View(irkki);
        }

        // POST: irkkis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            irkki irkki = db.irkki.Find(id);
            db.irkki.Remove(irkki);
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
