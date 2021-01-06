using Japanese_WebApp.Models;
using Japanese_WebApp.Models.Entities;
using Japanese_WebApp.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Japanese_WebApp.Controllers
{
    public class SentencesController : Controller
    {
        private string URL = "https://receptomanijalogi.web.app/audio/";
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sentences
        public ActionResult Index(string search)
        {
            List<Sentence> sentences = new List<Sentence>();
            List<SentenceViewModel> model = new List<SentenceViewModel>();
            List<UserFavourite> userFavourites = new List<UserFavourite>();
            string searchLang = "";

            if (!string.IsNullOrEmpty(search))
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/Files/all_v10.json")))
                {
                    string json = r.ReadToEnd();
                    List<Sentence> obj = JsonConvert.DeserializeObject<List<Sentence>>(json);

                    if (User.Identity.IsAuthenticated)
                    {
                        string userId = User.Identity.GetUserId();

                        userFavourites = db.UserFavourites
                            .Where(f => f.UserId == userId)
                            .ToList();
                    }

                    // Assign search word
                    foreach (var item in obj)
                    {
                        bool _inList = false;

                        if (userFavourites.Any(f => f.Eng.Equals(item.eng)) &&
                                 userFavourites.Any(f => f.Jap.Equals(item.jap))){
                            _inList = true;
                        }

                        if (item.eng.Contains(search))
                        {
                            searchLang = "Eng";

                            model.Add(new SentenceViewModel 
                            { 
                                Audio_Jap = URL + item.audio_jap,
                                Eng = item.eng,
                                Jap = item.jap,
                                Source = item.source,
                                InMyList = _inList,
                            });

                        }
                        else if (item.jap.Contains(search))
                        {
                            searchLang = "Jap";

                            model.Add(new SentenceViewModel
                            {
                                Audio_Jap = URL + item.audio_jap,
                                Eng = item.eng,
                                Jap = item.jap,
                                Source = item.source,
                                InMyList = _inList
                            });
                        }
                        else { }
                    }
                }
            }

            ViewBag.SearchLang = searchLang;
            ViewBag.SearchValue = search;

            return View(model);
        }

        public async Task<string> AddToList(string eng, string jap, string audio, string source)
        {
            var userId = User.Identity.GetUserId();

            db.UserFavourites.Add(new Models.Entities.UserFavourite
            {
                Audio_Jap = audio,
                Eng = eng,
                Jap = jap,
                InMyList = true,
                Source = source,
                UserId = userId
            });

            await db.SaveChangesAsync();

            return "Success";
        }

        public async Task<ActionResult> MyList()
        {
            string userId = User.Identity.GetUserId();

            List<UserFavourite> userFavourites = await db.UserFavourites
                .Where(f => f.UserId == userId).ToListAsync();

            return View(userFavourites);
        }

        public ActionResult CreateAnki(string eng, string jap, string search)
        {
            AnkiSharp.Anki test = new AnkiSharp.Anki("MyAnkiPackage");

            test.AddItem("Hello", "Bonjour");

            test.CreateApkgFile(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            //test.CreateApkgFile(@"c:\\Users\\Cameron\\Downloads");

            return RedirectToAction("Index", new { search = search });
        }

    }
}