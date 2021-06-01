using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class UserBestellingAfrondenVM
    {
        public Bestelling Bestelling { get; set; }
        public List<Bestellijn> Bestellijnen { get; set; }
        public Klant Klant { get; set; }
        public SelectList Leveradressen { get; set; }
        public LeverAdres NieuwLeveradres { get; set; }
        public SelectList Landen { get; set; }
        public SelectList Postcodes { get; set; }
        public string Melding { get; set; }
    }
}
