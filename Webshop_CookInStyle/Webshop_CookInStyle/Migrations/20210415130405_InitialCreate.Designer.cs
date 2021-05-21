﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Webshop_CookInStyle.Data;

namespace Webshop_CookInStyle.Migrations
{
    [DbContext(typeof(WebshopContext))]
    [Migration("20210415130405_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Allergeen", b =>
                {
                    b.Property<int>("AllergeenID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Omschrijving")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AllergeenID");

                    b.ToTable("Allergenen");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.AllergeenProduct", b =>
                {
                    b.Property<int>("AllergeenID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("AllergeenProductID")
                        .HasColumnType("int");

                    b.HasKey("AllergeenID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("AllergeenProducten");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Bestellijn", b =>
                {
                    b.Property<int>("BestellijnID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Aantal")
                        .HasColumnType("int");

                    b.Property<int>("BestellingID")
                        .HasColumnType("int");

                    b.Property<decimal>("BtwBedrag")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Eenheidsprijs")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Lijnprijs")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.HasKey("BestellijnID");

                    b.HasIndex("BestellingID");

                    b.HasIndex("ProductID");

                    b.ToTable("Bestellijnen");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Bestelling", b =>
                {
                    b.Property<int>("BestellingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BestelDatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("KlantFK")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("KlantId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LeverAdresID")
                        .HasColumnType("int");

                    b.Property<int?>("LeverAdresID1")
                        .HasColumnType("int");

                    b.Property<DateTime>("Leverdatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Opmerking")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Totaalprijs")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("BestellingID");

                    b.HasIndex("KlantFK")
                        .IsUnique()
                        .HasFilter("[KlantFK] IS NOT NULL");

                    b.HasIndex("KlantId");

                    b.HasIndex("LeverAdresID")
                        .IsUnique();

                    b.HasIndex("LeverAdresID1");

                    b.ToTable("Bestellingen");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Btwtype", b =>
                {
                    b.Property<int>("BtwID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Omschrijving")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("Percentage")
                        .HasColumnType("int");

                    b.HasKey("BtwID");

                    b.ToTable("BtwTypes");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Eenheid", b =>
                {
                    b.Property<int>("EenheidID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Afkorting")
                        .HasColumnType("nvarchar(5)")
                        .HasMaxLength(5);

                    b.Property<string>("Omschrijving")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EenheidID");

                    b.ToTable("Eenheden");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Factuur", b =>
                {
                    b.Property<int>("FactuurID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BestellingID")
                        .HasColumnType("int");

                    b.Property<int?>("BestellingID1")
                        .HasColumnType("int");

                    b.Property<string>("BtwnummerFacFirma")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BtwnummerKlant")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FactuurDatum")
                        .HasColumnType("datetime2");

                    b.Property<int>("FactuurfirmaID")
                        .HasColumnType("int");

                    b.Property<int?>("FactuurfirmaID1")
                        .HasColumnType("int");

                    b.Property<string>("Factuurnummer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBetaald")
                        .HasColumnType("bit");

                    b.Property<string>("KlantFK")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("KlantId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NaamKlant")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotaalPrijsEx")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("TotaalPrijsIncl")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("FactuurID");

                    b.HasIndex("BestellingID")
                        .IsUnique();

                    b.HasIndex("BestellingID1");

                    b.HasIndex("FactuurfirmaID")
                        .IsUnique();

                    b.HasIndex("FactuurfirmaID1");

                    b.HasIndex("KlantFK")
                        .IsUnique()
                        .HasFilter("[KlantFK] IS NOT NULL");

                    b.HasIndex("KlantId");

                    b.ToTable("Facturen");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Factuurfirma", b =>
                {
                    b.Property<int>("FactuurfirmaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Banknaam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BtwNummer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Iban")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LandID")
                        .HasColumnType("int");

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostcodeID")
                        .HasColumnType("int");

                    b.Property<string>("Straat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefoon")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FactuurfirmaID");

                    b.HasIndex("LandID");

                    b.HasIndex("PostcodeID");

                    b.ToTable("Facturatiefirmas");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Factuurlijn", b =>
                {
                    b.Property<int>("FactuurlijnID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Aantal")
                        .HasColumnType("int");

                    b.Property<decimal>("BtwBedrag")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Eenheidsprijs")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("FactuurID")
                        .HasColumnType("int");

                    b.Property<decimal>("Lijnprijs")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.HasKey("FactuurlijnID");

                    b.HasIndex("FactuurID");

                    b.HasIndex("ProductID");

                    b.ToTable("Factuurlijnen");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Klant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Achternaam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BtwNummer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBedrijf")
                        .HasColumnType("bit");

                    b.Property<int>("LandID")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Mobiel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("PostcodeID")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StraatEnNummer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefoon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Voornaam")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LandID");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PostcodeID");

                    b.ToTable("Klanten");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Land", b =>
                {
                    b.Property<int>("LandID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Afkorting")
                        .HasColumnType("nvarchar(5)")
                        .HasMaxLength(5);

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LandID");

                    b.ToTable("Landen");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.LeverAdres", b =>
                {
                    b.Property<int>("LeverAdresID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("KlantFK")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("KlantId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LandID")
                        .HasColumnType("int");

                    b.Property<int?>("LandID1")
                        .HasColumnType("int");

                    b.Property<string>("Omschrijving")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostcodeID")
                        .HasColumnType("int");

                    b.Property<int?>("PostcodeID1")
                        .HasColumnType("int");

                    b.Property<string>("Straat")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LeverAdresID");

                    b.HasIndex("KlantFK")
                        .IsUnique()
                        .HasFilter("[KlantFK] IS NOT NULL");

                    b.HasIndex("KlantId");

                    b.HasIndex("LandID")
                        .IsUnique();

                    b.HasIndex("LandID1");

                    b.HasIndex("PostcodeID")
                        .IsUnique();

                    b.HasIndex("PostcodeID1");

                    b.ToTable("LeverAdressen");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Postcode", b =>
                {
                    b.Property<int>("PostcodeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Gemeente")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nummer")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PostcodeID");

                    b.ToTable("Postcodes");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("BeschikbaarInWebshop")
                        .HasColumnType("bit");

                    b.Property<int>("BtwID")
                        .HasColumnType("int");

                    b.Property<int>("EenheidID")
                        .HasColumnType("int");

                    b.Property<decimal>("Eenheidsprijs")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Omschrijving")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductTypeID")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.HasIndex("BtwID");

                    b.HasIndex("EenheidID");

                    b.HasIndex("ProductTypeID");

                    b.ToTable("Producten");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.ProductType", b =>
                {
                    b.Property<int>("ProductTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Omschrijving")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductTypeID");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Klant", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Klant", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Klant", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Klant", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.AllergeenProduct", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Allergeen", "Allergeen")
                        .WithMany("Producten")
                        .HasForeignKey("AllergeenID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Product", "Product")
                        .WithMany("Allergenen")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Bestellijn", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Bestelling", "Bestelling")
                        .WithMany("Bestellijnen")
                        .HasForeignKey("BestellingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Product", "Product")
                        .WithMany("Bestellijnen")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Bestelling", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Klant", "Klant")
                        .WithOne()
                        .HasForeignKey("Webshop_CookInStyle.Models.Bestelling", "KlantFK")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Webshop_CookInStyle.Models.Klant", null)
                        .WithMany("Bestellingen")
                        .HasForeignKey("KlantId");

                    b.HasOne("Webshop_CookInStyle.Models.LeverAdres", "LeverAdres")
                        .WithOne()
                        .HasForeignKey("Webshop_CookInStyle.Models.Bestelling", "LeverAdresID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.LeverAdres", null)
                        .WithMany("Bestellingen")
                        .HasForeignKey("LeverAdresID1");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Factuur", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Bestelling", "Bestelling")
                        .WithOne()
                        .HasForeignKey("Webshop_CookInStyle.Models.Factuur", "BestellingID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Bestelling", null)
                        .WithMany("Facturen")
                        .HasForeignKey("BestellingID1");

                    b.HasOne("Webshop_CookInStyle.Models.Factuurfirma", "Factuurfirma")
                        .WithOne()
                        .HasForeignKey("Webshop_CookInStyle.Models.Factuur", "FactuurfirmaID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Factuurfirma", null)
                        .WithMany("Facturen")
                        .HasForeignKey("FactuurfirmaID1");

                    b.HasOne("Webshop_CookInStyle.Models.Klant", "Klant")
                        .WithOne()
                        .HasForeignKey("Webshop_CookInStyle.Models.Factuur", "KlantFK")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Webshop_CookInStyle.Models.Klant", null)
                        .WithMany("Facturen")
                        .HasForeignKey("KlantId");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Factuurfirma", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Land", "Land")
                        .WithMany("Factuurfirmas")
                        .HasForeignKey("LandID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Postcode", "Postcode")
                        .WithMany("Factuurfirmas")
                        .HasForeignKey("PostcodeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Factuurlijn", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Factuur", "Factuur")
                        .WithMany("Factuurlijnen")
                        .HasForeignKey("FactuurID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Product", "Product")
                        .WithMany("Factuurlijnen")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Klant", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Land", "Land")
                        .WithMany("Klanten")
                        .HasForeignKey("LandID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Postcode", "Postcode")
                        .WithMany("Klanten")
                        .HasForeignKey("PostcodeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.LeverAdres", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Klant", "Klant")
                        .WithOne()
                        .HasForeignKey("Webshop_CookInStyle.Models.LeverAdres", "KlantFK")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Webshop_CookInStyle.Models.Klant", null)
                        .WithMany("LeverAdressen")
                        .HasForeignKey("KlantId");

                    b.HasOne("Webshop_CookInStyle.Models.Land", "Land")
                        .WithOne()
                        .HasForeignKey("Webshop_CookInStyle.Models.LeverAdres", "LandID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Land", null)
                        .WithMany("LeverAdressen")
                        .HasForeignKey("LandID1");

                    b.HasOne("Webshop_CookInStyle.Models.Postcode", "Postcode")
                        .WithOne()
                        .HasForeignKey("Webshop_CookInStyle.Models.LeverAdres", "PostcodeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Postcode", null)
                        .WithMany("LeverAdressen")
                        .HasForeignKey("PostcodeID1");
                });

            modelBuilder.Entity("Webshop_CookInStyle.Models.Product", b =>
                {
                    b.HasOne("Webshop_CookInStyle.Models.Btwtype", "Btwtype")
                        .WithMany("Producten")
                        .HasForeignKey("BtwID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.Eenheid", "Eenheid")
                        .WithMany("Producten")
                        .HasForeignKey("EenheidID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop_CookInStyle.Models.ProductType", "ProductType")
                        .WithMany("Producten")
                        .HasForeignKey("ProductTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
