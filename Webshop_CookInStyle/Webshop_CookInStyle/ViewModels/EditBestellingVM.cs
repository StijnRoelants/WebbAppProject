using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class EditBestellingVM
    {
        public Bestelling Bestelling { get; set; }
        public Klant Klant { get; set; }
        public SelectList Leveradressen { get; set; }
        public SelectList Postcodes { get; set; }
        public SelectList Landen { get; set; }
        public LeverAdres LeverAdres { get; set; }
        public DateTime AanTePassen { get; set; }
    }
}
