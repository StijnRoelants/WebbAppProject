using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    [Table("Usertyppen")]
    public class Usertype
    {
        [Key]
        public int UsertypeID { get; set; }
        public string Omschrijving { get; set; }
        // Navprop
        public ICollection<Login> Logins { get; set; }
    }
}
