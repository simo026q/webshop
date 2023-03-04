using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class ProductRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    
    public List<CategoryRequest> Categories { get; set; } = new();
    public List<ProductVariantRequest> Variants { get; set; } = new();

    public Product ToEntity()
    {
        var product = new Product
        {
            Id = Id,
            Name = Name,
            Description = Description,
            ImageUrl = ImageUrl,
            IsActive = IsActive,
            Variants = Variants.Select(v => v.ToEntity()).ToList(),
            Categories = Categories.Select(c => new ProductCategory { CategoryId = c.Id, Category = c.ToEntity() }).ToList()
        };

        //product.Categories = categories.Select(c => new ProductCategory { CategoryId = c.Id, Category = c, ProductId = Id }).ToList();

        return product;
    }
}
