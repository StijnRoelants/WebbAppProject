using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Landen")]
    public class Land
    {
        [Key]
        public int LandID { get; set; }
        public string Naam { get; set; }
        [MaxLength(5)]
        public string Afkorting { get; set; }

        // NavProp
        public ICollection<Klant> Klanten { get; set; }
        public ICollection<LeverAdres> LeverAdressen { get; set; }
        public ICollection<Factuurfirma> Factuurfirmas { get; set; }
    }
}
