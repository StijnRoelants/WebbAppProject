using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
    [Authorize(Roles = "Admin")]
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
            viewModel.Bestellingen = await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= DateTime.Now).OrderBy(x => x.Leverdatum).ThenBy(x => x.Bestelbonnummer).ToListAsync();
            viewModel.ZoekenDatumVan = DateTime.Now;
            viewModel.ZoekenDatumTot = DateTime.Now.AddDays(15);
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
            Zoekopdracht.AddRange(await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Klant.Voornaam.Contains( viewModel.ZoekenOpNaam )|| x.Klant.Achternaam == viewModel.ZoekenOpNaam).ToListAsync());
            Zoekopdracht.AddRange(await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Bestelbonnummer.Contains( viewModel.ZoekenOpNaam )).ToListAsync());
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

        #region Bestelling details + Leveradres

        public async Task<IActionResult> AddBestellingDetails(int id)
        {
            AddBestellingDetailsVM viewModel = new AddBestellingDetailsVM();
            viewModel.Bestelling = await _context.Bestellingen.Where(x => x.BestellingID == id).Include(x => x.Bestellijnen).FirstOrDefaultAsync();
            viewModel.Bestellijnen = await _context.Bestellijnen.Where(x => x.BestellingID == id).Include(x => x.Product).ToListAsync();
            viewModel.Klanten = new SelectList(await _context.Klanten.Include(x => x.Postcode).ToListAsync(), "Id", "NaamWeergaveExtended");
            viewModel.Bestelling.Leverdatum = DateTime.Now;
            BtwWeergave(viewModel);
            return View(viewModel);
        }

        public async Task<IActionResult> CheckEnAfsluiten(int? id, AddBestellingDetailsVM viewModel)
        {
            Bestelling aangepasteBestelling = await _context.Bestellingen.Where(x => x.BestellingID == id).FirstOrDefaultAsync();
            aangepasteBestelling.Leverdatum = viewModel.Bestelling.Leverdatum;
            aangepasteBestelling.KlantFK = viewModel.Bestelling.KlantFK;
            aangepasteBestelling.Opmerking = viewModel.Bestelling.Opmerking;
            if (viewModel.LeverAdresIsStandaard == true)
            {
                Klant klant = await _context.Klanten.Where(x => x.Id == aangepasteBestelling.KlantFK).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
                aangepasteBestelling.LeverAdresID = klant.LeverAdressen.OrderBy(x => x.LeverAdresID).FirstOrDefault().LeverAdresID;
                _context.Bestellingen.Update(aangepasteBestelling);
                _context.SaveChanges();
            }
            else
            {
                _context.Bestellingen.Update(aangepasteBestelling);
                _context.SaveChanges();
                await LeverAdresAanpassen(aangepasteBestelling.BestellingID);
            }

            viewModel.Bestelling = await _context.Bestellingen.Where(x => x.BestellingID == id).Include(x => x.Bestellijnen).FirstOrDefaultAsync();
            viewModel.Bestellijnen = await _context.Bestellijnen.Where(x => x.BestellingID == id).Include(x => x.Product).ToListAsync();
            viewModel.Klanten = new SelectList(await _context.Klanten.Include(x => x.Postcode).ToListAsync(), "Id", "NaamWeergaveExtended");
            viewModel.Bestelling.Leverdatum = DateTime.Now;
            viewModel.Bericht = $"Bestelling {aangepasteBestelling.Bestelbonnummer} werd succesvol opgeslagen!";
            BtwWeergave(viewModel);
            return View("AddBestellingDetails", viewModel);
        }

        public async Task<IActionResult> LeverAdresAanpassen(int? id)
        {
            LeverAdresAanpassenVM viewModel = new LeverAdresAanpassenVM();
            Bestelling bestelling = await _context.Bestellingen.Where(x => x.BestellingID == id).FirstOrDefaultAsync();
            Klant klant = await _context.Klanten.Where(x => x.Id == bestelling.KlantFK).FirstOrDefaultAsync();
            viewModel.Leveradressen = new SelectList(await _context.LeverAdressen.Where(x => x.KlantFK == klant.Id).Include(x => x.Postcode).OrderBy(x => x.Omschrijving).ToListAsync(), "LeverAdresID", "Weergave");
            return View(viewModel);
        }

        private void BtwWeergave(AddBestellingDetailsVM viewModel)
        {
            decimal zesProcent = 0, eenentwintigProcent = 0;
            foreach (var bestellijn in viewModel.Bestellijnen)
            {
                Product check = _context.Producten.Where(x => x.ProductID == bestellijn.Product.ProductID).Include(x => x.Btwtype).FirstOrDefault();
                if (check.Btwtype.Percentage == 6)
                {
                    zesProcent += bestellijn.BtwBedrag;
                }
                else
                {
                    eenentwintigProcent += bestellijn.BtwBedrag;
                }
            }
            viewModel.Btw6 = zesProcent;
            viewModel.Btw21 = eenentwintigProcent;
        }



        #endregion

        #region Nieuwe bestelling plaatsen

        public async Task<IActionResult> AddBestelling()
        {
            AddBestellingVM viewModel = new AddBestellingVM();
            Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            LeverAdres tempLeverAdres = admin.LeverAdressen.First();
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin);
            if (bestelling == null)
            {
                bestelling = new Bestelling { Bestelbonnummer = "In afwachting", BestelDatum = DateTime.Now, Klant = admin, Totaalprijs = 0, Opmerking = "", LeverAdres = tempLeverAdres };
                bestelling.Bestellijnen = new List<Bestellijn>();
            }
            viewModel.NieuweBestelling = bestelling;
            viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType).ThenBy(x => x.Naam).ToListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Verhoog(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            #region inladen
            AddBestellingVM viewModel = new AddBestellingVM();
            Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            LeverAdres tempLeverAdres = admin.LeverAdressen.First();
            Product product = await _context.Producten.Where(x => x.ProductID == id).Include(x => x.Btwtype).FirstOrDefaultAsync();
            
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin);
            #endregion
            if (bestelling == null)
            {
                string nummer = BestelnummerOphalen();
                bestelling = new Bestelling { Bestelbonnummer = nummer, BestelDatum = DateTime.Now, Klant = admin, Totaalprijs = 0, Opmerking = "", LeverAdres = tempLeverAdres };
                int parsen = product.Btwtype.Percentage;
                decimal btw = Convert.ToDecimal(parsen);
                decimal btwlijn = product.Eenheidsprijs / 100 * btw;
                Bestellijn nieuweLijn = new Bestellijn { Aantal = 1, Bestelling = bestelling, BestellingID = bestelling.BestellingID, Product = product, ProductID = product.ProductID, BtwBedrag = btwlijn, Lijnprijs = product.Eenheidsprijs + btwlijn, Eenheidsprijs = product.Eenheidsprijs };
                _context.Bestellijnen.Add(nieuweLijn);
            }
            else
            {
                if (bestelling.Bestellijnen.Count() != 0)
                {
                    Bestellijn bestellijn = await _context.Bestellijnen.FirstOrDefaultAsync(x => x.BestellingID == bestelling.BestellingID && x.ProductID == product.ProductID);
                    if (bestellijn == null)
                    {
                        
                        bestellijn = new Bestellijn { Aantal = 1, Bestelling = bestelling, BestellingID = bestelling.BestellingID, Product = product, ProductID = product.ProductID, BtwBedrag = product.Btwtype.Percentage, Lijnprijs = 0, Eenheidsprijs = product.Eenheidsprijs };
                        LijnPrijsBerekenen(bestellijn, product);
                        _context.Bestellijnen.Add(bestellijn);
                    }
                    else
                    {
                        bestellijn.Aantal++;
                        LijnPrijsBerekenen(bestellijn, product);
                        _context.Bestellijnen.Update(bestellijn);
                    }
                }
                else
                {
                    Bestellijn nieuweLijn = new Bestellijn { Aantal = 1, Bestelling = bestelling, BestellingID = bestelling.BestellingID, Product = product, ProductID = product.ProductID, BtwBedrag = product.Btwtype.Percentage, Lijnprijs = 0, Eenheidsprijs = product.Eenheidsprijs };
                    LijnPrijsBerekenen(nieuweLijn, product);
                    _context.Bestellijnen.Add(nieuweLijn);
                }
            }
            TotalenBerkenen(bestelling);
            _context.SaveChanges();

            viewModel.NieuweBestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin);
            viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType).ThenBy(x => x.Naam).ToListAsync();
            return View("AddBestelling", viewModel);
        }

        public async Task<IActionResult> Annuleren()
        {
            Bestelling nieuweBestelling = null;
            Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            Bestelling teAnnuleren = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin);
            if (teAnnuleren != null)
            {
                nieuweBestelling = new Bestelling { Bestelbonnummer = teAnnuleren.Bestelbonnummer, Totaalprijs = 0, Klant = admin, KlantFK = admin.Id, LeverAdres = teAnnuleren.LeverAdres, LeverAdresID = teAnnuleren.LeverAdresID };
                nieuweBestelling.Bestellijnen = new List<Bestellijn>();
                if (teAnnuleren.Bestellijnen.Count > 0)
                {
                    _context.Bestellingen.Remove(teAnnuleren);
                    _context.SaveChanges();
                }
            }
            else
            {
                LeverAdres tempLeverAdres = admin.LeverAdressen.First();
                string nummer = BestelnummerOphalen();
                nieuweBestelling = new Bestelling { Bestelbonnummer = nummer, Totaalprijs = 0, Klant = admin, KlantFK = admin.Id, LeverAdres = tempLeverAdres, LeverAdresID = tempLeverAdres.LeverAdresID };
                nieuweBestelling.Bestellijnen = new List<Bestellijn>();
            }

            AddBestellingVM viewModel = new AddBestellingVM();
            viewModel.NieuweBestelling = nieuweBestelling;
            viewModel.Orderlijnen = nieuweBestelling.Bestellijnen.ToList();
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType).ThenBy(x => x.Naam).ToListAsync();
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin);
            return View("AddBestelling", viewModel);
        }

        public async Task<IActionResult> Verminder (int? id)
        {
            AddBestellingVM viewModel = new AddBestellingVM();
            Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            Product product = await _context.Producten.Where(x => x.ProductID == id).Include(x => x.Btwtype).FirstOrDefaultAsync();
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin);
            Bestellijn teVermindereLijn = await _context.Bestellijnen.FirstOrDefaultAsync(x => x.ProductID == product.ProductID && x.BestellingID == bestelling.BestellingID);
            if (teVermindereLijn != null)
            {
                teVermindereLijn.Aantal--;
                if (teVermindereLijn.Aantal == 0)
                {
                    _context.Bestellijnen.Remove(teVermindereLijn);
                }
                else
                {
                    LijnPrijsBerekenen(teVermindereLijn, product);
                    _context.Bestellijnen.Update(teVermindereLijn);
                }
            }
            else
            {
                viewModel.NieuweBestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin);
                viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
                viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType).ThenBy(x => x.Naam).ToListAsync();
                return View("AddBestelling", viewModel);
            }
            TotalenBerkenen(bestelling);
            _context.SaveChanges();
            viewModel.NieuweBestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin);
            viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType).ThenBy(x => x.Naam).ToListAsync();
            return View("AddBestelling", viewModel);
        }

        private void LijnPrijsBerekenen(Bestellijn lijn, Product product)
        {
            decimal btw = Convert.ToDecimal(product.Btwtype.Percentage);
            decimal aantal = Convert.ToDecimal(lijn.Aantal);
            lijn.BtwBedrag = (product.Eenheidsprijs / (100+btw) * btw) * aantal;
            lijn.Lijnprijs = product.Eenheidsprijs * aantal;
        }

        private void TotalenBerkenen(Bestelling bestelling)
        {
            decimal totaal = 0;
            foreach (var orderlijn in bestelling.Bestellijnen)
            {
                totaal += orderlijn.Lijnprijs;
            }
            bestelling.Totaalprijs = totaal;
        }

        public string BestelnummerOphalen()
        {
            Factuurfirma ff = _context.FactuurFirmas.FirstOrDefault();
            string bonnummer = ff.Bestelbonnummering;
            string nieuwenummer = "";
            string voorvoegsel = bonnummer.Substring(0, 2);
            string check = bonnummer.Substring(2);
            if (int.TryParse(check, out int teller))
            {
                Bestelling checkHighestNumber = _context.Bestellingen.OrderByDescending(x => x.BestellingID).FirstOrDefault();
                if (checkHighestNumber != null)
                {
                    string DbCheck = checkHighestNumber.Bestelbonnummer.Substring(2);
                    int dbNummer = int.Parse(DbCheck);
                    if (dbNummer > teller)
                    {
                        teller = int.Parse(checkHighestNumber.Bestelbonnummer.Substring(2));
                    }
                }
                teller++;
                nieuwenummer = voorvoegsel + teller.ToString();
                ff.Bestelbonnummering = nieuwenummer;
                _context.Update(ff);
                _context.SaveChanges();
            }
            return nieuwenummer;
        }

        #endregion
    }
}
