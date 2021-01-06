using AnkiSharp;
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
        public async Task<string> RemoveFromList(string eng, string jap)
        {
            var userId = User.Identity.GetUserId();

            var sentence = db.UserFavourites.Where(f => f.UserId == userId && 
                f.Eng == eng && f.Jap == jap).FirstOrDefault();

            db.UserFavourites.Remove(sentence);

            await db.SaveChangesAsync();

            return sentence.Id.ToString();
        }


        public async Task<ActionResult> MyList()
        {
            string userId = User.Identity.GetUserId();

            List<UserFavourite> userFavourites = await db.UserFavourites
                .Where(f => f.UserId == userId).ToListAsync();

            return View(userFavourites);
        }

        public async Task<ActionResult> CreateAnki()
        {
            var userId = User.Identity.GetUserId();
            var myList = await db.UserFavourites.Where(f => f.UserId == userId).ToListAsync();

            //Anki deck = new Anki("My List - Sentence Search",
            //    new ApkgFile(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));

            Anki deck = new Anki("My List - Sentence Search");

            deck.SetFields("Japanese", "English", "Audio");

            // Be careful, keep the same fields !
            foreach (var item in myList)
            {
                var ankiItem = deck.CreateAnkiItem(item.Jap, item.Eng, item.Audio_Jap);

                if (deck.ContainsItem(ankiItem) == false){ // will not add if the card is entirely the same (same fields' value)
                    deck.AddItem(ankiItem);
                }
            }

            deck.CreateApkgFile(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            TempData["SM"] = "Anki Deck successfully created. The anki deck will be located on your desktop.";

            return RedirectToAction("MyList");
        }

    }
}