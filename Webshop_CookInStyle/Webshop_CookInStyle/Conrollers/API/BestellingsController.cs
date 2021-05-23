using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop_CookInStyle.Data;
using Webshop_CookInStyle.Data.UnitOfWork;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.Conrollers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestellingsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public BestellingsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Bestellings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bestelling>>> GetBestellingen()
        {
            return await _uow.BestellingRepository.GetAll()
                .ToListAsync();
        }

        // GET: api/Bestellings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bestelling>> GetBestelling(int id)
        {
            var bestelling = await _uow.BestellingRepository.GetAll()
                .Include(x => x.Bestellijnen).ThenInclude(y => y.Product).ThenInclude(z => z.Allergenen)
                .Include(x => x.Klant)
                .Where(x => x.BestellingID == id)
                .FirstOrDefaultAsync();

            if (bestelling == null)
            {
                return NotFound();
            }

            return bestelling;
        }

        // GET api/Bestellings/lijst
        [HttpGet("lijstDetails")]
        public async Task<ActionResult<IEnumerable<Bestelling>>> GetBestellinglijstDetails()
        {
            return await _uow.BestellingRepository.GetAll()
                .Include(x => x.Bestellijnen)
                .Include(x => x.Klant)
                .Include(x => x.LeverAdres)
                .ToListAsync();
        }

        // GET api/Bestellings/lijst
        [HttpGet("ByKlantID")]
        public async Task<ActionResult<IEnumerable<Bestelling>>> GetBestellingByKlantID(string id)
        {
            return await _uow.BestellingRepository.GetAll()
                .Include(x => x.Bestellijnen).ThenInclude(y => y.Product).ThenInclude(z => z.Allergenen)
                .Include(x => x.Klant)
                .Where(x => x.KlantFK == id)
                .ToListAsync();
        }

        // PUT: api/Bestellings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBestelling(int id, Bestelling bestelling)
        {
            if (id != bestelling.BestellingID)
            {
                return BadRequest();
            }

            _uow.BestellingRepository.Update(bestelling);

            try
            {
                await _uow.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BestellingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bestellings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Bestelling>> PostBestelling(Bestelling bestelling)
        {
            _uow.BestellingRepository.Create(bestelling);
            await _uow.Save();

            return CreatedAtAction("GetBestelling", new { id = bestelling.BestellingID }, bestelling);
        }

        // DELETE: api/Bestellings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bestelling>> DeleteBestelling(int id)
        {
            var bestelling = await _uow.BestellingRepository.GetById(id);
            if (bestelling == null)
            {
                return NotFound();
            }

            _uow.BestellingRepository.Delete(bestelling);
            await _uow.Save();

            return bestelling;
        }

        private bool BestellingExists(int id)
        {
            var check = _uow.BestellingRepository.GetById(id);
            if (check == null)
            {
                return false;
            }
            return true;
        }
    }
}
