using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.Data
{
    public class WebshopContext : IdentityDbContext<Klant>
    {
        public WebshopContext (DbContextOptions<WebshopContext> options)
            :base(options)
        {
        }

        public DbSet<Allergeen> Allergenen { get; set; }
        public DbSet<AllergeenProduct> AllergeenProducten { get; set; }
        public DbSet<Bestellijn> Bestellijnen { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<Btwtype> BtwTypes { get; set; }
        public DbSet<Eenheid> Eenheden { get; set; }
        public DbSet<Factuur> Facturen { get; set; }
        public DbSet<Factuurfirma> FactuurFirmas { get; set; }
        public DbSet<Factuurlijn> Factuurlijnen { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Land> Landen { get; set; }
        public DbSet<LeverAdres> LeverAdressen { get; set; }
        public DbSet<Postcode> Postcodes { get; set; }
        public DbSet<Product> Producten { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<Klant>().ToTable("Klanten");

            modelBuilder.Entity<LeverAdres>()
                .HasOne(x => x.Klant)
                .WithMany(x => x.LeverAdressen).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeverAdres>()
                .HasOne(x => x.Land)
                .WithMany(x => x.LeverAdressen).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeverAdres>()
                .HasOne(x => x.Postcode)
                .WithMany(x => x.LeverAdressen).OnDelete(DeleteBehavior.Restrict);

            // Bestelling
            modelBuilder.Entity<Bestelling>()
                .HasOne(x => x.Klant)
                .WithMany(x => x.Bestellingen).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bestelling>()
                .HasOne(x => x.LeverAdres)
                .WithMany(x => x.Bestellingen).OnDelete(DeleteBehavior.Restrict);

            // Factuur
            modelBuilder.Entity<Factuur>()
                .HasOne(x => x.Klant)
                .WithMany(x => x.Facturen).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Factuur>()
                .HasOne(x => x.Bestelling)
                .WithMany(x => x.Facturen).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Factuur>()
                .HasOne(x => x.Factuurfirma)
                .WithMany(x => x.Facturen).OnDelete(DeleteBehavior.Restrict);

            // Many to Many
            modelBuilder.Entity<AllergeenProduct>()
                .HasKey(ap => new { ap.AllergeenID, ap.ProductID });
            modelBuilder.Entity<AllergeenProduct>()
                .HasOne(ap => ap.Allergeen)
                .WithMany(a => a.Producten)
                .HasForeignKey(ap => ap.AllergeenID);
            modelBuilder.Entity<AllergeenProduct>()
                .HasOne(ap => ap.Product)
                .WithMany(p => p.Allergenen)
                .HasForeignKey(ap => ap.ProductID);

            // LeverAdres
            /*
            modelBuilder.Entity<LeverAdres>()
                .HasOne(x => x.Klant)
                .WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeverAdres>()
                .HasOne(x => x.Land)
                .WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeverAdres>()
                .HasOne(x => x.Postcode)
                .WithOne().OnDelete(DeleteBehavior.Restrict);

            // Bestelling
            modelBuilder.Entity<Bestelling>()
                .HasOne(x => x.Klant)
                .WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bestelling>()
                .HasOne(x => x.LeverAdres)
                .WithOne().OnDelete(DeleteBehavior.Restrict);

            // Factuur
            modelBuilder.Entity<Factuur>()
                .HasOne(x => x.Klant)
                .WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Factuur>()
                .HasOne(x => x.Bestelling)
                .WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Factuur>()
                .HasOne(x => x.Factuurfirma)
                .WithOne().OnDelete(DeleteBehavior.Restrict);

            // Many to Many
            modelBuilder.Entity<AllergeenProduct>()
                .HasKey(ap => new { ap.AllergeenID, ap.ProductID });
            modelBuilder.Entity<AllergeenProduct>()
                .HasOne(ap => ap.Allergeen)
                .WithMany(a => a.Producten)
                .HasForeignKey(ap => ap.AllergeenID);
            modelBuilder.Entity<AllergeenProduct>()
                .HasOne(ap => ap.Product)
                .WithMany(p => p.Allergenen)
                .HasForeignKey(ap => ap.ProductID);*/
        }


    }
}
