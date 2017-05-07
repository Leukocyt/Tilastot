using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stateMonitor.Models
{
    public class Line
    {
        public string content { get; set; }
        public DateTime created_on { get; set; }
    }
    public class Memo
    {
        public DateTime created_on { get; set; }
        public List<Line> lines { get; set; }
        public string name { get; set; }
    }
    public class RootObject
    {
        public Memo memo { get; set; }
    }



    public class irkkiViewModel
    {
        public List<irkki> ircList;
        public List<string> channels;
        public List<ircViewModel> messageList;
        public irkkiViewModel()
        {
            this.ircList = new List<irkki>();
            channels = new List<string>();
            messageList = new List<ircViewModel>();
        }       
    }
    public class wordsViewModel
    {
        public List<nickStats> nStats;
        public List<string> channels;
        public List<graphItem> resultLists;
        public wordsViewModel()
        {
            nStats = new List<nickStats>();
            channels = new List<string>();
            resultLists = new List<graphItem>();
        }
    }

    public class ircViewModel
    {
        public string viesti { get; set; }
        public string nick { get; set; }
        public Nullable<long> maara { get; set; }
        public string kanava { get; set; }
        public Nullable<System.DateTime> aika { get; set; }
    }
}