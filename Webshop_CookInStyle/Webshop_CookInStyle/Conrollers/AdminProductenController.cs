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
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).OrderBy(x => x.ProductType).ThenBy(x => x.Naam)
                .ToListAsync();
            viewModel.Product = new Product();
            viewModel.AllergeenList = new List<Allergeen>();
            viewModel.Allergenen = new MultiSelectList(_context.Allergenen.OrderBy(x => x.Omschrijving), "AllergeenID", "Omschrijving");
            viewModel.Producttypes = new SelectList(_context.ProductTypes.OrderBy(x => x.Omschrijving), "ProductTypeID", "Omschrijving");
            viewModel.BtwTypes = new SelectList(_context.BtwTypes.OrderBy(x => x.Percentage), "BtwID", "Weergave");
            viewModel.Eenheden = new SelectList(_context.Eenheden.OrderBy(x => x.Omschrijving), "EenheidID", "Omschrijving");
            return View(viewModel);
        }

        public IActionResult Create()
        {
            IndexAdminProductenVM viewModel = new IndexAdminProductenVM();
            LoadIn(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndexAdminProductenVM viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            LoadIn(viewModel);
            return View(viewModel);
        }

        // Zoekfunctie binnen producten
        public async Task<IActionResult> Search(IndexAdminProductenVM viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Productsearch))
            {
                viewModel.Producten = await _context.Producten
                    .Where(x => x.Naam.Contains(viewModel.Productsearch))
                    .Include(x => x.ProductType)
                    .ToListAsync();
            }
            else
            {
                viewModel.Producten = await _context.Producten.Include(x=>x.ProductType)
                    .ToListAsync();
            }
            return View("Index", viewModel);
        }

        public void LoadIn(IndexAdminProductenVM viewModel)
        {
            viewModel.AllergeenList = new List<Allergeen>();
            viewModel.Producten = new List<Product>(_context.Producten.Include(x => x.ProductType).OrderBy(x => x.ProductType).ThenBy(x => x.Naam));
            viewModel.Allergenen = new MultiSelectList(_context.Allergenen.OrderBy(x => x.Omschrijving), "AllergeenID", "Omschrijving");
            viewModel.Producttypes = new SelectList(_context.ProductTypes.OrderBy(x => x.Omschrijving), "ProductTypeID", "Omschrijving");
            viewModel.BtwTypes = new SelectList(_context.BtwTypes.OrderBy(x => x.Percentage), "BtwID", "Weergave");
            viewModel.Eenheden = new SelectList(_context.Eenheden.OrderBy(x => x.Omschrijving), "EenheidID", "Omschrijving");
        }

        // Producten in detail bekijken eventueel editeren en verwijderen
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EditProductenVM viewModel = new EditProductenVM();
            viewModel.Product = await _context.Producten
                .Include(x => x.ProductType)
                .Include(x => x.Allergenen).ThenInclude(x => x.Allergeen)
                .Include(x => x.Btwtype)
                .Include(x => x.Eenheid)
                .Where(x => x.ProductID == id)
                .FirstOrDefaultAsync();
            if (viewModel.Product == null)
            {
                return NotFound();
            }
            viewModel.Allergenen = new SelectList(_context.Allergenen);
            return View(viewModel);
        }

        #endregion

        #region Allergenen
        // Inladen Allergenen bij opstart pagina
        public async Task<IActionResult> AddAllergeen()
        {
            AddAllergeenVM viewModel = new AddAllergeenVM();
            viewModel.AllergenenList = await _context.Allergenen
                .OrderBy(x => x.Omschrijving)
                .ToListAsync();
            return View(viewModel);
        }


        // Zoekfunctie binnen Allergeen
        public async Task<IActionResult> ASearch(AddAllergeenVM viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.AllergeenSearch))
            {
                viewModel.AllergenenList = await _context.Allergenen
                    .Where(x => x.Omschrijving.Contains(viewModel.AllergeenSearch))
                    .OrderBy(x => x.Omschrijving)
                    .ToListAsync();
            }
            else
            {
                viewModel.AllergenenList = await _context.Allergenen
                    .OrderBy(x => x.Omschrijving)
                    .ToListAsync();
            }
            return View("AddAllergeen", viewModel);

        }

        // Nieuw Allergeen aanmaken
        public IActionResult CreateAllergeen()
        {
            AddAllergeenVM viewModel = new AddAllergeenVM();
            viewModel.Allergeen = new Allergeen();
            return View(viewModel);
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
                    ViewBag.Visibility = false;
                    return RedirectToAction(nameof(AddAllergeen));
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(viewModel.Allergeen.Omschrijving))
                    {
                        ViewBag.ErrorMessage = $"Gelieve een geldige text in te geven!";
                        ViewBag.Visibility = true;
                        viewModel.AllergenenList = new List<Allergeen>(_context.Allergenen);
                        return View("AddAllergeen", viewModel);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"{viewModel.Allergeen.Omschrijving} bestaat reeds als allergeen";
                        ViewBag.Visibility = true;
                        viewModel.AllergenenList = new List<Allergeen>(_context.Allergenen);
                        return View("AddAllergeen", viewModel);
                    }

                }
            }
            return View(viewModel);
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

        #region Product Type

        // Inladen types bij opstart pagina
        public async Task<IActionResult> AddProductType()
        {
            AddProductTypeVM viewModel = new AddProductTypeVM();
            viewModel.ProductTypes = await _context.ProductTypes
                .OrderBy(x => x.Omschrijving)
                .ToListAsync();
            return View(viewModel);
        }


        // Zoekfunctie binnen types
        public async Task<IActionResult> TSearch(AddProductTypeVM viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.TypeSearch))
            {
                viewModel.ProductTypes = await _context.ProductTypes
                    .Where(x => x.Omschrijving.Contains(viewModel.TypeSearch))
                    .OrderBy(x => x.Omschrijving)
                    .ToListAsync();
            }
            else
            {
                viewModel.ProductTypes = await _context.ProductTypes
                    .OrderBy(x => x.Omschrijving)
                    .ToListAsync();
            }
            return View("AddProductType", viewModel);

        }

        // Nieuw types aanmaken
        public IActionResult CreateProductType()
        {
            AddProductTypeVM viewModel = new AddProductTypeVM();
            viewModel.ProductType = new ProductType();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductType(AddProductTypeVM viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!PTExists(viewModel.ProductType.Omschrijving))
                {
                    _context.Add(viewModel.ProductType);
                    await _context.SaveChangesAsync();
                    ViewBag.Visibility = false;
                    return RedirectToAction(nameof(AddProductType));
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(viewModel.ProductType.Omschrijving))
                    {
                        ViewBag.ErrorMessage = $"Gelieve een geldige tekst in te geven!";
                        ViewBag.Visibility = true;
                        viewModel.ProductTypes = new List<ProductType>(_context.ProductTypes);
                        return View("AddProductType", viewModel);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"{viewModel.ProductType.Omschrijving} bestaat reeds als producttype";
                        ViewBag.Visibility = true;
                        viewModel.ProductTypes = new List<ProductType>(_context.ProductTypes);
                        return View("AddProductType", viewModel);
                    }

                }
            }
            return View(viewModel);
        }

        private bool PTExists(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                return true;
            }
            else
            {
                return _context.ProductTypes.Any(x => x.Omschrijving.ToUpper() == txt.ToUpper());
            }
        }

        // Types verwijderen
        public async Task<IActionResult> DeleteType(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AddProductTypeVM viewModel = new AddProductTypeVM();
            viewModel.ProductType = await _context.ProductTypes
                .Where(x => x.ProductTypeID == id)
                .FirstOrDefaultAsync();
            if (viewModel.ProductType == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpPost, ActionName("DeleteType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTypeConfirmed(int id, AddProductTypeVM viewModel)
        {
            if (id != viewModel.ProductType.ProductTypeID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Remove(viewModel.ProductType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddProductType));
            }
            else
            {
                return RedirectToAction(nameof(AddProductType));
            }
        }

        // Producttypes aanpassen inladen
        public async Task<IActionResult> EditType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AddProductTypeVM viewModel = new AddProductTypeVM();
            viewModel.ProductType = await _context.ProductTypes.FindAsync(id);
            if (viewModel.ProductType == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // Types aanpassen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditType(int id, AddProductTypeVM viewModel)
        {
            if (id != viewModel.ProductType.ProductTypeID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.ProductType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeCheck(viewModel.ProductType.ProductTypeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AddProductType));
            }
            return View("AddProductType", viewModel);
        }

        private bool TypeCheck(int id)
        {
            return _context.ProductTypes.Any(x => x.ProductTypeID == id);
        }

        #endregion
    }
}
