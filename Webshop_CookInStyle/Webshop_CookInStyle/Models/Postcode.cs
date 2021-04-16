using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    public class Postcode
    {
        [Key]
        public int PostcodeID { get; set; }
        public string Gemeente { get; set; }
        [DisplayName("Postcode")]
        public string Nummer { get; set; }

        // Navprops
        public ICollection<Klant> Klanten { get; set; }
        public ICollection<LeverAdres> LeverAdressen { get; set; }
        public ICollection<Factuurfirma> Factuurfirmas { get; set; }

        // Methodes
        public override string ToString()
        {
            return $"{Nummer} {Gemeente}";
        }
    }
}
