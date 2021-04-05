using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Allergenen")]

    public class Allergeen
    {
        [Key]
        public int AllergeenID { get; set; }
        public string Omschrijving { get; set; }

        //Navprop 
        public ICollection<AllergeenProduct> Producten { get; set; }
    }
}
