using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    public class Login
    {
        [Key]
        public int LoginID { get; set; }
        public string Username { get; set; }
        public string Wachtwoord { get; set; }
        // FK Usertype
        public int UsertypeID { get; set; }
        //Navprops
        public Klant Klant { get; set; }
        public Usertype Usertype { get; set; }
    }
}
