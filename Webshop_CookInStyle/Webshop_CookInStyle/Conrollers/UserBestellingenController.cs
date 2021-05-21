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
    public class UserBestellingenController : Controller
    {
        private readonly WebshopContext _context;

        public UserBestellingenController(WebshopContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            IndexUserBestellingenVM viewModel = new IndexUserBestellingenVM();
            Klant klant = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            LeverAdres tempLeverAdres = klant.LeverAdressen.Where(x => x.Omschrijving == "Standaard").FirstOrDefault();
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == klant && x.IsVoltooid == false);
            if (bestelling == null)
            {
                bestelling = new Bestelling { Bestelbonnummer = "In afwachting", BestelDatum = DateTime.Now, Klant = klant, Totaalprijs = 0, Opmerking = "", LeverAdres = tempLeverAdres, IsVoltooid = false };
                bestelling.Bestellijnen = new List<Bestellijn>();
            }
            viewModel.NieuweBestelling = bestelling;
            viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
            viewModel.BeschikbareProducten = await _context.Producten
                .Include(x => x.ProductType).Include(x => x.Allergenen).ThenInclude(x => x.Allergeen).Include(x => x.Eenheid)
                .Where(x => x.BeschikbaarInWebshop == true).ToListAsync();
            OpbouwProductLijst(viewModel);
            return View(viewModel);
        }

        public async Task<IActionResult> Verhoog(int? id, IndexUserBestellingenVM viewModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            #region inladen

            //AddBestellingVM viewModel = new AddBestellingVM();
            Klant klant = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            LeverAdres tempLeverAdres = klant.LeverAdressen.First();
            Product product = await _context.Producten.Where(x => x.ProductID == id).Include(x => x.Btwtype).FirstOrDefaultAsync();
            int aantal = CheckAantal(viewModel);
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == klant && x.IsVoltooid == false);

            #endregion inladen

            if (bestelling == null)
            {
                string nummer = BestelnummerOphalen();
                bestelling = new Bestelling { Bestelbonnummer = nummer, BestelDatum = DateTime.Now, Klant = klant, Totaalprijs = 0, Opmerking = "", LeverAdres = tempLeverAdres, IsVoltooid = false };
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

            viewModel.NieuweBestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == klant && x.IsVoltooid == false);
            viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
            viewModel.BeschikbareProducten = await _context.Producten
                    .Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid)
                    .Where(x => x.BeschikbaarInWebshop == true).ToListAsync();
            OpbouwProductLijst(viewModel);
            return View("Index", viewModel);
        }

        public async Task<IActionResult> Verminder(int? id, IndexUserBestellingenVM viewModel)
        {
            //AddBestellingVM viewModel = new AddBestellingVM();
            Klant klant = await _context.Klanten.Where(x => x.UserName == HttpContext.User.Identity.Name).Include(x => x.LeverAdressen).FirstOrDefaultAsync();
            Product product = await _context.Producten.Where(x => x.ProductID == id).Include(x => x.Btwtype).FirstOrDefaultAsync();
            Bestelling bestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == klant && x.IsVoltooid == false);
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
                viewModel.NieuweBestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == klant && x.IsVoltooid == false);
                viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
                viewModel.BeschikbareProducten = await _context.Producten
                    .Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid)
                    .Where(x => x.BeschikbaarInWebshop == true).ToListAsync();
                OpbouwProductLijst(viewModel);
                return View("Index", viewModel);
            }
            TotalenBerkenen(bestelling);
            _context.SaveChanges();
            viewModel.NieuweBestelling = await _context.Bestellingen.Include(x => x.Bestellijnen).FirstOrDefaultAsync(x => x.Klant == klant && x.IsVoltooid == false);
            viewModel.Orderlijnen = bestelling.Bestellijnen.ToList();
            viewModel.BeschikbareProducten = await _context.Producten
                .Include(x => x.ProductType).Include(x => x.Allergenen).Include(x => x.Eenheid)
                .Where(x => x.BeschikbaarInWebshop == true).ToListAsync();
            OpbouwProductLijst(viewModel);
            return View("Index", viewModel);
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

        // BTW tarieven op lijnniveau berekenen
        private void LijnPrijsBerekenen(Bestellijn lijn, Product product)
        {
            decimal btw = Convert.ToDecimal(product.Btwtype.Percentage);
            decimal aantal = Convert.ToDecimal(lijn.Aantal);
            lijn.BtwBedrag = (product.Eenheidsprijs / (100 + btw) * btw) * aantal;
            lijn.Lijnprijs = product.Eenheidsprijs * aantal;
        }

        // Aantallen voor bestelling
        private int CheckAantal(IndexUserBestellingenVM viewModel)
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
    }
}
