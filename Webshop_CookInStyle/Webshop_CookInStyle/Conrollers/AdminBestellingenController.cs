using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_CookInStyle.Data;
using Webshop_CookInStyle.Models;
using Webshop_CookInStyle.ViewModels;

namespace Webshop_CookInStyle.Conrollers
{
    public class AdminBestellingenController : Controller
    {
        private readonly WebshopContext _context;

        public AdminBestellingenController(WebshopContext context)
        {
            _context = context;
        }

        #region Index-Page
        public async Task<IActionResult> Index()
        {
            IndexAdminBestellingenVM viewModel = new IndexAdminBestellingenVM();
            viewModel.Bestellingen = await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= DateTime.Now).ToListAsync();
            return View(viewModel);
        }
        #endregion
    }
}
