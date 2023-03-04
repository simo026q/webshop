using Webshop.Api.Entities;
using Webshop.Api.Extensions;

namespace Webshop.Api.Dtos;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public double FromPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<CategoryResponse> Categories { get; set; } = new();
    public List<ProductVariantResponse> Variants { get; set; } = new();

    public static ProductResponse FromEntity(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive ?? true,
            FromPrice = product.Variants.MinOrDefault(v => v.SellingPrice) ?? 0,
            CreatedAt = product.CreatedAt,
            Categories = product.Categories.Select(c => CategoryResponse.FromEntity(c.Category)).ToList(),
            Variants = product.Variants.Select(ProductVariantResponse.FromEntity).ToList()
        };
    }
}
