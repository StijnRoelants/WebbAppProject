﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{

    public class ProductType
    {
        [Key]
        public int ProductTypeID { get; set; }
        public string Omschrijving { get; set; }

        // Navprop
        public ICollection<Product> Producten { get; set; }
    }
}
