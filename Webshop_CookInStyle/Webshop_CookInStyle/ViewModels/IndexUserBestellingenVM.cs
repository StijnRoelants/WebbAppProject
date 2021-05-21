using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class IndexUserBestellingenVM
    {
        public List<ProductType> Typelijst { get; set; }
        public List<Product> BeschikbareProducten { get; set; }
        public int Aantal { get; set; }
        public Klant Klant { get; set; }
        public Bestelling NieuweBestelling { get; set; }
        public List<Bestellijn> Orderlijnen { get; set; }
        public string Allergenen { get; set; }
    }
}
