using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class IndexAdminSettingsVM
    {
        public Factuurfirma Factuurfirma { get; set; }
        public List<Factuur> Facturen { get; set; }
        public List<Land> Landen { get; set; }
        public SelectList LandList { get; set; }
        public SelectList PostcodeList { get; set; }
    }
}
