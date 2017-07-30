using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kuluseuranta.Models;
using System.Globalization;

namespace Kuluseuranta.Controllers
{
    public enum reports_kuluTyypit
    {
        weeknumber = 0,
        type = 1,
        place = 2,
        month = 3
    }

    public static class commonFunctions
    {
        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
        public static List<SelectListItem> filterList(List<kulutyypit> tyypit, string[] selected = null)
        {
            var list = new List<SelectListItem>();
            bool sel = false;
            foreach (var v in tyypit)
            {
                sel = false;
                if (selected == null || selected.Where(x => x == v.id.ToString()).Any())
                {
                    sel = true;
                }
                list.Add(new SelectListItem { Text = v.kuvaus, Value = v.id.ToString(), Selected = sel });
            }
            return list;
        }

        public static List<int?> formIntList(string[] selected = null)
        {
            List<int?> retList = new List<int?>();
            if (selected != null)
            {
                foreach (string s in selected)
                {
                    try
                    {
                        retList.Add(Convert.ToInt32(s));
                    }
                    catch (Exception)
                    { }
                }
            }
            return retList;
        }

        public static string formStringList(List<int?> list)
        {
            try
            {
                string retVal = "";
                int i, c = 0;
                c = list.Count();
                for (i = 0; i < c; i++)
                {
                    if (i > 0)
                    {
                        retVal += ",";
                    }
                    retVal += list[i].ToString();
                }
                return retVal;
            } catch(Exception)
            {
                return "";
            }
        }
    }
}