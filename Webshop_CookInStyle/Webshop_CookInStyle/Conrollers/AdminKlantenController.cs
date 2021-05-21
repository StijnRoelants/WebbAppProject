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
    [Authorize(Roles ="Admin")]
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
                LeverAdres leverAdres = new LeverAdres { Klant = nieuweKlant, KlantFK = nieuweKlant.Id, LandID = nieuweKlant.LandID, Land = nieuweKlant.Land, PostcodeID = nieuweKlant.PostcodeID, Postcode = nieuweKlant.Postcode, Straat = nieuweKlant.StraatEnNummer, Omschrijving = "Standaard" };
                _context.Add(leverAdres);
                _context.SaveChanges();
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

        public async Task<IActionResult> EditKlant(string id)
        {
            EditKlantVM viewModel = new EditKlantVM();
            if (id == null)
            {
                NotFound();
            }
            viewModel.UserRoleList = _context.UserRoles;
            viewModel.Klant = await _context.Klanten.Where(x => x.Id == id).Include(x => x.Postcode).Include(x => x.Land).FirstOrDefaultAsync();
            viewModel.Postcodes = new SelectList(_context.Postcodes.OrderBy(x => x.Nummer), "PostcodeID", "Weergave");
            viewModel.Landen = new SelectList(_context.Landen.OrderBy(x => x.Naam), "LandID", "Naam");
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditKlant(string id, EditKlantVM viewModel)
        {
            if (id != viewModel.Klant.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                Klant aangepasteKlant = await _context.Klanten
                    .Include(x => x.Postcode)
                    .Include(x => x.Land)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (viewModel.Klant.Email.ToUpper() != aangepasteKlant.Email.ToUpper())
                {
                    aangepasteKlant.Email = viewModel.Klant.Email.ToLower();
                    aangepasteKlant.NormalizedEmail = viewModel.Klant.Email.ToUpper();
                    aangepasteKlant.NormalizedUserName = viewModel.Klant.Email.ToUpper();
                    aangepasteKlant.UserName = viewModel.Klant.Email.ToLower();
                }

                if (viewModel.MaakAdmin == true)
                {
                    DbSet<IdentityUserRole<string>> roles = _context.UserRoles;
                    IdentityRole adminRole = _context.Roles.Where(x => x.NormalizedName == "ADMIN").FirstOrDefault();
                    if (adminRole != null)
                    {
                        if (!roles.Any(x => x.UserId == aangepasteKlant.Id && x.RoleId == adminRole.Id))
                        {
                            roles.Add(new IdentityUserRole<string>() { UserId = aangepasteKlant.Id, RoleId = adminRole.Id });
                        }
                    }
                }

                if (viewModel.Klant.IsBedrijf == true)
                {
                    aangepasteKlant.Naam = viewModel.Klant.Naam;
                    aangepasteKlant.BtwNummer = viewModel.Klant.BtwNummer;
                }

                aangepasteKlant.Voornaam = viewModel.Klant.Voornaam;
                aangepasteKlant.Achternaam = viewModel.Klant.Achternaam;
                aangepasteKlant.StraatEnNummer = viewModel.Klant.StraatEnNummer;
                aangepasteKlant.Telefoon = viewModel.Klant.Telefoon;
                aangepasteKlant.Mobiel = viewModel.Klant.Mobiel;
                aangepasteKlant.PostcodeID = viewModel.Klant.PostcodeID;
                aangepasteKlant.LandID = viewModel.Klant.LandID;

                _context.Update(aangepasteKlant);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View("Index", viewModel);
        }
    }
    }
