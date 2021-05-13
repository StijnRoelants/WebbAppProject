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

        // Inladen van de pagina lijst van bestelling 2 komnende weken om performatie steeds te behouden kan door user vergroot worden 
        public async Task<IActionResult> Index()
        {
            IndexAdminBestellingenVM viewModel = new IndexAdminBestellingenVM();
            viewModel.Bestellingen = await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= DateTime.Now).OrderBy(x => x.Leverdatum).ThenBy(x => x.Bestelbonnummer).ToListAsync();
            viewModel.ZoekenDatumVan = DateTime.Now;
            viewModel.ZoekenDatumTot = DateTime.Now.AddDays(15);
            return View(viewModel);
        }

        // Bestelling zoeken, zowel op naam als op bonnummer
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

        // bestellingen zoeken op datum
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

        #region Nieuwe bestelling plaatsen

        // Inladen van de pagina
        public async Task<IActionResult> AddBestelling()
        {
            AddBestellingVM viewModel = new AddBestellingVM();
            Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            LeverAdres tempLeverAdres = admin.LeverAdressen.First();
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin && x.IsVoltooid == false);
            if (bestelling == null)
            {
                bestelling = new Bestelling { Bestelbonnummer = "In afwachting", BestelDatum = DateTime.Now, Klant = admin, Totaalprijs = 0, Opmerking = "", LeverAdres = tempLeverAdres, IsVoltooid = false };
                bestelling.Bestellijnen = new List<Bestellijn>();
            }
            viewModel.NieuweBestelling = bestelling;
            viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType.Volgnummer).ThenBy(x => x.Naam).ToListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> ProductZoeken(AddBestellingVM viewModel)
        {
            if (viewModel.Zoekopdracht != null)
            {
                Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
                LeverAdres tempLeverAdres = admin.LeverAdressen.First();
                Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin && x.IsVoltooid == false);
                if (bestelling == null)
                {
                    bestelling = new Bestelling { Bestelbonnummer = "In afwachting", BestelDatum = DateTime.Now, Klant = admin, Totaalprijs = 0, Opmerking = "", LeverAdres = tempLeverAdres, IsVoltooid = false };
                    bestelling.Bestellijnen = new List<Bestellijn>();
                }
                AddBestellingVM vm = new AddBestellingVM();
                vm.NieuweBestelling = bestelling;
                vm.Orderlijnen = await _context.Bestellijnen.Where(x => x.BestellingID == bestelling.BestellingID).Include(x => x.Product.ProductType).ToListAsync();
                vm.Producten = await _context.Producten.Where(x => x.Naam.Contains(viewModel.Zoekopdracht) || x.ProductType.Omschrijving.Contains(viewModel.Zoekopdracht))
                    .Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType.Volgnummer).ThenBy(x => x.Naam).ToListAsync();
                if (vm.Producten.Count != 0)
                {
                    return View("AddBestelling", vm);
                }
            }

            return RedirectToAction("AddBestelling");
        }

        // Lijnaantal verhogen wanneer het de eerste lijn van een bestelling is wordt er voor de bestelling een bonnummer gemaakt
        public async Task<IActionResult> Verhoog(int? id, AddBestellingVM viewModel)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            #region inladen
            //AddBestellingVM viewModel = new AddBestellingVM();
            Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            LeverAdres tempLeverAdres = admin.LeverAdressen.First();
            Product product = await _context.Producten.Where(x => x.ProductID == id).Include(x => x.Btwtype).FirstOrDefaultAsync();
            int aantal = CheckAantal(viewModel);
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin && x.IsVoltooid == false);
            #endregion
            if (bestelling == null)
            {
                string nummer = BestelnummerOphalen();
                bestelling = new Bestelling { Bestelbonnummer = nummer, BestelDatum = DateTime.Now, Klant = admin, Totaalprijs = 0, Opmerking = "", LeverAdres = tempLeverAdres, IsVoltooid = false };
                Bestellijn nieuweLijn = new Bestellijn { Aantal = aantal, Bestelling = bestelling, BestellingID = bestelling.BestellingID, Product = product, ProductID = product.ProductID, Eenheidsprijs = product.Eenheidsprijs };
                LijnPrijsBerekenen(nieuweLijn, product);
                _context.Bestellijnen.Add(nieuweLijn);
            }
            else
            {
                if (bestelling.Bestellijnen.Count() != 0)
                {
                    Bestellijn bestellijn = await _context.Bestellijnen.FirstOrDefaultAsync(x => x.BestellingID == bestelling.BestellingID && x.ProductID == product.ProductID);
                    if (bestellijn == null)
                    {
                        
                        bestellijn = new Bestellijn { Aantal = aantal, Bestelling = bestelling, BestellingID = bestelling.BestellingID, Product = product, ProductID = product.ProductID, BtwBedrag = product.Btwtype.Percentage, Lijnprijs = 0, Eenheidsprijs = product.Eenheidsprijs };
                        LijnPrijsBerekenen(bestellijn, product);
                        _context.Bestellijnen.Add(bestellijn);
                    }
                    else
                    {
                        bestellijn.Aantal += aantal;
                        LijnPrijsBerekenen(bestellijn, product);
                        _context.Bestellijnen.Update(bestellijn);
                    }
                }
                else
                {
                    Bestellijn nieuweLijn = new Bestellijn { Aantal = aantal, Bestelling = bestelling, BestellingID = bestelling.BestellingID, Product = product, ProductID = product.ProductID, BtwBedrag = product.Btwtype.Percentage, Lijnprijs = 0, Eenheidsprijs = product.Eenheidsprijs };
                    LijnPrijsBerekenen(nieuweLijn, product);
                    _context.Bestellijnen.Add(nieuweLijn);
                }
            }
            TotalenBerkenen(bestelling);
            _context.SaveChanges();

            viewModel.NieuweBestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin && x.IsVoltooid == false);
            viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType.Volgnummer).ThenBy(x => x.Naam).ToListAsync();
            return View("AddBestelling", viewModel);
        }

        // Bestelling annuleren + opvangen wanneer bestelling nog niet bestaat
        public async Task<IActionResult> Annuleren()
        {
            Bestelling nieuweBestelling = null;
            Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            Bestelling teAnnuleren = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin && x.IsVoltooid == false);
            if (teAnnuleren != null)
            {
                BonnummerCorrigeren(teAnnuleren);
                nieuweBestelling = new Bestelling { Bestelbonnummer = teAnnuleren.Bestelbonnummer, Totaalprijs = 0, Klant = admin, KlantFK = admin.Id, LeverAdres = teAnnuleren.LeverAdres, LeverAdresID = teAnnuleren.LeverAdresID, IsVoltooid = false };
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
                nieuweBestelling = new Bestelling { Bestelbonnummer = nummer, Totaalprijs = 0, Klant = admin, KlantFK = admin.Id, LeverAdres = tempLeverAdres, LeverAdresID = tempLeverAdres.LeverAdresID, IsVoltooid = false };
                nieuweBestelling.Bestellijnen = new List<Bestellijn>();
            }

            AddBestellingVM viewModel = new AddBestellingVM();
            viewModel.NieuweBestelling = nieuweBestelling;
            viewModel.Orderlijnen = nieuweBestelling.Bestellijnen.ToList();
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType.Volgnummer).ThenBy(x => x.Naam).ToListAsync();
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin && x.IsVoltooid == false);
            return View("AddBestelling", viewModel);
        }

        // Lijn in mindering brengen, wanneer er een lijn met dit product niet voorkomt gewoon herladen
        // bij 0 aantal van de lijn wordt de lijn verwijderd van de db
        public async Task<IActionResult> Verminder (int? id, AddBestellingVM viewModel)
        {
            //AddBestellingVM viewModel = new AddBestellingVM();
            Klant admin = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            Product product = await _context.Producten.Where(x => x.ProductID == id).Include(x => x.Btwtype).FirstOrDefaultAsync();
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin && x.IsVoltooid == false);
            Bestellijn teVermindereLijn = await _context.Bestellijnen.FirstOrDefaultAsync(x => x.ProductID == product.ProductID && x.BestellingID == bestelling.BestellingID);
            int aantal = CheckAantal(viewModel);


            if (teVermindereLijn != null)
            {
                if (aantal > teVermindereLijn.Aantal)
                {
                    aantal = teVermindereLijn.Aantal;
                }
                teVermindereLijn.Aantal -= aantal;
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
                viewModel.NieuweBestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin && x.IsVoltooid == false);
                viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
                viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType.Volgnummer).ThenBy(x => x.Naam).ToListAsync();
                return View("AddBestelling", viewModel);
            }
            TotalenBerkenen(bestelling);
            _context.SaveChanges();
            viewModel.NieuweBestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == admin && x.IsVoltooid == false);
            viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
            viewModel.Producten = await _context.Producten.Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid).OrderBy(x => x.ProductType.Volgnummer).ThenBy(x => x.Naam).ToListAsync();
            return View("AddBestelling", viewModel);
        }

        // BTW tarieven op lijnniveau berekenen
        private void LijnPrijsBerekenen(Bestellijn lijn, Product product)
        {
            decimal btw = Convert.ToDecimal(product.Btwtype.Percentage);
            decimal aantal = Convert.ToDecimal(lijn.Aantal);
            lijn.BtwBedrag = (product.Eenheidsprijs / (100+btw) * btw) * aantal;
            lijn.Lijnprijs = product.Eenheidsprijs * aantal;
        }

        // Aantallen voor bestelling
        private int CheckAantal(AddBestellingVM viewModel)
        {
            if (viewModel.Aantal == 0)
            {
                return 1;
            }
            else
            {
                return viewModel.Aantal;
            }
        }


        // Bestelling totaal berekenen
        private void TotalenBerkenen(Bestelling bestelling)
        {
            decimal totaal = 0;
            foreach (var orderlijn in bestelling.Bestellijnen)
            {
                totaal += orderlijn.Lijnprijs;
            }
            bestelling.Totaalprijs = totaal;
        }

        // Verhogen van de bonnummer
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

        // Bij Annuleren bonnummer terugplaatsen
        public void BonnummerCorrigeren(Bestelling bestelling)
        {
            Factuurfirma ff = _context.FactuurFirmas.FirstOrDefault();
            string bonnummer = bestelling.Bestelbonnummer;
            string nieuwenummer = "";
            string voorvoegsel = bonnummer.Substring(0, 2);
            string check = bonnummer.Substring(2);
            int toParse = int.Parse(check);
            toParse--;
            nieuwenummer = voorvoegsel + toParse.ToString();
            ff.Bestelbonnummering = nieuwenummer;
            _context.Update(ff);
            _context.SaveChanges();
        }

        #endregion

        #region Bestelling details + Leveradres

        // inladen van de pagina
        public async Task<IActionResult> AddBestellingDetails(int id)
        {
            AddBestellingDetailsVM viewModel = new AddBestellingDetailsVM();
            viewModel.Bestelling = await _context.Bestellingen.Where(x => x.BestellingID == id && x.IsVoltooid == false).Include(x => x.Bestellijnen).FirstOrDefaultAsync();
            viewModel.Bestellijnen = await _context.Bestellijnen.Where(x => x.BestellingID == id).Include(x => x.Product).ToListAsync();
            if (viewModel.Bestellijnen.Count == 0)
            {
                IndexAdminBestellingenVM vm = new IndexAdminBestellingenVM();
                vm.Bestellingen = await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= DateTime.Now).OrderBy(x => x.Leverdatum).ThenBy(x => x.Bestelbonnummer).ToListAsync();
                vm.ZoekenDatumVan = DateTime.Now;
                vm.ZoekenDatumTot = DateTime.Now.AddDays(15);
                return View("Index", vm);
            }
            viewModel.Klanten = new SelectList(await _context.Klanten.Include(x => x.Postcode).ToListAsync(), "Id", "NaamWeergave");
            viewModel.Bestelling.Leverdatum = DateTime.Now;
            BtwWeergave(viewModel);
            return View(viewModel);
        }

        // Bestelling voltooien, leveradres = standaard
        public async Task<IActionResult> CheckEnAfsluiten(int? id, AddBestellingDetailsVM viewModel)
        {
            Bestelling aangepasteBestelling = await _context.Bestellingen.Where(x => x.BestellingID == id).FirstOrDefaultAsync();
            aangepasteBestelling.Leverdatum = viewModel.Bestelling.Leverdatum;
            aangepasteBestelling.KlantFK = viewModel.Bestelling.KlantFK;
            aangepasteBestelling.Opmerking = viewModel.Bestelling.Opmerking;
            Klant klant = await _context.Klanten.Where(x => x.Id == aangepasteBestelling.KlantFK).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            aangepasteBestelling.LeverAdresID = klant.LeverAdressen.OrderBy(x => x.LeverAdresID).FirstOrDefault().LeverAdresID;
            aangepasteBestelling.IsVoltooid = true;
            _context.Bestellingen.Update(aangepasteBestelling);
            _context.SaveChanges();
            IndexAdminBestellingenVM vm = new IndexAdminBestellingenVM();
            vm.Bestellingen = await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= DateTime.Now).OrderBy(x => x.Leverdatum).ThenBy(x => x.Bestelbonnummer).ToListAsync();
            vm.ZoekenDatumVan = DateTime.Now;
            vm.ZoekenDatumTot = DateTime.Now.AddDays(15);
            vm.Bericht = $"Bestelling {aangepasteBestelling.Bestelbonnummer} werd succesvol opgeslagen!";
            return View("Index", vm);
        }

        // Eerst bestellings details opslaan om vervolgens nieuwe view in te laden
        public async Task<IActionResult> LeveradresWijzigen(int? id, AddBestellingDetailsVM viewModel)
        {
            Bestelling aangepasteBestelling = await _context.Bestellingen.Where(x => x.BestellingID == id).FirstOrDefaultAsync();
            aangepasteBestelling.Leverdatum = viewModel.Bestelling.Leverdatum;
            aangepasteBestelling.KlantFK = viewModel.Bestelling.KlantFK;
            aangepasteBestelling.Opmerking = viewModel.Bestelling.Opmerking;
            Klant klant = await _context.Klanten.Where(x => x.Id == aangepasteBestelling.KlantFK).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            aangepasteBestelling.LeverAdresID = klant.LeverAdressen.OrderBy(x => x.LeverAdresID).FirstOrDefault().LeverAdresID;
            _context.Bestellingen.Update(aangepasteBestelling);
            _context.SaveChanges();

            LeverAdresAanpassenVM viewModelNew = new LeverAdresAanpassenVM();
            Bestelling bestelling = await _context.Bestellingen.Where(x => x.BestellingID == id).FirstOrDefaultAsync();
            klant = await _context.Klanten.Where(x => x.Id == bestelling.KlantFK).FirstOrDefaultAsync();
            viewModelNew.Leveradressen = new SelectList(await _context.LeverAdressen.Where(x => x.KlantFK == klant.Id).Include(x => x.Postcode).OrderBy(x => x.Omschrijving).ToListAsync(), "LeverAdresID", "Weergave");
            viewModelNew.Postcodes = new SelectList(await _context.Postcodes.OrderBy(x => x.Nummer).ToListAsync(), "PostcodeID", "Weergave");
            viewModelNew.Landen = new SelectList(await _context.Landen.OrderBy(x => x.Naam).ToListAsync(), "LandID", "Naam");
            viewModelNew.Klant = klant;
            viewModelNew.Bestelling = bestelling;
            return View("LeverAdresAanpassen", viewModelNew);
        }

        // Bestelling voltooien met een bestaand leveradres uit lijst
        public async Task<IActionResult> SelectAdres(LeverAdresAanpassenVM viewModel)
        {
            Bestelling bestelling = await _context.Bestellingen.Where(x => x.BestellingID == viewModel.Bestelling.BestellingID).Include(x => x.Klant).FirstOrDefaultAsync();
            Klant klant = await _context.Klanten.Where(x => x.Id == bestelling.KlantFK).FirstOrDefaultAsync();
            bestelling.LeverAdres = viewModel.Bestelling.LeverAdres;
            bestelling.IsVoltooid = true;
            _context.Bestellingen.Update(bestelling);
            _context.SaveChanges();
            IndexAdminBestellingenVM vm = new IndexAdminBestellingenVM();
            vm.Bestellingen = await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= DateTime.Now).OrderBy(x => x.Leverdatum).ThenBy(x => x.Bestelbonnummer).ToListAsync();
            vm.ZoekenDatumVan = DateTime.Now;
            vm.ZoekenDatumTot = DateTime.Now.AddDays(15);
            vm.Bericht = $"Bestelling {bestelling.Bestelbonnummer} werd succesvol opgeslagen!";
            return View("Index", vm);
        }

        // bestelling voltooien met een nieuw leveradres
        public async Task<IActionResult> CreateAdres(LeverAdresAanpassenVM viewModel)
        {
            Bestelling bestelling = await _context.Bestellingen.Where(x => x.BestellingID == viewModel.Bestelling.BestellingID).FirstOrDefaultAsync();
            Klant klant = await _context.Klanten.Where(x => x.Id == bestelling.KlantFK).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                LeverAdres leverAdres = new LeverAdres { Omschrijving = viewModel.LeverAdres.Omschrijving, Straat = viewModel.LeverAdres.Straat, KlantFK = klant.Id, PostcodeID = viewModel.LeverAdres.PostcodeID, LandID = viewModel.LeverAdres.LandID };
                bestelling.LeverAdres = leverAdres;
                bestelling.IsVoltooid = true;
                _context.LeverAdressen.Add(leverAdres);
                _context.Bestellingen.Update(bestelling);
                _context.SaveChanges();
                IndexAdminBestellingenVM vm = new IndexAdminBestellingenVM();
                vm.Bestellingen = await _context.Bestellingen.Include(x => x.Klant).Where(x => x.Leverdatum >= DateTime.Now).OrderBy(x => x.Leverdatum).ThenBy(x => x.Bestelbonnummer).ToListAsync();
                vm.ZoekenDatumVan = DateTime.Now;
                vm.ZoekenDatumTot = DateTime.Now.AddDays(15);
                vm.Bericht = $"Bestelling {bestelling.Bestelbonnummer} werd succesvol opgeslagen!";
                return View("Index", vm);
            }
            else
            {
                LeverAdresAanpassenVM viewModelNew = new LeverAdresAanpassenVM();
                bestelling = await _context.Bestellingen.Where(x => x.BestellingID == viewModel.Bestelling.BestellingID).FirstOrDefaultAsync();
                klant = await _context.Klanten.Where(x => x.Id == bestelling.KlantFK).FirstOrDefaultAsync();
                viewModelNew.Leveradressen = new SelectList(await _context.LeverAdressen.Where(x => x.KlantFK == klant.Id).Include(x => x.Postcode).OrderBy(x => x.Omschrijving).ToListAsync(), "LeverAdresID", "Weergave");
                viewModelNew.Klant = klant;
                viewModelNew.Bestelling = bestelling;
                return View("LeverAdresAanpassen", viewModelNew);
            }

        }

        // inladen van btw gegevens
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

        #region Bestelling details
        public async Task<IActionResult> BestellingDetails(int? id)
        {
            BestellingDetailsVM viewModel = new BestellingDetailsVM();
            ModelOpbouw(viewModel, id);
            return View(viewModel);
        }

        public void ModelOpbouw(BestellingDetailsVM viewModel, int? id)
        {
            viewModel.Bestelling =  _context.Bestellingen.Where(x => x.BestellingID == id).Include(x => x.LeverAdres.Postcode).FirstOrDefault();
            viewModel.Klant =  _context.Klanten.Where(x => x.Id == viewModel.Bestelling.KlantFK).Include(x => x.Postcode).FirstOrDefault();
            viewModel.Bestellijnen = BestelijnenGenereren(id);
            viewModel.LeverAdres = viewModel.Bestelling.LeverAdres;
        }

        private List<string> BestelijnenGenereren(int? id)
        {
            List<Bestellijn> lijnen = _context.Bestellijnen.Where(x => x.BestellingID == id).Include(x => x.Product.Eenheid).Include(x => x.Product.ProductType).OrderBy(x => x.Product.ProductType.Volgnummer).ToList();
            List<string> Lijst = new List<string>();
            if (lijnen != null)
            {
                foreach (var item in lijnen)
                {
                    string lijn = "";
                    Lijst.Add(lijn = $"{item.Product.Naam}\t {item.Aantal}/{item.Product.Eenheid.Omschrijving} - € {item.Lijnprijs}");
                }
            }
            return Lijst;
        }


        #endregion
    }
}
