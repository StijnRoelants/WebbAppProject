using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class AddBestellingDetailsVM
    {
        public SelectList Klanten { get; set; }
        public Bestelling Bestelling { get; set; }
        public DateTime BestelDatum { get; set; }
        public DateTime LeverDatum { get; set; }
        public Factuurfirma Factuurfirma { get; set; }
        public List<LeverAdres> LeverAdressen { get; set; }
    }
}
