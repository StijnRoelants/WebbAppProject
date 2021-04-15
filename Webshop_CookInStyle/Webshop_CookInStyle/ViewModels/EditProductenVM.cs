using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Data;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class EditProductenVM
    {
        public Product Product { get; set; }
        public Allergeen Allergeen { get; set; }
        public SelectList Allergenen { get; set; }
        public SelectList BtwTypes { get; set; }
        public SelectList ProductTypes { get; set; }
        public SelectList Eenheden { get; set; }
    }
}
