using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop_CookInStyle.Models
{
    public class AllergeenProduct
    {
        [Key]
        public int AllergeenProductID { get; set; }
        // FK Allergeen
        [ForeignKey("Allergeen")]
        public int AllergeenID { get; set; }
        // FK Product
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        // Navprop
        [ForeignKey("AllergeenID")]
        public Allergeen Allergeen { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
