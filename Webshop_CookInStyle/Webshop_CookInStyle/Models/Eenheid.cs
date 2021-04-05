using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Eenheden")]

    public class Eenheid
    {
        [Key]
        public int EenheidID { get; set; }
        public string Omschrijving { get; set; }
        [MaxLength(5)]
        public string Afkorting { get; set; }

        // Navprop
        public ICollection<Product> Producten { get; set; }
    }
}
