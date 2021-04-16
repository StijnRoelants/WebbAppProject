using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Data;
using Webshop_CookInStyle.Models;
using Webshop_CookInStyle.ViewModels;

namespace Webshop_CookInStyle.Conrollers
{
    public class AdminSettingsController : Controller
    {
        private readonly WebshopContext _context;

        public AdminSettingsController(WebshopContext context)
        {
            _context = context;
        }

        public int InitialCheck()
        {
            var firma = _context.FactuurFirmas.FirstOrDefault();
            if (firma == null)
            {
                firma = new Factuurfirma()
                {
                    Naam = "Uw bedrijfsnaam",
                    PostcodeID = 1684,
                    LandID = 1,
                    Factuurnummering = "VF20210001"
                };

                _context.Add(firma);
                _context.SaveChanges();
            }
            return firma.FactuurfirmaID;
        }

        public async Task<IActionResult> Index()
        {
            int id = InitialCheck();
            IndexAdminSettingsVM viewModel = new IndexAdminSettingsVM();
            viewModel.Factuurfirma = await _context.FactuurFirmas.Include(x => x.Postcode).Include(x => x.Land)
                .Where(x => x.FactuurfirmaID == id)
                .FirstOrDefaultAsync();
            return View(viewModel);
        }
    }
}
