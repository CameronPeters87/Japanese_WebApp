using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Japanese_WebApp.Models.ViewModels
{
    public class SentenceViewModel
    {
        public string Source { get; set; }
        public string Audio_Jap { get; set; }
        public string Jap { get; set; }
        public string Eng { get; set; }
        public bool InMyList { get; set; }
    }
}