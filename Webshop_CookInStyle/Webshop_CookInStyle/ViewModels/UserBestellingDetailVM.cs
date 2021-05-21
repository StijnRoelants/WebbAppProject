using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class UserBestellingDetailVM
    {
        public Bestelling Bestelling { get; set; }
        public List<string> Bestellijnen { get; set; }
        public Klant Klant { get; set; }
        public LeverAdres LeverAdres { get; set; }
        public List<Allergeen> Allergenen { get; set; }
    }
}
