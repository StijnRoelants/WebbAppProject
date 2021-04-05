using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop_CookInStyle.Models
{
    [Table("Factuurlijnen")]

    public class Factuurlijn
    {
        [Key]
        public int FactuurlijnID { get; set; }
        public int Aantal { get; set; }
        public int Lijnprijs { get; set; }
        public int Eenheidsprijs { get; set; }
        public int BtwBedrag { get; set; }

        // FK Product
        public int ProductID { get; set; }
        // FK Factuur
        public int FactuurID { get; set; }

        // Navprop
        public Factuur Factuur { get; set; }
        public Product Product { get; set; }
    }
}