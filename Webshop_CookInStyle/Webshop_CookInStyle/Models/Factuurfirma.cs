using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Facturatiefirmas")]
    public class Factuurfirma
    {
        public int FactuurfirmaID { get; set; }
        public string Naam { get; set; }
        [DisplayName("Straat en nr.")]
        public string Straat { get; set; }
        [DisplayName("IBAN")]
        public string Iban { get; set; }
        [DisplayName("BIC")]
        public string Bic { get; set; }
        public string Banknaam { get; set; }
        public string BtwNummer { get; set; }
        public string Email { get; set; }
        public string Telefoon { get; set; }
        public string Factuurnummering { get; set; }
        // FK Postcode
        public int PostcodeID { get; set; }
        // FK Postcode
        public int LandID { get; set; }
        // Navprop
        public Postcode Postcode { get; set; }
        public Land Land { get; set; }
        public ICollection<Factuur> Facturen { get; set; }
    }
}
