using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class AddProductTypeVM
    {
        public ProductType ProductType { get; set; }
        public List<ProductType> ProductTypes { get; set; }
        public string TypeSearch { get; set; }
    }
}
