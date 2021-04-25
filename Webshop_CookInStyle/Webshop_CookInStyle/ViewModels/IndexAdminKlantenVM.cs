using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class IndexAdminKlantenVM
    {
        public Klant Klant { get; set; }
        public List<Klant> Klanten { get; set; }
        public bool NaamSearch { get; set; }
        public bool FirmanaamSearch { get; set; }
        public bool BtwnummerSearch { get; set; }
        public string SearchQuerry { get; set; }
        public List<Postcode> Postcodes { get; set; }
        public List<Land> Landen { get; set; }
    }
}
