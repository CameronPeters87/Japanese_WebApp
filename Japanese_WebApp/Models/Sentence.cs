using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Japanese_WebApp.Models
{
    public class Sentence
    {
        public string source { get; set; }
        public string audio_jap { get; set; }
        public string jap { get; set; }
        public string eng { get; set; }
    }
}