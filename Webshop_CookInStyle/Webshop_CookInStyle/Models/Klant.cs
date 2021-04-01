using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Klanten")]
    public class Klant
    {
        [Key]
        public int KlantID { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string StraatEnNummer { get; set; }
        public bool IsBedrijf { get; set; }
        public string BtwNummer { get; set; }
        public string Email { get; set; }
        public string Mobiel { get; set; }
        public string Telefoon { get; set; }

        // FK Postcode
        public int PostcodeID { get; set; }
        // FK Land
        public int LandID { get; set; }
        //FK Login
        public int LoginID { get; set; }

        // NavProps
        public Postcode Postcode { get; set; }
        public Land Land { get; set; }
        public Login Login { get; set; }
        public ICollection<LeverAdres> LeverAdressen { get; set; }
    }
}
