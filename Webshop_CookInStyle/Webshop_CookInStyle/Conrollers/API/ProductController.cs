using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ProductController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducten()
        {
            return await _uow.ProductRepository.GetAll().ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _uow.ProductRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // GET api/Product/withDetails/#
        [HttpGet("withDetails")]
        public async Task<ActionResult<Product>> GetProductWithDetails(int id)
        {
            return await _uow.ProductRepository.GetAll()
                .Include(x => x.Allergenen).ThenInclude(y => y.Allergeen)
                .Include(x => x.ProductType)
                .Include(x => x.Eenheid)
                .Include(x => x.Btwtype)
                .Where(x => x.ProductID == id)
                .FirstOrDefaultAsync();
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _uow.ProductRepository.Update(product);

            try
            {
                await _uow.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Product
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _uow.ProductRepository.Create(product);
            await _uow.Save();

            return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        }

        // DELETE: api/Product/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _uow.ProductRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _uow.ProductRepository.Delete(product);
            await _uow.Save();

            return product;
        }

        private bool ProductExists(int id)
        {
            var check = _uow.ProductRepository.GetById(id);
            if (check == null)
            {
                return false;
            }
            return true;
        }
    }
}
