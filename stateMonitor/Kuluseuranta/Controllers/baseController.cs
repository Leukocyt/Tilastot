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
    //[Authorize(Roles = @"Raportit")]
    public class baseController : Controller
    {
        public string baseUrl { get; set; }
        public baseController()
        {
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            // now Server has been initialized
            this.baseUrl = HttpContext.Request.ApplicationPath;  //Server.MapPath("~/uploads");
            ViewBag.baseUrl = this.baseUrl;
        }
    }
}
