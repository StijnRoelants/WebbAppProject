using Microsoft.AspNetCore.Mvc;
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
    public class AdminProductenController : Controller
    {
        private readonly WebshopContext _context;

        public AdminProductenController(WebshopContext context)
        {
            _context = context;
        }

        #region Producten
        // Inladen producten bij opstart pagina
        public async Task<IActionResult> Index()
        {
            IndexAdminProductenVM viewModel = new IndexAdminProductenVM();
            viewModel.Producten = await _context.Producten
                .ToListAsync();
            return View(viewModel);
        }

        // Zoekfunctie binnen producten
        public async Task<IActionResult> Search(IndexAdminProductenVM viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Productsearch))
            {
                viewModel.Producten = await _context.Producten
                    .Where(x => x.Naam.Contains(viewModel.Productsearch))
                    .ToListAsync();
            }
            else
            {
                viewModel.Producten = await _context.Producten
                    .ToListAsync();
            }
            return View("Index", viewModel);
        }
        #endregion

        #region Allergenen
        // Inladen Allergenen bij opstart pagina
        public async Task<IActionResult> AddAllergeen()
        {
            AddAllergeenVM viewModel = new AddAllergeenVM();
            viewModel.AllergenenList = await _context.Allergenen
                .ToListAsync();
            return View(viewModel);
        }

        // Nieuw Allergeen aanmaken
        public IActionResult CreateAllergeen()
        {
            AddAllergeenVM viewModel = new AddAllergeenVM();
            viewModel.Allergeen = new Allergeen();
            return View(viewModel);
        }


        // Zoekfunctie binnen Allergeen
        public async Task<IActionResult> ASearch(AddAllergeenVM viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.AllergeenSearch))
            {
                viewModel.AllergenenList = await _context.Allergenen
                    .Where(x => x.Omschrijving.Contains(viewModel.AllergeenSearch))
                    .ToListAsync();
            }
            else
            {
                viewModel.AllergenenList = await _context.Allergenen
                    .ToListAsync();
            }
            return View("AddAllergeen",viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAllergeen(AddAllergeenVM viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!AllergeenExists(viewModel.Allergeen.Omschrijving))
                {
                    _context.Add(viewModel.Allergeen);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(AddAllergeen));
                }
                else
                {
                    
                    ViewBag.ErrorMessage = $"{viewModel.Allergeen.Omschrijving} bestaat reeds als allergeen";
                    return RedirectToAction(nameof(AddAllergeen));
                }
            }
            return RedirectToAction(nameof(AddAllergeen));
        }

        private bool AllergeenExists(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                return true;
            }
            else
            {
                return _context.Allergenen.Any(x => x.Omschrijving.ToUpper() == txt.ToUpper());

            }
        }

        // Allergeen verwijderen
        public async Task<IActionResult> DeleteAllergeen(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AddAllergeenVM viewModel = new AddAllergeenVM();
            viewModel.Allergeen = await _context.Allergenen
                .Where(x => x.AllergeenID == id)
                .FirstOrDefaultAsync();
            if (viewModel.Allergeen == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpPost, ActionName("DeleteAllergeen")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllergeenConfirmed(int id, AddAllergeenVM viewModel)
        {
            if (id != viewModel.Allergeen.AllergeenID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Remove(viewModel.Allergeen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddAllergeen));
            }
            else
            {
                return RedirectToAction(nameof(AddAllergeen));
            }
        }

        #endregion
    }
}
