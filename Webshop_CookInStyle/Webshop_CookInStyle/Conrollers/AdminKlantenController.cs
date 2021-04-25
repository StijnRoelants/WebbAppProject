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
    public class AdminKlantenController : Controller
    {
        private readonly WebshopContext _context;
        private readonly UserManager<Klant> _userManager;

        public AdminKlantenController(WebshopContext context, UserManager<Klant> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IndexAdminKlantenVM viewModel = new IndexAdminKlantenVM();
            viewModel.Klanten = await _context.Klanten.Include(x => x.Postcode).Include(x => x.Land).OrderBy(x => x.Achternaam).ThenBy(x => x.Voornaam).ToListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Search(IndexAdminKlantenVM viewModel)
        {
            List<Klant> SearchResult = new List<Klant>();

            if (viewModel.NaamSearch == true)
            {
                SearchResult.AddRange(_context.Klanten.Where(x => x.Voornaam.Contains(viewModel.SearchQuerry) || x.Achternaam.Contains(viewModel.SearchQuerry)).Include(x => x.Postcode).Include(x => x.Land).OrderBy(x => x.Achternaam).ThenBy(x => x.Voornaam));
            }
            if (viewModel.FirmanaamSearch == true)
            {
                SearchResult.AddRange(_context.Klanten.Where(x => x.Naam.Contains(viewModel.SearchQuerry)).Include(x => x.Postcode).Include(x => x.Land).OrderBy(x => x.Achternaam).ThenBy(x => x.Voornaam));
            }
            if (viewModel.BtwnummerSearch == true)
            {
                SearchResult.AddRange(_context.Klanten.Where(x => x.BtwNummer == viewModel.SearchQuerry).Include(x => x.Postcode).Include(x => x.Land).OrderBy(x => x.Achternaam).ThenBy(x => x.Voornaam));
            }
            if (SearchResult.Count == 0)
            {
                SearchResult = await _context.Klanten.Include(x => x.Postcode).Include(x => x.Land).OrderBy(x => x.Achternaam).ThenBy(x => x.Voornaam).ToListAsync();
            }

            viewModel.Klanten = SearchResult;

            return View("Index", viewModel);
        }

        public async Task<IActionResult> AddKlant()
        {
            AddKlantVM viewModel = new AddKlantVM();
            viewModel.Klant = new Klant();
            viewModel.Postcodes = new SelectList( _context.Postcodes.OrderBy(x => x.Nummer), "PostcodeID", "Weergave");
            viewModel.Landen = new SelectList( _context.Landen.OrderBy(x => x.Naam), "LandID", "Naam");
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddKlant(AddKlantVM viewModel)
        {
                var Password = "Cookinstyle!20@";
                Klant nieuweKlant = new Klant()
                {
                    Voornaam = viewModel.Klant.Voornaam,
                    Achternaam = viewModel.Klant.Achternaam,
                    Email = viewModel.Klant.Email,
                    UserName = viewModel.Klant.Email.ToLower(),
                    NormalizedEmail = viewModel.Klant.Email.ToUpper(),
                    NormalizedUserName = viewModel.Klant.Email.ToUpper(),
                    EmailConfirmed = true,
                    Mobiel = viewModel.Klant.Mobiel,
                    Telefoon = viewModel.Klant.Telefoon,
                    PostcodeID = viewModel.Klant.PostcodeID,
                    StraatEnNummer = viewModel.Klant.StraatEnNummer,
                    LandID = viewModel.Klant.LandID,
                    WachtWoordReset = true,
                    Naam = viewModel.Klant.Naam
                };
                if (viewModel.Klant.Naam != null)
                {
                    nieuweKlant.IsBedrijf = true;
                    nieuweKlant.BtwNummer = viewModel.Klant.BtwNummer.ToString().Replace(".", "");
                }

                var result = await _userManager.CreateAsync(nieuweKlant, Password);
                if (result.Succeeded)
                {
                    viewModel.Klant = new Klant();
                    viewModel.Postcodes = new SelectList(_context.Postcodes.OrderBy(x => x.Nummer), "PostcodeID", "Weergave");
                    viewModel.Landen = new SelectList(_context.Landen.OrderBy(x => x.Naam), "LandID", "Naam");
                    return View(viewModel);
                }
                else
                {
                    viewModel.Klant = new Klant();
                    viewModel.Postcodes = new SelectList(_context.Postcodes.OrderBy(x => x.Nummer), "PostcodeID", "Weergave");
                    viewModel.Landen = new SelectList(_context.Landen.OrderBy(x => x.Naam), "LandID", "Naam");
                    return View(viewModel);
                }
            }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                NotFound();
            }
            AddKlantVM viewModel = new AddKlantVM();
            viewModel.Klant = await _context.Klanten.Where(x => x.Id == id).Include(x => x.Postcode).Include(x => x.Land).FirstOrDefaultAsync();
            if (viewModel.Klant == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }
    }
    }
