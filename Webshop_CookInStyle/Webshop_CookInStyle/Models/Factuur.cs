using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Facturen")]

    public class Factuur
    {
        [Key]
        public int FactuurID { get; set; }
        public string Factuurnummer { get; set; }
        public DateTime FactuurDatum { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotaalPrijsEx { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotaalPrijsIncl { get; set; }
        public string BtwnummerKlant { get; set; }
        public string BtwnummerFacFirma { get; set; }
        public bool IsBetaald { get; set; }
        public string NaamKlant { get; set; }


        // FK Bestelling
        public int BestellingID { get; set; }
        // FK Factuur Firma
        public int FactuurfirmaID { get; set; }
        // FK Klant
        [ForeignKey("Klant")]
        public string KlantFK { get; set; }

        // Navprop
        public Klant Klant { get; set; }
        [ForeignKey("BestellingID")]
        public Bestelling Bestelling { get; set; }
        [ForeignKey("FactuurfirmaID")]
        public Factuurfirma Factuurfirma { get; set; }
        public ICollection<Factuurlijn> Factuurlijnen { get; set; }
    }
}
