using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{

    public class ProductType
    {
        [Key]
        public int ProductTypeID { get; set; }
        [DisplayName("Producttype"), Required]
        public string Omschrijving { get; set; }
        [Required]
        public int Volgnummer { get; set; }

        // Navprop
        public ICollection<Product> Producten { get; set; }
    }
}
