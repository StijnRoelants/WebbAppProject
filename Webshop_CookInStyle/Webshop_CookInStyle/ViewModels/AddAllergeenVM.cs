using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class AddAllergeenVM
    {
        public Allergeen Allergeen { get; set; }
        public List<Allergeen> AllergenenList { get; set; }
        public string AllergeenSearch { get; set; }



    }
}
