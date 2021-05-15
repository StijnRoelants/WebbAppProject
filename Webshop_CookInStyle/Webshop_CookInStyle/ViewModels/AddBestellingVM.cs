using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class AddBestellingVM
    {
        public List<Product> Producten { get; set; }
        public List<Klant> Klanten { get; set; }
        public Bestelling NieuweBestelling { get; set; }
        public List<Bestellijn> Orderlijnen { get; set; }
        public int Aantal { get; set; }
        public string Zoekopdracht { get; set; }
    }
}
