﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Webshop.Api.Contexts;

#nullable disable

namespace Webshop.Api.Migrations
{
    [DbContext(typeof(WebshopContext))]
    [Migration("20230118141705_DefaultDataAndAddedDescriptionToProductVariant")]
    partial class DefaultDataAndAddedDescriptionToProductVariant
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Webshop.Api.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Webshop.Api.Entities.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = "Wheel Base"
                        });
                });

            modelBuilder.Entity("Webshop.Api.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Webshop.Api.Entities.OrderProduct", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductVariantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductVariantId");

                    b.HasIndex("ProductVariantId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("Webshop.Api.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "All the variants of Simucube 2 use industrial quality electronics, advanced software and firmware, and are built with high-quality materials for durability and long-lasting performance. They also have a wide range of compatibility with various sim racing software and games.",
                            IsActive = true,
                            Name = "Simucube 2"
                        });
                });

            modelBuilder.Entity("Webshop.Api.Entities.ProductCategory", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProductId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ProductCategories");

                    b.HasData(
                        new
                        {
                            ProductId = new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
                            CategoryId = "Wheel Base"
                        });
                });

            modelBuilder.Entity("Webshop.Api.Entities.ProductOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("OffPercentage")
                        .HasColumnType("float");

                    b.Property<Guid>("ProductVariantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProductVariantId");

                    b.ToTable("ProductOffers");
                });

            modelBuilder.Entity("Webshop.Api.Entities.ProductVariant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("PurchasePrice")
                        .HasColumnType("float");

                    b.Property<double>("SellingPrice")
                        .HasColumnType("float");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductVariants");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9b3a5373-35ec-4283-aec3-b53a708dc706"),
                            Description = "The SimuCube 2 Sport is the entry-level variant and is designed for entry-level and club-level sim racers. It features a compact design and a torque of up to 10Nm.",
                            IsActive = true,
                            Name = "Sport",
                            ProductId = new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
                            PurchasePrice = 800.0,
                            SellingPrice = 1295.0,
                            Stock = 25
                        },
                        new
                        {
                            Id = new Guid("24e6a157-10ae-4188-ac26-cdf2a88903b7"),
                            Description = "The SimuCube 2 Pro is designed for professional and semi-professional sim racers. It features a torque of up to 20Nm and a higher level of customization options.",
                            IsActive = true,
                            Name = "Pro",
                            ProductId = new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
                            PurchasePrice = 1000.0,
                            SellingPrice = 1495.0,
                            Stock = 10
                        },
                        new
                        {
                            Id = new Guid("f820dd70-109e-49b5-b06b-11043779d073"),
                            Description = "The SimuCube 2 Ultimate is the top-of-the-line variant and is designed for elite sim racers. It features a torque of up to 32Nm and the most advanced customization options.",
                            IsActive = true,
                            Name = "Ultimate",
                            ProductId = new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
                            PurchasePrice = 2000.0,
                            SellingPrice = 3295.0,
                            Stock = 4
                        });
                });

            modelBuilder.Entity("Webshop.Api.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("401fa43f-1cde-4554-90b5-3ac178128e60"),
                            CreatedAt = new DateTime(2023, 1, 18, 14, 17, 5, 646, DateTimeKind.Utc).AddTicks(8400),
                            Email = "admin@gunthers-sim-gear.com",
                            FullName = "Admin",
                            IsActive = true,
                            PasswordHash = "5DFB28JuB/syGSoMjtb4uck33gOopQYNOEJ6hnK2ikG1d11qv8eeFgLk9f6DjJ/1fQGxhSq4dLb0p0mTPtVdug==",
                            PasswordSalt = "988be5f2ae2d46df83a71cd0b7799557",
                            Role = 2
                        });
                });

            modelBuilder.Entity("Webshop.Api.Entities.Order", b =>
                {
                    b.HasOne("Webshop.Api.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop.Api.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Webshop.Api.Entities.OrderProduct", b =>
                {
                    b.HasOne("Webshop.Api.Entities.Order", null)
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop.Api.Entities.ProductVariant", "ProductVariant")
                        .WithMany()
                        .HasForeignKey("ProductVariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductVariant");
                });

            modelBuilder.Entity("Webshop.Api.Entities.ProductCategory", b =>
                {
                    b.HasOne("Webshop.Api.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webshop.Api.Entities.Product", "Product")
                        .WithMany("Categories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Webshop.Api.Entities.ProductOffer", b =>
                {
                    b.HasOne("Webshop.Api.Entities.ProductVariant", null)
                        .WithMany("Offers")
                        .HasForeignKey("ProductVariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Webshop.Api.Entities.ProductVariant", b =>
                {
                    b.HasOne("Webshop.Api.Entities.Product", null)
                        .WithMany("Variants")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Webshop.Api.Entities.User", b =>
                {
                    b.HasOne("Webshop.Api.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Webshop.Api.Entities.Order", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Webshop.Api.Entities.Product", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Variants");
                });

            modelBuilder.Entity("Webshop.Api.Entities.ProductVariant", b =>
                {
                    b.Navigation("Offers");
                });
#pragma warning restore 612, 618
        }
    }
}