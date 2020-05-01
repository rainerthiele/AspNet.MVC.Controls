using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspNet.MVC.Controls.Sample.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Artikelname")]
        public String Name { get; set; }
       
        [Display(Name = "Preis")]
        public Decimal Price { get; set; }
   
        [Display(Name = "Erscheinungsdatum")]
        public DateTime ReleaseDate { get; set; }
    }
}