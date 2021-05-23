using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Data.Repository;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebshopContext _context;

        public UnitOfWork(WebshopContext context)
        {
            _context = context;
        }

        private IGenericRepository<Bestelling> bestellingRepository;
        private IGenericRepository<Bestellijn> bestellijnRepository;
        private IGenericRepository<Product> productRepository;

        public IGenericRepository<Bestelling> BestellingRepository
        {
            get
            {
                if (this.bestellingRepository == null)
                {
                    this.bestellingRepository = new GenericRepository<Bestelling>(_context);
                }
                return bestellingRepository;
            }
        }
        public IGenericRepository<Bestellijn> BestellijnRepository
        {
            get
            {
                if (this.bestellijnRepository == null)
                {
                    this.bestellijnRepository = new GenericRepository<Bestellijn>(_context);
                }
                return bestellijnRepository;
            }
        }

        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new GenericRepository<Product>(_context);
                }
                return productRepository;
            }
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
