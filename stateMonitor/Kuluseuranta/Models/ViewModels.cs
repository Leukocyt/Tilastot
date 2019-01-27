using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kuluseuranta.Models
{

    public class baseViewModel
    {
        public string baseUrl { get; set; }
        public HttpRequest request { get; set; }

        public baseViewModel()
        {
            baseUrl = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
            request = request;
        }
    }
    public class KulutViewModel: baseViewModel
    {
        public List<kulut> kulut { get; set; }

        public DateTime periodStart { get; set; }
        public DateTime periodEnd { get; set; }
        public double kulujenSumma { get; set; }

        public string[] selectedTypes { get; set; }


        public List<kulutyypit> tyypit {get; set;}

        public List<SelectListItem> typeList { get; set;}
        public KulutViewModel()
        {
            kulut = new List<kulut>();
            periodStart = DateTime.Now;
            periodEnd = DateTime.Now;            
        }
    }

    public class flotGraphModel:baseViewModel
    {
        public string graphTitle { get; set; }
        public List<object[]> flotResultList { get; set; }

        public List<string> addInfoRows { get; set; }

        public flotGraphModel()
        {
            flotResultList = new List<object[]>();
            addInfoRows = new List<string>();
        }
    }

    public class RaportitViewModel:baseViewModel
    {
        public List<flotGraphModel> gList { get; set; }
        public string title { get; set; }

        public DateTime ?start {get; set;}
        public DateTime? end { get; set; }
        public List<F_Menot_Per_Viikko_Result> paikkaListaus { get; set; }

        public double totalSum { get; set; }

        public RaportitViewModel()
        {

            gList = new List<flotGraphModel>();
            title = "";
            start = null;
            end = null;
            paikkaListaus = new List<F_Menot_Per_Viikko_Result>();
            totalSum = 0;
        }

        public List<SelectListItem> typeList { get; set; }
        public List<kulutyypit> tyypit { get; set; }
    }

}