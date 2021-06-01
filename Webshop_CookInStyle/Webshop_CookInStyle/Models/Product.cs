using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required]
        public string Naam { get; set; }
        public string Omschrijving { get; set; }
        [Column(TypeName = "decimal(18, 2)"), Range(0,500000000, ErrorMessage ="Prijs kan geen negatief getal zijn")]
        [Required]
        public decimal Eenheidsprijs { get; set; }
        public bool BeschikbaarInWebshop { get; set; }
        [NotMapped, DisplayName("Prijs")]
        public string Weergave => $"€ {Eenheidsprijs}";

        // FK Eenheid
        public int EenheidID { get; set; }
        // FK Producttype
        public int ProductTypeID { get; set; }
        // FK BTW-Type
        [ForeignKey("BtwType")]
        public int BtwID { get; set; }

        // Navprop
        public Eenheid Eenheid { get; set; }
        public ProductType ProductType { get; set; }
        [ForeignKey("BtwID")]
        public Btwtype Btwtype { get; set; }
        public ICollection<Factuurlijn> Factuurlijnen { get; set; }
        public ICollection<Bestellijn> Bestellijnen { get; set; }
        public List<AllergeenProduct> Allergenen { get; set; }
        /*
        [NotMapped]
        public List<Allergeen> TestLijst { get; set; }

        public Product()
        {
            this.TestLijst = new List<Allergeen>();
        }*/
    }
}
