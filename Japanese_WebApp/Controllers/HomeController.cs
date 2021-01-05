using Japanese_WebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Japanese_WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Sentence obj = new Sentence();

            using (StreamReader r = new StreamReader(Server.MapPath("~/Files/all_v10.json")))
            {
                string json = r.ReadToEnd();
                List<Sentence> sentences = JsonConvert.DeserializeObject<List<Sentence>>(json);

                obj = sentences[0];
            }


            return View(obj);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}