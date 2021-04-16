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

        public async Task<IActionResult> EditNummering(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IndexAdminSettingsVM viewModel = new IndexAdminSettingsVM();
            viewModel.Factuurfirma = await _context.FactuurFirmas.FindAsync(id);
            if (viewModel.Factuurfirma == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // Nummering aanpassen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNummering(int id, IndexAdminSettingsVM viewModel)
        {
            if (id != viewModel.Factuurfirma.FactuurfirmaID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string error = FactuurnummeringCheck(viewModel);
                if (error == "")
                {
                    try
                    {
                        _context.Update(viewModel.Factuurfirma);
                        await _context.SaveChangesAsync();
                        ViewBag.Visibility = false;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NummeringCheck(viewModel.Factuurfirma.FactuurfirmaID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = error;
                    ViewBag.Visibility = true;
                    viewModel.Factuurfirma = _context.FactuurFirmas.Where(x => x.FactuurfirmaID == viewModel.Factuurfirma.FactuurfirmaID).FirstOrDefault();
                    return View("EditNummering", viewModel);
                }
            }
            return View(viewModel);
        }

        // Onderstaande methode gaat nakijken of de opbouw correct is.
        // Factuurnummers mogen nooit lager zijn als bestaande factuurnummers om problemen met factuurnummering te voorkomen!
        private string FactuurnummeringCheck(IndexAdminSettingsVM viewModel)
        {
            string error = "";
            int nieuweNummering = 0;
            var bedrijfsgegevens = _context.FactuurFirmas.Where(x => x.FactuurfirmaID == viewModel.Factuurfirma.FactuurfirmaID).FirstOrDefault();
            int oudeNummering = int.Parse(bedrijfsgegevens.Factuurnummering.Substring(2));
            if (viewModel.Factuurfirma.Factuurnummering == null)
            {
                return error = $"Gelieve een nieuw factuurnummering op te geven!";
            }
            else
            {
                if (int.TryParse(viewModel.Factuurfirma.Factuurnummering.Substring(2), out nieuweNummering))
                {
                    if (oudeNummering < nieuweNummering)
                    {
                        return error;
                    }
                    else
                    {
                        return error = $"Nieuwe factuurnummering ({viewModel.Factuurfirma.Factuurnummering}) kan niet lager zijn als vorige ({bedrijfsgegevens.Factuurnummering})";
                    }
                }
                else
                {
                    return error = $"{viewModel.Factuurfirma.Factuurnummering}: Factuurnummer mag niet meer dan 2 letters bevatten";

                }

            }
        }

        private bool NummeringCheck(int id)
        {
            return _context.FactuurFirmas.Any(x => x.FactuurfirmaID == id);
        }

        //Bedrijfsgegevens aanpassen inladen
        public async Task<IActionResult> EditGegevens(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IndexAdminSettingsVM viewModel = new IndexAdminSettingsVM();
            viewModel.Factuurfirma = await _context.FactuurFirmas.FindAsync(id);
            viewModel.LandList = new SelectList(_context.Landen.OrderBy(x => x.Naam), "LandID", "Naam");
            viewModel.PostcodeList = new SelectList(_context.Postcodes.OrderBy(x => x.Nummer), "PostcodeID", "Weergave");
            if (viewModel.Factuurfirma == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // Bedrijfsgegevens aanpassen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGegevens(int id, IndexAdminSettingsVM viewModel)
        {
            if (id != viewModel.Factuurfirma.FactuurfirmaID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string error = GegevensCheck(viewModel);
                if (error == "")
                {
                    try
                    {
                        _context.Update(viewModel.Factuurfirma);
                        await _context.SaveChangesAsync();
                        ViewBag.Visibility = false;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BedrijfCheck(viewModel.Factuurfirma.FactuurfirmaID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = error;
                    ViewBag.Visibility = true;
                    viewModel.Factuurfirma = _context.FactuurFirmas.Where(x => x.FactuurfirmaID == viewModel.Factuurfirma.FactuurfirmaID).FirstOrDefault();
                    viewModel.LandList = new SelectList(_context.Landen.OrderBy(x => x.Naam), "LandID", "Naam");
                    viewModel.PostcodeList = new SelectList(_context.Postcodes.OrderBy(x => x.Nummer), "PostcodeID", "Weergave");
                    return View("EditGegevens", viewModel);
                }
            }
            return View(viewModel);
        }

        // Onderstaande methode gaat nakijken of er een geldig BTW-nummer opgegeven is.
        private string GegevensCheck(IndexAdminSettingsVM viewModel)
        {
            string error = "";
            if (viewModel.Factuurfirma.BtwNummer == null)
            {
                return error = $"Gelieve een geldig btw-nummer op te geven";
            }
            else
            {
                string nummer = viewModel.Factuurfirma.BtwNummer.Substring(2).Replace(".", "").Replace(" ","");
                if (nummer.Count() != 10)
                {
                    return error = $"Gelieve een geldig btw-nummer op te geven";
                }
                return error;
            }
        }

        private bool BedrijfCheck(int id)
        {
            return _context.FactuurFirmas.Any(x => x.FactuurfirmaID == id);
        }
    }
}
