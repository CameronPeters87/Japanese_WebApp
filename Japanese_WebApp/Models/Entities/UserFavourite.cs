using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Japanese_WebApp.Models.Entities
{
    public class UserFavourite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Source { get; set; }
        public string Audio_Jap { get; set; }
        public string Jap { get; set; }
        public string Eng { get; set; }
        public bool InMyList { get; set; }

        public string UserId { get; set; }
    }
}