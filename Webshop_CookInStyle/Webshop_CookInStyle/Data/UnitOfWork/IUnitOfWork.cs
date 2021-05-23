using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Data.Repository;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Bestelling> BestellingRepository { get; }
        IGenericRepository<Bestellijn> BestellijnRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        Task Save();
    }
}
