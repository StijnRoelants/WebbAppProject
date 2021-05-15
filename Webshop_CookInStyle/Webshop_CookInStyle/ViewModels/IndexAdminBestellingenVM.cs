using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class IndexAdminBestellingenVM
    {
        public List<Bestelling> Bestellingen { get; set; }
        public string ZoekenOpNaam { get; set; }
        public DateTime ZoekenDatumVan { get; set; }
        public DateTime ZoekenDatumTot { get; set; }
        public string Bericht { get; set; }
    }
}
