using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    //[Table("Klanten")]
    public class Klant : IdentityUser
    {
        public string Naam { get; set; }
        [PersonalData]
        public string Voornaam { get; set; }
        [PersonalData]
        public string Achternaam { get; set; }
        [PersonalData]
        public string StraatEnNummer { get; set; }
        public bool IsBedrijf { get; set; }
        public string BtwNummer { get; set; }
        [PersonalData]
        public string Mobiel { get; set; }
        [PersonalData]
        public string Telefoon { get; set; }

        // FK Postcode
        public int PostcodeID { get; set; }
        // FK Land
        public int LandID { get; set; }



        // NavProps
        public Postcode Postcode { get; set; }
        public Land Land { get; set; }
        public ICollection<LeverAdres> LeverAdressen { get; set; }
        public ICollection<Bestelling> Bestellingen { get; set; }
        public ICollection<Factuur> Facturen { get; set; }

        // Methodes
        public override string ToString()
        {
            if (IsBedrijf == true)
            {
                return $"{Naam} - {Voornaam}";
            }
            else
            {
                return $"{Voornaam} {Achternaam}";
            }
        }

    }
}
