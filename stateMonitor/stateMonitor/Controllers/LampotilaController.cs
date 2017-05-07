using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;


namespace stateMonitor.Controllers
{
    public class LampotilaController : Controller
    {
        // GET: Lampotilas
        private internetEntities1 db = new internetEntities1();
        public ActionResult Index()
        {
            //List<stateMonitor.lampotilaTaulu> temps = db.lampotilaTaulu.OrderByDescending(i => i.rowId).Take(30).ToList();
            Chart c = testGraph();

            using (var ms = new MemoryStream())
            {
                c.SaveImage(ms, ChartImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms.ToArray(), "image/png");
            }
        }

        private Chart testGraph()
        {
            Chart c = new Chart();
            c.AntiAliasing = AntiAliasingStyles.All;
            c.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            c.Width = 640; //SET HEIGHT
            c.Height = 480; //SET WIDTH
            //Asetukset...
            ChartArea ca = new ChartArea();
            ca.BackColor = Color.FromArgb(248, 248, 248);
            ca.BackSecondaryColor = Color.FromArgb(255, 255, 255);
            ca.BackGradientStyle = GradientStyle.TopBottom;
            ca.AxisY.IsMarksNextToAxis = true;
            ca.AxisY.Title = "Gigabytes Used";
            ca.AxisY.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisY.MajorTickMark.Enabled = true;
            ca.AxisY.MinorTickMark.Enabled = true;
            ca.AxisY.MajorTickMark.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisY.MinorTickMark.LineColor = Color.FromArgb(200, 200, 200);
            ca.AxisY.LabelStyle.ForeColor = Color.FromArgb(89, 89, 89);
            ca.AxisY.LabelStyle.Format = "{0:0.0}";
            //ca.AxisY.LabelStyle.IsEndLabelVisible = false;
            /*
            ca.AxisY.LabelStyle.Font = new Font("Calibri", 4, FontStyle.Regular);
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(234, 234, 234);
            ca.AxisX.IsMarksNextToAxis = true;
            ca.AxisX.LabelStyle.Enabled = true;
            ca.AxisX.Interval = 1;
            ca.AxisX.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisX.MajorGrid.LineWidth = 0;
            ca.AxisX.MajorTickMark.Enabled = true;
            ca.AxisX.MinorTickMark.Enabled = true;
            ca.AxisX.MajorTickMark.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisX.MinorTickMark.LineColor = Color.FromArgb(200, 200, 200);
            */
            c.ChartAreas.Add(ca);
            //Series paska.
            List<stateMonitor.lampotilaTaulu> temps = db.lampotilaTaulu.OrderByDescending(x => x.rowId).Take(1000).ToList();
            temps = temps.OrderBy(o => o.rowId).ToList();
            Series s = new Series();
            s.Font = new Font("Lucida Sans Unicode", 6f);
            s.Color = Color.FromArgb(215, 47, 6);
            s.BorderColor = Color.FromArgb(159, 27, 13);
            s.BackSecondaryColor = Color.FromArgb(173, 32, 11);
            s.BackGradientStyle = GradientStyle.LeftRight;
            s.XValueType = ChartValueType.DateTime;
            s.ChartType = SeriesChartType.Line;
            int i, count = 0;
            count = temps.Count;

            for(i = 0; i < count; i++ )
            {
                s.Points.AddXY(temps[i].aika.Value.ToString(), temps[i].lampotila1.Value);
            }

            c.Series.Add(s);


            return c;

        }
    }
}