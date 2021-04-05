using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Producten")]

    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string Naam { get; set; }
        public string Omschrijving { get; set; }
        public decimal Eenheidsprijs { get; set; }
        public bool BeschikbaarInWebshop { get; set; }

        // FK Eenheid
        public int EenheidID { get; set; }
        // FK Producttype
        public int ProductTypeID { get; set; }
        // FK BTW-Type
        public int BtwID { get; set; }

        // Navprop
        public Eenheid Eenheid { get; set; }
        public ProductType ProductType { get; set; }
        public Btwtype Btwtype { get; set; }
        public ICollection<Factuurlijn> Factuurlijnen { get; set; }
        public ICollection<Bestellijn> Bestellijnen { get; set; }
        public ICollection<AllergeenProduct> Allergenen { get; set; }
    }
}
