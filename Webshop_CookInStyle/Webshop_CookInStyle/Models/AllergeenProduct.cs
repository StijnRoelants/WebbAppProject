using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    public class AllergeenProduct
    {
        [Key]
        public int AllergeenProductID { get; set; }
        // FK Allergeen
        public int AllergeenID { get; set; }
        // FK Product
        public int ProductID { get; set; }

        // Navprop
        public Allergeen Allergeen { get; set; }
        public Product Product { get; set; }
    }
}
