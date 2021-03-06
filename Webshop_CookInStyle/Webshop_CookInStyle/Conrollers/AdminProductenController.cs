using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Data;
using Webshop_CookInStyle.Models;
using Webshop_CookInStyle.ViewModels;

namespace Webshop_CookInStyle.Conrollers
{
    [Authorize(Roles = "Admin")]
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
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType)
                .OrderBy(x => x.ProductType.Volgnummer).ThenBy(x => x.Naam)
                .ToListAsync();
            viewModel.Product = new Product();
            viewModel.AllergeenList = new List<Allergeen>();
            viewModel.Allergenen = new SelectList(_context.Allergenen.OrderBy(x => x.Omschrijving), "AllergeenID", "Omschrijving");
            viewModel.GeselecteerdeAllergenen = new List<int>();
            viewModel.Producttypes = new SelectList(_context.ProductTypes.OrderBy(x => x.Volgnummer), "ProductTypeID", "Omschrijving");
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
                if (viewModel.GeselecteerdeAllergenen == null)
                {
                    viewModel.GeselecteerdeAllergenen = new List<int>();
                }

                if (!ProductExists(viewModel))
                {
                    List<AllergeenProduct> nieuweAllergenen = new List<AllergeenProduct>();
                    foreach (int allergeenID in viewModel.GeselecteerdeAllergenen)
                    {
                        nieuweAllergenen.Add(new AllergeenProduct
                        {
                            AllergeenID = allergeenID,
                            ProductID = viewModel.Product.ProductID
                        });
                    }
                    _context.Add(viewModel.Product);
                    await _context.SaveChangesAsync();

                    Product product = await _context.Producten.Include(x => x.Allergenen)
                        .SingleOrDefaultAsync(x => x.ProductID == viewModel.Product.ProductID);
                    product.Allergenen.AddRange(nieuweAllergenen);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    IndexAdminProductenVM vm = new IndexAdminProductenVM();
                    LoadIn(vm);
                    vm.Bericht = $"Product: {product.Naam} werd succesvol opgeslagen!";
                    vm.BerichIsError = false;
                    return View("Index", vm);
                }
                else
                {
                    ProductType Test = _context.ProductTypes.Where(x => x.ProductTypeID == viewModel.Product.ProductTypeID).FirstOrDefault();
                    string PTNaam = Test.Omschrijving;
                    viewModel.Bericht = $"Er bestaat reeds een product met de naam: {viewModel.Product.Naam} en producttype: {PTNaam}";
                    viewModel.BerichIsError = true;
                    LoadIn(viewModel);
                    return View("Index", viewModel);
                }
            }
            LoadIn(viewModel);
            return View(viewModel);
        }

        private bool ProductExists(IndexAdminProductenVM viewModel)
        {
            if (viewModel == null)
            {
                return true;
            }
            else
            {
                return _context.Producten.Any(x => x.Naam.ToUpper() == viewModel.Product.Naam.ToUpper() && x.ProductTypeID == viewModel.Product.ProductTypeID);
            }
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
                viewModel.Producten = await _context.Producten.Include(x => x.ProductType)
                    .ToListAsync();
            }
            return View("Index", viewModel);
        }

        public void LoadIn(IndexAdminProductenVM viewModel)
        {
            viewModel.AllergeenList = new List<Allergeen>();
            viewModel.GeselecteerdeAllergenen = new List<int>();
            viewModel.Producten = _context.Producten.Include(x => x.ProductType)
                                                    .OrderBy(x => x.ProductType.Volgnummer).ThenBy(x => x.Naam)
                                                    .ToList();
            viewModel.Allergenen = new SelectList(_context.Allergenen.OrderBy(x => x.Omschrijving), "AllergeenID", "Omschrijving");
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

        public async Task<IActionResult> DeleteProduct(int id, EditProductenVM viewModel)
        {
            Product toDelete = await _context.Producten.Where(x => x.ProductID == id).FirstOrDefaultAsync();
            string naam = toDelete.Naam;
            _context.Remove(toDelete);
            await _context.SaveChangesAsync();
            IndexAdminProductenVM vm = new IndexAdminProductenVM();
            LoadIn(vm);
            vm.BerichIsError = false;
            vm.Bericht = $"Product: {naam} werd succesvol verwijderd!";
            return View("Index", vm);
        }

        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EditProductenVM viewModel = new EditProductenVM();
            viewModel.Product = await _context.Producten
                .Include(x => x.ProductType)
                .Include(x => x.Btwtype)
                .Include(x => x.Eenheid)
                .Include(x => x.Allergenen)
                .Where(x => x.ProductID == id)
                .FirstOrDefaultAsync();
            if (viewModel.Product == null)
            {
                return NotFound();
            }
            viewModel.Allergenen = new SelectList(_context.Allergenen.OrderBy(x => x.Omschrijving), "AllergeenID", "Omschrijving");
            viewModel.GeselecteerdeAllergenen = new List<int>();
            viewModel.ProductTypes = new SelectList(_context.ProductTypes.OrderBy(x => x.Omschrijving), "ProductTypeID", "Omschrijving");
            viewModel.BtwTypes = new SelectList(_context.BtwTypes.OrderBy(x => x.Percentage), "BtwID", "Weergave");
            viewModel.Eenheden = new SelectList(_context.Eenheden.OrderBy(x => x.Omschrijving), "EenheidID", "Omschrijving");
            return View(viewModel);
        }

        // Types aanpassen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, EditProductenVM viewModel)
        {
            Product aangepastProduct = null;
            IndexAdminProductenVM vm = new IndexAdminProductenVM();
            if (id != viewModel.Product.ProductID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                aangepastProduct = await _context.Producten
                    .Include(x => x.Allergenen)
                    .Include(x => x.ProductType)
                    .Include(x => x.Btwtype)
                    .Include(x => x.Eenheid)
                    .Include(x => x.Allergenen)
                    .Where(x => x.ProductID == id)
                    .FirstOrDefaultAsync();

                aangepastProduct.Naam = viewModel.Product.Naam;
                aangepastProduct.Omschrijving = viewModel.Product.Omschrijving;
                aangepastProduct.Eenheidsprijs = viewModel.Product.Eenheidsprijs;
                aangepastProduct.ProductTypeID = viewModel.Product.ProductTypeID;
                aangepastProduct.BtwID = viewModel.Product.BtwID;
                aangepastProduct.EenheidID = viewModel.Product.EenheidID;
                aangepastProduct.BeschikbaarInWebshop = viewModel.Product.BeschikbaarInWebshop;
                if (viewModel.GeselecteerdeAllergenen == null)
                {
                    viewModel.GeselecteerdeAllergenen = new List<int>();
                }

                List<AllergeenProduct> allergeenProducts = new List<AllergeenProduct>();
                foreach (var allergeenID in viewModel.GeselecteerdeAllergenen)
                {
                    allergeenProducts.Add(new AllergeenProduct
                    {
                        ProductID = viewModel.Product.ProductID,
                        AllergeenID = allergeenID
                    });
                }
                if (viewModel.GeselecteerdeAllergenen.Count() > 0)
                {
                    aangepastProduct.Allergenen
                                    .RemoveAll(a => !allergeenProducts.Contains(a));
                    aangepastProduct.Allergenen.AddRange(
                        allergeenProducts.Where(x => !aangepastProduct.Allergenen.Contains(x)));
                }

                _context.Update(aangepastProduct);
                await _context.SaveChangesAsync();

                LoadIn(vm);
                vm.BerichIsError = false;
                vm.Bericht = $"Product: {aangepastProduct.Naam} werd succesvol aangepast!";
                return View("Index", vm);
            }
            LoadIn(vm);
            vm.BerichIsError = true;
            vm.Bericht = $"Product: {aangepastProduct.Naam} is niet aangepast!";
            return View("Index", vm);
        }

        #endregion Producten

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
                return RedirectToAction(nameof(AddAllergeen));
            }
            //Als allergenen in gebruik zijn = niet te verwijderen.
            AllergeenProduct check = await _context.AllergeenProducten.Where(x => x.AllergeenID == id).FirstOrDefaultAsync();
            if (check != null)
            {
                Allergeen allergeen = await _context.Allergenen.Where(x => x.AllergeenID == id).FirstOrDefaultAsync();
                AddAllergeenVM vm = new AddAllergeenVM();
                vm.AllergenenList = await _context.Allergenen
                    .OrderBy(x => x.Omschrijving)
                    .ToListAsync();
                vm.Melding = $"{allergeen.Omschrijving}, kan niet verwijderd worden zolang er nog producten aan gekoppeld zijn.";
                return View("AddAllergeen", vm);
            }
            else
            {
                _context.Remove(viewModel.Allergeen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddAllergeen)); 
            }

        }

        #endregion Allergenen

        #region Product Type

        // Inladen types bij opstart pagina
        public async Task<IActionResult> AddProductType()
        {
            AddProductTypeVM viewModel = new AddProductTypeVM();
            viewModel.ProductTypes = await _context.ProductTypes
                .OrderBy(x => x.Volgnummer)
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
                    .OrderBy(x => x.Volgnummer)
                    .ToListAsync();
            }
            else
            {
                viewModel.ProductTypes = await _context.ProductTypes
                    .OrderBy(x => x.Volgnummer)
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
                return RedirectToAction(nameof(AddProductType));
            }

                Product check = await _context.Producten.Where(x => x.ProductTypeID == id).FirstOrDefaultAsync();
                if (check != null)
                {
                    ProductType productType = await _context.ProductTypes.Where(x => x.ProductTypeID == id).FirstOrDefaultAsync();
                    AddProductTypeVM vm = new AddProductTypeVM();
                    vm.ProductTypes = await _context.ProductTypes
                        .OrderBy(x => x.Volgnummer)
                        .ToListAsync();
                    vm.Melding = $"{productType.Omschrijving}, kan niet verwijderd worden zolang er nog producten aan gekoppeld zijn.";
                    return View("AddProductType", vm);
                }
                else
                {
                    _context.Remove(viewModel.ProductType);
                    await _context.SaveChangesAsync();
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

        #endregion Product Type
    }
}