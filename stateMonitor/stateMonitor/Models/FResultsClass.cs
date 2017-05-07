using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stateMonitor.Models
{
    public class FResultsClass
    {

        public List<F_ROWS_Result> fList;
        public List<object[]> flotResultList;
        public List<object[]> flotResultList2;
        public List<graphItem> resultLists = new List<graphItem>();
        public List<barGraphRes> barGraphRes;
        public List<string> channels;
        public List<hourGroup> hList;
        public FResultsClass()
        {
            fList = new List<F_ROWS_Result>();
            flotResultList = new List<object[]>();
            flotResultList2 = new List<object[]>();
            channels = new List<string>();
            hList = new List<hourGroup>();
            resultLists = new List<graphItem>();
        }

        public void fillFlotArray()
        {
            try
            {
                int i, c = 0;
                c = fList.Count();
                fList = fList.OrderByDescending(o => o.Count).ToList();
                for (i = 0; i < c; i++)
                {
                    flotResultList.Add(new object[] { fList[i].nick, fList[i].Count });
                }

            }
            catch (Exception e)
            {

            }
        }
        public void fillFromHourList()
        {
            try
            {
                int i, c = 0;
                c = hList.Count();
                hList = hList.OrderBy(h => h.hour).ToList();
                for (i = 0; i < c; i++)
                {
                    flotResultList2.Add(new object[] { hList[i].hour, hList[i].count });
                }
            } catch (Exception e) { }
        }
    }

    public  class graphItem {
        public string title;
        public List<object[]> resultList;
        public graphItem()
        {
            resultList = new List<object[]>();
        }
    }
}