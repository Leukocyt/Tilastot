using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stateMonitor.Models
{
    public class hourGroup
    {
        public int hour
        {
            get;
            set;
        }
        public long count
        {
            get;
            set;
        }
    }
    public class dateTimeSeries
    {
        public DateTime time;
        public double value;
    }
    public class barGraphRes
    {
        public string label
        {
            get;
            set;
        }
        public float yValue
        {
            get;
            set;
        }

    }

    public class monthRes
    {
        public int year;
        public int month;
        public int count;
    }

    public class nickStats
    {

        public string nick
        {
            get;
            set;
        }
        public long count
        {
            get;
            set;
        }
    }
}