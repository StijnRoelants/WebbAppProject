using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class IndexAdminProductenVM
    {
        public Product Product { get; set; }
        public Allergeen Allergeen { get; set; }
        public AllergeenProduct AllergeenProduct { get; set; }
        public ProductType ProductType { get; set; }
        public string Productsearch { get; set; }
        public string Error { get; set; }
        public List<Product> Producten { get; set; }
        public List<Allergeen> AllergeenList { get; set; }
        public SelectList Producttypes { get; set; }
        public MultiSelectList Allergenen { get; set; }
        public SelectList BtwTypes { get; set; }
        public SelectList Eenheden { get; set; }
    }
}
