using Microsoft.EntityFrameworkCore;
using Webshop.Api.Entities;

namespace Webshop.Api.Contexts;

public class WebshopContext : DbContext
{
    public WebshopContext(DbContextOptions<WebshopContext> options) : base(options)
    {
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductOffer> ProductOffers { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasData(
                new Category() { Id = "Wheel Base" },
                new Category() { Id = "Pedals" },
                new Category() { Id = "Steering Wheels" }
            );
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Address);
            entity.HasOne(x => x.User);
            entity.HasMany(x => x.Products);

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("getutcdate()");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(x => new { x.OrderId, x.ProductVariantId });
            entity.HasOne(x => x.ProductVariant);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasMany(x => x.Categories);
            entity.HasMany(x => x.Variants);

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);
            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("getutcdate()");

            entity.HasData(
                new Product 
                {
				    Id = Guid.Parse("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
				    Name = "Simucube 2",
				    Description = "All the variants of Simucube 2 use industrial quality electronics, advanced software and firmware, and are built with high-quality materials for durability and long-lasting performance. They also have a wide range of compatibility with various sim racing software and games.",
				    IsActive = true
                },
			    new Product
                {
				    Id = Guid.Parse("297c7eda-33c4-47d3-ab1a-60f5d477dd13"),
				    Name = "ClubSport Pedals V3",
				    Description = "The Fanatec Clubsport v3 pedals are a high-end set of pedals designed for use with racing simulators. They feature a durable aluminum construction and are fully adjustable to fit the user's preferences. The pedals include a load cell brake, which provides precise braking force feedback, as well as an adjustable brake stiffness. The pedals also feature a realistic and adjustable clutch, allowing for smooth gear changes. Additionally, the pedals have a vibration motor built-in to provide even more immersive feedback. These pedals are compatible with most racing simulators and are a great choice for serious sim racers looking for the most realistic and immersive experience.",
				    ImageUrl = "https://fanatec.com/media/image/60/ef/a1/CSP_V3_prime2_1280x1280.jpg",
				    IsActive = true
			    },
                new Product
                {
                    Id = Guid.Parse("c0b5b2f5-0b1f-4b5e-8b1a-5b5b5b5b5b5b"),
                    Name = "Podium BMW M4 GT3",
                    ImageUrl = "https://fanatec.com/media/image/9c/70/21/Product_Page_top_banner_Podium_SW_BMW_M4_GT3_Front_1280x1280.jpg",
                    IsActive = true
                }
            );
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(x => new { x.ProductId, x.CategoryId });
            entity.HasOne(x => x.Product).WithMany(x => x.Categories);
            entity.HasOne(x => x.Category).WithMany(x => x.Products);

            entity.HasData(
                new ProductCategory
                {
                    ProductId = Guid.Parse("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
                    CategoryId = "Wheel Base"
                },
                new ProductCategory
                {
                    ProductId = Guid.Parse("297c7eda-33c4-47d3-ab1a-60f5d477dd13"),
                    CategoryId = "Pedals"
                },
                new ProductCategory
                {
                    ProductId = Guid.Parse("c0b5b2f5-0b1f-4b5e-8b1a-5b5b5b5b5b5b"),
                    CategoryId = "Steering Wheels"
                }
            );
        });

        modelBuilder.Entity<ProductOffer>(entity =>
        {
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasMany(x => x.Offers);

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            entity.HasData(
                new ProductVariant
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
                    Name = "Sport",
                    Description = "The SimuCube 2 Sport is the entry-level variant and is designed for entry-level and club-level sim racers. It features a compact design and a torque of up to 10Nm.",
                    Stock = 25,
                    PurchasePrice = 800,
                    SellingPrice = 1295,
                    IsActive = true
                }, 
                new ProductVariant
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
                    Name = "Pro",
                    Description = "The SimuCube 2 Pro is designed for professional and semi-professional sim racers. It features a torque of up to 20Nm and a higher level of customization options.",
                    Stock = 10,
                    PurchasePrice = 1000,
                    SellingPrice = 1495,
                    IsActive = true
                }, 
                new ProductVariant
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse("9c5f329d-1ed1-4d09-8a15-d056a5df6599"),
                    Name = "Ultimate",
                    Description = "The SimuCube 2 Ultimate is the top-of-the-line variant and is designed for elite sim racers. It features a torque of up to 32Nm and the most advanced customization options.",
                    Stock = 4,
                    PurchasePrice = 2000,
                    SellingPrice = 3295,
                    IsActive = true
                },
                new ProductVariant
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse("297c7eda-33c4-47d3-ab1a-60f5d477dd13"),
                    Name = "Default",
                    Stock = 25,
                    PurchasePrice = 300,
                    SellingPrice = 399,
                    IsActive = true
                },
                new ProductVariant
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse("297c7eda-33c4-47d3-ab1a-60f5d477dd13"),
                    Name = "Inverted",
                    Stock = 4,
                    PurchasePrice = 400,
                    SellingPrice = 599,
                    IsActive = true
                },
                new ProductVariant
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse("c0b5b2f5-0b1f-4b5e-8b1a-5b5b5b5b5b5b"),
                    Name = "Default",
                    Stock = 1,
                    PurchasePrice = 1100,
                    SellingPrice = 1499,
                    IsActive = true
                }
            );
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Address);
            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);
            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("getutcdate()");

            entity.HasData(User.CreateUser("admin@gunthers-sim-gear.com", "Passw0rd", UserRole.Admin, "Admin"));
        });            
    }
}
