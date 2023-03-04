using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class ProductVariantResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public double? PurchasePrice { get; set; }
    public double OriginalPrice { get; set; }
    public double SellingPrice { get; set; }
    public bool IsActive { get; set; }

    public static ProductVariantResponse? FromEntity(ProductVariant? entity)
    {
        if (entity == null)
            return null;

        var offers = entity.Offers.Where(x => x.StartAt <= DateTime.UtcNow && x.EndAt >= DateTime.UtcNow).ToList();

        var offer = offers.OrderByDescending(x => x.OffPercentage).FirstOrDefault();

        var sellingPrice = (1 - offer?.OffPercentage) * entity.SellingPrice ?? entity.SellingPrice;

        return new ProductVariantResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Stock = entity.Stock,
            PurchasePrice = entity.PurchasePrice,
            OriginalPrice = entity.SellingPrice,
            SellingPrice = sellingPrice,
            IsActive = entity.IsActive ?? true
        };
    }
}
