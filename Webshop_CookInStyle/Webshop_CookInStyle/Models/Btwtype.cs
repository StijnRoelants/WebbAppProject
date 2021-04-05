using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    public class Btwtype
    {
        [Key]
        public int BtwID { get; set; }
        public int Percentage { get; set; }
        [MaxLength(20)]
        public string Omschrijving { get; set; }

        // Navprop
        public ICollection<Product> Producten { get; set; }
    }
}
