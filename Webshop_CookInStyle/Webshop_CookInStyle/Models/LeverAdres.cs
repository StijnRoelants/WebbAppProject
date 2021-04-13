using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("LeverAdressen")]
    public class LeverAdres
    {
        [Key]
        public int LeverAdresID { get; set; }
        public string Straat { get; set; }
        public string Omschrijving { get; set; }
        // FK Postcode
        public int PostcodeID { get; set; }
        // FK Land 
        public int LandID { get; set; }


        // Navprop
        [ForeignKey("PostcodeID")]
        public Postcode Postcode { get; set; }
        [ForeignKey("LandID")]
        public Land Land { get; set; }
        public Klant Klant { get; set; }
        public ICollection<Bestelling> Bestellingen { get; set; }
    }
}
