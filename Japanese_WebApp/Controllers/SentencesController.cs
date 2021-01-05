using Japanese_WebApp.Models;
using Japanese_WebApp.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Japanese_WebApp.Controllers
{
    public class SentencesController : Controller
    {
        private string URL = "https://receptomanijalogi.web.app/audio/";

        // GET: Sentences
        public ActionResult Index(string search)
        {
            List<Sentence> sentences = new List<Sentence>();
            List<Sentence> model = new List<Sentence>();
            string searchLang = "";

            if (!string.IsNullOrEmpty(search))
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/Files/all_v10.json")))
                {
                    string json = r.ReadToEnd();
                    List<Sentence> obj = JsonConvert.DeserializeObject<List<Sentence>>(json);

                    // Assign search word
                    foreach (var item in obj)
                    {
                        if (item.eng.Contains(search))
                        {
                            if(!string.IsNullOrEmpty(item.eng) || string.IsNullOrWhiteSpace(item.eng)
                                || !string.IsNullOrEmpty(item.jap) || string.IsNullOrWhiteSpace(item.jap))
                            {
                                searchLang = "Eng";
                                sentences.Add(item);
                            }
                        }
                        else if (item.jap.Contains(search))
                        {
                            if (!string.IsNullOrEmpty(item.eng) || string.IsNullOrWhiteSpace(item.eng)
                                || !string.IsNullOrEmpty(item.jap) || string.IsNullOrWhiteSpace(item.jap))
                            {
                                sentences.Add(item);
                                searchLang = "Jap";
                            }
                        }
                        else { }
                    }
                }

                foreach (var item in sentences)
                {
                    // Concatenate audio link url
                    item.audio_jap = URL + item.audio_jap;

                    // bold the searched word
                    //if (searchLang == "Eng")
                    //{
                    //    String search_term = search;
                    //    String value = item.eng;
                    //    String result = Regex.Replace(value, String.Join("|", search_term.Split(' ')), @"<strong>$&</strong>");
                    //    item.eng = result;
                    //}
                    //else
                    //{
                    //    String search_term = search;
                    //    String value = item.jap;
                    //    String result = Regex.Replace(value, String.Join("|", search_term.Split(' ')), @"<strong>$&</strong>");
                    //    item.jap = result;
                    //}
                }
            }

            ViewBag.SearchLang = searchLang;
            ViewBag.SearchValue = search;

            return View(sentences);
        }

        public ActionResult CreateAnki(string eng, string jap, string search)
        {
            AnkiSharp.Anki test = new AnkiSharp.Anki("MyAnkiPackage");

            test.AddItem("Hello", "Bonjour");

            test.CreateApkgFile(@"c:\\Users\\Cameron\\Downloads");

            return RedirectToAction("Index", new { search = search });
        }
    }
}