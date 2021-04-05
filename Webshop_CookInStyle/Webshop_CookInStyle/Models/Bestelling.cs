using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Bestellingen")]

    public class Bestelling
    {
        [Key]
        public int BestellingID { get; set; }
        public DateTime BestelDatum { get; set; }
        public DateTime Leverdatum { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Totaalprijs { get; set; }
        public string Opmerking { get; set; }

        // FK Klant
        public int KlantID { get; set; }
        // FK Leveradres
        public int LeverAdresID { get; set; }

        // Navprop
        [ForeignKey("KlantID")]
        public Klant Klant { get; set; }
        [ForeignKey("LeverAdresID")]
        public LeverAdres LeverAdres { get; set; }
        public ICollection<Factuur> Facturen { get; set; }
        public ICollection<Bestellijn> Bestellijnen { get; set; }
    }
}
