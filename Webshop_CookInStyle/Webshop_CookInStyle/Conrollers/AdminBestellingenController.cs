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

        public async Task<IActionResult> BestellingZoeken(IndexAdminBestellingenVM viewModel)
        {
            if (viewModel.ZoekenOpNaam == null)
            {
                viewModel.Bestellingen = await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= DateTime.Now).ToListAsync();
                return View(viewModel);
            }
            List<Bestelling> Zoekopdracht = new List<Bestelling>();
            Zoekopdracht.AddRange(await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Klant.Voornaam == viewModel.ZoekenOpNaam || x.Klant.Achternaam == viewModel.ZoekenOpNaam).ToListAsync());
            Zoekopdracht.AddRange(await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Bestelbonnummer == viewModel.ZoekenOpNaam).ToListAsync());
            viewModel.Bestellingen = Zoekopdracht;
            return View("Index", viewModel);
        }

        public async Task<IActionResult> BestellingZoekenOpDatum(IndexAdminBestellingenVM viewModel)
        {
            if (viewModel.ZoekenDatumVan == null || viewModel.ZoekenDatumTot == null)
            {
                viewModel.Bestellingen = await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= DateTime.Now).ToListAsync();
                return View(viewModel);
            }
            List<Bestelling> Zoekopdracht = new List<Bestelling>();
            Zoekopdracht.AddRange(await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= viewModel.ZoekenDatumVan && x.Leverdatum <= viewModel.ZoekenDatumTot).ToListAsync());
            viewModel.Bestellingen = Zoekopdracht;
            return View("Index", viewModel);
        }
        #endregion

        #region Bestelling details

        public async Task<IActionResult> AddBestellingDetails()
        {
            AddBestellingDetailsVM viewModel = new AddBestellingDetailsVM();
            viewModel.Bestelling = new Bestelling();
            viewModel.Klanten = new SelectList(await _context.Klanten.ToListAsync());
            viewModel.Bestelling.Leverdatum = DateTime.Now;
            return View(viewModel);
        }

        #endregion

        #region Nieuwe bestelling plaatsen

        public async Task<IActionResult> AddBestelling()
        {
            Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).FirstOrDefaultAsync();
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin);
            if (true)
            {

            }
            AddBestellingVM viewModel = new AddBestellingVM();
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).ToListAsync();
            return View(viewModel);
        }

        #endregion
    }
}
