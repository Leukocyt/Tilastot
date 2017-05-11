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
    [Authorize(Roles = "Raportit")]
    public class RaportitController : Controller
    {
        private kulutEntities db = new kulutEntities();

        // GET: kuluts
        public ActionResult Index(DateTime? start, DateTime? end, string[] sList = null, int resMax = 5)
        {
            RaportitViewModel rVM = new RaportitViewModel();
            ////var result = persons.Where(p => p.Locations.Any(l => searachIds.Contains(l.Id)));
            DateTime startForQuery, endForQuery;
            if( start == null)
            {
                startForQuery = DateTime.Now.AddMonths(-2);
                endForQuery = DateTime.Now;
            } else
            {
                startForQuery = start.Value;
                endForQuery = end.Value;
            }
            DateTime d = DateTime.Now;
            string tyyppiLista = "";
            using (var db = new kulutEntities())
            {
                List<kulutyypit> tList = db.kulutyypit.ToList();
                List<int?> types = null;
                if (sList == null)
                {
                    types = (tList.Select(x => (int?)x.id).ToList());
                } else
                {
                    types = commonFunctions.formIntList(sList);
                }
                tyyppiLista = commonFunctions.formStringList(types);
                List<S_Menot_Per_Viikko_Result> weeklySum = db.S_Menot_Per_Viikko(startForQuery,endForQuery, tyyppiLista, "", (int)reports_kuluTyypit.weeknumber, resMax ).ToList();
                rVM.gList.Add(new flotGraphModel { flotResultList = (from s in weeklySum select new object[] { s.weekNumber, s.summa }).ToList(), graphTitle = "Viikottaiset kulut" });
                weeklySum = db.S_Menot_Per_Viikko(startForQuery, endForQuery, tyyppiLista, "", (int)reports_kuluTyypit.type, resMax).ToList();
                rVM.gList.Add(new flotGraphModel { flotResultList = (from s in weeklySum select new object[] { s.weekNumber, s.summa }).ToList(), graphTitle = "Kulut tyypeittäin" });
                rVM.typeList = commonFunctions.filterList(tList, sList);
                rVM.paikkaListaus = db.F_Menot_Per_Viikko(startForQuery, endForQuery, tyyppiLista, "", (int)reports_kuluTyypit.place, resMax).ToList(); 
            }
            try
            {
                rVM.totalSum = rVM.paikkaListaus.Sum(x => x.summa.Value);
            } catch(Exception e) { }
            rVM.start = startForQuery;
            rVM.end = endForQuery;
            ViewBag.start = startForQuery;
            ViewBag.end = endForQuery;
            ViewBag.resMax = resMax;
            ViewBag.tyyppiLista = tyyppiLista;
            return View(rVM);
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #region helpers



        #endregion
    }
}
