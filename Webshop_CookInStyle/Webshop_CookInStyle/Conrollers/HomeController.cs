using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Data;
using Webshop_CookInStyle.Models;
using Webshop_CookInStyle.ViewModels;

namespace Webshop_CookInStyle.Conrollers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebshopContext _context;

        public HomeController(ILogger<HomeController> logger, WebshopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
            {
                IndexUserBestellingenVM viewModel = new IndexUserBestellingenVM();
                viewModel.BeschikbareProducten = await _context.Producten
                    .Include(x => x.ProductType).Include(x => x.Allergenen).ThenInclude(x => x.Allergeen).Include(x => x.Eenheid)
                    .OrderBy(x => x.ProductType.Volgnummer).ThenBy(x => x.Naam)
                    .Where(x => x.BeschikbaarInWebshop == true).ToListAsync();
                OpbouwProductLijst(viewModel);
                    return View(viewModel);
            }

        private void OpbouwProductLijst(IndexUserBestellingenVM viewModel)
        {
            viewModel.Typelijst = new List<ProductType>();
            foreach (var item in viewModel.BeschikbareProducten)
            {
                ProductType type = item.ProductType;
                if (!viewModel.Typelijst.Contains(type))
                {
                    viewModel.Typelijst.Add(type);
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
