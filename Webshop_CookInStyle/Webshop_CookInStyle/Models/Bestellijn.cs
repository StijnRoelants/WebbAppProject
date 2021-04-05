using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Bestellijnen")]

    public class Bestellijn
    {
        [Key]
        public int BestellijnID { get; set; }
        public int Aantal { get; set; }
        public int Lijnprijs { get; set; }
        public int Eenheidsprijs { get; set; }
        public int BtwBedrag { get; set; }

        // FK Product
        public int ProductID { get; set; }
        // FK Factuur
        public int BestellingID { get; set; }

        // Navprop
        public Bestelling Bestelling { get; set; }
        public Product Product { get; set; }
    }
}
