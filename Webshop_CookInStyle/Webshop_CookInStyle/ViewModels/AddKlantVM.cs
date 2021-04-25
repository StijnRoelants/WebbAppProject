using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class AddKlantVM
    {
        public Klant Klant { get; set; }
        public List<Klant> Klanten { get; set; }
        public SelectList Postcodes { get; set; }
        public SelectList Landen { get; set; }
        public string wachtwoord { get; set; }
    }
}
