using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class ProductVariantRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public double? PurchasePrice { get; set; }
    public double SellingPrice { get; set; }
    public bool IsActive { get; set; }

    public ProductVariant ToEntity()
    {
        return new ProductVariant
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Stock = Stock,
            PurchasePrice = PurchasePrice,
            SellingPrice = SellingPrice,
            IsActive = IsActive
        };
    }
}
