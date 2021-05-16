using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Data;

namespace Webshop_CookInStyle.Conrollers
{
    public class UserBestellingenController : Controller
    {
        private readonly WebshopContext _context;

        public UserBestellingenController(WebshopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
