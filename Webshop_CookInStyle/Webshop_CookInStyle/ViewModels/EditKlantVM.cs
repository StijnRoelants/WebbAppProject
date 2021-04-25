using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.ViewModels
{
    public class EditKlantVM
    {
        public Klant Klant { get; set; }
        public SelectList Postcodes { get; set; }
        public SelectList Landen { get; set; }
        public bool MaakAdmin { get; set; }
        public DbSet<IdentityUserRole<string>> UserRoleList { get; set; }
    }
}
